using System;
using System.Runtime.InteropServices;
using System.Threading;
using Brutal.VulkanApi;
using Waylith.Math;

namespace Waylith.Rendering;

public sealed class TexturePixelData
{
	private static long _currentCpuBytes;

	private ulong _trackedBytes;

	public static ulong CurrentCpuBytes => (ulong)System.Math.Max(0L, Interlocked.Read(in _currentCpuBytes));

	public int Width { get; private set; }

	public int Height { get; private set; }

	public int Depth { get; private set; }

	public int MipCount { get; set; }

	public LoadedTextureFormat Format { get; private set; }

	public bool Linear { get; private set; }

	public bool AlphaIsPremultiplied { get; set; }

	public TextureSwizzle Swizzle { get; set; }

	public byte[][] Levels { get; private set; } = Array.Empty<byte[]>();

	public string Name { get; set; } = string.Empty;

	public bool HasData => Levels.Length != 0;

	public bool Dirty { get; private set; } = true;

	private bool HasRgba32Level0
	{
		get
		{
			if (Format == LoadedTextureFormat.Rgba32 || IsDecodable(Format))
			{
				byte[][] levels = Levels;
				if (levels != null && levels.Length > 0)
				{
					return Levels[0] != null;
				}
			}
			return false;
		}
	}

	public static MipFilter Rgba32MipChainFilter { get; set; }

	public TexturePixelData(int width, int height, LoadedTextureFormat format, bool linear, int mipCount, bool allocate, int depth = 1)
	{
		Width = System.Math.Max(1, width);
		Height = System.Math.Max(1, height);
		Depth = System.Math.Max(1, depth);
		Format = format;
		Linear = linear;
		MipCount = System.Math.Max(1, mipCount);
		if (allocate)
		{
			Levels = AllocateMipStorage(MipCount);
		}
	}

	public void MarkDirty()
	{
		Dirty = true;
	}

	internal void MarkClean()
	{
		Dirty = false;
	}

	public static bool IsOpaqueRgba32(byte[] rgba)
	{
		for (int i = 3; i < rgba.Length; i += 4)
		{
			if (rgba[i] != byte.MaxValue)
			{
				return false;
			}
		}
		return true;
	}

	public void Replace(int width, int height, LoadedTextureFormat format, byte[][] levels, int mipCount, bool linear)
	{
		Width = System.Math.Max(1, width);
		Height = System.Math.Max(1, height);
		Format = format;
		Linear = linear;
		MipCount = System.Math.Max(1, mipCount);
		Levels = ((levels != null && levels.Length > 0) ? levels : AllocateMipStorage(MipCount));
		Dirty = true;
	}

	public void ReplaceWithoutLevels(int width, int height, LoadedTextureFormat format, int mipCount, bool linear)
	{
		Width = System.Math.Max(1, width);
		Height = System.Math.Max(1, height);
		Format = format;
		Linear = linear;
		MipCount = System.Math.Max(1, mipCount);
		Levels = Array.Empty<byte[]>();
		Dirty = true;
	}

	public void ReplaceLevels(LoadedTextureFormat format, byte[][] levels, bool linear)
	{
		Format = format;
		Linear = linear;
		Levels = levels;
		MipCount = System.Math.Max(1, levels.Length);
		Dirty = true;
	}

	public void Release()
	{
		Levels = Array.Empty<byte[]>();
	}

	public LoadedTextureData ToLoadedTextureData()
	{
		int width = Width;
		int height = Height;
		int mipCount = MipCount;
		LoadedTextureFormat format = Format;
		byte[][] levels = Levels;
		TextureSwizzle swizzle = Swizzle;
		return new LoadedTextureData(width, height, mipCount, format, levels, Opaque: false, null, swizzle);
	}

	public TextureLevelUpload[] BuildLevelUploads()
	{
		TextureLevelUpload[] array = new TextureLevelUpload[Levels.Length];
		for (int i = 0; i < Levels.Length; i++)
		{
			array[i] = BuildLevelUpload(i, Width, Height, Depth, Format, Levels[i], 0);
		}
		return array;
	}

