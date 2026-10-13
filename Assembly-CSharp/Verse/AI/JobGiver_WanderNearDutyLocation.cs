using System;

namespace Verse.AI
{
	// Token: 0x02001029 RID: 4137
	public class JobGiver_WanderNearDutyLocation : JobGiver_Wander
	{
		// Token: 0x06006520 RID: 25888 RVA: 0x001FF5F5 File Offset: 0x001FD7F5
		public JobGiver_WanderNearDutyLocation()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06006521 RID: 25889 RVA: 0x001FF61A File Offset: 0x001FD81A
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.mindState.duty.focus.Cell, pawn);
		}
	}
}
