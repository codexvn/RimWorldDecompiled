using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002754 RID: 10068
	public class RitualOutcomeComp_PawnStatScaled : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x0600DCD7 RID: 56535 RVA: 0x0040FC6C File Offset: 0x0040DE6C
		public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			return this.Count(ritual, data);
		}

		// Token: 0x0600DCD8 RID: 56536 RVA: 0x0040FC76 File Offset: 0x0040DE76
		protected float StatValue(Pawn pawn)
		{
			if (this.curve == null)
			{
				return pawn.GetStatValue(this.statDef, true, -1);
			}
			return this.curve.Evaluate(pawn.GetStatValue(this.statDef, true, -1));
		}

		// Token: 0x0600DCD9 RID: 56537 RVA: 0x0040FCA8 File Offset: 0x0040DEA8
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			Pawn pawn = ritual.PawnWithRole(this.roleId);
			if (pawn == null)
			{
				return 0f;
			}
			if (this.statDef.Worker.IsDisabledFor(pawn))
			{
				return 0f;
			}
			return this.StatValue(pawn) * this.scaledBy;
		}

		// Token: 0x0600DCDA RID: 56538 RVA: 0x0040FCF4 File Offset: 0x0040DEF4
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
			float num = 0f;
			if (!this.statDef.Worker.IsDisabledFor(pawn))
			{
				num = this.StatValue(pawn);
			}
			float num2 = num * this.scaledBy;
			string str = (num2 < 0f) ? "" : "+";
			return this.LabelForDesc.Formatted(pawn.Named("PAWN")) + ": " + "OutcomeBonusDesc_QualitySingleOffset".Translate(str + num2.ToStringPercent()) + ".";
		}

		// Token: 0x0600DCDB RID: 56539 RVA: 0x0040FDB4 File Offset: 0x0040DFB4
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			Pawn pawn = assignments.FirstAssignedPawn(this.roleId);
			if (pawn == null)
			{
				return null;
			}
			float f = 0f;
			float num = 0f;
			if (!this.statDef.Worker.IsDisabledFor(pawn))
			{
				f = pawn.GetStatValue(this.statDef, true, -1);
				num = this.StatValue(pawn) * this.scaledBy;
			}
			return new QualityFactor
			{
				label = this.label.Formatted(pawn.Named("PAWN")),
				count = f.ToStringPercent(),
				qualityChange = ((Math.Abs(num) > float.Epsilon) ? "OutcomeBonusDesc_QualitySingleOffset".Translate(num.ToStringWithSign("0.#%")).Resolve() : " - "),
				positive = (num >= 0f),
				quality = num,
				priority = 0f
			};
		}

		// Token: 0x0600DCDC RID: 56540 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool Applies(LordJob_Ritual ritual)
		{
			return true;
		}

		// Token: 0x040094F2 RID: 38130
		[NoTranslate]
		public string roleId;

		// Token: 0x040094F3 RID: 38131
		public StatDef statDef;

		// Token: 0x040094F4 RID: 38132
		public float scaledBy = 1f;
	}
}
