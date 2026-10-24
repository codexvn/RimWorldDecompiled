using System;
using System.Collections.Generic;
using System.IO;

namespace PsdSharp.Compression;

internal static class Rle
{
	public static byte[] Decode(Stream stream, uint width, uint height, byte channelDepth, bool isPsb = false)
	{
		int num = (int)(width * (channelDepth / 8));
		byte[] array = new byte[num * height];
		using BinaryReader binaryReader = new BinaryReader(stream);
		int[] array2 = new int[height];
		for (int i = 0; i < height; i++)
		{
			if (isPsb)
			{
				array2[i] = (binaryReader.ReadByte() << 24) | (binaryReader.ReadByte() << 16) | (binaryReader.ReadByte() << 8) | binaryReader.ReadByte();
			}
			else
			{
				array2[i] = (binaryReader.ReadByte() << 8) | binaryReader.ReadByte();
			}
		}
		int num2 = 0;
		for (int j = 0; j < height; j++)
		{
			byte[] array3 = DecodePackBits(binaryReader.ReadBytes(array2[j]), num);
			if (array3.Length != num)
			{
				throw new InvalidDataException($"RLE row {j} expected {num} bytes but got {array3.Length}.");
			}
			Buffer.BlockCopy(array3, 0, array, num2, num);
			num2 += num;
		}
		return array;
	}

	private static byte[] DecodePackBits(byte[] data, int expectedSize)
	{
		List<byte> list = new List<byte>(expectedSize);
		int num = 0;
		while (num < data.Length)
		{
			int num2 = (sbyte)data[num++];
			if (num2 >= 0)
			{
				int num3 = num2 + 1;
				if (num + num3 > data.Length)
				{
					throw new InvalidDataException("PackBits literal overrun.");
				}
				for (int i = 0; i < num3; i++)
				{
					list.Add(data[num++]);
				}
			}
			else if (num2 != -128)
			{
				int num4 = -num2 + 1;
				if (num >= data.Length)
				{
					throw new InvalidDataException("PackBits repeat missing value.");
				}
				byte item = data[num++];
				for (int j = 0; j < num4; j++)
				{
					list.Add(item);
				}
			}
		}
		if (list.Count != expectedSize)
		{
			throw new InvalidDataException($"PackBits expected {expectedSize} bytes but got {list.Count}.");
		}
		return list.ToArray();
	}
}
