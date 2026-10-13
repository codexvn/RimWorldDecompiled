using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001435 RID: 5173
	public class JobGiver_DeliverPawnToPsychicRitualCell : JobGiver_DeliverPawnToCell
	{
		// Token: 0x06007D90 RID: 32144 RVA: 0x0025CFD6 File Offset: 0x0025B1D6
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_DeliverPawnToPsychicRitualCell jobGiver_DeliverPawnToPsychicRitualCell = (JobGiver_DeliverPawnToPsychicRitualCell)base.DeepCopy(resolve);
			jobGiver_DeliverPawnToPsychicRitualCell.skipIfTargetCanReach = this.skipIfTargetCanReach;
			return jobGiver_DeliverPawnToPsychicRitualCell;
		}

		// Token: 0x06007D91 RID: 32145 RVA: 0x0025CFF0 File Offset: 0x0025B1F0
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawn2 = pawn.mindState.duty.focusSecond.Pawn;
			if (pawn2 == null || pawn2.Dead)
			{
				return null;
			}
			if (pawn2.GetLord() != pawn.GetLord())
			{
				return null;
			}
			if (this.skipIfTargetCanReach && !pawn2.Downed && !pawn2.IsPrisoner)
			{
				return null;
			}
			if (pawn2.mindState.duty == null)
			{
				return null;
			}
			LocalTargetInfo destination = this.GetDestination(pawn2);
			if (!destination.IsValid || pawn2.Position == destination.Cell)
			{
				return null;
			}
			if (!pawn.CanReach(pawn2, PathEndMode.OnCell, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger), false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.DeliverToCell, pawn2, destination).WithCount(1);
			job.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, this.locomotionUrgency);
			job.expiryInterval = this.jobMaxDuration;
			return job;
		}

		// Token: 0x0400545D RID: 21597
		public bool skipIfTargetCanReach;
	}
}
