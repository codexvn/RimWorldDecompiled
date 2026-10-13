using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200227D RID: 8829
	public class CompAbilityEffect_SocialInteraction : CompAbilityEffect
	{
		// Token: 0x17001D66 RID: 7526
		// (get) Token: 0x0600C05E RID: 49246 RVA: 0x0037B706 File Offset: 0x00379906
		public new CompProperties_AbilitySocialInteraction Props
		{
			get
			{
				return (CompProperties_AbilitySocialInteraction)this.props;
			}
		}

		// Token: 0x0600C05F RID: 49247 RVA: 0x0037B714 File Offset: 0x00379914
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Pawn;
			if (pawn != null && this.parent.pawn != pawn)
			{
				Pawn_InteractionsTracker interactions = this.parent.pawn.interactions;
				if (interactions == null)
				{
					return;
				}
				interactions.TryInteractWith(pawn, this.Props.interactionDef);
			}
		}

		// Token: 0x0600C060 RID: 49248 RVA: 0x0037369F File Offset: 0x0037189F
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Valid(target, false);
		}

		// Token: 0x0600C061 RID: 49249 RVA: 0x0037B76C File Offset: 0x0037996C
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (!this.Props.canApplyToMentallyBroken && !AbilityUtility.ValidateNoMentalState(pawn, throwMessages, this.parent))
				{
					return false;
				}
				if (!this.Props.canApplyToUnconscious && !AbilityUtility.ValidateIsConscious(pawn, throwMessages, this.parent))
				{
					return false;
				}
			}
			return true;
		}
	}
}
