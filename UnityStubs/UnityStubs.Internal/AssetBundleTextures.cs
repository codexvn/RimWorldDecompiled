using System;
using System.Collections.Generic;
using AssetsTools.NET.Texture;
using UnityEngine;
using Waylith.Rendering;

namespace UnityStubs.Internal;

internal static class AssetBundleTextures
{
	public static UnityEngine.Texture2D Create(TextureFile tf, byte[] data, string bundleName)
	{
		AssetsTools.NET.Texture.TextureFormat textureFormat = (AssetsTools.NET.Texture.TextureFormat)tf.m_TextureFormat;
		int width = tf.m_Width;
		int height = tf.m_Height;
		if (width <= 0 || height <= 0)
		{
			return null;
		}
		int mipCount = Math.Max(1, tf.m_MipCount);
		LoadedTextureFormat loadedFormat;
		byte[][] array = ConvertUnityLevels(data, textureFormat, width, height, ref mipCount, out loadedFormat);
		if (array == null)
		{
			Debug.LogWarning($"AssetBundle '{bundleName}': texture '{tf.m_Name}' ({width}x{height} {textureFormat}) could not be converted.");
			return null;
		}
		UnityEngine.Texture2D texture2D = new UnityEngine.Texture2D(width, height, (UnityEngine.TextureFormat)loadedFormat, mipCount, linear: false, createUninitialized: true);
		texture2D.CoreTexture.ReplaceCpuData(width, height, loadedFormat, array, mipCount, linear: false);
		texture2D.ImportedUnreadable = !tf.m_IsReadable;
		return texture2D;
	}

	internal static byte[][] ConvertUnityLevels(byte[] data, AssetsTools.NET.Texture.TextureFormat format, int width, int height, ref int mipCount, out LoadedTextureFormat loadedFormat)
	{
		if (TryMapNative(format, out loadedFormat, out var _) && !NeedsDecode(loadedFormat, height, mipCount))
		{
			return SplitNativeLevels(data, loadedFormat, width, height, ref mipCount);
		}
		loadedFormat = LoadedTextureFormat.Rgba32;
		return DecodeLevels(data, format, width, height, ref mipCount);
	}

	internal static byte[] ToUnityOrder(byte[] level, LoadedTextureFormat format, int w, int h)
	{
		byte[] array = (byte[])level.Clone();
		if (!NeedsDecode(format, h, 1))
		{
			FlipNative(array, format, w, h);
		}
		return array;
	}

	private static bool TryMapNative(AssetsTools.NET.Texture.TextureFormat format, out LoadedTextureFormat loaded, out UnityEngine.TextureFormat unity)
	{
		(loaded, unity) = format switch
		{
			AssetsTools.NET.Texture.TextureFormat.Alpha8 => (LoadedTextureFormat.Alpha8, UnityEngine.TextureFormat.Alpha8), 
			AssetsTools.NET.Texture.TextureFormat.RGBA32 => (LoadedTextureFormat.Rgba32, UnityEngine.TextureFormat.RGBA32), 
			AssetsTools.NET.Texture.TextureFormat.DXT1 => (LoadedTextureFormat.Dxt1, UnityEngine.TextureFormat.DXT1), 
			AssetsTools.NET.Texture.TextureFormat.DXT5 => (LoadedTextureFormat.Dxt5, UnityEngine.TextureFormat.DXT5), 
			AssetsTools.NET.Texture.TextureFormat.BC4 => (LoadedTextureFormat.Bc4, UnityEngine.TextureFormat.BC4), 
			AssetsTools.NET.Texture.TextureFormat.BC5 => (LoadedTextureFormat.Bc5, UnityEngine.TextureFormat.BC5), 
			AssetsTools.NET.Texture.TextureFormat.BC7 => (LoadedTextureFormat.Bc7, UnityEngine.TextureFormat.BC7), 
			AssetsTools.NET.Texture.TextureFormat.R8 => (LoadedTextureFormat.R8, UnityEngine.TextureFormat.R8), 
			AssetsTools.NET.Texture.TextureFormat.RFloat => (LoadedTextureFormat.RFloat, UnityEngine.TextureFormat.RFloat), 
			AssetsTools.NET.Texture.TextureFormat.RGFloat => (LoadedTextureFormat.RGFloat, UnityEngine.TextureFormat.RGFloat), 
			AssetsTools.NET.Texture.TextureFormat.RGBAFloat => (LoadedTextureFormat.RgbaFloat, UnityEngine.TextureFormat.RGBAFloat), 
			_ => ((LoadedTextureFormat)0, (UnityEngine.TextureFormat)0), 
		};
		return loaded != (LoadedTextureFormat)0;
	}

