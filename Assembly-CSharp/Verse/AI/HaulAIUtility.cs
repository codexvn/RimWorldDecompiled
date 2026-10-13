using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000E6A RID: 3690
	public static class HaulAIUtility
	{
		// Token: 0x06005D01 RID: 23809 RVA: 0x001D9E14 File Offset: 0x001D8014
		public static void Reset()
		{
			HaulAIUtility.ForbiddenLowerTrans = "ForbiddenLower".Translate();
			HaulAIUtility.ForbiddenOutsideAllowedAreaLowerTrans = "ForbiddenOutsideAllowedAreaLower".Translate();
			HaulAIUtility.ReservedForPrisonersTrans = "ReservedForPrisoners".Translate();
			HaulAIUtility.BurningLowerTrans = "BurningLower".Translate();
			HaulAIUtility.NoEmptyPlaceLowerTrans = "NoEmptyPlaceLower".Translate();
			HaulAIUtility.ContainerFullLowerTrans = "ContainerFull".Translate();
		}

		// Token: 0x06005D02 RID: 23810 RVA: 0x001D9E99 File Offset: 0x001D8099
		public static bool IsInHaulableInventory(Thing thing)
		{
			return !thing.Spawned && thing.ParentHolder is IHaulSource;
		}

		// Token: 0x06005D03 RID: 23811 RVA: 0x001D9EB4 File Offset: 0x001D80B4
		public static bool PawnCanAutomaticallyHaul(Pawn p, Thing t, bool forced)
		{
			if (!t.def.EverHaulable)
			{
				return false;
			}
			if (t.Position.Fogged(t.Map))
			{
				return false;
			}
			if (t.IsForbidden(p))
			{
				if (!t.Position.InAllowedArea(p))
				{
					JobFailReason.Is(HaulAIUtility.ForbiddenOutsideAllowedAreaLowerTrans + "(" + p.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap.Label + ")", null);
				}
				else
				{
					JobFailReason.Is(HaulAIUtility.ForbiddenLowerTrans, null);
				}
				return false;
			}
			return (t.def.alwaysHaulable || t.Map.designationManager.DesignationOn(t, DesignationDefOf.Haul) != null || t.IsInValidStorage()) && HaulAIUtility.PawnCanAutomaticallyHaulFast(p, t, forced);
		}

		// Token: 0x06005D04 RID: 23812 RVA: 0x001D9F74 File Offset: 0x001D8174
		public static bool PawnCanAutomaticallyHaulFast(Pawn p, Thing t, bool forced)
		{
			if (t.Fogged())
			{
				return false;
			}
			if (!p.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (!p.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return false;
			}
			UnfinishedThing unfinishedThing = t as UnfinishedThing;
			if (unfinishedThing != null && unfinishedThing.BoundBill != null && unfinishedThing.BoundBill.billStack.FirstShouldDoNow == unfinishedThing.BoundBill)
			{
				Building building = unfinishedThing.BoundBill.billStack.billGiver as Building;
				if (building == null || (building.Spawned && building.OccupiedRect().ExpandedBy(1).Contains(unfinishedThing.Position)))
				{
					return false;
				}
			}
			using (ProfilerBlock.Scope("CanReach"))
			{
				if (!p.CanReach(t, PathEndMode.ClosestTouch, p.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
				{
					return false;
				}
			}
			if (t.def.IsNutritionGivingIngestible && t.def.ingestible.HumanEdible && !t.IsSociallyProper(p, false, true))
			{
				JobFailReason.Is(HaulAIUtility.ReservedForPrisonersTrans, null);
				return false;
			}
			if (t.IsBurning())
			{
				JobFailReason.Is(HaulAIUtility.BurningLowerTrans, null);
				return false;
			}
			return true;
		}

		// Token: 0x06005D05 RID: 23813 RVA: 0x001DA0BC File Offset: 0x001D82BC
		public static Job HaulToStorageJob(Pawn p, Thing t, bool forced)
		{
			StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(t, forced);
			IntVec3 storeCell;
			IHaulDestination haulDestination;
			if (!StoreUtility.TryFindBestBetterStorageFor(t, p, p.Map, currentPriority, p.Faction, out storeCell, out haulDestination, true))
			{
				JobFailReason.Is(HaulAIUtility.NoEmptyPlaceLowerTrans, null);
				return null;
			}
			if (haulDestination is ISlotGroupParent)
			{
				return HaulAIUtility.HaulToCellStorageJob(p, t, storeCell, false);
			}
			Thing thing = haulDestination as Thing;
			if (thing != null && thing.TryGetInnerInteractableThingOwner() != null)
			{
				return HaulAIUtility.HaulToContainerJob(p, t, thing);
			}
			Log.Error("Don't know how to handle HaulToStorageJob for storage " + haulDestination.ToStringSafe<IHaulDestination>() + ". thing=" + t.ToStringSafe<Thing>());
			return null;
		}

		// Token: 0x06005D06 RID: 23814 RVA: 0x001DA148 File Offset: 0x001D8348
		public static Job HaulToCellStorageJob(Pawn p, Thing t, IntVec3 storeCell, bool fitInStoreCell)
		{
			Job job = JobMaker.MakeJob(JobDefOf.HaulToCell, t, storeCell);
			ISlotGroup slotGroup = p.Map.haulDestinationManager.SlotGroupAt(storeCell);
			ISlotGroup storageGroup = slotGroup.StorageGroup;
			ISlotGroup slotGroup2 = storageGroup ?? slotGroup;
			if (p.Map.thingGrid.ThingAt(storeCell, t.def) != null)
			{
				if (fitInStoreCell)
				{
					job.count = storeCell.GetItemStackSpaceLeftFor(p.Map, t.def);
				}
				else
				{
					job.count = t.def.stackLimit;
				}
			}
			else
			{
				job.count = 99999;
			}
			int num = 0;
			float statValue = p.GetStatValue(StatDefOf.CarryingCapacity, true, -1);
			List<IntVec3> cellsList = slotGroup2.CellsList;
			for (int i = 0; i < cellsList.Count; i++)
			{
				if (StoreUtility.IsGoodStoreCell(cellsList[i], p.Map, t, p, p.Faction))
				{
					num += cellsList[i].GetItemStackSpaceLeftFor(p.Map, t.def);
					if (num >= job.count || (float)num >= statValue)
					{
						break;
					}
				}
			}
			job.count = Mathf.Min(job.count, num);
			job.haulOpportunisticDuplicates = true;
			job.haulMode = HaulMode.ToCellStorage;
			return job;
		}

		// Token: 0x06005D07 RID: 23815 RVA: 0x001DA278 File Offset: 0x001D8478
		public static Job HaulToContainerJob(Pawn p, Thing t, Thing container)
		{
			ThingOwner thingOwner = container.TryGetInnerInteractableThingOwner();
			if (thingOwner == null)
			{
				Log.Error(container.ToStringSafe<Thing>() + " gave null ThingOwner.");
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.HaulToContainer, t, container);
			job.count = Mathf.Min(t.stackCount, thingOwner.GetCountCanAccept(t, true));
			job.haulMode = HaulMode.ToContainer;
			return job;
		}

		// Token: 0x06005D08 RID: 23816 RVA: 0x001DA2DC File Offset: 0x001D84DC
		public static bool CanHaulAside(Pawn p, Thing t, out IntVec3 storeCell)
		{
			storeCell = IntVec3.Invalid;
			return t.def.EverHaulable && !t.IsBurning() && p.CanReserveAndReach(t, PathEndMode.ClosestTouch, p.NormalMaxDanger(), 1, -1, null, false) && HaulAIUtility.TryFindSpotToPlaceHaulableCloseTo(t, p, t.PositionHeld, out storeCell);
		}

		// Token: 0x06005D09 RID: 23817 RVA: 0x001DA33C File Offset: 0x001D853C
		public static Job HaulAsideJobFor(Pawn p, Thing t)
		{
			IntVec3 c;
			if (!HaulAIUtility.CanHaulAside(p, t, out c))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.HaulToCell, t, c);
			job.count = 99999;
			job.haulOpportunisticDuplicates = false;
			job.haulMode = HaulMode.ToCellNonStorage;
			job.ignoreDesignations = true;
			return job;
		}

		// Token: 0x06005D0A RID: 23818 RVA: 0x001DA38C File Offset: 0x001D858C
		private static bool TryFindSpotToPlaceHaulableCloseTo(Thing haulable, Pawn worker, IntVec3 center, out IntVec3 spot)
		{
			Region region = center.GetRegion(worker.Map, RegionType.Set_Passable);
			if (region == null)
			{
				spot = center;
				return false;
			}
			TraverseParms traverseParms = TraverseParms.For(worker, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true);
			IntVec3 foundCell = IntVec3.Invalid;
			Comparison<IntVec3> <>9__2;
			RegionTraverser.BreadthFirstTraverse(region, (Region from, Region r) => r.Allows(traverseParms, false), delegate(Region r)
			{
				HaulAIUtility.candidates.Clear();
				HaulAIUtility.candidates.AddRange(r.Cells);
				List<IntVec3> list = HaulAIUtility.candidates;
				Comparison<IntVec3> comparison;
				if ((comparison = <>9__2) == null)
				{
					comparison = (<>9__2 = ((IntVec3 a, IntVec3 b) => a.DistanceToSquared(center).CompareTo(b.DistanceToSquared(center))));
				}
				list.Sort(comparison);
				for (int i = 0; i < HaulAIUtility.candidates.Count; i++)
				{
					IntVec3 intVec = HaulAIUtility.candidates[i];
					if (HaulAIUtility.HaulablePlaceValidator(haulable, worker, intVec))
					{
						foundCell = intVec;
						return true;
					}
				}
				return false;
			}, 100, RegionType.Set_Passable);
			if (foundCell.IsValid)
			{
				spot = foundCell;
				return true;
			}
			spot = center;
			return false;
		}

		// Token: 0x06005D0B RID: 23819 RVA: 0x001DA44C File Offset: 0x001D864C
		private static bool HaulablePlaceValidator(Thing haulable, Pawn worker, IntVec3 c)
		{
			if (c.IsForbidden(worker))
			{
				return false;
			}
			if (!worker.CanReserveAndReach(c, PathEndMode.OnCell, worker.NormalMaxDanger(), 1, -1, null, false))
			{
				return false;
			}
			if (GenPlace.HaulPlaceBlockerIn(haulable, c, worker.Map, true) != null)
			{
				return false;
			}
			if (!c.Standable(worker.Map))
			{
				return false;
			}
			if (c == haulable.Position && haulable.Spawned)
			{
				return false;
			}
			if (c.ContainsStaticFire(worker.Map))
			{
				return false;
			}
			if (haulable != null && haulable.def.BlocksPlanting(false) && worker.Map.zoneManager.ZoneAt(c) is Zone_Growing)
			{
				return false;
			}
			if (haulable.def.passability != Traversability.Standable)
			{
				for (int i = 0; i < 8; i++)
				{
					IntVec3 c2 = c + GenAdj.AdjacentCells[i];
					if (worker.Map.designationManager.DesignationAt(c2, DesignationDefOf.Mine) != null || worker.Map.designationManager.DesignationAt(c2, DesignationDefOf.MineVein) != null)
					{
						return false;
					}
				}
			}
			Building edifice = c.GetEdifice(worker.Map);
			if (edifice != null && edifice is Building_Trap)
			{
				return false;
			}
			UnfinishedThing unfinishedThing = haulable as UnfinishedThing;
			if (unfinishedThing != null && unfinishedThing.BoundWorkTable != null)
			{
				if (unfinishedThing.BoundWorkTable.InteractionCell == c)
				{
					return false;
				}
				List<Thing> thingList = c.GetThingList(haulable.Map);
				for (int j = 0; j < thingList.Count; j++)
				{
					if (unfinishedThing.BoundWorkTable == thingList[j])
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06005D0C RID: 23820 RVA: 0x001DA5CC File Offset: 0x001D87CC
		public static void UpdateJobWithPlacedThings(Job curJob, Thing th, int added)
		{
			if (curJob.placedThings == null)
			{
				curJob.placedThings = new List<ThingCountClass>();
			}
			ThingCountClass thingCountClass = curJob.placedThings.Find((ThingCountClass x) => x.thing == th);
			if (thingCountClass != null)
			{
				thingCountClass.Count += added;
				return;
			}
			curJob.placedThings.Add(new ThingCountClass(th, added));
		}

		// Token: 0x06005D0D RID: 23821 RVA: 0x001DA63C File Offset: 0x001D883C
		public static List<Thing> FindFixedIngredientCount(Pawn pawn, ThingDef def, int maxCount)
		{
			HaulAIUtility.<>c__DisplayClass19_0 CS$<>8__locals1 = new HaulAIUtility.<>c__DisplayClass19_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.maxCount = maxCount;
			Region region = CS$<>8__locals1.pawn.Position.GetRegion(CS$<>8__locals1.pawn.Map, RegionType.Set_Passable);
			CS$<>8__locals1.chosenThings = new List<Thing>();
			CS$<>8__locals1.countFound = 0;
			CS$<>8__locals1.thingRequest = ThingRequest.ForDef(def);
			CS$<>8__locals1.<FindFixedIngredientCount>g__ThingListProcessor|0(CS$<>8__locals1.pawn.Position.GetThingList(region.Map), region);
			if (CS$<>8__locals1.countFound < CS$<>8__locals1.maxCount)
			{
				RegionTraverser.BreadthFirstTraverse(region, (Region from, Region r) => r.Allows(TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), false), (Region r) => base.<FindFixedIngredientCount>g__ThingListProcessor|0(r.ListerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.HaulableEver)), r), 99999, RegionType.Set_Passable);
			}
			return CS$<>8__locals1.chosenThings;
		}

		// Token: 0x0400432D RID: 17197
		private static string ForbiddenLowerTrans;

		// Token: 0x0400432E RID: 17198
		private static string ForbiddenOutsideAllowedAreaLowerTrans;

		// Token: 0x0400432F RID: 17199
		private static string ReservedForPrisonersTrans;

		// Token: 0x04004330 RID: 17200
		private static string BurningLowerTrans;

		// Token: 0x04004331 RID: 17201
		public static string NoEmptyPlaceLowerTrans;

		// Token: 0x04004332 RID: 17202
		public static string ContainerFullLowerTrans;

		// Token: 0x04004333 RID: 17203
		private static List<IntVec3> candidates = new List<IntVec3>();
	}
}
