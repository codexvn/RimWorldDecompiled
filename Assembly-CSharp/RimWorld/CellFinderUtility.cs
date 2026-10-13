using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003522 RID: 13602
	public static class CellFinderUtility
	{
		// Token: 0x170031AF RID: 12719
		// (get) Token: 0x0601317C RID: 78204 RVA: 0x005A9C3B File Offset: 0x005A7E3B
		public static ByteGrid DistToColonyBuilding
		{
			get
			{
				return CellFinderUtility.distToColonyBuilding;
			}
		}

		// Token: 0x0601317D RID: 78205 RVA: 0x005A9C44 File Offset: 0x005A7E44
		public static void CalculateDistanceToColonyBuildingGrid(Map map)
		{
			if (CellFinderUtility.distToColonyBuilding == null)
			{
				CellFinderUtility.distToColonyBuilding = new ByteGrid(map);
			}
			else if (!CellFinderUtility.distToColonyBuilding.MapSizeMatches(map))
			{
				CellFinderUtility.distToColonyBuilding.ClearAndResizeTo(map);
			}
			CellFinderUtility.distToColonyBuilding.Clear(byte.MaxValue);
			CellFinderUtility.tmpColonyBuildingsLocs.Clear();
			List<Building> allBuildingsColonist = map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				Building building = allBuildingsColonist[i];
				if (!building.IsClearableFreeBuilding)
				{
					CellFinderUtility.tmpColonyBuildingsLocs.Add(building.Position);
				}
			}
			Dijkstra<IntVec3>.Run(CellFinderUtility.tmpColonyBuildingsLocs, (IntVec3 x) => DijkstraUtility.AdjacentCellsNeighborsGetter(x, map), delegate(IntVec3 a, IntVec3 b)
			{
				if (a.x == b.x || a.z == b.z)
				{
					return 1f;
				}
				return 1.4142135f;
			}, CellFinderUtility.tmpDistanceResult, null);
			for (int j = 0; j < CellFinderUtility.tmpDistanceResult.Count; j++)
			{
				CellFinderUtility.distToColonyBuilding[CellFinderUtility.tmpDistanceResult[j].Key] = (byte)Mathf.Min(CellFinderUtility.tmpDistanceResult[j].Value, 254.999f);
			}
		}

		// Token: 0x0400C5FB RID: 50683
		private static ByteGrid distToColonyBuilding;

		// Token: 0x0400C5FC RID: 50684
		private static readonly List<IntVec3> tmpColonyBuildingsLocs = new List<IntVec3>();

		// Token: 0x0400C5FD RID: 50685
		private static readonly List<KeyValuePair<IntVec3, float>> tmpDistanceResult = new List<KeyValuePair<IntVec3, float>>();
	}
}
