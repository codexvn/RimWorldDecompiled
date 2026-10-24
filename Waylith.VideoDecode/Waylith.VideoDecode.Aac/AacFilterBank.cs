using System;

namespace Waylith.VideoDecode.Aac;

internal sealed class AacFilterBank
{
	public const int OnlyLong = 0;

	public const int LongStart = 1;

	public const int EightShort = 2;

	public const int LongStop = 3;

	private const int Long = 1024;

	private const int Short = 128;

	private const int Flat = 448;

	private static readonly float[][] LongWindows = new float[2][]
	{
		SineWindow(1024),
		KaiserBesselWindow(1024, 4.0)
	};

	private static readonly float[][] ShortWindows = new float[2][]
	{
		SineWindow(128),
		KaiserBesselWindow(128, 6.0)
	};

	private readonly AacImdct longTransform = new AacImdct(1024);

	private readonly AacImdct shortTransform = new AacImdct(128);

	private readonly float[] block = new float[2048];

	private readonly float[] shortBlock = new float[256];

	private static float[] SineWindow(int half)
	{
		float[] array = new float[half];
		for (int i = 0; i < half; i++)
		{
			array[i] = (float)System.Math.Sin(System.Math.PI / (double)(2 * half) * ((double)i + 0.5));
		}
		return array;
	}

	private static float[] KaiserBesselWindow(int half, double alpha)
	{
		double[] array = new double[half + 1];
		double num = 0.0;
		for (int i = 0; i <= half; i++)
		{
			double num2 = 2.0 * (double)i / (double)half - 1.0;
			array[i] = BesselI0(System.Math.PI * alpha * System.Math.Sqrt(1.0 - num2 * num2));
			num += array[i];
		}
		float[] array2 = new float[half];
		double num3 = 0.0;
		for (int j = 0; j < half; j++)
		{
			num3 += array[j];
			array2[j] = (float)System.Math.Sqrt(num3 / num);
		}
		return array2;
	}

	private static double BesselI0(double x)
	{
		double num = 1.0;
		double num2 = 1.0;
		for (int i = 1; i < 64; i++)
		{
			num2 *= x / (double)(2 * i);
			num += num2 * num2;
		}
		return num;
	}

	public void Process(ReadOnlySpan<float> spectrum, int sequence, int shape, int previousShape, float[] overlap, Span<float> output, int stride)
	{
		if (sequence == 2)
		{
			ProcessShort(spectrum, shape, previousShape);
		}
		else
		{
			longTransform.Transform(spectrum, block);
			float[] array = LongWindows[previousShape];
			float[] array2 = LongWindows[shape];
			if (sequence == 3)
			{
				float[] array3 = ShortWindows[previousShape];
				Array.Clear(block, 0, 448);
				for (int i = 0; i < 128; i++)
				{
					block[448 + i] *= array3[i];
				}
			}
			else
			{
				for (int j = 0; j < 1024; j++)
				{
					block[j] *= array[j];
				}
			}
			if (sequence == 1)
			{
				float[] array4 = ShortWindows[shape];
				for (int k = 0; k < 128; k++)
				{
					block[1472 + k] *= array4[127 - k];
				}
				Array.Clear(block, 1600, 448);
			}
			else
			{
				for (int l = 0; l < 1024; l++)
				{
					block[1024 + l] *= array2[1023 - l];
				}
			}
		}
		for (int m = 0; m < 1024; m++)
		{
			output[m * stride] = overlap[m] + block[m];
			overlap[m] = block[1024 + m];
		}
	}

	private void ProcessShort(ReadOnlySpan<float> spectrum, int shape, int previousShape)
	{
		Array.Clear(block);
		for (int i = 0; i < 8; i++)
		{
			shortTransform.Transform(spectrum.Slice(i * 128, 128), shortBlock);
			float[] array = ShortWindows[(i == 0) ? previousShape : shape];
			float[] array2 = ShortWindows[shape];
			int num = 448 + i * 128;
			for (int j = 0; j < 128; j++)
			{
				block[num + j] += shortBlock[j] * array[j];
				block[num + 128 + j] += shortBlock[128 + j] * array2[127 - j];
			}
		}
	}
}
