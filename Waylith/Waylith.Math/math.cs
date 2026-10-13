using System;
using System.Numerics;

namespace Waylith.Math;

public static class math
{
	public enum RotationOrder : byte
	{
		Default = 4,
		XYZ = 0,
		XZY = 1,
		YXZ = 2,
		YZX = 3,
		ZXY = 4,
		ZYX = 5
	}

	public const double DBL_MIN_NORMAL = 2.2250738585072014E-308;

	public const float E = (float)System.Math.E;

	public const double E_DBL = System.Math.E;

	public const float EPSILON = 1.1920929E-07f;

	public const double EPSILON_DBL = 2.220446049250313E-16;

	public const float FLT_MIN_NORMAL = 1.1754944E-38f;

	public const float INFINITY = float.PositiveInfinity;

	public const double INFINITY_DBL = double.PositiveInfinity;

	public const float LN10 = 2.3025851f;

	public const double LN10_DBL = 2.302585092994046;

	public const float LN2 = 0.6931472f;

	public const double LN2_DBL = 0.6931471805599453;

	public const float LOG10E = 0.4342945f;

	public const double LOG10E_DBL = 0.4342944819032518;

	public const float LOG2E = 1.442695f;

	public const double LOG2E_DBL = 1.4426950408889634;

	public const float NAN = float.NaN;

	public const double NAN_DBL = double.NaN;

	public const float PI = (float)System.Math.PI;

	public const double PI_DBL = System.Math.PI;

	public const float PI2 = (float)System.Math.PI * 2f;

	public const double PI2_DBL = System.Math.PI * 2.0;

	public const float PIHALF = (float)System.Math.PI / 2f;

	public const double PIHALF_DBL = System.Math.PI / 2.0;

	public const float SQRT2 = 1.4142135f;

	public const double SQRT2_DBL = 1.4142135623730951;

	public const float TAU = (float)System.Math.PI * 2f;

	public const double TAU_DBL = System.Math.PI * 2.0;

	public const float TODEGREES = 57.29578f;

	public const double TODEGREES_DBL = 180.0 / System.Math.PI;

	public const float TORADIANS = (float)System.Math.PI / 180f;

	public const double TORADIANS_DBL = System.Math.PI / 180.0;

	public static float abs(float x)
	{
		return MathF.Abs(x);
	}

	public static int3 abs(int3 x)
	{
		return new int3(abs(x.x), abs(x.y), abs(x.z));
	}

	public static int abs(int x)
	{
		return System.Math.Abs(x);
	}

	public static double abs(double x)
	{
		return System.Math.Abs(x);
	}

	public static float acos(float x)
	{
		return MathF.Acos(x);
	}

	public static bool all(bool3 x)
	{
		if (x.x && x.y)
		{
			return x.z;
		}
		return false;
	}

	public static bool any(bool3 x)
	{
		if (!x.x && !x.y)
		{
			return x.z;
		}
		return true;
	}

	public static double asin(double x)
	{
		return System.Math.Asin(x);
	}

	public static float asin(float x)
	{
		return MathF.Asin(x);
	}

	public static double atan2(double y, double x)
	{
		return System.Math.Atan2(y, x);
	}

	public static float atan2(float y, float x)
	{
		return MathF.Atan2(y, x);
	}

	public static float3 clamp(float3 valueToClamp, float3 lowerBound, float3 upperBound)
	{
		return new float3(clamp(valueToClamp.x, lowerBound.x, upperBound.x), clamp(valueToClamp.y, lowerBound.y, upperBound.y), clamp(valueToClamp.z, lowerBound.z, upperBound.z));
	}

	public static float clamp(float valueToClamp, float lowerBound, float upperBound)
	{
		return System.Math.Clamp(valueToClamp, lowerBound, upperBound);
	}

	public static int clamp(int valueToClamp, int lowerBound, int upperBound)
	{
		return System.Math.Clamp(valueToClamp, lowerBound, upperBound);
	}

	public static int3 clamp(int3 valueToClamp, int3 lowerBound, int3 upperBound)
	{
		return new int3(clamp(valueToClamp.x, lowerBound.x, upperBound.x), clamp(valueToClamp.y, lowerBound.y, upperBound.y), clamp(valueToClamp.z, lowerBound.z, upperBound.z));
	}

	public static int countbits(int x)
	{
		return BitOperations.PopCount((uint)x);
	}

	public static int countbits(uint x)
	{
		return BitOperations.PopCount(x);
	}

