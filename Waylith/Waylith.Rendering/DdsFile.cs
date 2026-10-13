using System;
using System.Buffers.Binary;

namespace Waylith.Rendering;

public static class DdsFile
{
	public const int HeaderLength = 148;

	private const uint Magic = 542327876u;

	private const uint HeaderFlags = 659463u;

	private const uint SurfaceCaps = 4198408u;

	private const uint PixelFormatFourCc = 4u;

	private const uint FourCcDx10 = 808540228u;

	private const uint Texture2D = 3u;

	private const int SwizzleOffset = 32;

	public static void WriteHeader(Span<byte> destination, int width, int height, int mipCount, LoadedTextureFormat format, DdsAlphaMode alphaMode, TextureSwizzle swizzle = default(TextureSwizzle))
	{
		destination = destination.Slice(0, 148);
		destination.Clear();
		BinaryPrimitives.WriteUInt32LittleEndian(destination, 542327876u);
		BinaryPrimitives.WriteUInt32LittleEndian(destination.Slice(4), 124u);
		BinaryPrimitives.WriteUInt32LittleEndian(destination.Slice(8), 659463u);
		BinaryPrimitives.WriteInt32LittleEndian(destination.Slice(12), height);
		BinaryPrimitives.WriteInt32LittleEndian(destination.Slice(16), width);
		BinaryPrimitives.WriteInt32LittleEndian(destination.Slice(20), LevelLength(width, height, format));
		BinaryPrimitives.WriteInt32LittleEndian(destination.Slice(28), mipCount);
		BinaryPrimitives.WriteInt32LittleEndian(destination.Slice(32), swizzle.Pack());
		BinaryPrimitives.WriteUInt32LittleEndian(destination.Slice(76), 32u);
		BinaryPrimitives.WriteUInt32LittleEndian(destination.Slice(80), 4u);
		BinaryPrimitives.WriteUInt32LittleEndian(destination.Slice(84), 808540228u);
		BinaryPrimitives.WriteUInt32LittleEndian(destination.Slice(108), 4198408u);
		BinaryPrimitives.WriteUInt32LittleEndian(destination.Slice(128), ToDxgiFormat(format));
		BinaryPrimitives.WriteUInt32LittleEndian(destination.Slice(132), 3u);
		BinaryPrimitives.WriteUInt32LittleEndian(destination.Slice(140), 1u);
		BinaryPrimitives.WriteUInt32LittleEndian(destination.Slice(144), (uint)alphaMode);
	}

	public static bool TryReadHeader(ReadOnlySpan<byte> source, out DdsInfo info)
	{
		info = default(DdsInfo);
		if (source.Length < 148 || BinaryPrimitives.ReadUInt32LittleEndian(source) != 542327876 || BinaryPrimitives.ReadUInt32LittleEndian(source.Slice(84)) != 808540228)
		{
			return false;
		}
		LoadedTextureFormat loadedTextureFormat = FromDxgiFormat(BinaryPrimitives.ReadUInt32LittleEndian(source.Slice(128)));
		if (loadedTextureFormat == (LoadedTextureFormat)0)
		{
			return false;
		}
		info = new DdsInfo(BinaryPrimitives.ReadInt32LittleEndian(source.Slice(16)), BinaryPrimitives.ReadInt32LittleEndian(source.Slice(12)), System.Math.Max(1, BinaryPrimitives.ReadInt32LittleEndian(source.Slice(28))), loadedTextureFormat, (DdsAlphaMode)BinaryPrimitives.ReadUInt32LittleEndian(source.Slice(144)), TextureSwizzle.Unpack(BinaryPrimitives.ReadInt32LittleEndian(source.Slice(32))));
		return true;
	}

	public static byte[] Encode(in LoadedTextureData texture, DdsAlphaMode alphaMode)
	{
		int num = 148;
		for (int i = 0; i < texture.MipCount; i++)
		{
			num += texture.Levels[i].Length;
		}
		byte[] array = new byte[num];
		WriteHeader(array, texture.Width, texture.Height, texture.MipCount, texture.Format, alphaMode, texture.Swizzle);
		int num2 = 148;
		for (int j = 0; j < texture.MipCount; j++)
		{
			texture.Levels[j].CopyTo(array.AsSpan(num2));
			num2 += texture.Levels[j].Length;
		}
		return array;
	}

	public static bool TryDecode(ReadOnlySpan<byte> dds, out LoadedTextureData texture, out DdsAlphaMode alphaMode)
	{
		texture = default(LoadedTextureData);
		alphaMode = DdsAlphaMode.Unknown;
		if (!TryReadHeader(dds, out var info))
		{
			return false;
		}
		alphaMode = info.AlphaMode;
		byte[][] array = new byte[info.MipCount][];
		int num = 148;
		for (int i = 0; i < info.MipCount; i++)
		{
			int width = System.Math.Max(1, info.Width >> i);
			int height = System.Math.Max(1, info.Height >> i);
			int num2 = LevelLength(width, height, info.Format);
			if (num + num2 > dds.Length)
			{
				return false;
			}
			array[i] = dds.Slice(num, num2).ToArray();
			num += num2;
		}
		int width2 = info.Width;
		int height2 = info.Height;
		int mipCount = info.MipCount;
		LoadedTextureFormat format = info.Format;
		bool opaque = alphaMode == DdsAlphaMode.Opaque;
		TextureSwizzle swizzle = info.Swizzle;
		texture = new LoadedTextureData(width2, height2, mipCount, format, array, opaque, null, swizzle);
		return true;
	}

	public static int LevelLength(int width, int height, LoadedTextureFormat format)
	{
		int num = TexturePixelData.BlockBytes(format);
		if (num > 0)
		{
			return (width + 3) / 4 * ((height + 3) / 4) * num;
		}
		return width * height * TexturePixelData.BytesPerPixel(format);
	}

	private static uint ToDxgiFormat(LoadedTextureFormat format)
	{
		switch (format)
		{
		case LoadedTextureFormat.Bc7:
			return 98u;
		case LoadedTextureFormat.Dxt1:
			return 71u;
		case LoadedTextureFormat.Dxt5:
			return 77u;
		case LoadedTextureFormat.Bc4:
			return 80u;
		case LoadedTextureFormat.Bc5:
			return 83u;
		case LoadedTextureFormat.Rgba32:
			return 28u;
		case LoadedTextureFormat.Rgba64:
			return 11u;
		case LoadedTextureFormat.Alpha8:
		case LoadedTextureFormat.R8:
			return 61u;
		default:
			throw new NotSupportedException($"No DXGI format for {format}");
		}
	}

	private static LoadedTextureFormat FromDxgiFormat(uint dxgi)
	{
		return dxgi switch
		{
			98u => LoadedTextureFormat.Bc7, 
			71u => LoadedTextureFormat.Dxt1, 
			77u => LoadedTextureFormat.Dxt5, 
			80u => LoadedTextureFormat.Bc4, 
			83u => LoadedTextureFormat.Bc5, 
			28u => LoadedTextureFormat.Rgba32, 
			11u => LoadedTextureFormat.Rgba64, 
			61u => LoadedTextureFormat.R8, 
			_ => (LoadedTextureFormat)0, 
		};
	}
}
