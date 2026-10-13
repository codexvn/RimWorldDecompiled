using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F3A RID: 12090
	public class CompProperties_TargetEffect_MoteOnTarget : CompProperties
	{
		// Token: 0x060110E4 RID: 69860 RVA: 0x004E6B1C File Offset: 0x004E4D1C
		public CompProperties_TargetEffect_MoteOnTarget()
		{
			this.compClass = typeof(ComTargetEffect_MoteOnTarget);
		}

		// Token: 0x0400B128 RID: 45352
		public ThingDef moteDef;
	}
}
