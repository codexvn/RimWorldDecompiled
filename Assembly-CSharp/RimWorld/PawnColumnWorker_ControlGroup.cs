using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200344A RID: 13386
	public class PawnColumnWorker_ControlGroup : PawnColumnWorker
	{
		// Token: 0x06012D49 RID: 77129 RVA: 0x00595718 File Offset: 0x00593918
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.IsGestating())
			{
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect, "Gestating".Translate().Colorize(ColoredText.SubtleGrayColor));
				Text.Anchor = TextAnchor.UpperLeft;
				return;
			}
			if (pawn.GetOverseer() != null)
			{
				Widgets.Dropdown<Pawn, MechanitorControlGroup>(rect, pawn, (Pawn p) => p.GetMechControlGroup(), new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<MechanitorControlGroup>>>(this.Button_GenerateMenu), pawn.GetMechControlGroup().Index.ToString(), null, null, null, null, this.def.paintable);
			}
		}

		// Token: 0x06012D4A RID: 77130 RVA: 0x005957B0 File Offset: 0x005939B0
		private IEnumerable<Widgets.DropdownMenuElement<MechanitorControlGroup>> Button_GenerateMenu(Pawn pawn)
		{
			Pawn overseer = pawn.GetOverseer();
			PawnColumnWorker_ControlGroup.tmpControlGroups.Clear();
			PawnColumnWorker_ControlGroup.tmpControlGroups.AddRange(overseer.mechanitor.controlGroups);
			MechanitorControlGroup currentControlGroup = pawn.GetMechControlGroup();
			int num;
			for (int i = 0; i < PawnColumnWorker_ControlGroup.tmpControlGroups.Count; i = num + 1)
			{
				MechanitorControlGroup localControlGroup = PawnColumnWorker_ControlGroup.tmpControlGroups[i];
				if (currentControlGroup == localControlGroup)
				{
					yield return new Widgets.DropdownMenuElement<MechanitorControlGroup>
					{
						option = new FloatMenuOption("CannotAssignMechToControlGroup".Translate(localControlGroup.LabelIndexWithWorkMode) + ": " + "AssignMechAlreadyAssigned".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
						payload = localControlGroup
					};
				}
				else
				{
					yield return new Widgets.DropdownMenuElement<MechanitorControlGroup>
					{
						option = new FloatMenuOption("AssignMechToControlGroup".Translate(localControlGroup.LabelIndexWithWorkMode), delegate()
						{
							localControlGroup.Assign(pawn);
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
						payload = localControlGroup
					};
				}
				num = i;
			}
			PawnColumnWorker_ControlGroup.tmpControlGroups.Clear();
			yield break;
		}

		// Token: 0x06012D4B RID: 77131 RVA: 0x005957C0 File Offset: 0x005939C0
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 160);
		}

		// Token: 0x06012D4C RID: 77132 RVA: 0x0059455B File Offset: 0x0059275B
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x0400C358 RID: 50008
		private const int Width = 160;

		// Token: 0x0400C359 RID: 50009
		private static List<MechanitorControlGroup> tmpControlGroups = new List<MechanitorControlGroup>();
	}
}
