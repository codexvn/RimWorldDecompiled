using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002749 RID: 10057
	public class RitualOutcomeComp_AtLeastOneSpectator : RitualOutcomeComp_ParticipantCount
	{
		// Token: 0x0600DCA2 RID: 56482 RVA: 0x0040EEEC File Offset: 0x0040D0EC
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			int num = assignments.Participants.Count((Pawn p) => this.Counts(assignments, p));
			float quality = (num > 0) ? this.qualityOffset : 0f;
			return new QualityFactor
			{
				label = this.label.CapitalizeFirst(),
				present = (num > 0),
				qualityChange = this.ExpectedOffsetDesc(true, quality),
				quality = quality,
				positive = true,
				priority = 4f
			};
		}

		// Token: 0x0600DCA3 RID: 56483 RVA: 0x0040EF84 File Offset: 0x0040D184
		public override string GetDesc(LordJob_Ritual ritual = null, RitualOutcomeComp_Data data = null)
		{
			return ((this.Count(ritual, data) > 0f) ? this.label : this.labelNotMet).CapitalizeFirst().Formatted(Array.Empty<NamedArgument>()) + ": " + "OutcomeBonusDesc_QualitySingleOffset".Translate(this.qualityOffset.ToStringPercent()) + ".";
		}

		// Token: 0x0600DCA4 RID: 56484 RVA: 0x0040EFF5 File Offset: 0x0040D1F5
		public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			if (this.Count(ritual, data) <= 0f)
			{
				return 0f;
			}
			return this.qualityOffset;
		}

		// Token: 0x0600DCA5 RID: 56485 RVA: 0x0040F012 File Offset: 0x0040D212
		protected override string ExpectedOffsetDesc(bool positive, float quality = 0f)
		{
			if (!positive)
			{
				return "";
			}
			return quality.ToStringWithSign("0.#%");
		}

		// Token: 0x040094DA RID: 38106
		[MustTranslate]
		public string labelNotMet;
	}
}
