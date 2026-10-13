using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200268A RID: 9866
	public class RitualAttachableOutcomeEffectWorker_FarmAnimalsWanderIn : RitualAttachableOutcomeEffectWorker
	{
		// Token: 0x0600D8D2 RID: 55506 RVA: 0x004040A8 File Offset: 0x004022A8
		public override void Apply(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = null;
			IncidentParms parms = new IncidentParms
			{
				target = jobRitual.Map,
				totalBodySize = (outcome.BestPositiveOutcome(jobRitual) ? 3f : 2f),
				customLetterText = "RitualAttachedOutcome_FarmAnimalsWanderIn_Desc".Translate(jobRitual.RitualLabel)
			};
			if (IncidentDefOf.FarmAnimalsWanderIn.Worker.TryExecute(parms))
			{
				extraOutcomeDesc = this.def.letterInfoText;
			}
		}

		// Token: 0x040092FE RID: 37630
		public const float PositiveOutcomeBodysize = 2f;

		// Token: 0x040092FF RID: 37631
		public const float BestOutcomeBodysize = 3f;
	}
}
