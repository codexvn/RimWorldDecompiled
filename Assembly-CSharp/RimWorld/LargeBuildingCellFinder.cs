using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200353A RID: 13626
	public static class LargeBuildingCellFinder
	{
		// Token: 0x060131DE RID: 78302 RVA: 0x005AC25B File Offset: 0x005AA45B
		private static float MinRequiredScore(IntVec2 size)
		{
			return 0.5f * (float)size.Area;
		}

		// Token: 0x060131DF RID: 78303 RVA: 0x005AC26C File Offset: 0x005AA46C
		public static bool AnyCellFast(Map map, LargeBuildingSpawnParms parms)
		{
			for (int i = 0; i < 1000; i++)
			{
				if (LargeBuildingCellFinder.GetAreaScore(CellFinder.RandomCell(map), map, parms) >= LargeBuildingCellFinder.MinRequiredScore(parms.Size))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060131E0 RID: 78304 RVA: 0x005AC2A8 File Offset: 0x005AA4A8
		public static bool TryFindCell(out IntVec3 cell, Map map, LargeBuildingSpawnParms parms, List<CellRect> usedRects = null, Predicate<IntVec3> extraValidator = null, bool forceRecalculate = false)
		{
			if (LargeBuildingCellFinder.lastCalculatedCandidatesTick != Find.TickManager.TicksGame || forceRecalculate)
			{
				using (new ProfilerBlock("CalculateLocationCandidates"))
				{
					LargeBuildingCellFinder.CalculateLocationCandidates(map, parms);
				}
			}
			int num = 0;
			cell = IntVec3.Invalid;
			while (cell == IntVec3.Invalid && num < 1000)
			{
				num++;
				LocationCandidate locationCandidate;
				if (LargeBuildingCellFinder.locationCandidates.TryRandomElementByWeight((LocationCandidate x) => x.score, out locationCandidate) && !LargeBuildingCellFinder.IsRectUsed(GenAdj.OccupiedRect(locationCandidate.cell, Rot4.North, parms.Size), usedRects, parms) && (extraValidator == null || extraValidator(locationCandidate.cell)))
				{
					cell = locationCandidate.cell;
				}
			}
			return num < 1000;
		}

		// Token: 0x060131E1 RID: 78305 RVA: 0x005AC3AC File Offset: 0x005AA5AC
		public static bool TryFindCellNear(IntVec3 root, Map map, int squareRadius, LargeBuildingSpawnParms parms, out IntVec3 cell, bool forceRecalculate = false)
		{
			if (LargeBuildingCellFinder.lastCalculatedCandidatesTick != Find.TickManager.TicksGame || forceRecalculate)
			{
				using (new ProfilerBlock("CalculateLocationCandidates"))
				{
					LargeBuildingCellFinder.CalculateLocationCandidates(map, parms);
				}
			}
			int minX = root.x - squareRadius;
			int maxX = root.x + squareRadius;
			int minZ = root.z - squareRadius;
			int maxZ = root.z + squareRadius;
			LocationCandidate locationCandidate;
			if (!LargeBuildingCellFinder.locationCandidates.TryRandomElementByWeight(delegate(LocationCandidate x)
			{
				if (x.cell.x < minX || x.cell.x > maxX || x.cell.z < minZ || x.cell.z > maxZ)
				{
					return 0f;
				}
				return x.score;
			}, out locationCandidate))
			{
				cell = IntVec3.Invalid;
				return false;
			}
			cell = locationCandidate.cell;
			return true;
		}

		// Token: 0x060131E2 RID: 78306 RVA: 0x005AC47C File Offset: 0x005AA67C
		private static bool IsRectUsed(CellRect rect, List<CellRect> usedRects, LargeBuildingSpawnParms parms)
		{
			if (usedRects.NullOrEmpty<CellRect>())
			{
				return false;
			}
			foreach (CellRect cellRect in usedRects)
			{
				if (cellRect.Overlaps(rect))
				{
					return true;
				}
			}
			if (parms.minDistanceFromUsedRects > 0f)
			{
				foreach (CellRect cellRect2 in usedRects)
				{
					if (cellRect2.CenterCell.InHorDistOf(rect.CenterCell, parms.minDistanceFromUsedRects))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060131E3 RID: 78307 RVA: 0x005AC544 File Offset: 0x005AA744
		private static List<LocationCandidate> CalculateLocationCandidates(Map map, LargeBuildingSpawnParms parms)
		{
			LargeBuildingCellFinder.locationCandidates.Clear();
			if (parms.maxDistanceToColonyBuilding > 0f || parms.minDistanceToColonyBuilding > 0f || parms.preferFarFromColony)
			{
				CellFinderUtility.CalculateDistanceToColonyBuildingGrid(map);
			}
			int num = Mathf.Max(parms.Size.x / 2, 1);
			int num2 = Mathf.Max(parms.Size.z / 2, 1);
			for (int i = 0; i < map.Size.z; i += num2)
			{
				for (int j = 0; j < map.Size.x; j += num)
				{
					IntVec3 intVec = new IntVec3(j, 0, i);
					float areaScore = LargeBuildingCellFinder.GetAreaScore(intVec, map, parms);
					if (areaScore >= LargeBuildingCellFinder.MinRequiredScore(parms.Size))
					{
						LargeBuildingCellFinder.locationCandidates.Add(new LocationCandidate(intVec + (parms.Size / 2).ToIntVec3, areaScore));
					}
				}
			}
			LargeBuildingCellFinder.lastCalculatedCandidatesTick = Find.TickManager.TicksGame;
			return LargeBuildingCellFinder.locationCandidates;
		}

		// Token: 0x060131E4 RID: 78308 RVA: 0x005AC640 File Offset: 0x005AA840
		private static float GetAreaScore(IntVec3 topLeftCell, Map map, LargeBuildingSpawnParms parms)
		{
			float num = 0f;
			IntVec2 size = parms.Size;
			for (int i = 0; i < size.x; i++)
			{
				for (int j = 0; j < size.z; j++)
				{
					IntVec3 intVec = topLeftCell + new IntVec3(i, 0, j);
					if (!intVec.InBounds(map))
					{
						return 0f;
					}
					if (!parms.canSpawnOnImpassable && !intVec.Walkable(map))
					{
						return 0f;
					}
					float cellScore = LargeBuildingCellFinder.GetCellScore(intVec, map, parms);
					if (cellScore <= 0f)
					{
						return 0f;
					}
					num += cellScore;
				}
			}
			if (parms.preferFarFromColony && (float)CellFinderUtility.DistToColonyBuilding[topLeftCell] < 50f)
			{
				num *= 0.5f;
			}
			return num;
		}

		// Token: 0x060131E5 RID: 78309 RVA: 0x005AC6F8 File Offset: 0x005AA8F8
		private static float GetCellScore(IntVec3 cell, Map map, LargeBuildingSpawnParms parms)
		{
			float num = 1f;
			if (!parms.ignoreTerrainAffordance && !cell.SupportsStructureType(map, parms.thingDef.terrainAffordanceNeeded))
			{
				return 0f;
			}
			if (!parms.ignoreFoundations && map.terrainGrid.FoundationAt(cell) != null)
			{
				return 0f;
			}
			if (parms.maxDistanceToColonyBuilding > 0f && (float)CellFinderUtility.DistToColonyBuilding[cell] > parms.maxDistanceToColonyBuilding)
			{
				return 0f;
			}
			if (parms.minDistanceToColonyBuilding > 0f && (float)CellFinderUtility.DistToColonyBuilding[cell] < parms.minDistanceToColonyBuilding)
			{
				return 0f;
			}
			if (parms.minDistToEdge > 0 && cell.DistanceToEdge(map) < parms.minDistToEdge)
			{
				return 0f;
			}
			if (parms.maxDistanceFromPlayerStartPosition > 0f && !cell.InHorDistOf(MapGenerator.PlayerStartSpot, parms.maxDistanceFromPlayerStartPosition))
			{
				return 0f;
			}
			Building edifice = cell.GetEdifice(map);
			bool flag;
			if (edifice == null)
			{
				flag = false;
			}
			else
			{
				ThingDef def = edifice.def;
				bool? flag2;
				if (def == null)
				{
					flag2 = null;
				}
				else
				{
					BuildingProperties building = def.building;
					flag2 = ((building != null) ? new bool?(building.isNaturalRock) : null);
				}
				bool? flag3 = flag2;
				bool flag4 = true;
				flag = (flag3.GetValueOrDefault() == flag4 & flag3 != null);
			}
			if (flag)
			{
				return 0f;
			}
			if (!parms.allowFogged && cell.Fogged(map))
			{
				return 0f;
			}
			if (cell.GetTerrain(map).IsWater)
			{
				return 0f;
			}
			if (!parms.canSpawnOnImpassable && !cell.Walkable(map))
			{
				return 0f;
			}
			foreach (Thing thing in cell.GetThingList(map))
			{
				if (thing.def.preventSkyfallersLandingOn)
				{
					return 0f;
				}
				Building building2 = thing as Building;
				if (building2 != null)
				{
					if (!building2.def.Minifiable && building2.MarketValue > 50f)
					{
						return 0f;
					}
					if (!building2.def.destroyable || !building2.def.building.IsDeconstructible)
					{
						return 0f;
					}
					if (parms.attemptNotUnderBuildings)
					{
						num *= 0.5f;
					}
				}
			}
			SpawnLocationType attemptSpawnLocationType = parms.attemptSpawnLocationType;
			if (attemptSpawnLocationType != SpawnLocationType.Indoors)
			{
				if (attemptSpawnLocationType == SpawnLocationType.Outdoors)
				{
					if (!cell.GetRoom(map).PsychologicallyOutdoors)
					{
						num *= 0.01f;
					}
				}
			}
			else if (cell.GetRoom(map).PsychologicallyOutdoors)
			{
				num *= 0.01f;
			}
			return num;
		}

		// Token: 0x060131E6 RID: 78310 RVA: 0x005AC994 File Offset: 0x005AAB94
		public static void DebugDraw()
		{
			if (DebugViewSettings.drawFleshmassHeartChance)
			{
				if (LargeBuildingCellFinder.tmpHeartChanceCellColors == null)
				{
					LargeBuildingCellFinder.tmpHeartChanceCellColors = new List<Pair<IntVec3, float>>();
				}
				if (Time.frameCount % 60 == 0)
				{
					LargeBuildingCellFinder.tmpHeartChanceCellColors.Clear();
					Map currentMap = Find.CurrentMap;
					CellRect cellRect = Find.CameraDriver.CurrentViewRect;
					cellRect.ClipInsideMap(currentMap);
					cellRect = cellRect.ExpandedBy(1);
					CellFinderUtility.CalculateDistanceToColonyBuildingGrid(currentMap);
					List<LocationCandidate> list = LargeBuildingCellFinder.CalculateLocationCandidates(currentMap, IncidentWorker_FleshmassHeart.HeartSpawnParms.ForThing(ThingDefOf.FleshmassHeart));
					if (list.NullOrEmpty<LocationCandidate>())
					{
						return;
					}
					float inTo = list.Max((LocationCandidate c) => c.score);
					foreach (LocationCandidate locationCandidate in list)
					{
						if (cellRect.Contains(locationCandidate.cell) && locationCandidate.score >= LargeBuildingCellFinder.MinRequiredScore(ThingDefOf.FleshmassHeart.size))
						{
							float second = GenMath.LerpDouble(LargeBuildingCellFinder.MinRequiredScore(ThingDefOf.FleshmassHeart.size) - 1f, inTo, 0.25f, 0.5f, locationCandidate.score);
							LargeBuildingCellFinder.tmpHeartChanceCellColors.Add(new Pair<IntVec3, float>(locationCandidate.cell, second));
						}
					}
				}
				for (int i = 0; i < LargeBuildingCellFinder.tmpHeartChanceCellColors.Count; i++)
				{
					IntVec3 first = LargeBuildingCellFinder.tmpHeartChanceCellColors[i].First;
					float second2 = LargeBuildingCellFinder.tmpHeartChanceCellColors[i].Second;
					CellRenderer.RenderCell(first, SolidColorMaterials.SimpleSolidColorMaterial(new Color(0f, 0f, 1f, second2), false));
				}
				return;
			}
			LargeBuildingCellFinder.tmpHeartChanceCellColors = null;
		}

		// Token: 0x0400C655 RID: 50773
		private static int lastCalculatedCandidatesTick = -99999;

		// Token: 0x0400C656 RID: 50774
		private static List<LocationCandidate> locationCandidates = new List<LocationCandidate>();

		// Token: 0x0400C657 RID: 50775
		private const float MinRequiredScorePerCell = 0.5f;

		// Token: 0x0400C658 RID: 50776
		private const float SafeBuildingMarketValue = 50f;

		// Token: 0x0400C659 RID: 50777
		private static List<Pair<IntVec3, float>> tmpHeartChanceCellColors;
	}
}
