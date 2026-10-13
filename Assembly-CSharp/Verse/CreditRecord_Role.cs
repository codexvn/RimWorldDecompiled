using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C6E RID: 3182
	public class CreditRecord_Role : CreditsEntry
	{
		// Token: 0x0600514C RID: 20812 RVA: 0x001A6D7F File Offset: 0x001A4F7F
		public CreditRecord_Role()
		{
		}

		// Token: 0x0600514D RID: 20813 RVA: 0x001A6DCA File Offset: 0x001A4FCA
		public CreditRecord_Role(string roleKey, string creditee, string extra = null)
		{
			this.roleKey = roleKey;
			this.creditee = creditee;
			this.extra = extra;
		}

		// Token: 0x0600514E RID: 20814 RVA: 0x001A6DE7 File Offset: 0x001A4FE7
		public override float DrawHeight(float width)
		{
			if (this.roleKey.NullOrEmpty())
			{
				width *= 0.5f;
			}
			if (!this.compressed)
			{
				return 50f;
			}
			return Text.CalcHeight(this.creditee, width * 0.5f);
		}

		// Token: 0x0600514F RID: 20815 RVA: 0x001A6E20 File Offset: 0x001A5020
		public override void Draw(Rect rect)
		{
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect2 = rect;
			rect2.width = 0f;
			if (!this.roleKey.NullOrEmpty())
			{
				rect2.width = rect.width / 2f;
				if (this.displayKey)
				{
					Widgets.Label(rect2, this.roleKey);
				}
			}
			Rect rect3 = rect;
			rect3.xMin = rect2.xMax;
			if (this.roleKey.NullOrEmpty())
			{
				Text.Anchor = TextAnchor.MiddleCenter;
			}
			Widgets.Label(rect3, this.creditee);
			Text.Anchor = TextAnchor.MiddleLeft;
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

		// Token: 0x06005150 RID: 20816 RVA: 0x001A6F08 File Offset: 0x001A5108
		public CreditRecord_Role Compress()
		{
			this.compressed = true;
			return this;
		}

		// Token: 0x04003A9E RID: 15006
		public string roleKey;

		// Token: 0x04003A9F RID: 15007
		public string creditee;

		// Token: 0x04003AA0 RID: 15008
		public string extra;

		// Token: 0x04003AA1 RID: 15009
		public bool displayKey;

		// Token: 0x04003AA2 RID: 15010
		public bool compressed;
	}
}
