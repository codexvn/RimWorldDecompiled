using System;

namespace RimWorld
{
	// Token: 0x0200227C RID: 8828
	public class CompProperties_AbilitySocialInteraction : CompProperties_AbilityEffect
	{
		// Token: 0x0600C05D RID: 49245 RVA: 0x0037B6EE File Offset: 0x003798EE
		public CompProperties_AbilitySocialInteraction()
		{
			this.compClass = typeof(CompAbilityEffect_SocialInteraction);
		}

		// Token: 0x040083C7 RID: 33735
		public InteractionDef interactionDef;

		// Token: 0x040083C8 RID: 33736
		public bool canApplyToMentallyBroken;

		// Token: 0x040083C9 RID: 33737
		public bool canApplyToUnconscious;

		// Token: 0x040083CA RID: 33738
		public bool canApplyToAsleep;
	}
}
