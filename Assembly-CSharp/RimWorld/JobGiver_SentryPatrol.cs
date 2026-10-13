using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013E4 RID: 5092
	public class JobGiver_SentryPatrol : ThinkNode_JobGiver
	{
		// Token: 0x06007C54 RID: 31828 RVA: 0x00257170 File Offset: 0x00255370
		protected override Job TryGiveJob(Pawn pawn)
		{
			CompSentryDrone comp = pawn.GetComp<CompSentryDrone>();
			if (comp == null)
			{
				return null;
			}
			if (comp.Mode != CompSentryDrone.SentryDroneMode.Patrol)
			{
				return null;
			}
			if (pawn.CurJobDef == JobDefOf.GotoPatrolDest && pawn.CurJob.jobGiver == this)
			{
				LocalTargetInfo destination = pawn.pather.Destination;
				if (destination.IsValid && destination.Cell.DistanceTo(pawn.Position) >= 1f)
				{
					return pawn.CurJob;
				}
			}
			IntVec3 nextPatrolDest = comp.GetNextPatrolDest();
			if (!nextPatrolDest.IsValid)
			{
				return null;
			}
			if (!pawn.CanReach(nextPatrolDest, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.GotoPatrolDest, nextPatrolDest);
		}
	}
}
