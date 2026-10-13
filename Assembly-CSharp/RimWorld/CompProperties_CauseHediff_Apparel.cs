using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C3F RID: 11327
	public class CompProperties_CauseHediff_Apparel : CompProperties
	{
		// Token: 0x0600FDFB RID: 65019 RVA: 0x0049AAE0 File Offset: 0x00498CE0
		public CompProperties_CauseHediff_Apparel()
		{
			this.compClass = typeof(CompCauseHediff_Apparel);
		}

		// Token: 0x0400A67F RID: 42623
		public HediffDef hediff;

		// Token: 0x0400A680 RID: 42624
		public BodyPartDef part;
	}
}
