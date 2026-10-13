using System;
using System.Collections.Generic;
using Unity.Collections;

namespace Verse
{
	// Token: 0x020005B1 RID: 1457
	public abstract class SimplePathFinderDataSource<T> : IPathFinderDataSource, IDisposable where T : struct
	{
		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x060025A0 RID: 9632 RVA: 0x000C5878 File Offset: 0x000C3A78
		public NativeArray<T>.ReadOnly Data
		{
			get
			{
				return this.data.AsReadOnly();
			}
		}

		// Token: 0x060025A1 RID: 9633 RVA: 0x000C5885 File Offset: 0x000C3A85
		public SimplePathFinderDataSource(Map map)
		{
			this.map = map;
			this.cellCount = map.cellIndices.NumGridCells;
			this.data = new NativeArray<T>(this.cellCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x060025A2 RID: 9634 RVA: 0x000C58B8 File Offset: 0x000C3AB8
		public virtual void Dispose()
		{
			this.data.Dispose();
		}

		// Token: 0x060025A3 RID: 9635
		public abstract void ComputeAll(IEnumerable<PathRequest> requests);

		// Token: 0x060025A4 RID: 9636
		public abstract bool UpdateIncrementally(IEnumerable<PathRequest> requests, List<IntVec3> cellDeltas);

		// Token: 0x04001EE8 RID: 7912
		protected readonly Map map;

		// Token: 0x04001EE9 RID: 7913
		protected readonly int cellCount;

		// Token: 0x04001EEA RID: 7914
		protected NativeArray<T> data;
	}
}
