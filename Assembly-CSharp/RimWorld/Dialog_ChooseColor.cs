using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003131 RID: 12593
	public class Dialog_ChooseColor : Window
	{
		// Token: 0x17002E08 RID: 11784
		// (get) Token: 0x06011B41 RID: 72513 RVA: 0x005185CB File Offset: 0x005167CB
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 410f);
			}
		}

		// Token: 0x06011B42 RID: 72514 RVA: 0x005185DC File Offset: 0x005167DC
		public Dialog_ChooseColor(string header, Color selectedColor, List<Color> colors, Action<Color> onApply) : base(null)
		{
			this.header = header;
			this.selectedColor = selectedColor;
			this.onApply = onApply;
			this.colors = colors;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06011B43 RID: 72515 RVA: 0x00518610 File Offset: 0x00516810
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(inRect.x, inRect.y, inRect.width, 35f), this.header);
			Text.Font = GameFont.Small;
			Widgets.ColorSelectorIcon(new Rect(inRect.x, inRect.y + 35f + 10f, 88f, 88f).ContractedBy(2f), null, this.selectedColor, true);
			Rect rect = inRect;
			rect.xMin += 105f;
			rect.yMin += 45f;
			rect.height -= Window.CloseButSize.y + 10f;
			float num = (float)this.colors.Count / (rect.width / 24f);
			Rect viewRect = rect;
			viewRect.width -= 16f;
			viewRect.height = num * 24f;
			Widgets.BeginScrollView(rect, ref this.scrollPosition, viewRect, true);
			float num2;
			Widgets.ColorSelector(rect, ref this.selectedColor, this.colors, out num2, null, 22, 2, null);
			Widgets.EndScrollView();
			Text.Font = GameFont.Small;
			if (Widgets.ButtonText(new Rect(inRect.x, inRect.height - Window.CloseButSize.y, Window.CloseButSize.x, Window.CloseButSize.y), "CloseButton".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(inRect.width - Window.CloseButSize.x, inRect.height - Window.CloseButSize.y, Window.CloseButSize.x, Window.CloseButSize.y), "OK".Translate(), true, true, true, null))
			{
				Action<Color> action = this.onApply;
				if (action != null)
				{
					action(this.selectedColor);
				}
				this.Close(true);
			}
		}

		// Token: 0x0400B659 RID: 46681
		private string header;

		// Token: 0x0400B65A RID: 46682
		private Color selectedColor;

		// Token: 0x0400B65B RID: 46683
		private Action<Color> onApply;

		// Token: 0x0400B65C RID: 46684
		private List<Color> colors;

		// Token: 0x0400B65D RID: 46685
		private Vector2 scrollPosition;

		// Token: 0x0400B65E RID: 46686
		private const float HeaderHeight = 35f;

		// Token: 0x0400B65F RID: 46687
		private const int ColorSize = 22;

		// Token: 0x0400B660 RID: 46688
		private const int ColorPadding = 2;
	}
}
