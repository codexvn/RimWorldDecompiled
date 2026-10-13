using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002697 RID: 9879
	public class StageEndTrigger_PawnDeliveredOrNotValid : StageEndTrigger
	{
		// Token: 0x0600D8F8 RID: 55544 RVA: 0x0040482C File Offset: 0x00402A2C
		public override Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage)
		{
			return new Trigger_TickCondition(delegate()
			{
				foreach (TargetInfo targetInfo in foci)
				{
					Pawn pawn = targetInfo.Thing as Pawn;
					IntVec3 c = (spot.Thing != null) ? spot.Thing.OccupiedRect().CenterCell : spot.Cell;
					if (!pawn.CanReachImmediate(c, PathEndMode.Touch) && !pawn.Dead)
					{
						return false;
					}
				}
				return true;
			}, 1);
		}

		// Token: 0x0600D8F9 RID: 55545 RVA: 0x000026EA File Offset: 0x000008EA
		public override void ExposeData()
		{
		}
	}
}
