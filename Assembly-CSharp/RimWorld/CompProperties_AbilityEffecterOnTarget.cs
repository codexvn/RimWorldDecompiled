using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002219 RID: 8729
	public class CompProperties_AbilityEffecterOnTarget : CompProperties_AbilityEffect
	{
		// Token: 0x0600BEE3 RID: 48867 RVA: 0x00375B39 File Offset: 0x00373D39
		public CompProperties_AbilityEffecterOnTarget()
		{
			this.compClass = typeof(CompAbilityEffect_EffecterOnTarget);
		}

		// Token: 0x0400830A RID: 33546
		public EffecterDef effecterDef;

		// Token: 0x0400830B RID: 33547
		public int maintainForTicks = -1;

		// Token: 0x0400830C RID: 33548
		public float scale = 1f;
	}
}
