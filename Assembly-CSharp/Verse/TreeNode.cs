using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000BC7 RID: 3015
	public class TreeNode
	{
		// Token: 0x06004D25 RID: 19749 RVA: 0x0018890B File Offset: 0x00186B0B
		public bool IsOpen(int mask)
		{
			return (this.openBits & mask) != 0;
		}

		// Token: 0x06004D26 RID: 19750 RVA: 0x00188918 File Offset: 0x00186B18
		public void SetOpen(int mask, bool val)
		{
			if (val)
			{
				this.openBits |= mask;
				return;
			}
			this.openBits &= ~mask;
		}

		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x06004D27 RID: 19751 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool Openable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x040036DA RID: 14042
		public TreeNode parentNode;

		// Token: 0x040036DB RID: 14043
		public List<TreeNode> children;

		// Token: 0x040036DC RID: 14044
		public int nestDepth;

		// Token: 0x040036DD RID: 14045
		private int openBits;
	}
}
