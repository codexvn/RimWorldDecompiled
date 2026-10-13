using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200146E RID: 5230
	public class JobGiver_TakeWoundedGuest : ThinkNode_JobGiver
	{
		// Token: 0x06007E2E RID: 32302 RVA: 0x0025FE94 File Offset: 0x0025E094
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			if (!RCellFinder.TryFindBestExitSpot(pawn, out c, TraverseMode.ByPawn, true))
			{
				return null;
			}
			Pawn pawn2 = KidnapAIUtility.ReachableWoundedGuest(pawn);
			if (pawn2 == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Kidnap);
			job.targetA = pawn2;
			job.targetB = c;
			job.count = 1;
			return job;
		}
	}
}
