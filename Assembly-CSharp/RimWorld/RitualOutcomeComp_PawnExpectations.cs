using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002758 RID: 10072
	public class RitualOutcomeComp_PawnExpectations : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x170021CF RID: 8655
		// (get) Token: 0x0600DCEE RID: 56558 RVA: 0x00410258 File Offset: 0x0040E458
		public PawnExpectationsQualityOffset SlaveExpectationCached
		{
			get
			{
				if (this.slaveExpectationCached == null)
				{
					this.slaveExpectationCached = new PawnExpectationsQualityOffset
					{
						labelOverride = "Slave".Translate().CapitalizeFirst(),
						offset = this.offsetForSlave
					};
				}
				return this.slaveExpectationCached;
			}
		}

		// Token: 0x0600DCEF RID: 56559 RVA: 0x0040FC6C File Offset: 0x0040DE6C
		public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			return this.Count(ritual, data);
		}

		// Token: 0x0600DCF0 RID: 56560 RVA: 0x004102A8 File Offset: 0x0040E4A8
		protected PawnExpectationsQualityOffset ExpectationOffset(Pawn pawn)
		{
			if (pawn.IsSlaveOfColony)
			{
				return this.SlaveExpectationCached;
			}
			ExpectationDef expectations = ExpectationsUtility.CurrentExpectationFor(pawn);
			return this.offsetPerExpectation.FirstOrDefault((PawnExpectationsQualityOffset e) => e.expectation == expectations);
		}

		// Token: 0x0600DCF1 RID: 56561 RVA: 0x004102F0 File Offset: 0x0040E4F0
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			Pawn pawn = ritual.assignments.FirstAssignedPawn(this.roleId);
			if (pawn == null)
			{
				return 0f;
			}
			PawnExpectationsQualityOffset pawnExpectationsQualityOffset = this.ExpectationOffset(pawn);
			if (pawnExpectationsQualityOffset == null)
			{
				return 0f;
			}
			return pawnExpectationsQualityOffset.offset;
		}

		// Token: 0x0600DCF2 RID: 56562 RVA: 0x00410330 File Offset: 0x0040E530
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
			PawnExpectationsQualityOffset pawnExpectationsQualityOffset = this.ExpectationOffset(pawn);
			string str = (pawnExpectationsQualityOffset.offset < 0f) ? "" : "+";
			return this.LabelForDesc.Formatted(pawn.Named("PAWN")) + ": " + "OutcomeBonusDesc_QualitySingleOffset".Translate(str + pawnExpectationsQualityOffset.offset.ToStringPercent()) + ".";
		}

		// Token: 0x0600DCF3 RID: 56563 RVA: 0x004103D8 File Offset: 0x0040E5D8
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			Pawn pawn = assignments.FirstAssignedPawn(this.roleId);
			if (pawn == null)
			{
				return null;
			}
			PawnExpectationsQualityOffset pawnExpectationsQualityOffset = this.ExpectationOffset(pawn);
			if (pawnExpectationsQualityOffset != null)
			{
				return new QualityFactor
				{
					label = "RitualPredictedOutcomeDescPawnExpectations".Translate(pawn.Named("PAWN")),
					count = pawnExpectationsQualityOffset.Label,
					qualityChange = ((Math.Abs(pawnExpectationsQualityOffset.offset) > float.Epsilon) ? "OutcomeBonusDesc_QualitySingleOffset".Translate(pawnExpectationsQualityOffset.offset.ToStringWithSign("0.#%")).Resolve() : " - "),
					positive = (pawnExpectationsQualityOffset.offset >= 0f),
					quality = pawnExpectationsQualityOffset.offset,
					priority = 0f
				};
			}
			return null;
		}

		// Token: 0x040094FB RID: 38139
		[NoTranslate]
		public string roleId;

		// Token: 0x040094FC RID: 38140
		public float offsetForSlave;

		// Token: 0x040094FD RID: 38141
		public List<PawnExpectationsQualityOffset> offsetPerExpectation = new List<PawnExpectationsQualityOffset>();

		// Token: 0x040094FE RID: 38142
		private PawnExpectationsQualityOffset slaveExpectationCached;
	}
}
