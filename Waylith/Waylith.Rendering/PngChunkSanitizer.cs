using System;
using System.Buffers.Binary;
using System.IO;

namespace Waylith.Rendering;

public static class PngChunkSanitizer
{
	private static ReadOnlySpan<byte> Signature => new byte[8] { 137, 80, 78, 71, 13, 10, 26, 10 };

	public static bool TryStripInvalidChunks(byte[] png, out byte[] sanitized)
	{
		sanitized = null;
		if (png.Length < 33 || !((ReadOnlySpan<byte>)png.AsSpan(0, 8)).SequenceEqual(Signature))
		{
			return false;
		}
		Span<byte> span = png.AsSpan(8);
		if (BinaryPrimitives.ReadUInt32BigEndian(span) != 13 || !((ReadOnlySpan<byte>)span.Slice(4, 4)).SequenceEqual("IHDR"u8))
		{
			return false;
		}
		byte b = png[25];
		using MemoryStream memoryStream = new MemoryStream(png.Length);
		memoryStream.Write(png, 0, 8);
		int i = 8;
		bool flag = false;
		int num;
		for (; i + 12 <= png.Length; i += 12 + num)
		{
			num = (int)BinaryPrimitives.ReadUInt32BigEndian(png.AsSpan(i));
			if (num < 0 || i + 12 + num > png.Length)
			{
				return false;
			}
			Span<byte> span2 = png.AsSpan(i + 4, 4);
			bool flag2 = ((ReadOnlySpan<byte>)span2).SequenceEqual("IHDR"u8) || ((ReadOnlySpan<byte>)span2).SequenceEqual("IDAT"u8) || ((ReadOnlySpan<byte>)span2).SequenceEqual("IEND"u8);
			if (!flag2)
			{
				bool flag3 = ((ReadOnlySpan<byte>)span2).SequenceEqual("PLTE"u8);
				if (flag3)
				{
					bool flag4 = (((uint)(b - 2) <= 1u || b == 6) ? true : false);
					flag3 = flag4;
				}
				flag2 = flag3;
			}
			bool flag5 = flag2;
			if (!flag5)
			{
				bool flag3 = ((ReadOnlySpan<byte>)span2).SequenceEqual("tRNS"u8);
				if (flag3)
				{
					bool flag4 = ((b == 0 || (uint)(b - 2) <= 1u) ? true : false);
					flag3 = flag4;
				}
				flag5 = flag3;
			}
			if (flag5)
			{
				memoryStream.Write(png, i, 12 + num);
			}
			else
			{
				flag = true;
			}
		}
		if (!flag)
		{
			return false;
		}
		sanitized = memoryStream.ToArray();
		return true;
	}
}
