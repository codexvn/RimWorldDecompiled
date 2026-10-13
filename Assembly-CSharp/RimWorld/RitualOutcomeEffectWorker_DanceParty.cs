using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002773 RID: 10099
	public class RitualOutcomeEffectWorker_DanceParty : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x0600DD82 RID: 56706 RVA: 0x00411501 File Offset: 0x0040F701
		public RitualOutcomeEffectWorker_DanceParty()
		{
		}

		// Token: 0x0600DD83 RID: 56707 RVA: 0x00411509 File Offset: 0x0040F709
		public RitualOutcomeEffectWorker_DanceParty(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x0600DD84 RID: 56708 RVA: 0x004135A0 File Offset: 0x004117A0
		protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = null;
			if (outcome.Positive && Rand.Chance(0.1f))
			{
				float statFactorFromList = HediffDefOf.WorkFocus.stages[0].statOffsets.GetStatFactorFromList(StatDefOf.WorkSpeedGlobal);
				extraOutcomeDesc = "RitualOutcomeExtraDesc_DancePartyWorkFocus".Translate((((statFactorFromList > 0f) ? "+" : "") + statFactorFromList.ToStringPercent()).Named("PERCENTAGE"));
				foreach (Pawn pawn in totalPresence.Keys)
				{
					pawn.health.AddHediff(HediffDefOf.WorkFocus, null, null, null);
				}
			}
		}

		// Token: 0x0400954B RID: 38219
		public const float WorkFocusChance = 0.1f;
	}
}
