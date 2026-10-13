using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.IO.Hashing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace Waylith.Assets;

public sealed class AssetPackWriter : IDisposable
{
	public readonly record struct Payload
	{
		internal AssetPack? Source { get; init; }

		internal string? SourceName { get; init; }

		internal long Offset { get; init; }

		internal int Length { get; init; }

		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			return false;
		}
	}

	private readonly record struct Pending(string Name, Payload Payload, uint Crc, int Length, bool Stored);

	private readonly record struct Central(string Name, uint Crc, int CompressedLength, int Length, long Offset, bool Stored);

	private readonly FileStream stream;

	private readonly SafeFileHandle spill;

	private readonly int alignment;

	private readonly List<Pending> pending = new List<Pending>();

	private readonly Lock gate = new Lock();

	private long spillLength;

	private const ushort VersionNeeded = 20;

	private const ushort VersionNeededZip64 = 45;

	private const ushort VersionMadeBy = 788;

	private const uint DosTime = 2162688u;

	private const uint ExternalAttributes = 27525120u;

	public AssetPackWriter(string path, int alignment = 0)
	{
		stream = File.Create(path);
		spill = File.OpenHandle(path + ".spill", FileMode.Create, FileAccess.ReadWrite, FileShare.None, FileOptions.DeleteOnClose, 0L);
		this.alignment = alignment;
	}

	public static byte[] Compress(ReadOnlySpan<byte> data, AssetPackCodec codec = AssetPackCodec.Zstd, int level = 19)
	{
		return AssetPackCodecs.Compress(data, codec, level);
	}

	public static uint Crc32Of(ReadOnlySpan<byte> data)
	{
		return BinaryPrimitives.ReadUInt32LittleEndian(Crc32.Hash(data));
	}

	public Payload Store(ReadOnlySpan<byte> compressed)
	{
		long num = Interlocked.Add(ref spillLength, compressed.Length) - compressed.Length;
		RandomAccess.Write(spill, compressed, num);
		Payload result = default(Payload);
		result.Offset = num;
		result.Length = compressed.Length;
		return result;
	}

	public static bool TryReference(AssetPack pack, string name, out Payload payload, out int length, out uint crc32, out AssetPackCodec codec)
	{
		payload = default(Payload);
		if (!pack.TryGetCompressedInfo(name, out var compressedLength, out length, out crc32, out codec))
		{
			return false;
		}
		payload = new Payload
		{
			Source = pack,
			SourceName = name,
			Length = compressedLength
		};
		return true;
	}

	public void Add(string name, byte[] compressed, int length, uint crc32, AssetPackCodec codec = AssetPackCodec.Zstd)
	{
		Add(name, Store(compressed), length, crc32, codec);
	}

	public void Add(string name, Payload payload, int length, uint crc32, AssetPackCodec codec = AssetPackCodec.Zstd)
	{
		name = ZipFormat.StoredName(name, codec);
		using (gate.EnterScope())
		{
			pending.Add(new Pending(name, payload, crc32, length, codec != AssetPackCodec.Zstd));
		}
	}

	public void Dispose()
	{
		try
		{
			Write();
		}
		finally
		{
			stream.Dispose();
			spill.Dispose();
		}
	}

	private void Write()
	{
		pending.Sort((Pending a, Pending b) => string.CompareOrdinal(a.Name, b.Name));
		List<Central> list = new List<Central>(pending.Count);
		Span<byte> span = stackalloc byte[30];
		byte[] array = ArrayPool<byte>.Shared.Rent((pending.Count != 0) ? pending.Max((Pending entry) => entry.Payload.Length) : 0);
		foreach (Pending item in pending)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(item.Name);
			int length = item.Payload.Length;
			PadToBoundaryIfNeeded(30 + bytes.Length + length);
			long position = stream.Position;
			ZipFormat.WriteLocalHeader(span, 20, MethodOf(item.Stored), 2162688u, item.Crc, (uint)length, (uint)item.Length, bytes.Length, 0);
			stream.Write(span);
			stream.Write(bytes);
			stream.Write(PayloadBytes(item.Payload, array));
			list.Add(new Central(item.Name, item.Crc, length, item.Length, position, item.Stored));
		}
		ArrayPool<byte>.Shared.Return(array);
		long position2 = stream.Position;
		Span<byte> span2 = stackalloc byte[46];
		Span<byte> span3 = stackalloc byte[12];
		foreach (Central item2 in list)
		{
			byte[] bytes2 = Encoding.UTF8.GetBytes(item2.Name);
			bool flag = ZipFormat.NeedsZip64(item2.Offset);
			ZipFormat.WriteCentralHeader(span2, 788, (ushort)(flag ? 45 : 20), MethodOf(item2.Stored), 2162688u, item2.Crc, (uint)item2.CompressedLength, (uint)item2.Length, bytes2.Length, flag ? 12 : 0, 27525120u, item2.Offset);
			stream.Write(span2);
			stream.Write(bytes2);
			if (flag)
			{
				ZipFormat.WriteZip64OffsetField(span3, item2.Offset);
				stream.Write(span3);
			}
		}
		ZipFormat.WriteEnd(stream, list.Count, stream.Position - position2, position2, 788);
	}

	private static ushort MethodOf(bool stored)
	{
		if (!stored)
		{
			return 93;
		}
		return 0;
	}

	private ReadOnlySpan<byte> PayloadBytes(in Payload payload, byte[] buffer)
	{
		AssetPack source = payload.Source;
		if (source != null)
		{
			return source.CompressedPayload(payload.SourceName);
		}
		Span<byte> span = buffer.AsSpan(0, payload.Length);
		int num;
		for (int i = 0; i < span.Length; i += num)
		{
			num = RandomAccess.Read(spill, span.Slice(i), payload.Offset + i);
			if (num == 0)
			{
				throw new EndOfStreamException("Pack spill file ended early");
			}
		}
		return span;
	}

	private void PadToBoundaryIfNeeded(int recordLength)
	{
		if (alignment > 0 && stream.Position % alignment != 0L)
		{
			int num = alignment - (int)(stream.Position % alignment);
			if (recordLength >= alignment || (recordLength < alignment / 4 && recordLength > num))
			{
				stream.Write(new byte[num]);
			}
		}
	}
}
