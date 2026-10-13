using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Waylith.Math;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct Mathf
{
	public const float Deg2Rad = (float)System.Math.PI / 180f;

	public const float Infinity = float.PositiveInfinity;

	public const int kMaxDecimals = 15;

	public const float NegativeInfinity = float.NegativeInfinity;

	public const float PI = (float)System.Math.PI;

	public const float Rad2Deg = 57.29578f;

	private static volatile float FloatMinNormal = 1.1754944E-38f;

	private static volatile float FloatMinDenormal = float.Epsilon;

	private static bool IsFlushToZeroEnabled = FloatMinDenormal == 0f;

	public static readonly float Epsilon = (IsFlushToZeroEnabled ? FloatMinNormal : FloatMinDenormal);

	public static float Abs(float f)
	{
		return MathF.Abs(f);
	}

	public static int Abs(int value)
	{
		return System.Math.Abs(value);
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
		return (int)MathF.Ceiling(f);
	}

	public static int Clamp(int value, int min, int max)
	{
		return System.Math.Clamp(value, min, max);
	}

	public static float Clamp(float value, float min, float max)
	{
		return System.Math.Clamp(value, min, max);
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
		return (int)MathF.Floor(f);
	}

	public static float InverseLerp(float a, float b, float value)
	{
		if (!(System.Math.Abs(b - a) < Epsilon))
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
		return System.Math.Max(a, b);
	}

	public static float Max(params float[] values)
	{
		if (values != null && values.Length != 0)
		{
			return values.Max();
		}
		return 0f;
	}

	public static float Min(float a, float b)
	{
		return MathF.Min(a, b);
	}

	public static int Min(int a, int b)
	{
		return System.Math.Min(a, b);
	}

	public static float Min(params float[] values)
	{
		if (values != null && values.Length != 0)
		{
			return values.Min();
		}
		return 0f;
	}

	public static float MoveTowards(float current, float target, float maxDelta)
	{
		if (Abs(target - current) <= maxDelta)
		{
			return target;
		}
		return current + Sign(target - current) * maxDelta;
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

	public static float Repeat(float t, float length)
	{
		return t - Floor(t / length) * length;
	}

	public static float Round(float f)
	{
		return MathF.Round(f);
	}

	public static int RoundToInt(float f)
	{
		return (int)MathF.Round(f);
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

	public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed = float.PositiveInfinity, float deltaTime = -1f)
	{
		if (deltaTime < 0f)
		{
			deltaTime = Time.DeltaTime;
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
		t = Clamp01((t - a) / (b - a));
		return t * t * (3f - 2f * t);
	}
}
