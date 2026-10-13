using System;

namespace Verse.AI
{
	// Token: 0x0200102D RID: 4141
	public class JobGiver_RunRandom : JobGiver_Wander
	{
		// Token: 0x0600652A RID: 25898 RVA: 0x001FF707 File Offset: 0x001FD907
		public JobGiver_RunRandom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(5, 10);
			this.locomotionUrgency = LocomotionUrgency.Sprint;
		}

		// Token: 0x0600652B RID: 25899 RVA: 0x001FF049 File Offset: 0x001FD249
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
