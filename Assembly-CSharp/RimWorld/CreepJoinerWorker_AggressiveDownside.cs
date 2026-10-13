using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020022D2 RID: 8914
	public class CreepJoinerWorker_AggressiveDownside : BaseCreepJoinerWorker
	{
		// Token: 0x0600C1F0 RID: 49648 RVA: 0x00383064 File Offset: 0x00381264
		public override void DoResponse(List<TargetInfo> looktargets, List<NamedArgument> namedArgs)
		{
			base.Tracker.DoAggressive();
		}
	}
}
