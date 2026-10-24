using System;
using System.IO;

namespace PsdSharp.Compression;

internal static class Compression
{
	public static byte[] Decompress(byte[] data, ImageCompression compression, uint width, uint height, byte channelDepth, bool isPsb = false)
	{
		return Decompress(new MemoryStream(data), compression, width, height, channelDepth, isPsb);
	}

	public static byte[] Decompress(Stream stream, ImageCompression compression, uint width, uint height, byte channelDepth, bool isPsb = false)
	{
		MemoryStream memoryStream = new MemoryStream();
		switch (compression)
		{
		case ImageCompression.Raw:
		case ImageCompression.Rle:
			stream.CopyTo(memoryStream);
			break;
		case ImageCompression.ZipWithoutPrediction:
		case ImageCompression.ZipWithPrediction:
			ZLibHelper.Decompress(stream, memoryStream);
			break;
		}
		memoryStream.Position = 0L;
		return compression switch
		{
			ImageCompression.Raw => memoryStream.ToArray(), 
			ImageCompression.Rle => Rle.Decode(memoryStream, width, height, channelDepth, isPsb), 
			ImageCompression.ZipWithoutPrediction => memoryStream.ToArray(), 
			_ => DecodePrediction(memoryStream.ToArray(), width, height, channelDepth), 
		};
	}

	private static byte[] DecodePrediction(byte[] data, uint width, uint height, byte depth)
	{
		switch (depth)
		{
		case 8:
		{
			byte[] obj = (byte[])data.Clone();
			DeltaDecode(obj, 256, width, height);
			return obj;
		}
		case 16:
		{
			ushort[] array = new ushort[width * height];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (ushort)((data[2 * i] << 8) | data[2 * i + 1]);
			}
			DeltaDecode(array, 65536, width, height);
			byte[] array2 = new byte[array.Length * 2];
			for (int j = 0; j < array.Length; j++)
			{
				array2[2 * j] = (byte)(array[j] >> 8);
				array2[2 * j + 1] = (byte)(array[j] & 0xFF);
			}
			return array2;
		}
		case 32:
		{
			byte[] arr = (byte[])data.Clone();
			DeltaDecode(arr, 256, width * 4, height);
			return RestoreByteOrder(arr, width, height);
		}
		default:
			throw new ArgumentException($"Unsupported depth {depth}");
		}
	}

	private static void DeltaDecode(byte[] arr, int mod, uint w, uint h)
	{
		for (int i = 0; i < h; i++)
		{
			long num = i * w;
			for (int j = 0; j < w - 1; j++)
			{
				long num2 = num + j;
				arr[num2 + 1] = (byte)((arr[num2 + 1] + arr[num2]) % mod);
			}
		}
	}

	private static void DeltaDecode(ushort[] arr, int mod, uint w, uint h)
	{
		for (int i = 0; i < h; i++)
		{
			long num = i * w;
			for (int j = 0; j < w - 1; j++)
			{
				long num2 = num + j;
				arr[num2 + 1] = (ushort)((arr[num2 + 1] + arr[num2]) % mod);
			}
		}
		for (int k = 0; k < arr.Length; k++)
		{
			ushort num3 = arr[k];
			arr[k] = (ushort)((num3 >> 8) | (num3 << 8));
		}
	}

	private static byte[] RestoreByteOrder(byte[] arr, uint w, uint h)
	{
		byte[] array = new byte[arr.Length];
		int num = 0;
		int num2 = (int)(4 * w);
		for (int i = 0; i < num2 * h; i += num2)
		{
			for (int j = i; j < i + w; j++)
			{
				for (int k = j; k < j + num2; k += (int)w)
				{
					array[num++] = arr[k];
				}
			}
		}
		return array;
	}
}
