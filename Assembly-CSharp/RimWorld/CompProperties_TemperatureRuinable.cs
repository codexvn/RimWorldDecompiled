using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E80 RID: 11904
	public class CompProperties_TemperatureRuinable : CompProperties
	{
		// Token: 0x06010C28 RID: 68648 RVA: 0x004D4563 File Offset: 0x004D2763
		public CompProperties_TemperatureRuinable()
		{
			this.compClass = typeof(CompTemperatureRuinable);
		}

		// Token: 0x0400AE9D RID: 44701
		public float minSafeTemperature;

		// Token: 0x0400AE9E RID: 44702
		public float maxSafeTemperature = 100f;

		// Token: 0x0400AE9F RID: 44703
		public float progressPerDegreePerTick = 1E-05f;
	}
}