	public static TextureLevelUpload BuildLevelUpload(int level, int width, int height, int depth, LoadedTextureFormat format, byte[]? bytes, int byteCount)
	{
		int num = System.Math.Max(1, width >> level);
		int height2 = System.Math.Max(1, height >> level);
		int depth2 = System.Math.Max(1, depth >> level);
		int rowLengthTexels = ((!IsBlockCompressed(format)) ? num : 0);
		return new TextureLevelUpload(level, num, height2, depth2, rowLengthTexels, bytes, byteCount);
	}

	public Color GetPixel(int x, int y)
	{
		ValidateCoordinates(x, y);
		EnsureReadable();
		byte[] array = Levels[0];
		if (Format == LoadedTextureFormat.RFloat)
		{
			int startIndex = (y * Width + x) * 4;
			return new Color(BitConverter.ToSingle(array, startIndex), 0f, 0f, 1f);
		}
		if (Format == LoadedTextureFormat.RGFloat)
		{
			int num = (y * Width + x) * 8;
			return new Color(BitConverter.ToSingle(array, num), BitConverter.ToSingle(array, num + 4), 0f, 1f);
		}
		LoadedTextureFormat format = Format;
		if ((format == LoadedTextureFormat.Alpha8 || format == LoadedTextureFormat.R8) ? true : false)
		{
			int num2 = y * Width + x;
			return new Color((float)(int)array[num2] / 255f, 0f, 0f, 1f);
		}
		if (IsDecodable(Format))
		{
			Span<byte> span = stackalloc byte[64];
			DecodeBlock(array, Width, Format, x / 4, y / 4, span);
			Swizzle.ApplyInPlace(span);
			int num3 = ((y & 3) * 4 + (x & 3)) * 4;
			return new Color32(span[num3], span[num3 + 1], span[num3 + 2], span[num3 + 3]);
		}
		int num4 = (y * Width + x) * 4;
		return new Color32(array[num4], array[num4 + 1], array[num4 + 2], array[num4 + 3]);
	}

	public Color[] GetPixels()
	{
		EnsureReadable();
		Color[] array = new Color[Width * Height];
		byte[] array2 = Rgba32Level0();
		for (int i = 0; i < array.Length; i++)
		{
			int num = i * 4;
			array[i] = new Color32(array2[num], array2[num + 1], array2[num + 2], array2[num + 3]);
		}
		return array;
	}

	public Color32[] GetPixels32()
	{
		EnsureReadable();
		Color32[] array = new Color32[Width * Height];
		byte[] array2 = Rgba32Level0();
		for (int i = 0; i < array.Length; i++)
		{
			int num = i * 4;
			array[i] = new Color32(array2[num], array2[num + 1], array2[num + 2], array2[num + 3]);
		}
		return array;
	}

	internal static bool IsDecodable(LoadedTextureFormat format)
	{
		if (format == LoadedTextureFormat.Dxt1 || (uint)(format - 25) <= 2u)
		{
			return true;
		}
		return false;
	}

	private byte[] Rgba32Level0()
	{
		if (!IsDecodable(Format))
		{
			return Levels[0];
		}
		return DecodeToRgba32(Levels[0], Width, Height, Format, Swizzle);
	}

	internal static byte[] DecodeToRgba32(byte[] blocks, int width, int height, LoadedTextureFormat format, TextureSwizzle swizzle)
	{
		byte[] array = new byte[width * height * 4];
		Span<byte> texels = stackalloc byte[64];
		for (int i = 0; i < (height + 3) / 4; i++)
		{
			for (int j = 0; j < (width + 3) / 4; j++)
			{
				DecodeBlock(blocks, width, format, j, i, texels);
				for (int k = 0; k < 4 && i * 4 + k < height; k++)
				{
					texels.Slice(k * 16, System.Math.Min(4, width - j * 4) * 4).CopyTo(array.AsSpan(((i * 4 + k) * width + j * 4) * 4));
				}
			}
		}
		swizzle.ApplyInPlace(array);
		return array;
	}

