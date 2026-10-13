using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Hashing;
using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Microsoft.Win32.SafeHandles;

namespace Waylith.Assets;

public sealed class AssetPack : IDisposable
{
	private readonly record struct Entry(long LocalHeaderOffset, int CompressedLength, int Length, uint Crc, AssetPackCodec Codec);

	private readonly SafeFileHandle handle;

	private readonly MemoryMappedFile map;

	private readonly SafeMemoryMappedViewHandle view;

	private unsafe readonly byte* bytes;

	private readonly long length;

	private readonly Dictionary<string, Entry> entries;

	private readonly Dictionary<string, string> aliases = new Dictionary<string, string>(StringComparer.Ordinal);

	public static long DecodedBytes;

	public static long DecodeTicks;

	public static long DecodeFirstTicks;

	public static long DecodeLastTicks;

	public const int DecodeBucketMs = 50;

	public static readonly long[] DecodeBuckets = new long[400];

	private static readonly long ProcessStartTicks = Stopwatch.GetTimestamp();

	public static int CorruptEntries;

	public IReadOnlyDictionary<string, string> ContentHashes { get; }

	public int Count => entries.Count;

	public IReadOnlyCollection<string> Names => entries.Keys;

	private unsafe AssetPack(SafeFileHandle handle, MemoryMappedFile map, SafeMemoryMappedViewHandle view, Dictionary<string, Entry> entries)
	{
		this.handle = handle;
		this.map = map;
		this.view = view;
		this.entries = entries;
		byte* pointer = null;
		view.AcquirePointer(ref pointer);
		bytes = pointer;
		length = RandomAccess.GetLength(handle);
		ContentHashes = AssetPackManifest.ReadContentHashes(this);
		AliasDeduplicatedEntries();
	}

	private void AliasDeduplicatedEntries()
	{
		Dictionary<string, string> dictionary = null;
		foreach (KeyValuePair<string, string> contentHash in ContentHashes)
		{
			contentHash.Deconstruct(out var key, out var value);
			string key2 = key;
			string key3 = value;
			if (entries.ContainsKey(key2))
			{
				continue;
			}
			if (dictionary == null)
			{
				dictionary = new Dictionary<string, string>(StringComparer.Ordinal);
				foreach (KeyValuePair<string, string> contentHash2 in ContentHashes)
				{
					contentHash2.Deconstruct(out value, out key);
					string text = value;
					string key4 = key;
					if (entries.ContainsKey(text))
					{
						dictionary.TryAdd(key4, text);
					}
				}
			}
			if (dictionary.TryGetValue(key3, out var value2))
			{
				aliases[key2] = value2;
			}
		}
	}

	public bool Contains(string name)
	{
		if (!entries.ContainsKey(name))
		{
			return aliases.ContainsKey(name);
		}
		return true;
	}

	private bool TryGetEntry(string name, out Entry entry)
	{
		if (entries.TryGetValue(name, out entry))
		{
			return true;
		}
		if (aliases.TryGetValue(name, out string value))
		{
			return entries.TryGetValue(value, out entry);
		}
		return false;
	}

	public static AssetPack? TryOpen(string path)
	{
		if (!File.Exists(path))
		{
			return null;
		}
		SafeFileHandle safeFileHandle = File.OpenHandle(path, FileMode.Open, FileAccess.Read, FileShare.Read, FileOptions.RandomAccess, 0L);
		MemoryMappedFile memoryMappedFile = null;
		SafeMemoryMappedViewHandle safeMemoryMappedViewHandle = null;
		try
		{
			memoryMappedFile = MemoryMappedFile.CreateFromFile(safeFileHandle, null, 0L, MemoryMappedFileAccess.Read, HandleInheritability.None, leaveOpen: true);
			safeMemoryMappedViewHandle = memoryMappedFile.CreateViewAccessor(0L, 0L, MemoryMappedFileAccess.Read).SafeMemoryMappedViewHandle;
			AssetPack assetPack = new AssetPack(safeFileHandle, memoryMappedFile, safeMemoryMappedViewHandle, ReadCentralDirectory(safeFileHandle));
			if (assetPack.Contains("bake.manifest") && assetPack.ContentHashes.Count == 0)
			{
				assetPack.Dispose();
				throw new InvalidDataException("The pack's manifest is damaged");
			}
			return assetPack;
		}
		catch
		{
			safeMemoryMappedViewHandle?.Dispose();
			memoryMappedFile?.Dispose();
			safeFileHandle.Dispose();
			throw;
		}
	}

