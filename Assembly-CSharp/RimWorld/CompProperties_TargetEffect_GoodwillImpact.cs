using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F32 RID: 12082
	public class CompProperties_TargetEffect_GoodwillImpact : CompProperties
	{
		// Token: 0x060110D4 RID: 69844 RVA: 0x004E68F8 File Offset: 0x004E4AF8
		public CompProperties_TargetEffect_GoodwillImpact()
		{
			this.compClass = typeof(CompTargetEffect_GoodwillImpact);
		}

		// Token: 0x0400B121 RID: 45345
		public int goodwillImpact = -200;
	}
}
