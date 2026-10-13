using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;

namespace Waylith.Audio;

public static class OggVorbisDemuxer
{
	private const int PageHeaderSize = 27;

	public static bool TryExtractVorbisStream(string path, out byte[] vorbis)
	{
		vorbis = null;
		byte[] array;
		try
		{
			array = File.ReadAllBytes(path);
		}
		catch
		{
			return false;
		}
		return TryExtractVorbisStream(array, out vorbis);
	}

	public static bool TryExtractVorbisStream(ReadOnlySpan<byte> span, out byte[] vorbis)
	{
		vorbis = null;
		int num = 0;
		uint num2 = 0u;
		bool flag = false;
		List<(uint, int, int)> list = new List<(uint, int, int)>();
		int num6;
		for (int i = 0; i + 27 <= span.Length; i += num6)
		{
			if (!span.Slice(i, 4).SequenceEqual("OggS"u8))
			{
				return false;
			}
			byte b = span[i + 5];
			uint num3 = BinaryPrimitives.ReadUInt32LittleEndian(span.Slice(i + 14));
			byte b2 = span[i + 26];
			int num4 = 27 + b2;
			if (i + num4 > span.Length)
			{
				return false;
			}
			int num5 = 0;
			ReadOnlySpan<byte> readOnlySpan = span.Slice(i + 27, b2);
			for (int j = 0; j < readOnlySpan.Length; j++)
			{
				byte b3 = readOnlySpan[j];
				num5 += b3;
			}
			num6 = num4 + num5;
			if (i + num6 > span.Length)
			{
				return false;
			}
			if ((b & 2) != 0)
			{
				num++;
				ReadOnlySpan<byte> readOnlySpan2 = span.Slice(i + num4, num5);
				if (!flag && readOnlySpan2.Length >= 7 && readOnlySpan2[0] == 1 && readOnlySpan2.Slice(1, 6).SequenceEqual("vorbis"u8))
				{
					num2 = num3;
					flag = true;
				}
			}
			list.Add((num3, i, num6));
		}
		if (!flag || num < 2)
		{
			return false;
		}
		int num7 = 0;
		foreach (var item in list)
		{
			if (item.Item1 == num2)
			{
				num7 += item.Item3;
			}
		}
		vorbis = new byte[num7];
		int num8 = 0;
		foreach (var item2 in list)
		{
			if (item2.Item1 == num2)
			{
				span.Slice(item2.Item2, item2.Item3).CopyTo(vorbis.AsSpan(num8));
				num8 += item2.Item3;
			}
		}
		return true;
	}
}
