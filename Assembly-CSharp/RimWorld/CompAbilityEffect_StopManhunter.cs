using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200228E RID: 8846
	public class CompAbilityEffect_StopManhunter : CompAbilityEffect
	{
		// Token: 0x17001D70 RID: 7536
		// (get) Token: 0x0600C090 RID: 49296 RVA: 0x0037C468 File Offset: 0x0037A668
		public new CompProperties_StopManhunter Props
		{
			get
			{
				return (CompProperties_StopManhunter)this.props;
			}
		}

		// Token: 0x0600C091 RID: 49297 RVA: 0x0037C478 File Offset: 0x0037A678
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (!ModLister.CheckIdeology("Animal calm"))
			{
				return;
			}
			Pawn pawn = target.Pawn;
			pawn.MentalState.RecoverFromState();
			Messages.Message(this.Props.successMessage.Formatted(this.parent.pawn.Named("INITIATOR"), pawn.Named("RECIPIENT")), new LookTargets(new Pawn[]
			{
				this.parent.pawn,
				pawn
			}), MessageTypeDefOf.PositiveEvent, true);
		}

		// Token: 0x0600C092 RID: 49298 RVA: 0x0037C504 File Offset: 0x0037A704
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return pawn != null && AbilityUtility.ValidateMustBeAnimal(pawn, throwMessages, this.parent) && AbilityUtility.ValidateIsMaddened(pawn, throwMessages, this.parent) && AbilityUtility.ValidateIsAwake(pawn, throwMessages, this.parent);
		}

		// Token: 0x0600C093 RID: 49299 RVA: 0x0037C552 File Offset: 0x0037A752
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return AbilityUtility.ValidateIsMaddened(target.Pawn, false, this.parent) && base.CanApplyOn(target, dest);
		}
	}
}
