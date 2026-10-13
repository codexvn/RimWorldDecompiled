using System;
using System.Collections.Generic;
using Unity.Collections;
using Verse.AI;

namespace Verse
{
	// Token: 0x020005D0 RID: 1488
	public class CostSource : SimplePathFinderDataSource<int>
	{
		// Token: 0x0600263B RID: 9787 RVA: 0x000C9E15 File Offset: 0x000C8015
		public CostSource(Map map, PathingContext context) : base(map)
		{
			this.context = context;
		}

		// Token: 0x0600263C RID: 9788 RVA: 0x000C9E25 File Offset: 0x000C8025
		public override void ComputeAll(IEnumerable<PathRequest> _)
		{
			this.data.CopyFrom(this.context.pathGrid.Grid_Unsafe);
		}

		// Token: 0x0600263D RID: 9789 RVA: 0x000C9E44 File Offset: 0x000C8044
		public override bool UpdateIncrementally(IEnumerable<PathRequest> _, List<IntVec3> cellDeltas)
		{
			CellIndices cellIndices = this.map.cellIndices;
			NativeArray<int> grid_Unsafe = this.context.pathGrid.Grid_Unsafe;
			foreach (IntVec3 c in cellDeltas)
			{
				int index = cellIndices.CellToIndex(c);
				this.data[index] = grid_Unsafe[index];
			}
			return false;
		}

		// Token: 0x04001FB4 RID: 8116
		protected readonly PathingContext context;
	}
}
