using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F3E RID: 12094
	public class CompProperties_TargetEffectResurrect : CompProperties
	{
		// Token: 0x060110EE RID: 69870 RVA: 0x004E6CA7 File Offset: 0x004E4EA7
		public CompProperties_TargetEffectResurrect()
		{
			this.compClass = typeof(CompTargetEffect_Resurrect);
		}

		// Token: 0x0400B12A RID: 45354
		public ThingDef moteDef;

		// Token: 0x0400B12B RID: 45355
		public bool withSideEffects = true;

		// Token: 0x0400B12C RID: 45356
		public HediffDef addsHediff;
	}
}
