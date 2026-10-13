using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001434 RID: 5172
	public class JobGiver_DeliverPawnToCell : JobGiver_GotoTravelDestination
	{
		// Token: 0x06007D8D RID: 32141 RVA: 0x0025CEAC File Offset: 0x0025B0AC
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_DeliverPawnToCell jobGiver_DeliverPawnToCell = (JobGiver_DeliverPawnToCell)base.DeepCopy(resolve);
			jobGiver_DeliverPawnToCell.addArrivalTagIfTargetIsDead = this.addArrivalTagIfTargetIsDead;
			jobGiver_DeliverPawnToCell.addArrivalTagIfInvalidTarget = this.addArrivalTagIfInvalidTarget;
			return jobGiver_DeliverPawnToCell;
		}

		// Token: 0x06007D8E RID: 32142 RVA: 0x0025CED4 File Offset: 0x0025B0D4
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawn2 = pawn.mindState.duty.focusSecond.Pawn;
			if ((this.addArrivalTagIfTargetIsDead && pawn2.Dead) || pawn2.Position == pawn.mindState.duty.focus.Cell || (this.addArrivalTagIfInvalidTarget && !pawn.mindState.duty.focus.IsValid))
			{
				RitualUtility.AddArrivalTag(pawn);
				RitualUtility.AddArrivalTag(pawn2);
				return null;
			}
			if (!pawn.CanReach(pawn2, PathEndMode.Touch, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger), false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.DeliverToCell, pawn2, pawn.mindState.duty.focus, pawn.mindState.duty.focusThird);
			job.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, this.locomotionUrgency);
			job.expiryInterval = this.jobMaxDuration;
			job.count = 1;
			job.ritualTag = "Arrived";
			return job;
		}

		// Token: 0x0400545B RID: 21595
		public bool addArrivalTagIfTargetIsDead;

		// Token: 0x0400545C RID: 21596
		public bool addArrivalTagIfInvalidTarget;
	}
}
