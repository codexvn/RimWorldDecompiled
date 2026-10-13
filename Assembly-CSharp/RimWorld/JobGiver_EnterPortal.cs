using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001455 RID: 5205
	public class JobGiver_EnterPortal : ThinkNode_JobGiver
	{
		// Token: 0x06007DE5 RID: 32229 RVA: 0x0025E96C File Offset: 0x0025CB6C
		protected override Job TryGiveJob(Pawn pawn)
		{
			MapPortal mapPortal = pawn.mindState.duty.focus.Thing as MapPortal;
			if (mapPortal == null || mapPortal.Map != pawn.Map || !pawn.CanReach(mapPortal, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.EnterPortal, mapPortal);
			job.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, LocomotionUrgency.Jog);
			return job;
		}
	}
}
