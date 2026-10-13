using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Waylith.Assets;

internal static class ZipFormat
{
	public readonly record struct Record(string Name, AssetPackCodec Codec, uint Crc, long CompressedLength, long Length, long LocalHeaderOffset, uint DosTime, byte[] Extra);

	public const uint LocalHeaderSignature = 67324752u;

	public const uint CentralHeaderSignature = 33639248u;

	public const uint EndSignature = 101010256u;

	public const uint Zip64EndSignature = 101075792u;

	public const uint Zip64LocatorSignature = 117853008u;

	public const int LocalHeaderLength = 30;

	public const int CentralHeaderLength = 46;

	public const int EndLength = 22;

	public const int Zip64EndLength = 56;

	public const int Zip64LocatorLength = 20;

	public const ushort MethodStore = 0;

	public const ushort MethodZstd = 93;

	public const ushort Utf8NamesFlag = 2048;

	public const ushort Zip64FieldId = 1;

	public const int Zip64OffsetFieldLength = 12;

	public static ushort MethodFor(AssetPackCodec codec)
	{
		if (codec != AssetPackCodec.Zstd)
		{
			return 0;
		}
		return 93;
	}

	public static string StoredName(string name, AssetPackCodec codec)
	{
		if (codec != AssetPackCodec.Lz4)
		{
			return name;
		}
		return name + ".lz4";
	}

	public static AssetPackCodec CodecOf(ushort method, ref string storedName)
	{
		if (method == 93)
		{
			return AssetPackCodec.Zstd;
		}
		if (!storedName.EndsWith(".lz4", StringComparison.Ordinal))
		{
			return AssetPackCodec.Store;
		}
		string text = storedName;
		int length = ".lz4".Length;
		storedName = text.Substring(0, text.Length - length);
		return AssetPackCodec.Lz4;
	}

	public static void WriteSharedFields(Span<byte> fields, ushort versionNeeded, ushort method, uint dosTime, uint crc, uint compressedLength, uint length, int nameLength, int extraLength)
	{
		BinaryPrimitives.WriteUInt16LittleEndian(fields, versionNeeded);
		BinaryPrimitives.WriteUInt16LittleEndian(fields.Slice(2), 2048);
		BinaryPrimitives.WriteUInt16LittleEndian(fields.Slice(4), method);
		BinaryPrimitives.WriteUInt32LittleEndian(fields.Slice(6), dosTime);
		BinaryPrimitives.WriteUInt32LittleEndian(fields.Slice(10), crc);
		BinaryPrimitives.WriteUInt32LittleEndian(fields.Slice(14), compressedLength);
		BinaryPrimitives.WriteUInt32LittleEndian(fields.Slice(18), length);
		BinaryPrimitives.WriteUInt16LittleEndian(fields.Slice(22), (ushort)nameLength);
		BinaryPrimitives.WriteUInt16LittleEndian(fields.Slice(24), (ushort)extraLength);
	}

	public static void WriteLocalHeader(Span<byte> header, ushort versionNeeded, ushort method, uint dosTime, uint crc, uint compressedLength, uint length, int nameLength, int extraLength)
	{
		BinaryPrimitives.WriteUInt32LittleEndian(header, 67324752u);
		WriteSharedFields(header.Slice(4), versionNeeded, method, dosTime, crc, compressedLength, length, nameLength, extraLength);
	}

	public static void WriteCentralHeader(Span<byte> header, ushort versionMadeBy, ushort versionNeeded, ushort method, uint dosTime, uint crc, uint compressedLength, uint length, int nameLength, int extraLength, uint externalAttributes, long localHeaderOffset)
	{
		header.Slice(0, 46).Clear();
		BinaryPrimitives.WriteUInt32LittleEndian(header, 33639248u);
		BinaryPrimitives.WriteUInt16LittleEndian(header.Slice(4), versionMadeBy);
		WriteSharedFields(header.Slice(6), versionNeeded, method, dosTime, crc, compressedLength, length, nameLength, extraLength);
		BinaryPrimitives.WriteUInt32LittleEndian(header.Slice(38), externalAttributes);
		BinaryPrimitives.WriteUInt32LittleEndian(header.Slice(42), (uint)(NeedsZip64(localHeaderOffset) ? uint.MaxValue : localHeaderOffset));
	}

	public static bool NeedsZip64(long offset)
	{
		return offset >= uint.MaxValue;
	}

