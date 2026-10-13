using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B98 RID: 2968
	public class GridLayout
	{
		// Token: 0x06004BEE RID: 19438 RVA: 0x00182B68 File Offset: 0x00180D68
		public GridLayout(Rect container, int cols = 1, int rows = 1, float outerPadding = 4f, float innerPadding = 4f)
		{
			this.container = new Rect(container);
			this.cols = cols;
			this.innerPadding = innerPadding;
			this.outerPadding = outerPadding;
			float num = container.width - outerPadding * 2f - (float)(cols - 1) * innerPadding;
			float num2 = container.height - outerPadding * 2f - (float)(rows - 1) * innerPadding;
			this.colWidth = num / (float)cols;
			this.rowHeight = num2 / (float)rows;
			this.colStride = this.colWidth + innerPadding;
			this.rowStride = this.rowHeight + innerPadding;
		}

		// Token: 0x06004BEF RID: 19439 RVA: 0x00182C04 File Offset: 0x00180E04
		public GridLayout(float colWidth, float rowHeight, int cols, int rows, float outerPadding = 4f, float innerPadding = 4f)
		{
			this.colWidth = colWidth;
			this.rowHeight = rowHeight;
			this.cols = cols;
			this.innerPadding = innerPadding;
			this.outerPadding = outerPadding;
			this.colStride = colWidth + innerPadding;
			this.rowStride = rowHeight + innerPadding;
			this.container = new Rect(0f, 0f, outerPadding * 2f + colWidth * (float)cols + innerPadding * (float)cols - 1f, outerPadding * 2f + rowHeight * (float)rows + innerPadding * (float)rows - 1f);
		}

		// Token: 0x06004BF0 RID: 19440 RVA: 0x00182C9C File Offset: 0x00180E9C
		public Rect GetCellRectByIndex(int index, int colspan = 1, int rowspan = 1)
		{
			int col = index % this.cols;
			int row = index / this.cols;
			return this.GetCellRect(col, row, colspan, rowspan);
		}

		// Token: 0x06004BF1 RID: 19441 RVA: 0x00182CC8 File Offset: 0x00180EC8
		public Rect GetCellRect(int col, int row, int colspan = 1, int rowspan = 1)
		{
			return new Rect(Mathf.Floor(this.container.x + this.outerPadding + (float)col * this.colStride), Mathf.Floor(this.container.y + this.outerPadding + (float)row * this.rowStride), Mathf.Ceil(this.colWidth) * (float)colspan + this.innerPadding * (float)(colspan - 1), Mathf.Ceil(this.rowHeight) * (float)rowspan + this.innerPadding * (float)(rowspan - 1));
		}

		// Token: 0x04003638 RID: 13880
		public Rect container;

		// Token: 0x04003639 RID: 13881
		private int cols;

		// Token: 0x0400363A RID: 13882
		private float outerPadding;

		// Token: 0x0400363B RID: 13883
		private float innerPadding;

		// Token: 0x0400363C RID: 13884
		private float colStride;

		// Token: 0x0400363D RID: 13885
		private float rowStride;

		// Token: 0x0400363E RID: 13886
		private float colWidth;

		// Token: 0x0400363F RID: 13887
		private float rowHeight;
	}
}
