using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001616 RID: 5654
	public class WorkGiver_TakeEntityToHoldingPlatform : WorkGiver_Scanner
	{
		// Token: 0x06008564 RID: 34148 RVA: 0x0027D860 File Offset: 0x0027BA60
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.AnomalyActive;
		}

		// Token: 0x17001477 RID: 5239
		// (get) Token: 0x06008565 RID: 34149 RVA: 0x0027E28B File Offset: 0x0027C48B
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.HoldingPlatformTarget);
			}
		}

		// Token: 0x06008566 RID: 34150 RVA: 0x0027E294 File Offset: 0x0027C494
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				JobFailReason.Is("IncapableOfCapacity".Translate(PawnCapacityDefOf.Manipulation.label).CapitalizeFirst(), null);
				return false;
			}
			CompHoldingPlatformTarget compHoldingPlatformTarget = t.TryGetComp<CompHoldingPlatformTarget>();
			if (((compHoldingPlatformTarget != null) ? compHoldingPlatformTarget.targetHolder : null) == null || compHoldingPlatformTarget.targetHolder.Destroyed || compHoldingPlatformTarget.targetHolder.MapHeld != t.MapHeld || compHoldingPlatformTarget.EntityHolder.HeldPawn != null)
			{
				return false;
			}
			if (!pawn.CanReserveAndReach(t, PathEndMode.ClosestTouch, Danger.Deadly, 1, -1, null, forced))
			{
				return false;
			}
			if (!pawn.CanReserveAndReach(compHoldingPlatformTarget.targetHolder, PathEndMode.ClosestTouch, Danger.Deadly, 1, -1, null, forced))
			{
				return false;
			}
			Pawn pawn2 = t as Pawn;
			return pawn2 == null || pawn2.ThreatDisabled(pawn);
		}

		// Token: 0x06008567 RID: 34151 RVA: 0x0027E374 File Offset: 0x0027C574
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompHoldingPlatformTarget compHoldingPlatformTarget = t.TryGetComp<CompHoldingPlatformTarget>();
			if (compHoldingPlatformTarget == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.CarryToEntityHolder, compHoldingPlatformTarget.targetHolder, t);
			job.count = 1;
			return job;
		}
	}
}
