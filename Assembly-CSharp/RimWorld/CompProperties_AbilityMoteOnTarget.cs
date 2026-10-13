using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200224B RID: 8779
	public class CompProperties_AbilityMoteOnTarget : CompProperties_AbilityEffect
	{
		// Token: 0x0600BFA0 RID: 49056 RVA: 0x00378BE5 File Offset: 0x00376DE5
		public CompProperties_AbilityMoteOnTarget()
		{
			this.compClass = typeof(CompAbilityEffect_MoteOnTarget);
		}

		// Token: 0x0400836F RID: 33647
		public ThingDef moteDef;

		// Token: 0x04008370 RID: 33648
		public List<ThingDef> moteDefs;

		// Token: 0x04008371 RID: 33649
		public float scale = 1f;

		// Token: 0x04008372 RID: 33650
		public int preCastTicks;
	}
}
