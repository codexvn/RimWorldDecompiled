using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Brutal.VulkanApi;
using Waylith.Jobs;
using Waylith.Profiling;

namespace Waylith.Rendering;

public static class CpuBlockEncoder
{
	private delegate void StripEncoder(byte[] rgba, int width, int height, int blocksWide, int firstBlockRow, int blockRowCount, byte[] output);

	private readonly record struct BlockStrip(int level, int levelWidth, int levelHeight, int blocksWide, int firstBlockRow, int blockRowCount);

	private const int BlockRowsPerStrip = 8;

	private const int RdoBlocksPerStrip = 4096;

	private const int DownsampleRowsPerStrip = 32;

	[CompilerGenerated]
	private static Bc7PackFlags _003CBc7Flags_003Ek__BackingField = Bc7PackFlags.WaylithDefault;

	private static readonly ConcurrentBag<byte[][]> LevelBuffers = new ConcurrentBag<byte[][]>();

	public static Bc7PackFlags Bc7Flags
	{
		[CompilerGenerated]
		get
		{
			return _003CBc7Flags_003Ek__BackingField;
		}
		set
		{
			Bc7BlockEncoder.ValidateFlags(value);
			_003CBc7Flags_003Ek__BackingField = value;
		}
	}

	public static byte[][]? TryEncodeBc7FromImage(GpuTextureAllocation? source, int width, int height, int mipCount, bool parallel = true)
	{
		if (source == null || source.Format != VkFormat.R8G8B8A8UNorm || width <= 0 || height <= 0 || mipCount <= 0)
		{
			return null;
		}
		mipCount = System.Math.Min(mipCount, source.MipCount);
		byte[][] array = RentLevels(mipCount);
		try
		{
			byte[][] array2;
			using (FrameProfiler.Scope("CpuBlockReadback"))
			{
				array2 = GpuTextureService.ReadbackTexture2DLevels(source, width, height, mipCount, 4, array);
			}
			if (array2 == null)
			{
				return null;
			}
			return EncodeBc7Levels(array2, width, height, mipCount, parallel);
		}
		finally
		{
			LevelBuffers.Add(array);
		}
	}

	public static byte[][]? TryEncodeFromPixels(byte[]? mip0, int width, int height, int mipCount, BcEncodeTarget target, bool parallel = true, bool premultiplied = false)
	{
		if (mip0 == null || width <= 0 || height <= 0 || mipCount <= 0 || mip0.Length < (long)width * (long)height * 4)
		{
			return null;
		}
		byte[][] array = RentLevels(mipCount);
		try
		{
			return EncodeLevels(BuildChain(mip0, array, width, height, mipCount, parallel, premultiplied), width, height, mipCount, target, parallel);
		}
		finally
		{
			LevelBuffers.Add(array);
		}
	}

	public static byte[][] EncodeLevels(byte[][] rgbaLevels, int width, int height, int mipCount, BcEncodeTarget target, bool parallel = true)
	{
		return EncodeLevels(rgbaLevels, width, height, mipCount, target, Bc7Flags, parallel);
	}

	public static byte[][] EncodeLevels(byte[][] rgbaLevels, int width, int height, int mipCount, BcEncodeTarget target, Bc7PackFlags flags, bool parallel)
	{
		using (FrameProfiler.Scope(target switch
		{
			BcEncodeTarget.Bc1 => "CpuBc1Encode", 
			BcEncodeTarget.Bc4 => "CpuBc4Encode", 
			BcEncodeTarget.Bc5 => "CpuBc5Encode", 
			BcEncodeTarget.Bc7 => "CpuBc7Encode", 
			_ => throw new ArgumentOutOfRangeException("target"), 
		}))
		{
			return target switch
			{
				BcEncodeTarget.Bc1 => EncodeLevels(rgbaLevels, width, height, mipCount, target, parallel, EncodeBc1Strip), 
				BcEncodeTarget.Bc4 => EncodeLevels(rgbaLevels, width, height, mipCount, target, parallel, delegate(byte[] rgba, int w, int h, int blocksWide, int first, int count, byte[] output)
				{
					EncodeBc4Strip(rgba, w, h, blocksWide, first, count, output);
				}), 
				BcEncodeTarget.Bc5 => EncodeLevels(rgbaLevels, width, height, mipCount, target, parallel, delegate(byte[] rgba, int w, int h, int blocksWide, int first, int count, byte[] output)
				{
					EncodeBc5Strip(rgba, w, h, blocksWide, first, count, output);
				}), 
				_ => EncodeLevels(rgbaLevels, width, height, mipCount, target, parallel, delegate(byte[] rgba, int w, int h, int blocksWide, int first, int count, byte[] output)
				{
					EncodeBc7Strip(rgba, w, h, blocksWide, first, count, flags, output);
				}), 
			};
		}
	}