	private static void DecodeBlock(byte[] blocks, int width, LoadedTextureFormat format, int bx, int by, Span<byte> texels)
	{
		bool flag = ((format == LoadedTextureFormat.Bc7 || format == LoadedTextureFormat.Bc5) ? true : false);
		int num = (flag ? 16 : 8);
		Span<byte> span = blocks.AsSpan((by * ((width + 3) / 4) + bx) * num, num);
		switch (format)
		{
		case LoadedTextureFormat.Bc7:
			Bc7BlockEncoder.DecodeBc7Block(span, texels);
			break;
		case LoadedTextureFormat.Bc4:
			Bc7BlockEncoder.DecodeBc4Block(span, texels);
			break;
		case LoadedTextureFormat.Bc5:
			Bc7BlockEncoder.DecodeBc5Block(span, texels);
			break;
		default:
			Bc7BlockEncoder.DecodeBc1Block(span, texels);
			break;
		}
	}

	public void SetPixel(int x, int y, Color color)
	{
		ValidateCoordinates(x, y);
		EnsureWritableMipZero();
		WritePixel(x, y, color);
	}

	public void SetPixels(Color[] colors)
	{
		ArgumentNullException.ThrowIfNull(colors, "colors");
		if (colors.Length != Width * Height * Depth)
		{
			throw new ArgumentException("Pixel array size does not match texture dimensions.", "colors");
		}
		EnsureWritableMipZero();
		for (int i = 0; i < colors.Length; i++)
		{
			WritePixelLinear(i, colors[i]);
		}
	}

	public void SetPixels32(Color32[] colors)
	{
		ArgumentNullException.ThrowIfNull(colors, "colors");
		if (colors.Length != Width * Height * Depth)
		{
			throw new ArgumentException("Pixel array size does not match texture dimensions.", "colors");
		}
		EnsureWritableMipZero();
		MemoryMarshal.Cast<Color32, byte>((ReadOnlySpan<Color32>)colors).CopyTo(Levels[0]);
	}

	public byte[]? GetRgba32OrNull()
	{
		if (!HasRgba32Level0)
		{
			return null;
		}
		if (Format != LoadedTextureFormat.Rgba32)
		{
			return Rgba32Level0();
		}
		return (byte[])Levels[0].Clone();
	}

	public byte[]? GetPremultipliedRgba32OrNull()
	{
		if (!HasRgba32Level0)
		{
			return null;
		}
		byte[] array = Rgba32Level0();
		byte[] array2 = new byte[array.Length];
		for (int i = 0; i + 3 < array.Length; i += 4)
		{
			int num = array[i + 3];
			array2[i] = (byte)((array[i] * num + 127) / 255);
			array2[i + 1] = (byte)((array[i + 1] * num + 127) / 255);
			array2[i + 2] = (byte)((array[i + 2] * num + 127) / 255);
			array2[i + 3] = (byte)num;
		}
		return array2;
	}

	public void WritePixel(int x, int y, Color color)
	{
		WritePixelLinear(y * Width + x, color);
	}

	private void WritePixelLinear(int pixelIndex, Color color)
	{
		if (Format == LoadedTextureFormat.RFloat)
		{
			int dstOffset = pixelIndex * 4;
			Buffer.BlockCopy(BitConverter.GetBytes(color.r), 0, Levels[0], dstOffset, 4);
			return;
		}
		if (Format == LoadedTextureFormat.RGFloat)
		{
			int num = pixelIndex * 8;
			Buffer.BlockCopy(BitConverter.GetBytes(color.r), 0, Levels[0], num, 4);
			Buffer.BlockCopy(BitConverter.GetBytes(color.g), 0, Levels[0], num + 4, 4);
			return;
		}
		LoadedTextureFormat format = Format;
		if ((format == LoadedTextureFormat.Alpha8 || format == LoadedTextureFormat.R8) ? true : false)
		{
			Levels[0][pixelIndex] = (byte)System.Math.Clamp((int)MathF.Round(color.r * 255f), 0, 255);
			return;
		}
		Color32 color2 = color;
		int num2 = pixelIndex * 4;
		Levels[0][num2] = color2.r;
		Levels[0][num2 + 1] = color2.g;
		Levels[0][num2 + 2] = color2.b;
		Levels[0][num2 + 3] = color2.a;
	}

