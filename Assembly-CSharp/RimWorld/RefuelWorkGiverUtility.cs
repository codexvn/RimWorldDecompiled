using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001605 RID: 5637
	public static class RefuelWorkGiverUtility
	{
		// Token: 0x0600850B RID: 34059 RVA: 0x0027D370 File Offset: 0x0027B570
		public static bool CanRefuel(Pawn pawn, Thing t, bool forced = false)
		{
			CompRefuelable compRefuelable = t.TryGetComp<CompRefuelable>();
			if (compRefuelable == null || compRefuelable.parent.Fogged() || compRefuelable.IsFull || (!forced && !compRefuelable.allowAutoRefuel))
			{
				return false;
			}
			if (compRefuelable.FuelPercentOfMax > 0f && !compRefuelable.Props.allowRefuelIfNotEmpty)
			{
				return false;
			}
			if (!forced && !compRefuelable.ShouldAutoRefuelNow)
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (t.Faction != pawn.Faction)
			{
				return false;
			}
			CompInteractable compInteractable;
			if (t.TryGetComp(out compInteractable) && compInteractable.Props.cooldownPreventsRefuel && compInteractable.OnCooldown)
			{
				JobFailReason.Is(compInteractable.Props.onCooldownString.CapitalizeFirst(), null);
				return false;
			}
			if (RefuelWorkGiverUtility.FindBestFuel(pawn, t) == null)
			{
				ThingFilter fuelFilter = compRefuelable.Props.fuelFilter;
				JobFailReason.Is("NoFuelToRefuel".Translate(fuelFilter.Summary), null);
				return false;
			}
			if (compRefuelable.Props.atomicFueling && RefuelWorkGiverUtility.FindAllFuel(pawn, t) == null)
			{
				ThingFilter fuelFilter2 = compRefuelable.Props.fuelFilter;
				JobFailReason.Is("NoFuelToRefuel".Translate(fuelFilter2.Summary), null);
				return false;
			}
			return true;
		}

		// Token: 0x0600850C RID: 34060 RVA: 0x0027D4AC File Offset: 0x0027B6AC
		public static Job RefuelJob(Pawn pawn, Thing t, bool forced = false, JobDef customRefuelJob = null, JobDef customAtomicRefuelJob = null)
		{
			if (!t.TryGetComp<CompRefuelable>().Props.atomicFueling)
			{
				Thing t2 = RefuelWorkGiverUtility.FindBestFuel(pawn, t);
				return JobMaker.MakeJob(customRefuelJob ?? JobDefOf.Refuel, t, t2);
			}
			List<Thing> source = RefuelWorkGiverUtility.FindAllFuel(pawn, t);
			Job job = JobMaker.MakeJob(customAtomicRefuelJob ?? JobDefOf.RefuelAtomic, t);
			job.targetQueueB = (from f in source
			select new LocalTargetInfo(f)).ToList<LocalTargetInfo>();
			return job;
		}

		// Token: 0x0600850D RID: 34061 RVA: 0x0027D540 File Offset: 0x0027B740
		private static Thing FindBestFuel(Pawn pawn, Thing refuelable)
		{
			RefuelWorkGiverUtility.<>c__DisplayClass2_0 CS$<>8__locals1 = new RefuelWorkGiverUtility.<>c__DisplayClass2_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.filter = refuelable.TryGetComp<CompRefuelable>().Props.fuelFilter;
			return GenClosest.ClosestThingReachable(CS$<>8__locals1.pawn.Position, CS$<>8__locals1.pawn.Map, CS$<>8__locals1.filter.BestThingRequest, PathEndMode.ClosestTouch, TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, new Predicate<Thing>(CS$<>8__locals1.<FindBestFuel>g__Validator|0), null, 0, -1, false, RegionType.Set_Passable, false, false);
		}

		// Token: 0x0600850E RID: 34062 RVA: 0x0027D5C4 File Offset: 0x0027B7C4
		private static List<Thing> FindAllFuel(Pawn pawn, Thing refuelable)
		{
			int fuelCountToFullyRefuel = refuelable.TryGetComp<CompRefuelable>().GetFuelCountToFullyRefuel();
			ThingFilter filter = refuelable.TryGetComp<CompRefuelable>().Props.fuelFilter;
			return RefuelWorkGiverUtility.FindEnoughReservableThings(pawn, refuelable.Position, new IntRange(fuelCountToFullyRefuel, fuelCountToFullyRefuel), (Thing t) => filter.Allows(t));
		}

		// Token: 0x0600850F RID: 34063 RVA: 0x0027D618 File Offset: 0x0027B818
		public static List<Thing> FindEnoughReservableThings(Pawn pawn, IntVec3 rootCell, IntRange desiredQuantity, Predicate<Thing> validThing)
		{
			RefuelWorkGiverUtility.<>c__DisplayClass4_0 CS$<>8__locals1 = new RefuelWorkGiverUtility.<>c__DisplayClass4_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.validThing = validThing;
			CS$<>8__locals1.desiredQuantity = desiredQuantity;
			Region region = rootCell.GetRegion(CS$<>8__locals1.pawn.Map, RegionType.Set_Passable);
			CS$<>8__locals1.traverseParams = TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true);
			CS$<>8__locals1.chosenThings = new List<Thing>();
			CS$<>8__locals1.accumulatedQuantity = 0;
			CS$<>8__locals1.<FindEnoughReservableThings>g__ThingListProcessor|2(rootCell.GetThingList(region.Map), region);
			if (CS$<>8__locals1.accumulatedQuantity < CS$<>8__locals1.desiredQuantity.max)
			{
				RegionTraverser.BreadthFirstTraverse(region, new RegionEntryPredicate(CS$<>8__locals1.<FindEnoughReservableThings>g__EntryCondition|1), new RegionProcessor(CS$<>8__locals1.<FindEnoughReservableThings>g__RegionProcessor|3), 99999, RegionType.Set_Passable);
			}
			if (CS$<>8__locals1.accumulatedQuantity >= CS$<>8__locals1.desiredQuantity.min)
			{
				return CS$<>8__locals1.chosenThings;
			}
			return null;
		}
	}
}
