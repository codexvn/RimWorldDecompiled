using System;

namespace Verse.AI
{
	// Token: 0x0200101B RID: 4123
	public class JobGiver_WanderAnywhere : JobGiver_Wander
	{
		// Token: 0x060064FD RID: 25853 RVA: 0x001FF01D File Offset: 0x001FD21D
		public JobGiver_WanderAnywhere()
		{
			this.wanderRadius = 7f;
			this.locomotionUrgency = LocomotionUrgency.Walk;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x060064FE RID: 25854 RVA: 0x001FF049 File Offset: 0x001FD249
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
