using System;

namespace RimWorld.Planet
{
	// Token: 0x02003D13 RID: 15635
	public static class LandmarkDensityUtility
	{
		// Token: 0x1700350A RID: 13578
		// (get) Token: 0x0601547D RID: 87165 RVA: 0x0065DC52 File Offset: 0x0065BE52
		public static int EnumValuesCount
		{
			get
			{
				if (LandmarkDensityUtility.cachedEnumValuesCount < 0)
				{
					LandmarkDensityUtility.cachedEnumValuesCount = Enum.GetNames(typeof(LandmarkDensity)).Length;
				}
				return LandmarkDensityUtility.cachedEnumValuesCount;
			}
		}

		// Token: 0x0601547E RID: 87166 RVA: 0x0065DC78 File Offset: 0x0065BE78
		public static float GetScaleFactor(this LandmarkDensity density)
		{
			switch (density)
			{
			case LandmarkDensity.Sparse:
				return 0.05f;
			case LandmarkDensity.SlightlyMoreSparse:
				return 0.33f;
			case LandmarkDensity.SlightlySparse:
				return 0.66f;
			case LandmarkDensity.Normal:
				return 1f;
			case LandmarkDensity.SlightlyCrowded:
				return 1.5f;
			case LandmarkDensity.SlightlyMoreCrowded:
				return 2f;
			case LandmarkDensity.Crowded:
				return 3f;
			default:
				return 1f;
			}
		}

		// Token: 0x0400ED47 RID: 60743
		private static int cachedEnumValuesCount = -1;

		// Token: 0x0400ED48 RID: 60744
		private const float ScaleFactor_Sparse = 0.05f;

		// Token: 0x0400ED49 RID: 60745
		private const float ScaleFactor_SlightlyMoreSparse = 0.33f;

		// Token: 0x0400ED4A RID: 60746
		private const float ScaleFactor_SlightlySparse = 0.66f;

		// Token: 0x0400ED4B RID: 60747
		private const float ScaleFactor_Normal = 1f;

		// Token: 0x0400ED4C RID: 60748
		private const float ScaleFactor_SlightlyCrowded = 1.5f;

		// Token: 0x0400ED4D RID: 60749
		private const float ScaleFactor_SlightlyMoreCrowded = 2f;

		// Token: 0x0400ED4E RID: 60750
		private const float ScaleFactor_Crowded = 3f;
	}
}
