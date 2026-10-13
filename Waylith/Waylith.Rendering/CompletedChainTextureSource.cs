using System;

namespace Waylith.Rendering;

internal sealed class CompletedChainTextureSource(ITextureCacheSource stored, int mipCount, bool premultiplied, bool borderMipMaps) : ITextureCacheSource, IDisposable
{
	private byte[][]? chain;

	public int Width => stored.Width;

	public int Height => stored.Height;

	public int MipCount => mipCount;

	public LoadedTextureFormat Format => stored.Format;

	public bool Opaque => stored.Opaque;

	public TextureSwizzle Swizzle => stored.Swizzle;

	public int LevelLength(int level)
	{
		if (level >= stored.MipCount)
		{
			return DdsFile.LevelLength(System.Math.Max(1, Width >> level), System.Math.Max(1, Height >> level), Format);
		}
		return stored.LevelLength(level);
	}

	public void ReadLevel(int level, Span<byte> destination)
	{
		if (level < stored.MipCount)
		{
			stored.ReadLevel(level, destination);
			return;
		}
		byte[] array = Chain()[level];
		array.AsSpan(0, System.Math.Min(array.Length, destination.Length)).CopyTo(destination);
	}

	public bool TryReadAll(out LoadedTextureData loadedTexture)
	{
		int width = Width;
		int height = Height;
		int mipCount = MipCount;
		LoadedTextureFormat format = Format;
		byte[][] levels = Chain();
		bool opaque = Opaque;
		TextureSwizzle swizzle = Swizzle;
		loadedTexture = new LoadedTextureData(width, height, mipCount, format, levels, opaque, null, swizzle);
		return true;
	}

	private byte[][] Chain()
	{
		if (chain != null)
		{
			return chain;
		}
		byte[] array = new byte[stored.LevelLength(0)];
		stored.ReadLevel(0, array);
		byte[][] array2 = TexturePixelData.GenerateRgba32MipChain((Format == LoadedTextureFormat.Rgba32) ? array : TexturePixelData.DecodeToRgba32(array, Width, Height, Format, default(TextureSwizzle)), Width, Height, mipCount, parallel: false, null, premultiplied);
		if (borderMipMaps)
		{
			TexturePixelData.ApplyBorderMipMaps(array2, Width, Height);
		}
		if (Format == LoadedTextureFormat.Rgba32 || mipCount == 1)
		{
			return chain = array2;
		}
		byte[][] array3 = CpuBlockEncoder.EncodeLevels(array2[1..], System.Math.Max(1, Width >> 1), System.Math.Max(1, Height >> 1), mipCount - 1, GpuTextureService.ToTarget(Format), parallel: false);
		byte[] array4 = array;
		byte[][] array5 = array3;
		int num = 0;
		byte[][] array6 = new byte[1 + array5.Length][];
		array6[num] = array4;
		num++;
		ReadOnlySpan<byte[]> readOnlySpan = new ReadOnlySpan<byte[]>(array5);
		readOnlySpan.CopyTo(new Span<byte[]>(array6).Slice(num, readOnlySpan.Length));
		num += readOnlySpan.Length;
		return chain = array6;
	}

	public void Dispose()
	{
		stored.Dispose();
	}
}
