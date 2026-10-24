using System;
using System.Numerics;

namespace Waylith.VideoDecode.Aac;

internal sealed class AacImdct
{
	private readonly int size;

	private readonly float[] preCos;

	private readonly float[] preSin;

	private readonly float[] postCos;

	private readonly float[] postSin;

	private readonly float[] stepCos;

	private readonly float[] stepSin;

	private readonly int[] reversed;

	private readonly float[] real;

	private readonly float[] imaginary;

	private readonly float[] folded;

	public AacImdct(int size)
	{
		this.size = size;
		int num = size / 2;
		preCos = new float[num];
		preSin = new float[num];
		postCos = new float[num];
		postSin = new float[num];
		stepCos = new float[num / 2];
		stepSin = new float[num / 2];
		reversed = new int[num];
		real = new float[num];
		imaginary = new float[num];
		folded = new float[size];
		double num2 = 1.0 / (double)size / 32768.0;
		int num3 = BitOperations.Log2((uint)num);
		for (int i = 0; i < num; i++)
		{
			double num4 = System.Math.PI * (double)(4 * i + 1) / (4.0 * (double)size);
			preCos[i] = (float)(System.Math.Cos(num4) * num2);
			preSin[i] = (float)(System.Math.Sin(num4) * num2);
			num4 = System.Math.PI * (double)i / (double)size;
			postCos[i] = (float)System.Math.Cos(num4);
			postSin[i] = (float)System.Math.Sin(num4);
			int num5 = 0;
			for (int j = 0; j < num3; j++)
			{
				num5 |= ((i >> j) & 1) << num3 - 1 - j;
			}
			reversed[i] = num5;
		}
		for (int k = 0; k < num / 2; k++)
		{
			double num6 = System.Math.PI * 2.0 * (double)k / (double)num;
			stepCos[k] = (float)System.Math.Cos(num6);
			stepSin[k] = (float)System.Math.Sin(num6);
		}
	}

	public void Transform(ReadOnlySpan<float> spectrum, Span<float> output)
	{
		int num = size / 2;
		for (int i = 0; i < num; i++)
		{
			float num2 = spectrum[2 * i];
			float num3 = spectrum[size - 1 - 2 * i];
			int num4 = reversed[i];
			real[num4] = num2 * preCos[i] + num3 * preSin[i];
			imaginary[num4] = num3 * preCos[i] - num2 * preSin[i];
		}
		for (int num5 = 2; num5 <= num; num5 <<= 1)
		{
			int num6 = num / num5;
			int num7 = num5 / 2;
			for (int j = 0; j < num; j += num5)
			{
				for (int k = 0; k < num7; k++)
				{
					float num8 = stepCos[k * num6];
					float num9 = stepSin[k * num6];
					int num10 = j + k;
					int num11 = num10 + num7;
					float num12 = real[num11] * num8 + imaginary[num11] * num9;
					float num13 = imaginary[num11] * num8 - real[num11] * num9;
					real[num11] = real[num10] - num12;
					imaginary[num11] = imaginary[num10] - num13;
					real[num10] += num12;
					imaginary[num10] += num13;
				}
			}
		}
		for (int l = 0; l < num; l++)
		{
			folded[2 * l] = real[l] * postCos[l] + imaginary[l] * postSin[l];
			folded[size - 1 - 2 * l] = real[l] * postSin[l] - imaginary[l] * postCos[l];
		}
		for (int m = 0; m < num; m++)
		{
			output[m] = folded[num + m];
			output[size + num + m] = 0f - folded[m];
		}
		for (int n = 0; n < size; n++)
		{
			output[num + n] = 0f - folded[size - 1 - n];
		}
	}
}
