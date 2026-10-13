using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x020027CE RID: 10190
	public abstract class RitualStageTickActionMaker
	{
		// Token: 0x0600DF12 RID: 57106
		public abstract IEnumerable<ActionOnTick> GenerateTimedActions(LordJob_Ritual ritual, RitualStage stage);
	}
}
