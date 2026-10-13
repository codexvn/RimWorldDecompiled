using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002764 RID: 10084
	public class RitualOutcomeEffectWorker_GiveMemoryAttended : RitualOutcomeEffectWorker
	{
		// Token: 0x0600DD37 RID: 56631 RVA: 0x00411463 File Offset: 0x0040F663
		public RitualOutcomeEffectWorker_GiveMemoryAttended()
		{
		}

		// Token: 0x0600DD38 RID: 56632 RVA: 0x0041146B File Offset: 0x0040F66B
		public RitualOutcomeEffectWorker_GiveMemoryAttended(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x0600DD39 RID: 56633 RVA: 0x00411474 File Offset: 0x0040F674
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			if (progress < 1f)
			{
				return;
			}
			foreach (KeyValuePair<Pawn, int> keyValuePair in totalPresence)
			{
				keyValuePair.Key.needs.mood.thoughts.memories.TryGainMemory(base.MakeMemory(keyValuePair.Key, jobRitual, null), null);
			}
		}
	}
}
