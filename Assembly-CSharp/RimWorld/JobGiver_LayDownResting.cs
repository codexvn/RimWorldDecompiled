using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001460 RID: 5216
	public class JobGiver_LayDownResting : ThinkNode_JobGiver
	{
		// Token: 0x06007E03 RID: 32259 RVA: 0x0025F344 File Offset: 0x0025D544
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c2;
			if (!GatheringsUtility.TryFindRandomCellInGatheringArea(pawn, (IntVec3 c) => pawn.CanReserveAndReach(c, PathEndMode.Touch, Danger.None, 1, -1, null, false), out c2))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.LayDownResting, c2);
			job.locomotionUrgency = LocomotionUrgency.Amble;
			return job;
		}
	}
}
