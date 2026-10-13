using System;

namespace RimWorld.Planet
{
	// Token: 0x02003C5A RID: 15450
	public static class OverallPopulationUtility
	{
		// Token: 0x17003412 RID: 13330
		// (get) Token: 0x06014FCB RID: 85963 RVA: 0x006484E4 File Offset: 0x006466E4
		public static int EnumValuesCount
		{
			get
			{
				if (OverallPopulationUtility.cachedEnumValuesCount < 0)
				{
					OverallPopulationUtility.cachedEnumValuesCount = Enum.GetNames(typeof(OverallPopulation)).Length;
				}
				return OverallPopulationUtility.cachedEnumValuesCount;
			}
		}

		// Token: 0x06014FCC RID: 85964 RVA: 0x0064850C File Offset: 0x0064670C
		public static float GetScaleFactor(this OverallPopulation population)
		{
			switch (population)
			{
			case OverallPopulation.AlmostNone:
				return 0.1f;
			case OverallPopulation.Little:
				return 0.4f;
			case OverallPopulation.LittleBitLess:
				return 0.7f;
			case OverallPopulation.Normal:
				return 1f;
			case OverallPopulation.LittleBitMore:
				return 1.5f;
			case OverallPopulation.High:
				return 2f;
			case OverallPopulation.VeryHigh:
				return 2.75f;
			default:
				return 1f;
			}
		}

		// Token: 0x0400E9FB RID: 59899
		private static int cachedEnumValuesCount = -1;

		// Token: 0x0400E9FC RID: 59900
		private const float ScaleFactor_AlmostNone = 0.1f;

		// Token: 0x0400E9FD RID: 59901
		private const float ScaleFactor_Little = 0.4f;

		// Token: 0x0400E9FE RID: 59902
		private const float ScaleFactor_LittleBitLess = 0.7f;

		// Token: 0x0400E9FF RID: 59903
		private const float ScaleFactor_Normal = 1f;

		// Token: 0x0400EA00 RID: 59904
		private const float ScaleFactor_LittleBitMore = 1.5f;

		// Token: 0x0400EA01 RID: 59905
		private const float ScaleFactor_High = 2f;

		// Token: 0x0400EA02 RID: 59906
		private const float ScaleFactor_VeryHigh = 2.75f;
	}
}
