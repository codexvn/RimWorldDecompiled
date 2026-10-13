using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010FF RID: 4351
	public class RecipeTooltipLayout
	{
		// Token: 0x17001062 RID: 4194
		// (get) Token: 0x060069FE RID: 27134 RVA: 0x00216323 File Offset: 0x00214523
		public Vector2 Size
		{
			get
			{
				return this.maxSize;
			}
		}

		// Token: 0x17001063 RID: 4195
		// (get) Token: 0x060069FF RID: 27135 RVA: 0x0021632B File Offset: 0x0021452B
		public bool Empty
		{
			get
			{
				return this.Size.x <= 0f || this.Size.y <= 0f;
			}
		}

		// Token: 0x06006A00 RID: 27136 RVA: 0x00216358 File Offset: 0x00214558
		public void Label(string text, bool draw, GameFont font = GameFont.Small)
		{
			GameFont font2 = Text.Font;
			Text.Font = font;
			Vector2 vector = Text.CalcSize(text);
			Rect rect = new Rect(this.curPos.x, this.curPos.y, vector.x, vector.y);
			if (draw)
			{
				Widgets.Label(rect, text);
			}
			this.curPos.x = rect.xMax;
			this.ExpandToFit(rect.xMax, rect.yMax);
			Text.Font = font2;
		}

		// Token: 0x06006A01 RID: 27137 RVA: 0x002163D8 File Offset: 0x002145D8
		public void Icon(Texture2D icon, Color color, float iconSize, bool draw)
		{
			Rect position = new Rect(this.curPos.x, this.curPos.y, iconSize, iconSize);
			if (draw)
			{
				Color color2 = GUI.color;
				GUI.color = color;
				GUI.DrawTexture(position, icon);
				GUI.color = color2;
			}
			this.curPos.x = position.xMax;
			this.ExpandToFit(position.xMax, position.yMax);
		}

		// Token: 0x06006A02 RID: 27138 RVA: 0x00216445 File Offset: 0x00214645
		public void Gap(float x, float y)
		{
			this.curPos.x = this.curPos.x + x;
			this.curPos.y = this.curPos.y + y;
			this.ExpandToFit(this.curPos.x, this.curPos.y);
		}

		// Token: 0x06006A03 RID: 27139 RVA: 0x00216484 File Offset: 0x00214684
		public void Reset(float newIndent = 0f)
		{
			this.curPos = Vector2.zero;
			this.maxSize = Vector2.zero;
			this.indent = newIndent;
			this.curPos.x = Mathf.Max(this.curPos.x, this.indent);
			this.curPos.y = Mathf.Max(this.curPos.y, this.indent);
		}

		// Token: 0x06006A04 RID: 27140 RVA: 0x002164F0 File Offset: 0x002146F0
		public void Expand(float width, float height)
		{
			this.maxSize.x = this.maxSize.x + width;
			this.maxSize.y = this.maxSize.y + height;
		}

		// Token: 0x06006A05 RID: 27141 RVA: 0x00216514 File Offset: 0x00214714
		public void Newline(GameFont font = GameFont.Small)
		{
			this.curPos.x = this.indent;
			this.curPos.y = this.curPos.y + Text.LineHeightOf(font);
			this.ExpandToFit(this.curPos.x, this.curPos.y);
		}

		// Token: 0x06006A06 RID: 27142 RVA: 0x00216563 File Offset: 0x00214763
		private void ExpandToFit(float x, float y)
		{
			this.maxSize.x = Mathf.Max(this.maxSize.x, x);
			this.maxSize.y = Mathf.Max(this.maxSize.y, y);
		}

		// Token: 0x04004AED RID: 19181
		private Vector2 curPos;

		// Token: 0x04004AEE RID: 19182
		private Vector2 maxSize;

		// Token: 0x04004AEF RID: 19183
		private float indent;
	}
}
