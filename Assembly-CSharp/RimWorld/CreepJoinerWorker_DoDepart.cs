using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020022D7 RID: 8919
	public class CreepJoinerWorker_DoDepart : BaseCreepJoinerWorker
	{
		// Token: 0x0600C1FE RID: 49662 RVA: 0x00383092 File Offset: 0x00381292
		public override void DoResponse(List<TargetInfo> looktargets, List<NamedArgument> namedArgs)
		{
			base.Tracker.DoLeave();
		}
	}
}
