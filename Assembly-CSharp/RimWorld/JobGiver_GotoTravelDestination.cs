using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200145A RID: 5210
	public class JobGiver_GotoTravelDestination : ThinkNode_JobGiver
	{
		// Token: 0x06007DF3 RID: 32243 RVA: 0x0025ECE4 File Offset: 0x0025CEE4
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_GotoTravelDestination jobGiver_GotoTravelDestination = (JobGiver_GotoTravelDestination)base.DeepCopy(resolve);
			jobGiver_GotoTravelDestination.locomotionUrgency = this.locomotionUrgency;
			jobGiver_GotoTravelDestination.maxDanger = this.maxDanger;
			jobGiver_GotoTravelDestination.jobMaxDuration = this.jobMaxDuration;
			jobGiver_GotoTravelDestination.exactCell = this.exactCell;
			jobGiver_GotoTravelDestination.ritualTagOnArrival = this.ritualTagOnArrival;
			jobGiver_GotoTravelDestination.wanderOnArrival = this.wanderOnArrival;
			jobGiver_GotoTravelDestination.jobDef = this.jobDef;
			jobGiver_GotoTravelDestination.destinationFocusIndex = this.destinationFocusIndex;
			jobGiver_GotoTravelDestination.allowZeroLengthPaths = this.allowZeroLengthPaths;
			return jobGiver_GotoTravelDestination;
		}

		// Token: 0x06007DF4 RID: 32244 RVA: 0x0025ED6C File Offset: 0x0025CF6C
		protected virtual LocalTargetInfo GetDestination(Pawn pawn)
		{
			switch (this.destinationFocusIndex)
			{
			case 1:
				return pawn.mindState.duty.focus;
			case 2:
				return pawn.mindState.duty.focusSecond;
			case 3:
				return pawn.mindState.duty.focusThird;
			default:
				Log.Error(string.Format("Invalid focus index {0}.", this.destinationFocusIndex));
				return LocalTargetInfo.Invalid;
			}
		}

		// Token: 0x06007DF5 RID: 32245 RVA: 0x0025EDE8 File Offset: 0x0025CFE8
		protected override Job TryGiveJob(Pawn pawn)
		{
			pawn.mindState.nextMoveOrderIsWait = !pawn.mindState.nextMoveOrderIsWait;
			Pawn pawn2 = this.CaravanBabyToCarry(pawn);
			if (pawn2 != null)
			{
				Job job = JobMaker.MakeJob(JobDefOf.PickupToHold, pawn2);
				job.count = 1;
				return job;
			}
			if (pawn.mindState.nextMoveOrderIsWait && !this.exactCell)
			{
				if (!this.wanderOnArrival)
				{
					return null;
				}
				return this.GetWaitJob();
			}
			else
			{
				LocalTargetInfo destination = this.GetDestination(pawn);
				if (!pawn.CanReach(destination, PathEndMode.OnCell, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger), false, false, TraverseMode.ByPawn))
				{
					return null;
				}
				if (pawn.Downed)
				{
					return null;
				}
				if (!this.exactCell || !(pawn.Position == destination))
				{
					LocalTargetInfo targetA = destination;
					if (!this.exactCell)
					{
						targetA = CellFinder.RandomClosewalkCellNear(destination.Cell, pawn.Map, 6, null);
					}
					Job job2 = JobMaker.MakeJob(this.jobDef ?? JobDefOf.Goto, targetA);
					job2.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, this.locomotionUrgency);
					job2.expiryInterval = this.jobMaxDuration;
					job2.ritualTag = this.ritualTagOnArrival;
					return job2;
				}
				if (!this.ritualTagOnArrival.NullOrEmpty())
				{
					Lord lord = pawn.GetLord();
					LordJob_Ritual lordJob_Ritual = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual;
					if (lordJob_Ritual != null)
					{
						lordJob_Ritual.AddTagForPawn(pawn, this.ritualTagOnArrival);
					}
				}
				if (this.allowZeroLengthPaths)
				{
					return this.GetWaitJob();
				}
				return null;
			}
		}

		// Token: 0x06007DF6 RID: 32246 RVA: 0x0025EF48 File Offset: 0x0025D148
		private Job GetWaitJob()
		{
			Job job = JobMaker.MakeJob(JobDefOf.Wait_Wander);
			job.expiryInterval = this.WaitTicks.RandomInRange;
			return job;
		}

		// Token: 0x06007DF7 RID: 32247 RVA: 0x0025EF68 File Offset: 0x0025D168
		private Pawn CaravanBabyToCarry(Pawn pawn)
		{
			if (!pawn.RaceProps.Humanlike || pawn.Downed || !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) || (pawn.carryTracker != null && pawn.carryTracker.CarriedThing != null))
			{
				return null;
			}
			Lord lord = pawn.GetLord();
			if (lord == null)
			{
				return null;
			}
			LordJob_FormAndSendCaravan lordJob_FormAndSendCaravan = lord.LordJob as LordJob_FormAndSendCaravan;
			List<Pawn> list = (lordJob_FormAndSendCaravan != null) ? lordJob_FormAndSendCaravan.downedPawns : null;
			if (list.NullOrEmpty<Pawn>())
			{
				return null;
			}
			foreach (Pawn pawn2 in list)
			{
				if (pawn2.Downed && pawn2 != pawn && pawn2.DevelopmentalStage.Baby() && pawn.CanReserveAndReach(pawn2, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false) && pawn2.Spawned)
				{
					return pawn2;
				}
			}
			return null;
		}

		// Token: 0x0400548E RID: 21646
		protected LocomotionUrgency locomotionUrgency = LocomotionUrgency.Walk;

		// Token: 0x0400548F RID: 21647
		protected Danger maxDanger = Danger.Some;

		// Token: 0x04005490 RID: 21648
		protected int jobMaxDuration = 999999;

		// Token: 0x04005491 RID: 21649
		protected bool exactCell;

		// Token: 0x04005492 RID: 21650
		protected string ritualTagOnArrival;

		// Token: 0x04005493 RID: 21651
		protected bool wanderOnArrival = true;

		// Token: 0x04005494 RID: 21652
		protected JobDef jobDef;

		// Token: 0x04005495 RID: 21653
		protected int destinationFocusIndex = 1;

		// Token: 0x04005496 RID: 21654
		protected bool allowZeroLengthPaths;

		// Token: 0x04005497 RID: 21655
		private IntRange WaitTicks = new IntRange(30, 80);
	}
}
