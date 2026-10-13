using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200141A RID: 5146
	public class JobGiver_AIWaitWithOverseer : ThinkNode_JobGiver
	{
		// Token: 0x06007D3B RID: 32059 RVA: 0x0025B764 File Offset: 0x00259964
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn overseer = pawn.GetOverseer();
			if (overseer == null || overseer.Awake())
			{
				return null;
			}
			IntVec3 c = this.CanUseCell(pawn.Position, pawn) ? pawn.Position : this.GetWaitDest(overseer.Position, pawn);
			if (c.IsValid)
			{
				Job job = JobMaker.MakeJob(JobDefOf.Wait_WithSleeping, c, overseer);
				job.expiryInterval = 120;
				job.expireRequiresEnemiesNearby = true;
				return job;
			}
			return null;
		}

		// Token: 0x06007D3C RID: 32060 RVA: 0x0025B7DC File Offset: 0x002599DC
		private IntVec3 GetWaitDest(IntVec3 root, Pawn mech)
		{
			Map map = mech.Map;
			IntVec3 result;
			if (CellFinder.TryFindRandomReachableNearbyCell(root, map, 1.9f, TraverseParms.For(mech, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), (IntVec3 c) => this.CanUseCell(c, mech), null, out result, 999999))
			{
				return result;
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06007D3D RID: 32061 RVA: 0x0025B844 File Offset: 0x00259A44
		private bool CanUseCell(IntVec3 c, Pawn mech)
		{
			Map map = mech.Map;
			return c.Standable(map) && mech.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn) && mech.CanReserve(c, 1, -1, null, false) && c.GetDoor(map) == null;
		}

		// Token: 0x04005442 RID: 21570
		private const float RandomCellNearRadius = 1.9f;
	}
}
