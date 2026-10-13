using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C40 RID: 3136
	public class Dialog_Slider : Window
	{
		// Token: 0x17000C30 RID: 3120
		// (get) Token: 0x0600502E RID: 20526 RVA: 0x0019D0D0 File Offset: 0x0019B2D0
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(300f, 130f + this.extraBottomSpace);
			}
		}

		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x0600502F RID: 20527 RVA: 0x0019D0E8 File Offset: 0x0019B2E8
		protected override float Margin
		{
			get
			{
				return 10f;
			}
		}

		// Token: 0x06005030 RID: 20528 RVA: 0x0019D0F0 File Offset: 0x0019B2F0
		public Dialog_Slider(Func<int, string> textGetter, int from, int to, Action<int> confirmAction, int startingValue = -2147483648, float roundTo = 1f) : base(null)
		{
			this.textGetter = textGetter;
			this.from = from;
			this.to = to;
			this.confirmAction = confirmAction;
			this.roundTo = roundTo;
			this.forcePause = true;
			this.closeOnClickedOutside = true;
			if (startingValue == -2147483648)
			{
				this.curValue = from;
				return;
			}
			this.curValue = startingValue;
		}

		// Token: 0x06005031 RID: 20529 RVA: 0x0019D15C File Offset: 0x0019B35C
		public Dialog_Slider(string text, int from, int to, Action<int> confirmAction, int startingValue = -2147483648, float roundTo = 1f) : this((int val) => string.Format(text, val), from, to, confirmAction, startingValue, roundTo)
		{
		}

		// Token: 0x06005032 RID: 20530 RVA: 0x0019D190 File Offset: 0x0019B390
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			string text = this.textGetter(this.curValue);
			float height = Text.CalcHeight(text, inRect.width);
			Rect rect = new Rect(inRect.x, inRect.y, inRect.width, height);
			Text.Anchor = TextAnchor.UpperCenter;
			Widgets.Label(rect, text);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect2 = new Rect(inRect.x, inRect.y + rect.height + 10f, inRect.width, 30f);
			this.curValue = (int)Widgets.HorizontalSlider(rect2, (float)this.curValue, (float)this.from, (float)this.to, true, null, null, null, this.roundTo);
			GUI.color = ColoredText.SubtleGrayColor;
			Text.Font = GameFont.Tiny;
			Widgets.Label(new Rect(inRect.x, rect2.yMax - 10f, inRect.width / 2f, Text.LineHeight), this.from.ToString());
			Text.Anchor = TextAnchor.UpperRight;
			Widgets.Label(new Rect(inRect.x + inRect.width / 2f, rect2.yMax - 10f, inRect.width / 2f, Text.LineHeight), this.to.ToString());
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			float num = (inRect.width - 10f) / 2f;
			if (Widgets.ButtonText(new Rect(inRect.x, inRect.yMax - 30f, num, 30f), "CancelButton".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(inRect.x + num + 10f, inRect.yMax - 30f, num, 30f), "OK".Translate(), true, true, true, null))
			{
				this.Close(true);
				this.confirmAction(this.curValue);
			}
		}

		// Token: 0x040039C5 RID: 14789
		public Func<int, string> textGetter;

		// Token: 0x040039C6 RID: 14790
		public int from;

		// Token: 0x040039C7 RID: 14791
		public int to;

		// Token: 0x040039C8 RID: 14792
		public float roundTo = 1f;

		// Token: 0x040039C9 RID: 14793
		public float extraBottomSpace;

		// Token: 0x040039CA RID: 14794
		private Action<int> confirmAction;

		// Token: 0x040039CB RID: 14795
		private int curValue;

		// Token: 0x040039CC RID: 14796
		private const float BotAreaHeight = 30f;

		// Token: 0x040039CD RID: 14797
		private const float NumberYOffset = 10f;
	}
}
