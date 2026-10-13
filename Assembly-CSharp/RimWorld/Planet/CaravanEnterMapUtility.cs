using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D47 RID: 15687
	public static class CaravanEnterMapUtility
	{
		// Token: 0x060155E1 RID: 87521 RVA: 0x00663938 File Offset: 0x00661B38
		public static void Enter(Caravan caravan, Map map, CaravanEnterMode enterMode, CaravanDropInventoryMode dropInventoryMode = CaravanDropInventoryMode.DoNotDrop, bool draftColonists = false, Predicate<IntVec3> extraCellValidator = null)
		{
			if (enterMode == CaravanEnterMode.None)
			{
				string[] array = new string[6];
				array[0] = "Caravan ";
				array[1] = ((caravan != null) ? caravan.ToString() : null);
				array[2] = " tried to enter map ";
				int num = 3;
				Map map2 = map;
				array[num] = ((map2 != null) ? map2.ToString() : null);
				array[4] = " with enter mode ";
				array[5] = enterMode.ToString();
				Log.Error(string.Concat(array));
				enterMode = CaravanEnterMode.Edge;
			}
			IntVec3 enterCell = CaravanEnterMapUtility.GetEnterCell(caravan, map, enterMode, extraCellValidator);
			Func<Pawn, IntVec3> spawnCellGetter = (Pawn p) => CellFinder.RandomSpawnCellForPawnNear(enterCell, map, 4);
			CaravanEnterMapUtility.Enter(caravan, map, spawnCellGetter, dropInventoryMode, draftColonists);
		}

		// Token: 0x060155E2 RID: 87522 RVA: 0x006639E8 File Offset: 0x00661BE8
		public static void Enter(Caravan caravan, Map map, Func<Pawn, IntVec3> spawnCellGetter, CaravanDropInventoryMode dropInventoryMode = CaravanDropInventoryMode.DoNotDrop, bool draftColonists = false)
		{
			CaravanEnterMapUtility.tmpPawns.Clear();
			CaravanEnterMapUtility.tmpPawns.AddRange(caravan.PawnsListForReading);
			Building_PassengerShuttle shuttle = caravan.Shuttle;
			if (shuttle != null)
			{
				CaravanInventoryUtility.GetOwnerOf(caravan, shuttle).inventory.innerContainer.Remove(shuttle);
			}
			for (int i = 0; i < CaravanEnterMapUtility.tmpPawns.Count; i++)
			{
				IntVec3 loc = spawnCellGetter(CaravanEnterMapUtility.tmpPawns[i]);
				GenSpawn.Spawn(CaravanEnterMapUtility.tmpPawns[i], loc, map, Rot4.Random, WipeMode.Vanish, false, false);
			}
			if (shuttle != null)
			{
				IntVec3 position;
				if (!CellFinder.TryFindRandomCellNear(CaravanEnterMapUtility.tmpPawns.RandomElement<Pawn>().Position, map, 20, (IntVec3 c) => RoyalTitlePermitWorker_CallShuttle.ShuttleCanLandHere(c, map, null, null), out position, -1) && CellFinder.TryFindRandomCell(map, (IntVec3 c) => RoyalTitlePermitWorker_CallShuttle.ShuttleCanLandHere(c, map, null, null), out position))
				{
					position = CaravanEnterMapUtility.tmpPawns.RandomElement<Pawn>().Position;
				}
				if (!GenPlace.TryPlaceThing(shuttle, position, map, ThingPlaceMode.Near, null, null, null, 1))
				{
					Log.Error("Failed to find a location to place the shuttle for caravan.");
				}
				CaravanShuttleUtility.LoadCaravanItemsIntoContainer(CaravanEnterMapUtility.tmpPawns, shuttle.TransporterComp.innerContainer);
			}
			if (dropInventoryMode == CaravanDropInventoryMode.DropInstantly)
			{
				CaravanEnterMapUtility.DropAllInventory(CaravanEnterMapUtility.tmpPawns);
			}
			else if (dropInventoryMode == CaravanDropInventoryMode.UnloadIndividually)
			{
				for (int j = 0; j < CaravanEnterMapUtility.tmpPawns.Count; j++)
				{
					CaravanEnterMapUtility.tmpPawns[j].inventory.UnloadEverything = true;
				}
			}
			if (draftColonists)
			{
				CaravanEnterMapUtility.DraftColonists(CaravanEnterMapUtility.tmpPawns);
			}
			if (!draftColonists && map.IsPlayerHome)
			{
				CaravanFormingUtility.LeadAnimalsToPen(CaravanEnterMapUtility.tmpPawns);
			}
			if (map.IsPlayerHome)
			{
				for (int k = 0; k < CaravanEnterMapUtility.tmpPawns.Count; k++)
				{
					if (CaravanEnterMapUtility.tmpPawns[k].IsPrisoner)
					{
						CaravanEnterMapUtility.tmpPawns[k].guest.WaitInsteadOfEscapingForDefaultTicks();
					}
				}
			}
			caravan.RemoveAllPawns();
			if (!caravan.Destroyed)
			{
				caravan.Destroy();
			}
			CaravanEnterMapUtility.tmpPawns.Clear();
		}

		// Token: 0x060155E3 RID: 87523 RVA: 0x00663BFB File Offset: 0x00661DFB
		private static IntVec3 GetEnterCell(Caravan caravan, Map map, CaravanEnterMode enterMode, Predicate<IntVec3> extraCellValidator)
		{
			if (enterMode == CaravanEnterMode.Edge)
			{
				return CaravanEnterMapUtility.FindNearEdgeCell(map, extraCellValidator);
			}
			if (enterMode != CaravanEnterMode.Center)
			{
				throw new NotImplementedException("CaravanEnterMode");
			}
			return CaravanEnterMapUtility.FindCenterCell(map, extraCellValidator);
		}

		// Token: 0x060155E4 RID: 87524 RVA: 0x00663C24 File Offset: 0x00661E24
		private static IntVec3 FindNearEdgeCell(Map map, Predicate<IntVec3> extraCellValidator)
		{
			Predicate<IntVec3> baseValidator = (IntVec3 x) => x.Standable(map);
			Faction hostFaction = map.ParentFaction;
			IntVec3 root;
			if (CellFinder.TryFindRandomEdgeCellWith((IntVec3 x) => baseValidator(x) && (extraCellValidator == null || extraCellValidator(x)) && ((hostFaction != null && map.reachability.CanReachFactionBase(x, hostFaction)) || (hostFaction == null && map.reachability.CanReachBiggestMapEdgeDistrict(x))), map, CellFinder.EdgeRoadChance_Neutral, out root))
			{
				return CellFinder.RandomClosewalkCellNear(root, map, 5, null);
			}
			if (extraCellValidator != null && CellFinder.TryFindRandomEdgeCellWith((IntVec3 x) => baseValidator(x) && extraCellValidator(x), map, CellFinder.EdgeRoadChance_Neutral, out root))
			{
				return CellFinder.RandomClosewalkCellNear(root, map, 5, null);
			}
			if (CellFinder.TryFindRandomEdgeCellWith(baseValidator, map, CellFinder.EdgeRoadChance_Neutral, out root))
			{
				return CellFinder.RandomClosewalkCellNear(root, map, 5, null);
			}
			Log.Warning("Could not find any valid edge cell.");
			return CellFinder.RandomCell(map);
		}

		// Token: 0x060155E5 RID: 87525 RVA: 0x00663D0C File Offset: 0x00661F0C
		private static IntVec3 FindCenterCell(Map map, Predicate<IntVec3> extraCellValidator)
		{
			TraverseParms traverseParms = TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false).WithFenceblocked(true);
			Predicate<IntVec3> baseValidator = (IntVec3 x) => x.Standable(map) && map.reachability.CanReachMapEdge(x, traverseParms);
			IntVec3 result;
			if (extraCellValidator != null && RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => baseValidator(x) && extraCellValidator(x), map, out result))
			{
				return result;
			}
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith(baseValidator, map, out result))
			{
				return result;
			}
			Log.Warning("Could not find any valid cell.");
			return CellFinder.RandomCell(map);
		}

		// Token: 0x060155E6 RID: 87526 RVA: 0x00663DAC File Offset: 0x00661FAC
		public static void DropAllInventory(List<Pawn> pawns)
		{
			for (int i = 0; i < pawns.Count; i++)
			{
				pawns[i].inventory.DropAllNearPawn(pawns[i].Position, false, true);
			}
		}

		// Token: 0x060155E7 RID: 87527 RVA: 0x00663DEC File Offset: 0x00661FEC
		private static void DraftColonists(List<Pawn> pawns)
		{
			for (int i = 0; i < pawns.Count; i++)
			{
				if (pawns[i].IsColonist && !pawns[i].InMentalState)
				{
					pawns[i].drafter.Drafted = true;
				}
			}
		}

		// Token: 0x0400EDF2 RID: 60914
		private static List<Pawn> tmpPawns = new List<Pawn>();
	}
}
