using System;
using System.Collections;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020005AE RID: 1454
	public struct CellConnectionEnumerator : IEnumerator<CellConnection>, IEnumerator, IDisposable
	{
		// Token: 0x0600258E RID: 9614 RVA: 0x000C55D5 File Offset: 0x000C37D5
		internal CellConnectionEnumerator(CellConnection bitmask)
		{
			this.bitmask = bitmask;
			this.current = CellConnection.Self;
			this.index = bitmask.BitIndex();
			this.end = bitmask.BitLoopEndIndex();
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x000026EA File Offset: 0x000008EA
		public void Dispose()
		{
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x000C5600 File Offset: 0x000C3800
		public bool MoveNext()
		{
			while (this.index < this.end)
			{
				CellConnection[] flagFromBitIndex = CellConnectionExtensions.FlagFromBitIndex;
				int num = this.index;
				this.index = num + 1;
				CellConnection testFlag = flagFromBitIndex[num];
				if (this.bitmask.HasBit(testFlag))
				{
					this.current = testFlag;
					return true;
				}
			}
			this.current = CellConnection.Self;
			return false;
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06002591 RID: 9617 RVA: 0x000C5654 File Offset: 0x000C3854
		public CellConnection Current
		{
			get
			{
				return this.current;
			}
		}

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06002592 RID: 9618 RVA: 0x000C565C File Offset: 0x000C385C
		object IEnumerator.Current
		{
			get
			{
				return this.current;
			}
		}

		// Token: 0x06002593 RID: 9619 RVA: 0x000C5669 File Offset: 0x000C3869
		void IEnumerator.Reset()
		{
			this.current = CellConnection.Self;
			this.index = this.bitmask.BitIndex();
			this.end = this.bitmask.BitLoopEndIndex();
		}

		// Token: 0x04001EE1 RID: 7905
		private readonly CellConnection bitmask;

		// Token: 0x04001EE2 RID: 7906
		private CellConnection current;

		// Token: 0x04001EE3 RID: 7907
		private int index;

		// Token: 0x04001EE4 RID: 7908
		private int end;
	}
}
