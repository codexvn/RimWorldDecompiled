using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002766 RID: 10086
	public class RitualOutcomeEffectWorker_GiveMemoryBelievers : RitualOutcomeEffectWorker
	{
		// Token: 0x0600DD3F RID: 56639 RVA: 0x00411463 File Offset: 0x0040F663
		public RitualOutcomeEffectWorker_GiveMemoryBelievers()
		{
		}

		// Token: 0x0600DD40 RID: 56640 RVA: 0x0041146B File Offset: 0x0040F66B
		public RitualOutcomeEffectWorker_GiveMemoryBelievers(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x0600DD41 RID: 56641 RVA: 0x004116C0 File Offset: 0x0040F8C0
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
			{
				if (pawn.Ideo == jobRitual.Ritual.ideo)
				{
					pawn.needs.mood.thoughts.memories.TryGainMemory(base.MakeMemory(pawn, jobRitual, null), null);
				}
			}
		}
	}
}
