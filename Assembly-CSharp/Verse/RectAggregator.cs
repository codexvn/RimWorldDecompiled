using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BCF RID: 3023
	public struct RectAggregator
	{
		// Token: 0x17000BEB RID: 3051
		// (get) Token: 0x06004D5D RID: 19805 RVA: 0x00189B9E File Offset: 0x00187D9E
		public Rect Rect
		{
			get
			{
				return this.currentRect;
			}
		}

		// Token: 0x06004D5E RID: 19806 RVA: 0x00189BA8 File Offset: 0x00187DA8
		public RectAggregator(Rect parent, int contextHash, Vector2? margin = null)
		{
			this.currentRect = parent;
			this.margin = (margin ?? new Vector2(10f, 4f));
			this.contextHash = contextHash;
		}

		// Token: 0x06004D5F RID: 19807 RVA: 0x00189B9E File Offset: 0x00187D9E
		public static implicit operator Rect(RectAggregator rect)
		{
			return rect.currentRect;
		}

		// Token: 0x06004D60 RID: 19808 RVA: 0x00189BEC File Offset: 0x00187DEC
		public RectDivider NewRow(float height, VerticalJustification addAt = VerticalJustification.Bottom)
		{
			Rect parent;
			if (addAt == VerticalJustification.Top)
			{
				this.currentRect.yMin = this.currentRect.yMin - (this.margin.y + height);
				parent = new Rect(this.currentRect.x, this.currentRect.y, this.currentRect.width, height);
			}
			else
			{
				if (addAt != VerticalJustification.Bottom)
				{
					throw new InvalidOperationException();
				}
				this.currentRect.yMax = this.currentRect.yMax + (this.margin.y + height);
				parent = new Rect(this.currentRect.x, this.currentRect.yMax - height, this.currentRect.width, height);
			}
			return new RectDivider(parent, this.contextHash, new Vector2?(this.margin));
		}

		// Token: 0x06004D61 RID: 19809 RVA: 0x00189CB4 File Offset: 0x00187EB4
		public RectDivider NewCol(float width, HorizontalJustification addAt = HorizontalJustification.Right)
		{
			Rect parent;
			if (addAt == HorizontalJustification.Left)
			{
				this.currentRect.xMin = this.currentRect.xMin - (this.margin.x + width);
				parent = new Rect(this.currentRect.x, this.currentRect.y, width, this.currentRect.height);
			}
			else
			{
				if (addAt != HorizontalJustification.Right)
				{
					throw new InvalidOperationException();
				}
				this.currentRect.xMax = this.currentRect.xMax + (this.margin.x + width);
				parent = new Rect(this.currentRect.xMax - width, this.currentRect.y, width, this.currentRect.height);
			}
			return new RectDivider(parent, this.contextHash, new Vector2?(this.margin));
		}

		// Token: 0x040036FF RID: 14079
		private Rect currentRect;

		// Token: 0x04003700 RID: 14080
		private Vector2 margin;

		// Token: 0x04003701 RID: 14081
		private int contextHash;
	}
}
