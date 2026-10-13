using System;

namespace Verse.AI
{
	// Token: 0x0200102B RID: 4139
	public class JobGiver_WanderNearMaster : JobGiver_Wander
	{
		// Token: 0x06006524 RID: 25892 RVA: 0x001FF654 File Offset: 0x001FD854
		public JobGiver_WanderNearMaster()
		{
			this.wanderRadius = 3f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
			this.wanderDestValidator = ((Pawn p, IntVec3 c, IntVec3 root) => !this.MustUseRootRoom(p) || root.GetRoom(p.Map) == null || WanderRoomUtility.IsValidWanderDest(p, c, root));
		}

		// Token: 0x06006525 RID: 25893 RVA: 0x001FF68B File Offset: 0x001FD88B
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.playerSettings.Master.PositionHeld, pawn);
		}

		// Token: 0x06006526 RID: 25894 RVA: 0x001FF6A3 File Offset: 0x001FD8A3
		private bool MustUseRootRoom(Pawn pawn)
		{
			return !pawn.playerSettings.Master.playerSettings.animalsReleased;
		}
	}
}
