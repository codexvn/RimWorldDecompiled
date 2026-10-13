using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003533 RID: 13619
	public static class InfestationCellFinder
	{
		// Token: 0x060131C2 RID: 78274 RVA: 0x005AB7B4 File Offset: 0x005A99B4
		public static bool TryFindCell(out IntVec3 cell, Map map)
		{
			InfestationCellFinder.CalculateLocationCandidates(map);
			LocationCandidate locationCandidate;
			if (!InfestationCellFinder.locationCandidates.TryRandomElementByWeight((LocationCandidate x) => x.score, out locationCandidate))
			{
				cell = IntVec3.Invalid;
				return false;
			}
			cell = CellFinder.FindNoWipeSpawnLocNear(locationCandidate.cell, map, ThingDefOf.Hive, Rot4.North, 2, (IntVec3 x) => InfestationCellFinder.GetScoreAt(x, map) > 0f && x.GetFirstThing(map, ThingDefOf.Hive) == null && x.GetFirstThing(map, ThingDefOf.TunnelHiveSpawner) == null);
			return true;
		}

		// Token: 0x060131C3 RID: 78275 RVA: 0x005AB844 File Offset: 0x005A9A44
		private static float GetScoreAt(IntVec3 cell, Map map)
		{
			if ((float)CellFinderUtility.DistToColonyBuilding[cell] > 30f)
			{
				return 0f;
			}
			if (!cell.Walkable(map))
			{
				return 0f;
			}
			if (cell.Fogged(map))
			{
				return 0f;
			}
			if (InfestationCellFinder.CellHasBlockingThings(cell, map))
			{
				return 0f;
			}
			if (!cell.Roofed(map) || !cell.GetRoof(map).isThickRoof)
			{
				return 0f;
			}
			Region region = cell.GetRegion(map, RegionType.Set_Passable);
			if (region == null)
			{
				return 0f;
			}
			if (InfestationCellFinder.closedAreaSize[cell] < 2)
			{
				return 0f;
			}
			float temperature = cell.GetTemperature(map);
			if (temperature < -17f)
			{
				return 0f;
			}
			float mountainousnessScoreAt = InfestationCellFinder.GetMountainousnessScoreAt(cell, map);
			if (mountainousnessScoreAt < 0.17f)
			{
				return 0f;
			}
			int num = InfestationCellFinder.StraightLineDistToUnroofed(cell, map);
			float num2;
			if (!InfestationCellFinder.regionsDistanceToUnroofed.TryGetValue(region, out num2))
			{
				num2 = (float)num * 1.15f;
			}
			else
			{
				num2 = Mathf.Min(num2, (float)num * 4f);
			}
			num2 = Mathf.Pow(num2, 1.55f);
			float num3 = Mathf.InverseLerp(0f, 12f, (float)num);
			float num4 = Mathf.Lerp(1f, 0.18f, map.glowGrid.GroundGlowAt(cell, false, false));
			float num5 = 1f - Mathf.Clamp(InfestationCellFinder.DistToBlocker(cell, map) / 11f, 0f, 0.6f);
			float num6 = Mathf.InverseLerp(-17f, -7f, temperature);
			float num7 = num2 * num3 * num5 * mountainousnessScoreAt * num4 * num6;
			num7 = Mathf.Pow(num7, 1.2f);
			if (num7 < 7.5f)
			{
				return 0f;
			}
			return num7;
		}

		// Token: 0x060131C4 RID: 78276 RVA: 0x005AB9E4 File Offset: 0x005A9BE4
		public static void DebugDraw()
		{
			if (DebugViewSettings.drawInfestationChance)
			{
				if (InfestationCellFinder.tmpCachedInfestationChanceCellColors == null)
				{
					InfestationCellFinder.tmpCachedInfestationChanceCellColors = new List<Pair<IntVec3, float>>();
				}
				if (Time.frameCount % 8 == 0)
				{
					InfestationCellFinder.tmpCachedInfestationChanceCellColors.Clear();
					Map currentMap = Find.CurrentMap;
					CellRect cellRect = Find.CameraDriver.CurrentViewRect;
					cellRect.ClipInsideMap(currentMap);
					cellRect = cellRect.ExpandedBy(1);
					InfestationCellFinder.CalculateTraversalDistancesToUnroofed(currentMap);
					InfestationCellFinder.CalculateClosedAreaSizeGrid(currentMap);
					CellFinderUtility.CalculateDistanceToColonyBuildingGrid(currentMap);
					float num = 0.001f;
					for (int i = 0; i < currentMap.Size.z; i++)
					{
						for (int j = 0; j < currentMap.Size.x; j++)
						{
							float scoreAt = InfestationCellFinder.GetScoreAt(new IntVec3(j, 0, i), currentMap);
							if (scoreAt > num)
							{
								num = scoreAt;
							}
						}
					}
					for (int k = 0; k < currentMap.Size.z; k++)
					{
						for (int l = 0; l < currentMap.Size.x; l++)
						{
							IntVec3 intVec = new IntVec3(l, 0, k);
							if (cellRect.Contains(intVec))
							{
								float scoreAt2 = InfestationCellFinder.GetScoreAt(intVec, currentMap);
								if (scoreAt2 > 7.5f)
								{
									float second = GenMath.LerpDouble(7.5f, num, 0f, 1f, scoreAt2);
									InfestationCellFinder.tmpCachedInfestationChanceCellColors.Add(new Pair<IntVec3, float>(intVec, second));
								}
							}
						}
					}
				}
				for (int m = 0; m < InfestationCellFinder.tmpCachedInfestationChanceCellColors.Count; m++)
				{
					IntVec3 first = InfestationCellFinder.tmpCachedInfestationChanceCellColors[m].First;
					float second2 = InfestationCellFinder.tmpCachedInfestationChanceCellColors[m].Second;
					CellRenderer.RenderCell(first, SolidColorMaterials.SimpleSolidColorMaterial(new Color(0f, 0f, 1f, second2), false));
				}
				return;
			}
			InfestationCellFinder.tmpCachedInfestationChanceCellColors = null;
		}

		// Token: 0x060131C5 RID: 78277 RVA: 0x005ABBA0 File Offset: 0x005A9DA0
		private static void CalculateLocationCandidates(Map map)
		{
			InfestationCellFinder.locationCandidates.Clear();
			InfestationCellFinder.CalculateTraversalDistancesToUnroofed(map);
			InfestationCellFinder.CalculateClosedAreaSizeGrid(map);
			CellFinderUtility.CalculateDistanceToColonyBuildingGrid(map);
			for (int i = 0; i < map.Size.z; i++)
			{
				for (int j = 0; j < map.Size.x; j++)
				{
					IntVec3 cell = new IntVec3(j, 0, i);
					float scoreAt = InfestationCellFinder.GetScoreAt(cell, map);
					if (scoreAt > 0f)
					{
						InfestationCellFinder.locationCandidates.Add(new LocationCandidate(cell, scoreAt));
					}
				}
			}
		}

		// Token: 0x060131C6 RID: 78278 RVA: 0x005ABC20 File Offset: 0x005A9E20
		private static bool CellHasBlockingThings(IntVec3 cell, Map map)
		{
			List<Thing> thingList = cell.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i] is Pawn || thingList[i] is Hive || thingList[i] is TunnelHiveSpawner)
				{
					return true;
				}
				if (thingList[i].def.category == ThingCategory.Building && thingList[i].def.passability == Traversability.Impassable && GenSpawn.SpawningWipes(ThingDefOf.Hive, thingList[i].def, false))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060131C7 RID: 78279 RVA: 0x005ABCC0 File Offset: 0x005A9EC0
		private static int StraightLineDistToUnroofed(IntVec3 cell, Map map)
		{
			int num = int.MaxValue;
			int i = 0;
			while (i < 4)
			{
				IntVec3 facingCell = new Rot4(i).FacingCell;
				int num2 = 0;
				int num3;
				for (;;)
				{
					IntVec3 intVec = cell + facingCell * num2;
					if (!intVec.InBounds(map))
					{
						goto Block_1;
					}
					num3 = num2;
					if (InfestationCellFinder.NoRoofAroundAndWalkable(intVec, map))
					{
						break;
					}
					num2++;
				}
				IL_56:
				if (num3 < num)
				{
					num = num3;
				}
				i++;
				continue;
				Block_1:
				num3 = int.MaxValue;
				goto IL_56;
			}
			if (num == 2147483647)
			{
				return map.Size.x;
			}
			return num;
		}

		// Token: 0x060131C8 RID: 78280 RVA: 0x005ABD48 File Offset: 0x005A9F48
		private static float DistToBlocker(IntVec3 cell, Map map)
		{
			int num = int.MinValue;
			int num2 = int.MinValue;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 facingCell = new Rot4(i).FacingCell;
				int num3 = 0;
				int num4;
				for (;;)
				{
					IntVec3 c = cell + facingCell * num3;
					num4 = num3;
					if (!c.InBounds(map) || !c.Walkable(map))
					{
						break;
					}
					num3++;
				}
				if (num4 > num)
				{
					num2 = num;
					num = num4;
				}
				else if (num4 > num2)
				{
					num2 = num4;
				}
			}
			return (float)Mathf.Min(num, num2);
		}

		// Token: 0x060131C9 RID: 78281 RVA: 0x005ABDCC File Offset: 0x005A9FCC
		private static bool NoRoofAroundAndWalkable(IntVec3 cell, Map map)
		{
			if (!cell.Walkable(map))
			{
				return false;
			}
			if (cell.Roofed(map))
			{
				return false;
			}
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = new Rot4(i).FacingCell + cell;
				if (c.InBounds(map) && c.Roofed(map))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060131CA RID: 78282 RVA: 0x005ABE28 File Offset: 0x005AA028
		private static float GetMountainousnessScoreAt(IntVec3 cell, Map map)
		{
			float num = 0f;
			int num2 = 0;
			for (int i = 0; i < 700; i += 10)
			{
				IntVec3 c = cell + GenRadial.RadialPattern[i];
				if (c.InBounds(map))
				{
					Building edifice = c.GetEdifice(map);
					if (edifice != null && edifice.def.category == ThingCategory.Building && edifice.def.building.isNaturalRock)
					{
						num += 1f;
					}
					else if (c.Roofed(map) && c.GetRoof(map).isThickRoof)
					{
						num += 0.5f;
					}
					num2++;
				}
			}
			return num / (float)num2;
		}

		// Token: 0x060131CB RID: 78283 RVA: 0x005ABECC File Offset: 0x005AA0CC
		private static void CalculateTraversalDistancesToUnroofed(Map map)
		{
			InfestationCellFinder.tempUnroofedRegions.Clear();
			for (int i = 0; i < map.Size.z; i++)
			{
				for (int j = 0; j < map.Size.x; j++)
				{
					IntVec3 intVec = new IntVec3(j, 0, i);
					Region region = intVec.GetRegion(map, RegionType.Set_Passable);
					if (region != null && InfestationCellFinder.NoRoofAroundAndWalkable(intVec, map))
					{
						InfestationCellFinder.tempUnroofedRegions.Add(region);
					}
				}
			}
			Dijkstra<Region>.Run(InfestationCellFinder.tempUnroofedRegions, (Region x) => x.Neighbors, (Region a, Region b) => Mathf.Sqrt((float)a.extentsClose.CenterCell.DistanceToSquared(b.extentsClose.CenterCell)), InfestationCellFinder.regionsDistanceToUnroofed, null);
			InfestationCellFinder.tempUnroofedRegions.Clear();
		}

		// Token: 0x060131CC RID: 78284 RVA: 0x005ABF94 File Offset: 0x005AA194
		private static void CalculateClosedAreaSizeGrid(Map map)
		{
			if (InfestationCellFinder.closedAreaSize == null)
			{
				InfestationCellFinder.closedAreaSize = new ByteGrid(map);
			}
			else
			{
				InfestationCellFinder.closedAreaSize.ClearAndResizeTo(map);
			}
			Predicate<IntVec3> <>9__0;
			Predicate<IntVec3> <>9__2;
			for (int i = 0; i < map.Size.z; i++)
			{
				for (int j = 0; j < map.Size.x; j++)
				{
					IntVec3 intVec = new IntVec3(j, 0, i);
					if (InfestationCellFinder.closedAreaSize[j, i] == 0 && !intVec.Impassable(map))
					{
						int area = 0;
						FloodFiller floodFiller = map.floodFiller;
						IntVec3 root = intVec;
						Predicate<IntVec3> passCheck;
						if ((passCheck = <>9__0) == null)
						{
							passCheck = (<>9__0 = ((IntVec3 c) => !c.Impassable(map)));
						}
						floodFiller.FloodFill(root, passCheck, delegate(IntVec3 c)
						{
							int area = area;
							area++;
						}, int.MaxValue, false, null);
						area = Mathf.Min(area, 255);
						FloodFiller floodFiller2 = map.floodFiller;
						IntVec3 root2 = intVec;
						Predicate<IntVec3> passCheck2;
						if ((passCheck2 = <>9__2) == null)
						{
							passCheck2 = (<>9__2 = ((IntVec3 c) => !c.Impassable(map)));
						}
						floodFiller2.FloodFill(root2, passCheck2, delegate(IntVec3 c)
						{
							InfestationCellFinder.closedAreaSize[c] = (byte)area;
						}, int.MaxValue, false, null);
					}
				}
			}
		}

		// Token: 0x0400C62C RID: 50732
		private static List<LocationCandidate> locationCandidates = new List<LocationCandidate>();

		// Token: 0x0400C62D RID: 50733
		private static Dictionary<Region, float> regionsDistanceToUnroofed = new Dictionary<Region, float>();

		// Token: 0x0400C62E RID: 50734
		private static ByteGrid closedAreaSize;

		// Token: 0x0400C62F RID: 50735
		private const float MinRequiredScore = 7.5f;

		// Token: 0x0400C630 RID: 50736
		private const float MinMountainousnessScore = 0.17f;

		// Token: 0x0400C631 RID: 50737
		private const int MountainousnessScoreRadialPatternIdx = 700;

		// Token: 0x0400C632 RID: 50738
		private const int MountainousnessScoreRadialPatternSkip = 10;

		// Token: 0x0400C633 RID: 50739
		private const float MountainousnessScorePerRock = 1f;

		// Token: 0x0400C634 RID: 50740
		private const float MountainousnessScorePerThickRoof = 0.5f;

		// Token: 0x0400C635 RID: 50741
		private const float MinCellTempToSpawnHive = -17f;

		// Token: 0x0400C636 RID: 50742
		private const float MaxDistanceToColonyBuilding = 30f;

		// Token: 0x0400C637 RID: 50743
		private static List<Pair<IntVec3, float>> tmpCachedInfestationChanceCellColors;

		// Token: 0x0400C638 RID: 50744
		private static HashSet<Region> tempUnroofedRegions = new HashSet<Region>();
	}
}
