using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200313F RID: 12607
	public abstract class Dialog_ColorPickerBase : Window
	{
		// Token: 0x17002E19 RID: 11801
		// (get) Token: 0x06011BA6 RID: 72614 RVA: 0x0051D087 File Offset: 0x0051B287
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(600f, 450f);
			}
		}

		// Token: 0x17002E1A RID: 11802
		// (get) Token: 0x06011BA7 RID: 72615
		protected abstract bool ShowDarklight { get; }

		// Token: 0x17002E1B RID: 11803
		// (get) Token: 0x06011BA8 RID: 72616
		protected abstract Color DefaultColor { get; }

		// Token: 0x17002E1C RID: 11804
		// (get) Token: 0x06011BA9 RID: 72617
		protected abstract List<Color> PickableColors { get; }

		// Token: 0x17002E1D RID: 11805
		// (get) Token: 0x06011BAA RID: 72618
		protected abstract float ForcedColorValue { get; }

		// Token: 0x17002E1E RID: 11806
		// (get) Token: 0x06011BAB RID: 72619
		protected abstract bool ShowColorTemperatureBar { get; }

		// Token: 0x06011BAC RID: 72620
		protected abstract void SaveColor(Color color);

		// Token: 0x06011BAD RID: 72621 RVA: 0x0051D098 File Offset: 0x0051B298
		public Dialog_ColorPickerBase(Widgets.ColorComponents visibleTextfields, Widgets.ColorComponents editableTextfields) : base(null)
		{
			this.visibleTextfields = visibleTextfields;
			this.editableTextfields = editableTextfields;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.closeOnClickedOutside = true;
			this.closeOnAccept = false;
		}

		// Token: 0x06011BAE RID: 72622 RVA: 0x0051D0D8 File Offset: 0x0051B2D8
		private static void HeaderRow(ref RectDivider layout)
		{
			using (new TextBlock(GameFont.Medium))
			{
				TaggedString taggedString = "ChooseAColor".Translate().CapitalizeFirst();
				RectDivider rectDivider = layout.NewRow(Text.CalcHeight(taggedString, layout.Rect.width), VerticalJustification.Top, null);
				GUI.SetNextControlName(Dialog_ColorPickerBase.focusableControlNames[0]);
				Rect rect = rectDivider.Rect;
				rect.y -= 5f;
				Widgets.Label(rect, taggedString);
			}
		}

		// Token: 0x06011BAF RID: 72623 RVA: 0x0051D180 File Offset: 0x0051B380
		private void BottomButtons(ref RectDivider layout)
		{
			RectDivider rectDivider = layout.NewRow(Dialog_ColorPickerBase.ButSize.y, VerticalJustification.Bottom, null);
			if (Widgets.ButtonText(rectDivider.NewCol(Dialog_ColorPickerBase.ButSize.x, HorizontalJustification.Left, null), "Cancel".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(rectDivider.NewCol(Dialog_ColorPickerBase.ButSize.x, HorizontalJustification.Right, null), "Accept".Translate(), true, true, true, null))
			{
				this.SaveColor(this.color);
				this.Close(true);
			}
		}

		// Token: 0x06011BB0 RID: 72624 RVA: 0x0051D248 File Offset: 0x0051B448
		private void ColorPalette(ref RectDivider layout, ref Color color, Color defaultColor, bool showDarklight, out float paletteHeight)
		{
			using (new TextBlock(TextAnchor.MiddleLeft))
			{
				RectDivider rectDivider = layout;
				RectDivider rect = rectDivider.NewCol(250f, HorizontalJustification.Right, null);
				int num = 26;
				RectDivider rectDivider2 = rect.NewRow((float)num, VerticalJustification.Top, null);
				int num2 = 4;
				Rect parent;
				Rect parent2;
				rectDivider2.Rect.SplitVertically((float)(num2 * (num + 2)), out parent, out parent2);
				RectDivider rect2 = new RectDivider(parent, 195906069, new Vector2?(new Vector2(10f, 2f)));
				Widgets.ColorBox(rect2.NewCol((float)num, HorizontalJustification.Left, null), ref color, defaultColor, 22, 2, null);
				Widgets.Label(rect2, "Default".Translate().CapitalizeFirst());
				RectDivider rect3 = new RectDivider(parent2, 195906069, new Vector2?(new Vector2(10f, 2f)));
				Color defaultDarklight = DarklightUtility.DefaultDarklight;
				Rect rect4 = rect3.NewCol((float)num, HorizontalJustification.Left, null);
				if (showDarklight)
				{
					Widgets.ColorBox(rect4, ref color, defaultDarklight, 22, 2, null);
					Widgets.Label(rect3, "Darklight".Translate().CapitalizeFirst());
				}
				Widgets.ColorSelector(rect, ref color, this.PickableColors, out paletteHeight, null, 22, 2, null);
				paletteHeight += (float)(num + 2);
			}
		}

		// Token: 0x06011BB1 RID: 72625 RVA: 0x0051D3DC File Offset: 0x0051B5DC
		private void ColorTextfields(ref RectDivider layout, out Vector2 size)
		{
			RectAggregator rectAggregator = new RectAggregator(new Rect(layout.Rect.position, new Vector2(125f, 0f)), 195906069, null);
			bool flag = Widgets.ColorTextfields(ref rectAggregator, ref this.color, ref this.textfieldBuffers, ref this.textfieldColorBuffer, this.previousFocusedControlName, "colorTextfields", this.editableTextfields, this.visibleTextfields);
			size = rectAggregator.Rect.size;
			if (flag)
			{
				float h;
				float s;
				float num;
				Color.RGBToHSV(this.color, out h, out s, out num);
				this.color = Color.HSVToRGB(h, s, this.ForcedColorValue);
			}
		}

		// Token: 0x06011BB2 RID: 72626 RVA: 0x0051D48C File Offset: 0x0051B68C
		private static void ColorReadback(Rect rect, Color color, Color oldColor, bool showDarklight)
		{
			Rect parent;
			Rect parent2;
			rect.SplitVertically((rect.width - 26f) / 2f, out parent, out parent2);
			RectDivider rectDivider = new RectDivider(parent, 195906069, null);
			TaggedString label = "CurrentColor".Translate().CapitalizeFirst();
			TaggedString label2 = "OldColor".Translate().CapitalizeFirst();
			float width = Mathf.Max(new float[]
			{
				100f,
				label.GetWidthCached(),
				label2.GetWidthCached()
			});
			RectDivider rect2 = rectDivider.NewRow(Text.LineHeight, VerticalJustification.Top, null);
			Widgets.Label(rect2.NewCol(width, HorizontalJustification.Left, null), label);
			Widgets.DrawBoxSolid(rect2, color);
			RectDivider rect3 = rectDivider.NewRow(Text.LineHeight, VerticalJustification.Top, null);
			Widgets.Label(rect3.NewCol(width, HorizontalJustification.Left, null), label2);
			Widgets.DrawBoxSolid(rect3, oldColor);
			RectDivider rect4 = new RectDivider(parent2, 195906069, null);
			rect4.NewCol(26f, HorizontalJustification.Left, null);
			if (showDarklight)
			{
				if (DarklightUtility.IsDarklight(color))
				{
					Widgets.Label(rect4, "Darklight".Translate().CapitalizeFirst());
					return;
				}
				Widgets.Label(rect4, "NotDarklight".Translate().CapitalizeFirst());
			}
		}

		// Token: 0x06011BB3 RID: 72627 RVA: 0x0051D624 File Offset: 0x0051B824
		private static void TabControl()
		{
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Tab)
			{
				bool flag = !Event.current.shift;
				Event.current.Use();
				string text = GUI.GetNameOfFocusedControl();
				if (text.NullOrEmpty())
				{
					text = Dialog_ColorPickerBase.focusableControlNames[0];
				}
				int num = Dialog_ColorPickerBase.focusableControlNames.IndexOf(text);
				if (num < 0)
				{
					num = Dialog_ColorPickerBase.focusableControlNames.Count;
				}
				if (flag)
				{
					num++;
				}
				else
				{
					num--;
				}
				if (num >= Dialog_ColorPickerBase.focusableControlNames.Count)
				{
					num = 0;
				}
				else if (num < 0)
				{
					num = Dialog_ColorPickerBase.focusableControlNames.Count - 1;
				}
				GUI.FocusControl(Dialog_ColorPickerBase.focusableControlNames[num]);
			}
		}

		// Token: 0x06011BB4 RID: 72628 RVA: 0x0051D6DC File Offset: 0x0051B8DC
		public override void DoWindowContents(Rect inRect)
		{
			using (TextBlock.Default())
			{
				RectDivider rect = new RectDivider(inRect, 195906069, null);
				Dialog_ColorPickerBase.HeaderRow(ref rect);
				rect.NewRow(0f, VerticalJustification.Top, null);
				this.BottomButtons(ref rect);
				rect.NewRow(0f, VerticalJustification.Bottom, null);
				float h;
				float s;
				float num;
				Color.RGBToHSV(this.DefaultColor, out h, out s, out num);
				Color defaultColor = Color.HSVToRGB(h, s, this.ForcedColorValue);
				defaultColor.a = 1f;
				float num2;
				this.ColorPalette(ref rect, ref this.color, defaultColor, this.ShowDarklight, out num2);
				Vector2 vector;
				this.ColorTextfields(ref rect, out vector);
				float height = Mathf.Max(new float[]
				{
					num2,
					128f,
					vector.y
				});
				RectDivider rectDivider = rect.NewRow(height, VerticalJustification.Top, null);
				rectDivider.NewCol(vector.x, HorizontalJustification.Left, null);
				rectDivider.NewCol(250f, HorizontalJustification.Right, null);
				Widgets.HSVColorWheel(rectDivider.Rect.ContractedBy((rectDivider.Rect.width - 128f) / 2f, (rectDivider.Rect.height - 128f) / 2f), ref this.color, ref this.hsvColorWheelDragging, new float?(this.ForcedColorValue), null);
				rect.NewRow(10f, VerticalJustification.Top, null);
				Rect rect2 = rect.NewRow(34f, VerticalJustification.Top, null);
				if (this.ShowColorTemperatureBar)
				{
					Widgets.ColorTemperatureBar(rect2, ref this.color, ref this.colorTemperatureDragging, new float?(this.ForcedColorValue));
				}
				rect.NewRow(26f, VerticalJustification.Top, null);
				Dialog_ColorPickerBase.ColorReadback(rect, this.color, this.oldColor, this.ShowDarklight);
				Dialog_ColorPickerBase.TabControl();
				if (Event.current.type == EventType.Layout)
				{
					this.previousFocusedControlName = GUI.GetNameOfFocusedControl();
				}
			}
		}

		// Token: 0x0400B6DD RID: 46813
		private const int ContextHash = 195906069;

		// Token: 0x0400B6DE RID: 46814
		private static readonly List<string> focusableControlNames = new List<string>
		{
			"title",
			"colorTextfields_0",
			"colorTextfields_1",
			"colorTextfields_2",
			"colorTextfields_3",
			"colorTextfields_4"
		};

		// Token: 0x0400B6DF RID: 46815
		private const int ColorWheelSize = 128;

		// Token: 0x0400B6E0 RID: 46816
		private const int ColorTextfieldsWidth = 125;

		// Token: 0x0400B6E1 RID: 46817
		private const int PaletteColumns = 9;

		// Token: 0x0400B6E2 RID: 46818
		private const int ColorIconSize = 22;

		// Token: 0x0400B6E3 RID: 46819
		private const int ColorIconPadding = 2;

		// Token: 0x0400B6E4 RID: 46820
		private const int CurrentColorLabelWidth = 100;

		// Token: 0x0400B6E5 RID: 46821
		private const int PaletteWidth = 250;

		// Token: 0x0400B6E6 RID: 46822
		private const int ColorTemperatureBarHeight = 34;

		// Token: 0x0400B6E7 RID: 46823
		protected Color color;

		// Token: 0x0400B6E8 RID: 46824
		protected Color oldColor;

		// Token: 0x0400B6E9 RID: 46825
		private bool hsvColorWheelDragging;

		// Token: 0x0400B6EA RID: 46826
		private bool colorTemperatureDragging;

		// Token: 0x0400B6EB RID: 46827
		private string[] textfieldBuffers = new string[6];

		// Token: 0x0400B6EC RID: 46828
		private Color textfieldColorBuffer;

		// Token: 0x0400B6ED RID: 46829
		private string previousFocusedControlName;

		// Token: 0x0400B6EE RID: 46830
		private Widgets.ColorComponents visibleTextfields;

		// Token: 0x0400B6EF RID: 46831
		private Widgets.ColorComponents editableTextfields;

		// Token: 0x0400B6F0 RID: 46832
		protected static readonly Vector2 ButSize = new Vector2(150f, 38f);
	}
}
