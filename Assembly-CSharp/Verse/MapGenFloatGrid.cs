using System;
using LudeonTK;
using Unity.Collections;

namespace Verse
{
	// Token: 0x020004BF RID: 1215
	public class MapGenFloatGrid : IDisposable
	{
		// Token: 0x1700059B RID: 1435
		public float this[IntVec3 c]
		{
			get
			{
				return this.grid[this.map.cellIndices.CellToIndex(c)];
			}
			set
			{
				this.grid[this.map.cellIndices.CellToIndex(c)] = value;
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06001F80 RID: 8064 RVA: 0x000A5B6B File Offset: 0x000A3D6B
		internal ref NativeArray<float> Grid_Unsafe
		{
			get
			{
				return ref this.grid;
			}
		}

		// Token: 0x06001F81 RID: 8065 RVA: 0x000A5B73 File Offset: 0x000A3D73
		public MapGenFloatGrid(Map map)
		{
			this.map = map;
			this.grid = new NativeArray<float>(map.cellIndices.NumGridCells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x06001F82 RID: 8066 RVA: 0x000A5B9A File Offset: 0x000A3D9A
		public void Clear()
		{
			NativeArrayUtility.MemClear<float>(this.grid);
		}

		// Token: 0x06001F83 RID: 8067 RVA: 0x000A5BA7 File Offset: 0x000A3DA7
		public void Dispose()
		{
			this.grid.Dispose();
		}

		// Token: 0x04001B24 RID: 6948
		private readonly Map map;

		// Token: 0x04001B25 RID: 6949
		private NativeArray<float> grid;
	}
}