	public bool TryRead(string name, out byte[] data)
	{
		data = Array.Empty<byte>();
		if (!TryGetEntry(name, out var entry))
		{
			return false;
		}
		byte[] array = new byte[entry.Length];
		if (!TryDecode(name, in entry, array))
		{
			return false;
		}
		data = array;
		return true;
	}

	public byte[] Read(string name)
	{
		if (!TryGetEntry(name, out var entry))
		{
			throw new KeyNotFoundException(name);
		}
		return Read(in entry);
	}

	public bool TryReadCompressed(string name, out byte[] compressed, out int length, out uint crc32, out AssetPackCodec codec)
	{
		compressed = Array.Empty<byte>();
		length = 0;
		crc32 = 0u;
		codec = AssetPackCodec.Store;
		if (!TryGetEntry(name, out var entry))
		{
			return false;
		}
		codec = entry.Codec;
		compressed = ReadPayload(in entry);
		length = entry.Length;
		crc32 = entry.Crc;
		return true;
	}

	internal bool TryGetCompressedInfo(string name, out int compressedLength, out int length, out uint crc32, out AssetPackCodec codec)
	{
		Entry entry;
		bool flag = TryGetEntry(name, out entry);
		compressedLength = entry.CompressedLength;
		length = entry.Length;
		crc32 = entry.Crc;
		codec = (flag ? entry.Codec : AssetPackCodec.Store);
		return flag;
	}

	internal ReadOnlySpan<byte> CompressedPayload(string name)
	{
		if (!TryGetEntry(name, out var entry))
		{
			throw new KeyNotFoundException(name);
		}
		return Payload(in entry);
	}

	public bool TryGetStoredPayload(string name, out ReadOnlySpan<byte> payload)
	{
		payload = default(ReadOnlySpan<byte>);
		if (!TryGetEntry(name, out var entry) || entry.Codec != 0)
		{
			return false;
		}
		payload = Payload(in entry);
		return true;
	}

	public unsafe bool TryGetStoredPayload(string name, out nint pointer, out int length)
	{
		pointer = IntPtr.Zero;
		length = 0;
		if (!TryGetStoredPayload(name, out var payload))
		{
			return false;
		}
		pointer = (nint)Unsafe.AsPointer(in MemoryMarshal.GetReference(payload));
		length = payload.Length;
		return true;
	}

	private unsafe ReadOnlySpan<byte> Payload(scoped in Entry entry)
	{
		if (entry.LocalHeaderOffset + 30 > length)
		{
			throw new InvalidDataException("Pack entry header lies past the end of the file");
		}
		ReadOnlySpan<byte> source = new ReadOnlySpan<byte>(bytes + entry.LocalHeaderOffset, 30);
		if (BinaryPrimitives.ReadUInt32LittleEndian(source) != 67324752)
		{
			throw new InvalidDataException("Corrupt pack entry header");
		}
		ushort num = BinaryPrimitives.ReadUInt16LittleEndian(source.Slice(26));
		ushort num2 = BinaryPrimitives.ReadUInt16LittleEndian(source.Slice(28));
		long num3 = entry.LocalHeaderOffset + 30 + num + num2;
		if (num3 + entry.CompressedLength > length)
		{
			throw new InvalidDataException("Pack entry runs past the end of the file");
		}
		return new ReadOnlySpan<byte>(bytes + num3, entry.CompressedLength);
	}

	private byte[] ReadPayload(in Entry entry)
	{
		return Payload(in entry).ToArray();
	}

	private static byte[] ReadPayload(SafeFileHandle handle, in Entry entry)
	{
		Span<byte> destination = stackalloc byte[30];
		ReadExactly(handle, destination, entry.LocalHeaderOffset);
		ushort num = BinaryPrimitives.ReadUInt16LittleEndian(destination.Slice(26));
		ushort num2 = BinaryPrimitives.ReadUInt16LittleEndian(destination.Slice(28));
		byte[] array = new byte[entry.CompressedLength];
		ReadExactly(handle, array, entry.LocalHeaderOffset + 30 + num + num2);
		return array;
	}

	private byte[] Read(in Entry entry)
	{
		byte[] array = new byte[entry.Length];
		Decode(in entry, array);
		return array;
	}

