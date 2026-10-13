using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200273C RID: 10044
	public class RitualOutcomeComp_TargetThingStat : RitualOutcomeComp_Quality
	{
		// Token: 0x170021C5 RID: 8645
		// (get) Token: 0x0600DC6E RID: 56430 RVA: 0x00002501 File Offset: 0x00000701
		public override bool DataRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DC6F RID: 56431 RVA: 0x0040E346 File Offset: 0x0040C546
		public override bool Applies(LordJob_Ritual ritual)
		{
			return ritual.selectedTarget.HasThing && (!this.mustBeBed || ritual.selectedTarget.Thing.def.IsBed);
		}

		// Token: 0x0600DC70 RID: 56432 RVA: 0x0040E376 File Offset: 0x0040C576
		private float StatValue(TargetInfo ritualTarget)
		{
			if (ritualTarget.HasThing)
			{
				return ritualTarget.Thing.GetStatValue(this.statDef, true, -1);
			}
			return 0f;
		}

		// Token: 0x0600DC71 RID: 56433 RVA: 0x0040E39B File Offset: 0x0040C59B
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			return this.StatValue(ritual.selectedTarget);
		}

		// Token: 0x0600DC72 RID: 56434 RVA: 0x0040E3AC File Offset: 0x0040C5AC
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			float x = this.StatValue(ritualTarget);
			float quality = this.curve.Evaluate(x);
			return new QualityFactor
			{
				label = this.label.CapitalizeFirst(),
				count = x.ToString(),
				present = (!this.mustBeBed || (ritualTarget.HasThing && ritualTarget.Thing.def.IsBed)),
				qualityChange = this.ExpectedOffsetDesc(true, quality),
				quality = quality,
				positive = true,
				priority = 0f
			};
		}

		// Token: 0x040094C7 RID: 38087
		public StatDef statDef;

		// Token: 0x040094C8 RID: 38088
		public bool mustBeBed;
	}
}
