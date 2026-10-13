using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001433 RID: 5171
	public class JobGiver_DeliverPawnToBed : JobGiver_GotoTravelDestination
	{
		// Token: 0x06007D8A RID: 32138 RVA: 0x0025CD47 File Offset: 0x0025AF47
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_DeliverPawnToBed jobGiver_DeliverPawnToBed = (JobGiver_DeliverPawnToBed)base.DeepCopy(resolve);
			jobGiver_DeliverPawnToBed.addArrivalTagIfTargetIsDead = this.addArrivalTagIfTargetIsDead;
			jobGiver_DeliverPawnToBed.addArrivalTagIfNoBedAvailable = this.addArrivalTagIfNoBedAvailable;
			jobGiver_DeliverPawnToBed.ignoreOtherReservations = this.ignoreOtherReservations;
			return jobGiver_DeliverPawnToBed;
		}

		// Token: 0x06007D8B RID: 32139 RVA: 0x0025CD7C File Offset: 0x0025AF7C
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawn2 = pawn.mindState.duty.focusSecond.Pawn;
			if (pawn2 == null)
			{
				return null;
			}
			if (this.addArrivalTagIfTargetIsDead && pawn2.Dead)
			{
				RitualUtility.AddArrivalTag(pawn);
				RitualUtility.AddArrivalTag(pawn2);
				return null;
			}
			if (!pawn.CanReserveAndReach(pawn2, PathEndMode.Touch, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger), 1, -1, null, this.ignoreOtherReservations))
			{
				return null;
			}
			Building_Bed building_Bed = pawn.mindState.duty.focusThird.Thing as Building_Bed;
			if (building_Bed == null && pawn2.CurrentBed() != null)
			{
				return null;
			}
			building_Bed = (building_Bed ?? RestUtility.FindBedFor(pawn2, pawn, true, false, pawn2.GuestStatus));
			if (building_Bed != null && !RestUtility.IsValidBedFor(building_Bed, pawn2, pawn, true, false, false, null))
			{
				building_Bed = null;
			}
			if (building_Bed != null)
			{
				pawn2.ownership.ClaimBedIfNonMedical(building_Bed);
				Job job = JobMaker.MakeJob(JobDefOf.DeliverToBed, pawn2, building_Bed);
				job.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, this.locomotionUrgency);
				job.expiryInterval = this.jobMaxDuration;
				job.count = 1;
				job.ritualTag = "Arrived";
				return job;
			}
			if (this.addArrivalTagIfNoBedAvailable)
			{
				RitualUtility.AddArrivalTag(pawn);
				RitualUtility.AddArrivalTag(pawn2);
			}
			return null;
		}

		// Token: 0x04005458 RID: 21592
		public bool addArrivalTagIfTargetIsDead;

		// Token: 0x04005459 RID: 21593
		public bool addArrivalTagIfNoBedAvailable;

		// Token: 0x0400545A RID: 21594
		public bool ignoreOtherReservations;
	}
}
