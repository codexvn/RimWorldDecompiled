using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace UnityEngine;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct Mathf
{
	public const float Deg2Rad = (float)Math.PI / 180f;

	public const float Infinity = float.PositiveInfinity;

	public const int kMaxDecimals = 15;

	public const float NegativeInfinity = float.NegativeInfinity;

	public const float PI = (float)Math.PI;

	public const float Rad2Deg = 57.29578f;

	private static volatile float FloatMinNormal = 1.1754944E-38f;

	private static volatile float FloatMinDenormal = float.Epsilon;

	private static bool IsFlushToZeroEnabled = FloatMinDenormal == 0f;

	public static readonly float Epsilon = (IsFlushToZeroEnabled ? FloatMinNormal : FloatMinDenormal);

	private static readonly int[] PerlinPerm = BuildPerlinPerm();

	public static float Asin(float f)
	{
		return (float)Math.Asin(f);
	}

	public static bool IsPowerOfTwo(int value)
	{
		if (value > 0)
		{
			return (value & (value - 1)) == 0;
		}
		return false;
	}

	public static float PerlinNoise1D(float x)
	{
		return PerlinNoise(x, 0f);
	}

	public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime)
	{
		return SmoothDamp(current, target, ref currentVelocity, smoothTime, float.PositiveInfinity, -1f);
	}

	public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed)
	{
		return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, -1f);
	}

	public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
	{
		if (deltaTime < 0f)
		{
			deltaTime = Time.deltaTime;
		}
		if (deltaTime <= 0f)
		{
			return current;
		}
		smoothTime = Max(0.0001f, smoothTime);
		float num = 2f / smoothTime;
		float num2 = num * deltaTime;
		float num3 = 1f / (1f + num2 + 0.48f * num2 * num2 + 0.235f * num2 * num2 * num2);
		float value = current - target;
		float num4 = target;
		float num5 = maxSpeed * smoothTime;
		value = Clamp(value, 0f - num5, num5);
		target = current - value;
		float num6 = (currentVelocity + num * value) * deltaTime;
		currentVelocity = (currentVelocity - num * num6) * num3;
		float num7 = target + (value + num6) * num3;
		if (num4 - current > 0f == num7 > num4)
		{
			num7 = num4;
			currentVelocity = (num7 - num4) / deltaTime;
		}
		return num7;
	}

	public static float Abs(float f)
	{
		return MathF.Abs(f);
	}

	public static int Abs(int value)
	{
		return Math.Abs(value);
	}

	public static float Acos(float f)
	{
		return MathF.Acos(f);
	}

	public static bool Approximately(float a, float b)
	{
		return Abs(b - a) <= Max(1E-06f * Max(Abs(a), Abs(b)), Epsilon * 8f);
	}

	public static float Atan2(float y, float x)
	{
		return MathF.Atan2(y, x);
	}

	public static float Ceil(float f)
	{
		return MathF.Ceiling(f);
	}

	public static int CeilToInt(float f)
	{
		return MonoToInt(MathF.Ceiling(f));
	}

	public static int Clamp(int value, int min, int max)
	{
		if (value >= min)
		{
			if (value <= max)
			{
				return value;
			}
			return max;
		}
		return min;
	}

	public static float Clamp(float value, float min, float max)
	{
		if (!(value < min))
		{
			if (!(value > max))
			{
				return value;
			}
			return max;
		}
		return min;
	}

	public static float Clamp01(float value)
	{
		return Clamp(value, 0f, 1f);
	}

	public static float Cos(float f)
	{
		return MathF.Cos(f);
	}

	public static float DeltaAngle(float current, float target)
	{
		float num = Repeat(target - current, 360f);
		if (num > 180f)
		{
			num -= 360f;
		}
		return num;
	}

	public static float Exp(float power)
	{
		return MathF.Exp(power);
	}

	public static float Floor(float f)
	{
		return MathF.Floor(f);
	}

	public static int FloorToInt(float f)
	{
		return MonoToInt(MathF.Floor(f));
	}

	public static float InverseLerp(float a, float b, float value)
	{
		if (!(Math.Abs(b - a) < Epsilon))
		{
			return Clamp01((value - a) / (b - a));
		}
		return 0f;
	}

	public static float Lerp(float a, float b, float t)
	{
		return a + (b - a) * Clamp01(t);
	}

	public static float LerpUnclamped(float a, float b, float t)
	{
		return a + (b - a) * t;
	}

	public static float Log(float f, float p)
	{
		return MathF.Log(f, p);
	}

	public static float Log(float f)
	{
		return MathF.Log(f);
	}

	public static float Max(float a, float b)
	{
		return MathF.Max(a, b);
	}

	public static int Max(int a, int b)
	{
		return Math.Max(a, b);
	}

	public static float Max(params float[] values)
	{
		if (values != null && values.Length != 0)
		{
			return values.Max();
		}
		return 0f;
	}

	public static int Max(params int[] values)
	{
		if (values != null && values.Length != 0)
		{
			return values.Max();
		}
		return 0;
	}

	public static float Min(float a, float b)
	{
		return MathF.Min(a, b);
	}

	public static int Min(int a, int b)
	{
		return Math.Min(a, b);
	}

	public static float Min(params float[] values)
	{
		if (values != null && values.Length != 0)
		{
			return values.Min();
		}
		return 0f;
	}

	public static int Min(params int[] values)
	{
		if (values != null && values.Length != 0)
		{
			return values.Min();
		}
		return 0;
	}

	public static float MoveTowards(float current, float target, float maxDelta)
	{
		if (Abs(target - current) <= maxDelta)
		{
			return target;
		}
		return current + Sign(target - current) * maxDelta;
	}

	public static float MoveTowardsAngle(float current, float target, float maxDelta)
	{
		float num = DeltaAngle(current, target);
		if (0f - maxDelta < num && num < maxDelta)
		{
			return target;
		}
		return MoveTowards(current, current + num, maxDelta);
	}

	public static int ClosestPowerOfTwo(int value)
	{
		int num = NextPowerOfTwo(value);
		int num2 = num >> 1;
		if (num2 <= 0 || value - num2 >= num - value)
		{
			return num;
		}
		return num2;
	}

	public static ushort FloatToHalf(float val)
	{
		return BitConverter.HalfToUInt16Bits((Half)val);
	}

	public static float HalfToFloat(ushort val)
	{
		return (float)BitConverter.UInt16BitsToHalf(val);
	}

	public static int NextPowerOfTwo(int value)
	{
		if (value <= 0)
		{
			return 1;
		}
		value--;
		value |= value >> 1;
		value |= value >> 2;
		value |= value >> 4;
		value |= value >> 8;
		value |= value >> 16;
		return value + 1;
	}

	public static float PingPong(float t, float length)
	{
		t = Repeat(t, length * 2f);
		return length - Abs(t - length);
	}

	public static float Pow(float f, float p)
	{
		return MathF.Pow(f, p);
	}

	public static float GammaToLinearSpace(float value)
	{
		if (!(value <= 0.04045f))
		{
			return MathF.Pow((value + 0.055f) / 1.055f, 2.4f);
		}
		return value / 12.92f;
	}

	public static float LinearToGammaSpace(float value)
	{
		if (!(value <= 0.0031308f))
		{
			return 1.055f * MathF.Pow(value, 5f / 12f) - 0.055f;
		}
		return value * 12.92f;
	}

	public static float Repeat(float t, float length)
	{
		return t - Floor(t / length) * length;
	}

	public static float Round(float f)
	{
		return MathF.Round(f);
	}

	public static float RoundToMultipleOf(float value, float roundingValue)
	{
		if (roundingValue != 0f)
		{
			return MathF.Round(value / roundingValue) * roundingValue;
		}
		return value;
	}

	public static int RoundToInt(float f)
	{
		return MonoToInt(MathF.Round(f));
	}

	private static int MonoToInt(float f)
	{
		if (!(f >= -2.1474836E+09f) || !(f < 2.1474836E+09f))
		{
			return int.MinValue;
		}
		return (int)f;
	}

	public static float Sign(float f)
	{
		if (!(f >= 0f))
		{
			return -1f;
		}
		return 1f;
	}

	public static float Sin(float f)
	{
		return MathF.Sin(f);
	}

	public static float Sqrt(float f)
	{
		return MathF.Sqrt(f);
	}

	public static float Tan(float f)
	{
		return MathF.Tan(f);
	}

	public static float SmoothStep(float a, float b, float t)
	{
		t = Clamp01(t);
		t = t * t * (3f - 2f * t);
		return b * t + a * (1f - t);
	}

	public static float Log10(float f)
	{
		return MathF.Log10(f);
	}

	public static float Atan(float f)
	{
		return MathF.Atan(f);
	}

	public static float LerpAngle(float a, float b, float t)
	{
		float num = Repeat(b - a, 360f);
		if (num > 180f)
		{
			num -= 360f;
		}
		return a + num * Clamp01(t);
	}

	public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime)
	{
		return SmoothDampAngle(current, target, ref currentVelocity, smoothTime, float.PositiveInfinity, -1f);
	}

	public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed)
	{
		return SmoothDampAngle(current, target, ref currentVelocity, smoothTime, maxSpeed, -1f);
	}

	public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
	{
		target = current + DeltaAngle(current, target);
		return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
	}

	public static float PerlinNoise(float x, float y)
	{
		int num = (int)Floor(x) & 0xFF;
		int num2 = (int)Floor(y) & 0xFF;
		float num3 = x - Floor(x);
		float num4 = y - Floor(y);
		float t2 = Fade(num3);
		float t3 = Fade(num4);
		int hash2 = PerlinPerm[PerlinPerm[num] + num2];
		int hash3 = PerlinPerm[PerlinPerm[num] + num2 + 1];
		int hash4 = PerlinPerm[PerlinPerm[num + 1] + num2];
		int hash5 = PerlinPerm[PerlinPerm[num + 1] + num2 + 1];
		float a = Lerp(Grad(hash2, num3, num4), Grad(hash4, num3 - 1f, num4), t2);
		float b = Lerp(Grad(hash3, num3, num4 - 1f), Grad(hash5, num3 - 1f, num4 - 1f), t2);
		return Clamp01(Lerp(a, b, t3) * 0.5f + 0.5f);
		static float Fade(float t)
		{
			return t * t * t * (t * (t * 6f - 15f) + 10f);
		}
		static float Grad(int hash, float x, float y)
		{
			return (hash & 3) switch
			{
				0 => x + y, 
				1 => 0f - x + y, 
				2 => x - y, 
				_ => 0f - x - y, 
			};
		}
	}

	private static int[] BuildPerlinPerm()
	{
		int[] array = new int[256]
		{
			151, 160, 137, 91, 90, 15, 131, 13, 201, 95,
			96, 53, 194, 233, 7, 225, 140, 36, 103, 30,
			69, 142, 8, 99, 37, 240, 21, 10, 23, 190,
			6, 148, 247, 120, 234, 75, 0, 26, 197, 62,
			94, 252, 219, 203, 117, 35, 11, 32, 57, 177,
			33, 88, 237, 149, 56, 87, 174, 20, 125, 136,
			171, 168, 68, 175, 74, 165, 71, 134, 139, 48,
			27, 166, 77, 146, 158, 231, 83, 111, 229, 122,
			60, 211, 133, 230, 220, 105, 92, 41, 55, 46,
			245, 40, 244, 102, 143, 54, 65, 25, 63, 161,
			1, 216, 80, 73, 209, 76, 132, 187, 208, 89,
			18, 169, 200, 196, 135, 130, 116, 188, 159, 86,
			164, 100, 109, 198, 173, 186, 3, 64, 52, 217,
			226, 250, 124, 123, 5, 202, 38, 147, 118, 126,
			255, 82, 85, 212, 207, 206, 59, 227, 47, 16,
			58, 17, 182, 189, 28, 42, 223, 183, 170, 213,
			119, 248, 152, 2, 44, 154, 163, 70, 221, 153,
			101, 155, 167, 43, 172, 9, 129, 22, 39, 253,
			19, 98, 108, 110, 79, 113, 224, 232, 178, 185,
			112, 104, 218, 246, 97, 228, 251, 34, 242, 193,
			238, 210, 144, 12, 191, 179, 162, 241, 81, 51,
			145, 235, 249, 14, 239, 107, 49, 192, 214, 31,
			181, 199, 106, 157, 184, 84, 204, 176, 115, 121,
			50, 45, 127, 4, 150, 254, 138, 236, 205, 93,
			222, 114, 67, 29, 24, 72, 243, 141, 128, 195,
			78, 66, 215, 61, 156, 180
		};
		int[] array2 = new int[512];
		for (int i = 0; i < 512; i++)
		{
			array2[i] = array[i & 0xFF];
		}
		return array2;
	}
}