	public void EnsureWritableMipZero()
	{
		Dirty = true;
		if (Levels.Length == 0)
		{
			Levels = AllocateMipStorage(System.Math.Max(1, MipCount));
		}
		int levelSizeBytes = GetLevelSizeBytes(0);
		if (Levels[0].Length != levelSizeBytes)
		{
			Levels[0] = new byte[levelSizeBytes];
		}
	}

	public void EnsureWritableMipLevel(int level)
	{
		Dirty = true;
		if (Levels.Length == 0)
		{
			Levels = AllocateMipStorage(System.Math.Max(1, MipCount));
		}
		else if (Levels.Length < System.Math.Max(1, MipCount))
		{
			byte[][] array = Levels;
			Array.Resize(ref array, System.Math.Max(1, MipCount));
			Levels = array;
		}
		int levelSizeBytes = GetLevelSizeBytes(level);
		if (Levels[level] == null || Levels[level].Length != levelSizeBytes)
		{
			Levels[level] = new byte[levelSizeBytes];
		}
	}

	public void GenerateMipmaps()
	{
		if (IsBlockCompressed(Format))
		{
			return;
		}
		EnsureWritableMipZero();
		if (MipCount > 1)
		{
			if (Levels.Length < MipCount)
			{
				byte[][] array = Levels;
				Array.Resize(ref array, MipCount);
				Levels = array;
			}
			for (int i = 1; i < MipCount; i++)
			{
				EnsureWritableMipLevel(i);
				GenerateMipLevel(i - 1, i);
			}
		}
	}

	private void GenerateMipLevel(int sourceLevel, int destinationLevel)
	{
		int sourceWidth = System.Math.Max(1, Width >> sourceLevel);
		int sourceHeight = System.Math.Max(1, Height >> sourceLevel);
		int destinationWidth = System.Math.Max(1, Width >> destinationLevel);
		int destinationHeight = System.Math.Max(1, Height >> destinationLevel);
		DownsampleRgba32(Levels[sourceLevel], sourceWidth, sourceHeight, Levels[destinationLevel], destinationWidth, destinationHeight, AlphaIsPremultiplied);
	}

	public static byte[][] GenerateRgba32MipChain(byte[] mip0, int width, int height, int mipCount, bool parallel = false, Func<int, byte[]>? allocate = null, bool premultiplied = false)
	{
		return GenerateRgba32MipChain(mip0, width, height, mipCount, Rgba32MipChainFilter, parallel, allocate, premultiplied);
	}

	public static byte[][] GenerateRgba32MipChain(byte[] mip0, int width, int height, int mipCount, MipFilter filter, bool parallel = false, Func<int, byte[]>? allocate = null, bool premultiplied = false)
	{
		byte[][] array = new byte[mipCount][];
		array[0] = mip0;
		for (int i = 1; i < mipCount; i++)
		{
			int sourceWidth = System.Math.Max(1, width >> i - 1);
			int sourceHeight = System.Math.Max(1, height >> i - 1);
			int num = System.Math.Max(1, width >> i);
			int num2 = System.Math.Max(1, height >> i);
			int num3 = num * num2 * 4;
			array[i] = allocate?.Invoke(num3) ?? new byte[num3];
			switch (filter)
			{
			case MipFilter.Kaiser:
				KaiserMipFilter.Downsample(array[i - 1], sourceWidth, sourceHeight, array[i], num, num2, parallel: true, premultiplied);
				break;
			case MipFilter.Box:
				CpuBlockEncoder.Downsample(array[i - 1], sourceWidth, sourceHeight, array[i], num, num2, parallel, premultiplied);
				break;
			}
		}
		return array;
	}

