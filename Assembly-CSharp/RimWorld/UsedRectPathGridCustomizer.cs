using System;
using Unity.Collections;
using Verse;

namespace RimWorld
{
	// Token: 0x0200203C RID: 8252
	public class UsedRectPathGridCustomizer : PathRequest.IPathGridCustomizer, IDisposable
	{
		// Token: 0x0600B54E RID: 46414 RVA: 0x0034222C File Offset: 0x0034042C
		public UsedRectPathGridCustomizer(Map map)
		{
			this.grid = new NativeArray<ushort>(map.cellIndices.NumGridCells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			foreach (CellRect cellRect in MapGenerator.UsedRects)
			{
				foreach (IntVec3 c in cellRect.ExpandedBy(2).Cells)
				{
					if (c.InBounds(map))
					{
						this.grid[map.cellIndices.CellToIndex(c)] = 10000;
					}
				}
			}
		}

		// Token: 0x0600B54F RID: 46415 RVA: 0x003422FC File Offset: 0x003404FC
		public NativeArray<ushort> GetOffsetGrid()
		{
			return this.grid;
		}

		// Token: 0x0600B550 RID: 46416 RVA: 0x00342304 File Offset: 0x00340504
		public void Dispose()
		{
			this.grid.Dispose();
		}

		// Token: 0x04007C84 RID: 31876
		private NativeArray<ushort> grid;
	}
}
