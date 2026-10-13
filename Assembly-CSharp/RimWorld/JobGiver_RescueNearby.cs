using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014C0 RID: 5312
	public class JobGiver_RescueNearby : ThinkNode_JobGiver
	{
		// Token: 0x06007F35 RID: 32565 RVA: 0x00264FFC File Offset: 0x002631FC
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_RescueNearby jobGiver_RescueNearby = (JobGiver_RescueNearby)base.DeepCopy(resolve);
			jobGiver_RescueNearby.radius = this.radius;
			return jobGiver_RescueNearby;
		}

		// Token: 0x06007F36 RID: 32566 RVA: 0x00265018 File Offset: 0x00263218
		protected override Job TryGiveJob(Pawn pawn)
		{
			JobGiver_RescueNearby.<>c__DisplayClass3_0 CS$<>8__locals1 = new JobGiver_RescueNearby.<>c__DisplayClass3_0();
			CS$<>8__locals1.pawn = pawn;
			Pawn pawn2 = (Pawn)GenClosest.ClosestThingReachable(CS$<>8__locals1.pawn.Position, CS$<>8__locals1.pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.OnCell, TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), this.radius, new Predicate<Thing>(CS$<>8__locals1.<TryGiveJob>g__Validator|0), null, 0, -1, false, RegionType.Set_Passable, false, false);
			if (pawn2 == null)
			{
				return null;
			}
			Building_Bed building_Bed = RestUtility.FindBedFor(pawn2, CS$<>8__locals1.pawn, false, false, pawn2.GuestStatus);
			if (building_Bed == null || !pawn2.CanReserve(building_Bed, 1, -1, null, false))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Rescue, pawn2, building_Bed);
			job.count = 1;
			return job;
		}

		// Token: 0x04005530 RID: 21808
		private float radius = 30f;

		// Token: 0x04005531 RID: 21809
		private const float MinDistFromEnemy = 25f;
	}
}
