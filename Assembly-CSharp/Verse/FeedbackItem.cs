using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B7A RID: 2938
	public abstract class FeedbackItem
	{
		// Token: 0x06004B4F RID: 19279 RVA: 0x0017FBC0 File Offset: 0x0017DDC0
		public FeedbackItem(Vector2 ScreenPos)
		{
			this.uniqueID = FeedbackItem.freeUniqueID++;
			this.CurScreenPos = ScreenPos;
			this.CurScreenPos.y = this.CurScreenPos.y - 15f;
		}

		// Token: 0x06004B50 RID: 19280 RVA: 0x0017FC21 File Offset: 0x0017DE21
		public void Update()
		{
			this.TimeLeft -= Time.deltaTime;
			this.CurScreenPos += this.FloatPerSecond * Time.deltaTime;
		}

		// Token: 0x06004B51 RID: 19281
		public abstract void FeedbackOnGUI();

		// Token: 0x06004B52 RID: 19282 RVA: 0x0017FC58 File Offset: 0x0017DE58
		protected void DrawFloatingText(string str, Color TextColor)
		{
			float x = Text.CalcSize(str).x;
			Rect wordRect = new Rect(this.CurScreenPos.x - x / 2f, this.CurScreenPos.y, x, 20f);
			Find.WindowStack.ImmediateWindow(5983 * this.uniqueID + 495, wordRect, WindowLayer.Super, delegate
			{
				Rect rect = wordRect.AtZero();
				Text.Anchor = TextAnchor.UpperCenter;
				Text.Font = GameFont.Small;
				GUI.DrawTexture(rect, TexUI.GrayTextBG);
				GUI.color = TextColor;
				Widgets.Label(rect, str);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
			}, false, false, 1f, null, false);
		}

		// Token: 0x040035A0 RID: 13728
		protected Vector2 FloatPerSecond = new Vector2(20f, -20f);

		// Token: 0x040035A1 RID: 13729
		private int uniqueID;

		// Token: 0x040035A2 RID: 13730
		public float TimeLeft = 2f;

		// Token: 0x040035A3 RID: 13731
		protected Vector2 CurScreenPos;

		// Token: 0x040035A4 RID: 13732
		private static int freeUniqueID;
	}
}