	public static float3 cross(float3 x, float3 y)
	{
		return new float3(x.y * y.z - x.z * y.y, x.z * y.x - x.x * y.z, x.x * y.y - x.y * y.x);
	}

	public static float dot(float3 x, float3 y)
	{
		return x.x * y.x + x.y * y.y + x.z * y.z;
	}

	public static float dot(float2 x, float2 y)
	{
		return x.x * y.x + x.y * y.y;
	}

	public static float2 float2(float x, float y)
	{
		return new float2(x, y);
	}

	public static float3 float3(float x, float y, float z)
	{
		return new float3(x, y, z);
	}

	public static float3 float3(int v)
	{
		return new float3(v);
	}

	public static float3 float3(float v)
	{
		return new float3(v);
	}

	public static float4 float4(float3 xyz, float w)
	{
		return new float4(xyz.x, xyz.y, xyz.z, w);
	}

	public static float4 float4(float x, float y, float z, float w)
	{
		return new float4(x, y, z, w);
	}

	public static float floor(float x)
	{
		return MathF.Floor(x);
	}

	public static float3 frac(float3 x)
	{
		return new float3(frac(x.x), frac(x.y), frac(x.z));
	}

	public static float frac(float x)
	{
		return x - floor(x);
	}

	public static quaternion inverse(quaternion q)
	{
		float num = q.value.x * q.value.x + q.value.y * q.value.y + q.value.z * q.value.z + q.value.w * q.value.w;
		if (num <= 1.1920929E-07f)
		{
			return quaternion.identity;
		}
		float num2 = 1f / num;
		quaternion result = default(quaternion);
		result.value = new float4((0f - q.value.x) * num2, (0f - q.value.y) * num2, (0f - q.value.z) * num2, q.value.w * num2);
		return result;
	}

	public static bool3 isnan(float3 x)
	{
		return new bool3(float.IsNaN(x.x), float.IsNaN(x.y), float.IsNaN(x.z));
	}

	public static bool isnan(float x)
	{
		return float.IsNaN(x);
	}

	public static float length(float3 x)
	{
		return sqrt(lengthsq(x));
	}

	public static float lengthsq(float3 x)
	{
		return dot(x, x);
	}

	public static float3 lerp(float3 start, float3 end, float t)
	{
		return new float3(lerp(start.x, end.x, t), lerp(start.y, end.y, t), lerp(start.z, end.z, t));
	}

	public static float4 lerp(float4 start, float4 end, float t)
	{
		return new float4(lerp(start.x, end.x, t), lerp(start.y, end.y, t), lerp(start.z, end.z, t), lerp(start.w, end.w, t));
	}

	public static double lerp(double start, double end, double t)
	{
		return start + (end - start) * t;
	}

	public static float lerp(float start, float end, float t)
	{
		return start + (end - start) * t;
	}

	public static float3 lerp(float3 start, float3 end, float3 t)
	{
		return new float3(lerp(start.x, end.x, t.x), lerp(start.y, end.y, t.y), lerp(start.z, end.z, t.z));
	}

	public static int lzcnt(uint x)
	{
		return BitOperations.LeadingZeroCount(x);
	}

	public static float max(float x, float y)
	{
		return MathF.Max(x, y);
	}

	public static int max(int x, int y)
	{
		return System.Math.Max(x, y);
	}

	public static float3 max(float3 x, float3 y)
	{
		return new float3(max(x.x, y.x), max(x.y, y.y), max(x.z, y.z));
	}

	public static int3 max(int3 x, int3 y)
	{
		return new int3(max(x.x, y.x), max(x.y, y.y), max(x.z, y.z));
	}

	public static float min(float x, float y)
	{
		return MathF.Min(x, y);
	}

	public static int min(int x, int y)
	{
		return System.Math.Min(x, y);
	}

	public static float3 min(float3 x, float3 y)
	{
		return new float3(min(x.x, y.x), min(x.y, y.y), min(x.z, y.z));
	}

	public static int3 min(int3 x, int3 y)
	{
		return new int3(min(x.x, y.x), min(x.y, y.y), min(x.z, y.z));
	}

	public static float3 mul(float3x3 a, float3 b)
	{
		return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z;
	}

	public static float4 mul(float4x4 a, float4 b)
	{
		return a.c0 * b.x + a.c1 * b.y + a.c2 * b.z + a.c3 * b.w;
	}

