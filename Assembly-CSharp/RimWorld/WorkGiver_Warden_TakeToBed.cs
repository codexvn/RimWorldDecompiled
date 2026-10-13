using System;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001596 RID: 5526
	public class WorkGiver_Warden_TakeToBed : WorkGiver_Warden
	{
		// Token: 0x060082CD RID: 33485 RVA: 0x00274A6D File Offset: 0x00272C6D
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfPrisoner(pawn, t, false))
			{
				return null;
			}
			return WorkGiver_Warden_TakeToBed.TryMakeJob(pawn, t, forced);
		}

		// Token: 0x060082CE RID: 33486 RVA: 0x00274A84 File Offset: 0x00272C84
		public static Job TryMakeJob(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = (Pawn)t;
			Job job = WorkGiver_Warden_TakeToBed.TakeDownedToBedJob(pawn2, pawn);
			if (job != null)
			{
				return job;
			}
			Job job2 = WorkGiver_Warden_TakeToBed.TakeToPreferredBedJob(pawn2, pawn);
			if (job2 != null)
			{
				return job2;
			}
			if (pawn2.ownership.OwnedBed == null)
			{
				JobFailReason.Is("NoPrisonerBedShort".Translate(), null);
			}
			return null;
		}

		// Token: 0x060082CF RID: 33487 RVA: 0x00274AD8 File Offset: 0x00272CD8
		private static Job TakeToPreferredBedJob(Pawn prisoner, Pawn warden)
		{
			if (prisoner.Downed || !warden.CanReserve(prisoner, 1, -1, null, false))
			{
				return null;
			}
			if (RestUtility.FindBedFor(prisoner, prisoner, true, false, new GuestStatus?(GuestStatus.Prisoner)) != null)
			{
				return null;
			}
			Room room = prisoner.GetRoom(RegionType.Set_All);
			Building_Bed building_Bed = RestUtility.FindBedFor(prisoner, warden, false, false, new GuestStatus?(GuestStatus.Prisoner));
			if (building_Bed != null && building_Bed.GetRoom(RegionType.Set_All) != room)
			{
				Job job = JobMaker.MakeJob(JobDefOf.EscortPrisonerToBed, prisoner, building_Bed);
				job.count = 1;
				return job;
			}
			return null;
		}

		// Token: 0x060082D0 RID: 33488 RVA: 0x00274B5C File Offset: 0x00272D5C
		private static Job TakeDownedToBedJob(Pawn prisoner, Pawn warden)
		{
			if (!prisoner.Downed || !HealthAIUtility.ShouldSeekMedicalRest(prisoner) || prisoner.InBed() || !warden.CanReserve(prisoner, 1, -1, null, false))
			{
				return null;
			}
			Building_Bed building_Bed = RestUtility.FindBedFor(prisoner, warden, true, false, new GuestStatus?(GuestStatus.Prisoner));
			if (building_Bed != null)
			{
				Job job = JobMaker.MakeJob(JobDefOf.TakeWoundedPrisonerToBed, prisoner, building_Bed);
				job.count = 1;
				return job;
			}
			return null;
		}

		// Token: 0x060082D1 RID: 33489 RVA: 0x00274BC8 File Offset: 0x00272DC8
		public static void TryTakePrisonerToBed(Pawn prisoner, Pawn warden)
		{
			if (!prisoner.Spawned || prisoner.InAggroMentalState || prisoner.IsForbidden(warden) || prisoner.IsFormingCaravan() || !warden.CanReserveAndReach(prisoner, PathEndMode.OnCell, warden.NormalMaxDanger(), 1, -1, null, true))
			{
				return;
			}
			Job job = WorkGiver_Warden_TakeToBed.TryMakeJob(warden, prisoner, true);
			if (job != null)
			{
				warden.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
			}
		}
	}
}
