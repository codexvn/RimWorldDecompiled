using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001956 RID: 6486
	public class CompProperties_CausesGameCondition_ClimateAdjuster : CompProperties_CausesGameCondition
	{
		// Token: 0x0600936E RID: 37742 RVA: 0x002B5B78 File Offset: 0x002B3D78
		public CompProperties_CausesGameCondition_ClimateAdjuster()
		{
			this.compClass = typeof(CompCauseGameCondition_TemperatureOffset);
		}

		// Token: 0x04005EDE RID: 24286
		public FloatRange temperatureOffsetRange = new FloatRange(-10f, 10f);
	}
}
