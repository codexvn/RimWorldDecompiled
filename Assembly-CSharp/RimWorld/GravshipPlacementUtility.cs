using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02003659 RID: 13913
	public static class GravshipPlacementUtility
	{
		// Token: 0x06013514 RID: 79124 RVA: 0x005BDC48 File Offset: 0x005BBE48
		public static void PlaceGravshipInMap(Gravship gravship, IntVec3 root, Map map, out List<Thing> spawned)
		{
			spawned = null;
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			new ProfilerBlock("GravshipPlacementUtility.PlaceGravshipInMap");
			GravshipPlacementUtility.placingGravship = true;
			spawned = (from x in gravship.ThingPlacements
			select x.Key).ToList<Thing>();
			spawned.SortBy(new Func<Thing, int>(GravshipUtility.ThingSpawnPriority));
			map.regionAndRoomUpdater.Enabled = true;
			GravshipPlacementUtility.SpawnFoundations(gravship, map, root);
			GravshipPlacementUtility.SpawnTerrain(gravship, map, root);
			GravshipPlacementUtility.SpawnGas(gravship, map, root);
			GravshipPlacementUtility.CopyZonesIntoMap(gravship, map, root);
			GravshipPlacementUtility.CopyDesignations(gravship, map, root);
			GravshipPlacementUtility.SpawnNonPawnThings(gravship, map, spawned, root);
			GravshipPlacementUtility.SpawnPawns(gravship, map, root);
			GravshipPlacementUtility.SpawnRoofs(gravship, map, root);
			GravshipPlacementUtility.CopyAreasIntoMap(gravship, map, root);
			GravshipPlacementUtility.UpdateBillDestinations(map);
			GravshipPlacementUtility.PowerBuildings(gravship);
			gravship.StoryState.CopyTo(map.storyState);
			map.autoSlaughterManager.configs = gravship.autoSlaughterConfigs;
			map.autoSlaughterManager.Notify_ConfigChanged();
			map.substructureGrid.MarkDirty();
			GravshipPlacementUtility.placingGravship = false;
		}

		// Token: 0x06013515 RID: 79125 RVA: 0x005BDD58 File Offset: 0x005BBF58
		private static void SpawnFoundations(Gravship gravship, Map map, IntVec3 root)
		{
			using (new ProfilerBlock("Substructure Foundation"))
			{
				GravshipPlacementUtility.SpawnTerrain(map, root, gravship.Foundations, true);
			}
		}

		// Token: 0x06013516 RID: 79126 RVA: 0x005BDDA0 File Offset: 0x005BBFA0
		private static void SpawnTerrain(Gravship gravship, Map map, IntVec3 root)
		{
			using (new ProfilerBlock("Substructure Terrain"))
			{
				GravshipPlacementUtility.SpawnTerrain(map, root, gravship.Terrains, false);
				foreach (ValueTuple<IntVec3, ColorDef> valueTuple in gravship.TerrainColors)
				{
					IntVec3 item = valueTuple.Item1;
					ColorDef item2 = valueTuple.Item2;
					IntVec3 c = root + item;
					if (c.InBounds(map))
					{
						map.terrainGrid.SetTerrainColor(c, item2);
					}
				}
			}
		}

		// Token: 0x06013517 RID: 79127 RVA: 0x005BDE48 File Offset: 0x005BC048
		private static void SpawnPawns(Gravship gravship, Map map, IntVec3 root)
		{
			using (new ProfilerBlock("Pawns"))
			{
				foreach (KeyValuePair<Pawn, PositionData.Data> keyValuePair in gravship.PawnPlacements)
				{
					Pawn pawn;
					PositionData.Data data;
					keyValuePair.Deconstruct(out pawn, out data);
					Pawn pawn2 = pawn;
					PositionData.Data data2 = data;
					if (pawn2.Discarded || pawn2.Destroyed)
					{
						break;
					}
					GenSpawn.Spawn(pawn2, root + data2.local, map, data2.rotation, WipeMode.Vanish, false, false);
					if (pawn2.drafter != null)
					{
						pawn2.drafter.Drafted = data2.drafted;
					}
					if (pawn2.Downed || pawn2.Deathresting)
					{
						Building_Bed building_Bed = map.thingGrid.ThingAt<Building_Bed>(pawn2.Position);
						if (building_Bed != null)
						{
							RestUtility.TuckIntoBed(building_Bed, pawn2, pawn2, false);
						}
					}
				}
			}
		}

		// Token: 0x06013518 RID: 79128 RVA: 0x005BDF50 File Offset: 0x005BC150
		private static void SpawnNonPawnThings(Gravship gravship, Map map, List<Thing> gravshipThings, IntVec3 root)
		{
			using (new ProfilerBlock("Things"))
			{
				foreach (Thing thing in gravshipThings)
				{
					PositionData.Data data = gravship.ThingPlacements[thing];
					if (!thing.Destroyed)
					{
						PawnFlyer pawnFlyer = thing as PawnFlyer;
						if (pawnFlyer != null)
						{
							pawnFlyer.Notify_TransportedOnGravship(gravship);
						}
						GenSpawn.Spawn(thing, root + data.local, map, data.rotation, WipeMode.Vanish, false, false);
					}
				}
			}
		}

		// Token: 0x06013519 RID: 79129 RVA: 0x005BE004 File Offset: 0x005BC204
		private static void SpawnRoofs(Gravship gravship, Map map, IntVec3 root)
		{
			using (new ProfilerBlock("Roofs"))
			{
				foreach (ValueTuple<IntVec3, RoofDef> valueTuple in gravship.Roofs)
				{
					IntVec3 item = valueTuple.Item1;
					RoofDef item2 = valueTuple.Item2;
					map.roofGrid.SetRoof(root + item, item2);
				}
			}
		}

		// Token: 0x0601351A RID: 79130 RVA: 0x005BE090 File Offset: 0x005BC290
		private static void SpawnGas(Gravship gravship, Map map, IntVec3 root)
		{
			using (new ProfilerBlock("Gas"))
			{
				foreach (ValueTuple<IntVec3, uint> valueTuple in gravship.Gases)
				{
					IntVec3 item = valueTuple.Item1;
					uint item2 = valueTuple.Item2;
					IntVec3 intVec = root + item;
					map.gasGrid.SetDirect(intVec, item2);
					map.mapDrawer.MapMeshDirty(intVec, MapMeshFlagDefOf.Gas);
				}
			}
		}

		// Token: 0x0601351B RID: 79131 RVA: 0x005BE138 File Offset: 0x005BC338
		private static void PowerBuildings(Gravship gravship)
		{
			using (new ProfilerBlock("Power"))
			{
				foreach (KeyValuePair<Thing, Thing> keyValuePair in gravship.ConnectParents)
				{
					Thing thing;
					Thing thing2;
					keyValuePair.Deconstruct(out thing, out thing2);
					Thing thing3 = thing;
					Thing thing4 = thing2;
					if (thing4 != null && thing4.Spawned)
					{
						thing3.TryGetComp<CompPower>().ConnectToTransmitter(thing4.TryGetComp<CompPower>(), false);
					}
				}
				foreach (KeyValuePair<Thing, bool> keyValuePair2 in gravship.PoweredOn)
				{
					Thing thing2;
					bool flag;
					keyValuePair2.Deconstruct(out thing2, out flag);
					Thing thing5 = thing2;
					bool powerOn = flag;
					thing5.TryGetComp<CompPowerTrader>().PowerOn = powerOn;
				}
			}
		}

		// Token: 0x0601351C RID: 79132 RVA: 0x005BE234 File Offset: 0x005BC434
		internal static void PostSwapMap(Gravship gravship, List<Thing> gravshipThings)
		{
			foreach (Thing thing in gravshipThings)
			{
				if (thing != null && !thing.Destroyed)
				{
					thing.PostSwapMap();
				}
			}
			foreach (Pawn pawn in gravship.Pawns)
			{
				if (pawn != null && !pawn.Destroyed)
				{
					pawn.PostSwapMap();
				}
			}
		}

		// Token: 0x0601351D RID: 79133 RVA: 0x005BE2D4 File Offset: 0x005BC4D4
		private static void SpawnTerrain(Map map, IntVec3 root, Dictionary<IntVec3, TerrainDef> data, bool clear = false)
		{
			foreach (KeyValuePair<IntVec3, TerrainDef> keyValuePair in data)
			{
				IntVec3 intVec;
				TerrainDef terrainDef;
				keyValuePair.Deconstruct(out intVec, out terrainDef);
				IntVec3 b = intVec;
				TerrainDef terrainDef2 = terrainDef;
				IntVec3 intVec2 = root + b;
				if (intVec2.InBounds(map))
				{
					if (clear)
					{
						GravshipPlacementUtility.ClearThingsAt(intVec2, map, GravshipPlacementUtility.ClearMode.All);
					}
					if (ModsConfig.BiotechActive)
					{
						map.pollutionGrid.SetPolluted(intVec2, false, true);
					}
					map.snowGrid.SetDepth(intVec2, 0f);
					map.sandGrid.SetDepth(intVec2, 0f);
					if (!terrainDef2.isFoundation && !intVec2.SupportsStructureType(map, terrainDef2.terrainAffordanceNeeded))
					{
						map.terrainGrid.SetTerrain(intVec2, map.Biome.TerrainForAffordance(terrainDef2.terrainAffordanceNeeded));
					}
					if (terrainDef2.isFoundation)
					{
						if (map.terrainGrid.CanRemoveTopLayerAt(intVec2))
						{
							map.terrainGrid.RemoveTopLayer(intVec2, false);
						}
						map.terrainGrid.SetFoundation(intVec2, terrainDef2);
					}
					else
					{
						map.terrainGrid.SetTerrain(intVec2, terrainDef2);
					}
					map.fogGrid.FloodUnfogAdjacent(intVec2, true);
				}
			}
		}

		// Token: 0x0601351E RID: 79134 RVA: 0x005BE424 File Offset: 0x005BC624
		public static void ClearAreaForGravship(Map map, IntVec3 root, HashSet<IntVec3> clearCells)
		{
			GravshipPlacementUtility.ClearArea(map, root, clearCells, GravshipPlacementUtility.ClearMode.AllButNonTreePlants);
		}

		// Token: 0x0601351F RID: 79135 RVA: 0x005BE430 File Offset: 0x005BC630
		public static void ClearArea(Map map, IntVec3 root, HashSet<IntVec3> clearCells, GravshipPlacementUtility.ClearMode mode)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			bool flag;
			MapGenerator.TryGetVar<bool>("DontGenerateClearedGravShipTerrain", out flag);
			if (flag)
			{
				return;
			}
			foreach (IntVec3 b in clearCells)
			{
				IntVec3 intVec = root + b;
				if (intVec.InBounds(map))
				{
					if (mode == GravshipPlacementUtility.ClearMode.All || mode == GravshipPlacementUtility.ClearMode.AllButNonTreePlants)
					{
						if (ModsConfig.BiotechActive)
						{
							map.pollutionGrid.SetPolluted(intVec, false, true);
						}
						if (!intVec.SupportsStructureType(map, TerrainAffordanceDefOf.Walkable))
						{
							TerrainDef replacementTerrain = GravshipPlacementUtility.GetReplacementTerrain(intVec.GetTerrain(map), map);
							if (replacementTerrain != null)
							{
								map.terrainGrid.SetTerrain(intVec, replacementTerrain);
							}
						}
					}
					GravshipPlacementUtility.ClearThingsAt(intVec, map, mode);
					if (GravshipPlacementUtility.ShouldRemoveRoof(map, intVec, mode))
					{
						map.roofGrid.SetRoof(intVec, null);
					}
					if (map.roofGrid.RoofAt(intVec) == null)
					{
						map.fogGrid.FloodUnfogAdjacent(intVec, true);
					}
				}
			}
		}

		// Token: 0x06013520 RID: 79136 RVA: 0x005BE530 File Offset: 0x005BC730
		private static bool ShouldRemoveRoof(Map map, IntVec3 cell, GravshipPlacementUtility.ClearMode mode)
		{
			if (mode != GravshipPlacementUtility.ClearMode.NaturalRockOnly)
			{
				return true;
			}
			Room room = cell.GetRoom(map);
			if (room != null && room.ProperRoom)
			{
				return false;
			}
			Building edifice = cell.GetEdifice(map);
			return (edifice == null || edifice.def.building.isNaturalRock) && map.roofGrid.RoofAt(cell) != RoofDefOf.RoofConstructed;
		}

		// Token: 0x06013521 RID: 79137 RVA: 0x005BE594 File Offset: 0x005BC794
		private static TerrainDef GetReplacementTerrain(TerrainDef original, Map map)
		{
			if (original == TerrainDefOf.Space)
			{
				return null;
			}
			if (original.gravshipReplacementTerrain != null)
			{
				return original.gravshipReplacementTerrain;
			}
			if (original.passability == Traversability.Impassable || original.dangerous)
			{
				return map.Biome.TerrainForAffordance(TerrainAffordanceDefOf.Medium);
			}
			return null;
		}

		// Token: 0x06013522 RID: 79138 RVA: 0x005BE5D4 File Offset: 0x005BC7D4
		private static void ClearThingsAt(IntVec3 cell, Map map, GravshipPlacementUtility.ClearMode clearMode)
		{
			Zone zone = map.zoneManager.ZoneAt(cell);
			if (zone != null)
			{
				zone.RemoveCell(cell);
			}
			map.areaManager.BuildRoof[cell] = false;
			map.areaManager.NoRoof[cell] = false;
			List<Thing> list = map.thingGrid.ThingsListAt(cell);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				Thing thing = list[i];
				if (thing.def.destroyable && !thing.Destroyed)
				{
					switch (clearMode)
					{
					case GravshipPlacementUtility.ClearMode.All:
						thing.Destroy(DestroyMode.Vanish);
						break;
					case GravshipPlacementUtility.ClearMode.AllButNonTreePlants:
						if (thing.def.IsPlant)
						{
							PlantProperties plant = thing.def.plant;
							if (plant == null || !plant.IsTree)
							{
								break;
							}
						}
						thing.Destroy(DestroyMode.Vanish);
						break;
					case GravshipPlacementUtility.ClearMode.BlockingBuildingsOnly:
					{
						BuildingProperties building = thing.def.building;
						if (building != null && !building.canLandGravshipOn)
						{
							thing.Destroy(DestroyMode.Vanish);
						}
						break;
					}
					case GravshipPlacementUtility.ClearMode.NaturalRockOnly:
					{
						BuildingProperties building = thing.def.building;
						if (building != null && building.isNaturalRock)
						{
							thing.Destroy(DestroyMode.Vanish);
						}
						break;
					}
					}
				}
			}
		}

		// Token: 0x06013523 RID: 79139 RVA: 0x005BE6FC File Offset: 0x005BC8FC
		private static void CopyZonesIntoMap(Gravship gravship, Map map, IntVec3 root)
		{
			using (new ProfilerBlock("Zones"))
			{
				foreach (MoveableStockpile moveableStockpile in gravship.areas.stockpiles)
				{
					moveableStockpile.TryCreateStockpile(map.zoneManager, root);
				}
				foreach (MoveableGrowZone moveableGrowZone in gravship.areas.growZones)
				{
					moveableGrowZone.TryCreateGrowZone(map.zoneManager, root);
				}
			}
		}

		// Token: 0x06013524 RID: 79140 RVA: 0x005BE7CC File Offset: 0x005BC9CC
		private static void CopyAreasIntoMap(Gravship gravship, Map map, IntVec3 root)
		{
			using (new ProfilerBlock("Areas"))
			{
				foreach (MoveableArea_Allowed moveableArea_Allowed in gravship.areas.allowedAreas)
				{
					moveableArea_Allowed.TryCreateArea(map.areaManager, root);
				}
				if (gravship.areas.homeArea != null)
				{
					foreach (IntVec3 b in gravship.areas.homeArea.RelativeCells)
					{
						map.areaManager.Home[root + b] = true;
					}
					foreach (Pawn pawn in gravship.areas.homeArea.assignedPawns)
					{
						pawn.playerSettings.AreaRestrictionInPawnCurrentMap = map.areaManager.Home;
					}
				}
				if (gravship.areas.buildRoofArea != null)
				{
					foreach (IntVec3 b2 in gravship.areas.buildRoofArea.RelativeCells)
					{
						map.areaManager.BuildRoof[root + b2] = true;
					}
				}
				if (gravship.areas.noRoofArea != null)
				{
					foreach (IntVec3 b3 in gravship.areas.noRoofArea.RelativeCells)
					{
						map.areaManager.NoRoof[root + b3] = true;
					}
				}
				if (gravship.areas.snowClearArea != null)
				{
					foreach (IntVec3 b4 in gravship.areas.snowClearArea.RelativeCells)
					{
						map.areaManager.SnowOrSandClear[root + b4] = true;
					}
				}
				if (ModsConfig.BiotechActive && gravship.areas.pollutionClearArea != null)
				{
					foreach (IntVec3 b5 in gravship.areas.pollutionClearArea.RelativeCells)
					{
						map.areaManager.PollutionClear[root + b5] = true;
					}
				}
				foreach (MoveableStorageGroup moveableStorageGroup in gravship.areas.storageGroups)
				{
					moveableStorageGroup.TryCreateStorageGroup(map);
				}
			}
		}

		// Token: 0x06013525 RID: 79141 RVA: 0x005BEB64 File Offset: 0x005BCD64
		private static void UpdateBillDestinations(Map map)
		{
			using (IEnumerator<Bill> enumerator = BillUtility.GlobalBills().GetEnumerator())
			{
				IL_129:
				while (enumerator.MoveNext())
				{
					Bill bill = enumerator.Current;
					foreach (StorageGroup storageGroup in map.storageGroups.StorageGroupsForReading)
					{
						StorageGroup storageGroup2 = bill.GetSlotGroup() as StorageGroup;
						if (storageGroup2 != null && storageGroup2.RenamableLabel == storageGroup.RenamableLabel)
						{
							bill.SetStoreMode(BillStoreModeDefOf.SpecificStockpile, storageGroup);
							goto IL_129;
						}
					}
					foreach (Zone zone in map.zoneManager.AllZones)
					{
						Zone_Stockpile zone_Stockpile = zone as Zone_Stockpile;
						if (zone_Stockpile != null)
						{
							SlotGroup slotGroup = bill.GetSlotGroup() as SlotGroup;
							if (slotGroup != null)
							{
								Zone zone2 = slotGroup.parent as Zone;
								if (zone2 != null && zone2.RenamableLabel == zone.RenamableLabel)
								{
									bill.SetStoreMode(BillStoreModeDefOf.SpecificStockpile, zone_Stockpile.GetSlotGroup());
									goto IL_129;
								}
							}
						}
					}
					if (bill.GetStoreMode() == BillStoreModeDefOf.SpecificStockpile)
					{
						bill.SetStoreMode(BillStoreModeDefOf.DropOnFloor, null);
					}
				}
			}
		}

		// Token: 0x06013526 RID: 79142 RVA: 0x005BED00 File Offset: 0x005BCF00
		private static void CopyDesignations(Gravship gravship, Map map, IntVec3 root)
		{
			using (new ProfilerBlock("Designations"))
			{
				foreach (ValueTuple<IntVec3, Designation> valueTuple in gravship.TerrainDesignations)
				{
					IntVec3 item = valueTuple.Item1;
					Designation item2 = valueTuple.Item2;
					if (item2 != null)
					{
						item2.target = root + item;
						map.designationManager.AddDesignation(item2);
					}
				}
			}
		}

		// Token: 0x06013527 RID: 79143 RVA: 0x005BED9C File Offset: 0x005BCF9C
		public static void ApplyTemperatureVacuumFromBase(Gravship gravship, IntVec3 root, Map map)
		{
			if (!ModsConfig.OdysseyActive || gravship == null)
			{
				return;
			}
			foreach (RoomTemperatureVacuum.Data data in gravship.RoomTemperatures)
			{
				Room room = (root + data.local).GetRoom(map);
				if (room != null && !room.UsesOutdoorTemperature)
				{
					room.Temperature = data.temperature;
					room.Vacuum = (room.ExposedToSpace ? 1f : data.vacuum);
				}
			}
		}

		// Token: 0x06013528 RID: 79144 RVA: 0x005BEE34 File Offset: 0x005BD034
		public static HashSet<IntVec3> GetCellsAdjacentToSubstructure(IEnumerable<CellRect> occupiedRects, int expansion = 1)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return null;
			}
			HashSet<IntVec3> hashSet = new HashSet<IntVec3>();
			foreach (CellRect cellRect in occupiedRects)
			{
				CellRect cellRect2 = cellRect.ExpandedBy(expansion);
				for (int i = cellRect2.minX; i <= cellRect2.maxX; i++)
				{
					for (int j = cellRect2.minZ; j <= cellRect2.maxZ; j++)
					{
						hashSet.Add(new IntVec3(i, 0, j));
					}
				}
			}
			return hashSet;
		}

		// Token: 0x0400D666 RID: 54886
		public static bool placingGravship;

		// Token: 0x0200365A RID: 13914
		public enum ClearMode
		{
			// Token: 0x0400D668 RID: 54888
			All,
			// Token: 0x0400D669 RID: 54889
			AllButNonTreePlants,
			// Token: 0x0400D66A RID: 54890
			BlockingBuildingsOnly,
			// Token: 0x0400D66B RID: 54891
			NaturalRockOnly
		}
	}
}