	private static bool NeedsDecode(LoadedTextureFormat format, int height, int mipCount)
	{
		if (!TexturePixelData.IsBlockCompressed(format))
		{
			return false;
		}
		for (int i = 0; i < mipCount; i++)
		{
			int num = Math.Max(1, height >> i);
			if (num >= 4 && (num & 3) != 0)
			{
				return true;
			}
		}
		return false;
	}

	private static int NativeLevelSize(LoadedTextureFormat format, int w, int h)
	{
		int num = TexturePixelData.BlockBytes(format);
		if (num != 0)
		{
			return (w + 3) / 4 * ((h + 3) / 4) * num;
		}
		return w * h * TexturePixelData.BytesPerPixel(format);
	}

	private static byte[][] SplitNativeLevels(byte[] data, LoadedTextureFormat format, int width, int height, ref int mipCount)
	{
		List<byte[]> list = new List<byte[]>(mipCount);
		int num = 0;
		for (int i = 0; i < mipCount; i++)
		{
			int w = Math.Max(1, width >> i);
			int h = Math.Max(1, height >> i);
			int num2 = NativeLevelSize(format, w, h);
			if (num + num2 > data.Length)
			{
				break;
			}
			byte[] array = new byte[num2];
			Buffer.BlockCopy(data, num, array, 0, num2);
			num += num2;
			FlipNative(array, format, w, h);
			list.Add(array);
		}
		if (list.Count == 0)
		{
			return null;
		}
		mipCount = list.Count;
		return list.ToArray();
	}

	private static void FlipNative(byte[] level, LoadedTextureFormat format, int w, int h)
	{
		int num = TexturePixelData.BlockBytes(format);
		if (num == 0)
		{
			FlipRows(level, w * TexturePixelData.BytesPerPixel(format), h);
			return;
		}
		int num2 = (w + 3) / 4;
		int rows = (h + 3) / 4;
		int validRows = ((h < 4) ? h : 4);
		FlipRows(level, num2 * num, rows);
		for (int i = 0; i + num <= level.Length; i += num)
		{
			switch (format)
			{
			case LoadedTextureFormat.Dxt1:
				FlipBc1Block(level, i, validRows);
				break;
			case LoadedTextureFormat.Bc4:
				FlipBc4Block(level, i, validRows);
				break;
			case LoadedTextureFormat.Dxt5:
				FlipBc4Block(level, i, validRows);
				FlipBc1Block(level, i + 8, validRows);
				break;
			case LoadedTextureFormat.Bc5:
				FlipBc4Block(level, i, validRows);
				FlipBc4Block(level, i + 8, validRows);
				break;
			case LoadedTextureFormat.Bc7:
				Bc7BlockFlip.FlipRows(level.AsSpan(i, num), validRows);
				break;
			}
		}
	}

	private static void FlipRows(byte[] data, int rowBytes, int rows)
	{
		if (rows >= 2 && rowBytes > 0)
		{
			Span<byte> span = ((rowBytes > 4096) ? ((Span<byte>)new byte[rowBytes]) : stackalloc byte[rowBytes]);
			Span<byte> destination = span;
			int num = 0;
			int num2 = rows - 1;
			while (num < num2)
			{
				Span<byte> destination2 = data.AsSpan(num * rowBytes, rowBytes);
				Span<byte> destination3 = data.AsSpan(num2 * rowBytes, rowBytes);
				destination2.CopyTo(destination);
				destination3.CopyTo(destination2);
				destination.CopyTo(destination3);
				num++;
				num2--;
			}
		}
	}

