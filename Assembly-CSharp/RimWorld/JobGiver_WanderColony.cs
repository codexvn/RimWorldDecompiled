using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200153D RID: 5437
	public class JobGiver_WanderColony : JobGiver_Wander
	{
		// Token: 0x06008117 RID: 33047 RVA: 0x0026DD28 File Offset: 0x0026BF28
		public JobGiver_WanderColony()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
			this.wanderDestValidator = ((Pawn pawn, IntVec3 loc, IntVec3 root) => true);
		}

		// Token: 0x06008118 RID: 33048 RVA: 0x0026DD7D File Offset: 0x0026BF7D
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.GetColonyWanderRoot(pawn);
		}
	}
}
