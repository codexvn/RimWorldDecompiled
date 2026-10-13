using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200102C RID: 4140
	public class JobGiver_WanderNearBreacher : JobGiver_Wander
	{
		// Token: 0x06006528 RID: 25896 RVA: 0x001FF5F5 File Offset: 0x001FD7F5
		public JobGiver_WanderNearBreacher()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06006529 RID: 25897 RVA: 0x001FF6E4 File Offset: 0x001FD8E4
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			Pawn pawn2 = BreachingUtility.FindPawnToEscort(pawn);
			if (pawn2 == null)
			{
				return IntVec3.Invalid;
			}
			return pawn2.Position;
		}
	}
}