	public static byte[][] EncodeBc7Levels(byte[][] rgbaLevels, int width, int height, int mipCount, bool parallel = true)
	{
		return EncodeLevels(rgbaLevels, width, height, mipCount, BcEncodeTarget.Bc7, parallel);
	}

	public static byte[][] EncodeBc1Levels(byte[][] rgbaLevels, int width, int height, int mipCount, bool parallel = true)
	{
		return EncodeLevels(rgbaLevels, width, height, mipCount, BcEncodeTarget.Bc1, parallel);
	}

	public static void Downsample(byte[] source, int sourceWidth, int sourceHeight, byte[] destination, int destinationWidth, int destinationHeight, bool parallel = false, bool premultiplied = false)
	{
		int num = (destinationHeight + 32 - 1) / 32;
		if (!parallel || num <= 1)
		{
			CpuMipDownsample.DownsampleRows(source, sourceWidth, sourceHeight, destination, destinationWidth, destinationHeight, 0, destinationHeight, premultiplied);
			return;
		}
		WaylithJobRuntime.For(0, num, delegate(int strip)
		{
			int num2 = strip * 32;
			CpuMipDownsample.DownsampleRows(source, sourceWidth, sourceHeight, destination, destinationWidth, destinationHeight, num2, System.Math.Min(32, destinationHeight - num2), premultiplied);
		});
	}

	private static byte[][] RentLevels(int mipCount)
	{
		LevelBuffers.TryTake(out byte[][] result);
		if (result == null || result.Length < mipCount)
		{
			Array.Resize(ref result, mipCount);
		}
		return result;
	}

