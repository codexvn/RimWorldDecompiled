using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200268B RID: 9867
	public class RitualAttachableOutcomeEffectWorker_PsyfocusRecharge : RitualAttachableOutcomeEffectWorker
	{
		// Token: 0x0600D8D4 RID: 55508 RVA: 0x00404130 File Offset: 0x00402330
		public override void Apply(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = this.def.letterInfoText;
			foreach (Pawn pawn in totalPresence.Keys)
			{
				Pawn_PsychicEntropyTracker psychicEntropy = pawn.psychicEntropy;
				if (psychicEntropy != null)
				{
					psychicEntropy.RechargePsyfocus();
				}
			}
		}
	}
}
