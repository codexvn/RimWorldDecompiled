using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003452 RID: 13394
	public class PawnColumnWorker_WorkMode : PawnColumnWorker
	{
		// Token: 0x06012D6B RID: 77163 RVA: 0x00595CAC File Offset: 0x00593EAC
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			MechanitorControlGroup mechControlGroup = pawn.GetMechControlGroup();
			if (mechControlGroup == null)
			{
				return;
			}
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			Rect rect2 = rect;
			rect2.xMin += 3f;
			Widgets.Label(rect2, mechControlGroup.WorkMode.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			if (Mouse.IsOver(rect))
			{
				AcceptanceReport canControlMechs = mechControlGroup.Tracker.CanControlMechs;
				TipSignal tooltip = pawn.GetTooltip();
				tooltip.text = "ClickToChangeWorkMode".Translate();
				if (!canControlMechs && !canControlMechs.Reason.NullOrEmpty())
				{
					tooltip.text = tooltip.text + "\n\n" + ("DisabledCommand".Translate() + ": " + canControlMechs.Reason).Colorize(ColorLibrary.RedReadable);
				}
				TooltipHandler.TipRegion(rect, tooltip);
				if (canControlMechs && Widgets.ButtonInvisible(rect, true))
				{
					PawnColumnWorker_WorkMode.tmpFloatMenuOptions.Clear();
					PawnColumnWorker_WorkMode.tmpFloatMenuOptions.AddRange(MechanitorControlGroupGizmo.GetWorkModeOptions(mechControlGroup));
					Find.WindowStack.Add(new FloatMenu(PawnColumnWorker_WorkMode.tmpFloatMenuOptions));
					PawnColumnWorker_WorkMode.tmpFloatMenuOptions.Clear();
				}
				Widgets.DrawHighlight(rect);
			}
		}

		// Token: 0x06012D6C RID: 77164 RVA: 0x00595DDC File Offset: 0x00593FDC
		public override bool CanGroupWith(Pawn pawn, Pawn other)
		{
			MechanitorControlGroup mechControlGroup = pawn.GetMechControlGroup();
			return mechControlGroup != null && other.GetMechControlGroup() == mechControlGroup;
		}

		// Token: 0x06012D6D RID: 77165 RVA: 0x005957C0 File Offset: 0x005939C0
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 160);
		}

		// Token: 0x06012D6E RID: 77166 RVA: 0x0059455B File Offset: 0x0059275B
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x0400C36A RID: 50026
		private const int Width = 160;

		// Token: 0x0400C36B RID: 50027
		private const int LeftMargin = 3;

		// Token: 0x0400C36C RID: 50028
		private static List<FloatMenuOption> tmpFloatMenuOptions = new List<FloatMenuOption>();
	}
}
