using System;

namespace Waylith.Math;

public static class noise
{
	public static float snoise(float3 v, out float3 gradient)
	{
		float num = (v.x + v.y + v.z) * (1f / 3f);
		float num2 = MathF.Floor(v.x + num);
		float num3 = MathF.Floor(v.y + num);
		float num4 = MathF.Floor(v.z + num);
		float num5 = (num2 + num3 + num4) * (1f / 6f);
		float num6 = v.x - num2 + num5;
		float num7 = v.y - num3 + num5;
		float num8 = v.z - num4 + num5;
		float num9 = ((num6 >= num7) ? 1f : 0f);
		float num10 = ((num7 >= num8) ? 1f : 0f);
		float num11 = ((num8 >= num6) ? 1f : 0f);
		float y = 1f - num9;
		float y2 = 1f - num10;
		float y3 = 1f - num11;
		float num12 = MathF.Min(num9, y3);
		float num13 = MathF.Min(num10, y);
		float num14 = MathF.Min(num11, y2);
		float num15 = MathF.Max(num9, y3);
		float num16 = MathF.Max(num10, y);
		float num17 = MathF.Max(num11, y2);
		float num18 = num6 - num12 + 1f / 6f;
		float num19 = num7 - num13 + 1f / 6f;
		float num20 = num8 - num14 + 1f / 6f;
		float num21 = num6 - num15 + 1f / 3f;
		float num22 = num7 - num16 + 1f / 3f;
		float num23 = num8 - num17 + 1f / 3f;
		float num24 = num6 - 0.5f;
		float num25 = num7 - 0.5f;
		float num26 = num8 - 0.5f;
		num2 = Mod289(num2);
		num3 = Mod289(num3);
		num4 = Mod289(num4);
		float p = Permute(Permute(Permute(num4 + 0f) + num3 + 0f) + num2 + 0f);
		float p2 = Permute(Permute(Permute(num4 + num14) + num3 + num13) + num2 + num12);
		float p3 = Permute(Permute(Permute(num4 + num17) + num3 + num16) + num2 + num15);
		float p4 = Permute(Permute(Permute(num4 + 1f) + num3 + 1f) + num2 + 1f);
		float gx = 0f;
		float gy = 0f;
		float gz = 0f;
		float gx2 = 0f;
		float gy2 = 0f;
		float gz2 = 0f;
		float gx3 = 0f;
		float gy3 = 0f;
		float gz3 = 0f;
		float gx4 = 0f;
		float gy4 = 0f;
		float gz4 = 0f;
		Gradient(p, 0.2857143f, -13f / 14f, 1f / 7f, ref gx, ref gy, ref gz);
		Gradient(p2, 0.2857143f, -13f / 14f, 1f / 7f, ref gx2, ref gy2, ref gz2);
		Gradient(p3, 0.2857143f, -13f / 14f, 1f / 7f, ref gx3, ref gy3, ref gz3);
		Gradient(p4, 0.2857143f, -13f / 14f, 1f / 7f, ref gx4, ref gy4, ref gz4);
		float num27 = MathF.Max(0.6f - (num6 * num6 + num7 * num7 + num8 * num8), 0f);
		float num28 = MathF.Max(0.6f - (num18 * num18 + num19 * num19 + num20 * num20), 0f);
		float num29 = MathF.Max(0.6f - (num21 * num21 + num22 * num22 + num23 * num23), 0f);
		float num30 = MathF.Max(0.6f - (num24 * num24 + num25 * num25 + num26 * num26), 0f);
		float num31 = num27 * num27;
		float num32 = num31 * num31;
		float num33 = num28 * num28;
		float num34 = num33 * num33;
		float num35 = num29 * num29;
		float num36 = num35 * num35;
		float num37 = num30 * num30;
		float num38 = num37 * num37;
		float num39 = gx * num6 + gy * num7 + gz * num8;
		float num40 = gx2 * num18 + gy2 * num19 + gz2 * num20;
		float num41 = gx3 * num21 + gy3 * num22 + gz3 * num23;
		float num42 = gx4 * num24 + gy4 * num25 + gz4 * num26;
		float num43 = num31 * num27 * num39;
		float num44 = num33 * num28 * num40;
		float num45 = num35 * num29 * num41;
		float num46 = num37 * num30 * num42;
		float num47 = -8f * (num43 * num6 + num44 * num18 + num45 * num21 + num46 * num24) + num32 * gx + num34 * gx2 + num36 * gx3 + num38 * gx4;
		float num48 = -8f * (num43 * num7 + num44 * num19 + num45 * num22 + num46 * num25) + num32 * gy + num34 * gy2 + num36 * gy3 + num38 * gy4;
		float num49 = -8f * (num43 * num8 + num44 * num20 + num45 * num23 + num46 * num26) + num32 * gz + num34 * gz2 + num36 * gz3 + num38 * gz4;
		gradient = new float3(num47 * 42f, num48 * 42f, num49 * 42f);
		return 42f * (num32 * num39 + num34 * num40 + num36 * num41 + num38 * num42);
	}

