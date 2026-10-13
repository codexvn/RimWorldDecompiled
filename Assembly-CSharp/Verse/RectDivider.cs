using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BD2 RID: 3026
	public struct RectDivider
	{
		// Token: 0x17000BEC RID: 3052
		// (get) Token: 0x06004D62 RID: 19810 RVA: 0x00189D7B File Offset: 0x00187F7B
		private int ErrorHash
		{
			get
			{
				return this.contextHash ^ 1006562692;
			}
		}

		// Token: 0x17000BED RID: 3053
		// (get) Token: 0x06004D63 RID: 19811 RVA: 0x00189D89 File Offset: 0x00187F89
		public Rect Rect
		{
			get
			{
				return this.currentRect;
			}
		}

		// Token: 0x06004D64 RID: 19812 RVA: 0x00189D94 File Offset: 0x00187F94
		public RectDivider(Rect parent, int contextHash, Vector2? margin = null)
		{
			this.currentRect = parent;
			this.margin = (margin ?? new Vector2(17f, 4f));
			this.contextHash = contextHash;
		}

		// Token: 0x06004D65 RID: 19813 RVA: 0x00189D89 File Offset: 0x00187F89
		public static implicit operator Rect(RectDivider rect)
		{
			return rect.currentRect;
		}

		// Token: 0x06004D66 RID: 19814 RVA: 0x00189DD8 File Offset: 0x00187FD8
		public RectDivider NewRow(float height, VerticalJustification justification = VerticalJustification.Top, float? marginOverride = null)
		{
			if (justification == VerticalJustification.Top)
			{
				Rect rect = this.currentRect;
				float? num = marginOverride;
				float compressibleMargin = num ?? this.margin.y;
				float? topHeight = new float?(height);
				num = null;
				Rect parent;
				Rect rect2;
				float num2;
				if (!rect.SplitHorizontallyWithMargin(out parent, out rect2, out num2, compressibleMargin, topHeight, num))
				{
					Log.ErrorOnce(string.Format("Rect height was too small by {0} for the requested row height.", num2), this.ErrorHash);
				}
				this.currentRect = rect2;
				return new RectDivider(parent, this.contextHash, new Vector2?(this.margin));
			}
			if (justification == VerticalJustification.Bottom)
			{
				Rect rect3 = this.currentRect;
				float compressibleMargin2 = marginOverride ?? this.margin.y;
				float? num = new float?(height);
				Rect rect4;
				Rect parent2;
				float num3;
				if (!rect3.SplitHorizontallyWithMargin(out rect4, out parent2, out num3, compressibleMargin2, null, num))
				{
					Log.ErrorOnce(string.Format("Rect height was too small by {0} for the requested rows height.", num3), this.ErrorHash);
				}
				this.currentRect = rect4;
				return new RectDivider(parent2, this.contextHash, new Vector2?(this.margin));
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06004D67 RID: 19815 RVA: 0x00189EF4 File Offset: 0x001880F4
		public RectDivider NewCol(float width, HorizontalJustification justification = HorizontalJustification.Left, float? marginOverride = null)
		{
			if (justification == HorizontalJustification.Left)
			{
				Rect rect = this.currentRect;
				float? num = marginOverride;
				float compressibleMargin = num ?? this.margin.x;
				float? leftWidth = new float?(width);
				num = null;
				Rect parent;
				Rect rect2;
				float num2;
				if (!rect.SplitVerticallyWithMargin(out parent, out rect2, out num2, compressibleMargin, leftWidth, num))
				{
					Log.ErrorOnce(string.Format("Rect width was too small by {0} for the requested column width.", num2), this.ErrorHash);
				}
				this.currentRect = rect2;
				return new RectDivider(parent, this.contextHash, new Vector2?(this.margin));
			}
			if (justification == HorizontalJustification.Right)
			{
				Rect rect3 = this.currentRect;
				float compressibleMargin2 = marginOverride ?? this.margin.x;
				float? num = new float?(width);
				Rect rect4;
				Rect parent2;
				float num3;
				if (!rect3.SplitVerticallyWithMargin(out rect4, out parent2, out num3, compressibleMargin2, null, num))
				{
					Log.ErrorOnce(string.Format("Rect width was too small by {0} for the requested column width.", num3), this.ErrorHash);
				}
				this.currentRect = rect4;
				return new RectDivider(parent2, this.contextHash, new Vector2?(this.margin));
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06004D68 RID: 19816 RVA: 0x0018A010 File Offset: 0x00188210
		public RectDivider CreateViewRect(int count, float rowHeight)
		{
			float num = 0f;
			if (count > 0)
			{
				num = (float)count * rowHeight + this.margin.y * (float)(count - 1);
			}
			float num2 = this.Rect.width;
			if (num > this.Rect.height)
			{
				num2 -= 16f;
			}
			return new RectDivider(new Rect(0f, 0f, num2, num), this.contextHash, null);
		}

		// Token: 0x04003708 RID: 14088
		private Rect currentRect;

		// Token: 0x04003709 RID: 14089
		private Vector2 margin;

		// Token: 0x0400370A RID: 14090
		private int contextHash;
	}
}
