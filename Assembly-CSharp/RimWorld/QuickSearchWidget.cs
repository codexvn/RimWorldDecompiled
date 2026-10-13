using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02003491 RID: 13457
	public class QuickSearchWidget
	{
		// Token: 0x06012ED5 RID: 77525 RVA: 0x0059A988 File Offset: 0x00598B88
		public QuickSearchWidget()
		{
			this.controlName = string.Format("QuickSearchWidget_{0}", QuickSearchWidget.instanceCounter++);
		}

		// Token: 0x06012ED6 RID: 77526 RVA: 0x0059A9DC File Offset: 0x00598BDC
		public void OnGUI(Rect rect, Action onFilterChange = null, Action onClear = null)
		{
			if (this.CurrentlyFocused() && Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
			{
				this.Unfocus();
				Event.current.Use();
			}
			if (OriginalEventUtility.EventType == EventType.MouseDown && !rect.Contains(Event.current.mousePosition))
			{
				this.Unfocus();
			}
			Color color = GUI.color;
			GUI.color = Color.white;
			float num = Mathf.Min(18f, rect.height);
			float num2 = num + 8f;
			float y = rect.y + (rect.height - num2) / 2f + 4f;
			Rect position = new Rect(rect.x + 4f, y, num, num);
			GUI.DrawTexture(position, TexButton.Search);
			GUI.SetNextControlName(this.controlName);
			Rect rect2 = new Rect(position.xMax + 4f, rect.y, rect.width - num2, rect.height);
			if (this.filter.Active)
			{
				rect2.xMax -= num2;
			}
			using (new TextBlock(GameFont.Small))
			{
				if (this.noResultsMatched && this.filter.Active)
				{
					GUI.color = ColorLibrary.RedReadable;
				}
				else if (!this.filter.Active && !this.CurrentlyFocused())
				{
					GUI.color = this.inactiveTextColor;
				}
				string text = Widgets.TextField(rect2, this.filter.Text, this.maxSearchTextLength, null);
				GUI.color = Color.white;
				if (text != this.filter.Text)
				{
					this.filter.Text = text;
					if (onFilterChange != null)
					{
						onFilterChange();
					}
				}
			}
			if (this.filter.Active && Widgets.ButtonImage(new Rect(rect2.xMax + 4f, y, num, num), TexButton.CloseXSmall, true, null))
			{
				this.filter.Text = "";
				SoundDefOf.CancelMode.PlayOneShotOnCamera(null);
				if (onFilterChange != null)
				{
					onFilterChange();
				}
				if (onClear != null)
				{
					onClear();
				}
			}
			GUI.color = color;
		}

		// Token: 0x06012ED7 RID: 77527 RVA: 0x0059AC18 File Offset: 0x00598E18
		public void Unfocus()
		{
			if (this.CurrentlyFocused())
			{
				UI.UnfocusCurrentControl();
			}
		}

		// Token: 0x06012ED8 RID: 77528 RVA: 0x0059AC27 File Offset: 0x00598E27
		public void Focus()
		{
			GUI.FocusControl(this.controlName);
		}

		// Token: 0x06012ED9 RID: 77529 RVA: 0x0059AC34 File Offset: 0x00598E34
		public bool CurrentlyFocused()
		{
			return GUI.GetNameOfFocusedControl() == this.controlName;
		}

		// Token: 0x06012EDA RID: 77530 RVA: 0x0059AC46 File Offset: 0x00598E46
		public void Reset()
		{
			this.filter.Text = "";
			this.noResultsMatched = false;
		}

		// Token: 0x0400C405 RID: 50181
		public QuickSearchFilter filter = new QuickSearchFilter();

		// Token: 0x0400C406 RID: 50182
		public bool noResultsMatched;

		// Token: 0x0400C407 RID: 50183
		public Color inactiveTextColor = Color.white;

		// Token: 0x0400C408 RID: 50184
		public int maxSearchTextLength = 30;

		// Token: 0x0400C409 RID: 50185
		private readonly string controlName;

		// Token: 0x0400C40A RID: 50186
		public const float WidgetHeight = 24f;

		// Token: 0x0400C40B RID: 50187
		public const float IconSize = 18f;

		// Token: 0x0400C40C RID: 50188
		public const float IconMargin = 4f;

		// Token: 0x0400C40D RID: 50189
		private const int BaseMaxSearchTextLength = 30;

		// Token: 0x0400C40E RID: 50190
		private static int instanceCounter;
	}
}
