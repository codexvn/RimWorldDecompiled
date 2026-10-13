using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000085 RID: 133
	public abstract class Dialog_DebugOptionLister : Dialog_OptionLister
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x0000F9E0 File Offset: 0x0000DBE0
		protected virtual int HighlightedIndex
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000F9E3 File Offset: 0x0000DBE3
		public Dialog_DebugOptionLister()
		{
			this.forcePause = true;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000F9F2 File Offset: 0x0000DBF2
		public void NewColumn(float columnWidth)
		{
			this.curY = 0f;
			this.curX += columnWidth + 17f;
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000FA13 File Offset: 0x0000DC13
		protected void NewColumnIfNeeded(float columnWidth, float neededHeight)
		{
			if (this.curY + neededHeight > this.windowRect.height)
			{
				this.NewColumn(columnWidth);
			}
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000FA34 File Offset: 0x0000DC34
		public bool ButtonDebug(string label, float columnWidth, bool highlight)
		{
			Text.Font = GameFont.Tiny;
			this.NewColumnIfNeeded(columnWidth, 22f);
			Rect rect = new Rect(this.curX, this.curY, columnWidth, 22f);
			bool result = false;
			if (base.BoundingRectCached == null || rect.Overlaps(base.BoundingRectCached.Value))
			{
				bool wordWrap = Text.WordWrap;
				Text.WordWrap = false;
				result = DevGUI.ButtonText(rect, "  " + label, new TextAnchor?(TextAnchor.MiddleLeft));
				Text.WordWrap = wordWrap;
				if (highlight)
				{
					GUI.color = Color.yellow;
					DevGUI.DrawBox(rect, 2, null);
					GUI.color = Color.white;
				}
			}
			this.curY += 22f + this.verticalSpacing;
			return result;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000FAF8 File Offset: 0x0000DCF8
		protected void DebugLabel(string label, float columnWidth)
		{
			Text.Font = GameFont.Small;
			float num = Text.CalcHeight(label, columnWidth);
			this.NewColumnIfNeeded(columnWidth, num);
			DevGUI.Label(new Rect(this.curX, this.curY, columnWidth, num), label);
			this.curY += num + this.verticalSpacing;
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight += 24f;
			}
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000FB68 File Offset: 0x0000DD68
		protected bool DebugAction(string label, float columnWidth, Action action, bool highlight)
		{
			bool result = false;
			if (!base.FilterAllows(label))
			{
				return false;
			}
			if (this.ButtonDebug(label, columnWidth, highlight))
			{
				this.Close(true);
				action();
				result = true;
			}
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight += 24f;
			}
			return result;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000FBC0 File Offset: 0x0000DDC0
		protected void DebugToolMap(string label, float columnWidth, Action toolAction, bool highlight)
		{
			if (WorldRendererUtility.WorldSelected)
			{
				return;
			}
			if (!base.FilterAllows(label))
			{
				GUI.color = new Color(1f, 1f, 1f, 0.3f);
			}
			if (this.ButtonDebug(label, columnWidth, highlight))
			{
				this.Close(true);
				DebugTools.curTool = new DebugTool(label, toolAction, null);
			}
			GUI.color = Color.white;
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight += 24f;
			}
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void ChangeHighlightedOption()
		{
		}

		// Token: 0x040001A9 RID: 425
		protected int currentHighlightIndex;

		// Token: 0x040001AA RID: 426
		protected int prioritizedHighlightedIndex;

		// Token: 0x040001AB RID: 427
		private const float DebugOptionsGap = 7f;
	}
}
