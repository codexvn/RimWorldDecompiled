using System;
using System.Text;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002207 RID: 8711
	public class CompAbilityEffect_Convert : CompAbilityEffect
	{
		// Token: 0x17001D18 RID: 7448
		// (get) Token: 0x0600BEAF RID: 48815 RVA: 0x003745BF File Offset: 0x003727BF
		public new CompProperties_AbilityConvert Props
		{
			get
			{
				return (CompProperties_AbilityConvert)this.props;
			}
		}

		// Token: 0x17001D19 RID: 7449
		// (get) Token: 0x0600BEB0 RID: 48816 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool HideTargetPawnTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600BEB1 RID: 48817 RVA: 0x003745CC File Offset: 0x003727CC
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (!ModLister.CheckIdeology("Ideoligion conversion"))
			{
				return;
			}
			Pawn pawn = this.parent.pawn;
			Pawn pawn2 = target.Pawn;
			float certaintyReduction = InteractionWorker_ConvertIdeoAttempt.CertaintyReduction(pawn, pawn2) * this.Props.convertPowerFactor;
			float certainty = pawn2.ideo.Certainty;
			if (pawn2.ideo.IdeoConversionAttempt(certaintyReduction, pawn.Ideo, true))
			{
				pawn2.ideo.SetIdeo(this.parent.pawn.Ideo);
				Messages.Message(this.Props.successMessage.Formatted(pawn.Named("INITIATOR"), pawn2.Named("RECIPIENT"), pawn.Ideo.name.Named("IDEO")), new LookTargets(new Pawn[]
				{
					pawn,
					pawn2
				}), MessageTypeDefOf.PositiveEvent, true);
				PlayLogEntry_Interaction entry = new PlayLogEntry_Interaction(InteractionDefOf.Convert_Success, this.parent.pawn, pawn2, null);
				Find.PlayLog.Add(entry);
			}
			else
			{
				pawn.needs.mood.thoughts.memories.TryGainMemory(this.Props.failedThoughtInitiator, pawn2, null);
				pawn2.needs.mood.thoughts.memories.TryGainMemory(this.Props.failedThoughtRecipient, pawn, null);
				Messages.Message(this.Props.failMessage.Formatted(pawn.Named("INITIATOR"), pawn2.Named("RECIPIENT"), pawn.Ideo.name.Named("IDEO"), certainty.ToStringPercent().Named("CERTAINTYBEFORE"), pawn2.ideo.Certainty.ToStringPercent().Named("CERTAINTYAFTER")), new LookTargets(new Pawn[]
				{
					pawn,
					pawn2
				}), MessageTypeDefOf.NeutralEvent, true);
				PlayLogEntry_Interaction entry2 = new PlayLogEntry_Interaction(InteractionDefOf.Convert_Failure, this.parent.pawn, pawn2, null);
				Find.PlayLog.Add(entry2);
			}
			if (this.Props.sound != null)
			{
				this.Props.sound.PlayOneShot(new TargetInfo(target.Cell, this.parent.pawn.Map, false));
			}
		}

		// Token: 0x0600BEB2 RID: 48818 RVA: 0x00374810 File Offset: 0x00372A10
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return pawn != null && AbilityUtility.ValidateMustBeHuman(pawn, throwMessages, this.parent) && AbilityUtility.ValidateMustNotBeBaby(pawn, throwMessages, this.parent) && AbilityUtility.ValidateNoMentalState(pawn, throwMessages, this.parent) && AbilityUtility.ValidateNotSameIdeo(this.parent.pawn, pawn, throwMessages, this.parent) && AbilityUtility.ValidateIsConscious(pawn, throwMessages, this.parent);
		}

		// Token: 0x0600BEB3 RID: 48819 RVA: 0x0037488C File Offset: 0x00372A8C
		public override string ExtraLabelMouseAttachment(LocalTargetInfo target)
		{
			if (target.Pawn == null || !this.Valid(target, false))
			{
				return null;
			}
			Pawn pawn = this.parent.pawn;
			Pawn pawn2 = target.Pawn;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("AbilityIdeoConvertBreakdownLabel".Translate().CapitalizeFirst() + ": " + (InteractionWorker_ConvertIdeoAttempt.CertaintyReduction(pawn, pawn2) * this.Props.convertPowerFactor).ToStringPercent());
			stringBuilder.AppendLine();
			stringBuilder.AppendInNewLine("Factors".Translate() + ":");
			stringBuilder.AppendInNewLine(" -  " + "Base".Translate().CapitalizeFirst() + ": " + 0.06f.ToStringPercent());
			stringBuilder.AppendInNewLine(" -  " + "AbilityIdeoConvertBreakdownUsingAbility".Translate(this.parent.def.LabelCap.Named("ABILITY")) + ": " + this.Props.convertPowerFactor.ToStringPercent());
			float statValue = pawn.GetStatValue(StatDefOf.ConversionPower, true, -1);
			if (statValue != 1f)
			{
				stringBuilder.AppendInNewLine(" -  " + "AbilityIdeoConvertBreakdownConversionPower".Translate(pawn.Named("PAWN")) + ": " + statValue.ToStringPercent());
			}
			TaggedString certaintyReductionFactorsDescription = ConversionUtility.GetCertaintyReductionFactorsDescription(pawn2);
			if (!certaintyReductionFactorsDescription.NullOrEmpty())
			{
				stringBuilder.AppendInNewLine(" -  " + certaintyReductionFactorsDescription);
			}
			Ideo ideo = pawn2.Ideo;
			Precept_Role precept_Role = (ideo != null) ? ideo.GetRole(pawn2) : null;
			if (precept_Role != null && precept_Role.def.certaintyLossFactor != 1f)
			{
				stringBuilder.AppendInNewLine(" -  " + "AbilityIdeoConvertBreakdownRole".Translate(pawn2.Named("PAWN"), precept_Role.Named("ROLE")) + ": " + precept_Role.def.certaintyLossFactor.ToStringPercent());
			}
			ReliquaryUtility.GetRelicConvertPowerFactorForPawn(pawn, stringBuilder);
			ConversionUtility.ConversionPowerFactor_MemesVsTraits(pawn, pawn2, stringBuilder);
			return stringBuilder.ToString();
		}
	}
}
