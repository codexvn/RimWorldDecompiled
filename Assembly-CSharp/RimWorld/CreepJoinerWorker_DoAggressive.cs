using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020022D6 RID: 8918
	public class CreepJoinerWorker_DoAggressive : BaseCreepJoinerWorker
	{
		// Token: 0x0600C1FC RID: 49660 RVA: 0x00383064 File Offset: 0x00381264
		public override void DoResponse(List<TargetInfo> looktargets, List<NamedArgument> namedArgs)
		{
			base.Tracker.DoAggressive();
		}
	}
}