	public static void ApplyBorderMipMaps(byte[][] levels, int width, int height)
	{
		for (int i = 1; i < levels.Length; i++)
		{
			byte[] source = levels[i - 1];
			byte[] destination = levels[i];
			int num = System.Math.Max(1, width >> i - 1);
			int num2 = System.Math.Max(1, height >> i - 1);
			int num3 = System.Math.Max(1, width >> i);
			int num4 = System.Math.Max(1, height >> i);
			for (int j = 0; j < num3; j++)
			{
				AverageEdgePair(source, num, j * 2, 0, System.Math.Min(j * 2 + 1, num - 1), 0, destination, num3, j, 0);
				AverageEdgePair(source, num, j * 2, num2 - 1, System.Math.Min(j * 2 + 1, num - 1), num2 - 1, destination, num3, j, num4 - 1);
			}
			for (int k = 0; k < num4; k++)
			{
				AverageEdgePair(source, num, 0, k * 2, 0, System.Math.Min(k * 2 + 1, num2 - 1), destination, num3, 0, k);
				AverageEdgePair(source, num, num - 1, k * 2, num - 1, System.Math.Min(k * 2 + 1, num2 - 1), destination, num3, num3 - 1, k);
			}
		}
	}

	private static void AverageEdgePair(byte[] source, int sourceWidth, int ax, int ay, int bx, int by, byte[] destination, int destinationWidth, int dx, int dy)
	{
		int num = (ay * sourceWidth + ax) * 4;
		int num2 = (by * sourceWidth + bx) * 4;
		int num3 = (dy * destinationWidth + dx) * 4;
		for (int i = 0; i < 4; i++)
		{
			destination[num3 + i] = (byte)(source[num + i] + source[num2 + i] + 1 >> 1);
		}
	}

	private static void DownsampleRgba32(byte[] source, int sourceWidth, int sourceHeight, byte[] destination, int destinationWidth, int destinationHeight, bool premultiplied)
	{
		for (int i = 0; i < destinationHeight; i++)
		{
			for (int j = 0; j < destinationWidth; j++)
			{
				int num = j * 2;
				int num2 = i * 2;
				float num3 = 0f;
				float num4 = 0f;
				float num5 = 0f;
				int num6 = 0;
				int num7 = 0;
				int num8 = 0;
				int num9 = 0;
				for (int k = 0; k < 2; k++)
				{
					for (int l = 0; l < 2; l++)
					{
						int num10 = System.Math.Min(sourceWidth - 1, num + l);
						int num11 = (System.Math.Min(sourceHeight - 1, num2 + k) * sourceWidth + num10) * 4;
						int num12 = source[num11];
						int num13 = source[num11 + 1];
						int num14 = source[num11 + 2];
						int num15 = source[num11 + 3];
						num3 += (float)(num12 * num15);
						num4 += (float)(num13 * num15);
						num5 += (float)(num14 * num15);
						num6 += num12;
						num7 += num13;
						num8 += num14;
						num9 += num15;
					}
				}
				int num16 = (i * destinationWidth + j) * 4;
				if (num9 > 0 && !premultiplied)
				{
					destination[num16] = (byte)(num3 / (float)num9 + 0.5f);
					destination[num16 + 1] = (byte)(num4 / (float)num9 + 0.5f);
					destination[num16 + 2] = (byte)(num5 / (float)num9 + 0.5f);
				}
				else
				{
					destination[num16] = (byte)((num6 + 2) / 4);
					destination[num16 + 1] = (byte)((num7 + 2) / 4);
					destination[num16 + 2] = (byte)((num8 + 2) / 4);
				}
				destination[num16 + 3] = (byte)((num9 + 2) / 4);
			}
		}
	}

	private void EnsureReadable()
	{
		if (Levels.Length == 0)
		{
			throw new InvalidOperationException("Texture '" + Name + "' is not readable.");
		}
	}

	private void ValidateCoordinates(int x, int y)
	{
		if ((uint)x >= Width || (uint)y >= Height)
		{
			throw new ArgumentOutOfRangeException($"Pixel coordinates ({x}, {y}) are outside texture bounds {Width}x{Height}.");
		}
	}

