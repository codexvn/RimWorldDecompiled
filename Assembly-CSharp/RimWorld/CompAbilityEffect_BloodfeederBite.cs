using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020021F7 RID: 8695
	public class CompAbilityEffect_BloodfeederBite : CompAbilityEffect
	{
		// Token: 0x17001D0E RID: 7438
		// (get) Token: 0x0600BE6F RID: 48751 RVA: 0x00373613 File Offset: 0x00371813
		public new CompProperties_AbilityBloodfeederBite Props
		{
			get
			{
				return (CompProperties_AbilityBloodfeederBite)this.props;
			}
		}

		// Token: 0x0600BE70 RID: 48752 RVA: 0x00373620 File Offset: 0x00371820
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Pawn;
			if (pawn == null)
			{
				return;
			}
			SanguophageUtility.DoBite(this.parent.pawn, pawn, this.Props.hemogenGain, this.Props.nutritionGain, this.Props.targetBloodLoss, this.Props.resistanceGain, this.Props.bloodFilthToSpawnRange, this.Props.thoughtDefToGiveTarget, this.Props.opinionThoughtDefToGiveTarget);
		}

		// Token: 0x0600BE71 RID: 48753 RVA: 0x0037369F File Offset: 0x0037189F
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Valid(target, false);
		}

		// Token: 0x0600BE72 RID: 48754 RVA: 0x003736AC File Offset: 0x003718AC
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			if (pawn == null)
			{
				return false;
			}
			if (!AbilityUtility.ValidateMustBeHumanOrWildMan(pawn, throwMessages, this.parent))
			{
				return false;
			}
			if (pawn.Faction != null && !pawn.IsSlaveOfColony && !pawn.IsPrisonerOfColony)
			{
				if (pawn.Faction.HostileTo(this.parent.pawn.Faction))
				{
					if (!pawn.Downed)
					{
						if (throwMessages)
						{
							Messages.Message("MessageCantUseOnResistingPerson".Translate(this.parent.def.Named("ABILITY")), pawn, MessageTypeDefOf.RejectInput, false);
						}
						return false;
					}
				}
				else if (pawn.IsQuestLodger() || pawn.Faction != this.parent.pawn.Faction)
				{
					if (throwMessages)
					{
						Messages.Message("MessageCannotUseOnOtherFactions".Translate(this.parent.def.Named("ABILITY")), pawn, MessageTypeDefOf.RejectInput, false);
					}
					return false;
				}
			}
			if (pawn.IsWildMan() && !pawn.IsPrisonerOfColony && !pawn.Downed)
			{
				if (throwMessages)
				{
					Messages.Message("MessageCantUseOnResistingPerson".Translate(this.parent.def.Named("ABILITY")), pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (pawn.InMentalState || PrisonBreakUtility.IsPrisonBreaking(pawn))
			{
				if (throwMessages)
				{
					Messages.Message("MessageCantUseOnResistingPerson".Translate(this.parent.def.Named("ABILITY")), pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (ModsConfig.AnomalyActive && pawn.IsMutant && !pawn.mutant.Def.canBleed)
			{
				if (throwMessages)
				{
					Messages.Message("MessageCannotUseOnNonBleeder".Translate(this.parent.def.Named("ABILITY")), pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600BE73 RID: 48755 RVA: 0x003738AC File Offset: 0x00371AAC
		public override string ExtraLabelMouseAttachment(LocalTargetInfo target)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				string text = null;
				if (pawn.HostileTo(this.parent.pawn) && !pawn.Downed)
				{
					text += "MessageCantUseOnResistingPerson".Translate(this.parent.def.Named("ABILITY"));
				}
				float num = this.BloodlossAfterBite(pawn);
				if (num >= HediffDefOf.BloodLoss.lethalSeverity)
				{
					if (!text.NullOrEmpty())
					{
						text += "\n";
					}
					text += "WillKill".Translate();
				}
				else if (HediffDefOf.BloodLoss.stages[HediffDefOf.BloodLoss.StageAtSeverity(num)].lifeThreatening)
				{
					if (!text.NullOrEmpty())
					{
						text += "\n";
					}
					text += "WillCauseSeriousBloodloss".Translate();
				}
				return text;
			}
			return base.ExtraLabelMouseAttachment(target);
		}

		// Token: 0x0600BE74 RID: 48756 RVA: 0x003739A4 File Offset: 0x00371BA4
		public override Window ConfirmationDialog(LocalTargetInfo target, Action confirmAction)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (pawn.genes != null && pawn.genes.HasActiveGene(GeneDefOf.Deathless))
				{
					return null;
				}
				float num = this.BloodlossAfterBite(pawn);
				if (num >= HediffDefOf.BloodLoss.lethalSeverity)
				{
					return Dialog_MessageBox.CreateConfirmation("WarningPawnWillDieFromBloodfeeding".Translate(pawn.Named("PAWN")), confirmAction, true, null, WindowLayer.Dialog);
				}
				if (HediffDefOf.BloodLoss.stages[HediffDefOf.BloodLoss.StageAtSeverity(num)].lifeThreatening)
				{
					return Dialog_MessageBox.CreateConfirmation("WarningPawnWillHaveSeriousBloodlossFromBloodfeeding".Translate(pawn.Named("PAWN")), confirmAction, true, null, WindowLayer.Dialog);
				}
			}
			return null;
		}

		// Token: 0x0600BE75 RID: 48757 RVA: 0x00373A50 File Offset: 0x00371C50
		private float BloodlossAfterBite(Pawn target)
		{
			if (target.Dead || !target.RaceProps.IsFlesh)
			{
				return 0f;
			}
			float num = this.Props.targetBloodLoss;
			Hediff firstHediffOfDef = target.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss, false);
			if (firstHediffOfDef != null)
			{
				num += firstHediffOfDef.Severity;
			}
			return num;
		}
	}
}
