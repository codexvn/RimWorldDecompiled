using System;

namespace Verse.AI
{
	// Token: 0x0200104F RID: 4175
	public static class BreachingNoiseDebugDrawer
	{
		// Token: 0x060065CC RID: 26060 RVA: 0x00203F10 File Offset: 0x00202110
		public static void DebugDrawNoise(BreachingGrid grid)
		{
			Map currentMap = Find.CurrentMap;
			BreachingNoiseDebugDrawer.CheckInitDebugDrawGrid(grid);
			foreach (IntVec3 c in currentMap.AllCells)
			{
				if (BreachingNoiseDebugDrawer.debugDrawGrid[c] > 0)
				{
					CellRenderer.RenderCell(c, (float)BreachingNoiseDebugDrawer.debugDrawGrid[c] / 100f);
				}
			}
		}

		// Token: 0x060065CD RID: 26061 RVA: 0x00203F88 File Offset: 0x00202188
		private static void CheckInitDebugDrawGrid(BreachingGrid grid)
		{
			if (grid != BreachingNoiseDebugDrawer.debugGrid)
			{
				BreachingNoiseDebugDrawer.debugDrawGrid = null;
				BreachingNoiseDebugDrawer.debugGrid = grid;
			}
			if (BreachingNoiseDebugDrawer.debugDrawGrid == null)
			{
				BreachingNoiseDebugDrawer.debugDrawGrid = new IntGrid(grid.Map);
				BreachingNoiseDebugDrawer.debugDrawGrid.Clear(0);
				foreach (IntVec3 intVec in grid.Map.AllCells)
				{
					if (BreachingNoiseDebugDrawer.debugGrid.WithinNoise(intVec))
					{
						BreachingNoiseDebugDrawer.debugDrawGrid[intVec] = 1;
					}
				}
			}
		}

		// Token: 0x040048E2 RID: 18658
		private static BreachingGrid debugGrid;

		// Token: 0x040048E3 RID: 18659
		private static IntGrid debugDrawGrid;
	}
}
