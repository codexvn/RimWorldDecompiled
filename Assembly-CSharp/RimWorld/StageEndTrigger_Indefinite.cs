using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200269E RID: 9886
	public class StageEndTrigger_Indefinite : StageEndTrigger
	{
		// Token: 0x0600D908 RID: 55560 RVA: 0x00404C5C File Offset: 0x00402E5C
		public override Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage)
		{
			return new Trigger_Custom((TriggerSignal signal) => false);
		}
	}
}
