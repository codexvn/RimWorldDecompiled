using System;
using System.Collections.Generic;
using LudeonTK;
using Unity.Collections;

namespace Verse
{
	// Token: 0x020005D4 RID: 1492
	public class FogSource : SimpleBoolPathFinderDataSource
	{
		// Token: 0x0600264F RID: 9807 RVA: 0x000CA662 File Offset: 0x000C8862
		public FogSource(Map map) : base(map)
		{
		}

		// Token: 0x06002650 RID: 9808 RVA: 0x000CA798 File Offset: 0x000C8998
		public override void ComputeAll(IEnumerable<PathRequest> _)
		{
			this.data.CopyFrom(this.map.fogGrid.FogGrid_Unsafe);
		}

		// Token: 0x06002651 RID: 9809 RVA: 0x000CA7B8 File Offset: 0x000C89B8
		public override bool UpdateIncrementally(IEnumerable<PathRequest> _, List<IntVec3> cellDeltas)
		{
			CellIndices cellIndices = this.map.cellIndices;
			NativeBitArray fogGrid_Unsafe = this.map.fogGrid.FogGrid_Unsafe;
			foreach (IntVec3 c in cellDeltas)
			{
				int pos = cellIndices.CellToIndex(c);
				this.data.Set(pos, fogGrid_Unsafe.IsSet(pos));
			}
			return false;
		}
	}
}
