using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D42 RID: 7490
	public static class HateChantDroneLevelUtility
	{
		// Token: 0x0600A7CF RID: 42959 RVA: 0x00301020 File Offset: 0x002FF220
		public static string GetLabel(this HateChantDroneLevel level)
		{
			switch (level)
			{
			case HateChantDroneLevel.None:
				return "HateChantDroneLevel_None".Translate();
			case HateChantDroneLevel.VeryLow:
				return "HateChantDroneLevel_VeryLow".Translate();
			case HateChantDroneLevel.Low:
				return "HateChantDroneLevel_Low".Translate();
			case HateChantDroneLevel.Medium:
				return "HateChantDroneLevel_Medium".Translate();
			case HateChantDroneLevel.High:
				return "HateChantDroneLevel_High".Translate();
			case HateChantDroneLevel.Extreme:
				return "HateChantDroneLevel_Extreme".Translate();
			default:
				return "error";
			}
		}

		// Token: 0x0600A7D0 RID: 42960 RVA: 0x003010B2 File Offset: 0x002FF2B2
		public static string GetLabelCap(this HateChantDroneLevel level)
		{
			return level.GetLabel().CapitalizeFirst();
		}
	}
}
