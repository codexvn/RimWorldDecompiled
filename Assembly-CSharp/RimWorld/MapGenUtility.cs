using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LudeonTK;
using RimWorld.Planet;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200209B RID: 8347
	[BurstCompile]
	public static class MapGenUtility
	{
		// Token: 0x0600B6B2 RID: 46770 RVA: 0x0034870C File Offset: 0x0034690C
		public static bool ShouldGenerateBeachSand(IntVec3 cell, Map map)
		{
			TerrainDef terrainDef = map.terrainGrid.TerrainAt(cell);
			return !terrainDef.IsWater && !terrainDef.IsIce;
		}

		// Token: 0x0600B6B3 RID: 46771 RVA: 0x00348739 File Offset: 0x00346939
		public static TerrainDef DeepFreshWaterTerrainAt(IntVec3 cell, Map map)
		{
			return map.BiomeAt(cell).waterDeepTerrain ?? TerrainDefOf.WaterDeep;
		}

		// Token: 0x0600B6B4 RID: 46772 RVA: 0x00348750 File Offset: 0x00346950
		public static TerrainDef ShallowFreshWaterTerrainAt(IntVec3 cell, Map map)
		{
			return map.BiomeAt(cell).waterShallowTerrain ?? TerrainDefOf.WaterShallow;
		}

		// Token: 0x0600B6B5 RID: 46773 RVA: 0x00348767 File Offset: 0x00346967
		public static TerrainDef DeepMovingWaterTerrainAt(IntVec3 cell, Map map)
		{
			return map.BiomeAt(cell).waterMovingChestDeepTerrain ?? TerrainDefOf.WaterMovingChestDeep;
		}

		// Token: 0x0600B6B6 RID: 46774 RVA: 0x0034877E File Offset: 0x0034697E
		public static TerrainDef ShallowMovingWaterTerrainAt(IntVec3 cell, Map map)
		{
			return map.BiomeAt(cell).waterMovingShallowTerrain ?? TerrainDefOf.WaterMovingShallow;
		}

		// Token: 0x0600B6B7 RID: 46775 RVA: 0x00348795 File Offset: 0x00346995
		public static TerrainDef DeepOceanWaterTerrainAt(IntVec3 cell, Map map)
		{
			return map.BiomeAt(cell).oceanDeepTerrain ?? TerrainDefOf.WaterOceanDeep;
		}

		// Token: 0x0600B6B8 RID: 46776 RVA: 0x003487AC File Offset: 0x003469AC
		public static TerrainDef ShallowOceanWaterTerrainAt(IntVec3 cell, Map map)
		{
			return map.BiomeAt(cell).oceanShallowTerrain ?? TerrainDefOf.WaterOceanShallow;
		}

		// Token: 0x0600B6B9 RID: 46777 RVA: 0x003487C4 File Offset: 0x003469C4
		public static TerrainDef BeachTerrainAt(IntVec3 cell, Map map)
		{
			foreach (TileMutatorDef tileMutatorDef in map.TileInfo.Mutators)
			{
				if (tileMutatorDef.overrideCoastalBeachTerrain != null)
				{
					return tileMutatorDef.overrideCoastalBeachTerrain;
				}
			}
			return map.BiomeAt(cell).coastalBeachTerrain ?? TerrainDefOf.Sand;
		}

		// Token: 0x0600B6BA RID: 46778 RVA: 0x00348838 File Offset: 0x00346A38
		public static TerrainDef LakeshoreTerrainAt(IntVec3 cell, Map map)
		{
			foreach (TileMutatorDef tileMutatorDef in map.TileInfo.Mutators)
			{
				if (tileMutatorDef.overrideLakeBeachTerrain != null)
				{
					return tileMutatorDef.overrideLakeBeachTerrain;
				}
			}
			return map.BiomeAt(cell).lakeBeachTerrain ?? TerrainDefOf.Sand;
		}

		// Token: 0x0600B6BB RID: 46779 RVA: 0x003488AC File Offset: 0x00346AAC
		public static TerrainDef MudTerrainAt(IntVec3 cell, Map map)
		{
			foreach (TileMutatorDef tileMutatorDef in map.TileInfo.Mutators)
			{
				if (tileMutatorDef.overrideMudTerrain != null)
				{
					return tileMutatorDef.overrideMudTerrain;
				}
			}
			return map.BiomeAt(cell).mudTerrain ?? TerrainDefOf.Mud;
		}

		// Token: 0x0600B6BC RID: 46780 RVA: 0x00348920 File Offset: 0x00346B20
		public static TerrainDef RiverbankTerrainAt(IntVec3 cell, Map map)
		{
			foreach (TileMutatorDef tileMutatorDef in map.TileInfo.Mutators)
			{
				if (tileMutatorDef.overrideRiverbankTerrain != null)
				{
					return tileMutatorDef.overrideRiverbankTerrain;
				}
			}
			return map.BiomeAt(cell).riverbankTerrain ?? TerrainDefOf.Riverbank;
		}

		// Token: 0x0600B6BD RID: 46781 RVA: 0x00348994 File Offset: 0x00346B94
		public static void ScatterCorpses(CellRect rect, Map map, Faction faction, IntRange count, FloatRange deadDaysRange)
		{
			MapGenUtility.<>c__DisplayClass11_0 CS$<>8__locals1 = new MapGenUtility.<>c__DisplayClass11_0();
			CS$<>8__locals1.map = map;
			int randomInRange = count.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				IntVec3 cell = CellFinder.RandomClosewalkCellNear(rect.RandomCell, CS$<>8__locals1.map, 10, new Predicate<IntVec3>(CS$<>8__locals1.<ScatterCorpses>g__Validator|0));
				if (cell.IsValid)
				{
					Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(faction.RandomPawnKind(), null, PawnGenerationContext.NonPlayer, null, false, false, true, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
					MapGenUtility.KillSpawnCorpse(CS$<>8__locals1.map, deadDaysRange, pawn, cell);
				}
			}
		}

		// Token: 0x0600B6BE RID: 46782 RVA: 0x00348AA0 File Offset: 0x00346CA0
		public static void ScatterCorpses(Map map, Faction faction, float points, FloatRange deadDaysRange)
		{
			Predicate<IntVec3> <>9__0;
			foreach (Pawn pawn in PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				faction = faction,
				groupKind = PawnGroupKindDefOf.Settlement,
				points = points,
				tile = map.Tile
			}, false))
			{
				Map map2 = map;
				int tightness = 10;
				int minCellCount = 5;
				Predicate<IntVec3> extraValidator;
				if ((extraValidator = <>9__0) == null)
				{
					extraValidator = (<>9__0 = ((IntVec3 x) => x.Walkable(map)));
				}
				IntVec3 cell = CellFinder.RandomClosewalkCellNear(CellFinderLoose.TryFindCentralCell(map2, tightness, minCellCount, extraValidator, false), map, 10, null);
				if (cell.IsValid)
				{
					MapGenUtility.KillSpawnCorpse(map, deadDaysRange, pawn, cell);
				}
				else
				{
					pawn.Discard(false);
				}
			}
		}

		// Token: 0x0600B6BF RID: 46783 RVA: 0x00348B84 File Offset: 0x00346D84
		private static void KillSpawnCorpse(Map map, FloatRange deadDaysRange, Pawn pawn, IntVec3 cell)
		{
			HealthUtility.SimulateKilled(pawn, DamageDefOf.Bullet, null, null, null);
			pawn.Corpse.Age = Mathf.RoundToInt(deadDaysRange.RandomInRange * 60000f - (float)Rand.Range(0, 60000));
			pawn.relations.hidePawnRelations = true;
			pawn.Corpse.GetComp<CompRottable>().RotProgress += (float)pawn.Corpse.Age;
			if (pawn.RaceProps.BloodDef != null)
			{
				GenSpawn.SpawnIrregularLump(pawn.RaceProps.BloodDef, cell, map, new IntRange(1, 5), new IntRange(2, 4), WipeMode.Vanish, null, null, null, null, null);
			}
			GenSpawn.Spawn(pawn.Corpse, cell, map, WipeMode.Vanish);
		}

		// Token: 0x0600B6C0 RID: 46784 RVA: 0x00348C3C File Offset: 0x00346E3C
		public static void DestroyTurrets(Map map)
		{
			foreach (Building building in map.listerBuildings.AllBuildingsNonColonistOfDef(ThingDefOf.Turret_MiniTurret).ToList<Building>())
			{
				building.Kill(null, null);
			}
		}

		// Token: 0x0600B6C1 RID: 46785 RVA: 0x00348CA8 File Offset: 0x00346EA8
		public static void DestroyProcessedFood(Map map)
		{
			foreach (Thing thing in (from t in map.listerThings.AllThings
			where t.def.IsIngestible && t.def.IsHumanFood()
			select t).ToList<Thing>())
			{
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600B6C2 RID: 46786 RVA: 0x00348D28 File Offset: 0x00346F28
		public static void ForbidAllItems(Map map)
		{
			foreach (Thing thing in map.listerThings.AllThings)
			{
				if (thing.def.category == ThingCategory.Item)
				{
					CompForbiddable compForbiddable = thing.TryGetComp<CompForbiddable>();
					if (compForbiddable != null && !compForbiddable.Forbidden)
					{
						thing.SetForbidden(true, false);
					}
				}
			}
		}

		// Token: 0x0600B6C3 RID: 46787 RVA: 0x00348DA4 File Offset: 0x00346FA4
		public static void DamageBuildings(List<Building> buildings, Map map, float maxPercentDestroyed = 0.65f, float hpRandomFactor = 1.2f, float destroyChanceExponent = 1.32f)
		{
			if (buildings.NullOrEmpty<Building>())
			{
				return;
			}
			CellRect rect = buildings[0].OccupiedRect();
			for (int i = 1; i < buildings.Count; i++)
			{
				rect = rect.Encapsulate(buildings[i].OccupiedRect());
			}
			Rot4 random = Rot4.Random;
			int num = 0;
			int count = buildings.Count;
			foreach (Building building in buildings)
			{
				if (!building.Destroyed)
				{
					bool flag;
					MapGenUtility.DamageBuilding(building, map, rect, random, out flag, hpRandomFactor, destroyChanceExponent);
					if (flag)
					{
						num++;
						MapGenUtility.ClearDisconnectedDoors(building, map);
						if ((float)num > (float)count * maxPercentDestroyed)
						{
							break;
						}
					}
				}
			}
		}

		// Token: 0x0600B6C4 RID: 46788 RVA: 0x00348E70 File Offset: 0x00347070
		private static void ClearDisconnectedDoors(Building building, Map map)
		{
			foreach (IntVec3 c in building.OccupiedRect().AdjacentCellsCardinal)
			{
				Building_Door door = c.GetDoor(map);
				if (door != null && !door.Destroyed)
				{
					bool flag = false;
					foreach (IntVec3 c2 in door.OccupiedRect().AdjacentCellsCardinal)
					{
						if (c2.GetEdifice(map) != null && c2.GetEdifice(map).def == ThingDefOf.Wall)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						door.Destroy(DestroyMode.Vanish);
					}
				}
			}
		}

		// Token: 0x0600B6C5 RID: 46789 RVA: 0x00348F44 File Offset: 0x00347144
		private static void DamageBuilding(Building building, Map map, CellRect rect, Rot4 dir, out bool destroyed, float hpRandomFactor = 1.2f, float destroyChanceExponent = 1.32f)
		{
			float num;
			if (dir.IsHorizontal)
			{
				num = (float)(building.Position.x - rect.minX) / (float)rect.Width;
			}
			else
			{
				num = (float)(building.Position.z - rect.minZ) / (float)rect.Height;
			}
			if (dir == Rot4.East || dir == Rot4.South)
			{
				num = 1f - num;
			}
			if (Rand.Chance(Mathf.Pow(num, destroyChanceExponent)))
			{
				IThingHolder thingHolder = building as IThingHolder;
				if (thingHolder != null)
				{
					foreach (Thing thing in thingHolder.GetDirectlyHeldThings().ToList<Thing>())
					{
						thing.Destroy(DestroyMode.Vanish);
					}
				}
				building.Destroy(DestroyMode.Vanish);
				destroyed = true;
				using (CellRect.Enumerator enumerator2 = building.OccupiedRect().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						IntVec3 c = enumerator2.Current;
						TerrainDef terrain = c.GetTerrain(map);
						if (terrain.burnedDef != null)
						{
							map.terrainGrid.SetTerrain(c, terrain.burnedDef);
						}
					}
					return;
				}
			}
			destroyed = false;
			building.HitPoints = Mathf.Clamp(Mathf.RoundToInt((float)building.MaxHitPoints * (1f - num) * Rand.Range(1f, hpRandomFactor)), 1, building.MaxHitPoints);
		}

		// Token: 0x0600B6C6 RID: 46790 RVA: 0x003490C8 File Offset: 0x003472C8
		public static bool TryGetRandomClearRect(int width, int height, out CellRect rect, int maxWidth = -1, int maxHeight = -1, Predicate<CellRect> rectValidator = null, float minElevation = -1f, float maxElevation = 0.7f, MapGenUtility.SearchWeightMode weightMode = MapGenUtility.SearchWeightMode.Largest, int mapBorderPadding = 1)
		{
			List<CellRect> list = MapGenUtility.GetClearRects(width, height, maxWidth, maxHeight, minElevation, maxElevation, mapBorderPadding);
			rect = CellRect.Empty;
			if (!list.Any<CellRect>())
			{
				return false;
			}
			list.Shuffle<CellRect>();
			if (weightMode == MapGenUtility.SearchWeightMode.Largest)
			{
				list = (from r in list
				orderby r.Area
				select r).ToList<CellRect>();
			}
			else if (weightMode == MapGenUtility.SearchWeightMode.Center)
			{
				list = (from r in list
				orderby r.CenterCell.DistanceToSquared(MapGenerator.mapBeingGenerated.Center)
				select r).ToList<CellRect>();
			}
			foreach (CellRect cellRect in list)
			{
				for (int i = 0; i < 10; i++)
				{
					CellRect cellRect2;
					if (!cellRect.TryFindRandomInnerRect(new IntVec2(width, height), out cellRect2, null))
					{
						return false;
					}
					if (rectValidator == null || rectValidator(cellRect2))
					{
						rect = cellRect2;
						return true;
					}
				}
			}
			foreach (CellRect cellRect3 in list)
			{
				for (int j = 0; j < cellRect3.Width - width; j++)
				{
					for (int k = 0; k < cellRect3.Height - height; k++)
					{
						CellRect cellRect4 = new CellRect(cellRect3.minX + j, cellRect3.minZ + k, width, height);
						if (rectValidator == null || rectValidator(cellRect4))
						{
							rect = cellRect4;
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600B6C7 RID: 46791 RVA: 0x00349288 File Offset: 0x00347488
		public static bool TryGetLargestClearRect(out CellRect rect, Func<CellRect, bool> rectValidator = null, float minElevation = -1f, float maxElevation = 0.7f, int mapBorderPadding = 8)
		{
			List<CellRect> clearRects = MapGenUtility.GetClearRects(1, 1, -1, -1, minElevation, maxElevation, mapBorderPadding);
			if (rectValidator != null)
			{
				for (int i = clearRects.Count - 1; i >= 0; i--)
				{
					if (!rectValidator(clearRects[i]))
					{
						clearRects.RemoveAt(i);
					}
				}
			}
			if (clearRects.Any<CellRect>())
			{
				rect = clearRects.MaxBy((CellRect r) => r.Area);
				return true;
			}
			rect = CellRect.Empty;
			return false;
		}

		// Token: 0x0600B6C8 RID: 46792 RVA: 0x00349310 File Offset: 0x00347510
		public static bool TryGetClosestClearRectTo(out CellRect rect, IntVec2 size, IntVec3 position, Func<CellRect, bool> rectValidator = null, float minElevation = -1f, float maxElevation = 0.7f, int mapBorderPadding = 8)
		{
			List<CellRect> clearRects = MapGenUtility.GetClearRects(size.x, size.z, -1, -1, minElevation, maxElevation, mapBorderPadding);
			if (rectValidator != null)
			{
				for (int i = clearRects.Count - 1; i >= 0; i--)
				{
					if (!rectValidator(clearRects[i]))
					{
						clearRects.RemoveAt(i);
					}
				}
			}
			if (clearRects.Empty<CellRect>())
			{
				rect = CellRect.Empty;
				return false;
			}
			CellRect cellRect;
			if (!clearRects.MinBy((CellRect r) => r.CenterCell.DistanceToSquared(position)).TryFindNearestInnerRectTo(size, position, out cellRect))
			{
				rect = CellRect.Empty;
				return false;
			}
			rect = cellRect;
			return true;
		}

		// Token: 0x0600B6C9 RID: 46793 RVA: 0x003493C8 File Offset: 0x003475C8
		public static List<CellRect> GetClearRects(int minWidth, int minHeight, int maxWidth = -1, int maxHeight = -1, float minElevation = -1f, float maxElevation = 0.7f, int mapBorderPadding = 8)
		{
			object obj;
			if (!MapGenerator.TryGetVar<object>("Elevation", out obj))
			{
				Log.Error("Elevation grid did not exist - a map is not being generated. This can only be used as long as map generator data exists.");
				return new List<CellRect>();
			}
			CellIndices cellIndices = MapGenerator.mapBeingGenerated.cellIndices;
			UnsafeList<CellRect> source = new UnsafeList<CellRect>(12, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			NativeArray<MapGenUtility.CellData> nativeArray = new NativeArray<MapGenUtility.CellData>(cellIndices.NumGridCells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			MapGenUtility.ComputeLargestRects(ref source, ref nativeArray, MapGenerator.Elevation.Grid_Unsafe, cellIndices, minWidth, minHeight, maxWidth, maxHeight, minElevation, maxElevation, mapBorderPadding);
			List<CellRect> list = new List<CellRect>();
			NativeArrayUtility.CopyUnsafeListToList<CellRect>(list, source);
			source.Dispose();
			nativeArray.Dispose();
			return list;
		}

		// Token: 0x0600B6CA RID: 46794 RVA: 0x00349458 File Offset: 0x00347658
		[BurstCompile]
		private static void ComputeLargestRects(ref UnsafeList<CellRect> rects, ref NativeArray<MapGenUtility.CellData> set, in NativeArray<float> elevation, in CellIndices indices, int minWidth, int minHeight, int maxWidth = -1, int maxHeight = -1, float minElevation = -1f, float maxElevation = 0.7f, int mapBorderPadding = 8)
		{
			MapGenUtility.ComputeLargestRects_0000B6CA$BurstDirectCall.Invoke(ref rects, ref set, elevation, indices, minWidth, minHeight, maxWidth, maxHeight, minElevation, maxElevation, mapBorderPadding);
		}

		// Token: 0x0600B6CB RID: 46795 RVA: 0x0034947C File Offset: 0x0034767C
		[BurstCompile]
		private static void RectsComputeSpaces(ref NativeArray<MapGenUtility.CellData> set, in NativeArray<float> elevation, in CellIndices indices, float minElevation, float maxElevation, int borderPadding)
		{
			MapGenUtility.RectsComputeSpaces_0000B6CB$BurstDirectCall.Invoke(ref set, elevation, indices, minElevation, maxElevation, borderPadding);
		}

		// Token: 0x0600B6CC RID: 46796 RVA: 0x0034948C File Offset: 0x0034768C
		public static void PostProcessSettlement(Map map, List<Building> placed, MapGenUtility.PostProcessSettlementParams parms)
		{
			foreach (Building building2 in placed)
			{
				if (parms.clearBuildingFaction)
				{
					if (building2 is Building_Turret)
					{
						continue;
					}
					if (building2.Faction == parms.faction)
					{
						building2.SetFaction(null, null);
					}
				}
				CompRefuelable compRefuelable;
				if (parms.noFuel && building2.TryGetComp(out compRefuelable))
				{
					compRefuelable.ConsumeFuel(compRefuelable.Fuel);
				}
				if (parms.ageCorpses)
				{
					Building_CorpseCasket building_CorpseCasket = building2 as Building_CorpseCasket;
					if (building_CorpseCasket != null && building_CorpseCasket.HasCorpse)
					{
						int num = Rand.Range(0, 900000);
						building_CorpseCasket.Corpse.Age += num;
						building_CorpseCasket.Corpse.GetComp<CompRottable>().RotProgress += (float)num;
					}
				}
			}
			if (parms.damageBuildings)
			{
				if (!parms.canDamageWalls)
				{
					placed.RemoveWhere((Building building) => building.def == ThingDefOf.Wall);
				}
				MapGenUtility.DamageBuildings(placed, map, 0.6f, 1.2f, 1.2f);
			}
		}

		// Token: 0x0600B6CD RID: 46797 RVA: 0x003495C0 File Offset: 0x003477C0
		public static bool IsMixedBiome(Map map)
		{
			return map.MixedBiomeComp.IsMixedBiome;
		}

		// Token: 0x0600B6CE RID: 46798 RVA: 0x003495CD File Offset: 0x003477CD
		public static BiomeDef BiomeAt(this Map map, IntVec3 c)
		{
			return map.MixedBiomeComp.GetBiomeAt(c);
		}

		// Token: 0x0600B6CF RID: 46799 RVA: 0x003495DC File Offset: 0x003477DC
		public static TerrainDef GetNaturalTerrainAt(IntVec3 cell, Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			MapGenFloatGrid fertility = MapGenerator.Fertility;
			MapGenFloatGrid caves = MapGenerator.Caves;
			Building edifice = cell.GetEdifice(map);
			TerrainDef result;
			if ((edifice != null && edifice.def.Fillage == FillCategory.Full) || caves[cell] > 0f)
			{
				result = MapGenUtility.TerrainFrom(cell, map, elevation[cell], fertility[cell], true);
			}
			else
			{
				result = MapGenUtility.TerrainFrom(cell, map, elevation[cell], fertility[cell], false);
			}
			return result;
		}

		// Token: 0x0600B6D0 RID: 46800 RVA: 0x00349658 File Offset: 0x00347858
		public static TerrainDef TerrainFrom(IntVec3 c, Map map, float elevation, float fertility, bool preferRock)
		{
			TerrainDef terrainDef = null;
			BiomeDef biomeDef = map.BiomeAt(c);
			bool flag = map.TileInfo.Mutators.Any((TileMutatorDef m) => m.preventsPondGeneration);
			if (!map.TileInfo.Mutators.Any((TileMutatorDef m) => m.preventPatches))
			{
				foreach (TerrainPatchMaker terrainPatchMaker in biomeDef.terrainPatchMakers)
				{
					if (!flag || !terrainPatchMaker.isPond)
					{
						terrainDef = terrainPatchMaker.TerrainAt(c, map, fertility);
						if (terrainDef != null)
						{
							break;
						}
					}
				}
			}
			if (terrainDef == null)
			{
				if (elevation > 0.55f && elevation < 0.61f && !biomeDef.noGravel)
				{
					terrainDef = (biomeDef.gravelTerrain ?? TerrainDefOf.Gravel);
				}
				else if (elevation >= 0.61f)
				{
					terrainDef = GenStep_RocksFromGrid.RockDefAt(c).building.naturalTerrain;
				}
			}
			if (terrainDef == null)
			{
				terrainDef = TerrainThreshold.TerrainAtValue(biomeDef.terrainsByFertility, fertility);
			}
			if (terrainDef == null)
			{
				if (!MapGenUtility.debug_WarnedMissingTerrain)
				{
					Log.Error(string.Concat(new string[]
					{
						"No terrain found in biome ",
						biomeDef.defName,
						" for elevation=",
						elevation.ToString(),
						", fertility=",
						fertility.ToString()
					}));
					MapGenUtility.debug_WarnedMissingTerrain = true;
				}
				terrainDef = TerrainDefOf.Sand;
			}
			if (preferRock && terrainDef.supportsRock)
			{
				terrainDef = GenStep_RocksFromGrid.RockDefAt(c).building.naturalTerrain;
			}
			return terrainDef;
		}

		// Token: 0x0600B6D1 RID: 46801 RVA: 0x003497FC File Offset: 0x003479FC
		public static List<CellRect> GetLargestClearRects(Map map, IntVec2 minSize, IntVec2 maxSize, bool checkUsedRects, float minElevation = -1f, float maxElevation = 0.7f, TerrainAffordanceDef affordance = null, bool checkWaterRoads = true)
		{
			List<CellRect> clearRects = MapGenUtility.GetClearRects(minSize.x, minSize.z, maxSize.x, maxSize.z, minElevation, maxElevation, 8);
			for (int i = 0; i < clearRects.Count; i++)
			{
				CellRect cellRect = clearRects[i];
				if (cellRect.Width > maxSize.x)
				{
					cellRect.Width = maxSize.x;
				}
				if (cellRect.Height > maxSize.z)
				{
					cellRect.Height = maxSize.z;
				}
				CellRect value;
				if (clearRects[i].TryFindRandomInnerRect(new IntVec2(cellRect.Width, cellRect.Height), out value, null))
				{
					clearRects[i] = value;
				}
			}
			MapGenUtility.TryFixInvalidRects(clearRects, map, minSize.x, checkUsedRects, affordance, checkWaterRoads);
			MapGenUtility.RemoveInvalidRects(clearRects, map, checkUsedRects, affordance, checkWaterRoads);
			return clearRects;
		}

		// Token: 0x0600B6D2 RID: 46802 RVA: 0x003498CC File Offset: 0x00347ACC
		public static void TryFixInvalidRects(List<CellRect> rects, Map map, int minRegionSize, bool checkUsedRects, TerrainAffordanceDef affordance, bool checkWaterRoads)
		{
			for (int i = rects.Count - 1; i >= 0; i--)
			{
				CellRect cellRect = rects[i];
				bool flag = true;
				int num = 100;
				while (flag && num-- > 0)
				{
					flag = false;
					if (cellRect.Width < minRegionSize || cellRect.Height < minRegionSize)
					{
						rects.RemoveAt(i);
						break;
					}
					foreach (IntVec3 intVec in cellRect.Cells)
					{
						if (!MapGenUtility.IsCellValid(intVec, map, checkUsedRects, affordance, checkWaterRoads))
						{
							CellRect value;
							if (!cellRect.TryContractToRemove(intVec, out value) || value.Width < minRegionSize || value.Height < minRegionSize)
							{
								rects.RemoveAt(i);
								break;
							}
							cellRect = (rects[i] = value);
							flag = true;
							break;
						}
					}
				}
				if (num <= 0)
				{
					Log.ErrorOnce("Loop guard exceeded trying to remove points from chunk rects", 956842091);
				}
			}
		}

		// Token: 0x0600B6D3 RID: 46803 RVA: 0x003499D4 File Offset: 0x00347BD4
		public static void RemoveInvalidRects(List<CellRect> rects, Map map, bool checkUsedRects, TerrainAffordanceDef affordance, bool checkWaterRoads)
		{
			for (int i = rects.Count - 1; i >= 0; i--)
			{
				using (IEnumerator<IntVec3> enumerator = rects[i].Cells.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!MapGenUtility.IsCellValid(enumerator.Current, map, checkUsedRects, affordance, checkWaterRoads))
						{
							rects.RemoveAt(i);
							break;
						}
					}
				}
			}
		}

		// Token: 0x0600B6D4 RID: 46804 RVA: 0x00349A4C File Offset: 0x00347C4C
		public static bool IsCellValid(IntVec3 cell, Map map, bool checkUsedRects, TerrainAffordanceDef affordance, bool checkWaterRoads)
		{
			TerrainDef terrainDef = map.terrainGrid.TerrainAt(cell);
			if (checkWaterRoads && (terrainDef.IsWater || terrainDef.IsRoad))
			{
				return false;
			}
			if (affordance != null && !cell.SupportsStructureType(map, affordance))
			{
				return false;
			}
			if (checkUsedRects && MapGenerator.UsedRects.Any((CellRect rect) => rect.Contains(cell)))
			{
				return false;
			}
			List<Thing> thingList = cell.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (!thingList[i].def.destroyable)
				{
					return false;
				}
				Building building = thingList[i] as Building;
				if (building != null && building.def.IsBuildingArtificial && !building.def.IsNonDeconstructibleAttackableBuilding && !building.IsClearableFreeBuilding)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600B6D5 RID: 46805 RVA: 0x00349B2C File Offset: 0x00347D2C
		public static bool TryGetStructureRect(Map map, IntRange structureSizeRange, out CellRect rect, Predicate<CellRect> extraValidator = null)
		{
			MapGenUtility.<>c__DisplayClass40_0 CS$<>8__locals1 = new MapGenUtility.<>c__DisplayClass40_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.extraValidator = extraValidator;
			CS$<>8__locals1.usedRects = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			CS$<>8__locals1.structureWidth = structureSizeRange.RandomInRange;
			CS$<>8__locals1.structureHeight = structureSizeRange.RandomInRange;
			CS$<>8__locals1.structureArea = CS$<>8__locals1.structureWidth * CS$<>8__locals1.structureHeight;
			int min = structureSizeRange.min;
			if (!MapGenUtility.TryGetClosestClearRectTo(out rect, new IntVec2(CS$<>8__locals1.structureWidth, CS$<>8__locals1.structureHeight), CS$<>8__locals1.map.Center, new Func<CellRect, bool>(CS$<>8__locals1.<TryGetStructureRect>g__Validator|0), -1f, 0.7f, 8) && !MapGenUtility.TryGetClosestClearRectTo(out rect, new IntVec2(min, min), CS$<>8__locals1.map.Center, new Func<CellRect, bool>(CS$<>8__locals1.<TryGetStructureRect>g__Validator|0), -1f, 0.7f, 8) && !MapGenUtility.TryGetRandomClearRect(min, min, out rect, -1, -1, new Predicate<CellRect>(CS$<>8__locals1.<TryGetStructureRect>g__Validator|0), -1f, 0.7f, MapGenUtility.SearchWeightMode.Largest, 1))
			{
				IntVec3 center;
				if (!CellFinder.TryFindRandomCell(CS$<>8__locals1.map, (IntVec3 c) => base.<TryGetStructureRect>g__Validator|0(c.RectAbout(CS$<>8__locals1.structureWidth, CS$<>8__locals1.structureHeight)), out center))
				{
					Log.Error("Failed to find location structure");
					return false;
				}
				rect = center.RectAbout(CS$<>8__locals1.structureWidth, CS$<>8__locals1.structureHeight);
			}
			return true;
		}

		// Token: 0x0600B6D6 RID: 46806 RVA: 0x00349C64 File Offset: 0x00347E64
		public static void GeneratePawns(Map map, CellRect rect, Faction faction, Lord lord, PawnGroupKindDef pawnGroupKindDef = null, PawnGroupMakerParms pawnGroupMakerParams = null, float? points = null, int? seed = null, Predicate<IntVec3> cellValidator = null, bool requiresRoof = false)
		{
			MapGenUtility.<>c__DisplayClass43_0 CS$<>8__locals1 = new MapGenUtility.<>c__DisplayClass43_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.requiresRoof = requiresRoof;
			CS$<>8__locals1.cellValidator = cellValidator;
			CS$<>8__locals1.rect = rect;
			if (pawnGroupMakerParams == null)
			{
				pawnGroupMakerParams = new PawnGroupMakerParms
				{
					tile = CS$<>8__locals1.map.Tile,
					faction = faction,
					points = (points ?? MapGenUtility.DefaultPawnsPoints.RandomInRange),
					inhabitants = true,
					seed = seed
				};
			}
			pawnGroupMakerParams.groupKind = (pawnGroupKindDef ?? PawnGroupKindDefOf.Settlement);
			foreach (Pawn pawn in PawnGroupMakerUtility.GeneratePawns(pawnGroupMakerParams, true))
			{
				IntVec3 loc;
				if (CS$<>8__locals1.<GeneratePawns>g__TryFindSpawnCell|0(pawn, out loc))
				{
					GenSpawn.Spawn(pawn, loc, CS$<>8__locals1.map, WipeMode.Vanish);
					if (lord != null)
					{
						lord.AddPawn(pawn);
					}
				}
				else
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
				}
			}
		}

		// Token: 0x0600B6D7 RID: 46807 RVA: 0x00349D74 File Offset: 0x00347F74
		public static void GenerateLoot(Map map, CellRect rect, ThingSetMakerDef setMakerDef, FloatRange? marketValueRange = null, IntRange? countRange = null, Faction faction = null, bool requiresRoof = false)
		{
			ThingSetMakerParams parms = new ThingSetMakerParams
			{
				countRange = new IntRange?(countRange ?? MapGenUtility.DefaultLootCountRange),
				techLevel = new TechLevel?((faction != null) ? faction.def.techLevel : TechLevel.Undefined),
				makingFaction = faction,
				totalMarketValueRange = new FloatRange?(marketValueRange ?? MapGenUtility.DefaultLootMarketValue)
			};
			MapGenUtility.GenerateLoot(map, rect, setMakerDef.root.Generate(parms), false);
		}

		// Token: 0x0600B6D8 RID: 46808 RVA: 0x00349E14 File Offset: 0x00348014
		public static void GenerateLoot(Map map, CellRect rect, IList<Thing> things, bool requiresRoof = false)
		{
			MapGenUtility.<>c__DisplayClass47_0 CS$<>8__locals1 = new MapGenUtility.<>c__DisplayClass47_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.requiresRoof = requiresRoof;
			List<IntVec3> list = rect.Cells.Where(new Func<IntVec3, bool>(CS$<>8__locals1.<GenerateLoot>g__CanPlace|0)).ToList<IntVec3>();
			while (list.Count > 0 && things.Count > 0)
			{
				int index = Rand.Range(0, list.Count);
				IntVec3 cell = list[index];
				list.RemoveAt(index);
				index = Rand.Range(0, things.Count);
				Thing thing = things[index];
				things.RemoveAt(index);
				MapGenUtility.SpawnLoot(thing, cell, CS$<>8__locals1.map);
			}
			if (things.Count > 0)
			{
				Log.Warning("Could not scatter loot things in rooms: " + string.Join(", ", from t in things
				select t.Label));
				foreach (Thing thing2 in things)
				{
					for (int i = 1000; i > 0; i--)
					{
						IntVec3 cell2 = CellFinder.RandomCell(CS$<>8__locals1.map);
						if (CS$<>8__locals1.<GenerateLoot>g__CanPlace|0(cell2))
						{
							MapGenUtility.SpawnLoot(thing2, cell2, CS$<>8__locals1.map);
						}
					}
				}
			}
		}

		// Token: 0x0600B6D9 RID: 46809 RVA: 0x00349F68 File Offset: 0x00348168
		private static void SpawnLoot(Thing thing, IntVec3 cell, Map map)
		{
			GenSpawn.Spawn(thing, cell, map, WipeMode.Vanish);
			thing.TrySetForbidden(true);
		}

		// Token: 0x0600B6DA RID: 46810 RVA: 0x00349F7C File Offset: 0x0034817C
		public static bool IsGoodSpawnCell(ThingDef def, IntVec3 pos, Map map)
		{
			CellRect cellRect = pos.RectAbout(def.Size);
			if (!GenSpawn.CanSpawnAt(def, pos, map, null, true))
			{
				return false;
			}
			foreach (IntVec3 c in cellRect.Cells)
			{
				if (c.GetEdifice(map) != null)
				{
					return false;
				}
				foreach (Thing thing in c.GetThingList(map))
				{
					if (thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Pawn)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600B6DB RID: 46811 RVA: 0x0034A05C File Offset: 0x0034825C
		public static void SetMapRoomTemperature(Map map, LayoutDef layoutDef, float temperature)
		{
			if (map.Biome.constantOutdoorTemperature != null && Mathf.Approximately(map.Biome.constantOutdoorTemperature.Value, temperature))
			{
				return;
			}
			MapGenUtility.tmpSetRoomIds.Clear();
			foreach (LayoutStructureSketch layoutStructureSketch in map.layoutStructureSketches)
			{
				if (layoutStructureSketch.layoutDef == layoutDef)
				{
					foreach (LayoutRoom layoutRoom in layoutStructureSketch.structureLayout.Rooms)
					{
						foreach (CellRect cellRect in layoutRoom.rects)
						{
							foreach (IntVec3 intVec in cellRect.Cells)
							{
								if (intVec.GetRoof(map) != null)
								{
									Room room = intVec.GetRoom(map);
									if (room != null && MapGenUtility.tmpSetRoomIds.Add(room.ID))
									{
										room.Temperature = temperature;
									}
									if (map.Biome.inVacuum && room != null && !room.ExposedToSpace)
									{
										room.Vacuum = 0f;
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600B6DC RID: 46812 RVA: 0x0034A23C File Offset: 0x0034843C
		public static void SpawnScatter(Map map, ThingDef thingDef, FloatRange countPer10K)
		{
			float num = (float)map.Size.x * (float)map.Size.z / 10000f;
			int num2 = Mathf.RoundToInt(countPer10K.RandomInRange * num);
			List<CellRect> list = new List<CellRect>();
			foreach (LayoutStructureSketch layoutStructureSketch in map.layoutStructureSketches)
			{
				foreach (LayoutRoom layoutRoom in layoutStructureSketch.structureLayout.Rooms)
				{
					list.AddRange(layoutRoom.rects);
				}
			}
			if (list.Empty<CellRect>())
			{
				return;
			}
			for (int i = 0; i < num2; i++)
			{
				CellRect cellRect = list.RandomElementByWeight((CellRect x) => (float)x.Area);
				for (int j = 0; j < 10; j++)
				{
					IntVec3 randomCell = cellRect.RandomCell;
					Thing thing;
					if (thingDef.IsFilth)
					{
						if (FilthMaker.TryMakeFilth(randomCell, map, thingDef, 1, FilthSourceFlags.None, true))
						{
							break;
						}
					}
					else if (GenSpawn.TrySpawn(thingDef, randomCell, map, Rot4.Random, out thing, WipeMode.Vanish, true))
					{
						break;
					}
				}
			}
		}

		// Token: 0x0600B6DD RID: 46813 RVA: 0x0034A390 File Offset: 0x00348590
		public static List<CellRect> SubdivideRectIntoChunks(CellRect bounds, int minRegionSize, int separation)
		{
			int subdivisionCount = MapGenUtility.GetSubdivisionCount(bounds, ref minRegionSize);
			return bounds.Subdivide(subdivisionCount, separation);
		}

		// Token: 0x0600B6DE RID: 46814 RVA: 0x0034A3B0 File Offset: 0x003485B0
		public static int GetSubdivisionCount(CellRect bounds, ref int minRegionSize)
		{
			int num = 1;
			int i;
			for (i = bounds.Size.x / 2; i > minRegionSize; i /= 2)
			{
				num++;
			}
			if (i < minRegionSize)
			{
				num--;
				i *= 2;
			}
			minRegionSize = i;
			return num;
		}

		// Token: 0x0600B6DF RID: 46815 RVA: 0x0034A3F0 File Offset: 0x003485F0
		public static void SpawnExteriorLumps(Map map, ThingDef thingDef, FloatRange countPer10K, IntRange countRange, IntRange distRange, int maxDistFromBuilding = 6)
		{
			MapGenUtility.<>c__DisplayClass55_0 CS$<>8__locals1 = new MapGenUtility.<>c__DisplayClass55_0();
			CS$<>8__locals1.maxDistFromBuilding = maxDistFromBuilding;
			CS$<>8__locals1.map = map;
			float num = (float)CS$<>8__locals1.map.Size.x * (float)CS$<>8__locals1.map.Size.z / 10000f;
			int num2 = Mathf.RoundToInt(countPer10K.RandomInRange * num);
			HashSet<IntVec3> hashSet = new HashSet<IntVec3>(from c in CS$<>8__locals1.map.layoutStructureSketches.SelectMany(delegate(LayoutStructureSketch sketch)
			{
				IEnumerable<LayoutRoom> rooms = sketch.structureLayout.Rooms;
				Func<LayoutRoom, IEnumerable<IntVec3>> selector;
				if ((selector = CS$<>8__locals1.<>9__2) == null)
				{
					selector = (CS$<>8__locals1.<>9__2 = delegate(LayoutRoom room)
					{
						IEnumerable<CellRect> rects = room.rects;
						Func<CellRect, IEnumerable<IntVec3>> selector2;
						if ((selector2 = CS$<>8__locals1.<>9__3) == null)
						{
							selector2 = (CS$<>8__locals1.<>9__3 = ((CellRect rect) => rect.DifferenceCells(rect.ExpandedBy(CS$<>8__locals1.maxDistFromBuilding))));
						}
						return rects.SelectMany(selector2);
					});
				}
				return rooms.SelectMany(selector);
			})
			where c.InBounds(CS$<>8__locals1.map) && c.GetEdifice(CS$<>8__locals1.map) == null
			select c);
			foreach (LayoutStructureSketch layoutStructureSketch in CS$<>8__locals1.map.layoutStructureSketches)
			{
				using (List<LayoutRoom>.Enumerator enumerator2 = layoutStructureSketch.structureLayout.Rooms.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						LayoutRoom room = enumerator2.Current;
						hashSet.RemoveWhere((IntVec3 c) => room.Contains(c, 1));
					}
				}
			}
			if (hashSet.NullOrEmpty<IntVec3>())
			{
				return;
			}
			for (int i = 0; i < num2; i++)
			{
				IntVec3 pos = hashSet.RandomElement<IntVec3>();
				GenSpawn.SpawnIrregularLump(thingDef, pos, CS$<>8__locals1.map, countRange, distRange, WipeMode.Vanish, new Predicate<IntVec3>(CS$<>8__locals1.<SpawnExteriorLumps>g__Validator|5), null, null, null, null);
			}
		}

		// Token: 0x0600B6E0 RID: 46816 RVA: 0x0034A568 File Offset: 0x00348768
		public static int DoRectEdgeLumps(Map map, TerrainDef terrain, ref CellRect rect, IntRange lengthRange, IntRange offsetRange, TerrainDef scatter = null)
		{
			int num = 0;
			int randomInRange = lengthRange.RandomInRange;
			int randomInRange2 = offsetRange.RandomInRange;
			for (int i = rect.minX; i < rect.maxX; i++)
			{
				num += MapGenUtility.DoHorizontalLumps(i, rect.maxZ, terrain, map, ref randomInRange2, ref randomInRange, true, lengthRange, offsetRange, scatter);
			}
			for (int j = rect.minX; j < rect.maxX; j++)
			{
				num += MapGenUtility.DoHorizontalLumps(j, rect.minZ, terrain, map, ref randomInRange2, ref randomInRange, false, lengthRange, offsetRange, scatter);
			}
			for (int k = rect.minZ; k < rect.maxZ; k++)
			{
				num += MapGenUtility.DoVerticalLumps(k, rect.maxX, terrain, map, ref randomInRange2, ref randomInRange, true, lengthRange, offsetRange, scatter);
			}
			for (int l = rect.minZ; l < rect.maxZ; l++)
			{
				num += MapGenUtility.DoVerticalLumps(l, rect.minX, terrain, map, ref randomInRange2, ref randomInRange, false, lengthRange, offsetRange, scatter);
			}
			rect = rect.ExpandedBy(offsetRange.max);
			return num;
		}

		// Token: 0x0600B6E1 RID: 46817 RVA: 0x0034A670 File Offset: 0x00348870
		public static int DoHorizontalLumps(int x, int worldZ, TerrainDef terrain, Map map, ref int lumpOffset, ref int lumpLength, bool positive, IntRange lengthRange, IntRange offsetRange, TerrainDef scatter = null)
		{
			int num = 0;
			for (int i = 1; i <= lumpOffset; i++)
			{
				IntVec3 c = new IntVec3(x, 0, worldZ + i * (positive ? 1 : -1));
				if (c.InBounds(map))
				{
					TerrainDef terrainDef = terrain;
					if (scatter != null && Rand.Bool)
					{
						terrainDef = scatter;
					}
					if (terrainDef == null)
					{
						List<Thing> thingList = c.GetThingList(map);
						for (int j = thingList.Count - 1; j >= 0; j--)
						{
							thingList[j].Destroy(DestroyMode.Vanish);
						}
						map.roofGrid.SetRoof(c, null);
					}
					else
					{
						map.terrainGrid.SetTerrain(c, terrainDef);
					}
					num++;
				}
			}
			int num2 = lumpLength;
			lumpLength = num2 - 1;
			if (num2 <= 0)
			{
				MapGenUtility.ResetLumpVariables(ref lumpOffset, ref lumpLength, lengthRange, offsetRange);
			}
			return num;
		}

		// Token: 0x0600B6E2 RID: 46818 RVA: 0x0034A734 File Offset: 0x00348934
		public static int DoVerticalLumps(int z, int worldX, TerrainDef terrain, Map map, ref int lumpOffset, ref int lumpLength, bool positive, IntRange lengthRange, IntRange offsetRange, TerrainDef scatter = null)
		{
			int num = 0;
			for (int i = 1; i <= lumpOffset; i++)
			{
				IntVec3 c = new IntVec3(worldX + i * (positive ? 1 : -1), 0, z);
				if (c.InBounds(map))
				{
					TerrainDef terrainDef = terrain;
					if (scatter != null && Rand.Bool)
					{
						terrainDef = scatter;
					}
					if (terrainDef == null)
					{
						List<Thing> thingList = c.GetThingList(map);
						for (int j = thingList.Count - 1; j >= 0; j--)
						{
							thingList[j].Destroy(DestroyMode.Vanish);
						}
						map.roofGrid.SetRoof(c, null);
					}
					else
					{
						map.terrainGrid.SetTerrain(c, terrainDef);
					}
					num++;
				}
			}
			int num2 = lumpLength;
			lumpLength = num2 - 1;
			if (num2 <= 0)
			{
				MapGenUtility.ResetLumpVariables(ref lumpOffset, ref lumpLength, lengthRange, offsetRange);
			}
			return num;
		}

		// Token: 0x0600B6E3 RID: 46819 RVA: 0x0034A7F8 File Offset: 0x003489F8
		private static void ResetLumpVariables(ref int lumpOffset, ref int lumpLength, IntRange lengthRange, IntRange offsetRange)
		{
			int num = lumpLength;
			int num2 = lumpOffset;
			lumpLength = lengthRange.RandomInRange;
			lumpOffset = offsetRange.RandomInRange;
			if (num == lumpLength)
			{
				lumpLength = lengthRange.RandomInRange;
			}
			if (num2 == lumpOffset)
			{
				lumpOffset = offsetRange.RandomInRange;
			}
		}

		// Token: 0x0600B6E4 RID: 46820 RVA: 0x0034A838 File Offset: 0x00348A38
		public static void SpawnScatteredGroupPrefabs(Map map, CellRect structure, List<ScatteredPrefabs> groups)
		{
			foreach (ScatteredPrefabs scatteredPrefabs in groups)
			{
				GenStep_ScatterGroupPrefabs genStep_ScatterGroupPrefabs = new GenStep_ScatterGroupPrefabs();
				GenStepParams parms = default(GenStepParams);
				genStep_ScatterGroupPrefabs.minSpacing = (float)scatteredPrefabs.minSpacing;
				genStep_ScatterGroupPrefabs.countPer10kCellsRange = scatteredPrefabs.countPer10kCellsRange;
				genStep_ScatterGroupPrefabs.groups = scatteredPrefabs.prefabs;
				genStep_ScatterGroupPrefabs.validators.Add(new StructureScatterValidator
				{
					structureRect = structure,
					maxDistFromStructure = scatteredPrefabs.maxDistFromStructure
				});
				genStep_ScatterGroupPrefabs.Generate(map, parms);
			}
		}

		// Token: 0x0600B6E5 RID: 46821 RVA: 0x0034A8DC File Offset: 0x00348ADC
		public static void Line(TerrainDef terrain, Map map, IntVec3 start, IntVec3 end, float thickness, bool canExit = false)
		{
			int num = Mathf.CeilToInt(thickness / 2f);
			int num2 = start.x;
			int num3 = start.z;
			int x = end.x;
			int z = end.z;
			int num4 = Mathf.Abs(x - num2);
			int num5 = Mathf.Abs(z - num3);
			int num6 = (num2 < x) ? 1 : -1;
			int num7 = (num3 < z) ? 1 : -1;
			int num8 = num4 - num5;
			for (;;)
			{
				bool flag = false;
				for (int i = -num; i <= num; i++)
				{
					for (int j = -num; j <= num; j++)
					{
						int num9 = num2 + i;
						int num10 = num3 + j;
						if (Vector2.SqrMagnitude(new Vector2((float)num2, (float)num3) - new Vector2((float)num9, (float)num10)) <= (float)(num * num))
						{
							IntVec3 c = new IntVec3(num9, 0, num10);
							if (c.InBounds(map) && (!terrain.temporary || map.terrainGrid.TempTerrainAt(c) != terrain) && (!terrain.isFoundation || map.terrainGrid.FoundationAt(c) != terrain) && (terrain.isFoundation || map.terrainGrid.TerrainAt(c) != terrain))
							{
								map.terrainGrid.SetTerrain(c, terrain);
								flag = true;
							}
						}
					}
				}
				if ((canExit && !flag) || (num2 == x && num3 == z))
				{
					break;
				}
				int num11 = 2 * num8;
				if (num11 > -num5)
				{
					num8 -= num5;
					num2 += num6;
				}
				if (num11 < num4)
				{
					num8 += num4;
					num3 += num7;
				}
			}
		}

		// Token: 0x0600B6E7 RID: 46823 RVA: 0x0034AAA4 File Offset: 0x00348CA4
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ComputeLargestRects$BurstManaged(ref UnsafeList<CellRect> rects, ref NativeArray<MapGenUtility.CellData> set, in NativeArray<float> elevation, in CellIndices indices, int minWidth, int minHeight, int maxWidth = -1, int maxHeight = -1, float minElevation = -1f, float maxElevation = 0.7f, int mapBorderPadding = 8)
		{
			MapGenUtility.RectsComputeSpaces(ref set, elevation, indices, minElevation, maxElevation, mapBorderPadding);
			for (int i = indices.NumGridCells - 1; i >= 0; i--)
			{
				if (set[i].z >= minHeight)
				{
					IntVec3 intVec = indices.IndexToCell(i);
					bool flag = false;
					int num = set[i].z;
					int num2 = set[i].x;
					if (num2 >= minWidth && num >= minHeight)
					{
						for (int j = 0; j < rects.Length; j++)
						{
							if (rects[j].Contains(intVec))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							for (int k = intVec.z - 1; k >= intVec.z - num; k--)
							{
								IntVec3 c = new IntVec3(intVec.x, 0, k);
								int index = indices.CellToIndex(c);
								MapGenUtility.CellData cellData = set[index];
								if (cellData.x < minWidth)
								{
									num = intVec.z - k;
									break;
								}
								num2 = math.min(num2, cellData.x);
							}
							if (num2 >= minWidth && num >= minHeight)
							{
								if (maxWidth > 0)
								{
									num2 = math.min(num2, maxWidth);
								}
								if (maxHeight > 0)
								{
									num = math.min(num, maxHeight);
								}
								CellRect other = new CellRect(intVec.x - num2 + 1, intVec.z - num + 1, num2, num);
								bool flag2 = false;
								for (int l = 0; l < rects.Length; l++)
								{
									if (rects[l].Overlaps(other))
									{
										if (other.Area <= rects[l].Area)
										{
											flag = true;
											break;
										}
										flag2 = true;
									}
								}
								if (!flag)
								{
									if (flag2)
									{
										for (int m = rects.Length - 1; m >= 0; m--)
										{
											if (rects[m].Overlaps(other))
											{
												rects.RemoveAt(m);
											}
										}
									}
									rects.Add(other);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600B6E8 RID: 46824 RVA: 0x0034ACA0 File Offset: 0x00348EA0
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void RectsComputeSpaces$BurstManaged(ref NativeArray<MapGenUtility.CellData> set, in NativeArray<float> elevation, in CellIndices indices, float minElevation, float maxElevation, int borderPadding)
		{
			for (int i = 0; i < indices.SizeX; i++)
			{
				if (set[i].z == 0)
				{
					IntVec3 intVec = indices.IndexToCell(i);
					int num = 1;
					for (int j = 0; j < indices.SizeZ; j++)
					{
						IntVec3 intVec2 = new IntVec3(intVec.x, 0, j);
						int index = indices.CellToIndex(intVec2);
						NativeArray<float> nativeArray = elevation;
						float num2 = nativeArray[index];
						if (num2 <= minElevation || num2 >= maxElevation || intVec2.x < borderPadding || intVec2.z < borderPadding || intVec2.x > indices.SizeX - borderPadding || intVec2.z > indices.SizeZ - borderPadding)
						{
							set[index] = new MapGenUtility.CellData
							{
								x = -1,
								z = -1
							};
							num = 1;
						}
						else
						{
							IntVec3 c = new IntVec3(intVec.x - 1, 0, j);
							int index2 = indices.CellToIndex(c);
							int x = 1;
							if (indices.Contains(c) && set[index2].x > 0)
							{
								x = set[index2].x + 1;
							}
							set[index] = new MapGenUtility.CellData
							{
								x = x,
								z = num++
							};
						}
					}
				}
			}
		}

		// Token: 0x04007D61 RID: 32097
		private static bool debug_WarnedMissingTerrain;

		// Token: 0x04007D62 RID: 32098
		private const float MaxWaterOverlap = 0.25f;

		// Token: 0x04007D63 RID: 32099
		private static readonly FloatRange DefaultPawnsPoints = new FloatRange(1150f, 1600f);

		// Token: 0x04007D64 RID: 32100
		private const int RequiredCellCount = 100;

		// Token: 0x04007D65 RID: 32101
		private static readonly IntRange DefaultLootCountRange = new IntRange(3, 10);

		// Token: 0x04007D66 RID: 32102
		private static readonly FloatRange DefaultLootMarketValue = new FloatRange(1800f, 2000f);

		// Token: 0x04007D67 RID: 32103
		private static readonly HashSet<int> tmpSetRoomIds = new HashSet<int>(8);

		// Token: 0x0200209C RID: 8348
		public enum SearchWeightMode
		{
			// Token: 0x04007D69 RID: 32105
			None,
			// Token: 0x04007D6A RID: 32106
			Largest,
			// Token: 0x04007D6B RID: 32107
			Center
		}

		// Token: 0x0200209D RID: 8349
		public struct CellData
		{
			// Token: 0x04007D6C RID: 32108
			public int x;

			// Token: 0x04007D6D RID: 32109
			public int z;
		}

		// Token: 0x0200209E RID: 8350
		public class PostProcessSettlementParams
		{
			// Token: 0x04007D6E RID: 32110
			public bool clearBuildingFaction;

			// Token: 0x04007D6F RID: 32111
			public Faction faction;

			// Token: 0x04007D70 RID: 32112
			public bool noFuel;

			// Token: 0x04007D71 RID: 32113
			public bool ageCorpses;

			// Token: 0x04007D72 RID: 32114
			public bool damageBuildings;

			// Token: 0x04007D73 RID: 32115
			public bool canDamageWalls;
		}

		// Token: 0x020020AB RID: 8363
		// (Invoke) Token: 0x0600B712 RID: 46866
		public delegate void ComputeLargestRects_0000B6CA$PostfixBurstDelegate(ref UnsafeList<CellRect> rects, ref NativeArray<MapGenUtility.CellData> set, in NativeArray<float> elevation, in CellIndices indices, int minWidth, int minHeight, int maxWidth = -1, int maxHeight = -1, float minElevation = -1f, float maxElevation = 0.7f, int mapBorderPadding = 8);

		// Token: 0x020020AC RID: 8364
		internal static class ComputeLargestRects_0000B6CA$BurstDirectCall
		{
			// Token: 0x0600B715 RID: 46869 RVA: 0x0034B1FF File Offset: 0x003493FF
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (MapGenUtility.ComputeLargestRects_0000B6CA$BurstDirectCall.Pointer == 0)
				{
					MapGenUtility.ComputeLargestRects_0000B6CA$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(MapGenUtility.ComputeLargestRects_0000B6CA$BurstDirectCall.DeferredCompilation, methodof(MapGenUtility.ComputeLargestRects$BurstManaged(UnsafeList<CellRect>*, NativeArray<MapGenUtility.CellData>*, NativeArray<float>*, CellIndices*, int, int, int, int, float, float, int)).MethodHandle, typeof(MapGenUtility.ComputeLargestRects_0000B6CA$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = MapGenUtility.ComputeLargestRects_0000B6CA$BurstDirectCall.Pointer;
			}

			// Token: 0x0600B716 RID: 46870 RVA: 0x0034B22C File Offset: 0x0034942C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				MapGenUtility.ComputeLargestRects_0000B6CA$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x0600B717 RID: 46871 RVA: 0x0034B244 File Offset: 0x00349444
			public unsafe static void Constructor()
			{
				MapGenUtility.ComputeLargestRects_0000B6CA$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(MapGenUtility.ComputeLargestRects(UnsafeList<CellRect>*, NativeArray<MapGenUtility.CellData>*, NativeArray<float>*, CellIndices*, int, int, int, int, float, float, int)).MethodHandle);
			}

			// Token: 0x0600B718 RID: 46872 RVA: 0x000026EA File Offset: 0x000008EA
			public static void Initialize()
			{
			}

			// Token: 0x0600B719 RID: 46873 RVA: 0x0034B255 File Offset: 0x00349455
			// Note: this type is marked as 'beforefieldinit'.
			static ComputeLargestRects_0000B6CA$BurstDirectCall()
			{
				MapGenUtility.ComputeLargestRects_0000B6CA$BurstDirectCall.Constructor();
			}

			// Token: 0x0600B71A RID: 46874 RVA: 0x0034B25C File Offset: 0x0034945C
			public static void Invoke(ref UnsafeList<CellRect> rects, ref NativeArray<MapGenUtility.CellData> set, in NativeArray<float> elevation, in CellIndices indices, int minWidth, int minHeight, int maxWidth = -1, int maxHeight = -1, float minElevation = -1f, float maxElevation = 0.7f, int mapBorderPadding = 8)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = MapGenUtility.ComputeLargestRects_0000B6CA$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Collections.LowLevel.Unsafe.UnsafeList`1<Verse.CellRect>&,Unity.Collections.NativeArray`1<RimWorld.MapGenUtility/CellData>&,Unity.Collections.NativeArray`1<System.Single>&,Verse.CellIndices&,System.Int32,System.Int32,System.Int32,System.Int32,System.Single,System.Single,System.Int32), ref rects, ref set, ref elevation, ref indices, minWidth, minHeight, maxWidth, maxHeight, minElevation, maxElevation, mapBorderPadding, functionPointer);
						return;
					}
				}
				MapGenUtility.ComputeLargestRects$BurstManaged(ref rects, ref set, elevation, indices, minWidth, minHeight, maxWidth, maxHeight, minElevation, maxElevation, mapBorderPadding);
			}

			// Token: 0x04007D97 RID: 32151
			private static IntPtr Pointer;

			// Token: 0x04007D98 RID: 32152
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020020AD RID: 8365
		// (Invoke) Token: 0x0600B71C RID: 46876
		public delegate void RectsComputeSpaces_0000B6CB$PostfixBurstDelegate(ref NativeArray<MapGenUtility.CellData> set, in NativeArray<float> elevation, in CellIndices indices, float minElevation, float maxElevation, int borderPadding);

		// Token: 0x020020AE RID: 8366
		internal static class RectsComputeSpaces_0000B6CB$BurstDirectCall
		{
			// Token: 0x0600B71F RID: 46879 RVA: 0x0034B2AF File Offset: 0x003494AF
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (MapGenUtility.RectsComputeSpaces_0000B6CB$BurstDirectCall.Pointer == 0)
				{
					MapGenUtility.RectsComputeSpaces_0000B6CB$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(MapGenUtility.RectsComputeSpaces_0000B6CB$BurstDirectCall.DeferredCompilation, methodof(MapGenUtility.RectsComputeSpaces$BurstManaged(NativeArray<MapGenUtility.CellData>*, NativeArray<float>*, CellIndices*, float, float, int)).MethodHandle, typeof(MapGenUtility.RectsComputeSpaces_0000B6CB$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = MapGenUtility.RectsComputeSpaces_0000B6CB$BurstDirectCall.Pointer;
			}

			// Token: 0x0600B720 RID: 46880 RVA: 0x0034B2DC File Offset: 0x003494DC
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				MapGenUtility.RectsComputeSpaces_0000B6CB$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x0600B721 RID: 46881 RVA: 0x0034B2F4 File Offset: 0x003494F4
			public unsafe static void Constructor()
			{
				MapGenUtility.RectsComputeSpaces_0000B6CB$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(MapGenUtility.RectsComputeSpaces(NativeArray<MapGenUtility.CellData>*, NativeArray<float>*, CellIndices*, float, float, int)).MethodHandle);
			}

			// Token: 0x0600B722 RID: 46882 RVA: 0x000026EA File Offset: 0x000008EA
			public static void Initialize()
			{
			}

			// Token: 0x0600B723 RID: 46883 RVA: 0x0034B305 File Offset: 0x00349505
			// Note: this type is marked as 'beforefieldinit'.
			static RectsComputeSpaces_0000B6CB$BurstDirectCall()
			{
				MapGenUtility.RectsComputeSpaces_0000B6CB$BurstDirectCall.Constructor();
			}

			// Token: 0x0600B724 RID: 46884 RVA: 0x0034B30C File Offset: 0x0034950C
			public static void Invoke(ref NativeArray<MapGenUtility.CellData> set, in NativeArray<float> elevation, in CellIndices indices, float minElevation, float maxElevation, int borderPadding)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = MapGenUtility.RectsComputeSpaces_0000B6CB$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Collections.NativeArray`1<RimWorld.MapGenUtility/CellData>&,Unity.Collections.NativeArray`1<System.Single>&,Verse.CellIndices&,System.Single,System.Single,System.Int32), ref set, ref elevation, ref indices, minElevation, maxElevation, borderPadding, functionPointer);
						return;
					}
				}
				MapGenUtility.RectsComputeSpaces$BurstManaged(ref set, elevation, indices, minElevation, maxElevation, borderPadding);
			}

			// Token: 0x04007D99 RID: 32153
			private static IntPtr Pointer;

			// Token: 0x04007D9A RID: 32154
			private static IntPtr DeferredCompilation;
		}
	}
}
