using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020005D7 RID: 1495
	public class WaterSource : SimpleBoolPathFinderDataSource
	{
		// Token: 0x0600265B RID: 9819 RVA: 0x000CA662 File Offset: 0x000C8862
		public WaterSource(Map map) : base(map)
		{
		}

		// Token: 0x0600265C RID: 9820 RVA: 0x000CABA8 File Offset: 0x000C8DA8
		public override void ComputeAll(IEnumerable<PathRequest> _)
		{
			this.data.Clear();
			TerrainGrid terrainGrid = this.map.terrainGrid;
			for (int i = 0; i < this.cellCount; i++)
			{
				TerrainDef terrainDef = terrainGrid.TerrainAt(i);
				if (terrainDef != null && terrainDef.IsWater)
				{
					this.data.Set(i, true);
				}
			}
		}

		// Token: 0x0600265D RID: 9821 RVA: 0x000CAC00 File Offset: 0x000C8E00
		public override bool UpdateIncrementally(IEnumerable<PathRequest> _, List<IntVec3> cellDeltas)
		{
			CellIndices cellIndices = this.map.cellIndices;
			TerrainGrid terrainGrid = this.map.terrainGrid;
			foreach (IntVec3 c in cellDeltas)
			{
				int num = cellIndices.CellToIndex(c);
				int pos = num;
				TerrainDef terrainDef = terrainGrid.TerrainAt(num);
				this.data.Set(pos, terrainDef != null && terrainDef.IsWater);
			}
			return false;
		}
	}
}