	public static float snoise(float2 v)
	{
		float num = (v.x + v.y) * 0.36602542f;
		float num2 = MathF.Floor(v.x + num);
		float num3 = MathF.Floor(v.y + num);
		float num4 = (num2 + num3) * 0.21132487f;
		float num5 = v.x - num2 + num4;
		float num6 = v.y - num3 + num4;
		float num7;
		float num8;
		if (num5 > num6)
		{
			num7 = 1f;
			num8 = 0f;
		}
		else
		{
			num7 = 0f;
			num8 = 1f;
		}
		float num9 = num5 + 0.21132487f - num7;
		float num10 = num6 + 0.21132487f - num8;
		float num11 = num5 + -0.57735026f;
		float num12 = num6 + -0.57735026f;
		num2 = Mod289(num2);
		num3 = Mod289(num3);
		float num13 = Permute(Permute(num3 + 0f) + num2 + 0f);
		float num14 = Permute(Permute(num3 + num8) + num2 + num7);
		float num15 = Permute(Permute(num3 + 1f) + num2 + 1f);
		float num16 = MathF.Max(0.5f - (num5 * num5 + num6 * num6), 0f);
		float num17 = MathF.Max(0.5f - (num9 * num9 + num10 * num10), 0f);
		float num18 = MathF.Max(0.5f - (num11 * num11 + num12 * num12), 0f);
		num16 *= num16;
		num16 *= num16;
		num17 *= num17;
		num17 *= num17;
		num18 *= num18;
		num18 *= num18;
		float num19 = 2f * Frac(num13 * (1f / 41f)) - 1f;
		float num20 = 2f * Frac(num14 * (1f / 41f)) - 1f;
		float num21 = 2f * Frac(num15 * (1f / 41f)) - 1f;
		float num22 = MathF.Abs(num19) - 0.5f;
		float num23 = MathF.Abs(num20) - 0.5f;
		float num24 = MathF.Abs(num21) - 0.5f;
		float num25 = num19 - MathF.Floor(num19 + 0.5f);
		float num26 = num20 - MathF.Floor(num20 + 0.5f);
		float num27 = num21 - MathF.Floor(num21 + 0.5f);
		num16 *= 1.7928429f - 0.85373473f * (num25 * num25 + num22 * num22);
		num17 *= 1.7928429f - 0.85373473f * (num26 * num26 + num23 * num23);
		num18 *= 1.7928429f - 0.85373473f * (num27 * num27 + num24 * num24);
		float num28 = num25 * num5 + num22 * num6;
		float num29 = num26 * num9 + num23 * num10;
		float num30 = num27 * num11 + num24 * num12;
		return 130f * (num16 * num28 + num17 * num29 + num18 * num30);
	}

	private static float Mod289(float x)
	{
		return x - MathF.Floor(x * 0.0034602077f) * 289f;
	}

	private static float Permute(float x)
	{
		return Mod289((34f * x + 1f) * x);
	}

	private static float Frac(float x)
	{
		return x - MathF.Floor(x);
	}

	private static void Gradient(float p, float nsx, float nsy, float nsz, ref float gx, ref float gy, ref float gz)
	{
		float num = p - 49f * MathF.Floor(p * nsz * nsz);
		float num2 = MathF.Floor(num * nsz);
		float num3 = MathF.Floor(num - 7f * num2);
		float num4 = num2 * nsx + nsy;
		float num5 = num3 * nsx + nsy;
		float num6 = 1f - MathF.Abs(num4) - MathF.Abs(num5);
		float num7 = ((num6 <= 0f) ? (-1f) : 0f);
		float num8 = MathF.Floor(num4) * 2f + 1f;
		float num9 = MathF.Floor(num5) * 2f + 1f;
		gx = num4 + num8 * num7;
		gy = num5 + num9 * num7;
		gz = num6;
		float num10 = 1.7928429f - 0.85373473f * (gx * gx + gy * gy + gz * gz);
		gx *= num10;
		gy *= num10;
		gz *= num10;
	}
}
