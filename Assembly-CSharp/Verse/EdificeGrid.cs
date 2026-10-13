using System;

namespace Verse
{
	// Token: 0x02000426 RID: 1062
	public sealed class EdificeGrid
	{
		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06001AFD RID: 6909 RVA: 0x00088436 File Offset: 0x00086636
		public Building[] InnerArray
		{
			get
			{
				return this.innerArray;
			}
		}

		// Token: 0x170004D9 RID: 1241
		public Building this[int index]
		{
			get
			{
				return this.innerArray[index];
			}
		}

		// Token: 0x170004DA RID: 1242
		public Building this[IntVec3 c]
		{
			get
			{
				return this.innerArray[this.map.cellIndices.CellToIndex(c)];
			}
		}

		// Token: 0x06001B00 RID: 6912 RVA: 0x00088462 File Offset: 0x00086662
		public EdificeGrid(Map map)
		{
			this.map = map;
			this.innerArray = new Building[map.cellIndices.NumGridCells];
		}

		// Token: 0x06001B01 RID: 6913 RVA: 0x00088488 File Offset: 0x00086688
		public void Register(Building ed)
		{
			CellIndices cellIndices = this.map.cellIndices;
			CellRect cellRect = ed.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c = new IntVec3(j, 0, i);
					this.innerArray[cellIndices.CellToIndex(c)] = ed;
				}
			}
		}

		// Token: 0x06001B02 RID: 6914 RVA: 0x000884F0 File Offset: 0x000866F0
		public void DeRegister(Building ed)
		{
			CellIndices cellIndices = this.map.cellIndices;
			CellRect cellRect = ed.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					this.innerArray[cellIndices.CellToIndex(j, i)] = null;
				}
			}
		}

		// Token: 0x040017A9 RID: 6057
		private Map map;

		// Token: 0x040017AA RID: 6058
		private Building[] innerArray;
	}
}
