using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020022D3 RID: 8915
	public class CreepJoinerWorker_DepartDownside : BaseCreepJoinerWorker
	{
		// Token: 0x0600C1F2 RID: 49650 RVA: 0x00383071 File Offset: 0x00381271
		public override bool CanDoResponse()
		{
			return base.Pawn.Spawned && base.Pawn.Map.CanEverExit;
		}

		// Token: 0x0600C1F3 RID: 49651 RVA: 0x00383092 File Offset: 0x00381292
		public override void DoResponse(List<TargetInfo> looktargets, List<NamedArgument> namedArgs)
		{
			base.Tracker.DoLeave();
		}
	}
}
