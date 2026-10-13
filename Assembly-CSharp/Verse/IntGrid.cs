using System;

namespace Verse
{
	// Token: 0x02000435 RID: 1077
	public sealed class IntGrid
	{
		// Token: 0x170004E3 RID: 1251
		public int this[IntVec3 c]
		{
			get
			{
				return this.grid[CellIndicesUtility.CellToIndex(c, this.mapSizeX)];
			}
			set
			{
				int num = CellIndicesUtility.CellToIndex(c, this.mapSizeX);
				this.grid[num] = value;
			}
		}

		// Token: 0x170004E4 RID: 1252
		public int this[int index]
		{
			get
			{
				return this.grid[index];
			}
			set
			{
				this.grid[index] = value;
			}
		}

		// Token: 0x170004E5 RID: 1253
		public int this[int x, int z]
		{
			get
			{
				return this.grid[CellIndicesUtility.CellToIndex(x, z, this.mapSizeX)];
			}
			set
			{
				this.grid[CellIndicesUtility.CellToIndex(x, z, this.mapSizeX)] = value;
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06001B92 RID: 7058 RVA: 0x0008AAE1 File Offset: 0x00088CE1
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x06001B93 RID: 7059 RVA: 0x000024DF File Offset: 0x000006DF
		public IntGrid()
		{
		}

		// Token: 0x06001B94 RID: 7060 RVA: 0x0008AAEB File Offset: 0x00088CEB
		public IntGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x06001B95 RID: 7061 RVA: 0x0008AAFA File Offset: 0x00088CFA
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x06001B96 RID: 7062 RVA: 0x0008AB24 File Offset: 0x00088D24
		public void ClearAndResizeTo(Map map)
		{
			if (this.MapSizeMatches(map) && this.grid != null)
			{
				this.Clear(0);
				return;
			}
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
			this.grid = new int[this.mapSizeX * this.mapSizeZ];
		}

		// Token: 0x06001B97 RID: 7063 RVA: 0x0008AB84 File Offset: 0x00088D84
		public void Clear(int value = 0)
		{
			if (value == 0)
			{
				Array.Clear(this.grid, 0, this.grid.Length);
				return;
			}
			for (int i = 0; i < this.grid.Length; i++)
			{
				this.grid[i] = value;
			}
		}

		// Token: 0x06001B98 RID: 7064 RVA: 0x0008ABC8 File Offset: 0x00088DC8
		public void DebugDraw()
		{
			for (int i = 0; i < this.grid.Length; i++)
			{
				int num = this.grid[i];
				if (num > 0)
				{
					CellRenderer.RenderCell(CellIndicesUtility.IndexToCell(i, this.mapSizeX), (float)(num % 100) / 100f * 0.5f);
				}
			}
		}

		// Token: 0x040017F3 RID: 6131
		private int[] grid;

		// Token: 0x040017F4 RID: 6132
		private int mapSizeX;

		// Token: 0x040017F5 RID: 6133
		private int mapSizeZ;
	}
}
