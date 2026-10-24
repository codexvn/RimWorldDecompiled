using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Hashing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Microsoft.Win32.SafeHandles;
using Waylith.Assets;
using Waylith.Diagnostics;
using Waylith.Logging;
using Waylith.Profiling;
using Waylith.Utils;

namespace Waylith.Rendering;

internal static class TextureCacheStore
{
	private readonly record struct Meta(byte Flags, long SourceLength, long SourceTicks, int Width, int Height, int MipCount, int Format, int Swizzle)
	{
		public bool Premultiplied => (Flags & 1) != 0;

		public bool Opaque => (Flags & 2) != 0;

		public bool Matches(FileInfo source)
		{
			if (SourceLength == source.Length)
			{
				return SourceTicks == source.LastWriteTimeUtc.Ticks;
			}
			return false;
		}
	}

	private readonly record struct QueuedWrite(Pack Pack, string Name, Meta Meta, LoadedTextureData Texture, long Bytes);

	private readonly record struct Entry(long Offset, int HeaderSize, uint CompressedSize, uint UncompressedSize, uint Crc, uint DosTime, AssetPackCodec Codec, Meta Meta, bool WrittenThisRun = false)
	{
		public long Span => HeaderSize + CompressedSize;
	}

	private sealed class PackedTextureSource : ITextureCacheSource, IDisposable
	{
		private readonly byte[] _payload;

		private readonly int[] _offsets;

		private readonly int[] _lengths;

		private bool _disposed;

		public int Width { get; }

		public int Height { get; }

		public int MipCount => _lengths.Length;

		public LoadedTextureFormat Format { get; }

		public bool Opaque { get; }

		public TextureSwizzle Swizzle { get; }

		private PackedTextureSource(byte[] payload, in Meta meta, int[] offsets, int[] lengths)
		{
			_payload = payload;
			_offsets = offsets;
			_lengths = lengths;
			Width = meta.Width;
			Height = meta.Height;
			Format = (LoadedTextureFormat)meta.Format;
			Opaque = meta.Opaque;
			Swizzle = TextureSwizzle.Unpack(meta.Swizzle);
		}

		public int LevelLength(int level)
		{
			return _lengths[level];
		}

		public void ReadLevel(int level, Span<byte> destination)
		{
			_payload.AsSpan(_offsets[level], System.Math.Min(_lengths[level], destination.Length)).CopyTo(destination);
		}

		public bool TryReadAll(out LoadedTextureData loadedTexture)
		{
			byte[][] array = new byte[_lengths.Length][];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = _payload.AsSpan(_offsets[i], _lengths[i]).ToArray();
			}
			int width = Width;
			int height = Height;
			int mipCount = array.Length;
			LoadedTextureFormat format = Format;
			bool opaque = Opaque;
			TextureSwizzle swizzle = Swizzle;
			loadedTexture = new LoadedTextureData(width, height, mipCount, format, array, opaque, null, swizzle);
			return true;
		}

		public void Dispose()
		{
			if (!_disposed)
			{
				_disposed = true;
				_buffers.Return(_payload);
			}
		}

		public static bool TryOpen(Pack pack, string name, in Entry entry, [NotNullWhen(true)] out ITextureCacheSource? source)
		{
			source = null;
			ArrayPool<byte> buffers = _buffers;
			byte[] array = null;
			byte[] array2 = null;
			bool flag = false;
			try
			{
				array = buffers.Rent((int)entry.Span);
				array2 = buffers.Rent((int)entry.UncompressedSize);
				using (FrameProfiler.Scope("TextureCache/Read"))
				{
					if (!pack.TryRead(entry.Offset, array.AsSpan(0, (int)entry.Span)) || !IsExpectedLocalHeader(array, name, in entry))
					{
						return false;
					}
				}
				Span<byte> span = array2.AsSpan(0, (int)entry.UncompressedSize);
				using (FrameProfiler.Scope("TextureCache/Decompress"))
				{
					AssetPackCodecs.Decode(entry.Codec, array.AsSpan(entry.HeaderSize, (int)entry.CompressedSize), span);
				}
				if (Crc32.HashToUInt32(span) != entry.Crc || !TryReadLevelTable(span, entry.Meta, out int[] offsets, out int[] lengths))
				{
					return false;
				}
				source = new PackedTextureSource(array2, entry.Meta, offsets, lengths);
				flag = true;
				return true;
			}
			catch
			{
				return false;
			}
			finally
			{
				if (array != null)
				{
					buffers.Return(array);
				}
				if (!flag && array2 != null)
				{
					buffers.Return(array2);
				}
			}
		}

		private static bool TryReadLevelTable(ReadOnlySpan<byte> payload, in Meta meta, out int[] offsets, out int[] lengths)
		{
			offsets = (lengths = Array.Empty<int>());
			if (payload.Length < 42 || BinaryPrimitives.ReadUInt32LittleEndian(payload) != 1480741966 || BinaryPrimitives.ReadInt32LittleEndian(payload.Slice(4)) != 23 || meta.Width <= 0 || meta.Height <= 0 || meta.MipCount <= 0 || meta.MipCount > 32)
			{
				return false;
			}
			offsets = new int[meta.MipCount];
			lengths = new int[meta.MipCount];
			int num = 42;
			for (int i = 0; i < meta.MipCount; i++)
			{
				if (num + 4 > payload.Length)
				{
					return false;
				}
				int num2 = BinaryPrimitives.ReadInt32LittleEndian(payload.Slice(num));
				num += 4;
				if (num2 < 0 || num2 > payload.Length - num)
				{
					return false;
				}
				offsets[i] = num;
				lengths[i] = num2;
				num += num2;
			}
			return true;
		}

