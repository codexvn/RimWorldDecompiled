using System;

namespace Waylith.Rendering;

public static class TextureEncodingRules
{
	public const double MinBc1ChannelPsnr = 36.0;

	public static TextureEncoding Refine(BcEncodeTarget target, byte[]? rgba, int width, int height, bool opaque, string? sourcePath)
	{
		return Refine(target, rgba, width, height, opaque, TextureUsageHints.For(sourcePath));
	}

	public static TextureEncoding Refine(BcEncodeTarget target, byte[]? rgba, int width, int height, bool opaque, TextureUsage usage)
	{
		if (target == BcEncodeTarget.None)
		{
			return default(TextureEncoding);
		}
		bool flag;
		switch (usage)
		{
		case TextureUsage.NormalXY:
			return new TextureEncoding(BcEncodeTarget.Bc5);
		case TextureUsage.ArrayLayer:
		case TextureUsage.AtlasTile:
			flag = true;
			break;
		default:
			flag = false;
			break;
		}
		if (flag || rgba == null || rgba.Length < width * height * 4)
		{
			return new TextureEncoding(target);
		}
		if (!opaque && usage != TextureUsage.RgbOnly)
		{
			return new TextureEncoding(target);
		}
		if (TrySingleChannel(rgba.AsSpan(0, width * height * 4), out var swizzle))
		{
			return new TextureEncoding(BcEncodeTarget.Bc4, swizzle);
		}
		if (target != BcEncodeTarget.Bc1)
		{
			return new TextureEncoding(target);
		}
		return new TextureEncoding((WorstChannelBc1Psnr(rgba, width, height) >= 36.0) ? BcEncodeTarget.Bc1 : BcEncodeTarget.Bc7);
	}

	public static bool TrySingleChannel(ReadOnlySpan<byte> rgba, out TextureSwizzle swizzle)
	{
		swizzle = default(TextureSwizzle);
		if (rgba.Length < 4)
		{
			return false;
		}
		bool flag = true;
		bool flag2 = true;
		bool flag3 = true;
		bool flag4 = true;
		byte b = rgba[1];
		byte b2 = rgba[2];
		for (int i = 0; i < rgba.Length; i += 4)
		{
			byte b3 = rgba[i];
			byte b4 = rgba[i + 1];
			byte b5 = rgba[i + 2];
			flag = flag && b4 == b3;
			flag2 = flag2 && b5 == b3;
			flag3 = flag3 && b4 == b;
			flag4 = flag4 && b5 == b2;
			if (!(flag || flag3) || !(flag2 || flag4))
			{
				return false;
			}
		}
		TextureChannel? textureChannel = ChannelFrom(flag, flag3, b);
		if (textureChannel.HasValue)
		{
			TextureChannel valueOrDefault = textureChannel.GetValueOrDefault();
			textureChannel = ChannelFrom(flag2, flag4, b2);
			if (textureChannel.HasValue)
			{
				TextureChannel valueOrDefault2 = textureChannel.GetValueOrDefault();
				swizzle = ((valueOrDefault == TextureChannel.Zero && valueOrDefault2 == TextureChannel.Zero) ? default(TextureSwizzle) : new TextureSwizzle(TextureChannel.Red, valueOrDefault, valueOrDefault2, TextureChannel.One));
				return true;
			}
		}
		return false;
	}

	private static TextureChannel? ChannelFrom(bool isRed, bool isFixed, byte value)
	{
		if (!isRed)
		{
			if (!isFixed || value != 0)
			{
				if (!isFixed || value != byte.MaxValue)
				{
					return null;
				}
				return TextureChannel.One;
			}
			return TextureChannel.Zero;
		}
		return TextureChannel.Red;
	}

	public static double WorstChannelBc1Psnr(byte[] rgba, int width, int height)
	{
		byte[] array = CpuBlockEncoder.EncodeLevels(new byte[1][] { rgba }, width, height, 1, BcEncodeTarget.Bc1, parallel: false)[0];
		Span<byte> rgba2 = stackalloc byte[64];
		Span<long> span = stackalloc long[3];
		int num = (width + 3) / 4;
		for (int i = 0; i < (height + 3) / 4; i++)
		{
			for (int j = 0; j < num; j++)
			{
				Bc7BlockEncoder.DecodeBc1Block(array.AsSpan((i * num + j) * 8, 8), rgba2);
				for (int k = 0; k < 16; k++)
				{
					int num2 = j * 4 + (k & 3);
					int num3 = i * 4 + (k >> 2);
					if (num2 < width && num3 < height)
					{
						int num4 = (num3 * width + num2) * 4;
						for (int l = 0; l < 3; l++)
						{
							int num5 = rgba[num4 + l] - rgba2[k * 4 + l];
							span[l] += num5 * num5;
						}
					}
				}
			}
		}
		long num6 = System.Math.Max(span[0], System.Math.Max(span[1], span[2]));
		if (num6 != 0L)
		{
			return 10.0 * System.Math.Log10(65025.0 * (double)width * (double)height / (double)num6);
		}
		return double.PositiveInfinity;
	}
}
