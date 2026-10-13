using System;

namespace UnityEngine;

public static class Random
{
	[Serializable]
	public struct State
	{
		[SerializeField]
		private int s0;

		[SerializeField]
		private int s1;

		[SerializeField]
		private int s2;

		[SerializeField]
		private int s3;

		internal State(int seed)
		{
			s0 = seed;
			s1 = s0 * 1812433253 + 1;
			s2 = s1 * 1812433253 + 1;
			s3 = s2 * 1812433253 + 1;
		}

		internal uint Next()
		{
			uint num = (uint)(s0 ^ (s0 << 11));
			s0 = s1;
			s1 = s2;
			s2 = s3;
			s3 = (int)((uint)(s3 ^ (s3 >>> 19)) ^ num ^ (num >> 8));
			return (uint)s3;
		}
	}

	private static State s_State = new State(Environment.TickCount);

	public static State state
	{
		get
		{
			return s_State;
		}
		set
		{
			s_State = value;
		}
	}

	public static float value => (float)(s_State.Next() & 0x7FFFFF) / 8388607f;

	public static Vector2 insideUnitCircle
	{
		get
		{
			float x = Range(0f, (float)Math.PI * 2f);
			float num = MathF.Sqrt(value);
			return new Vector2(MathF.Cos(x) * num, MathF.Sin(x) * num);
		}
	}

	public static Vector3 onUnitSphere
	{
		get
		{
			float num = Range(-1f, 1f);
			float x = Range(0f, (float)Math.PI * 2f);
			float num2 = MathF.Sqrt(Math.Max(0f, 1f - num * num));
			return new Vector3(MathF.Cos(x) * num2, MathF.Sin(x) * num2, num);
		}
	}

	public static Vector3 insideUnitSphere => onUnitSphere * MathF.Cbrt(value);

	public static Quaternion rotation => Quaternion.Euler(Range(0f, 360f), Range(0f, 360f), Range(0f, 360f));

	public static void InitState(int seed)
	{
		s_State = new State(seed);
	}

	public static float Range(float minInclusive, float maxInclusive)
	{
		return minInclusive + value * (maxInclusive - minInclusive);
	}

	[Obsolete("Use Random.Range instead")]
	public static float RandomRange(float min, float max)
	{
		return Range(min, max);
	}

	[Obsolete("Use Random.Range instead")]
	public static int RandomRange(int min, int max)
	{
		return Range(min, max);
	}

	public static int Range(int minInclusive, int maxExclusive)
	{
		if (maxExclusive > minInclusive)
		{
			return (int)(minInclusive + s_State.Next() % ((long)maxExclusive - (long)minInclusive));
		}
		return minInclusive;
	}

	public static Color ColorHSV()
	{
		return ColorHSV(0f, 1f, 0f, 1f, 0f, 1f, 1f, 1f);
	}

	public static Color ColorHSV(float hueMin, float hueMax)
	{
		return ColorHSV(hueMin, hueMax, 0f, 1f, 0f, 1f, 1f, 1f);
	}

	public static Color ColorHSV(float hueMin, float hueMax, float saturationMin, float saturationMax)
	{
		return ColorHSV(hueMin, hueMax, saturationMin, saturationMax, 0f, 1f, 1f, 1f);
	}

	public static Color ColorHSV(float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin, float valueMax)
	{
		return ColorHSV(hueMin, hueMax, saturationMin, saturationMax, valueMin, valueMax, 1f, 1f);
	}

	public static Color ColorHSV(float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin, float valueMax, float alphaMin, float alphaMax)
	{
		Color result = Color.HSVToRGB(Range(hueMin, hueMax), Range(saturationMin, saturationMax), Range(valueMin, valueMax));
		result.a = Range(alphaMin, alphaMax);
		return result;
	}
}