	private static byte[][] BuildChain(byte[] mip0, byte[][] scratch, int width, int height, int mipCount, bool parallel, bool premultiplied)
	{
		using (FrameProfiler.Scope("CpuMipChain"))
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
				if (scratch[i] == null || scratch[i].Length < num3)
				{
					scratch[i] = new byte[num3];
				}
				array[i] = scratch[i];
				Downsample(array[i - 1], sourceWidth, sourceHeight, array[i], num, num2, parallel, premultiplied);
			}
			return array;
		}
	}

	private static byte[][] EncodeLevels(byte[][] rgbaLevels, int width, int height, int mipCount, BcEncodeTarget target, bool parallel, StripEncoder encodeStrip)
	{
		int num = target.BlockBytes();
		byte[][] blocks = new byte[mipCount][];
		List<BlockStrip> strips = new List<BlockStrip>();
		for (int j = 0; j < mipCount; j++)
		{
			int num2 = System.Math.Max(1, width >> j);
			int num3 = System.Math.Max(1, height >> j);
			int num4 = (num2 + 3) / 4;
			int num5 = (num3 + 3) / 4;
			blocks[j] = new byte[(long)num4 * (long)num5 * num];
			for (int k = 0; k < num5; k += 8)
			{
				strips.Add(new BlockStrip(j, num2, num3, num4, k, System.Math.Min(8, num5 - k)));
			}
		}
		if (strips.Count <= 1)
		{
			foreach (BlockStrip item in strips)
			{
				Encode(item);
			}
		}
		else if (parallel)
		{
			WaylithJobRuntime.For(0, strips.Count, delegate(int i)
			{
				Encode(strips[i]);
			});
		}
		else
		{
			WaylithJobRuntime.ForWithoutStealing(0, strips.Count, delegate(int i)
			{
				Encode(strips[i]);
			});
		}
		return blocks;
		void Encode(in BlockStrip strip)
		{
			encodeStrip(rgbaLevels[strip.level], strip.levelWidth, strip.levelHeight, strip.blocksWide, strip.firstBlockRow, strip.blockRowCount, blocks[strip.level]);
		}
	}

	public static void ReduceEntropy(byte[][] blocks, byte[][] rgbaLevels, int width, int height, int levelCount, BcEncodeTarget target, float lambda, int lookbackWindow, bool parallel = true)
	{
		int blockBytes = target.BlockBytes();
		List<BlockStrip> strips = new List<BlockStrip>();
		for (int j = 0; j < levelCount; j++)
		{
			int num = System.Math.Max(1, width >> j);
			int num2 = System.Math.Max(1, height >> j);
			int num3 = (num + 3) / 4;
			int num4 = (num2 + 3) / 4;
			int num5 = System.Math.Max(1, 4096 / num3);
			for (int k = 0; k < num4; k += num5)
			{
				strips.Add(new BlockStrip(j, num, num2, num3, k, System.Math.Min(num5, num4 - k)));
			}
		}
		if (strips.Count <= 1)
		{
			foreach (BlockStrip item in strips)
			{
				Reduce(item);
			}
			return;
		}
		if (parallel)
		{
			WaylithJobRuntime.For(0, strips.Count, delegate(int i)
			{
				Reduce(strips[i]);
			});
		}
		else
		{
			WaylithJobRuntime.ForWithoutStealing(0, strips.Count, delegate(int i)
			{
				Reduce(strips[i]);
			});
		}
		void Reduce(in BlockStrip strip)
		{
			int num6 = strip.levelWidth * 4;
			BcRdo.ReduceEntropy(blocks[strip.level].AsSpan(strip.firstBlockRow * strip.blocksWide * blockBytes, strip.blockRowCount * strip.blocksWide * blockBytes), target, rgbaLevels[strip.level].AsSpan(strip.firstBlockRow * 4 * num6), strip.levelWidth, System.Math.Min(strip.blockRowCount * 4, strip.levelHeight - strip.firstBlockRow * 4), num6, lambda, lookbackWindow);
		}
	}

	private unsafe static void EncodeBc7Strip(byte[] rgba, int width, int height, int blocksWide, int firstBlockRow, int blockRowCount, Bc7PackFlags flags, byte[] output)
	{
		fixed (byte* rgba2 = rgba)
		{
			fixed (byte* ptr = output)
			{
				Bc7BlockEncoder.EncodeRows(rgba2, width, height, width * 4, firstBlockRow, blockRowCount, flags, ptr + (long)firstBlockRow * (long)blocksWide * 16);
			}
		}
	}

	private unsafe static void EncodeBc1Strip(byte[] rgba, int width, int height, int blocksWide, int firstBlockRow, int blockRowCount, byte[] output)
	{
		fixed (byte* rgba2 = rgba)
		{
			fixed (byte* ptr = output)
			{
				Bc1BlockEncoder.EncodeRows(rgba2, width, height, width * 4, firstBlockRow, blockRowCount, ptr + (long)firstBlockRow * (long)blocksWide * 8);
			}
		}
	}

	private unsafe static void EncodeBc4Strip(byte[] rgba, int width, int height, int blocksWide, int firstBlockRow, int blockRowCount, byte[] output)
	{
		fixed (byte* rgba2 = rgba)
		{
			fixed (byte* ptr = output)
			{
				Bc4BlockEncoder.EncodeRows(rgba2, width, height, width * 4, firstBlockRow, blockRowCount, ptr + (long)firstBlockRow * (long)blocksWide * 8);
			}
		}
	}

	private unsafe static void EncodeBc5Strip(byte[] rgba, int width, int height, int blocksWide, int firstBlockRow, int blockRowCount, byte[] output)
	{
		fixed (byte* rgba2 = rgba)
		{
			fixed (byte* ptr = output)
			{
				Bc5BlockEncoder.EncodeRows(rgba2, width, height, width * 4, firstBlockRow, blockRowCount, ptr + (long)firstBlockRow * (long)blocksWide * 16);
			}
		}
	}
}
