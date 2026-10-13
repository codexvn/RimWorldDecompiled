using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200226A RID: 8810
	public class CompProperties_AbilityRemoveHediff : CompProperties_AbilityEffect
	{
		// Token: 0x0600C00B RID: 49163 RVA: 0x0037A5ED File Offset: 0x003787ED
		public CompProperties_AbilityRemoveHediff()
		{
			this.compClass = typeof(CompAbilityEffect_RemoveHediff);
		}

		// Token: 0x0400839E RID: 33694
		public HediffDef hediffDef;

		// Token: 0x0400839F RID: 33695
		public bool applyToSelf;

		// Token: 0x040083A0 RID: 33696
		public bool applyToTarget;
	}
}
