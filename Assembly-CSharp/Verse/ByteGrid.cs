using System;

namespace Verse
{
	// Token: 0x02000422 RID: 1058
	public sealed class ByteGrid : IExposable
	{
		// Token: 0x170004CD RID: 1229
		public byte this[IntVec3 c]
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

		// Token: 0x170004CE RID: 1230
		public byte this[int index]
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

		// Token: 0x170004CF RID: 1231
		public byte this[int x, int z]
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

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06001AD1 RID: 6865 RVA: 0x00087A91 File Offset: 0x00085C91
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x06001AD2 RID: 6866 RVA: 0x000024DF File Offset: 0x000006DF
		public ByteGrid()
		{
		}

		// Token: 0x06001AD3 RID: 6867 RVA: 0x00087A9B File Offset: 0x00085C9B
		public ByteGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x06001AD4 RID: 6868 RVA: 0x00087AAA File Offset: 0x00085CAA
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x06001AD5 RID: 6869 RVA: 0x00087AD4 File Offset: 0x00085CD4
		public void ClearAndResizeTo(Map map)
		{
			if (this.MapSizeMatches(map) && this.grid != null)
			{
				this.Clear(0);
				return;
			}
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
			this.grid = new byte[this.mapSizeX * this.mapSizeZ];
		}

		// Token: 0x06001AD6 RID: 6870 RVA: 0x00087B34 File Offset: 0x00085D34
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.mapSizeX, "mapSizeX", 0, false);
			Scribe_Values.Look<int>(ref this.mapSizeZ, "mapSizeZ", 0, false);
			DataExposeUtility.LookByteArray(ref this.grid, "grid");
		}

		// Token: 0x06001AD7 RID: 6871 RVA: 0x00087B6C File Offset: 0x00085D6C
		public void Clear(byte value = 0)
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

		// Token: 0x06001AD8 RID: 6872 RVA: 0x00087BB0 File Offset: 0x00085DB0
		public void DebugDraw()
		{
			for (int i = 0; i < this.grid.Length; i++)
			{
				byte b = this.grid[i];
				if (b > 0)
				{
					CellRenderer.RenderCell(CellIndicesUtility.IndexToCell(i, this.mapSizeX), (float)b / 255f * 0.5f);
				}
			}
		}

		// Token: 0x0400179A RID: 6042
		private byte[] grid;

		// Token: 0x0400179B RID: 6043
		private int mapSizeX;

		// Token: 0x0400179C RID: 6044
		private int mapSizeZ;
	}
}
