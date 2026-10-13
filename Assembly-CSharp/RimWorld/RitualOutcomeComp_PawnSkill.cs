using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002756 RID: 10070
	public class RitualOutcomeComp_PawnSkill : RitualOutcomeComp_Quality
	{
		// Token: 0x0600DCE4 RID: 56548 RVA: 0x0040FEB5 File Offset: 0x0040E0B5
		public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			return this.curve.Evaluate(this.Count(ritual, data));
		}

		// Token: 0x0600DCE5 RID: 56549 RVA: 0x00410053 File Offset: 0x0040E253
		protected float SkillValue(Pawn pawn)
		{
			return (float)pawn.skills.GetSkill(this.skillDef).Level;
		}

		// Token: 0x0600DCE6 RID: 56550 RVA: 0x0041006C File Offset: 0x0040E26C
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			Pawn pawn = ritual.PawnWithRole(this.roleId);
			if (pawn == null)
			{
				return 0f;
			}
			return this.SkillValue(pawn);
		}

		// Token: 0x0600DCE7 RID: 56551 RVA: 0x00410098 File Offset: 0x0040E298
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
			float x = this.SkillValue(pawn);
			float num = this.curve.Evaluate(x);
			string str = (num < 0f) ? "" : "+";
			return this.label.CapitalizeFirst().Formatted(pawn.Named("PAWN")) + ": " + "OutcomeBonusDesc_QualitySingleOffset".Translate(str + num.ToStringPercent()) + ".";
		}

		// Token: 0x0600DCE8 RID: 56552 RVA: 0x00410148 File Offset: 0x0040E348
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			Pawn pawn = assignments.FirstAssignedPawn(this.roleId);
			if (pawn == null)
			{
				return null;
			}
			float x = this.SkillValue(pawn);
			float num = this.curve.Evaluate(x);
			return new QualityFactor
			{
				label = this.label.Formatted(pawn.Named("PAWN")),
				count = x.ToString(),
				qualityChange = ((Math.Abs(num) > float.Epsilon) ? "OutcomeBonusDesc_QualitySingleOffset".Translate(num.ToStringWithSign("0.#%")).Resolve() : " - "),
				positive = (num >= 0f),
				quality = num,
				priority = 0f
			};
		}

		// Token: 0x0600DCE9 RID: 56553 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool Applies(LordJob_Ritual ritual)
		{
			return true;
		}

		// Token: 0x040094F6 RID: 38134
		[NoTranslate]
		public string roleId;

		// Token: 0x040094F7 RID: 38135
		public SkillDef skillDef;
	}
}
