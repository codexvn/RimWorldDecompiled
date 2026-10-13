using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200040E RID: 1038
	public static class FloodFillerFog
	{
		// Token: 0x06001A49 RID: 6729 RVA: 0x00084BBC File Offset: 0x00082DBC
		public static FloodUnfogResult FloodUnfog(IntVec3 root, Map map)
		{
			FloodFillerFog.<>c__DisplayClass3_0 CS$<>8__locals1 = new FloodFillerFog.<>c__DisplayClass3_0();
			CS$<>8__locals1.map = map;
			FloodFillerFog.cellsToUnfog.Clear();
			CS$<>8__locals1.result = default(FloodUnfogResult);
			CS$<>8__locals1.fogGridDirect = CS$<>8__locals1.map.fogGrid.FogGrid_Unsafe;
			CS$<>8__locals1.fogGrid = CS$<>8__locals1.map.fogGrid;
			CS$<>8__locals1.newlyUnfoggedCells = new List<IntVec3>();
			CS$<>8__locals1.numUnfogged = 0;
			CS$<>8__locals1.expanding = false;
			CS$<>8__locals1.viewRect = CellRect.ViewRect(CS$<>8__locals1.map);
			CS$<>8__locals1.result.allOnScreen = true;
			CS$<>8__locals1.map.floodFiller.FloodFill(root, new Predicate<IntVec3>(CS$<>8__locals1.<FloodUnfog>g__PassCheck|0), new Action<IntVec3>(CS$<>8__locals1.<FloodUnfog>g__Processor|1), int.MaxValue, false, null);
			CS$<>8__locals1.expanding = true;
			for (int i = 0; i < CS$<>8__locals1.newlyUnfoggedCells.Count; i++)
			{
				IntVec3 a = CS$<>8__locals1.newlyUnfoggedCells[i];
				for (int j = 0; j < 8; j++)
				{
					IntVec3 intVec = a + GenAdj.AdjacentCells[j];
					if (intVec.InBounds(CS$<>8__locals1.map) && CS$<>8__locals1.fogGrid.IsFogged(intVec) && !CS$<>8__locals1.<FloodUnfog>g__PassCheck|0(intVec))
					{
						FloodFillerFog.cellsToUnfog.Add(intVec);
					}
				}
			}
			for (int k = 0; k < FloodFillerFog.cellsToUnfog.Count; k++)
			{
				CS$<>8__locals1.fogGrid.Unfog(FloodFillerFog.cellsToUnfog[k]);
				if (FloodFillerFog.testMode)
				{
					CS$<>8__locals1.map.debugDrawer.FlashCell(FloodFillerFog.cellsToUnfog[k], 0.3f, "x", 50);
				}
			}
			FloodFillerFog.cellsToUnfog.Clear();
			return CS$<>8__locals1.result;
		}

		// Token: 0x06001A4A RID: 6730 RVA: 0x00084D68 File Offset: 0x00082F68
		public static void DebugFloodUnfog(IntVec3 root, Map map)
		{
			map.fogGrid.SetAllFogged();
			foreach (IntVec3 loc in map.AllCells)
			{
				map.mapDrawer.MapMeshDirty(loc, MapMeshFlagDefOf.FogOfWar);
			}
			FloodFillerFog.testMode = true;
			FloodFillerFog.FloodUnfog(root, map);
			FloodFillerFog.testMode = false;
		}

		// Token: 0x06001A4B RID: 6731 RVA: 0x00084DE4 File Offset: 0x00082FE4
		public static void DebugRefogMap(Map map)
		{
			map.fogGrid.SetAllFogged();
			foreach (IntVec3 loc in map.AllCells)
			{
				map.mapDrawer.MapMeshDirty(loc, MapMeshFlagDefOf.FogOfWar);
			}
			FloodFillerFog.FloodUnfog(map.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>().Position, map);
		}

		// Token: 0x0400173E RID: 5950
		private static bool testMode = false;

		// Token: 0x0400173F RID: 5951
		private static List<IntVec3> cellsToUnfog = new List<IntVec3>(1024);

		// Token: 0x04001740 RID: 5952
		private const int MaxNumTestUnfog = 500;
	}
}