	private static void FlipBc1Block(byte[] data, int offset, int validRows)
	{
		int i = 0;
		int num = offset + 4;
		for (; i < validRows / 2; i++)
		{
			ref byte reference = ref data[num + i];
			ref byte reference2 = ref data[num + validRows - 1 - i];
			byte b = data[num + validRows - 1 - i];
			byte b2 = data[num + i];
			reference = b;
			reference2 = b2;
		}
	}

	private static void FlipBc4Block(byte[] data, int offset, int validRows)
	{
		ulong num = 0uL;
		for (int i = 0; i < 6; i++)
		{
			num |= (ulong)data[offset + 2 + i] << 8 * i;
		}
		ulong num2 = num & 0xFFFF000000000000uL;
		for (int j = 0; j < 4; j++)
		{
			int num3 = ((j < validRows) ? (validRows - 1 - j) : j);
			ulong num4 = (num >> 12 * num3) & 0xFFF;
			num2 |= num4 << 12 * j;
		}
		for (int k = 0; k < 6; k++)
		{
			data[offset + 2 + k] = (byte)(num2 >> 8 * k);
		}
	}

	private static byte[][] DecodeLevels(byte[] data, AssetsTools.NET.Texture.TextureFormat format, int width, int height, ref int mipCount)
	{
		List<byte[]> list = new List<byte[]>(mipCount);
		if (TryLevelSize(format, width, height, out var _))
		{
			int num = 0;
			for (int i = 0; i < mipCount; i++)
			{
				int w = Math.Max(1, width >> i);
				int h = Math.Max(1, height >> i);
				TryLevelSize(format, w, h, out var size2);
				if (num + size2 > data.Length)
				{
					break;
				}
				byte[] array = new byte[size2];
				Buffer.BlockCopy(data, num, array, 0, size2);
				num += size2;
				byte[] array2 = Decode(array, format, w, h);
				if (array2 == null)
				{
					return null;
				}
				list.Add(array2);
			}
		}
		else
		{
			byte[] array3 = Decode(data, format, width, height);
			if (array3 == null)
			{
				return null;
			}
			list.AddRange((IEnumerable<byte[]>)((mipCount > 1) ? ((Array)TexturePixelData.GenerateRgba32MipChain(array3, width, height, mipCount)) : ((Array)new byte[1][] { array3 })));
		}
		if (list.Count == 0)
		{
			return null;
		}
		mipCount = list.Count;
		return list.ToArray();
	}

	internal static byte[] DecodeToRgba32(byte[] data, LoadedTextureFormat format, int w, int h)
	{
		AssetsTools.NET.Texture.TextureFormat? textureFormat = format switch
		{
			LoadedTextureFormat.Alpha8 => AssetsTools.NET.Texture.TextureFormat.Alpha8, 
			LoadedTextureFormat.Rgb24 => AssetsTools.NET.Texture.TextureFormat.RGB24, 
			LoadedTextureFormat.Rgba32 => AssetsTools.NET.Texture.TextureFormat.RGBA32, 
			LoadedTextureFormat.Dxt1 => AssetsTools.NET.Texture.TextureFormat.DXT1, 
			LoadedTextureFormat.Dxt5 => AssetsTools.NET.Texture.TextureFormat.DXT5, 
			LoadedTextureFormat.RFloat => AssetsTools.NET.Texture.TextureFormat.RFloat, 
			LoadedTextureFormat.RGFloat => AssetsTools.NET.Texture.TextureFormat.RGFloat, 
			LoadedTextureFormat.RgbaFloat => AssetsTools.NET.Texture.TextureFormat.RGBAFloat, 
			LoadedTextureFormat.Rgba64 => AssetsTools.NET.Texture.TextureFormat.RGBA64, 
			LoadedTextureFormat.Bc7 => AssetsTools.NET.Texture.TextureFormat.BC7, 
			LoadedTextureFormat.Bc4 => AssetsTools.NET.Texture.TextureFormat.BC4, 
			LoadedTextureFormat.Bc5 => AssetsTools.NET.Texture.TextureFormat.BC5, 
			LoadedTextureFormat.R8 => AssetsTools.NET.Texture.TextureFormat.R8, 
			_ => null, 
		};
		if (textureFormat.HasValue)
		{
			AssetsTools.NET.Texture.TextureFormat valueOrDefault = textureFormat.GetValueOrDefault();
			return Decode(data, valueOrDefault, w, h, flipRows: false);
		}
		return null;
	}

