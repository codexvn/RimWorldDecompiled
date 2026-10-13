using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002755 RID: 10069
	public class RitualOutcomeComp_PawnAge : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x0600DCDE RID: 56542 RVA: 0x0040FEB5 File Offset: 0x0040E0B5
		public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			return this.curve.Evaluate(this.Count(ritual, data));
		}

		// Token: 0x0600DCDF RID: 56543 RVA: 0x0040FECC File Offset: 0x0040E0CC
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			Pawn pawn = ritual.PawnWithRole(this.roleId);
			if (pawn != null)
			{
				return pawn.ageTracker.AgeBiologicalYearsFloat;
			}
			return 0f;
		}

		// Token: 0x0600DCE0 RID: 56544 RVA: 0x0040FEFC File Offset: 0x0040E0FC
		public override string GetDesc(LordJob_Ritual ritual = null, RitualOutcomeComp_Data data = null)
		{
			if (ritual == null)
			{
				return this.labelAbstract;
			}
			Pawn pawn = (ritual != null) ? ritual.PawnWithRole(this.roleId) : null;
			if (pawn == null)
			{
				return null;
			}
			float num = this.curve.Evaluate(pawn.ageTracker.AgeBiologicalYearsFloat);
			string str = (num < 0f) ? "" : "+";
			return this.LabelForDesc.Formatted(pawn.Named("PAWN")) + ": " + "OutcomeBonusDesc_QualitySingleOffset".Translate(str + num.ToStringPercent()) + ".";
		}

		// Token: 0x0600DCE1 RID: 56545 RVA: 0x0040FFA8 File Offset: 0x0040E1A8
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			Pawn pawn = assignments.FirstAssignedPawn(this.roleId);
			if (pawn == null)
			{
				return null;
			}
			float num = this.curve.Evaluate(pawn.ageTracker.AgeBiologicalYearsFloat);
			return new QualityFactor
			{
				label = this.label.Formatted(pawn.Named("PAWN")),
				count = pawn.ageTracker.AgeBiologicalYears.ToString(),
				qualityChange = this.ExpectedOffsetDesc(num > 0f, num),
				positive = (num > 0f),
				quality = num,
				priority = 0f
			};
		}

		// Token: 0x0600DCE2 RID: 56546 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool Applies(LordJob_Ritual ritual)
		{
			return true;
		}

		// Token: 0x040094F5 RID: 38133
		[NoTranslate]
		public string roleId;
	}
}