	public static void WriteZip64OffsetField(Span<byte> field, long localHeaderOffset)
	{
		BinaryPrimitives.WriteUInt16LittleEndian(field, 1);
		BinaryPrimitives.WriteUInt16LittleEndian(field.Slice(2), 8);
		BinaryPrimitives.WriteInt64LittleEndian(field.Slice(4), localHeaderOffset);
	}

	public static void WriteEnd(Stream stream, long count, long directorySize, long directoryOffset, ushort versionMadeBy, ReadOnlySpan<byte> comment = default(ReadOnlySpan<byte>))
	{
		if (count >= 65535 || directoryOffset >= uint.MaxValue || directorySize >= uint.MaxValue)
		{
			Span<byte> span = stackalloc byte[76];
			span.Clear();
			BinaryPrimitives.WriteUInt32LittleEndian(span, 101075792u);
			BinaryPrimitives.WriteInt64LittleEndian(span.Slice(4), 44L);
			BinaryPrimitives.WriteUInt16LittleEndian(span.Slice(12), versionMadeBy);
			BinaryPrimitives.WriteUInt16LittleEndian(span.Slice(14), versionMadeBy);
			BinaryPrimitives.WriteInt64LittleEndian(span.Slice(24), count);
			BinaryPrimitives.WriteInt64LittleEndian(span.Slice(32), count);
			BinaryPrimitives.WriteInt64LittleEndian(span.Slice(40), directorySize);
			BinaryPrimitives.WriteInt64LittleEndian(span.Slice(48), directoryOffset);
			Span<byte> destination = span.Slice(56);
			BinaryPrimitives.WriteUInt32LittleEndian(destination, 117853008u);
			BinaryPrimitives.WriteInt64LittleEndian(destination.Slice(8), directoryOffset + directorySize);
			BinaryPrimitives.WriteUInt32LittleEndian(destination.Slice(16), 1u);
			stream.Write(span);
		}
		Span<byte> span2 = stackalloc byte[22];
		span2.Clear();
		BinaryPrimitives.WriteUInt32LittleEndian(span2, 101010256u);
		BinaryPrimitives.WriteUInt16LittleEndian(span2.Slice(8), (ushort)System.Math.Min(count, 65535L));
		BinaryPrimitives.WriteUInt16LittleEndian(span2.Slice(10), (ushort)System.Math.Min(count, 65535L));
		BinaryPrimitives.WriteUInt32LittleEndian(span2.Slice(12), (uint)System.Math.Min(directorySize, 4294967295L));
		BinaryPrimitives.WriteUInt32LittleEndian(span2.Slice(16), (uint)System.Math.Min(directoryOffset, 4294967295L));
		BinaryPrimitives.WriteUInt16LittleEndian(span2.Slice(20), (ushort)comment.Length);
		stream.Write(span2);
		stream.Write(comment);
	}

