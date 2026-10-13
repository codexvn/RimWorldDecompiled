using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BBA RID: 3002
	public abstract class Listing
	{
		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x06004CA9 RID: 19625 RVA: 0x00185C03 File Offset: 0x00183E03
		public float CurHeight
		{
			get
			{
				return this.curY;
			}
		}

		// Token: 0x17000BCF RID: 3023
		// (get) Token: 0x06004CAA RID: 19626 RVA: 0x00185C0B File Offset: 0x00183E0B
		public float MaxColumnHeightSeen
		{
			get
			{
				return Math.Max(this.CurHeight, this.maxHeightColumnSeen);
			}
		}

		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x06004CAC RID: 19628 RVA: 0x00185C2E File Offset: 0x00183E2E
		// (set) Token: 0x06004CAB RID: 19627 RVA: 0x00185C1E File Offset: 0x00183E1E
		public float ColumnWidth
		{
			get
			{
				return this.columnWidthInt;
			}
			set
			{
				this.columnWidthInt = value;
				this.hasCustomColumnWidth = true;
			}
		}

		// Token: 0x06004CAD RID: 19629 RVA: 0x00185C36 File Offset: 0x00183E36
		public void NewColumn()
		{
			this.maxHeightColumnSeen = Math.Max(this.curY, this.maxHeightColumnSeen);
			this.curY = 0f;
			this.curX += this.ColumnWidth + 17f;
		}

		// Token: 0x06004CAE RID: 19630 RVA: 0x00185C73 File Offset: 0x00183E73
		protected void NewColumnIfNeeded(float neededHeight)
		{
			if (this.maxOneColumn)
			{
				return;
			}
			if (this.curY + neededHeight > this.listingRect.height)
			{
				this.NewColumn();
			}
		}

		// Token: 0x06004CAF RID: 19631 RVA: 0x00185C99 File Offset: 0x00183E99
		public Rect GetRect(float height, float widthPct = 1f)
		{
			this.NewColumnIfNeeded(height);
			Rect result = new Rect(this.curX, this.curY, this.ColumnWidth * widthPct, height);
			this.curY += height;
			return result;
		}

		// Token: 0x06004CB0 RID: 19632 RVA: 0x00185CCA File Offset: 0x00183ECA
		public void Gap(float gapHeight = 12f)
		{
			this.curY += gapHeight;
		}

		// Token: 0x06004CB1 RID: 19633 RVA: 0x00185CDC File Offset: 0x00183EDC
		public void GapLine(float gapHeight = 12f)
		{
			float y = this.curY + gapHeight / 2f;
			Color color = GUI.color;
			GUI.color = color * new Color(1f, 1f, 1f, 0.4f);
			Widgets.DrawLineHorizontal(this.curX, y, this.ColumnWidth);
			GUI.color = color;
			this.curY += gapHeight;
		}

		// Token: 0x06004CB2 RID: 19634 RVA: 0x00185D46 File Offset: 0x00183F46
		public void Indent(float gapWidth = 12f)
		{
			this.curX += gapWidth;
		}

		// Token: 0x06004CB3 RID: 19635 RVA: 0x00185D56 File Offset: 0x00183F56
		public void Outdent(float gapWidth = 12f)
		{
			this.curX -= gapWidth;
		}

		// Token: 0x06004CB4 RID: 19636 RVA: 0x00185D68 File Offset: 0x00183F68
		public virtual void Begin(Rect rect)
		{
			this.listingRect = rect;
			if (this.hasCustomColumnWidth)
			{
				if (this.columnWidthInt > this.listingRect.width)
				{
					Log.Error(string.Concat(new string[]
					{
						"Listing set ColumnWith to ",
						this.columnWidthInt.ToString(),
						" which is more than the whole listing rect width of ",
						this.listingRect.width.ToString(),
						". Clamping."
					}));
					this.columnWidthInt = this.listingRect.width;
				}
			}
			else
			{
				this.columnWidthInt = this.listingRect.width;
			}
			this.curX = 0f;
			this.curY = 0f;
			this.maxHeightColumnSeen = 0f;
			Widgets.BeginGroup(rect);
		}

		// Token: 0x06004CB5 RID: 19637 RVA: 0x00185E2E File Offset: 0x0018402E
		public virtual void End()
		{
			Widgets.EndGroup();
		}

		// Token: 0x04003698 RID: 13976
		public float verticalSpacing = 2f;

		// Token: 0x04003699 RID: 13977
		protected Rect listingRect;

		// Token: 0x0400369A RID: 13978
		protected float curY;

		// Token: 0x0400369B RID: 13979
		protected float curX;

		// Token: 0x0400369C RID: 13980
		private float columnWidthInt;

		// Token: 0x0400369D RID: 13981
		private bool hasCustomColumnWidth;

		// Token: 0x0400369E RID: 13982
		private float maxHeightColumnSeen;

		// Token: 0x0400369F RID: 13983
		public bool maxOneColumn;

		// Token: 0x040036A0 RID: 13984
		public const float ColumnSpacing = 17f;

		// Token: 0x040036A1 RID: 13985
		public const float DefaultGap = 12f;

		// Token: 0x040036A2 RID: 13986
		public const float DefaultIndent = 12f;
	}
}
