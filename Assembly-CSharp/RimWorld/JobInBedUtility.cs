using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013A0 RID: 5024
	public static class JobInBedUtility
	{
		// Token: 0x06007B3D RID: 31549 RVA: 0x002517E8 File Offset: 0x0024F9E8
		public static void KeepLyingDown(this JobDriver driver, TargetIndex bedIndex)
		{
			driver.AddFinishAction(delegate(JobCondition jobCondition)
			{
				Pawn pawn = driver.pawn;
				if (!pawn.Drafted)
				{
					pawn.jobs.jobQueue.EnqueueFirst(JobMaker.MakeJob(JobDefOf.LayDown, pawn.CurJob.GetTarget(bedIndex)), null);
				}
			});
		}

		// Token: 0x06007B3E RID: 31550 RVA: 0x00251820 File Offset: 0x0024FA20
		public static bool InBedOrRestSpotNow(Pawn pawn, LocalTargetInfo bedOrRestSpot)
		{
			if (!bedOrRestSpot.IsValid || !pawn.Spawned)
			{
				return false;
			}
			if (bedOrRestSpot.HasThing)
			{
				return bedOrRestSpot.Thing.Map == pawn.Map && RestUtility.GetBedSleepingSlotPosFor(pawn, (Building_Bed)bedOrRestSpot.Thing) == pawn.Position;
			}
			return bedOrRestSpot.Cell == pawn.Position;
		}
	}
}
