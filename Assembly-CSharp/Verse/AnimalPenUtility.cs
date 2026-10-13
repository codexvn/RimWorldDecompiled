using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020003AE RID: 942
	public static class AnimalPenUtility
	{
		// Token: 0x06001822 RID: 6178 RVA: 0x0007A398 File Offset: 0x00078598
		public static ThingFilter GetFixedAnimalFilter()
		{
			if (AnimalPenUtility.fixedFilter == null)
			{
				AnimalPenUtility.fixedFilter = new ThingFilter();
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs.Where(new Func<ThingDef, bool>(AnimalPenUtility.IsRopeManagedAnimalDef)))
				{
					AnimalPenUtility.fixedFilter.SetAllow(thingDef, true);
				}
			}
			return AnimalPenUtility.fixedFilter;
		}

		// Token: 0x06001823 RID: 6179 RVA: 0x0007A410 File Offset: 0x00078610
		public static bool IsRopeManagedAnimalDef(ThingDef td)
		{
			return td.race != null && td.race.Roamer && td.IsWithinCategory(ThingCategoryDefOf.Animals);
		}

		// Token: 0x06001824 RID: 6180 RVA: 0x0007A434 File Offset: 0x00078634
		private static bool ShouldBePennedByDefault(ThingDef td)
		{
			return AnimalPenUtility.IsRopeManagedAnimalDef(td) && td.race.FenceBlocked;
		}

		// Token: 0x06001825 RID: 6181 RVA: 0x0007A44B File Offset: 0x0007864B
		public static bool NeedsToBeManagedByRope(Pawn pawn)
		{
			return AnimalPenUtility.IsRopeManagedAnimalDef(pawn.def) && pawn.Spawned && pawn.Map.IsPlayerHome;
		}

		// Token: 0x06001826 RID: 6182 RVA: 0x0007A470 File Offset: 0x00078670
		public static ThingFilter GetDefaultAnimalFilter()
		{
			if (AnimalPenUtility.defaultFilter == null)
			{
				AnimalPenUtility.defaultFilter = new ThingFilter();
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs.Where(new Func<ThingDef, bool>(AnimalPenUtility.ShouldBePennedByDefault)))
				{
					AnimalPenUtility.defaultFilter.SetAllow(thingDef, true);
				}
			}
			return AnimalPenUtility.defaultFilter;
		}

		// Token: 0x06001827 RID: 6183 RVA: 0x0007A4E8 File Offset: 0x000786E8
		public static void ResetStaticData()
		{
			AnimalPenUtility.defaultFilter = null;
			AnimalPenUtility.fixedFilter = null;
		}

		// Token: 0x06001828 RID: 6184 RVA: 0x0007A4F8 File Offset: 0x000786F8
		public static CompAnimalPenMarker GetCurrentPenOf(Pawn animal, bool allowUnenclosedPens)
		{
			Map map = animal.Map;
			if (!map.listerBuildings.allBuildingsAnimalPenMarkers.Any<Building>())
			{
				return null;
			}
			List<District> list = AnimalPenUtility.tmpConnectedDistrictsCalc.CalculateConnectedDistricts(animal.Position, map);
			if (!list.Any<District>())
			{
				return null;
			}
			CompAnimalPenMarker compAnimalPenMarker = null;
			foreach (Building building in map.listerBuildings.allBuildingsAnimalPenMarkers)
			{
				if (list.Contains(building.Position.GetDistrict(map, RegionType.Set_Passable)))
				{
					CompAnimalPenMarker compAnimalPenMarker2 = building.TryGetComp<CompAnimalPenMarker>();
					if (AnimalPenUtility.CanUseAndReach(animal, compAnimalPenMarker2, allowUnenclosedPens, null) && (compAnimalPenMarker == null || (compAnimalPenMarker2.PenState.Enclosed && compAnimalPenMarker.PenState.Unenclosed) || map.cellIndices.CellToIndex(compAnimalPenMarker2.parent.Position) < map.cellIndices.CellToIndex(compAnimalPenMarker.parent.Position)))
					{
						compAnimalPenMarker = compAnimalPenMarker2;
					}
				}
			}
			AnimalPenUtility.tmpConnectedDistrictsCalc.Reset();
			return compAnimalPenMarker;
		}

		// Token: 0x06001829 RID: 6185 RVA: 0x0007A610 File Offset: 0x00078810
		public static bool AnySuitablePens(Pawn animal, bool allowUnenclosedPens)
		{
			foreach (Building thing in animal.Map.listerBuildings.allBuildingsAnimalPenMarkers)
			{
				CompAnimalPenMarker penMarker = thing.TryGetComp<CompAnimalPenMarker>();
				if (AnimalPenUtility.CanUseAndReach(animal, penMarker, allowUnenclosedPens, null))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600182A RID: 6186 RVA: 0x0007A680 File Offset: 0x00078880
		public static bool AnySuitableHitch(Pawn animal)
		{
			foreach (Building t in animal.Map.listerBuildings.allBuildingsHitchingPosts)
			{
				if (animal.CanReach(t, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600182B RID: 6187 RVA: 0x0007A6F0 File Offset: 0x000788F0
		public static CompAnimalPenMarker ClosestSuitablePen(Pawn animal, bool allowUnenclosedPens)
		{
			Map map = animal.Map;
			CompAnimalPenMarker compAnimalPenMarker = null;
			float num = 0f;
			foreach (Building thing in map.listerBuildings.allBuildingsAnimalPenMarkers)
			{
				CompAnimalPenMarker compAnimalPenMarker2 = thing.TryGetComp<CompAnimalPenMarker>();
				if (AnimalPenUtility.CanUseAndReach(animal, compAnimalPenMarker2, allowUnenclosedPens, null))
				{
					int num2 = animal.Position.DistanceToSquared(compAnimalPenMarker2.parent.Position);
					if (compAnimalPenMarker == null || (float)num2 < num)
					{
						compAnimalPenMarker = compAnimalPenMarker2;
						num = (float)num2;
					}
				}
			}
			return compAnimalPenMarker;
		}

		// Token: 0x0600182C RID: 6188 RVA: 0x0007A788 File Offset: 0x00078988
		private static bool CanUseAndReach(Pawn animal, CompAnimalPenMarker penMarker, bool allowUnenclosedPens, Pawn roper = null)
		{
			bool flag = false;
			return AnimalPenUtility.CheckUseAndReach(animal, penMarker, allowUnenclosedPens, roper, ref flag, ref flag, ref flag);
		}

		// Token: 0x0600182D RID: 6189 RVA: 0x0007A7A8 File Offset: 0x000789A8
		private static bool CheckUseAndReach(Pawn animal, CompAnimalPenMarker penMarker, bool allowUnenclosedPens, Pawn roper, ref bool foundEnclosed, ref bool foundUsable, ref bool foundReachable)
		{
			if (!allowUnenclosedPens && penMarker.PenState.Unenclosed)
			{
				return false;
			}
			foundEnclosed = true;
			if (!penMarker.AcceptsToPen(animal))
			{
				return false;
			}
			if (roper == null && penMarker.parent.IsForbidden(Faction.OfPlayer))
			{
				return false;
			}
			if (roper != null && penMarker.parent.IsForbidden(roper))
			{
				return false;
			}
			foundUsable = true;
			bool flag;
			if (roper == null)
			{
				TraverseParms traverseParams = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false).WithFenceblockedOf(animal);
				flag = animal.Map.reachability.CanReach(animal.Position, penMarker.parent, PathEndMode.Touch, traverseParams);
			}
			else
			{
				TraverseParms traverseParams2 = TraverseParms.For(roper, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true).WithFenceblockedOf(animal);
				flag = animal.Map.reachability.CanReach(animal.Position, penMarker.parent, PathEndMode.Touch, traverseParams2);
			}
			if (!flag)
			{
				return false;
			}
			foundReachable = true;
			return true;
		}

		// Token: 0x0600182E RID: 6190 RVA: 0x0007A888 File Offset: 0x00078A88
		public static CompAnimalPenMarker GetPenAnimalShouldBeTakenTo(Pawn roper, Pawn animal, out string jobFailReason, bool forced = false, bool canInteractWhileSleeping = true, bool allowUnenclosedPens = false, bool ignoreSkillRequirements = true, RopingPriority mode = RopingPriority.Closest, AnimalPenBalanceCalculator balanceCalculator = null)
		{
			jobFailReason = null;
			if (allowUnenclosedPens && mode == RopingPriority.Balanced)
			{
				Log.Warning("Cannot allow unenclosed pens in balanced mode");
				return null;
			}
			if (animal == roper)
			{
				return null;
			}
			if (animal == null || !AnimalPenUtility.NeedsToBeManagedByRope(animal))
			{
				return null;
			}
			if (animal.Faction != roper.Faction)
			{
				return null;
			}
			if (!forced && animal.roping.IsRopedByPawn && animal.roping.RopedByPawn != roper)
			{
				return null;
			}
			if (AnimalPenUtility.RopeAttachmentInteractionCell(roper, animal) == IntVec3.Invalid)
			{
				jobFailReason = "CantRopeAnimalCantTouch".Translate();
				return null;
			}
			if (!forced && !roper.CanReserve(animal, 1, -1, null, false))
			{
				return null;
			}
			CompAnimalPenMarker currentPenOf = AnimalPenUtility.GetCurrentPenOf(animal, allowUnenclosedPens);
			if (mode == RopingPriority.Closest && currentPenOf != null && currentPenOf.PenState.Enclosed)
			{
				return null;
			}
			if (!WorkGiver_InteractAnimal.CanInteractWithAnimal(roper, animal, out jobFailReason, forced, canInteractWhileSleeping, ignoreSkillRequirements, true))
			{
				return null;
			}
			Map map = animal.Map;
			AnimalPenBalanceCalculator animalPenBalanceCalculator = balanceCalculator ?? new AnimalPenBalanceCalculator(map, true);
			CompAnimalPenMarker compAnimalPenMarker = null;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			foreach (Building thing in map.listerBuildings.allBuildingsAnimalPenMarkers)
			{
				CompAnimalPenMarker compAnimalPenMarker2 = thing.TryGetComp<CompAnimalPenMarker>();
				flag2 = true;
				if (AnimalPenUtility.CheckUseAndReach(animal, compAnimalPenMarker2, allowUnenclosedPens, roper, ref flag3, ref flag4, ref flag5))
				{
					if (mode == RopingPriority.Closest)
					{
						if (compAnimalPenMarker == null || (compAnimalPenMarker2.PenState.Enclosed && compAnimalPenMarker.PenState.Unenclosed) || AnimalPenUtility.PenIsCloser(compAnimalPenMarker2, compAnimalPenMarker, animal))
						{
							compAnimalPenMarker = compAnimalPenMarker2;
						}
					}
					else if (mode == RopingPriority.Balanced)
					{
						if (currentPenOf != null && !animalPenBalanceCalculator.IsBetterPen(compAnimalPenMarker2, currentPenOf, true, animal))
						{
							flag = true;
						}
						else if (compAnimalPenMarker == null || animalPenBalanceCalculator.IsBetterPen(compAnimalPenMarker2, compAnimalPenMarker, false, animal))
						{
							compAnimalPenMarker = compAnimalPenMarker2;
							flag = false;
						}
					}
				}
			}
			if (currentPenOf != null && compAnimalPenMarker == currentPenOf)
			{
				return null;
			}
			if (compAnimalPenMarker == null)
			{
				if (flag)
				{
					jobFailReason = "CantRopeAnimalAlreadyInBestPen".Translate();
				}
				else if (!flag2)
				{
					jobFailReason = "CantRopeAnimalNoUsableReachablePens".Translate();
				}
				else if (!flag3)
				{
					jobFailReason = "CantRopeAnimalNoEnclosedPens".Translate();
				}
				else if (!flag4)
				{
					jobFailReason = "CantRopeAnimalNoAllowedPens".Translate();
				}
				else if (!flag5)
				{
					jobFailReason = "CantRopeAnimalNoReachablePens".Translate();
				}
				else
				{
					jobFailReason = "CantRopeAnimalNoUsableReachablePens".Translate();
				}
				return null;
			}
			return compAnimalPenMarker;
		}

		// Token: 0x0600182F RID: 6191 RVA: 0x0007AAE4 File Offset: 0x00078CE4
		public static Building GetHitchingPostAnimalShouldBeTakenTo(Pawn roper, Pawn animal, out string jobFailReason, bool forced = false)
		{
			jobFailReason = null;
			if (animal == roper)
			{
				return null;
			}
			if (animal == null || !AnimalPenUtility.IsRopeManagedAnimalDef(animal.def))
			{
				return null;
			}
			if (animal.Faction != roper.Faction)
			{
				return null;
			}
			if (!forced && animal.roping.IsRopedByPawn && animal.roping.RopedByPawn != roper)
			{
				return null;
			}
			if (AnimalPenUtility.RopeAttachmentInteractionCell(roper, animal) == IntVec3.Invalid)
			{
				jobFailReason = "CantRopeAnimalCantTouch".Translate();
				return null;
			}
			if (!forced && !roper.CanReserve(animal, 1, -1, null, false))
			{
				return null;
			}
			if (animal.roping.IsRopedToHitchingPost || AnimalPenUtility.GetCurrentPenOf(animal, false) != null)
			{
				return null;
			}
			if (!WorkGiver_InteractAnimal.CanInteractWithAnimal(roper, animal, out jobFailReason, forced, true, true, true))
			{
				return null;
			}
			Building building = null;
			foreach (Building building2 in animal.Map.listerBuildings.allBuildingsHitchingPosts)
			{
				if (!building2.IsForbidden(roper) && roper.CanReach(building2, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) && animal.Map.reachability.CanReach(animal.Position, building2, PathEndMode.Touch, TraverseParms.For(roper, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true).WithFenceblockedOf(animal)) && (building == null || (animal.Position - building2.Position).LengthManhattan < (animal.Position - building.Position).LengthManhattan))
				{
					building = building2;
				}
			}
			return building;
		}

		// Token: 0x06001830 RID: 6192 RVA: 0x0007AC84 File Offset: 0x00078E84
		private static bool PenIsCloser(CompAnimalPenMarker markerA, CompAnimalPenMarker markerB, Pawn animal)
		{
			return animal.Position.DistanceToSquared(markerA.parent.Position) < animal.Position.DistanceToSquared(markerB.parent.Position);
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x0007ACB4 File Offset: 0x00078EB4
		public static IntVec3 RopeAttachmentInteractionCell(Pawn roper, Pawn ropee)
		{
			if (!roper.Spawned || !ropee.Spawned)
			{
				return IntVec3.Invalid;
			}
			if (AnimalPenUtility.IsGoodRopeAttachmentInteractionCell(roper, ropee, roper.Position))
			{
				return roper.Position;
			}
			Map map = ropee.Map;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec = ropee.Position + GenAdj.CardinalDirections[i];
				if (intVec.InBounds(map) && AnimalPenUtility.MutuallyWalkable(roper, ropee, intVec))
				{
					return intVec;
				}
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x0007AD34 File Offset: 0x00078F34
		public static bool IsGoodRopeAttachmentInteractionCell(Pawn roper, Pawn ropee, IntVec3 cell)
		{
			return ropee.Position.AdjacentToCardinal(cell) && AnimalPenUtility.MutuallyWalkable(roper, ropee, cell);
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x0007AD5C File Offset: 0x00078F5C
		private static bool MutuallyWalkable(Pawn roper, Pawn ropee, IntVec3 c)
		{
			Map map = ropee.Map;
			return c.WalkableBy(map, ropee) && c.WalkableBy(map, roper);
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x0007AD84 File Offset: 0x00078F84
		public static IntVec3 FindPlaceInPenToStand(CompAnimalPenMarker marker, Pawn pawn)
		{
			AnimalPenUtility.<>c__DisplayClass21_0 CS$<>8__locals1 = new AnimalPenUtility.<>c__DisplayClass21_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.marker = marker;
			CS$<>8__locals1.map = CS$<>8__locals1.pawn.Map;
			CS$<>8__locals1.result = IntVec3.Invalid;
			RegionTraverser.BreadthFirstTraverse(CS$<>8__locals1.marker.parent.Position, CS$<>8__locals1.map, (Region from, Region to) => CS$<>8__locals1.marker.PenState.ContainsConnectedRegion(to), new RegionProcessor(CS$<>8__locals1.<FindPlaceInPenToStand>g__RegionProcessor|1), 999999, RegionType.Set_Passable);
			return CS$<>8__locals1.result;
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x0007AE04 File Offset: 0x00079004
		public static bool IsUnnecessarilyRoped(Pawn animal)
		{
			if (!AnimalPenUtility.NeedsToBeManagedByRope(animal))
			{
				return false;
			}
			if (animal.roping.IsRopedByPawn || animal.roping.IsRopedToHitchingPost || !animal.roping.IsRopedToSpot)
			{
				return false;
			}
			Lord lord = animal.GetLord();
			bool? flag;
			if (lord == null)
			{
				flag = null;
			}
			else
			{
				LordJob lordJob = lord.LordJob;
				flag = ((lordJob != null) ? new bool?(lordJob.ManagesRopableAnimals) : null);
			}
			bool? flag2 = flag;
			return !flag2.GetValueOrDefault();
		}

		// Token: 0x04001592 RID: 5522
		private static ThingFilter fixedFilter;

		// Token: 0x04001593 RID: 5523
		private static ThingFilter defaultFilter;

		// Token: 0x04001594 RID: 5524
		private static readonly AnimalPenConnectedDistrictsCalculator tmpConnectedDistrictsCalc = new AnimalPenConnectedDistrictsCalculator();
	}
}
