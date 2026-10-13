using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C2A RID: 3114
	public class Dialog_Confirm : Window
	{
		// Token: 0x06004FE8 RID: 20456 RVA: 0x0019B0A7 File Offset: 0x001992A7
		public Dialog_Confirm(string title, Action onConfirm) : this(title, "Confirm".Translate(), onConfirm)
		{
		}

		// Token: 0x06004FE9 RID: 20457 RVA: 0x0019B0C0 File Offset: 0x001992C0
		public Dialog_Confirm(string title, string confirm, Action onConfirm) : base(null)
		{
			this.title = title;
			this.confirm = confirm;
			this.onConfirm = onConfirm;
			this.forcePause = true;
			this.closeOnAccept = false;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x17000C26 RID: 3110
		// (get) Token: 0x06004FEA RID: 20458 RVA: 0x0019B0FA File Offset: 0x001992FA
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(280f, 150f);
			}
		}

		// Token: 0x06004FEB RID: 20459 RVA: 0x0019B10C File Offset: 0x0019930C
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			bool flag = false;
			if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter))
			{
				flag = true;
				Event.current.Use();
			}
			Rect rect = inRect;
			rect.width = inRect.width / 2f - 5f;
			rect.yMin = inRect.yMax - Dialog_Confirm.ButtonSize.y - 10f;
			Rect rect2 = inRect;
			rect2.xMin = rect.xMax + 10f;
			rect2.yMin = inRect.yMax - Dialog_Confirm.ButtonSize.y - 10f;
			Rect rect3 = inRect;
			rect3.y += 4f;
			rect3.yMax = rect2.y - 10f;
			using (new TextBlock(TextAnchor.UpperCenter))
			{
				Widgets.Label(rect3, this.title);
			}
			if (Widgets.ButtonText(rect, "Cancel".Translate(), true, true, true, null))
			{
				Find.WindowStack.TryRemove(this, true);
			}
			if (Widgets.ButtonText(rect2, this.confirm, true, true, true, null) || flag)
			{
				Action action = this.onConfirm;
				if (action != null)
				{
					action();
				}
				Find.WindowStack.TryRemove(this, true);
			}
		}

		// Token: 0x04003978 RID: 14712
		private string title;

		// Token: 0x04003979 RID: 14713
		private string confirm;

		// Token: 0x0400397A RID: 14714
		private Action onConfirm;

		// Token: 0x0400397B RID: 14715
		private static readonly Vector2 ButtonSize = new Vector2(120f, 32f);
	}
}