		private static bool IsExpectedLocalHeader(ReadOnlySpan<byte> record, string name, in Entry entry)
		{
			if (BinaryPrimitives.ReadUInt32LittleEndian(record) == 67324752 && BinaryPrimitives.ReadUInt32LittleEndian(record.Slice(18)) == entry.CompressedSize && 30 + BinaryPrimitives.ReadUInt16LittleEndian(record.Slice(26)) + BinaryPrimitives.ReadUInt16LittleEndian(record.Slice(28)) == entry.HeaderSize)
			{
				return Encoding.UTF8.GetByteCount(ZipFormat.StoredName(name, entry.Codec)) == BinaryPrimitives.ReadUInt16LittleEndian(record.Slice(26));
			}
			return false;
		}
	}

	private sealed class Pack : IDisposable
	{
		private readonly Lock _lock = new Lock();

		private Dictionary<string, Entry> _entries = new Dictionary<string, Entry>(StringComparer.Ordinal);

		private SafeFileHandle? _handle;

		private SafeFileHandle? _writerLock;

		private readonly bool _readOnly;

		private long _dataEnd;

		private long _lastAppend;

		private long _liveBytes;

		private int _appendedSinceCentral;

		private bool _centralDirty;

		public string Path { get; }

		public string Root { get; }

		private Pack(string path, string root, SafeFileHandle handle, SafeFileHandle? writerLock)
		{
			Path = path;
			Root = root;
			_handle = handle;
			_writerLock = writerLock;
			_readOnly = writerLock == null;
		}

		private static SafeFileHandle OpenHandle(string path, FileMode mode)
		{
			return File.OpenHandle(path, mode, FileAccess.ReadWrite, FileShare.Read, FileOptions.Asynchronous, 0L);
		}

		private static SafeFileHandle OpenReadOnlyHandle(string path)
		{
			return File.OpenHandle(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete, FileOptions.Asynchronous, 0L);
		}

		public static Pack? TryOpen(string path, string root)
		{
			SafeFileHandle safeFileHandle = null;
			SafeFileHandle handle;
			bool shouldAppend;
			try
			{
				Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
				safeFileHandle = TryAcquireWriterLock(path);
				handle = ((safeFileHandle != null) ? OpenHandle(path, FileMode.OpenOrCreate) : OpenReadOnlyHandle(path));
				if (safeFileHandle == null)
				{
					LogCategory log = RenderLog.Log;
					LogCategory logCategory = log;
					LogString<Information> message = new LogString<Information>(103, 1, log, out shouldAppend);
					if (shouldAppend)
					{
						message.AppendLiteral("Texture cache pack '");
						message.AppendFormatted(path);
						message.AppendLiteral("' is held by another instance and opened read only, textures it lacks load uncached");
					}
					logCategory.Info(message, "TryOpen", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\TextureCacheStore.cs", 821);
				}
			}
			catch (Exception ex)
			{
				safeFileHandle?.Dispose();
				LogCategory log = RenderLog.Log;
				LogCategory logCategory2 = log;
				LogString<Warning> message2 = new LogString<Warning>(63, 2, log, out shouldAppend);
				if (shouldAppend)
				{
					message2.AppendLiteral("Texture cache pack '");
					message2.AppendFormatted(path);
					message2.AppendLiteral("' unavailable, its textures load uncached: ");
					message2.AppendFormatted(ex.Message);
				}
				logCategory2.Warning(message2, "TryOpen", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\TextureCacheStore.cs", 826);
				return null;
			}
			Pack pack = new Pack(path, root, handle, safeFileHandle);
			try
			{
				using (pack._lock.EnterScope())
				{
					if (!pack.TryReadCentralDirectoryLocked())
					{
						pack.RecoverLocked();
					}
				}
				pack.CompactIfMostlyGarbage();
				return (pack._handle == null) ? null : pack;
			}
			catch (Exception ex2)
			{
				LogCategory log = RenderLog.Log;
				LogCategory logCategory3 = log;
				LogString<Warning> message2 = new LogString<Warning>(62, 2, log, out shouldAppend);
				if (shouldAppend)
				{
					message2.AppendLiteral("Texture cache pack '");
					message2.AppendFormatted(path);
					message2.AppendLiteral("' unreadable, its textures load uncached: ");
					message2.AppendFormatted(ex2.Message);
				}
				logCategory3.Warning(message2, "TryOpen", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\TextureCacheStore.cs", 843);
				pack.Dispose();
				return null;
			}
		}

		public KeyValuePair<string, Entry>[] Snapshot()
		{
			using (_lock.EnterScope())
			{
				return _entries.ToArray();
			}
		}

		public bool TryGet(string name, out Entry entry)
		{
			using (_lock.EnterScope())
			{
				return _entries.TryGetValue(name, out entry) && (entry.WrittenThisRun || !ColdBoot.Enabled);
			}
		}

		public bool TryRead(long offset, Span<byte> destination)
		{
			SafeFileHandle handle = _handle;
			if (handle != null)
			{
				return ZipFormat.TryReadExactly(handle, destination, offset);
			}
			return false;
		}

		public void Append(string name, in Meta meta, AssetPackCodec codec, uint crc, uint uncompressedSize, ReadOnlyMemory<byte> compressed)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(ZipFormat.StoredName(name, codec));
			int num = 30 + bytes.Length + 42;
			int num2 = num + compressed.Length;
			byte[] array = new byte[num];
			uint dosTime = DosTime(DateTime.Now);
			ZipFormat.WriteLocalHeader(array, 63, ZipFormat.MethodFor(codec), dosTime, crc, (uint)compressed.Length, uncompressedSize, bytes.Length, 42);
			bytes.CopyTo(array.AsSpan(30));
			WriteMetaField(array.AsSpan(30 + bytes.Length), in meta);
			using (_lock.EnterScope())
			{
				if (_handle != null && !_readOnly)
				{
					RandomAccess.Write(_handle, new _003C_003Ez__ReadOnlyArray<ReadOnlyMemory<byte>>(new ReadOnlyMemory<byte>[2] { array, compressed }), _dataEnd);
					Entry value = new Entry(_dataEnd, num, (uint)compressed.Length, uncompressedSize, crc, dosTime, codec, meta, WrittenThisRun: true);
					if (_entries.TryGetValue(name, out var value2))
					{
						_liveBytes -= value2.Span;
					}
					_entries[name] = value;
					_liveBytes += value.Span;
					_dataEnd += num2;
					_centralDirty = true;
					_lastAppend = Stopwatch.GetTimestamp();
					if (++_appendedSinceCentral >= 512)
					{
						WriteCentralDirectoryLocked();
					}
				}
			}
		}

		public int RemoveWhere(Func<string, bool> dead)
		{
			using (_lock.EnterScope())
			{
				int num = 0;
				KeyValuePair<string, Entry>[] array = _entries.ToArray();
				for (int i = 0; i < array.Length; i++)
				{
					KeyValuePair<string, Entry> keyValuePair = array[i];
					var (text2, entry2) = (KeyValuePair<string, Entry>)(ref keyValuePair);
					if (dead(text2))
					{
						_entries.Remove(text2);
						_liveBytes -= entry2.Span;
						num++;
					}
				}
				if (num > 0 && _handle != null && !_readOnly)
				{
					WriteCentralDirectoryLocked();
				}
				return num;
			}
		}

		private bool IsMostlyGarbageLocked()
		{
			return _dataEnd - _liveBytes > System.Math.Max(16777216L, _dataEnd / 2);
		}

		public void FlushCentralDirectoryIfInactive(long inactiveTicks)
		{
			using (_lock.EnterScope())
			{
				if (_centralDirty && _handle != null && Stopwatch.GetElapsedTime(_lastAppend).Ticks >= inactiveTicks)
				{
					WriteCentralDirectoryLocked();
				}
			}
		}

		public void FlushCentralDirectory()
		{
			using (_lock.EnterScope())
			{
				if (_centralDirty && _handle != null)
				{
					WriteCentralDirectoryLocked();
				}
			}
		}

		public void Dispose()
		{
			using (_lock.EnterScope())
			{
				_handle?.Dispose();
				_handle = null;
				_writerLock?.Dispose();
				_writerLock = null;
			}
		}

		private void WriteCentralDirectoryLocked()
		{
			WriteCentralDirectory(_handle, _entries, _dataEnd);
			_appendedSinceCentral = 0;
			_centralDirty = false;
		}

		private void WriteCentralDirectory(SafeFileHandle handle, Dictionary<string, Entry> entries, long dataEnd)
		{
			List<KeyValuePair<string, Entry>> list = new List<KeyValuePair<string, Entry>>(entries);
			list.Sort((KeyValuePair<string, Entry> a, KeyValuePair<string, Entry> b) => a.Value.Offset.CompareTo(b.Value.Offset));
			MemoryStream memoryStream = new MemoryStream();
			Span<byte> span = stackalloc byte[46];
			Span<byte> field = stackalloc byte[54];
			foreach (KeyValuePair<string, Entry> item in list)
			{
				item.Deconstruct(out var key, out var value);
				string name = key;
				Entry entry = value;
				byte[] bytes = Encoding.UTF8.GetBytes(ZipFormat.StoredName(name, entry.Codec));
				bool flag = ZipFormat.NeedsZip64(entry.Offset);
				int num = 42 + (flag ? 12 : 0);
				ZipFormat.WriteCentralHeader(span, 63, 63, ZipFormat.MethodFor(entry.Codec), entry.DosTime, entry.Crc, entry.CompressedSize, entry.UncompressedSize, bytes.Length, num, 0u, entry.Offset);
				memoryStream.Write(span);
				memoryStream.Write(bytes);
				WriteMetaField(field, entry.Meta);
				if (flag)
				{
					ZipFormat.WriteZip64OffsetField(field.Slice(42), entry.Offset);
				}
				memoryStream.Write(field.Slice(0, num));
			}
			long length = memoryStream.Length;
			ZipFormat.WriteEnd(memoryStream, list.Count, length, dataEnd, 63, Encoding.UTF8.GetBytes(Root));
			RandomAccess.Write(handle, memoryStream.GetBuffer().AsSpan(0, (int)memoryStream.Length), dataEnd);
			RandomAccess.SetLength(handle, dataEnd + memoryStream.Length);
		}

		private bool TryReadCentralDirectoryLocked()
		{
			if (RandomAccess.GetLength(_handle) == 0L)
			{
				return true;
			}
			if (!ZipFormat.TryReadCentralDirectory(_handle, out List<ZipFormat.Record> records, out long directoryOffset, out string _))
			{
				return false;
			}
			Dictionary<string, Entry> dictionary = new Dictionary<string, Entry>(records.Count, StringComparer.Ordinal);
			long num = 0L;
			foreach (ZipFormat.Record item in records)
			{
				if (!TryReadMeta(item.Extra, out var meta) || item.CompressedLength > uint.MaxValue || item.Length > int.MaxValue)
				{
					return false;
				}
				Entry value = new Entry(item.LocalHeaderOffset, LocalHeaderSize(item.Name, item.Codec), (uint)item.CompressedLength, (uint)item.Length, item.Crc, item.DosTime, item.Codec, meta);
				if (value.Offset + value.Span > directoryOffset)
				{
					return false;
				}
				dictionary[item.Name] = value;
				num += value.Span;
			}
			_entries = dictionary;
			_liveBytes = num;
			_dataEnd = directoryOffset;
			return true;
		}

		private void RecoverLocked()
		{
			long length = RandomAccess.GetLength(_handle);
			Dictionary<string, Entry> dictionary = new Dictionary<string, Entry>(StringComparer.Ordinal);
			long num = 0L;
			long num2 = 0L;
			Entry value;
			for (Span<byte> span = stackalloc byte[30]; num2 + 30 <= length && TryRead(num2, span); num2 += value.Span)
			{
				if (BinaryPrimitives.ReadUInt32LittleEndian(span) != 67324752)
				{
					break;
				}
				ushort method = BinaryPrimitives.ReadUInt16LittleEndian(span.Slice(8));
				ushort num3 = BinaryPrimitives.ReadUInt16LittleEndian(span.Slice(26));
				ushort num4 = BinaryPrimitives.ReadUInt16LittleEndian(span.Slice(28));
				uint num5 = BinaryPrimitives.ReadUInt32LittleEndian(span.Slice(18));
				int num6 = 30 + num3 + num4;
				if (num2 + num6 + num5 > length)
				{
					break;
				}
				byte[] array = new byte[num3 + num4];
				if (!TryRead(num2 + 30, array) || !TryReadMeta(array.AsSpan(num3), out var meta))
				{
					break;
				}
				string storedName = Encoding.UTF8.GetString(array, 0, num3);
				AssetPackCodec codec = ZipFormat.CodecOf(method, ref storedName);
				value = new Entry(num2, num6, num5, BinaryPrimitives.ReadUInt32LittleEndian(span.Slice(22)), BinaryPrimitives.ReadUInt32LittleEndian(span.Slice(14)), BinaryPrimitives.ReadUInt32LittleEndian(span.Slice(10)), codec, meta);
				if (dictionary.TryGetValue(storedName, out var value2))
				{
					num -= value2.Span;
				}
				dictionary[storedName] = value;
				num += value.Span;
			}
			_entries = dictionary;
			_liveBytes = num;
			_dataEnd = num2;
			if (_readOnly)
			{
				return;
			}
			if (length > 0)
			{
				LogCategory log = RenderLog.Log;
				bool shouldAppend;
				LogString<Warning> message = new LogString<Warning>(98, 2, log, out shouldAppend);
				if (shouldAppend)
				{
					message.AppendLiteral("Texture cache pack '");
					message.AppendFormatted(Path);
					message.AppendLiteral("' had no readable central directory, recovered ");
					message.AppendFormatted(dictionary.Count);
					message.AppendLiteral(" entries from its local headers");
				}
				log.Warning(message, "RecoverLocked", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\TextureCacheStore.cs", 1053);
			}
			RandomAccess.SetLength(_handle, num2);
			_centralDirty = dictionary.Count > 0;
		}

		public long CompactIfMostlyGarbage()
		{
			SafeFileHandle handle;
			KeyValuePair<string, Entry>[] array;
			long dataEnd;
			using (_lock.EnterScope())
			{
				if (_handle == null || _readOnly || !IsMostlyGarbageLocked())
				{
					return 0L;
				}
				handle = _handle;
				array = _entries.ToArray();
				dataEnd = _dataEnd;
			}
			string text = Path + ".tmp";
			try
			{
				Dictionary<string, Entry> dictionary = new Dictionary<string, Entry>(array.Length, StringComparer.Ordinal);
				long num = 0L;
				using (SafeFileHandle handle2 = File.OpenHandle(text, FileMode.Create, FileAccess.ReadWrite, FileShare.None, FileOptions.None, 0L))
				{
					Array.Sort(array, (KeyValuePair<string, Entry> a, KeyValuePair<string, Entry> b) => a.Value.Offset.CompareTo(b.Value.Offset));
					KeyValuePair<string, Entry>[] array2 = array;
					foreach (KeyValuePair<string, Entry> keyValuePair in array2)
					{
						keyValuePair.Deconstruct(out var key, out var value);
						string key2 = key;
						Entry entry = value;
						int num2 = (int)entry.Span;
						byte[] array3 = ArrayPool<byte>.Shared.Rent(num2);
						try
						{
							if (!ZipFormat.TryReadExactly(handle, array3.AsSpan(0, num2), entry.Offset))
							{
								return 0L;
							}
							RandomAccess.Write(handle2, array3.AsSpan(0, num2), num);
						}
						finally
						{
							ArrayPool<byte>.Shared.Return(array3);
						}
						value = (dictionary[key2] = entry with
						{
							Offset = num
						});
						num += num2;
					}
					WriteCentralDirectory(handle2, dictionary, num);
				}
				using (_lock.EnterScope())
				{
					if (_handle != handle || _dataEnd != dataEnd || _entries.Count != array.Length)
					{
						return 0L;
					}
					_handle.Dispose();
					_handle = null;
					try
					{
						File.Move(text, Path, overwrite: true);
					}
					catch
					{
						_handle = OpenHandle(Path, FileMode.Open);
						throw;
					}
					_handle = OpenHandle(Path, FileMode.Open);
					_entries = dictionary;
					_dataEnd = (_liveBytes = num);
					_appendedSinceCentral = 0;
					_centralDirty = false;
					return dataEnd - num;
				}
			}
			catch (Exception ex)
			{
				LogCategory log = RenderLog.Log;
				bool shouldAppend;
				LogString<Warning> message = new LogString<Warning>(41, 2, log, out shouldAppend);
				if (shouldAppend)
				{
					message.AppendLiteral("Texture cache pack '");
					message.AppendFormatted(Path);
					message.AppendLiteral("' compaction failed: ");
					message.AppendFormatted(ex.Message);
				}
				log.Warning(message, "CompactIfMostlyGarbage", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\TextureCacheStore.cs", 1131);
				return 0L;
			}
			finally
			{
				try
				{
					File.Delete(text);
				}
				catch
				{
				}
			}
		}
	}

	private const int CentralDirectoryEvery = 512;

	private const string WriterLockSuffix = ".lock";

	private const long IdleFlushTicks = 20000000L;

	private const long SealInactiveTicks = 5000000L;

	private const long CompactMinGarbageBytes = 16777216L;

	private const int PayloadHeaderSize = 42;

	private const ushort ZipVersion = 63;

	private const ushort MetaFieldId = 21591;

	private const int MetaFieldSize = 38;

	private const int MetaExtraSize = 42;

	private const long MaxQueuedWriteBytes = 268435456L;

	private const string KeyedPackPrefix = "pages.";

	private static readonly Lock PacksLock = new Lock();

	private static readonly object WriteGate = new object();

	private static readonly Queue<QueuedWrite> Writes = new Queue<QueuedWrite>();

	private static readonly int WriterCount = System.Math.Clamp(Environment.ProcessorCount / 4, 1, 4);

	private static long _queuedWriteBytes;

	private static int _activeWrites;

	private static int _writers;

	private static readonly Dictionary<string, Lazy<Pack?>> Packs = new Dictionary<string, Lazy<Pack>>(StringComparer.Ordinal);

	private static int _staleSweepStarted;

	private static long _lastWriteTicks;

	private static long _lastSealScan;

	private static ArrayPool<byte> _buffers = NewBufferPool();

	private static string KeyedPackRoot => CacheFileWriter.InstallRoot;

	internal static string KeyedPackFileName => KeyedPagesFileName(CacheFileWriter.InstallId);

	private static ArrayPool<byte> NewBufferPool()
	{
		return ArrayPool<byte>.Create(67108864, Environment.ProcessorCount);
	}

	public static void ReleaseBuffers()
	{
		_buffers = NewBufferPool();
	}

	public static bool TryOpen(string fullPath, string variant, bool premultiplied, [NotNullWhen(true)] out ITextureCacheSource? source)
	{
		source = null;
		FileInfo fileInfo = new FileInfo(fullPath);
		if (fileInfo.Exists && TryGetSourcePack(fullPath, variant, out Pack pack, out string name) && pack.TryGet(name, out var entry) && entry.Meta.Matches(fileInfo) && entry.Meta.Premultiplied == premultiplied)
		{
			return PackedTextureSource.TryOpen(pack, name, in entry, out source);
		}
		return false;
	}

	public static bool TryGetInfo(string fullPath, string variant, bool premultiplied, out AssetPackTextureInfo.Info info)
	{
		info = default(AssetPackTextureInfo.Info);
		FileInfo fileInfo = new FileInfo(fullPath);
		if (!fileInfo.Exists || !TryGetSourcePack(fullPath, variant, out Pack pack, out string name) || !pack.TryGet(name, out var entry) || !entry.Meta.Matches(fileInfo) || entry.Meta.Premultiplied != premultiplied)
		{
			return false;
		}
		Meta meta = entry.Meta;
		info = new AssetPackTextureInfo.Info(meta.Width, meta.Height, meta.MipCount, (LoadedTextureFormat)meta.Format, meta.Opaque, TextureSwizzle.Unpack(meta.Swizzle));
		return true;
	}

	public static bool TryOpenKeyed(string key, [NotNullWhen(true)] out ITextureCacheSource? source)
	{
		source = null;
		string name = KeyedName(key);
		if (TryGetKeyedPack(out Pack pack) && pack.TryGet(name, out var entry))
		{
			return PackedTextureSource.TryOpen(pack, name, in entry, out source);
		}
		return false;
	}

	public static bool HasValid(string fullPath, string variant, bool premultiplied, int width, int height)
	{
		FileInfo fileInfo = new FileInfo(fullPath);
		if (fileInfo.Exists && TryGetSourcePack(fullPath, variant, out Pack pack, out string name) && pack.TryGet(name, out var entry) && entry.Meta.Matches(fileInfo) && entry.Meta.Premultiplied == premultiplied && entry.Meta.Width == width)
		{
			return entry.Meta.Height == height;
		}
		return false;
	}

	public static bool StoreLater(string fullPath, string variant, bool premultiplied, in LoadedTextureData texture, (long Length, long WriteTicks) sourceStamp)
	{
		if (!TryGetSourcePack(fullPath, variant, out Pack pack, out string name))
		{
			return false;
		}
		Meta meta = new Meta((byte)((premultiplied ? 1u : 0u) | (uint)(texture.Opaque ? 2 : 0)), sourceStamp.Length, sourceStamp.WriteTicks, texture.Width, texture.Height, texture.MipCount, (int)texture.Format, texture.Swizzle.Pack());
		long num = 0L;
		byte[][] levels = texture.Levels;
		foreach (byte[] array in levels)
		{
			num += ((array != null) ? array.Length : 0);
		}
		lock (WriteGate)
		{
			while (_queuedWriteBytes > 0 && _queuedWriteBytes + num > 268435456)
			{
				Monitor.Wait(WriteGate);
			}
			Writes.Enqueue(new QueuedWrite(pack, name, meta, texture, num));
			_queuedWriteBytes += num;
			if (_writers < WriterCount)
			{
				_writers++;
				Thread thread = new Thread(WriterLoop);
				thread.IsBackground = true;
				thread.Name = "TextureCacheWriter";
				thread.Priority = ThreadPriority.BelowNormal;
				thread.Start();
			}
			Monitor.PulseAll(WriteGate);
		}
		return true;
	}

	public static Task StoreKeyed(string key, LoadedTextureData texture)
	{
		lock (WriteGate)
		{
			_activeWrites++;
		}
		return Task.Run(delegate
		{
			try
			{
				if (TryGetKeyedPack(out Pack pack))
				{
					Store(pack, KeyedName(key), new Meta(0, 0L, 0L, texture.Width, texture.Height, texture.MipCount, (int)texture.Format, texture.Swizzle.Pack()), texture.Levels, "keyed page");
				}
			}
			finally
			{
				lock (WriteGate)
				{
					_activeWrites--;
					Monitor.PulseAll(WriteGate);
				}
			}
		});
	}

	public static IReadOnlyList<(string Name, LoadedTextureData Texture)> ReadLargestKeyed(int count)
	{
		List<(string, LoadedTextureData)> list = new List<(string, LoadedTextureData)>();
		if (!TryGetKeyedPack(out Pack pack))
		{
			return list;
		}
		foreach (KeyValuePair<string, Entry> item in (from pair in pack.Snapshot()
			orderby pair.Value.UncompressedSize descending
			select pair).Take(count))
		{
			var (text2, entry2) = (KeyValuePair<string, Entry>)(ref item);
			if (!PackedTextureSource.TryOpen(pack, text2, in entry2, out ITextureCacheSource source))
			{
				continue;
			}
			using (source)
			{
				if (source.TryReadAll(out var loadedTexture))
				{
					list.Add((text2, loadedTexture));
				}
			}
		}
		return list;
	}

	private static void DrainWrites()
	{
		lock (WriteGate)
		{
			while (Writes.Count > 0 || _activeWrites > 0)
			{
				Monitor.Wait(WriteGate);
			}
		}
	}

	private static void WriterLoop()
	{
		while (true)
		{
			QueuedWrite queuedWrite;
			lock (WriteGate)
			{
				while (Writes.Count == 0)
				{
					Monitor.Wait(WriteGate);
				}
				queuedWrite = Writes.Dequeue();
				_activeWrites++;
			}
			try
			{
				Store(queuedWrite.Pack, queuedWrite.Name, queuedWrite.Meta, queuedWrite.Texture.Levels, queuedWrite.Name);
			}
			finally
			{
				lock (WriteGate)
				{
					_queuedWriteBytes -= queuedWrite.Bytes;
					_activeWrites--;
					Monitor.PulseAll(WriteGate);
				}
			}
		}
	}

	private static void Store(Pack pack, string name, in Meta meta, byte[][] levels, string what)
	{
		int num = 42;
		foreach (byte[] array in levels)
		{
			num += 4 + ((array != null) ? array.Length : 0);
		}
		(AssetPackCodec Codec, int Level) tuple = CodecFor(GpuTextureService.CacheCodec);
		AssetPackCodec item = tuple.Codec;
		int item2 = tuple.Level;
		ArrayPool<byte> buffers = _buffers;
		byte[] array2 = null;
		byte[] array3 = null;
		try
		{
			array2 = buffers.Rent(num);
			array3 = buffers.Rent(AssetPackCodecs.MaxCompressedLength(item, num));
			Span<byte> span = array2.AsSpan(0, num);
			WritePayload(span, meta, levels);
			int length = AssetPackCodecs.Compress(span, item, item2, array3);
			pack.Append(name, in meta, item, Crc32.HashToUInt32(span), (uint)num, array3.AsMemory(0, length));
			Volatile.Write(ref _lastWriteTicks, Stopwatch.GetTimestamp());
			SealInactivePacks();
		}
		catch (Exception ex)
		{
			LogCategory log = RenderLog.Log;
			bool shouldAppend;
			LogString<Warning> message = new LogString<Warning>(35, 2, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Texture cache write failed for '");
				message.AppendFormatted(what);
				message.AppendLiteral("': ");
				message.AppendFormatted(ex.Message);
			}
			log.Warning(message, "Store", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\TextureCacheStore.cs", 263);
		}
		finally
		{
			if (array2 != null)
			{
				buffers.Return(array2);
			}
			if (array3 != null)
			{
				buffers.Return(array3);
			}
		}
	}

	private static (AssetPackCodec Codec, int Level) CodecFor(TextureCacheCodec codec)
	{
		return codec switch
		{
			TextureCacheCodec.Zstd => (Codec: AssetPackCodec.Zstd, Level: 3), 
			TextureCacheCodec.Lz4 => (Codec: AssetPackCodec.Lz4, Level: 0), 
			TextureCacheCodec.Lz4Hc => (Codec: AssetPackCodec.Lz4, Level: 9), 
			_ => (Codec: AssetPackCodec.Store, Level: 0), 
		};
	}

	public static void FlushAll()
	{
		DrainWrites();
		foreach (Pack item in SnapshotPacks())
		{
			item.FlushCentralDirectory();
		}
	}

	private static void SealInactivePacks()
	{
		long num = Volatile.Read(in _lastSealScan);
		if (Stopwatch.GetElapsedTime(num).Ticks < 5000000 || Interlocked.CompareExchange(ref _lastSealScan, Stopwatch.GetTimestamp(), num) != num)
		{
			return;
		}
		foreach (Pack item in SnapshotPacks())
		{
			item.FlushCentralDirectoryIfInactive(5000000L);
		}
	}

	public static void FlushIfIdle()
	{
		long num = Volatile.Read(in _lastWriteTicks);
		if (num == 0L || Stopwatch.GetElapsedTime(num).Ticks < 20000000)
		{
			return;
		}
		Volatile.Write(ref _lastWriteTicks, 0L);
		foreach (Pack item in SnapshotPacks())
		{
			item.FlushCentralDirectory();
		}
	}

	public static void Shutdown()
	{
		FlushAll();
		foreach (Pack item in SnapshotPacks())
		{
			item.Dispose();
		}
		using (PacksLock.EnterScope())
		{
			Packs.Clear();
		}
	}

	private static List<Pack> SnapshotPacks()
	{
		List<Pack> list = new List<Pack>();
		using (PacksLock.EnterScope())
		{
			foreach (Lazy<Pack> value in Packs.Values)
			{
				if (value.IsValueCreated && value.Value != null)
				{
					list.Add(value.Value);
				}
			}
			return list;
		}
	}

	private static string KeyedName(string key)
	{
		return Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(key)));
	}

	private static bool TryGetSourcePack(string fullPath, string variant, [NotNullWhen(true)] out Pack? pack, out string name)
	{
		string text = PackRoot(fullPath);
		name = Path.GetRelativePath(text, fullPath).Replace('\\', '/') + variant;
		return TryGetPack(text, out pack);
	}

	private static bool TryGetPack(string root, [NotNullWhen(true)] out Pack? pack)
	{
		return TryGetPack(root, SanitizeFileName(Path.GetFileName(root)) + "-" + RootHash(root) + ".zip", out pack);
	}

	private static bool TryGetKeyedPack([NotNullWhen(true)] out Pack? pack)
	{
		return TryGetPack(KeyedPackRoot, KeyedPackFileName, out pack);
	}

	private static bool TryGetPack(string root, string fileName, [NotNullWhen(true)] out Pack? pack)
	{
		pack = null;
		string textureCacheRoot = GpuTextureService.TextureCacheRoot;
		if (string.IsNullOrWhiteSpace(textureCacheRoot))
		{
			return false;
		}
		string packPath = Path.Combine(textureCacheRoot, fileName);
		Lazy<Pack> value;
		using (PacksLock.EnterScope())
		{
			if (!Packs.TryGetValue(packPath, out value))
			{
				StartStaleVersionSweep(textureCacheRoot);
				value = new Lazy<Pack>(() => Pack.TryOpen(packPath, root));
				Packs[packPath] = value;
			}
		}
		pack = value.Value;
		return pack != null;
	}

	private static string RootHash(string root)
	{
		return Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(root)), 0, 8);
	}

	private static string KeyedPagesFileName(string installId)
	{
		return "pages." + installId + ".zip";
	}

	private static bool IsKeyedPackFile(string fileName)
	{
		if (fileName.Length == "pages.".Length + 16 + ".zip".Length && fileName.StartsWith("pages.", StringComparison.Ordinal))
		{
			return fileName.EndsWith(".zip", StringComparison.Ordinal);
		}
		return false;
	}

	private static string PackRoot(string fullPath)
	{
		string text = Path.GetDirectoryName(fullPath) ?? fullPath;
		string text2 = GpuTextureService.FindModRoot(text);
		if (text2 != null)
		{
			return text2;
		}
		int num = fullPath.Replace('\\', '/').LastIndexOf("/Resources/", StringComparison.OrdinalIgnoreCase);
		if (num < 0)
		{
			return text;
		}
		return fullPath.Substring(0, num + "/Resources".Length);
	}

	private static string SanitizeFileName(string name)
	{
		char[] array = name.ToCharArray();
		char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
		for (int i = 0; i < array.Length; i++)
		{
			if (Array.IndexOf(invalidFileNameChars, array[i]) >= 0 || array[i] == ' ')
			{
				array[i] = '_';
			}
		}
		if (array.Length != 0)
		{
			return new string(array);
		}
		return "root";
	}

	private static void StartStaleVersionSweep(string cacheRoot)
	{
		if (Interlocked.Exchange(ref _staleSweepStarted, 1) == 0)
		{
			string directoryName = Path.GetDirectoryName(cacheRoot);
			if (directoryName != null)
			{
				CacheFileWriter.SweepLater(directoryName);
			}
		}
	}

	public static void Sweep(IReadOnlyCollection<string> contentRoots, IReadOnlyCollection<string> usedKeys)
	{
		string textureCacheRoot = GpuTextureService.TextureCacheRoot;
		DirectoryInfo directoryInfo = (string.IsNullOrWhiteSpace(textureCacheRoot) ? null : Directory.GetParent(textureCacheRoot));
		if (directoryInfo == null || !directoryInfo.Exists)
		{
			return;
		}
		FlushAll();
		Dictionary<string, string> dictionary = SnapshotPacks().ToDictionary<Pack, string, string>((Pack pack) => pack.Path, (Pack pack) => pack.Root, StringComparer.Ordinal);
		string[] array = contentRoots.Where((string root) => !string.IsNullOrWhiteSpace(root)).Select(Path.GetFullPath).Append(Path.TrimEndingDirectorySeparator(AppContext.BaseDirectory))
			.Concat(dictionary.Values.Where((string root) => root.Length > 0))
			.ToArray();
		HashSet<string> usedNames = usedKeys.Select(KeyedName).ToHashSet<string>(StringComparer.Ordinal);
		string keyedPackFileName = KeyedPackFileName;
		string fullName = directoryInfo.Parent.FullName;
		CacheFileWriter.RecordInstall(fullName, directoryInfo.Name, (IReadOnlyCollection<string>?)(object)array);
		List<CacheFileWriter.InstallRecord> source = CacheFileWriter.OtherInstalls(fullName);
		HashSet<string> hashSet = source.Select((CacheFileWriter.InstallRecord install) => KeyedPagesFileName(install.Id)).ToHashSet<string>(StringComparer.Ordinal);
		string[] liveRoots = source.SelectMany((CacheFileWriter.InstallRecord install) => install.Roots).ToArray();
		int deleted = 0;
		int num = 0;
		long num2 = 0L;
		DirectoryInfo[] directories = directoryInfo.GetDirectories();
		for (int i = 0; i < directories.Length; i++)
		{
			foreach (string item in Directory.EnumerateFiles(directories[i].FullName))
			{
				try
				{
					if (item.EndsWith(".tmp", StringComparison.Ordinal))
					{
						if (File.GetLastWriteTimeUtc(item) < DateTime.UtcNow - TimeSpan.FromHours(1))
						{
							File.Delete(item);
						}
					}
					else
					{
						if (item.EndsWith(".lock", StringComparison.Ordinal))
						{
							continue;
						}
						bool flag = Path.GetDirectoryName(item) == Path.GetFullPath(textureCacheRoot);
						string fileName = Path.GetFileName(item);
						if (fileName == keyedPackFileName)
						{
							Pack pack2;
							if (!flag)
							{
								ClosePack(item);
								num2 += Delete(item, ref deleted);
							}
							else if (TryGetKeyedPack(out pack2))
							{
								num += pack2.RemoveWhere((string name) => !usedNames.Contains(name));
							}
							continue;
						}
						bool flag2;
						string root2;
						if (IsKeyedPackFile(fileName))
						{
							flag2 = hashSet.Contains(fileName);
						}
						else if ((dictionary.TryGetValue(item, out root2) || TryReadPackRoot(item, out root2)) && IsLive(root2, array))
						{
							if (flag && TryGetPack(root2, out Pack pack3))
							{
								num += pack3.RemoveWhere((string name) => !File.Exists(SourcePath(root2, name)));
							}
							flag2 = true;
						}
						else
						{
							flag2 = IsLive(root2, liveRoots);
						}
						if (flag2)
						{
							continue;
						}
						using (SafeFileHandle safeFileHandle = TryAcquireWriterLock(item))
						{
							if (safeFileHandle != null)
							{
								ClosePack(item);
								num2 += Delete(item, ref deleted);
							}
						}
						continue;
					}
				}
				catch (Exception ex) when (((ex is IOException || ex is UnauthorizedAccessException) ? 1 : 0) != 0)
				{
				}
			}
		}
		foreach (Pack item2 in SnapshotPacks())
		{
			num2 += item2.CompactIfMostlyGarbage();
		}
		VerboseLog.Log($"Texture cache cleanup: dropped {deleted} packs and {num} entries, {(double)num2 / 1048576.0:0.#} MB freed.");
	}

	private static bool IsLive(string root, string[] liveRoots)
	{
		StringComparison comparison = (OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
		if (root.Length > 0 && Directory.Exists(root))
		{
			return liveRoots.Any((string live) => root.Equals(live, comparison) || root.StartsWith(live.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar, comparison));
		}
		return false;
	}

	internal static string SourcePath(string root, string name)
	{
		int num = name.IndexOf('#', name.LastIndexOf('.') + 1);
		return Path.Combine(root, (num < 0) ? name : name.Substring(0, num));
	}

	private static SafeFileHandle? TryAcquireWriterLock(string packPath)
	{
		try
		{
			return File.OpenHandle(packPath + ".lock", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, FileOptions.None, 0L);
		}
		catch (IOException)
		{
			return null;
		}
	}

	private static long Delete(string path, ref int deleted)
	{
		long length = new FileInfo(path).Length;
		File.Delete(path);
		deleted++;
		return length;
	}

	private static void ClosePack(string packPath)
	{
		Lazy<Pack> value;
		using (PacksLock.EnterScope())
		{
			if (!Packs.Remove(packPath, out value))
			{
				return;
			}
		}
		if (value.IsValueCreated)
		{
			value.Value?.Dispose();
		}
	}

	private static bool TryReadPackRoot(string path, out string root)
	{
		root = "";
		using SafeFileHandle handle = File.OpenHandle(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, FileOptions.None, 0L);
		if (!ZipFormat.TryReadCentralDirectory(handle, out List<ZipFormat.Record> _, out long _, out string comment))
		{
			return false;
		}
		root = comment;
		return true;
	}

	private static void WritePayload(Span<byte> payload, Meta meta, byte[][] levels)
	{
		BinaryPrimitives.WriteUInt32LittleEndian(payload, 1480741966u);
		BinaryPrimitives.WriteInt32LittleEndian(payload.Slice(4), 23);
		BinaryPrimitives.WriteInt64LittleEndian(payload.Slice(8), meta.SourceLength);
		BinaryPrimitives.WriteInt64LittleEndian(payload.Slice(16), meta.SourceTicks);
		BinaryPrimitives.WriteInt32LittleEndian(payload.Slice(24), meta.Width);
		BinaryPrimitives.WriteInt32LittleEndian(payload.Slice(28), meta.Height);
		BinaryPrimitives.WriteInt32LittleEndian(payload.Slice(32), meta.MipCount);
		BinaryPrimitives.WriteInt32LittleEndian(payload.Slice(36), meta.Format);
		payload[40] = (byte)(meta.Flags & 1);
		payload[41] = (byte)((meta.Flags >> 1) & 1);
		int num = 42;
		for (int i = 0; i < levels.Length; i++)
		{
			byte[] array = levels[i] ?? Array.Empty<byte>();
			BinaryPrimitives.WriteInt32LittleEndian(payload.Slice(num), array.Length);
			array.CopyTo(payload.Slice(num + 4));
			num += 4 + array.Length;
		}
	}

	private static void WriteMetaField(Span<byte> field, in Meta meta)
	{
		BinaryPrimitives.WriteUInt16LittleEndian(field, 21591);
		BinaryPrimitives.WriteUInt16LittleEndian(field.Slice(2), 38);
		field[4] = 1;
		field[5] = meta.Flags;
		BinaryPrimitives.WriteInt64LittleEndian(field.Slice(6), meta.SourceLength);
		BinaryPrimitives.WriteInt64LittleEndian(field.Slice(14), meta.SourceTicks);
		BinaryPrimitives.WriteInt32LittleEndian(field.Slice(22), meta.Width);
		BinaryPrimitives.WriteInt32LittleEndian(field.Slice(26), meta.Height);
		BinaryPrimitives.WriteInt32LittleEndian(field.Slice(30), meta.MipCount);
		BinaryPrimitives.WriteInt32LittleEndian(field.Slice(34), meta.Format);
		BinaryPrimitives.WriteInt32LittleEndian(field.Slice(38), meta.Swizzle);
	}

	private static bool TryReadMeta(ReadOnlySpan<byte> extra, out Meta meta)
	{
		meta = default(Meta);
		if (!ZipFormat.TryFindExtraField(extra, 21591, out var data) || data.Length < 38 || data[0] != 1)
		{
			return false;
		}
		meta = new Meta(data[1], BinaryPrimitives.ReadInt64LittleEndian(data.Slice(2)), BinaryPrimitives.ReadInt64LittleEndian(data.Slice(10)), BinaryPrimitives.ReadInt32LittleEndian(data.Slice(18)), BinaryPrimitives.ReadInt32LittleEndian(data.Slice(22)), BinaryPrimitives.ReadInt32LittleEndian(data.Slice(26)), BinaryPrimitives.ReadInt32LittleEndian(data.Slice(30)), BinaryPrimitives.ReadInt32LittleEndian(data.Slice(34)));
		return true;
	}

	private static int LocalHeaderSize(string name, AssetPackCodec codec)
	{
		return 30 + Encoding.UTF8.GetByteCount(ZipFormat.StoredName(name, codec)) + 42;
	}

	private static uint DosTime(DateTime time)
	{
		if (time.Year < 1980)
		{
			time = new DateTime(1980, 1, 1);
		}
		int num = (time.Year - 1980 << 9) | (time.Month << 5) | time.Day;
		int num2 = (time.Hour << 11) | (time.Minute << 5) | (time.Second / 2);
		return (uint)((num << 16) | num2);
	}
}
