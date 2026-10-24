using System;
using System.Buffers.Binary;
using System.Linq;
using System.Threading.Tasks;

namespace PsdSharp.Images.DataConversion;

internal static class LabPlanarToRgbInterleavedConverter
{
	public static byte[] Convert(ImageData imageData, ColorType destinationColorType)
	{
		int pixelCount = (int)(imageData.Width * imageData.Height);
		int bytesPerSample = imageData.ChannelDepth / 8;
		int num = (int)Math.Ceiling((double)destinationColorType.Channels.Sum((ChannelInfo x) => x.PixelCount) / 8.0);
		byte[] interleavedBuffer = new byte[pixelCount * num];
		ImageData.ChannelData[] array = imageData.GetChannels().ToArray();
		byte[] lSrc = To8Bit(array[0].GetData(), bytesPerSample, pixelCount);
		byte[] aSrc = To8Bit(array[1].GetData(), bytesPerSample, pixelCount);
		byte[] bSrc = To8Bit(array[2].GetData(), bytesPerSample, pixelCount);
		Parallel.For(0L, (pixelCount + 50000 - 1) / 50000, delegate(long chunk)
		{
			long num2 = chunk * 50000;
			long num3 = Math.Min(num2 + 50000, pixelCount);
			long num4 = num2;
			long num5 = num2 * 4;
			while (num4 < num3)
			{
				float num6 = (float)(int)lSrc[num4] / 255f * 100f;
				float num7 = (float)(int)aSrc[num4] - 128f;
				float num8 = (float)(int)bSrc[num4] - 128f;
				float num9 = (num6 + 16f) / 116f;
				float num10 = num7 / 500f + num9;
				float num11 = num9 - num8 / 200f;
				num10 = ((num10 > 0.206893f) ? (num10 * num10 * num10) : ((num10 - 0.13793103f) / 7.787f));
				num9 = ((num9 > 0.206893f) ? (num9 * num9 * num9) : ((num9 - 0.13793103f) / 7.787f));
				num11 = ((num11 > 0.206893f) ? (num11 * num11 * num11) : ((num11 - 0.13793103f) / 7.787f));
				num10 *= 95.047f;
				num9 *= 100f;
				num11 *= 108.883f;
				float num12 = 3.2406f * num10 / 100f - 1.5372f * num9 / 100f - 0.4986f * num11 / 100f;
				float num13 = -0.9689f * num10 / 100f + 1.8758f * num9 / 100f + 0.0415f * num11 / 100f;
				float num14 = 0.0557f * num10 / 100f - 0.204f * num9 / 100f + 1.057f * num11 / 100f;
				num12 = ((num12 <= 0f) ? 0f : ((num12 >= 1f) ? 1f : ((num12 > 0.0031308f) ? (1.055f * MathF.Pow(num12, 5f / 12f) - 0.055f) : (12.92f * num12))));
				num13 = ((num13 <= 0f) ? 0f : ((num13 >= 1f) ? 1f : ((num13 > 0.0031308f) ? (1.055f * MathF.Pow(num13, 5f / 12f) - 0.055f) : (12.92f * num13))));
				num14 = ((num14 <= 0f) ? 0f : ((num14 >= 1f) ? 1f : ((num14 > 0.0031308f) ? (1.055f * MathF.Pow(num14, 5f / 12f) - 0.055f) : (12.92f * num14))));
				interleavedBuffer[num5] = (byte)(num12 * 255f + 0.5f);
				interleavedBuffer[num5 + 1] = (byte)(num13 * 255f + 0.5f);
				interleavedBuffer[num5 + 2] = (byte)(num14 * 255f + 0.5f);
				interleavedBuffer[num5 + 3] = byte.MaxValue;
				num4++;
				num5 += 4;
			}
		});
		return interleavedBuffer;
	}

	private static byte[] To8Bit(byte[] src, int bytesPerSample, int pixelCount)
	{
		byte[] array = new byte[pixelCount];
		switch (bytesPerSample)
		{
		case 1:
			Buffer.BlockCopy(src, 0, array, 0, pixelCount);
			break;
		case 2:
		{
			int num4 = 0;
			int num5 = 0;
			while (num4 < pixelCount)
			{
				ushort num6 = BinaryPrimitives.ReadUInt16BigEndian(src.AsSpan(num5, 2));
				array[num4] = (byte)(num6 / 257);
				num4++;
				num5 += 2;
			}
			break;
		}
		case 4:
		{
			int num = 0;
			int num2 = 0;
			while (num < pixelCount)
			{
				float num3 = BinaryPrimitives.ReadSingleBigEndian(src.AsSpan(num2, 4));
				if (num3 < 0f)
				{
					num3 = 0f;
				}
				else if (num3 > 1f)
				{
					num3 = 1f;
				}
				array[num] = (byte)(num3 * 255f + 0.5f);
				num++;
				num2 += 4;
			}
			break;
		}
		default:
			throw new NotSupportedException($"Unsupported channel depth: {bytesPerSample * 8} bits");
		}
		return array;
	}
}
