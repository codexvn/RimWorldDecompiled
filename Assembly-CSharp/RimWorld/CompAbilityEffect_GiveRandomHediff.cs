using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002242 RID: 8770
	public class CompAbilityEffect_GiveRandomHediff : CompAbilityEffect
	{
		// Token: 0x17001D3C RID: 7484
		// (get) Token: 0x0600BF7C RID: 49020 RVA: 0x003784FB File Offset: 0x003766FB
		public new CompProperties_AbilityGiveRandomHediff Props
		{
			get
			{
				return (CompProperties_AbilityGiveRandomHediff)this.props;
			}
		}

		// Token: 0x0600BF7D RID: 49021 RVA: 0x00378508 File Offset: 0x00376708
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			HediffOption hediffOption = this.GetApplicableHediffs(target.Pawn).RandomElement<HediffOption>();
			BodyPartRecord partRecord = this.GetAcceptablePartsForHediff(target.Pawn, hediffOption).RandomElement<BodyPartRecord>();
			Hediff hediff = HediffMaker.MakeHediff(hediffOption.hediffDef, target.Pawn, partRecord);
			target.Pawn.health.AddHediff(hediff, null, null, null);
			if (base.SendLetter)
			{
				Find.LetterStack.ReceiveLetter(this.Props.customLetterLabel.Formatted(hediff.def.LabelCap), this.Props.customLetterText.Formatted(this.parent.pawn, target.Pawn, hediff.def.label), LetterDefOf.PositiveEvent, new LookTargets(target.Pawn), null, null, null, null, 0, true);
			}
		}

		// Token: 0x0600BF7E RID: 49022 RVA: 0x003785FA File Offset: 0x003767FA
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return target.Pawn != null && this.GetApplicableHediffs(target.Pawn).Any<HediffOption>() && base.CanApplyOn(target, dest);
		}

		// Token: 0x0600BF7F RID: 49023 RVA: 0x00378628 File Offset: 0x00376828
		private List<HediffOption> GetApplicableHediffs(Pawn target)
		{
			List<HediffOption> list = new List<HediffOption>();
			foreach (HediffOption hediffOption in this.Props.options)
			{
				if (!this.GetAcceptablePartsForHediff(target, hediffOption).EnumerableNullOrEmpty<BodyPartRecord>())
				{
					list.Add(hediffOption);
				}
			}
			return list;
		}

		// Token: 0x0600BF80 RID: 49024 RVA: 0x00378698 File Offset: 0x00376898
		private IEnumerable<BodyPartRecord> GetAcceptablePartsForHediff(Pawn target, HediffOption option)
		{
			if (!this.Props.allowDuplicates && (from x in target.health.hediffSet.hediffs
			where x.def == option.hediffDef
			select x).Any<Hediff>())
			{
				return null;
			}
			return from p in target.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where (option.bodyPart == null || p.def == option.bodyPart) && !target.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(p)
			select p;
		}
	}
}
