using System;

namespace Verse.AI
{
	// Token: 0x02001027 RID: 4135
	public class JobGiver_WanderOwnRoom : JobGiver_Wander
	{
		// Token: 0x0600651B RID: 25883 RVA: 0x001FF560 File Offset: 0x001FD760
		public JobGiver_WanderOwnRoom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(300, 600);
			this.locomotionUrgency = LocomotionUrgency.Amble;
			this.wanderDestValidator = ((Pawn pawn, IntVec3 loc, IntVec3 root) => WanderRoomUtility.IsValidWanderDest(pawn, loc, root));
		}

		// Token: 0x0600651C RID: 25884 RVA: 0x001FF5C0 File Offset: 0x001FD7C0
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			MentalState_WanderOwnRoom mentalState_WanderOwnRoom = pawn.MentalState as MentalState_WanderOwnRoom;
			if (mentalState_WanderOwnRoom != null)
			{
				return mentalState_WanderOwnRoom.target;
			}
			return pawn.Position;
		}
	}
}
