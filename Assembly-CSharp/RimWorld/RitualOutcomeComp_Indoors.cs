using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200273D RID: 10045
	public class RitualOutcomeComp_Indoors : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x170021C6 RID: 8646
		// (get) Token: 0x0600DC74 RID: 56436 RVA: 0x00002501 File Offset: 0x00000701
		public override bool DataRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DC75 RID: 56437 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool Applies(LordJob_Ritual ritual)
		{
			return true;
		}

		// Token: 0x0600DC76 RID: 56438 RVA: 0x0040E448 File Offset: 0x0040C648
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			Room room = ritual.Spot.GetRoom(ritual.Map);
			if (room != null)
			{
				return (float)(room.PsychologicallyOutdoors ? 0 : 1);
			}
			return 0f;
		}

		// Token: 0x0600DC77 RID: 56439 RVA: 0x0040E480 File Offset: 0x0040C680
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			float quality = 0f;
			bool flag = false;
			if (ritualTarget.Map != null)
			{
				Room room = ritualTarget.Cell.GetRoom(ritualTarget.Map);
				flag = (room != null && !room.PsychologicallyOutdoors);
				quality = (flag ? this.qualityOffset : 0f);
			}
			return new QualityFactor
			{
				label = this.label.CapitalizeFirst(),
				qualityChange = this.ExpectedOffsetDesc(true, quality),
				quality = quality,
				present = flag,
				positive = true,
				priority = 0f
			};
		}
	}
}
