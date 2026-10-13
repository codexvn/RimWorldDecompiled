using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BFC RID: 3068
	public class ListableOption
	{
		// Token: 0x06004E6E RID: 20078 RVA: 0x0018EFF7 File Offset: 0x0018D1F7
		public ListableOption(string label, Action action, string uiHighlightTag = null)
		{
			this.label = label;
			this.action = action;
			this.uiHighlightTag = uiHighlightTag;
		}

		// Token: 0x06004E6F RID: 20079 RVA: 0x0018F020 File Offset: 0x0018D220
		public virtual float DrawOption(Vector2 pos, float width)
		{
			float b = Text.CalcHeight(this.label, width);
			float num = Mathf.Max(this.minHeight, b);
			Rect rect = new Rect(pos.x, pos.y, width, num);
			if (Widgets.ButtonText(rect, this.label, true, true, true, null))
			{
				this.action();
			}
			if (this.uiHighlightTag != null)
			{
				UIHighlighter.HighlightOpportunity(rect, this.uiHighlightTag);
			}
			return num;
		}

		// Token: 0x040037B7 RID: 14263
		public string label;

		// Token: 0x040037B8 RID: 14264
		public Action action;

		// Token: 0x040037B9 RID: 14265
		private string uiHighlightTag;

		// Token: 0x040037BA RID: 14266
		public float minHeight = 45f;
	}
}
