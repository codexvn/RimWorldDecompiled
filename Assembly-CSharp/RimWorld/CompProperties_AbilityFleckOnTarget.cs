using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002231 RID: 8753
	public class CompProperties_AbilityFleckOnTarget : CompProperties_AbilityEffect
	{
		// Token: 0x0600BF4B RID: 48971 RVA: 0x00377ADC File Offset: 0x00375CDC
		public CompProperties_AbilityFleckOnTarget()
		{
			this.compClass = typeof(CompAbilityEffect_FleckOnTarget);
		}

		// Token: 0x04008348 RID: 33608
		public FleckDef fleckDef;

		// Token: 0x04008349 RID: 33609
		public List<FleckDef> fleckDefs;

		// Token: 0x0400834A RID: 33610
		public float scale = 1f;

		// Token: 0x0400834B RID: 33611
		public int preCastTicks;
	}
}
