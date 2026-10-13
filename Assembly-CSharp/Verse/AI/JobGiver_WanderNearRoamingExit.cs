using System;

namespace Verse.AI
{
	// Token: 0x02001026 RID: 4134
	public class JobGiver_WanderNearRoamingExit : JobGiver_Wander
	{
		// Token: 0x06006518 RID: 25880 RVA: 0x001FF501 File Offset: 0x001FD701
		public JobGiver_WanderNearRoamingExit()
		{
			this.wanderRadius = 12f;
		}

		// Token: 0x06006519 RID: 25881 RVA: 0x001FF514 File Offset: 0x001FD714
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_Roaming mentalState_Roaming = pawn.MentalState as MentalState_Roaming;
			if (mentalState_Roaming == null)
			{
				return null;
			}
			if (mentalState_Roaming.ShouldExitMapNow())
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x0600651A RID: 25882 RVA: 0x001FF543 File Offset: 0x001FD743
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			MentalState_Roaming mentalState_Roaming = pawn.MentalState as MentalState_Roaming;
			if (mentalState_Roaming == null)
			{
				return pawn.Position;
			}
			return mentalState_Roaming.exitDest;
		}
	}
}
