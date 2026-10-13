using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02002241 RID: 8769
	public class CompProperties_AbilityGiveRandomHediff : CompProperties_AbilityEffect
	{
		// Token: 0x0600BF7B RID: 49019 RVA: 0x003784E3 File Offset: 0x003766E3
		public CompProperties_AbilityGiveRandomHediff()
		{
			this.compClass = typeof(CompAbilityEffect_GiveRandomHediff);
		}

		// Token: 0x04008364 RID: 33636
		public List<HediffOption> options;

		// Token: 0x04008365 RID: 33637
		public bool allowDuplicates;
	}
}
