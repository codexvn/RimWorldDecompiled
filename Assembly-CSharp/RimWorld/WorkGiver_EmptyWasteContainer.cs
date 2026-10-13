using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015D9 RID: 5593
	public class WorkGiver_EmptyWasteContainer : WorkGiver_Scanner
	{
		// Token: 0x06008416 RID: 33814 RVA: 0x00279D01 File Offset: 0x00277F01
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.WasteProducer);
		}

		// Token: 0x06008417 RID: 33815 RVA: 0x00279D15 File Offset: 0x00277F15
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.WasteProducer).Any<Thing>();
		}

		// Token: 0x06008418 RID: 33816 RVA: 0x00279D34 File Offset: 0x00277F34
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModLister.CheckBiotech("Empty waste container"))
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, this.GetReservationLayer(pawn, t), forced))
			{
				return false;
			}
			CompWasteProducer compWasteProducer = t.TryGetComp<CompWasteProducer>();
			if (compWasteProducer == null || !compWasteProducer.CanEmptyNow || compWasteProducer.Waste.stackCount <= 0)
			{
				return false;
			}
			IntVec3 intVec;
			IHaulDestination haulDestination;
			if (!StoreUtility.TryFindBestBetterStorageFor(compWasteProducer.Waste, pawn, pawn.Map, StoragePriority.Unstored, pawn.Faction, out intVec, out haulDestination, false))
			{
				JobFailReason.Is(HaulAIUtility.NoEmptyPlaceLowerTrans, null);
				return false;
			}
			return true;
		}

		// Token: 0x06008419 RID: 33817 RVA: 0x00279DC4 File Offset: 0x00277FC4
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompWasteProducer compWasteProducer = t.TryGetComp<CompWasteProducer>();
			if (compWasteProducer == null || !compWasteProducer.CanEmptyNow || compWasteProducer.Waste.stackCount <= 0)
			{
				return null;
			}
			IntVec3 c;
			IHaulDestination haulDestination;
			if (!StoreUtility.TryFindBestBetterStorageFor(compWasteProducer.Waste, pawn, pawn.Map, StoragePriority.Unstored, pawn.Faction, out c, out haulDestination, true))
			{
				JobFailReason.Is(HaulAIUtility.NoEmptyPlaceLowerTrans, null);
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.EmptyWasteContainer, t, compWasteProducer.Waste, c);
			job.count = compWasteProducer.Waste.stackCount;
			return job;
		}

		// Token: 0x0600841A RID: 33818 RVA: 0x00279E52 File Offset: 0x00278052
		public override ReservationLayerDef GetReservationLayer(Pawn pawn, LocalTargetInfo t)
		{
			return ReservationLayerDefOf.Empty;
		}
	}
}
