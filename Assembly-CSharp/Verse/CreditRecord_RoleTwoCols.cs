using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C6F RID: 3183
	public class CreditRecord_RoleTwoCols : CreditsEntry
	{
		// Token: 0x06005151 RID: 20817 RVA: 0x001A6D7F File Offset: 0x001A4F7F
		public CreditRecord_RoleTwoCols()
		{
		}

		// Token: 0x06005152 RID: 20818 RVA: 0x001A6F12 File Offset: 0x001A5112
		public CreditRecord_RoleTwoCols(string creditee1, string creditee2, string extra = null)
		{
			this.creditee1 = creditee1;
			this.creditee2 = creditee2;
			this.extra = extra;
		}

		// Token: 0x06005153 RID: 20819 RVA: 0x001A6F30 File Offset: 0x001A5130
		public override float DrawHeight(float width)
		{
			float a = Text.CalcHeight(this.creditee1, width * 0.5f);
			float b = Text.CalcHeight(this.creditee2, width * 0.5f);
			if (!this.compressed)
			{
				return 50f;
			}
			return Mathf.Max(a, b);
		}

		// Token: 0x06005154 RID: 20820 RVA: 0x001A6F78 File Offset: 0x001A5178
		public override void Draw(Rect rect)
		{
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect2 = rect;
			rect2.width = 0f;
			rect2.width = rect.width / 2f;
			Widgets.Label(rect2, this.creditee1);
			Rect rect3 = rect;
			rect3.xMin = rect2.xMax;
			Widgets.Label(rect3, this.creditee2);
			if (!this.extra.NullOrEmpty())
			{
				Rect rect4 = rect3;
				rect4.yMin += 28f;
				Text.Font = GameFont.Tiny;
				GUI.color = new Color(0.7f, 0.7f, 0.7f);
				Widgets.Label(rect4, this.extra);
				GUI.color = Color.white;
			}
		}

		// Token: 0x06005155 RID: 20821 RVA: 0x001A7032 File Offset: 0x001A5232
		public CreditRecord_RoleTwoCols Compress()
		{
			this.compressed = true;
			return this;
		}

		// Token: 0x04003AA3 RID: 15011
		public string creditee1;

		// Token: 0x04003AA4 RID: 15012
		public string creditee2;

		// Token: 0x04003AA5 RID: 15013
		public string extra;

		// Token: 0x04003AA6 RID: 15014
		public bool compressed;
	}
}
