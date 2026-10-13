using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200346A RID: 13418
	public class PawnColumnWorker_Label : PawnColumnWorker
	{
		// Token: 0x1700314E RID: 12622
		// (get) Token: 0x06012DE3 RID: 77283 RVA: 0x00075162 File Offset: 0x00073362
		protected virtual TextAnchor LabelAlignment
		{
			get
			{
				return TextAnchor.MiddleLeft;
			}
		}

		// Token: 0x1700314F RID: 12623
		// (get) Token: 0x06012DE4 RID: 77284 RVA: 0x00123991 File Offset: 0x00121B91
		protected override TextAnchor DefaultHeaderAlignment
		{
			get
			{
				return TextAnchor.LowerLeft;
			}
		}

		// Token: 0x06012DE5 RID: 77285 RVA: 0x005971EE File Offset: 0x005953EE
		protected override float GetHeaderOffsetX(Rect rect)
		{
			return 33f;
		}

		// Token: 0x06012DE6 RID: 77286 RVA: 0x005971F8 File Offset: 0x005953F8
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Rect rect2 = new Rect(rect.x, rect.y, rect.width, Mathf.Min(rect.height, this.def.groupable ? rect.height : ((float)this.GetMinCellHeight(pawn))));
			Rect rect3 = rect2;
			rect3.xMin += 3f;
			if (this.def.showIcon)
			{
				rect3.xMin += rect2.height;
				Rect rect4 = new Rect(rect2.x, rect2.y, rect2.height, rect2.height);
				if (Find.Selector.IsSelected(pawn))
				{
					SelectionDrawerUtility.DrawSelectionOverlayWholeGUI(rect4.ContractedBy(2f));
				}
				Widgets.ThingIcon(rect4, pawn, 1f, null, false, 1f, false);
			}
			if (pawn.health.summaryHealth.SummaryHealthPercent < 0.99f)
			{
				Rect rect5 = new Rect(rect3.x - 3f, rect3.y, rect3.width + 3f, rect3.height);
				rect5.yMin += 4f;
				rect5.yMax -= 6f;
				Widgets.FillableBar(rect5, pawn.health.summaryHealth.SummaryHealthPercent, GenMapUI.OverlayHealthTex, BaseContent.ClearTex, false);
			}
			if (Mouse.IsOver(rect2))
			{
				GUI.DrawTexture(rect2, TexUI.HighlightTex);
			}
			TaggedString taggedString = this.GetLabel(pawn);
			if (rect3.width != PawnColumnWorker_Label.labelCacheForWidth)
			{
				PawnColumnWorker_Label.labelCacheForWidth = rect3.width;
				PawnColumnWorker_Label.labelCache.Clear();
			}
			if (Text.CalcSize(taggedString).x > rect3.width)
			{
				taggedString = taggedString.Truncate(rect3.width, PawnColumnWorker_Label.labelCache);
			}
			if (pawn.IsSlave || pawn.IsColonyMech)
			{
				taggedString = taggedString.Colorize(PawnNameColorUtility.PawnNameColorOf(pawn));
			}
			Text.Font = GameFont.Small;
			Text.Anchor = this.LabelAlignment;
			Text.WordWrap = false;
			Widgets.Label(rect3, taggedString);
			Text.WordWrap = true;
			Text.Anchor = TextAnchor.UpperLeft;
			if (Widgets.ButtonInvisible(rect2, true))
			{
				CameraJumper.TryJumpAndSelect(pawn, CameraJumper.MovementMode.Pan);
				if (Current.ProgramState == ProgramState.Playing && Event.current.button == 0)
				{
					Find.MainTabsRoot.EscapeCurrentTab(false);
				}
				return;
			}
			if (Mouse.IsOver(rect2))
			{
				TipSignal tooltip = pawn.GetTooltip();
				tooltip.text = "ClickToJumpTo".Translate() + "\n\n" + tooltip.text;
				TooltipHandler.TipRegion(rect2, tooltip);
			}
		}

		// Token: 0x06012DE7 RID: 77287 RVA: 0x005974A0 File Offset: 0x005956A0
		private TaggedString GetLabel(Pawn pawn)
		{
			TaggedString result;
			if (!pawn.RaceProps.Humanlike && !pawn.RaceProps.Animal && pawn.Name != null && !pawn.Name.Numerical)
			{
				result = pawn.Name.ToStringShort.CapitalizeFirst() + ", " + pawn.KindLabel.Colorize(ColoredText.SubtleGrayColor);
			}
			else if (this.def.useLabelShort)
			{
				result = pawn.LabelShortCap;
			}
			else
			{
				result = pawn.LabelNoCount.CapitalizeFirst();
			}
			return result;
		}

		// Token: 0x06012DE8 RID: 77288 RVA: 0x0059753B File Offset: 0x0059573B
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 80);
		}

		// Token: 0x06012DE9 RID: 77289 RVA: 0x0059754B File Offset: 0x0059574B
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(165, this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x06012DEA RID: 77290 RVA: 0x00597565 File Offset: 0x00595765
		public override int Compare(Pawn a, Pawn b)
		{
			return string.Compare(this.GetValueToCompare(a), this.GetValueToCompare(b), StringComparison.Ordinal);
		}

		// Token: 0x06012DEB RID: 77291 RVA: 0x0059757B File Offset: 0x0059577B
		private string GetValueToCompare(Pawn pawn)
		{
			return this.GetLabel(pawn);
		}

		// Token: 0x0400C3AC RID: 50092
		private const int LeftMargin = 3;

		// Token: 0x0400C3AD RID: 50093
		private const float PortraitCameraZoom = 1.2f;

		// Token: 0x0400C3AE RID: 50094
		private static Dictionary<string, TaggedString> labelCache = new Dictionary<string, TaggedString>();

		// Token: 0x0400C3AF RID: 50095
		private static float labelCacheForWidth = -1f;
	}
}
