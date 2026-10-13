using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02001008 RID: 4104
	public class JobGiver_ForcedGoto : ThinkNode_JobGiver
	{
		// Token: 0x060064A9 RID: 25769 RVA: 0x001FE098 File Offset: 0x001FC298
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 forcedGotoPosition = pawn.mindState.forcedGotoPosition;
			if (!forcedGotoPosition.IsValid)
			{
				return null;
			}
			if (!pawn.CanReach(forcedGotoPosition, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				pawn.mindState.forcedGotoPosition = IntVec3.Invalid;
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Goto, forcedGotoPosition);
			job.locomotionUrgency = LocomotionUrgency.Walk;
			return job;
		}
	}
}