	private static byte[] Decode(byte[] data, AssetsTools.NET.Texture.TextureFormat format, int w, int h, bool flipRows = true)
	{
		int size;
		bool flag = TryLevelSize(format, 1, 1, out size) && size > 4 && ((w & 3) != 0 || (h & 3) != 0);
		int num = (flag ? ((w + 3) & -4) : w);
		int num2 = (flag ? ((h + 3) & -4) : h);
		byte[] array;
		try
		{
			array = TextureFile.DecodeManagedData(data, format, num, num2, useBgra: false);
			if (array != null && flag && array.Length >= num * num2 * 4)
			{
				byte[] array2 = new byte[w * h * 4];
				for (int i = 0; i < h; i++)
				{
					Buffer.BlockCopy(array, i * num * 4, array2, i * w * 4, w * 4);
				}
				array = array2;
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning($"Texture decode failed ({format} {w}x{h}): {ex.Message}");
			return null;
		}
		if (array == null || array.Length < w * h * 4)
		{
			Debug.LogWarning($"Texture decode produced {((array != null) ? array.Length : 0)} bytes for {format} {w}x{h}, expected {w * h * 4}.");
			return null;
		}
		if (flipRows)
		{
			FlipRows(array, w * 4, h);
		}
		return array;
	}

	private static bool TryLevelSize(AssetsTools.NET.Texture.TextureFormat format, int w, int h, out int size)
	{
		int num;
		switch (format)
		{
		case AssetsTools.NET.Texture.TextureFormat.Alpha8:
		case AssetsTools.NET.Texture.TextureFormat.R8:
			num = w * h;
			break;
		case AssetsTools.NET.Texture.TextureFormat.ARGB4444:
		case AssetsTools.NET.Texture.TextureFormat.RGB565:
		case AssetsTools.NET.Texture.TextureFormat.R16:
		case AssetsTools.NET.Texture.TextureFormat.RGBA4444:
		case AssetsTools.NET.Texture.TextureFormat.RHalf:
		case AssetsTools.NET.Texture.TextureFormat.RG16:
			num = w * h * 2;
			break;
		case AssetsTools.NET.Texture.TextureFormat.RGB24:
			num = w * h * 3;
			break;
		case AssetsTools.NET.Texture.TextureFormat.RGBA32:
		case AssetsTools.NET.Texture.TextureFormat.ARGB32:
		case AssetsTools.NET.Texture.TextureFormat.BGRA32:
		case AssetsTools.NET.Texture.TextureFormat.RGHalf:
		case AssetsTools.NET.Texture.TextureFormat.RFloat:
		case AssetsTools.NET.Texture.TextureFormat.RGB9e5Float:
		case AssetsTools.NET.Texture.TextureFormat.RG32:
			num = w * h * 4;
			break;
		case AssetsTools.NET.Texture.TextureFormat.RGB48:
			num = w * h * 6;
			break;
		case AssetsTools.NET.Texture.TextureFormat.RGBAHalf:
		case AssetsTools.NET.Texture.TextureFormat.RGFloat:
		case AssetsTools.NET.Texture.TextureFormat.RGBA64:
			num = w * h * 8;
			break;
		case AssetsTools.NET.Texture.TextureFormat.RGBAFloat:
			num = w * h * 16;
			break;
		case AssetsTools.NET.Texture.TextureFormat.DXT1:
		case AssetsTools.NET.Texture.TextureFormat.BC4:
			num = Blocks(8);
			break;
		case AssetsTools.NET.Texture.TextureFormat.DXT5:
		case AssetsTools.NET.Texture.TextureFormat.BC6H:
		case AssetsTools.NET.Texture.TextureFormat.BC7:
		case AssetsTools.NET.Texture.TextureFormat.BC5:
			num = Blocks(16);
			break;
		default:
			num = 0;
			break;
		}
		size = num;
		return size > 0;
		int Blocks(int bytes)
		{
			return (w + 3) / 4 * ((h + 3) / 4) * bytes;
		}
	}
}
