using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002260 RID: 8800
	public class CompAbilityEffect_Reassure : CompAbilityEffect
	{
		// Token: 0x17001D4E RID: 7502
		// (get) Token: 0x0600BFDE RID: 49118 RVA: 0x00379A9D File Offset: 0x00377C9D
		public new CompProperties_AbilityReassure Props
		{
			get
			{
				return (CompProperties_AbilityReassure)this.props;
			}
		}

		// Token: 0x17001D4F RID: 7503
		// (get) Token: 0x0600BFDF RID: 49119 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool HideTargetPawnTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600BFE0 RID: 49120 RVA: 0x00379AAC File Offset: 0x00377CAC
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return pawn != null && AbilityUtility.ValidateMustBeHuman(pawn, throwMessages, this.parent) && AbilityUtility.ValidateNoMentalState(pawn, throwMessages, this.parent) && AbilityUtility.ValidateSameIdeo(this.parent.pawn, pawn, throwMessages, this.parent);
		}

		// Token: 0x0600BFE1 RID: 49121 RVA: 0x00379B05 File Offset: 0x00377D05
		private float CertaintyGain(Pawn initiator, Pawn recipient)
		{
			return this.Props.baseCertaintyGain * initiator.GetStatValue(StatDefOf.NegotiationAbility, true, -1);
		}

		// Token: 0x0600BFE2 RID: 49122 RVA: 0x00379B20 File Offset: 0x00377D20
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (!ModLister.CheckIdeology("Ideoligion certainty"))
			{
				return;
			}
			Pawn pawn = this.parent.pawn;
			Pawn pawn2 = target.Pawn;
			float certaintyGain = this.CertaintyGain(pawn, pawn2);
			float certainty = pawn2.ideo.Certainty;
			pawn2.ideo.Reassure(certaintyGain);
			Messages.Message(this.Props.successMessage.Formatted(pawn.Named("INITIATOR"), pawn2.Named("RECIPIENT"), certainty.ToStringPercent().Named("BEFORECERTAINTY"), pawn2.ideo.Certainty.ToStringPercent().Named("AFTERCERTAINTY"), pawn.Ideo.name.Named("IDEO")), new LookTargets(new Pawn[]
			{
				pawn,
				pawn2
			}), MessageTypeDefOf.PositiveEvent, true);
			PlayLogEntry_Interaction entry = new PlayLogEntry_Interaction(InteractionDefOf.Reassure, this.parent.pawn, pawn2, null);
			Find.PlayLog.Add(entry);
			if (this.Props.sound != null)
			{
				this.Props.sound.PlayOneShot(new TargetInfo(target.Cell, this.parent.pawn.Map, false));
			}
		}
	}
}