	public byte[][] AllocateMipStorage(int mipCount)
	{
		byte[][] array = new byte[System.Math.Max(1, mipCount)][];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new byte[GetLevelSizeBytes(i)];
		}
		return array;
	}

	public int GetLevelSizeBytes(int level)
	{
		int num = System.Math.Max(1, Width >> level);
		int num2 = System.Math.Max(1, Height >> level);
		int num3 = System.Math.Max(1, Depth >> level);
		int num4 = BlockBytes(Format);
		if (num4 != 0)
		{
			return (num + 3) / 4 * ((num2 + 3) / 4) * num3 * num4;
		}
		return num * num2 * num3 * BytesPerPixel(Format);
	}

	public void TrackCurrentBytes()
	{
		ulong num = 0uL;
		byte[][] levels = Levels;
		foreach (byte[] array in levels)
		{
			num += (ulong)((array != null) ? array.Length : 0);
		}
		SetTrackedBytes(num);
	}

	public void ReleaseTracked()
	{
		SetTrackedBytes(0uL);
	}

	private void SetTrackedBytes(ulong bytes)
	{
		if (_trackedBytes != bytes)
		{
			long value = (long)(bytes - _trackedBytes);
			_trackedBytes = bytes;
			Interlocked.Add(ref _currentCpuBytes, value);
		}
	}

	public static int BlockBytes(LoadedTextureFormat format)
	{
		return format switch
		{
			LoadedTextureFormat.Dxt1 => 8, 
			LoadedTextureFormat.Bc4 => 8, 
			LoadedTextureFormat.Dxt5 => 16, 
			LoadedTextureFormat.Bc5 => 16, 
			LoadedTextureFormat.Bc7 => 16, 
			_ => 0, 
		};
	}

	public static bool IsBlockCompressed(LoadedTextureFormat format)
	{
		return BlockBytes(format) != 0;
	}

	public static int BytesPerPixel(LoadedTextureFormat format)
	{
		switch (format)
		{
		case LoadedTextureFormat.Alpha8:
		case LoadedTextureFormat.R8:
			return 1;
		case LoadedTextureFormat.Rgb24:
			return 3;
		case LoadedTextureFormat.RFloat:
			return 4;
		case LoadedTextureFormat.RGFloat:
			return 8;
		case LoadedTextureFormat.Rgba64:
			return 8;
		case LoadedTextureFormat.RgbaFloat:
			return 16;
		default:
			return 4;
		}
	}

	public static VkFormat ToVkFormat(LoadedTextureFormat format)
	{
		switch (format)
		{
		case LoadedTextureFormat.Alpha8:
		case LoadedTextureFormat.R8:
			return VkFormat.R8UNorm;
		case LoadedTextureFormat.Rgb24:
			return VkFormat.R8G8B8UNorm;
		case LoadedTextureFormat.RFloat:
			return VkFormat.R32SFloat;
		case LoadedTextureFormat.RGFloat:
			return VkFormat.R32G32SFloat;
		case LoadedTextureFormat.RgbaFloat:
			return VkFormat.R32G32B32A32SFloat;
		case LoadedTextureFormat.Rgba64:
			return VkFormat.R16G16B16A16UNorm;
		case LoadedTextureFormat.Bc7:
			return VkFormat.Bc7UNormBlock;
		case LoadedTextureFormat.Bc5:
			return VkFormat.Bc5UNormBlock;
		case LoadedTextureFormat.Bc4:
			return VkFormat.Bc4UNormBlock;
		case LoadedTextureFormat.Dxt1:
			return VkFormat.Bc1RGBAUNormBlock;
		case LoadedTextureFormat.Dxt5:
			return VkFormat.Bc3UNormBlock;
		default:
			return VkFormat.R8G8B8A8UNorm;
		}
	}

	public static int CalculateMipCount(int width, int height, int depth = 1)
	{
		int num = 1;
		int num2 = System.Math.Max(System.Math.Max(1, width), System.Math.Max(System.Math.Max(1, height), System.Math.Max(1, depth)));
		while (num2 > 1)
		{
			num2 >>= 1;
			num++;
		}
		return num;
	}
}
