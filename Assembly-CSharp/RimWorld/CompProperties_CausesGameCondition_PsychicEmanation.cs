using System;

namespace RimWorld
{
	// Token: 0x02002BBC RID: 11196
	public class CompProperties_CausesGameCondition_PsychicEmanation : CompProperties_CausesGameCondition
	{
		// Token: 0x0600FA56 RID: 64086 RVA: 0x0048C24B File Offset: 0x0048A44B
		public CompProperties_CausesGameCondition_PsychicEmanation()
		{
			this.compClass = typeof(CompCauseGameCondition_PsychicEmanation);
		}

		// Token: 0x0400A481 RID: 42113
		public PsychicDroneLevel droneLevel = PsychicDroneLevel.BadMedium;

		// Token: 0x0400A482 RID: 42114
		public int droneLevelIncreaseInterval = int.MinValue;
	}
}
