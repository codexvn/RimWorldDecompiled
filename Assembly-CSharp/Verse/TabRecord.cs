using System;
using LudeonTK;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C07 RID: 3079
	[StaticConstructorOnStartup]
	public class TabRecord
	{
		// Token: 0x17000C0C RID: 3084
		// (get) Token: 0x06004E8F RID: 20111 RVA: 0x0018FEC6 File Offset: 0x0018E0C6
		public bool Selected
		{
			get
			{
				if (this.selectedGetter == null)
				{
					return this.selected;
				}
				return this.selectedGetter();
			}
		}

		// Token: 0x17000C0D RID: 3085
		// (get) Token: 0x06004E90 RID: 20112 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string TutorTag
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06004E91 RID: 20113 RVA: 0x0018FEE2 File Offset: 0x0018E0E2
		public TabRecord(string label, Action clickedAction, bool selected)
		{
			this.label = label;
			this.clickedAction = clickedAction;
			this.selected = selected;
		}

		// Token: 0x06004E92 RID: 20114 RVA: 0x0018FF0A File Offset: 0x0018E10A
		public TabRecord(string label, Action clickedAction, Func<bool> selected)
		{
			this.label = label;
			this.clickedAction = clickedAction;
			this.selectedGetter = selected;
		}

		// Token: 0x06004E93 RID: 20115 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string GetTip()
		{
			return null;
		}

		// Token: 0x06004E94 RID: 20116 RVA: 0x0018FF34 File Offset: 0x0018E134
		public void Draw(Rect rect)
		{
			Rect drawRect = new Rect(rect);
			drawRect.width = 30f;
			Rect drawRect2 = new Rect(rect);
			drawRect2.width = 30f;
			drawRect2.x = rect.x + rect.width - 30f;
			Rect uvRect = new Rect(0.53125f, 0f, 0.46875f, 1f);
			Rect drawRect3 = new Rect(rect);
			drawRect3.x += drawRect.width;
			drawRect3.width -= 60f;
			drawRect3.xMin = UIScaling.AdjustCoordToUIScalingFloor(drawRect3.xMin);
			drawRect3.xMax = UIScaling.AdjustCoordToUIScalingCeil(drawRect3.xMax);
			Rect uvRect2 = new Rect(30f, 0f, 4f, (float)TabRecord.TabAtlas.height).ToUVRect(new Vector2((float)TabRecord.TabAtlas.width, (float)TabRecord.TabAtlas.height));
			Widgets.DrawTexturePart(drawRect, new Rect(0f, 0f, 0.46875f, 1f), TabRecord.TabAtlas);
			Widgets.DrawTexturePart(drawRect3, uvRect2, TabRecord.TabAtlas);
			Widgets.DrawTexturePart(drawRect2, uvRect, TabRecord.TabAtlas);
			GUI.color = (this.labelColor ?? Color.white);
			Rect rect2 = rect;
			rect2.width -= 10f;
			if (Mouse.IsOver(rect2))
			{
				GUI.color = Color.yellow;
				rect2.x += 2f;
				rect2.y -= 2f;
			}
			if (!this.TutorTag.NullOrEmpty())
			{
				UIHighlighter.HighlightOpportunity(rect2, this.TutorTag);
			}
			Text.WordWrap = false;
			Widgets.Label(rect, this.label);
			Text.WordWrap = true;
			GUI.color = Color.white;
			if (!this.Selected)
			{
				Rect drawRect4 = new Rect(rect);
				drawRect4.y += rect.height;
				drawRect4.y -= 1f;
				drawRect4.height = 1f;
				Rect uvRect3 = new Rect(0.5f, 0.01f, 0.01f, 0.01f);
				Widgets.DrawTexturePart(drawRect4, uvRect3, TabRecord.TabAtlas);
			}
		}

		// Token: 0x040037E1 RID: 14305
		public string label = "Tab";

		// Token: 0x040037E2 RID: 14306
		public Action clickedAction;

		// Token: 0x040037E3 RID: 14307
		public bool selected;

		// Token: 0x040037E4 RID: 14308
		public Func<bool> selectedGetter;

		// Token: 0x040037E5 RID: 14309
		public Color? labelColor;

		// Token: 0x040037E6 RID: 14310
		private const float TabEndWidth = 30f;

		// Token: 0x040037E7 RID: 14311
		private const float TabMiddleGraphicWidth = 4f;

		// Token: 0x040037E8 RID: 14312
		private static readonly Texture2D TabAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/TabAtlas", true);
	}
}
