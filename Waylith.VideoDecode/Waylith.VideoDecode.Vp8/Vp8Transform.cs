using System.Runtime.CompilerServices;

namespace Waylith.VideoDecode.Vp8;

internal static class Vp8Transform
{
	private const int CosPi8Sqrt2Minus1 = 20091;

	private const int SinPi8Sqrt2 = 35468;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static byte Clip(int value)
	{
		return (byte)(((uint)value <= 255u) ? ((uint)value) : ((value >= 0) ? 255u : 0u));
	}

	[SkipLocalsInit]
	public unsafe static void InverseDctAdd(short* input, byte* dst, int stride)
	{
		short* ptr = stackalloc short[16];
		for (int i = 0; i < 4; i++)
		{
			short num = input[i];
			int num2 = input[4 + i];
			int num3 = input[8 + i];
			int num4 = input[12 + i];
			int num5 = num + num3;
			int num6 = num - num3;
			int num7 = (num2 * 35468 >> 16) - (num4 + (num4 * 20091 >> 16));
			int num8 = num2 + (num2 * 20091 >> 16) + (num4 * 35468 >> 16);
			ptr[i] = (short)(num5 + num8);
			ptr[12 + i] = (short)(num5 - num8);
			ptr[4 + i] = (short)(num6 + num7);
			ptr[8 + i] = (short)(num6 - num7);
		}
		for (int j = 0; j < 4; j++)
		{
			short* num9 = ptr + j * 4;
			int num10 = *num9;
			int num11 = num9[1];
			int num12 = num9[2];
			int num13 = num9[3];
			int num14 = num10 + num12;
			int num15 = num10 - num12;
			int num16 = (num11 * 35468 >> 16) - (num13 + (num13 * 20091 >> 16));
			int num17 = num11 + (num11 * 20091 >> 16) + (num13 * 35468 >> 16);
			*dst = Clip(*dst + (short)(num14 + num17 + 4 >> 3));
			dst[3] = Clip(dst[3] + (short)(num14 - num17 + 4 >> 3));
			dst[1] = Clip(dst[1] + (short)(num15 + num16 + 4 >> 3));
			dst[2] = Clip(dst[2] + (short)(num15 - num16 + 4 >> 3));
			dst += stride;
		}
	}

	public unsafe static void DcAdd(short dc, byte* dst, int stride)
	{
		int num = dc + 4 >> 3;
		if (num != 0)
		{
			for (int i = 0; i < 4; i++)
			{
				*dst = Clip(*dst + num);
				dst[1] = Clip(dst[1] + num);
				dst[2] = Clip(dst[2] + num);
				dst[3] = Clip(dst[3] + num);
				dst += stride;
			}
		}
	}

	[SkipLocalsInit]
	public unsafe static void InverseWalsh(short* input, short* coefficients)
	{
		short* ptr = stackalloc short[16];
		for (int i = 0; i < 4; i++)
		{
			int num = input[i] + input[12 + i];
			int num2 = input[4 + i] + input[8 + i];
			int num3 = input[4 + i] - input[8 + i];
			int num4 = input[i] - input[12 + i];
			ptr[i] = (short)(num + num2);
			ptr[4 + i] = (short)(num3 + num4);
			ptr[8 + i] = (short)(num - num2);
			ptr[12 + i] = (short)(num4 - num3);
		}
		for (int j = 0; j < 4; j++)
		{
			short* ptr2 = ptr + j * 4;
			int num5 = *ptr2 + ptr2[3];
			int num6 = ptr2[1] + ptr2[2];
			int num7 = ptr2[1] - ptr2[2];
			int num8 = *ptr2 - ptr2[3];
			coefficients[j * 4 * 16] = (short)(num5 + num6 + 3 >> 3);
			coefficients[(j * 4 + 1) * 16] = (short)(num7 + num8 + 3 >> 3);
			coefficients[(j * 4 + 2) * 16] = (short)(num5 - num6 + 3 >> 3);
			coefficients[(j * 4 + 3) * 16] = (short)(num8 - num7 + 3 >> 3);
		}
	}

	public unsafe static void InverseWalshDc(short dc, short* coefficients)
	{
		short num = (short)(dc + 3 >> 3);
		for (int i = 0; i < 16; i++)
		{
			coefficients[i * 16] = num;
		}
	}
}
