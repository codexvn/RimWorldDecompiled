using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02001025 RID: 4133
	public class JobGiver_WanderMapEdge : JobGiver_Wander
	{
		// Token: 0x06006516 RID: 25878 RVA: 0x001FF4BC File Offset: 0x001FD6BC
		public JobGiver_WanderMapEdge()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(50, 125);
		}

		// Token: 0x06006517 RID: 25879 RVA: 0x001FF4E0 File Offset: 0x001FD6E0
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			IntVec3 result;
			if (RCellFinder.TryFindBestExitSpot(pawn, out result, TraverseMode.ByPawn, true))
			{
				return result;
			}
			return pawn.Position;
		}
	}
}
