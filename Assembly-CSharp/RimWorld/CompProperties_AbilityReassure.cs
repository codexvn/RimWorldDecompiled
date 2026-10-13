using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200225F RID: 8799
	public class CompProperties_AbilityReassure : CompProperties_AbilityEffect
	{
		// Token: 0x0600BFDD RID: 49117 RVA: 0x00379A7A File Offset: 0x00377C7A
		public CompProperties_AbilityReassure()
		{
			this.compClass = typeof(CompAbilityEffect_Reassure);
		}

		// Token: 0x04008389 RID: 33673
		[MustTranslate]
		public string successMessage;

		// Token: 0x0400838A RID: 33674
		public float baseCertaintyGain = 0.075f;
	}
}
