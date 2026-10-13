using System;
using Waylith.Assets;

namespace Waylith.Rendering;

public sealed class PackTextureSource : ITextureCacheSource, IDisposable
{
	private readonly byte[] dds;

	private readonly bool pooled;

	private readonly int[] offsets;

	private readonly int[] lengths;

	public int Width { get; }

	public int Height { get; }

	public int MipCount { get; }

	public LoadedTextureFormat Format { get; }

	public bool Opaque { get; }

	public bool Premultiplied { get; }

	public TextureSwizzle Swizzle { get; }

	private PackTextureSource(byte[] dds, bool pooled, in DdsInfo info, int[] offsets, int[] lengths)
	{
		this.dds = dds;
		this.pooled = pooled;
		this.offsets = offsets;
		this.lengths = lengths;
		Width = info.Width;
		Height = info.Height;
		MipCount = info.MipCount;
		Format = info.Format;
		Opaque = info.AlphaMode == DdsAlphaMode.Opaque;
		Premultiplied = info.AlphaMode == DdsAlphaMode.Premultiplied;
		Swizzle = info.Swizzle;
	}

	public static bool TryCreate(byte[] dds, out PackTextureSource source)
	{
		return TryCreate(dds, dds.Length, pooled: false, out source);
	}

	public static bool TryCreate(byte[] dds, int length, bool pooled, out PackTextureSource source)
	{
		source = null;
		if (!DdsFile.TryReadHeader(dds, out var info))
		{
			if (pooled)
			{
				AssetPack.Return(dds);
			}
			return false;
		}
		int[] array = new int[info.MipCount];
		int[] array2 = new int[info.MipCount];
		int num = 148;
		for (int i = 0; i < info.MipCount; i++)
		{
			int width = System.Math.Max(1, info.Width >> i);
			int height = System.Math.Max(1, info.Height >> i);
			int num2 = DdsFile.LevelLength(width, height, info.Format);
			if (num + num2 > length)
			{
				if (pooled)
				{
					AssetPack.Return(dds);
				}
				return false;
			}
			array[i] = num;
			array2[i] = num2;
			num += num2;
		}
		source = new PackTextureSource(dds, pooled, in info, array, array2);
		return true;
	}

	public int LevelLength(int level)
	{
		return lengths[level];
	}

	public void ReadLevel(int level, Span<byte> destination)
	{
		dds.AsSpan(offsets[level], System.Math.Min(lengths[level], destination.Length)).CopyTo(destination);
	}

	public bool TryReadAll(out LoadedTextureData loadedTexture)
	{
		byte[][] array = new byte[MipCount][];
		for (int i = 0; i < MipCount; i++)
		{
			array[i] = dds.AsSpan(offsets[i], lengths[i]).ToArray();
		}
		int width = Width;
		int height = Height;
		int mipCount = MipCount;
		LoadedTextureFormat format = Format;
		bool opaque = Opaque;
		TextureSwizzle swizzle = Swizzle;
		loadedTexture = new LoadedTextureData(width, height, mipCount, format, array, opaque, null, swizzle);
		return true;
	}

	public void Dispose()
	{
		if (pooled)
		{
			AssetPack.Return(dds);
		}
	}
}
