using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002693 RID: 9875
	public abstract class StageEndTrigger : IExposable
	{
		// Token: 0x1700214A RID: 8522
		// (get) Token: 0x0600D8EB RID: 55531 RVA: 0x004047B0 File Offset: 0x004029B0
		public virtual bool CountsTowardsProgress
		{
			get
			{
				return this.countsTowardsProgress;
			}
		}

		// Token: 0x0600D8EC RID: 55532
		public abstract Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage);

		// Token: 0x0600D8ED RID: 55533 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ExposeData()
		{
		}

		// Token: 0x0400930A RID: 37642
		public bool countsTowardsProgress;
	}
}
