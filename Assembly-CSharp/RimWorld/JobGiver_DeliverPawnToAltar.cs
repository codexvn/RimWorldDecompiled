using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001432 RID: 5170
	public class JobGiver_DeliverPawnToAltar : JobGiver_GotoTravelDestination
	{
		// Token: 0x06007D88 RID: 32136 RVA: 0x0025CC94 File Offset: 0x0025AE94
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!ModLister.CheckIdeology("Deliver to altar"))
			{
				return null;
			}
			Pawn pawn2 = pawn.mindState.duty.focusSecond.Pawn;
			if (!pawn.CanReach(pawn2.Position, PathEndMode.OnCell, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger), false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.DeliverToAltar, pawn2, pawn.mindState.duty.focus, pawn.mindState.duty.focusThird);
			job.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, this.locomotionUrgency);
			job.expiryInterval = this.jobMaxDuration;
			job.count = 1;
			return job;
		}
	}
}
