using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020027FA RID: 10234
	public class CompProperties_RitualEffectIntervalSpawnCircle : CompProperties_RitualEffectIntervalSpawn
	{
		// Token: 0x0600DFEF RID: 57327 RVA: 0x0041C0C7 File Offset: 0x0041A2C7
		public CompProperties_RitualEffectIntervalSpawnCircle()
		{
			this.compClass = typeof(CompRitualEffect_IntervalSpawnCircle);
		}

		// Token: 0x040096B4 RID: 38580
		public IntVec2 area;

		// Token: 0x040096B5 RID: 38581
		public float radius = 5f;

		// Token: 0x040096B6 RID: 38582
		public float concentration = 1f;
	}
}