	public static bool TryReadCentralDirectory(SafeFileHandle handle, out List<Record> records, out long directoryOffset, out string comment)
	{
		records = new List<Record>();
		directoryOffset = 0L;
		comment = "";
		long length = RandomAccess.GetLength(handle);
		if (length < 22)
		{
			return false;
		}
		int num = (int)System.Math.Min(length, 65557L);
		byte[] array = new byte[num];
		if (!TryReadExactly(handle, array, length - num))
		{
			return false;
		}
		int num2 = -1;
		for (int num3 = num - 22; num3 >= 0; num3--)
		{
			if (BinaryPrimitives.ReadUInt32LittleEndian(array.AsSpan(num3)) == 101010256 && num3 + 22 + BinaryPrimitives.ReadUInt16LittleEndian(array.AsSpan(num3 + 20)) == num)
			{
				num2 = num3;
				break;
			}
		}
		if (num2 < 0)
		{
			return false;
		}
		long num4 = BinaryPrimitives.ReadUInt16LittleEndian(array.AsSpan(num2 + 10));
		long num5 = BinaryPrimitives.ReadUInt32LittleEndian(array.AsSpan(num2 + 12));
		directoryOffset = BinaryPrimitives.ReadUInt32LittleEndian(array.AsSpan(num2 + 16));
		comment = Encoding.UTF8.GetString(array, num2 + 22, BinaryPrimitives.ReadUInt16LittleEndian(array.AsSpan(num2 + 20)));
		long num6 = length - num + num2;
		if (num4 == 65535 || num5 == uint.MaxValue || directoryOffset == uint.MaxValue)
		{
			Span<byte> span = stackalloc byte[20];
			Span<byte> span2 = stackalloc byte[56];
			if (num6 < 20 || !TryReadExactly(handle, span, num6 - 20) || BinaryPrimitives.ReadUInt32LittleEndian(span) != 117853008)
			{
				return false;
			}
			if (!TryReadExactly(handle, span2, BinaryPrimitives.ReadInt64LittleEndian(span.Slice(8))) || BinaryPrimitives.ReadUInt32LittleEndian(span2) != 101075792)
			{
				return false;
			}
			num4 = BinaryPrimitives.ReadInt64LittleEndian(span2.Slice(32));
			num5 = BinaryPrimitives.ReadInt64LittleEndian(span2.Slice(40));
			directoryOffset = BinaryPrimitives.ReadInt64LittleEndian(span2.Slice(48));
		}
		if (num4 < 0 || directoryOffset < 0 || num5 < 0 || directoryOffset + num5 > num6 || num5 > int.MaxValue)
		{
			return false;
		}
		byte[] array2 = new byte[num5];
		if (!TryReadExactly(handle, array2, directoryOffset))
		{
			return false;
		}
		records = new List<Record>((int)System.Math.Min(num4, 1048576L));
		int num7 = 0;
		for (long num8 = 0L; num8 < num4; num8++)
		{
			if (num7 + 46 > array2.Length)
			{
				return false;
			}
			Span<byte> span3 = array2.AsSpan(num7);
			if (BinaryPrimitives.ReadUInt32LittleEndian(span3) != 33639248)
			{
				return false;
			}
			ushort num9 = BinaryPrimitives.ReadUInt16LittleEndian(span3.Slice(28));
			ushort num10 = BinaryPrimitives.ReadUInt16LittleEndian(span3.Slice(30));
			ushort num11 = BinaryPrimitives.ReadUInt16LittleEndian(span3.Slice(32));
			if (num7 + 46 + num9 + num10 + num11 > array2.Length)
			{
				return false;
			}
			string storedName = Encoding.UTF8.GetString(span3.Slice(46, num9));
			AssetPackCodec codec = CodecOf(BinaryPrimitives.ReadUInt16LittleEndian(span3.Slice(10)), ref storedName);
			byte[] array3 = ((num10 == 0) ? Array.Empty<byte>() : span3.Slice(46 + num9, num10).ToArray());
			long num12 = BinaryPrimitives.ReadUInt32LittleEndian(span3.Slice(42));
			if (num12 == uint.MaxValue && TryFindExtraField(array3, 1, out var data) && data.Length >= 8)
			{
				num12 = BinaryPrimitives.ReadInt64LittleEndian(data);
			}
			uint num13 = BinaryPrimitives.ReadUInt32LittleEndian(span3.Slice(20));
			if (num12 < 0 || num12 + num13 > directoryOffset)
			{
				return false;
			}
			records.Add(new Record(storedName, codec, BinaryPrimitives.ReadUInt32LittleEndian(span3.Slice(16)), num13, BinaryPrimitives.ReadUInt32LittleEndian(span3.Slice(24)), num12, BinaryPrimitives.ReadUInt32LittleEndian(span3.Slice(12)), array3));
			num7 += 46 + num9 + num10 + num11;
		}
		return true;
	}

	public static bool TryFindExtraField(ReadOnlySpan<byte> extra, ushort id, out ReadOnlySpan<byte> data)
	{
		while (extra.Length >= 4)
		{
			ushort num = BinaryPrimitives.ReadUInt16LittleEndian(extra);
			ushort num2 = BinaryPrimitives.ReadUInt16LittleEndian(extra.Slice(2));
			if (4 + num2 > extra.Length)
			{
				break;
			}
			if (num == id)
			{
				data = extra.Slice(4, num2);
				return true;
			}
			extra = extra.Slice(4 + num2);
		}
		data = default(ReadOnlySpan<byte>);
		return false;
	}

	public static bool TryReadExactly(SafeFileHandle handle, Span<byte> destination, long offset)
	{
		try
		{
			while (!destination.IsEmpty)
			{
				int num = RandomAccess.Read(handle, destination, offset);
				if (num <= 0)
				{
					return false;
				}
				destination = destination.Slice(num);
				offset += num;
			}
			return true;
		}
		catch
		{
			return false;
		}
	}
}
