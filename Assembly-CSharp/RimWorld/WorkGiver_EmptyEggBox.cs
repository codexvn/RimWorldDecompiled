using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015D6 RID: 5590
	public class WorkGiver_EmptyEggBox : WorkGiver_Scanner
	{
		// Token: 0x17001439 RID: 5177
		// (get) Token: 0x0600840A RID: 33802 RVA: 0x00279AB9 File Offset: 0x00277CB9
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.EggBox);
			}
		}

		// Token: 0x0600840B RID: 33803 RVA: 0x00279AC8 File Offset: 0x00277CC8
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!t.Spawned)
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			CompEggContainer compEggContainer = t.TryGetComp<CompEggContainer>();
			if (((compEggContainer != null) ? compEggContainer.ContainedThing : null) == null)
			{
				return false;
			}
			if (!compEggContainer.CanEmpty && !forced)
			{
				return false;
			}
			IntVec3 intVec;
			IHaulDestination haulDestination;
			if (!StoreUtility.TryFindBestBetterStorageFor(compEggContainer.ContainedThing, pawn, pawn.Map, StoragePriority.Unstored, pawn.Faction, out intVec, out haulDestination, false))
			{
				JobFailReason.Is(HaulAIUtility.NoEmptyPlaceLowerTrans, null);
				return false;
			}
			return true;
		}

		// Token: 0x0600840C RID: 33804 RVA: 0x00279B48 File Offset: 0x00277D48
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompEggContainer compEggContainer = t.TryGetComp<CompEggContainer>();
			if (((compEggContainer != null) ? compEggContainer.ContainedThing : null) == null)
			{
				return null;
			}
			if (!compEggContainer.CanEmpty && !forced)
			{
				return null;
			}
			IntVec3 c;
			IHaulDestination haulDestination;
			if (!StoreUtility.TryFindBestBetterStorageFor(compEggContainer.ContainedThing, pawn, pawn.Map, StoragePriority.Unstored, pawn.Faction, out c, out haulDestination, true))
			{
				JobFailReason.Is(HaulAIUtility.NoEmptyPlaceLowerTrans, null);
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.EmptyThingContainer, t, compEggContainer.ContainedThing, c);
			job.count = compEggContainer.ContainedThing.stackCount;
			return job;
		}
	}
}
