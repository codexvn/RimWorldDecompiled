using System;
using System.Collections.Generic;
using Unity.Collections;

namespace Verse
{
	// Token: 0x020005B2 RID: 1458
	public abstract class SimpleBoolPathFinderDataSource : IPathFinderDataSource, IDisposable
	{
		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x060025A5 RID: 9637 RVA: 0x000C58C5 File Offset: 0x000C3AC5
		public NativeBitArray.ReadOnly Data
		{
			get
			{
				return this.data.AsReadOnly();
			}
		}

		// Token: 0x060025A6 RID: 9638 RVA: 0x000C58D2 File Offset: 0x000C3AD2
		public SimpleBoolPathFinderDataSource(Map map)
		{
			this.map = map;
			this.cellCount = map.cellIndices.NumGridCells;
			this.data = new NativeBitArray(this.cellCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x060025A7 RID: 9639 RVA: 0x000C590A File Offset: 0x000C3B0A
		public virtual void Dispose()
		{
			this.data.Dispose();
		}

		// Token: 0x060025A8 RID: 9640
		public abstract void ComputeAll(IEnumerable<PathRequest> requests);

		// Token: 0x060025A9 RID: 9641
		public abstract bool UpdateIncrementally(IEnumerable<PathRequest> requests, List<IntVec3> cellDeltas);

		// Token: 0x04001EEB RID: 7915
		protected readonly Map map;

		// Token: 0x04001EEC RID: 7916
		protected readonly int cellCount;

		// Token: 0x04001EED RID: 7917
		protected NativeBitArray data;
	}
}
