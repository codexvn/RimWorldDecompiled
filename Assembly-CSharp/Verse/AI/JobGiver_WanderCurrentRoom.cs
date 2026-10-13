using System;

namespace Verse.AI
{
	// Token: 0x0200101C RID: 4124
	public class JobGiver_WanderCurrentRoom : JobGiver_Wander
	{
		// Token: 0x060064FF RID: 25855 RVA: 0x001FF054 File Offset: 0x001FD254
		public JobGiver_WanderCurrentRoom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
			this.locomotionUrgency = LocomotionUrgency.Amble;
			this.wanderDestValidator = ((Pawn pawn, IntVec3 loc, IntVec3 root) => WanderRoomUtility.IsValidWanderDest(pawn, loc, root));
		}

		// Token: 0x06006500 RID: 25856 RVA: 0x001FF049 File Offset: 0x001FD249
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
