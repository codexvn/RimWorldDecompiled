using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C70 RID: 3184
	public class CreditRecord_Title : CreditsEntry
	{
		// Token: 0x06005156 RID: 20822 RVA: 0x001A6D7F File Offset: 0x001A4F7F
		public CreditRecord_Title()
		{
		}

		// Token: 0x06005157 RID: 20823 RVA: 0x001A703C File Offset: 0x001A523C
		public CreditRecord_Title(string title)
		{
			this.title = title;
		}

		// Token: 0x06005158 RID: 20824 RVA: 0x001A704B File Offset: 0x001A524B
		public override float DrawHeight(float width)
		{
			return 100f;
		}

		// Token: 0x06005159 RID: 20825 RVA: 0x001A7054 File Offset: 0x001A5254
		public override void Draw(Rect rect)
		{
			rect.yMin += 31f;
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, this.title);
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			Widgets.DrawLineHorizontal(rect.x + 10f, Mathf.Round(rect.yMax) - 14f, rect.width - 20f);
			GUI.color = Color.white;
		}

		// Token: 0x04003AA7 RID: 15015
		public string title;
	}
}
