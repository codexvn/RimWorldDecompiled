using System;
using System.Collections.Generic;
using LudeonTK;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000BBE RID: 3006
	[StaticConstructorOnStartup]
	public class Listing_Standard : Listing
	{
		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x06004CBB RID: 19643 RVA: 0x00186044 File Offset: 0x00184244
		public Rect? BoundingRectCached
		{
			get
			{
				if (this.boundingRectCachedForFrame != Time.frameCount)
				{
					if (this.boundingRect != null && this.boundingScrollPositionGetter != null)
					{
						Rect value = this.boundingRect.Value;
						Vector2 vector = this.boundingScrollPositionGetter();
						value.x += vector.x;
						value.y += vector.y;
						this.boundingRectCached = new Rect?(value);
					}
					this.boundingRectCachedForFrame = Time.frameCount;
				}
				return this.boundingRectCached;
			}
		}

		// Token: 0x06004CBC RID: 19644 RVA: 0x001860D0 File Offset: 0x001842D0
		public Listing_Standard(GameFont font)
		{
			this.font = font;
		}

		// Token: 0x06004CBD RID: 19645 RVA: 0x001860E6 File Offset: 0x001842E6
		public Listing_Standard()
		{
			this.font = GameFont.Small;
		}

		// Token: 0x06004CBE RID: 19646 RVA: 0x001860FC File Offset: 0x001842FC
		public Listing_Standard(Rect boundingRect, Func<Vector2> boundingScrollPositionGetter)
		{
			this.font = GameFont.Small;
			this.boundingRect = new Rect?(boundingRect);
			this.boundingScrollPositionGetter = boundingScrollPositionGetter;
		}

		// Token: 0x06004CBF RID: 19647 RVA: 0x00186125 File Offset: 0x00184325
		public override void Begin(Rect rect)
		{
			base.Begin(rect);
			Text.Font = this.font;
		}

		// Token: 0x06004CC0 RID: 19648 RVA: 0x0018613C File Offset: 0x0018433C
		public override void End()
		{
			base.End();
			if (this.labelScrollbarPositions != null)
			{
				for (int i = this.labelScrollbarPositions.Count - 1; i >= 0; i--)
				{
					if (!this.labelScrollbarPositionsSetThisFrame.Contains(this.labelScrollbarPositions[i].First))
					{
						this.labelScrollbarPositions.RemoveAt(i);
					}
				}
				this.labelScrollbarPositionsSetThisFrame.Clear();
			}
		}

		// Token: 0x06004CC1 RID: 19649 RVA: 0x001861A7 File Offset: 0x001843A7
		public Rect Label(TaggedString label, float maxHeight = -1f, string tooltip = null)
		{
			return this.Label(label.Resolve(), maxHeight, new TipSignal?(tooltip));
		}

		// Token: 0x06004CC2 RID: 19650 RVA: 0x001861C4 File Offset: 0x001843C4
		public Rect Label(string label, float maxHeight = -1f, TipSignal? tipSignal = null)
		{
			float num = Text.CalcHeight(label, base.ColumnWidth);
			bool flag = false;
			if (maxHeight >= 0f && num > maxHeight)
			{
				num = maxHeight;
				flag = true;
			}
			Rect rect = base.GetRect(num, 1f);
			if (this.BoundingRectCached != null && !rect.Overlaps(this.BoundingRectCached.Value))
			{
				return rect;
			}
			if (flag)
			{
				Vector2 labelScrollbarPosition = this.GetLabelScrollbarPosition(this.curX, this.curY);
				Widgets.LabelScrollable(rect, label, ref labelScrollbarPosition, false, true, false);
				this.SetLabelScrollbarPosition(this.curX, this.curY, labelScrollbarPosition);
			}
			else
			{
				Widgets.Label(rect, label);
			}
			if (tipSignal != null)
			{
				TooltipHandler.TipRegion(rect, tipSignal.Value);
			}
			base.Gap(this.verticalSpacing);
			return rect;
		}

		// Token: 0x06004CC3 RID: 19651 RVA: 0x0018628C File Offset: 0x0018448C
		public void LabelDouble(string leftLabel, string rightLabel, string tip = null)
		{
			float num = base.ColumnWidth / 2f;
			float width = base.ColumnWidth - num;
			float a = Text.CalcHeight(leftLabel, num);
			float b = Text.CalcHeight(rightLabel, width);
			float height = Mathf.Max(a, b);
			Rect rect = base.GetRect(height, 1f);
			if (this.BoundingRectCached != null && !rect.Overlaps(this.BoundingRectCached.Value))
			{
				return;
			}
			if (!tip.NullOrEmpty())
			{
				Widgets.DrawHighlightIfMouseover(rect);
				TooltipHandler.TipRegion(rect, tip);
			}
			Widgets.Label(rect.LeftHalf(), leftLabel);
			Widgets.Label(rect.RightHalf(), rightLabel);
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06004CC4 RID: 19652 RVA: 0x00186340 File Offset: 0x00184540
		public Rect SubLabel(string label, float widthPct)
		{
			float height = Text.CalcHeight(label, base.ColumnWidth * widthPct);
			Rect rect = base.GetRect(height, widthPct);
			float num = 20f;
			rect.x += num;
			rect.width -= num;
			Text.Font = GameFont.Tiny;
			GUI.color = Color.gray;
			Widgets.Label(rect, label);
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			base.Gap(this.verticalSpacing);
			return rect;
		}

		// Token: 0x06004CC5 RID: 19653 RVA: 0x001863BD File Offset: 0x001845BD
		public bool RadioButton(string label, bool active, float tabIn = 0f, string tooltip = null, float? tooltipDelay = null)
		{
			return this.RadioButton(label, active, tabIn, tooltip, tooltipDelay, false);
		}

		// Token: 0x06004CC6 RID: 19654 RVA: 0x001863CD File Offset: 0x001845CD
		public bool RadioButton(string label, bool active, float tabIn, string tooltip, float? tooltipDelay, bool disabled)
		{
			return this.RadioButton(label, active, tabIn, 0f, tooltip, tooltipDelay, disabled);
		}

		// Token: 0x06004CC7 RID: 19655 RVA: 0x001863E4 File Offset: 0x001845E4
		public bool RadioButton(string label, bool active, float tabIn, float tabInRight, string tooltip, float? tooltipDelay, bool disabled)
		{
			float lineHeight = Text.LineHeight;
			Rect rect = base.GetRect(lineHeight, 1f);
			rect.xMin += tabIn;
			rect.xMax -= tabInRight;
			if (this.BoundingRectCached != null && !rect.Overlaps(this.BoundingRectCached.Value))
			{
				return false;
			}
			if (!tooltip.NullOrEmpty())
			{
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
				}
				TipSignal tip = (tooltipDelay != null) ? new TipSignal(tooltip, tooltipDelay.Value) : new TipSignal(tooltip);
				TooltipHandler.TipRegion(rect, tip);
			}
			bool result = Widgets.RadioButtonLabeled(rect, label, active, disabled);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06004CC8 RID: 19656 RVA: 0x001864A4 File Offset: 0x001846A4
		public void CheckboxLabeled(string label, ref bool checkOn, float tabIn)
		{
			float height = Text.CalcHeight(label, base.ColumnWidth);
			Rect rect = base.GetRect(height, 1f);
			rect.xMin += tabIn;
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				Widgets.CheckboxLabeled(rect, label, ref checkOn, false, null, null, false, false);
				base.Gap(this.verticalSpacing);
			}
		}

		// Token: 0x06004CC9 RID: 19657 RVA: 0x0018651C File Offset: 0x0018471C
		public void CheckboxLabeled(string label, ref bool checkOn, string tooltip = null, float height = 0f, float labelPct = 1f)
		{
			float height2 = (height != 0f) ? height : Text.CalcHeight(label, base.ColumnWidth * labelPct);
			Rect rect = base.GetRect(height2, labelPct);
			rect.width = Math.Min(rect.width + 24f, base.ColumnWidth);
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				if (!tooltip.NullOrEmpty())
				{
					if (Mouse.IsOver(rect))
					{
						Widgets.DrawHighlight(rect);
					}
					TooltipHandler.TipRegion(rect, tooltip);
				}
				Widgets.CheckboxLabeled(rect, label, ref checkOn, false, null, null, false, false);
			}
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06004CCA RID: 19658 RVA: 0x001865D4 File Offset: 0x001847D4
		public bool CheckboxLabeledSelectable(string label, ref bool selected, ref bool checkOn)
		{
			float lineHeight = Text.LineHeight;
			Rect rect = base.GetRect(lineHeight, 1f);
			bool result = false;
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				result = Widgets.CheckboxLabeledSelectable(rect, label, ref selected, ref checkOn, null, 1f);
			}
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06004CCB RID: 19659 RVA: 0x0018663C File Offset: 0x0018483C
		public bool ButtonText(string label, string highlightTag = null, float widthPct = 1f)
		{
			Rect rect = base.GetRect(30f, widthPct);
			bool result = false;
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				result = Widgets.ButtonText(rect, label, true, true, true, null);
				if (highlightTag != null)
				{
					UIHighlighter.HighlightOpportunity(rect, highlightTag);
				}
			}
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06004CCC RID: 19660 RVA: 0x001866AA File Offset: 0x001848AA
		public bool ButtonTextLabeled(string label, string buttonLabel, TextAnchor anchor = TextAnchor.UpperLeft, string highlightTag = null, string tooltip = null)
		{
			return this.ButtonTextLabeledPct(label, buttonLabel, 0.5f, anchor, highlightTag, tooltip, null);
		}

		// Token: 0x06004CCD RID: 19661 RVA: 0x001866C0 File Offset: 0x001848C0
		public bool ButtonTextLabeledPct(string label, string buttonLabel, float labelPct, TextAnchor anchor = TextAnchor.UpperLeft, string highlightTag = null, string tooltip = null, Texture2D labelIcon = null)
		{
			float height = Math.Max(Text.CalcHeight(label, base.ColumnWidth * labelPct), 30f);
			Rect rect = base.GetRect(height, 1f);
			Rect rect2 = rect.RightPart(1f - labelPct);
			rect2.height = 30f;
			if (highlightTag != null)
			{
				UIHighlighter.HighlightOpportunity(rect, highlightTag);
			}
			bool result = false;
			Rect rect3 = rect.LeftPart(labelPct);
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				Text.Anchor = anchor;
				Widgets.Label(rect3, label);
				result = Widgets.ButtonText(rect2, buttonLabel.Truncate(rect2.width - 20f, null), true, true, true, null);
				Text.Anchor = TextAnchor.UpperLeft;
			}
			if (labelIcon != null)
			{
				GUI.DrawTexture(new Rect(Text.CalcSize(label).x + 10f, rect3.y + (rect3.height - Text.LineHeight) / 2f, Text.LineHeight, Text.LineHeight), labelIcon);
			}
			if (!tooltip.NullOrEmpty())
			{
				if (Mouse.IsOver(rect3))
				{
					Widgets.DrawHighlight(rect3);
				}
				TooltipHandler.TipRegion(rect3, tooltip);
			}
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06004CCE RID: 19662 RVA: 0x0018680C File Offset: 0x00184A0C
		public bool ButtonImage(Texture2D tex, float width, float height)
		{
			base.NewColumnIfNeeded(height);
			Rect butRect = new Rect(this.curX, this.curY, width, height);
			bool result = false;
			if (this.BoundingRectCached == null || butRect.Overlaps(this.BoundingRectCached.Value))
			{
				result = Widgets.ButtonImage(butRect, tex, true, null);
			}
			base.Gap(height + this.verticalSpacing);
			return result;
		}

		// Token: 0x06004CCF RID: 19663 RVA: 0x00186877 File Offset: 0x00184A77
		public void None()
		{
			GUI.color = Color.gray;
			Text.Anchor = TextAnchor.UpperCenter;
			this.Label("NoneBrackets".Translate(), -1f, null);
			GenUI.ResetLabelAlign();
			GUI.color = Color.white;
		}

		// Token: 0x06004CD0 RID: 19664 RVA: 0x001868B0 File Offset: 0x00184AB0
		public string TextEntry(string text, int lineCount = 1)
		{
			Rect rect = base.GetRect(Text.LineHeight * (float)lineCount, 1f);
			string result;
			if (lineCount == 1)
			{
				result = Widgets.TextField(rect, text);
			}
			else
			{
				result = Widgets.TextArea(rect, text, false);
			}
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06004CD1 RID: 19665 RVA: 0x001868F5 File Offset: 0x00184AF5
		public string TextEntryLabeled(string label, string text, int lineCount = 1)
		{
			string result = Widgets.TextEntryLabeled(base.GetRect(Text.LineHeight * (float)lineCount, 1f), label, text);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06004CD2 RID: 19666 RVA: 0x00186920 File Offset: 0x00184B20
		public void TextFieldNumeric<T>(ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			Rect rect = base.GetRect(Text.LineHeight, 1f);
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				Widgets.TextFieldNumeric<T>(rect, ref val, ref buffer, min, max);
			}
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06004CD3 RID: 19667 RVA: 0x0018697C File Offset: 0x00184B7C
		public void TextFieldNumericLabeled<T>(string label, ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			Rect rect = base.GetRect(Text.LineHeight, 1f);
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				Widgets.TextFieldNumericLabeled<T>(rect, label, ref val, ref buffer, min, max);
			}
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06004CD4 RID: 19668 RVA: 0x001869DC File Offset: 0x00184BDC
		public Rect IntRange(ref IntRange range, int min, int max)
		{
			Rect rect = base.GetRect(32f, 1f);
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				Widgets.IntRange(rect, (int)base.CurHeight, ref range, min, max, null, 0);
			}
			base.Gap(this.verticalSpacing);
			return rect;
		}

		// Token: 0x06004CD5 RID: 19669 RVA: 0x00186A40 File Offset: 0x00184C40
		public float Slider(float val, float min, float max)
		{
			float result = Widgets.HorizontalSlider(base.GetRect(22f, 1f), val, min, max, false, null, null, null, -1f);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06004CD6 RID: 19670 RVA: 0x00186A7C File Offset: 0x00184C7C
		public float SliderLabeled(string label, float val, float min, float max, float labelPct = 0.5f, string tooltip = null)
		{
			Rect rect = base.GetRect(30f, 1f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect.LeftPart(labelPct), label);
			if (tooltip != null)
			{
				TooltipHandler.TipRegion(rect.LeftPart(labelPct), tooltip);
			}
			Text.Anchor = TextAnchor.UpperLeft;
			float result = Widgets.HorizontalSlider(rect.RightPart(1f - labelPct), val, min, max, true, null, null, null, -1f);
			base.Gap(this.verticalSpacing);
			return result;
		}

		// Token: 0x06004CD7 RID: 19671 RVA: 0x00186AF8 File Offset: 0x00184CF8
		public void IntAdjuster(ref int val, int countChange, int min = 0)
		{
			Rect rect = base.GetRect(24f, 1f);
			rect.width = 42f;
			if (Widgets.ButtonText(rect, "-" + countChange.ToString(), true, true, true, null))
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				val -= countChange * GenUI.CurrentAdjustmentMultiplier();
				if (val < min)
				{
					val = min;
				}
			}
			rect.x += rect.width + 2f;
			if (Widgets.ButtonText(rect, "+" + countChange.ToString(), true, true, true, null))
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				val += countChange * GenUI.CurrentAdjustmentMultiplier();
				if (val < min)
				{
					val = min;
				}
			}
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06004CD8 RID: 19672 RVA: 0x00186BD4 File Offset: 0x00184DD4
		public void IntSetter(ref int val, int target, string label, float width = 42f)
		{
			if (Widgets.ButtonText(base.GetRect(24f, 1f), label, true, true, true, null))
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				val = target;
			}
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06004CD9 RID: 19673 RVA: 0x00186C20 File Offset: 0x00184E20
		public void IntEntry(ref int val, ref string editBuffer, int multiplier = 1, int min = 0)
		{
			Rect rect = base.GetRect(24f, 1f);
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				Widgets.IntEntry(rect, ref val, ref editBuffer, multiplier);
				if (val < min)
				{
					val = min;
					editBuffer = val.ToString();
				}
			}
			base.Gap(this.verticalSpacing);
		}

		// Token: 0x06004CDA RID: 19674 RVA: 0x00186C8C File Offset: 0x00184E8C
		public Listing_Standard BeginSection(float height, float sectionBorder = 4f, float bottomBorder = 4f)
		{
			Rect rect = base.GetRect(height + sectionBorder + bottomBorder, 1f);
			Widgets.DrawMenuSection(rect);
			Listing_Standard listing_Standard = new Listing_Standard();
			Rect rect2 = new Rect(rect.x + sectionBorder, rect.y + sectionBorder, rect.width - sectionBorder * 2f, rect.height - (sectionBorder + bottomBorder));
			listing_Standard.Begin(rect2);
			return listing_Standard;
		}

		// Token: 0x06004CDB RID: 19675 RVA: 0x00186CEF File Offset: 0x00184EEF
		public void EndSection(Listing_Standard listing)
		{
			listing.End();
		}

		// Token: 0x06004CDC RID: 19676 RVA: 0x00186CF8 File Offset: 0x00184EF8
		private Vector2 GetLabelScrollbarPosition(float x, float y)
		{
			if (this.labelScrollbarPositions == null)
			{
				return Vector2.zero;
			}
			for (int i = 0; i < this.labelScrollbarPositions.Count; i++)
			{
				Vector2 first = this.labelScrollbarPositions[i].First;
				if (first.x == x && first.y == y)
				{
					return this.labelScrollbarPositions[i].Second;
				}
			}
			return Vector2.zero;
		}

		// Token: 0x06004CDD RID: 19677 RVA: 0x00186D6C File Offset: 0x00184F6C
		private void SetLabelScrollbarPosition(float x, float y, Vector2 scrollbarPosition)
		{
			if (this.labelScrollbarPositions == null)
			{
				this.labelScrollbarPositions = new List<Pair<Vector2, Vector2>>();
				this.labelScrollbarPositionsSetThisFrame = new List<Vector2>();
			}
			this.labelScrollbarPositionsSetThisFrame.Add(new Vector2(x, y));
			for (int i = 0; i < this.labelScrollbarPositions.Count; i++)
			{
				Vector2 first = this.labelScrollbarPositions[i].First;
				if (first.x == x && first.y == y)
				{
					this.labelScrollbarPositions[i] = new Pair<Vector2, Vector2>(new Vector2(x, y), scrollbarPosition);
					return;
				}
			}
			this.labelScrollbarPositions.Add(new Pair<Vector2, Vector2>(new Vector2(x, y), scrollbarPosition));
		}

		// Token: 0x06004CDE RID: 19678 RVA: 0x00186E18 File Offset: 0x00185018
		public bool SelectableDef(string name, bool selected, Action deleteCallback)
		{
			Text.Font = GameFont.Tiny;
			float width = this.listingRect.width - 21f;
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect = new Rect(this.curX, this.curY, width, 21f);
			if (selected)
			{
				Widgets.DrawHighlight(rect);
			}
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawBox(rect, 1, null);
			}
			Text.WordWrap = false;
			Widgets.Label(rect, name);
			Text.WordWrap = true;
			if (deleteCallback != null && Widgets.ButtonImage(new Rect(rect.xMax, rect.y, 21f, 21f), TexButton.Delete, Color.white, GenUI.SubtleMouseoverColor, true, null))
			{
				deleteCallback();
			}
			Text.Anchor = TextAnchor.UpperLeft;
			this.curY += 21f;
			return Widgets.ButtonInvisible(rect, true);
		}

		// Token: 0x06004CDF RID: 19679 RVA: 0x00186EE8 File Offset: 0x001850E8
		public void LabelCheckboxDebug(string label, ref bool checkOn, bool highlight)
		{
			Text.Font = GameFont.Tiny;
			base.NewColumnIfNeeded(22f);
			Rect rect = new Rect(this.curX, this.curY, base.ColumnWidth, 22f);
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				Widgets.CheckboxLabeled(rect, label.Truncate(rect.width - 15f, null), ref checkOn, false, null, null, false, false);
				if (highlight)
				{
					GUI.color = Color.yellow;
					Widgets.DrawBox(rect, 2, null);
					GUI.color = Color.white;
				}
			}
			base.Gap(22f + this.verticalSpacing);
		}

		// Token: 0x06004CE0 RID: 19680 RVA: 0x00186F9C File Offset: 0x0018519C
		public bool ButtonDebug(string label, bool highlight)
		{
			Text.Font = GameFont.Tiny;
			base.NewColumnIfNeeded(22f);
			Rect rect = new Rect(this.curX, this.curY, base.ColumnWidth, 22f);
			bool result = false;
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				bool wordWrap = Text.WordWrap;
				Text.WordWrap = false;
				result = Widgets.ButtonText(rect, "  " + label, true, true, true, new TextAnchor?(TextAnchor.MiddleLeft));
				Text.WordWrap = wordWrap;
				if (highlight)
				{
					GUI.color = Color.yellow;
					Widgets.DrawBox(rect, 2, null);
					GUI.color = Color.white;
				}
			}
			base.Gap(22f + this.verticalSpacing);
			return result;
		}

		// Token: 0x06004CE1 RID: 19681 RVA: 0x00187060 File Offset: 0x00185260
		public DebugActionButtonResult ButtonDebugPinnable(string label, bool highlight, bool pinned)
		{
			Text.Font = GameFont.Tiny;
			base.NewColumnIfNeeded(22f);
			Rect rect = new Rect(this.curX, this.curY, base.ColumnWidth - 22f, 22f);
			DebugActionButtonResult result = DebugActionButtonResult.None;
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				bool wordWrap = Text.WordWrap;
				Text.WordWrap = false;
				if (Widgets.ButtonText(rect, "  " + label, true, true, true, new TextAnchor?(TextAnchor.MiddleLeft)))
				{
					result = DebugActionButtonResult.ButtonPressed;
				}
				Text.WordWrap = wordWrap;
				if (highlight)
				{
					GUI.color = Color.yellow;
					Widgets.DrawBox(rect, 2, null);
					GUI.color = Color.white;
				}
				Rect rect2 = new Rect(rect.xMax + 2f, rect.y, 22f, 22f).ContractedBy(4f);
				GUI.color = (pinned ? Color.white : new Color(1f, 1f, 1f, 0.2f));
				GUI.DrawTexture(rect2, pinned ? Listing_Standard.PinTex : Listing_Standard.PinOutlineTex);
				GUI.color = Color.white;
				if (Widgets.ButtonInvisible(rect2, true))
				{
					result = DebugActionButtonResult.PinPressed;
				}
				Widgets.DrawHighlightIfMouseover(rect2);
			}
			base.Gap(22f + this.verticalSpacing);
			return result;
		}

		// Token: 0x06004CE2 RID: 19682 RVA: 0x001871B4 File Offset: 0x001853B4
		public DebugActionButtonResult CheckboxPinnable(string label, ref bool checkOn, bool highlight, bool pinned)
		{
			Text.Font = GameFont.Tiny;
			base.NewColumnIfNeeded(22f);
			Rect rect = new Rect(this.curX, this.curY, base.ColumnWidth - 22f, 22f);
			DebugActionButtonResult result = DebugActionButtonResult.None;
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				Widgets.CheckboxLabeled(rect, label.Truncate(rect.width - 24f - 15f, null), ref checkOn, false, null, null, false, false);
				if (highlight)
				{
					GUI.color = Color.yellow;
					Widgets.DrawBox(rect, 2, null);
					GUI.color = Color.white;
				}
				if (Mouse.IsOver(rect))
				{
					TooltipHandler.TipRegion(rect, label);
				}
				Rect rect2 = new Rect(rect.xMax + 2f, rect.y, 22f, 22f).ContractedBy(4f);
				GUI.color = (pinned ? Color.white : new Color(1f, 1f, 1f, 0.2f));
				GUI.DrawTexture(rect2, pinned ? Listing_Standard.PinTex : Listing_Standard.PinOutlineTex);
				GUI.color = Color.white;
				if (Widgets.ButtonInvisible(rect2, true))
				{
					result = DebugActionButtonResult.PinPressed;
				}
				Widgets.DrawHighlightIfMouseover(rect2);
			}
			base.Gap(22f + this.verticalSpacing);
			return result;
		}

		// Token: 0x040036A8 RID: 13992
		private GameFont font;

		// Token: 0x040036A9 RID: 13993
		private Rect? boundingRect;

		// Token: 0x040036AA RID: 13994
		private Func<Vector2> boundingScrollPositionGetter;

		// Token: 0x040036AB RID: 13995
		private List<Pair<Vector2, Vector2>> labelScrollbarPositions;

		// Token: 0x040036AC RID: 13996
		private List<Vector2> labelScrollbarPositionsSetThisFrame;

		// Token: 0x040036AD RID: 13997
		private int boundingRectCachedForFrame = -1;

		// Token: 0x040036AE RID: 13998
		private Rect? boundingRectCached;

		// Token: 0x040036AF RID: 13999
		private static readonly Texture2D PinTex = ContentFinder<Texture2D>.Get("UI/Icons/Pin", true);

		// Token: 0x040036B0 RID: 14000
		private static readonly Texture2D PinOutlineTex = ContentFinder<Texture2D>.Get("UI/Icons/Pin-Outline", true);

		// Token: 0x040036B1 RID: 14001
		public const float PinnableActionHeight = 22f;

		// Token: 0x040036B2 RID: 14002
		private const float DefSelectionLineHeight = 21f;
	}
}
