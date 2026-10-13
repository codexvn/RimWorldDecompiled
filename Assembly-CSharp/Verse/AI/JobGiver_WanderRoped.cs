using System;

namespace Verse.AI
{
	// Token: 0x0200102E RID: 4142
	public class JobGiver_WanderRoped : JobGiver_Wander
	{
		// Token: 0x0600652C RID: 25900 RVA: 0x001FF72F File Offset: 0x001FD92F
		public JobGiver_WanderRoped()
		{
			this.wanderRadius = 6f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x0600652D RID: 25901 RVA: 0x001FF754 File Offset: 0x001FD954
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.roping.RopedTo.Cell;
		}

		// Token: 0x0600652E RID: 25902 RVA: 0x001FF774 File Offset: 0x001FD974
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.roping.IsRoped || pawn.roping.IsRopedByPawn)
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}
	}
}
