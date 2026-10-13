using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020027F4 RID: 10228
	public class CompProperties_RitualEffectIntervalSpawnArea : CompProperties_RitualEffectIntervalSpawn
	{
		// Token: 0x0600DFE1 RID: 57313 RVA: 0x0041BE11 File Offset: 0x0041A011
		public CompProperties_RitualEffectIntervalSpawnArea()
		{
			this.compClass = typeof(CompRitualEffect_IntervalSpawnArea);
		}

		// Token: 0x040096AC RID: 38572
		public IntVec2 area;

		// Token: 0x040096AD RID: 38573
		public bool smoothEdges = true;
	}
}
