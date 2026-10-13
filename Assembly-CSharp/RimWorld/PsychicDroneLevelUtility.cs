using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BBB RID: 11195
	public static class PsychicDroneLevelUtility
	{
		// Token: 0x0600FA54 RID: 64084 RVA: 0x0048C1AC File Offset: 0x0048A3AC
		public static string GetLabel(this PsychicDroneLevel level)
		{
			switch (level)
			{
			case PsychicDroneLevel.None:
				return "PsychicDroneLevel_None".Translate();
			case PsychicDroneLevel.GoodMedium:
				return "PsychicDroneLevel_GoodMedium".Translate();
			case PsychicDroneLevel.BadLow:
				return "PsychicDroneLevel_BadLow".Translate();
			case PsychicDroneLevel.BadMedium:
				return "PsychicDroneLevel_BadMedium".Translate();
			case PsychicDroneLevel.BadHigh:
				return "PsychicDroneLevel_BadHigh".Translate();
			case PsychicDroneLevel.BadExtreme:
				return "PsychicDroneLevel_BadExtreme".Translate();
			default:
				return "error";
			}
		}

		// Token: 0x0600FA55 RID: 64085 RVA: 0x0048C23E File Offset: 0x0048A43E
		public static string GetLabelCap(this PsychicDroneLevel level)
		{
			return level.GetLabel().CapitalizeFirst();
		}
	}
}
