using System;

namespace Verse.AI
{
	// Token: 0x0200102A RID: 4138
	public class JobGiver_WanderNearFallbackLocation : JobGiver_Wander
	{
		// Token: 0x06006522 RID: 25890 RVA: 0x001FF5F5 File Offset: 0x001FD7F5
		public JobGiver_WanderNearFallbackLocation()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06006523 RID: 25891 RVA: 0x001FF637 File Offset: 0x001FD837
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.mindState.duty.focusSecond.Cell, pawn);
		}
	}
}
