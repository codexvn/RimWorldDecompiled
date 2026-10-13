using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020027F7 RID: 10231
	public class CompProperties_RitualEffectStaticAreaRandomMote : CompProperties_RitualEffectIntervalSpawnArea
	{
		// Token: 0x0600DFE7 RID: 57319 RVA: 0x0041BF33 File Offset: 0x0041A133
		public CompProperties_RitualEffectStaticAreaRandomMote()
		{
			this.compClass = typeof(CompRitualEffect_StaticAreaRandomMote);
		}

		// Token: 0x040096AF RID: 38575
		public float minDist = 1.5f;

		// Token: 0x040096B0 RID: 38576
		public List<ThingDef> moteDefs;
	}
}