	public static quaternion mul(quaternion a, quaternion b)
	{
		float4 value = a.value;
		float4 value2 = b.value;
		quaternion result = default(quaternion);
		result.value = new float4(value.w * value2.x + value.x * value2.w + value.y * value2.z - value.z * value2.y, value.w * value2.y - value.x * value2.z + value.y * value2.w + value.z * value2.x, value.w * value2.z + value.x * value2.y - value.y * value2.x + value.z * value2.w, value.w * value2.w - value.x * value2.x - value.y * value2.y - value.z * value2.z);
		return result;
	}

	public static quaternion nlerp(quaternion q1, quaternion q2, float t)
	{
		float4 end = ((q1.value.x * q2.value.x + q1.value.y * q2.value.y + q1.value.z * q2.value.z + q1.value.w * q2.value.w < 0f) ? new float4(0f - q2.value.x, 0f - q2.value.y, 0f - q2.value.z, 0f - q2.value.w) : q2.value);
		float4 float5 = lerp(q1.value, end, t);
		float num = MathF.Sqrt(float5.x * float5.x + float5.y * float5.y + float5.z * float5.z + float5.w * float5.w);
		if (num <= 1.1920929E-07f)
		{
			return quaternion.identity;
		}
		float num2 = 1f / num;
		quaternion result = default(quaternion);
		result.value = new float4(float5.x * num2, float5.y * num2, float5.z * num2, float5.w * num2);
		return result;
	}

	public static float3 normalize(float3 x)
	{
		float num = lengthsq(x);
		if (num <= 1E-12f)
		{
			return Waylith.Math.float3.zero;
		}
		return x / MathF.Sqrt(num);
	}

	public static float pow(float x, float y)
	{
		return MathF.Pow(x, y);
	}

	public static float3 radians(float3 x)
	{
		return new float3(x.x * ((float)System.Math.PI / 180f), x.y * ((float)System.Math.PI / 180f), x.z * ((float)System.Math.PI / 180f));
	}

	public static float radians(float x)
	{
		return x * ((float)System.Math.PI / 180f);
	}

	public static double remap(double srcStart, double srcEnd, double dstStart, double dstEnd, double x)
	{
		return lerp(dstStart, dstEnd, unlerp(srcStart, srcEnd, x));
	}

	public static float remap(float srcStart, float srcEnd, float dstStart, float dstEnd, float x)
	{
		return lerp(dstStart, dstEnd, unlerp(srcStart, srcEnd, x));
	}

	public static float3 rotate(quaternion q, float3 v)
	{
		float3 xyz = q.value.xyz;
		float3 float5 = 2f * cross(xyz, v);
		return v + q.value.w * float5 + cross(xyz, float5);
	}

	public static float round(float f)
	{
		return (float)System.Math.Round(f);
	}

	public static float saturate(float x)
	{
		return clamp(x, 0f, 1f);
	}

	public static float3 sign(float3 x)
	{
		return new float3(sign(x.x), sign(x.y), sign(x.z));
	}

	public static double sign(double x)
	{
		if (!double.IsNaN(x))
		{
			if (!(x > 0.0))
			{
				if (!(x < 0.0))
				{
					return 0.0;
				}
				return -1.0;
			}
			return 1.0;
		}
		return double.NaN;
	}

	public static float sign(float x)
	{
		if (!float.IsNaN(x))
		{
			if (!(x > 0f))
			{
				if (!(x < 0f))
				{
					return 0f;
				}
				return -1f;
			}
			return 1f;
		}
		return float.NaN;
	}

	public static float sin(float x)
	{
		return MathF.Sin(x);
	}

	public static void sincos(float x, out float s, out float c)
	{
		s = MathF.Sin(x);
		c = MathF.Cos(x);
	}

	public static float sqrt(float x)
	{
		return MathF.Sqrt(x);
	}

	public static float tan(float x)
	{
		return MathF.Tan(x);
	}

	public static int tzcnt(uint x)
	{
		if (x != 0)
		{
			return BitOperations.TrailingZeroCount(x);
		}
		return 32;
	}

	public static int tzcnt(ulong x)
	{
		if (x != 0L)
		{
			return BitOperations.TrailingZeroCount(x);
		}
		return 64;
	}

	public static float unlerp(float start, float end, float x)
	{
		return (x - start) / (end - start);
	}

	public static double unlerp(double start, double end, double x)
	{
		return (x - start) / (end - start);
	}
}