	private void Decode(in Entry entry, Span<byte> destination)
	{
		ReadOnlySpan<byte> source = Payload(in entry);
		long timestamp = Stopwatch.GetTimestamp();
		AssetPackCodecs.Decode(entry.Codec, source, destination.Slice(0, entry.Length));
		if (Crc32.HashToUInt32(destination) != entry.Crc)
		{
			throw new InvalidDataException("Pack entry failed its CRC check");
		}
		Record(timestamp, entry.Length);
	}

	private bool TryDecode(string name, in Entry entry, Span<byte> destination)
	{
		try
		{
			Decode(in entry, destination);
			return true;
		}
		catch (Exception ex) when (!(ex is OutOfMemoryException))
		{
			if (Interlocked.Increment(ref CorruptEntries) <= 10)
			{
				LogCategory log = RenderLog.Log;
				bool shouldAppend;
				LogString<Error> message = new LogString<Error>(75, 2, log, out shouldAppend);
				if (shouldAppend)
				{
					message.AppendLiteral("Asset pack entry '");
					message.AppendFormatted(name);
					message.AppendLiteral("' is damaged (");
					message.AppendFormatted(ex.Message);
					message.AppendLiteral("); using it as if the pack did not have it.");
				}
				log.Error(message, "TryDecode", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Assets\\AssetPack.cs", 269);
			}
			return false;
		}
	}

	private static void Record(long started, int length)
	{
		long timestamp = Stopwatch.GetTimestamp();
		Interlocked.Add(ref DecodeTicks, timestamp - started);
		Interlocked.Add(ref DecodedBytes, length);
		long num = Volatile.Read(in DecodeFirstTicks);
		while ((num == 0L || started < num) && Interlocked.CompareExchange(ref DecodeFirstTicks, started, num) != num)
		{
			num = Volatile.Read(in DecodeFirstTicks);
		}
		long num2 = Volatile.Read(in DecodeLastTicks);
		while (timestamp > num2 && Interlocked.CompareExchange(ref DecodeLastTicks, timestamp, num2) != num2)
		{
			num2 = Volatile.Read(in DecodeLastTicks);
		}
		int num3 = (int)((started - ProcessStartTicks) * 1000 / Stopwatch.Frequency / 50);
		if ((uint)num3 < (uint)DecodeBuckets.Length)
		{
			Interlocked.Add(ref DecodeBuckets[num3], timestamp - started);
		}
	}

	public bool TryRent(string name, out byte[] buffer, out int length)
	{
		if (!TryGetEntry(name, out var entry))
		{
			buffer = Array.Empty<byte>();
			length = 0;
			return false;
		}
		buffer = ArrayPool<byte>.Shared.Rent(entry.Length);
		length = entry.Length;
		if (TryDecode(name, in entry, buffer.AsSpan(0, entry.Length)))
		{
			return true;
		}
		ArrayPool<byte>.Shared.Return(buffer);
		buffer = Array.Empty<byte>();
		length = 0;
		return false;
	}

	public static void Return(byte[] buffer)
	{
		ArrayPool<byte>.Shared.Return(buffer);
	}

	private void ReadExactly(Span<byte> destination, long offset)
	{
		ReadExactly(handle, destination, offset);
	}

	private static void ReadExactly(SafeFileHandle handle, Span<byte> destination, long offset)
	{
		while (!destination.IsEmpty)
		{
			int num = RandomAccess.Read(handle, destination, offset);
			if (num <= 0)
			{
				throw new EndOfStreamException();
			}
			destination = destination.Slice(num);
			offset += num;
		}
	}

	private static Dictionary<string, Entry> ReadCentralDirectory(SafeFileHandle handle)
	{
		if (!ZipFormat.TryReadCentralDirectory(handle, out List<ZipFormat.Record> records, out long _, out string _))
		{
			throw new InvalidDataException("Not a readable zip archive");
		}
		Dictionary<string, Entry> dictionary = new Dictionary<string, Entry>(records.Count, StringComparer.Ordinal);
		foreach (ZipFormat.Record item in records)
		{
			if (item.CompressedLength > int.MaxValue || item.Length > int.MaxValue)
			{
				throw new InvalidDataException("Corrupt central directory");
			}
			dictionary[item.Name] = new Entry(item.LocalHeaderOffset, (int)item.CompressedLength, (int)item.Length, item.Crc, item.Codec);
		}
		return dictionary;
	}

	public void Dispose()
	{
		view.ReleasePointer();
		view.Dispose();
		map.Dispose();
		handle.Dispose();
	}
}
