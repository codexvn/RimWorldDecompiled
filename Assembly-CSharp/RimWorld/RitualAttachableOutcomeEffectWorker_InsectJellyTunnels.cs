using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200268D RID: 9869
	public class RitualAttachableOutcomeEffectWorker_InsectJellyTunnels : RitualAttachableOutcomeEffectWorker
	{
		// Token: 0x0600D8D9 RID: 55513 RVA: 0x00404280 File Offset: 0x00402480
		public override void Apply(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = null;
			IncidentParms parms = new IncidentParms
			{
				target = jobRitual.Map,
				points = (float)(outcome.BestPositiveOutcome(jobRitual) ? 320 : 210)
			};
			if (IncidentDefOf.Infestation_Jelly.Worker.TryExecute(parms))
			{
				extraOutcomeDesc = this.def.letterInfoText;
			}
		}

		// Token: 0x04009300 RID: 37632
		public const int PositiveOutcomePoints = 210;

		// Token: 0x04009301 RID: 37633
		public const int BestOutcomePoints = 320;
	}
}
