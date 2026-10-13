using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002201 RID: 8705
	public class CompProperties_AbilityCoagulate : CompProperties_AbilityEffect
	{
		// Token: 0x0600BE9A RID: 48794 RVA: 0x003742D3 File Offset: 0x003724D3
		public CompProperties_AbilityCoagulate()
		{
			this.compClass = typeof(CompAbilityEffect_Coagulate);
		}

		// Token: 0x040082E8 RID: 33512
		public FloatRange tendQualityRange;
	}
}
