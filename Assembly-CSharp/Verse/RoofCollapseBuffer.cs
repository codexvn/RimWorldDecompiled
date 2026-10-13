using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000519 RID: 1305
	public class RoofCollapseBuffer
	{
		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06002216 RID: 8726 RVA: 0x000B1840 File Offset: 0x000AFA40
		public List<IntVec3> CellsMarkedToCollapse
		{
			get
			{
				return this.cellsToCollapse;
			}
		}

		// Token: 0x06002217 RID: 8727 RVA: 0x000B1848 File Offset: 0x000AFA48
		public bool IsMarkedToCollapse(IntVec3 c)
		{
			return this.cellsToCollapse.Contains(c);
		}

		// Token: 0x06002218 RID: 8728 RVA: 0x000B1856 File Offset: 0x000AFA56
		public void MarkToCollapse(IntVec3 c)
		{
			if (!this.cellsToCollapse.Contains(c))
			{
				this.cellsToCollapse.Add(c);
			}
		}

		// Token: 0x06002219 RID: 8729 RVA: 0x000B1872 File Offset: 0x000AFA72
		public void Clear()
		{
			this.cellsToCollapse.Clear();
		}

		// Token: 0x04001CBE RID: 7358
		private List<IntVec3> cellsToCollapse = new List<IntVec3>();
	}
}
