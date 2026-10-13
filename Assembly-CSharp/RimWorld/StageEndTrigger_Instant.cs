using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002694 RID: 9876
	public class StageEndTrigger_Instant : StageEndTrigger
	{
		// Token: 0x0600D8EF RID: 55535 RVA: 0x004047B8 File Offset: 0x004029B8
		public override Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage)
		{
			return new Trigger_Custom((TriggerSignal signal) => true);
		}
	}
}
