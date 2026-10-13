using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C6D RID: 3181
	public class CreditRecord_Text : CreditsEntry
	{
		// Token: 0x06005148 RID: 20808 RVA: 0x001A6D7F File Offset: 0x001A4F7F
		public CreditRecord_Text()
		{
		}

		// Token: 0x06005149 RID: 20809 RVA: 0x001A6D87 File Offset: 0x001A4F87
		public CreditRecord_Text(string text, TextAnchor anchor = TextAnchor.UpperLeft)
		{
			this.text = text;
			this.anchor = anchor;
		}

		// Token: 0x0600514A RID: 20810 RVA: 0x001A6D9D File Offset: 0x001A4F9D
		public override float DrawHeight(float width)
		{
			return Text.CalcHeight(this.text, width);
		}

		// Token: 0x0600514B RID: 20811 RVA: 0x001A6DAB File Offset: 0x001A4FAB
		public override void Draw(Rect r)
		{
			Text.Anchor = this.anchor;
			Widgets.Label(r, this.text);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x04003A9C RID: 15004
		public string text;

		// Token: 0x04003A9D RID: 15005
		public TextAnchor anchor;
	}
}
