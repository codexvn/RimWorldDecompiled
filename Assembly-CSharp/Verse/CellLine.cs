using System;

namespace Verse
{
	// Token: 0x02000C63 RID: 3171
	public struct CellLine
	{
		// Token: 0x17000C4F RID: 3151
		// (get) Token: 0x06005125 RID: 20773 RVA: 0x001A57AF File Offset: 0x001A39AF
		public float ZIntercept
		{
			get
			{
				return this.zIntercept;
			}
		}

		// Token: 0x17000C50 RID: 3152
		// (get) Token: 0x06005126 RID: 20774 RVA: 0x001A57B7 File Offset: 0x001A39B7
		public float Slope
		{
			get
			{
				return this.slope;
			}
		}

		// Token: 0x06005127 RID: 20775 RVA: 0x001A57BF File Offset: 0x001A39BF
		public CellLine(float zIntercept, float slope)
		{
			this.zIntercept = zIntercept;
			this.slope = slope;
		}

		// Token: 0x06005128 RID: 20776 RVA: 0x001A57CF File Offset: 0x001A39CF
		public CellLine(IntVec3 cell, float slope)
		{
			this.slope = slope;
			this.zIntercept = (float)cell.z - (float)cell.x * slope;
		}

		// Token: 0x06005129 RID: 20777 RVA: 0x001A57F0 File Offset: 0x001A39F0
		public static CellLine Between(IntVec3 a, IntVec3 b)
		{
			float num;
			if (a.x == b.x)
			{
				num = 100000000f;
			}
			else
			{
				num = (float)(b.z - a.z) / (float)(b.x - a.x);
			}
			return new CellLine((float)a.z - (float)a.x * num, num);
		}

		// Token: 0x0600512A RID: 20778 RVA: 0x001A5848 File Offset: 0x001A3A48
		public bool CellIsAbove(IntVec3 c)
		{
			return (float)c.z > this.slope * (float)c.x + this.zIntercept;
		}

		// Token: 0x04003A3C RID: 14908
		private float zIntercept;

		// Token: 0x04003A3D RID: 14909
		private float slope;
	}
}
