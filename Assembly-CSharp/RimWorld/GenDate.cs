using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200356E RID: 13678
	public static class GenDate
	{
		// Token: 0x170031B6 RID: 12726
		// (get) Token: 0x060132DB RID: 78555 RVA: 0x005B37DD File Offset: 0x005B19DD
		private static int TicksGame
		{
			get
			{
				return Find.TickManager.TicksGame;
			}
		}

		// Token: 0x170031B7 RID: 12727
		// (get) Token: 0x060132DC RID: 78556 RVA: 0x005B37E9 File Offset: 0x005B19E9
		private static int TicksSinceSettle
		{
			get
			{
				return Find.TickManager.TicksSinceSettle;
			}
		}

		// Token: 0x170031B8 RID: 12728
		// (get) Token: 0x060132DD RID: 78557 RVA: 0x005B37F5 File Offset: 0x005B19F5
		public static int DaysPassed
		{
			get
			{
				return GenDate.DaysPassedAt(GenDate.TicksGame);
			}
		}

		// Token: 0x170031B9 RID: 12729
		// (get) Token: 0x060132DE RID: 78558 RVA: 0x005B3801 File Offset: 0x005B1A01
		public static int DaysPassedSinceSettle
		{
			get
			{
				return GenDate.DaysPassedAt(GenDate.TicksSinceSettle);
			}
		}

		// Token: 0x170031BA RID: 12730
		// (get) Token: 0x060132DF RID: 78559 RVA: 0x005B380D File Offset: 0x005B1A0D
		public static float DaysPassedFloat
		{
			get
			{
				return (float)GenDate.TicksGame / 60000f;
			}
		}

		// Token: 0x170031BB RID: 12731
		// (get) Token: 0x060132E0 RID: 78560 RVA: 0x005B381B File Offset: 0x005B1A1B
		public static float DaysPassedSinceSettleFloat
		{
			get
			{
				return (float)GenDate.TicksSinceSettle / 60000f;
			}
		}

		// Token: 0x170031BC RID: 12732
		// (get) Token: 0x060132E1 RID: 78561 RVA: 0x005B3829 File Offset: 0x005B1A29
		public static int TwelfthsPassed
		{
			get
			{
				return GenDate.TwelfthsPassedAt(GenDate.TicksGame);
			}
		}

		// Token: 0x170031BD RID: 12733
		// (get) Token: 0x060132E2 RID: 78562 RVA: 0x005B3835 File Offset: 0x005B1A35
		public static float TwelfthsPassedFloat
		{
			get
			{
				return (float)GenDate.TicksGame / 300000f;
			}
		}

		// Token: 0x170031BE RID: 12734
		// (get) Token: 0x060132E3 RID: 78563 RVA: 0x005B3843 File Offset: 0x005B1A43
		public static int YearsPassed
		{
			get
			{
				return GenDate.YearsPassedAt(GenDate.TicksGame);
			}
		}

		// Token: 0x170031BF RID: 12735
		// (get) Token: 0x060132E4 RID: 78564 RVA: 0x005B384F File Offset: 0x005B1A4F
		public static float YearsPassedFloat
		{
			get
			{
				return (float)GenDate.TicksGame / 3600000f;
			}
		}

		// Token: 0x060132E5 RID: 78565 RVA: 0x005B385D File Offset: 0x005B1A5D
		public static int TickAbsToGame(int absTick)
		{
			return absTick - Find.TickManager.gameStartAbsTick;
		}

		// Token: 0x060132E6 RID: 78566 RVA: 0x005B386B File Offset: 0x005B1A6B
		public static int TickGameToAbs(int gameTick)
		{
			return gameTick + Find.TickManager.gameStartAbsTick;
		}

		// Token: 0x060132E7 RID: 78567 RVA: 0x005B3879 File Offset: 0x005B1A79
		public static int TickGameToSettled(int gameTick)
		{
			return gameTick - Find.TickManager.SettleTick;
		}

		// Token: 0x060132E8 RID: 78568 RVA: 0x005B3887 File Offset: 0x005B1A87
		public static int DaysPassedAt(int gameTicks)
		{
			return Mathf.FloorToInt((float)gameTicks / 60000f);
		}

		// Token: 0x060132E9 RID: 78569 RVA: 0x005B3896 File Offset: 0x005B1A96
		public static int TwelfthsPassedAt(int gameTicks)
		{
			return Mathf.FloorToInt((float)gameTicks / 300000f);
		}

		// Token: 0x060132EA RID: 78570 RVA: 0x005B38A5 File Offset: 0x005B1AA5
		public static int YearsPassedAt(int gameTicks)
		{
			return Mathf.FloorToInt((float)gameTicks / 3600000f);
		}

		// Token: 0x060132EB RID: 78571 RVA: 0x005B38B4 File Offset: 0x005B1AB4
		public static long LocalTicksOffsetFromLongitude(float longitude)
		{
			return (long)GenDate.TimeZoneAt(longitude) * 2500L;
		}

		// Token: 0x060132EC RID: 78572 RVA: 0x005B38C4 File Offset: 0x005B1AC4
		public static int HourOfDay(long absTicks, float longitude)
		{
			return GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 2500, 24);
		}

		// Token: 0x060132ED RID: 78573 RVA: 0x005B38DA File Offset: 0x005B1ADA
		public static int DayOfTwelfth(long absTicks, float longitude)
		{
			return GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 60000, 5);
		}

		// Token: 0x060132EE RID: 78574 RVA: 0x005B38EF File Offset: 0x005B1AEF
		public static int DayOfYear(long absTicks, float longitude)
		{
			return GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 60000, 60);
		}

		// Token: 0x060132EF RID: 78575 RVA: 0x005B3905 File Offset: 0x005B1B05
		public static Twelfth Twelfth(long absTicks, float longitude)
		{
			return (Twelfth)GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 300000, 12);
		}

		// Token: 0x060132F0 RID: 78576 RVA: 0x005B391C File Offset: 0x005B1B1C
		public static Season Season(long absTicks, Vector2 longLat)
		{
			return GenDate.Season(absTicks, longLat.y, longLat.x);
		}

		// Token: 0x060132F1 RID: 78577 RVA: 0x005B3930 File Offset: 0x005B1B30
		public static Season Season(long absTicks, float latitude, float longitude)
		{
			return SeasonUtility.GetReportedSeason(GenDate.YearPercent(absTicks, longitude), latitude);
		}

		// Token: 0x060132F2 RID: 78578 RVA: 0x005B393F File Offset: 0x005B1B3F
		public static Quadrum Quadrum(long absTicks, float longitude)
		{
			return GenDate.Twelfth(absTicks, longitude).GetQuadrum();
		}

		// Token: 0x060132F3 RID: 78579 RVA: 0x005B3950 File Offset: 0x005B1B50
		public static int Year(long absTicks, float longitude)
		{
			long num = absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude);
			return 5500 + Mathf.FloorToInt((float)num / 3600000f);
		}

		// Token: 0x060132F4 RID: 78580 RVA: 0x005B3979 File Offset: 0x005B1B79
		public static int DayOfSeason(long absTicks, float longitude)
		{
			return (GenDate.DayOfYear(absTicks, longitude) - (int)(SeasonUtility.FirstSeason.GetFirstTwelfth(0f) * RimWorld.Twelfth.Sixth)) % 15;
		}

		// Token: 0x060132F5 RID: 78581 RVA: 0x005B3997 File Offset: 0x005B1B97
		public static int DayOfQuadrum(long absTicks, float longitude)
		{
			return (GenDate.DayOfYear(absTicks, longitude) - (int)(QuadrumUtility.FirstQuadrum.GetFirstTwelfth() * RimWorld.Twelfth.Sixth)) % 15;
		}

		// Token: 0x060132F6 RID: 78582 RVA: 0x005B39B0 File Offset: 0x005B1BB0
		public static int DayTick(long absTicks, float longitude)
		{
			return (int)GenMath.PositiveMod(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 60000L);
		}

		// Token: 0x060132F7 RID: 78583 RVA: 0x005B39C8 File Offset: 0x005B1BC8
		public static float DayPercent(long absTicks, float longitude)
		{
			int num = GenDate.DayTick(absTicks, longitude);
			if (num == 0)
			{
				num = 1;
			}
			return (float)num / 60000f;
		}

		// Token: 0x060132F8 RID: 78584 RVA: 0x005B39EA File Offset: 0x005B1BEA
		public static float YearPercent(long absTicks, float longitude)
		{
			return (float)((int)GenMath.PositiveMod(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 3600000L)) / 3600000f;
		}

		// Token: 0x060132F9 RID: 78585 RVA: 0x005B38C4 File Offset: 0x005B1AC4
		public static int HourInteger(long absTicks, float longitude)
		{
			return GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 2500, 24);
		}

		// Token: 0x060132FA RID: 78586 RVA: 0x005B3A07 File Offset: 0x005B1C07
		public static float HourFloat(long absTicks, float longitude)
		{
			return GenDate.DayPercent(absTicks, longitude) * 24f;
		}

		// Token: 0x060132FB RID: 78587 RVA: 0x005B3A18 File Offset: 0x005B1C18
		public static string DateFullStringAt(long absTicks, Vector2 location)
		{
			int num = GenDate.DayOfSeason(absTicks, location.x) + 1;
			string value = Find.ActiveLanguageWorker.OrdinalNumber(num, Gender.None);
			return "FullDate".Translate(value, GenDate.Quadrum(absTicks, location.x).Label(), GenDate.Year(absTicks, location.x), num);
		}

		// Token: 0x060132FC RID: 78588 RVA: 0x005B3A83 File Offset: 0x005B1C83
		public static string DateMonthYearStringAt(long absTicks, Vector2 location)
		{
			return "MonthYearDate".Translate(GenDate.Quadrum(absTicks, location.x).Label(), GenDate.Year(absTicks, location.x));
		}

		// Token: 0x060132FD RID: 78589 RVA: 0x005B3ABC File Offset: 0x005B1CBC
		public static string DateFullStringWithHourAt(long absTicks, Vector2 location)
		{
			return GenDate.DateFullStringAt(absTicks, location) + ", " + GenDate.HourInteger(absTicks, location.x).ToString() + "LetterHour".Translate();
		}

		// Token: 0x060132FE RID: 78590 RVA: 0x005B3B04 File Offset: 0x005B1D04
		public static string DateReadoutStringAt(long absTicks, Vector2 location)
		{
			int num = GenDate.DayOfSeason(absTicks, location.x) + 1;
			string value = Find.ActiveLanguageWorker.OrdinalNumber(num, Gender.None);
			return "DateReadout".Translate(value, GenDate.Quadrum(absTicks, location.x).Label(), GenDate.Year(absTicks, location.x), num);
		}

		// Token: 0x060132FF RID: 78591 RVA: 0x005B3B70 File Offset: 0x005B1D70
		public static string DateShortStringAt(long absTicks, Vector2 location)
		{
			int value = GenDate.DayOfSeason(absTicks, location.x) + 1;
			return "ShortDate".Translate(value, GenDate.Quadrum(absTicks, location.x).LabelShort(), GenDate.Year(absTicks, location.x), value);
		}

		// Token: 0x06013300 RID: 78592 RVA: 0x005B3BD0 File Offset: 0x005B1DD0
		public static string SeasonDateStringAt(long absTicks, Vector2 longLat)
		{
			int num = GenDate.DayOfSeason(absTicks, longLat.x) + 1;
			string value = Find.ActiveLanguageWorker.OrdinalNumber(num, Gender.None);
			return "SeasonFullDate".Translate(value, GenDate.Season(absTicks, longLat).Label(), num);
		}

		// Token: 0x06013301 RID: 78593 RVA: 0x005B3C25 File Offset: 0x005B1E25
		public static string SeasonDateStringAt(Twelfth twelfth, Vector2 longLat)
		{
			return GenDate.SeasonDateStringAt((long)((int)twelfth * 300000 + 1), longLat);
		}

		// Token: 0x06013302 RID: 78594 RVA: 0x005B3C38 File Offset: 0x005B1E38
		public static string QuadrumDateStringAt(long absTicks, float longitude)
		{
			int num = GenDate.DayOfQuadrum(absTicks, longitude) + 1;
			string value = Find.ActiveLanguageWorker.OrdinalNumber(num, Gender.None);
			return "SeasonFullDate".Translate(value, GenDate.Quadrum(absTicks, longitude).Label(), num);
		}

		// Token: 0x06013303 RID: 78595 RVA: 0x005B3C88 File Offset: 0x005B1E88
		public static string QuadrumDateStringAt(Quadrum quadrum)
		{
			return GenDate.QuadrumDateStringAt((long)((int)quadrum * 900000 + 1), 0f);
		}

		// Token: 0x06013304 RID: 78596 RVA: 0x005B3C9E File Offset: 0x005B1E9E
		public static string QuadrumDateStringAt(Twelfth twelfth)
		{
			return GenDate.QuadrumDateStringAt((long)((int)twelfth * 300000 + 1), 0f);
		}

		// Token: 0x06013305 RID: 78597 RVA: 0x005B3CB4 File Offset: 0x005B1EB4
		public static float TicksToDays(this int numTicks)
		{
			return (float)numTicks / 60000f;
		}

		// Token: 0x06013306 RID: 78598 RVA: 0x005B3CBE File Offset: 0x005B1EBE
		public static int DaysToTicks(float days)
		{
			return Mathf.RoundToInt(days * 60000f);
		}

		// Token: 0x06013307 RID: 78599 RVA: 0x005B3CCC File Offset: 0x005B1ECC
		public static string ToStringTicksToDays(this int numTicks, string format = "F1")
		{
			string text = numTicks.TicksToDays().ToString(format);
			if (text == "1")
			{
				return "Period1Day".Translate();
			}
			return "PeriodDays".Translate(text);
		}

		// Token: 0x06013308 RID: 78600 RVA: 0x005B3D1C File Offset: 0x005B1F1C
		public static string ToStringTicksToPeriod(this int numTicks, bool allowSeconds = true, bool shortForm = false, bool canUseDecimals = true, bool allowYears = true, bool canUseDecimalsShortForm = false)
		{
			if (allowSeconds && numTicks < 2500 && (numTicks < 600 || Math.Round((double)((float)numTicks / 2500f), 1) == 0.0))
			{
				int num = Mathf.RoundToInt((float)numTicks / 60f);
				if (shortForm)
				{
					return num.ToString() + "LetterSecond".Translate();
				}
				if (num == 1)
				{
					return "Period1Second".Translate();
				}
				return "PeriodSeconds".Translate(num);
			}
			else if (numTicks < 60000)
			{
				if (shortForm)
				{
					if (canUseDecimals && canUseDecimalsShortForm)
					{
						return ((float)numTicks / 2500f).ToString("F1") + "LetterHour".Translate();
					}
					return Mathf.RoundToInt((float)numTicks / 2500f).ToString() + "LetterHour".Translate();
				}
				else if (numTicks < 2500)
				{
					string text = ((float)numTicks / 2500f).ToString("0.#");
					if (text == "1")
					{
						return "Period1Hour".Translate();
					}
					return "PeriodHours".Translate(text);
				}
				else
				{
					int num2 = Mathf.RoundToInt((float)numTicks / 2500f);
					if (num2 == 1)
					{
						return "Period1Hour".Translate();
					}
					return "PeriodHours".Translate(num2);
				}
			}
			else if (numTicks < 3600000 || !allowYears)
			{
				if (shortForm)
				{
					if (canUseDecimals && canUseDecimalsShortForm)
					{
						return ((float)numTicks / 60000f).ToString("F1") + "LetterDay".Translate();
					}
					return Mathf.RoundToInt((float)numTicks / 60000f).ToString() + "LetterDay".Translate();
				}
				else
				{
					string text2;
					if (canUseDecimals)
					{
						text2 = ((float)numTicks / 60000f).ToStringDecimalIfSmall();
					}
					else
					{
						text2 = Mathf.RoundToInt((float)numTicks / 60000f).ToString();
					}
					if (text2 == "1")
					{
						return "Period1Day".Translate();
					}
					return "PeriodDays".Translate(text2);
				}
			}
			else if (shortForm)
			{
				if (canUseDecimals && canUseDecimalsShortForm)
				{
					return ((float)numTicks / 3600000f).ToString("F1") + "LetterYear".Translate();
				}
				return Mathf.RoundToInt((float)numTicks / 3600000f).ToString() + "LetterYear".Translate();
			}
			else
			{
				string text3;
				if (canUseDecimals)
				{
					text3 = ((float)numTicks / 3600000f).ToStringDecimalIfSmall();
				}
				else
				{
					text3 = Mathf.RoundToInt((float)numTicks / 3600000f).ToString();
				}
				if (text3 == "1")
				{
					return "Period1Year".Translate();
				}
				return "PeriodYears".Translate(text3);
			}
		}

		// Token: 0x06013309 RID: 78601 RVA: 0x005B402C File Offset: 0x005B222C
		public static string ToStringTicksToPeriodVerbose(this int numTicks, bool allowHours = true, bool allowQuadrums = true)
		{
			if (numTicks < 0)
			{
				return "0";
			}
			int num;
			int num2;
			int num3;
			float num4;
			numTicks.TicksToPeriod(out num, out num2, out num3, out num4);
			if (!allowQuadrums)
			{
				num3 += 15 * num2;
				num2 = 0;
			}
			if (num > 0)
			{
				string text;
				if (num == 1)
				{
					text = "Period1Year".Translate();
				}
				else
				{
					text = "PeriodYears".Translate(num);
				}
				if (num2 > 0)
				{
					text += ", ";
					if (num2 == 1)
					{
						text += "Period1Quadrum".Translate();
					}
					else
					{
						text += "PeriodQuadrums".Translate(num2);
					}
				}
				return text;
			}
			if (num2 > 0)
			{
				string text2;
				if (num2 == 1)
				{
					text2 = "Period1Quadrum".Translate();
				}
				else
				{
					text2 = "PeriodQuadrums".Translate(num2);
				}
				if (num3 > 0)
				{
					text2 += ", ";
					if (num3 == 1)
					{
						text2 += "Period1Day".Translate();
					}
					else
					{
						text2 += "PeriodDays".Translate(num3);
					}
				}
				return text2;
			}
			if (num3 > 0)
			{
				string text3;
				if (num3 == 1)
				{
					text3 = "Period1Day".Translate();
				}
				else
				{
					text3 = "PeriodDays".Translate(num3);
				}
				int num5 = (int)num4;
				if (allowHours && num5 > 0)
				{
					text3 += ", ";
					if (num5 == 1)
					{
						text3 += "Period1Hour".Translate();
					}
					else
					{
						text3 += "PeriodHours".Translate(num5);
					}
				}
				return text3;
			}
			if (!allowHours)
			{
				return "PeriodDays".Translate(0);
			}
			if (num4 > 1f)
			{
				int num6 = Mathf.RoundToInt(num4);
				if (num6 == 1)
				{
					return "Period1Hour".Translate();
				}
				return "PeriodHours".Translate(num6);
			}
			else
			{
				if (Math.Round((double)num4, 1) == 1.0)
				{
					return "Period1Hour".Translate();
				}
				return "PeriodHours".Translate(num4.ToString("0.#"));
			}
		}

		// Token: 0x0601330A RID: 78602 RVA: 0x005B4290 File Offset: 0x005B2490
		public static string ToStringTicksToPeriodVague(this int numTicks, bool vagueMin = true, bool vagueMax = true)
		{
			if (vagueMax && numTicks > 36000000)
			{
				return "OverADecade".Translate();
			}
			if (vagueMin && numTicks < 60000)
			{
				return "LessThanADay".Translate();
			}
			return numTicks.ToStringTicksToPeriod(true, false, true, true, false);
		}

		// Token: 0x0601330B RID: 78603 RVA: 0x005B42DE File Offset: 0x005B24DE
		public static void TicksToPeriod(this int numTicks, out int years, out int quadrums, out int days, out float hoursFloat)
		{
			((long)numTicks).TicksToPeriod(out years, out quadrums, out days, out hoursFloat);
		}

		// Token: 0x0601330C RID: 78604 RVA: 0x005B42EC File Offset: 0x005B24EC
		public static void TicksToPeriod(this long numTicks, out int years, out int quadrums, out int days, out float hoursFloat)
		{
			if (numTicks < 0L)
			{
				Log.ErrorOnce("Tried to calculate period for negative ticks", 12841103);
			}
			years = (int)(numTicks / 3600000L);
			long num = numTicks - (long)years * 3600000L;
			quadrums = (int)(num / 900000L);
			num -= (long)quadrums * 900000L;
			days = (int)(num / 60000L);
			num -= (long)days * 60000L;
			hoursFloat = (float)num / 2500f;
		}

		// Token: 0x0601330D RID: 78605 RVA: 0x005B4364 File Offset: 0x005B2564
		public static string ToStringApproxAge(this float yearsFloat)
		{
			if (yearsFloat >= 1f)
			{
				return ((int)yearsFloat).ToStringCached();
			}
			int num;
			int num2;
			int num3;
			float num4;
			Mathf.Min((int)(yearsFloat * 3600000f), 3599999).TicksToPeriod(out num, out num2, out num3, out num4);
			if (num > 0)
			{
				if (num == 1)
				{
					return "Period1Year".Translate();
				}
				return "PeriodYears".Translate(num);
			}
			else if (num2 > 0)
			{
				if (num2 == 1)
				{
					return "Period1Quadrum".Translate();
				}
				return "PeriodQuadrums".Translate(num2);
			}
			else if (num3 > 0)
			{
				if (num3 == 1)
				{
					return "Period1Day".Translate();
				}
				return "PeriodDays".Translate(num3);
			}
			else
			{
				int num5 = (int)num4;
				if (num5 == 1)
				{
					return "Period1Hour".Translate();
				}
				return "PeriodHours".Translate(num5);
			}
		}

		// Token: 0x0601330E RID: 78606 RVA: 0x005B4459 File Offset: 0x005B2659
		public static int TimeZoneAt(float longitude)
		{
			return Mathf.RoundToInt(GenDate.TimeZoneFloatAt(longitude));
		}

		// Token: 0x0601330F RID: 78607 RVA: 0x005B4466 File Offset: 0x005B2666
		public static float TimeZoneFloatAt(float longitude)
		{
			return longitude / 15f;
		}

		// Token: 0x0400C708 RID: 50952
		public const int TicksPerDay = 60000;

		// Token: 0x0400C709 RID: 50953
		public const int HoursPerDay = 24;

		// Token: 0x0400C70A RID: 50954
		public const int DaysPerTwelfth = 5;

		// Token: 0x0400C70B RID: 50955
		public const int TwelfthsPerYear = 12;

		// Token: 0x0400C70C RID: 50956
		public const int GameStartHourOfDay = 6;

		// Token: 0x0400C70D RID: 50957
		public const int TicksPerTwelfth = 300000;

		// Token: 0x0400C70E RID: 50958
		public const int TicksPerSeason = 900000;

		// Token: 0x0400C70F RID: 50959
		public const int TicksPerQuadrum = 900000;

		// Token: 0x0400C710 RID: 50960
		public const int TicksPerYear = 3600000;

		// Token: 0x0400C711 RID: 50961
		public const int DaysPerYear = 60;

		// Token: 0x0400C712 RID: 50962
		public const int DaysPerSeason = 15;

		// Token: 0x0400C713 RID: 50963
		public const int DaysPerQuadrum = 15;

		// Token: 0x0400C714 RID: 50964
		public const int TicksPerHour = 2500;

		// Token: 0x0400C715 RID: 50965
		public const float TimeZoneWidth = 15f;

		// Token: 0x0400C716 RID: 50966
		public const int DefaultStartingYear = 5500;
	}
}
