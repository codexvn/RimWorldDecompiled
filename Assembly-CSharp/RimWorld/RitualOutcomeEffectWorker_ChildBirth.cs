using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200277E RID: 10110
	public class RitualOutcomeEffectWorker_ChildBirth : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x0600DDA2 RID: 56738 RVA: 0x00411501 File Offset: 0x0040F701
		public RitualOutcomeEffectWorker_ChildBirth()
		{
		}

		// Token: 0x0600DDA3 RID: 56739 RVA: 0x00411509 File Offset: 0x0040F709
		public RitualOutcomeEffectWorker_ChildBirth(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x170021E6 RID: 8678
		// (get) Token: 0x0600DDA4 RID: 56740 RVA: 0x00413C90 File Offset: 0x00411E90
		public override bool ShowQuality
		{
			get
			{
				return !Find.Storyteller.difficulty.babiesAreHealthy;
			}
		}

		// Token: 0x170021E7 RID: 8679
		// (get) Token: 0x0600DDA5 RID: 56741 RVA: 0x00413CA4 File Offset: 0x00411EA4
		public override string Description
		{
			get
			{
				if (!Find.Storyteller.difficulty.babiesAreHealthy)
				{
					return base.Description;
				}
				return "";
			}
		}

		// Token: 0x0600DDA6 RID: 56742 RVA: 0x00413CC4 File Offset: 0x00411EC4
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			if (progress == 0f)
			{
				return;
			}
			float quality = base.GetQuality(jobRitual, progress);
			RitualOutcomePossibility outcome = this.GetOutcome(quality, jobRitual);
			Pawn pawn = jobRitual.assignments.FirstAssignedPawn("mother");
			Pawn doctor = jobRitual.assignments.FirstAssignedPawn("doctor");
			Hediff_LaborPushing hediff_LaborPushing = (Hediff_LaborPushing)pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PregnancyLaborPushing, false);
			float quality2 = quality;
			Precept_Ritual ritual = jobRitual.Ritual;
			GeneSet geneSet = hediff_LaborPushing.geneSet;
			PregnancyUtility.ApplyBirthOutcome(outcome, quality2, ritual, (geneSet != null) ? geneSet.GenesListForReading : null, hediff_LaborPushing.Mother ?? pawn, pawn, hediff_LaborPushing.Father, doctor, jobRitual, jobRitual.assignments, false);
		}

		// Token: 0x0600DDA7 RID: 56743 RVA: 0x00413D63 File Offset: 0x00411F63
		public override RitualOutcomePossibility GetOutcome(float quality, LordJob_Ritual ritual)
		{
			if (Find.Storyteller.difficulty.babiesAreHealthy)
			{
				return this.def.BestOutcome;
			}
			return base.GetOutcome(quality, ritual);
		}
	}
}
