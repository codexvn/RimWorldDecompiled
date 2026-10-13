using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F30 RID: 12080
	public class CompProperties_TargetEffect_FleckOnTarget : CompProperties
	{
		// Token: 0x060110D0 RID: 69840 RVA: 0x004E68A4 File Offset: 0x004E4AA4
		public CompProperties_TargetEffect_FleckOnTarget()
		{
			this.compClass = typeof(ComTargetEffect_FleckOnTarget);
		}

		// Token: 0x0400B120 RID: 45344
		public FleckDef fleckDef;
	}
}
