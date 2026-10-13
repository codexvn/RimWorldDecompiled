using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200229A RID: 8858
	public class CompAbilityEffect_TransferEntropy : CompAbilityEffect
	{
		// Token: 0x17001D7C RID: 7548
		// (get) Token: 0x0600C0CA RID: 49354 RVA: 0x0037D3D2 File Offset: 0x0037B5D2
		public new CompProperties_AbilityTransferEntropy Props
		{
			get
			{
				return (CompProperties_AbilityTransferEntropy)this.props;
			}
		}

		// Token: 0x0600C0CB RID: 49355 RVA: 0x0037D3E0 File Offset: 0x0037B5E0
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				Pawn pawn2 = this.parent.pawn;
				if (this.Props.targetReceivesEntropy)
				{
					pawn.psychicEntropy.TryAddEntropy(pawn2.psychicEntropy.EntropyValue, pawn2, false, true);
				}
				pawn2.psychicEntropy.RemoveAllEntropy();
				MoteMaker.MakeInteractionOverlay(ThingDefOf.Mote_PsychicLinkPulse, this.parent.pawn, pawn);
				return;
			}
			Log.Error("CompAbilityEffect_TransferEntropy is only applicable to pawns.");
		}

		// Token: 0x0600C0CC RID: 49356 RVA: 0x0037D46A File Offset: 0x0037B66A
		public override bool GizmoDisabled(out string reason)
		{
			if (this.parent.pawn.psychicEntropy.EntropyValue <= 0f)
			{
				reason = "AbilityNoEntropyToDump".Translate();
				return true;
			}
			return base.GizmoDisabled(out reason);
		}

		// Token: 0x0600C0CD RID: 49357 RVA: 0x0037D4A4 File Offset: 0x0037B6A4
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return pawn == null || AbilityUtility.ValidateNoMentalState(pawn, throwMessages, this.parent);
		}
	}
}
