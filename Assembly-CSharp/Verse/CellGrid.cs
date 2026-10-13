using System;

namespace Verse
{
	// Token: 0x02000423 RID: 1059
	public class CellGrid
	{
		// Token: 0x170004D1 RID: 1233
		public IntVec3 this[IntVec3 c]
		{
			get
			{
				int num = CellIndicesUtility.CellToIndex(c, this.mapSizeX);
				return CellIndicesUtility.IndexToCell(this.grid[num], this.mapSizeX);
			}
			set
			{
				int num = CellIndicesUtility.CellToIndex(c, this.mapSizeX);
				this.grid[num] = CellIndicesUtility.CellToIndex(value, this.mapSizeX);
			}
		}

		// Token: 0x170004D2 RID: 1234
		public IntVec3 this[int index]
		{
			get
			{
				return CellIndicesUtility.IndexToCell(this.grid[index], this.mapSizeX);
			}
			set
			{
				this.grid[index] = CellIndicesUtility.CellToIndex(value, this.mapSizeX);
			}
		}

		// Token: 0x170004D3 RID: 1235
		public IntVec3 this[int x, int z]
		{
			get
			{
				int num = CellIndicesUtility.CellToIndex(x, z, this.mapSizeX);
				return CellIndicesUtility.IndexToCell(this.grid[num], this.mapSizeX);
			}
			set
			{
				int num = CellIndicesUtility.CellToIndex(x, z, this.mapSizeX);
				this.grid[num] = CellIndicesUtility.CellToIndex(x, z, this.mapSizeX);
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06001ADF RID: 6879 RVA: 0x00087CE8 File Offset: 0x00085EE8
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x06001AE0 RID: 6880 RVA: 0x000024DF File Offset: 0x000006DF
		public CellGrid()
		{
		}

		// Token: 0x06001AE1 RID: 6881 RVA: 0x00087CF2 File Offset: 0x00085EF2
		public CellGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x06001AE2 RID: 6882 RVA: 0x00087D01 File Offset: 0x00085F01
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x06001AE3 RID: 6883 RVA: 0x00087D2C File Offset: 0x00085F2C
		public void ClearAndResizeTo(Map map)
		{
			if (this.MapSizeMatches(map) && this.grid != null)
			{
				this.Clear();
				return;
			}
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
			this.grid = new int[this.mapSizeX * this.mapSizeZ];
			this.Clear();
		}

		// Token: 0x06001AE4 RID: 6884 RVA: 0x00087D94 File Offset: 0x00085F94
		public void Clear()
		{
			int num = CellIndicesUtility.CellToIndex(IntVec3.Invalid, this.mapSizeX);
			for (int i = 0; i < this.grid.Length; i++)
			{
				this.grid[i] = num;
			}
		}

		// Token: 0x0400179D RID: 6045
		private int[] grid;

		// Token: 0x0400179E RID: 6046
		private int mapSizeX;

		// Token: 0x0400179F RID: 6047
		private int mapSizeZ;
	}
}
