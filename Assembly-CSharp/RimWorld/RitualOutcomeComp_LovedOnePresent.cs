using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200274B RID: 10059
	public class RitualOutcomeComp_LovedOnePresent : RitualOutcomeComp_Quality
	{
		// Token: 0x0600DCA9 RID: 56489 RVA: 0x0040F044 File Offset: 0x0040D244
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			Pawn pawn = this.LovedOneParticipating(assignments);
			bool flag = pawn != null;
			float quality = flag ? this.qualityOffset : 0f;
			return new QualityFactor
			{
				label = "RitualOutcomeLovedOnePresentLabel".Translate(),
				present = flag,
				qualityChange = this.ExpectedOffsetDesc(true, quality),
				quality = quality,
				positive = true,
				priority = 4f,
				count = ((pawn != null) ? pawn.LabelShort : null),
				toolTip = "RitualOutcomeLovedOnePresentTip".Translate(assignments.FirstAssignedPawn(this.lovedOneOfRole).Named("PAWN"))
			};
		}

		// Token: 0x0600DCAA RID: 56490 RVA: 0x0040F0F8 File Offset: 0x0040D2F8
		private Pawn LovedOneParticipating(RitualRoleAssignments assignments)
		{
			Pawn lovedOnOf = assignments.FirstAssignedPawn(this.lovedOneOfRole);
			if (lovedOnOf == null)
			{
				return null;
			}
			RitualOutcomeComp_LovedOnePresent.lovedOnesTmp.Clear();
			RitualOutcomeComp_LovedOnePresent.lovedOnesTmp.AddRange(from p in SocialCardUtility.PawnsForSocialInfo(lovedOnOf)
			where assignments.PawnParticipating(p) && lovedOnOf.relations.OpinionOf(p) >= 20
			select p);
			if (RitualOutcomeComp_LovedOnePresent.lovedOnesTmp.Count > 0)
			{
				RitualOutcomeComp_LovedOnePresent.lovedOnesTmp.SortByDescending((Pawn l) => lovedOnOf.relations.OpinionOf(l));
				return RitualOutcomeComp_LovedOnePresent.lovedOnesTmp.First<Pawn>();
			}
			return null;
		}

		// Token: 0x0600DCAB RID: 56491 RVA: 0x0040F194 File Offset: 0x0040D394
		public override string GetDesc(LordJob_Ritual ritual = null, RitualOutcomeComp_Data data = null)
		{
			string str = (this.qualityOffset < 0f) ? "" : "+";
			return ((this.LovedOneParticipating(ritual.assignments) != null) ? this.label : this.labelNotMet).CapitalizeFirst().Formatted(Array.Empty<NamedArgument>()) + ": " + "OutcomeBonusDesc_QualitySingleOffset".Translate(str + this.qualityOffset.ToStringPercent()) + ".";
		}

		// Token: 0x0600DCAC RID: 56492 RVA: 0x0040F224 File Offset: 0x0040D424
		public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			if (this.LovedOneParticipating(ritual.assignments) == null)
			{
				return 0f;
			}
			return this.qualityOffset;
		}

		// Token: 0x0600DCAD RID: 56493 RVA: 0x0040F012 File Offset: 0x0040D212
		protected override string ExpectedOffsetDesc(bool positive, float quality = 0f)
		{
			if (!positive)
			{
				return "";
			}
			return quality.ToStringWithSign("0.#%");
		}

		// Token: 0x0600DCAE RID: 56494 RVA: 0x0040F240 File Offset: 0x0040D440
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			return (float)((this.LovedOneParticipating(ritual.assignments) != null) ? 1 : 0);
		}

		// Token: 0x040094DD RID: 38109
		[MustTranslate]
		public string labelNotMet;

		// Token: 0x040094DE RID: 38110
		[NoTranslate]
		public string lovedOneOfRole;

		// Token: 0x040094DF RID: 38111
		private static List<Pawn> lovedOnesTmp = new List<Pawn>();
	}
}
