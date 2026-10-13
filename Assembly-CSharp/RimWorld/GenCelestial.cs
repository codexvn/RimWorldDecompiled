using System;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200356B RID: 13675
	public static class GenCelestial
	{
		// Token: 0x170031B5 RID: 12725
		// (get) Token: 0x060132CB RID: 78539 RVA: 0x005B30E4 File Offset: 0x005B12E4
		private static int TicksAbsForSunPosInWorldSpace
		{
			get
			{
				if (Current.ProgramState != ProgramState.Entry)
				{
					return GenTicks.TicksAbs;
				}
				PlanetTile startingTile = Find.GameInitData.startingTile;
				float longitude = startingTile.Valid ? Find.WorldGrid.LongLatOf(startingTile).x : 0f;
				return Mathf.RoundToInt(2500f * (12f - GenDate.TimeZoneFloatAt(longitude)));
			}
		}

		// Token: 0x060132CC RID: 78540 RVA: 0x005B3141 File Offset: 0x005B1341
		public static float CurCelestialSunGlow(Map map)
		{
			return GenCelestial.CelestialSunGlow(map, Find.TickManager.TicksAbs);
		}

		// Token: 0x060132CD RID: 78541 RVA: 0x005B3153 File Offset: 0x005B1353
		public static float CelestialSunGlow(Map map, int ticksAbs)
		{
			return GenCelestial.CelestialSunGlow(map.Tile, ticksAbs);
		}

		// Token: 0x060132CE RID: 78542 RVA: 0x005B3164 File Offset: 0x005B1364
		public static float CelestialSunGlow(PlanetTile tile, int ticksAbs)
		{
			Vector2 vector = Find.WorldGrid.LongLatOf(tile);
			return GenCelestial.CelestialSunGlowPercent(vector.y, GenDate.DayOfYear((long)ticksAbs, vector.x), GenDate.DayPercent((long)ticksAbs, vector.x));
		}

		// Token: 0x060132CF RID: 78543 RVA: 0x005B31A2 File Offset: 0x005B13A2
		public static float CurShadowStrength(Map map)
		{
			return Mathf.Clamp01(Mathf.Abs(GenCelestial.CurCelestialSunGlow(map) - 0.6f) / 0.15f);
		}

		// Token: 0x060132D0 RID: 78544 RVA: 0x005B31C0 File Offset: 0x005B13C0
		public static GenCelestial.LightInfo GetLightSourceInfo(Map map, GenCelestial.LightType type)
		{
			float num = GenLocalDate.DayPercent(map);
			bool flag;
			float intensity;
			if (type == GenCelestial.LightType.Shadow)
			{
				flag = GenCelestial.IsDaytime(GenCelestial.CurCelestialSunGlow(map));
				intensity = GenCelestial.CurShadowStrength(map);
			}
			else if (type == GenCelestial.LightType.LightingSun)
			{
				flag = true;
				intensity = Mathf.Clamp01((GenCelestial.CurCelestialSunGlow(map) - 0.6f + 0.2f) / 0.15f);
			}
			else if (type == GenCelestial.LightType.LightingMoon)
			{
				flag = false;
				intensity = Mathf.Clamp01(-(GenCelestial.CurCelestialSunGlow(map) - 0.6f - 0.2f) / 0.15f);
			}
			else
			{
				Log.ErrorOnce("Invalid light type requested", 64275614);
				flag = true;
				intensity = 0f;
			}
			float t;
			float num2;
			float num3;
			if (flag)
			{
				t = num;
				num2 = -1.5f;
				num3 = 15f;
			}
			else
			{
				if (num > 0.5f)
				{
					t = Mathf.InverseLerp(0.5f, 1f, num) * 0.5f;
				}
				else
				{
					t = 0.5f + Mathf.InverseLerp(0f, 0.5f, num) * 0.5f;
				}
				num2 = -0.9f;
				num3 = 15f;
			}
			float num4 = Mathf.LerpUnclamped(-num3, num3, t);
			float y = num2 - 2.5f * (num4 * num4 / 100f);
			return new GenCelestial.LightInfo
			{
				vector = new Vector2(num4, y),
				intensity = intensity
			};
		}

		// Token: 0x060132D1 RID: 78545 RVA: 0x005B32F8 File Offset: 0x005B14F8
		public static Vector3 CurSunPositionInWorldSpace()
		{
			int ticksAbsForSunPosInWorldSpace = GenCelestial.TicksAbsForSunPosInWorldSpace;
			return GenCelestial.SunPositionUnmodified((float)GenDate.DayOfYear((long)ticksAbsForSunPosInWorldSpace, 0f), GenDate.DayPercent((long)ticksAbsForSunPosInWorldSpace, 0f), new Vector3(0f, 0f, -1f), 0f);
		}

		// Token: 0x060132D2 RID: 78546 RVA: 0x005B3342 File Offset: 0x005B1542
		public static bool IsDaytime(float glow)
		{
			return glow > 0.6f;
		}

		// Token: 0x060132D3 RID: 78547 RVA: 0x005B334C File Offset: 0x005B154C
		private static Vector3 SunPosition(float latitude, int dayOfYear, float dayPercent)
		{
			Vector3 target = GenCelestial.SurfaceNormal(latitude);
			Vector3 current = GenCelestial.SunPositionUnmodified((float)dayOfYear, dayPercent, new Vector3(1f, 0f, 0f), latitude);
			float num = GenCelestial.SunPeekAroundDegreesFactorCurve.Evaluate(latitude);
			current = Vector3.RotateTowards(current, target, 0.33161256f * num, 9999999f);
			float num2 = Mathf.InverseLerp(60f, 0f, Mathf.Abs(latitude));
			if (num2 > 0f)
			{
				current = Vector3.RotateTowards(current, target, 6.2831855f * (17f * num2 / 360f), 9999999f);
			}
			return current.normalized;
		}

		// Token: 0x060132D4 RID: 78548 RVA: 0x005B33E4 File Offset: 0x005B15E4
		private static Vector3 SunPositionUnmodified(float dayOfYear, float dayPercent, Vector3 initialSunPos, float latitude = 0f)
		{
			Vector3 point = initialSunPos * 100f;
			float num = -Mathf.Cos(dayOfYear / 60f * 3.1415927f * 2f);
			point.y += num * 100f * GenCelestial.SunOffsetFractionFromLatitudeCurve.Evaluate(latitude);
			point = Quaternion.AngleAxis((dayPercent - 0.5f) * 360f, Vector3.up) * point;
			return point.normalized;
		}

		// Token: 0x060132D5 RID: 78549 RVA: 0x005B345C File Offset: 0x005B165C
		private static float CelestialSunGlowPercent(float latitude, int dayOfYear, float dayPercent)
		{
			Vector3 vector = GenCelestial.SurfaceNormal(latitude);
			Vector3 rhs = GenCelestial.SunPosition(latitude, dayOfYear, dayPercent);
			float value = Vector3.Dot(vector.normalized, rhs);
			return Mathf.Clamp01(Mathf.InverseLerp(0f, 0.7f, value));
		}

		// Token: 0x060132D6 RID: 78550 RVA: 0x005B349C File Offset: 0x005B169C
		public static float AverageGlow(float latitude, int dayOfYear)
		{
			float num = 0f;
			for (int i = 0; i < 24; i++)
			{
				num += GenCelestial.CelestialSunGlowPercent(latitude, dayOfYear, (float)i / 24f);
			}
			return num / 24f;
		}

		// Token: 0x060132D7 RID: 78551 RVA: 0x005B34D8 File Offset: 0x005B16D8
		private static Vector3 SurfaceNormal(float latitude)
		{
			Vector3 vector = new Vector3(1f, 0f, 0f);
			vector = Quaternion.AngleAxis(latitude, new Vector3(0f, 0f, 1f)) * vector;
			return vector;
		}

		// Token: 0x060132D8 RID: 78552 RVA: 0x005B3520 File Offset: 0x005B1720
		public static void LogSunGlowForYear()
		{
			for (int i = -90; i <= 90; i += 10)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Sun visibility percents for latitude " + i.ToString() + ", for each hour of each day of the year");
				stringBuilder.AppendLine("---------------------------------------");
				stringBuilder.Append("Day/hr".PadRight(6));
				for (int j = 0; j < 24; j += 2)
				{
					stringBuilder.Append((j.ToString() + "h").PadRight(6));
				}
				stringBuilder.AppendLine();
				for (int k = 0; k < 60; k += 5)
				{
					stringBuilder.Append(k.ToString().PadRight(6));
					for (int l = 0; l < 24; l += 2)
					{
						stringBuilder.Append(GenCelestial.CelestialSunGlowPercent((float)i, k, (float)l / 24f).ToString("F2").PadRight(6));
					}
					stringBuilder.AppendLine();
				}
				Log.Message(stringBuilder.ToString());
			}
		}

		// Token: 0x060132D9 RID: 78553 RVA: 0x005B3628 File Offset: 0x005B1828
		public static void LogSunAngleForYear()
		{
			for (int i = -90; i <= 90; i += 10)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Sun angles for latitude " + i.ToString() + ", for each hour of each day of the year");
				stringBuilder.AppendLine("---------------------------------------");
				stringBuilder.Append("Day/hr".PadRight(6));
				for (int j = 0; j < 24; j += 2)
				{
					stringBuilder.Append((j.ToString() + "h").PadRight(6));
				}
				stringBuilder.AppendLine();
				for (int k = 0; k < 60; k += 5)
				{
					stringBuilder.Append(k.ToString().PadRight(6));
					for (int l = 0; l < 24; l += 2)
					{
						float num = Vector3.Angle(GenCelestial.SurfaceNormal((float)i), GenCelestial.SunPositionUnmodified((float)k, (float)l / 24f, new Vector3(1f, 0f, 0f), 0f));
						stringBuilder.Append((90f - num).ToString("F0").PadRight(6));
					}
					stringBuilder.AppendLine();
				}
				Log.Message(stringBuilder.ToString());
			}
		}

		// Token: 0x0400C6FC RID: 50940
		public const float ShadowMaxLengthDay = 15f;

		// Token: 0x0400C6FD RID: 50941
		public const float ShadowMaxLengthNight = 15f;

		// Token: 0x0400C6FE RID: 50942
		private const float ShadowGlowLerpSpan = 0.15f;

		// Token: 0x0400C6FF RID: 50943
		private const float ShadowDayNightThreshold = 0.6f;

		// Token: 0x0400C700 RID: 50944
		private static SimpleCurve SunPeekAroundDegreesFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(70f, 1f),
				true
			},
			{
				new CurvePoint(75f, 0.05f),
				true
			}
		};

		// Token: 0x0400C701 RID: 50945
		private static SimpleCurve SunOffsetFractionFromLatitudeCurve = new SimpleCurve
		{
			{
				new CurvePoint(70f, 0.2f),
				true
			},
			{
				new CurvePoint(75f, 1.5f),
				true
			}
		};

		// Token: 0x0200356C RID: 13676
		public struct LightInfo
		{
			// Token: 0x0400C702 RID: 50946
			public Vector2 vector;

			// Token: 0x0400C703 RID: 50947
			public float intensity;
		}

		// Token: 0x0200356D RID: 13677
		public enum LightType
		{
			// Token: 0x0400C705 RID: 50949
			Shadow,
			// Token: 0x0400C706 RID: 50950
			LightingSun,
			// Token: 0x0400C707 RID: 50951
			LightingMoon
		}
	}
}
