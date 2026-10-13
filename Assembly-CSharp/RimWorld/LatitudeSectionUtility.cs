using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003571 RID: 13681
	public static class LatitudeSectionUtility
	{
		// Token: 0x0601333A RID: 78650 RVA: 0x005B4808 File Offset: 0x005B2A08
		public static LatitudeSection GetReportedLatitudeSection(float latitude)
		{
			float num;
			float num2;
			float num3;
			LatitudeSectionUtility.GetLatitudeSection(latitude, out num, out num2, out num3);
			if (num == 0f && num2 == 0f && num3 == 0f)
			{
				return LatitudeSection.Undefined;
			}
			if (num == 1f)
			{
				return LatitudeSection.Equatorial;
			}
			if (num3 == 1f)
			{
				return LatitudeSection.Polar;
			}
			return LatitudeSection.Seasonal;
		}

		// Token: 0x0601333B RID: 78651 RVA: 0x005B4850 File Offset: 0x005B2A50
		public static LatitudeSection GetDominantLatitudeSection(float latitude)
		{
			float num;
			float num2;
			float num3;
			LatitudeSectionUtility.GetLatitudeSection(latitude, out num, out num2, out num3);
			if (num == 0f && num2 == 0f && num3 == 0f)
			{
				return LatitudeSection.Undefined;
			}
			return GenMath.MaxBy<LatitudeSection>(LatitudeSection.Equatorial, num, LatitudeSection.Seasonal, num2, LatitudeSection.Polar, num3);
		}

		// Token: 0x0601333C RID: 78652 RVA: 0x005B4890 File Offset: 0x005B2A90
		public static void GetLatitudeSection(float latitude, out float equatorial, out float seasonal, out float polar)
		{
			float num = Mathf.Abs(latitude);
			float maxLatitude = LatitudeSection.Equatorial.GetMaxLatitude();
			float maxLatitude2 = LatitudeSection.Seasonal.GetMaxLatitude();
			float maxLatitude3 = LatitudeSection.Polar.GetMaxLatitude();
			if (num <= maxLatitude)
			{
				equatorial = 1f;
				seasonal = 0f;
				polar = 0f;
				return;
			}
			if (num <= maxLatitude2)
			{
				equatorial = Mathf.InverseLerp(maxLatitude + 5f, maxLatitude, num);
				float a = 1f - equatorial;
				polar = Mathf.InverseLerp(maxLatitude2 - 5f, maxLatitude2, num);
				float b = 1f - polar;
				seasonal = Mathf.Min(a, b);
				GenMath.NormalizeToSum1(ref equatorial, ref seasonal, ref polar);
				return;
			}
			if (num <= maxLatitude3)
			{
				equatorial = 0f;
				seasonal = 0f;
				polar = 1f;
				return;
			}
			equatorial = 0f;
			seasonal = 0f;
			polar = 0f;
		}

		// Token: 0x0601333D RID: 78653 RVA: 0x005B4950 File Offset: 0x005B2B50
		public static float GetMaxLatitude(this LatitudeSection latitudeSection)
		{
			switch (Find.World.info.overallTemperature)
			{
			case OverallTemperature.VeryCold:
				switch (latitudeSection)
				{
				case LatitudeSection.Equatorial:
					return -999f;
				case LatitudeSection.Seasonal:
					return -999f;
				case LatitudeSection.Polar:
					return 999f;
				}
				break;
			case OverallTemperature.Cold:
				switch (latitudeSection)
				{
				case LatitudeSection.Equatorial:
					return -999f;
				case LatitudeSection.Seasonal:
					return 15f;
				case LatitudeSection.Polar:
					return 999f;
				}
				break;
			case OverallTemperature.LittleBitColder:
				switch (latitudeSection)
				{
				case LatitudeSection.Equatorial:
					return -999f;
				case LatitudeSection.Seasonal:
					return 40f;
				case LatitudeSection.Polar:
					return 999f;
				}
				break;
			case OverallTemperature.Normal:
				switch (latitudeSection)
				{
				case LatitudeSection.Equatorial:
					return 15f;
				case LatitudeSection.Seasonal:
					return 75f;
				case LatitudeSection.Polar:
					return 999f;
				}
				break;
			case OverallTemperature.LittleBitWarmer:
				switch (latitudeSection)
				{
				case LatitudeSection.Equatorial:
					return 35f;
				case LatitudeSection.Seasonal:
					return 999f;
				case LatitudeSection.Polar:
					return 999f;
				}
				break;
			case OverallTemperature.Hot:
				switch (latitudeSection)
				{
				case LatitudeSection.Equatorial:
					return 65f;
				case LatitudeSection.Seasonal:
					return 999f;
				case LatitudeSection.Polar:
					return 999f;
				}
				break;
			case OverallTemperature.VeryHot:
				switch (latitudeSection)
				{
				case LatitudeSection.Equatorial:
					return 999f;
				case LatitudeSection.Seasonal:
					return 999f;
				case LatitudeSection.Polar:
					return 999f;
				}
				break;
			}
			return -1f;
		}

		// Token: 0x0400C71C RID: 50972
		private const float LerpDistance = 5f;
	}
}
