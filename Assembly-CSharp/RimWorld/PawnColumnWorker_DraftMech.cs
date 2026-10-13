using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200344F RID: 13391
	public class PawnColumnWorker_DraftMech : PawnColumnWorker
	{
		// Token: 0x06012D5D RID: 77149 RVA: 0x00595A7C File Offset: 0x00593C7C
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			if (!pawn.IsColonyMechPlayerControlled || !pawn.Spawned)
			{
				return;
			}
			AcceptanceReport report = MechanitorUtility.CanDraftMech(pawn);
			if (!report && report.Reason.NullOrEmpty())
			{
				return;
			}
			rect.xMin += (rect.width - 24f) / 2f;
			rect.yMin += (rect.height - 24f) / 2f;
			bool drafted = pawn.Drafted;
			Vector2 position = rect.position;
			float size = 24f;
			bool paintable = this.def.paintable;
			Widgets.Checkbox(position, ref drafted, size, !report, paintable, null, null);
			if (!report.Reason.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, report.Reason.CapitalizeFirst());
			}
			if (drafted != pawn.Drafted)
			{
				pawn.drafter.Drafted = drafted;
			}
		}

		// Token: 0x06012D5E RID: 77150 RVA: 0x00595B6E File Offset: 0x00593D6E
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 44);
		}

		// Token: 0x06012D5F RID: 77151 RVA: 0x0059455B File Offset: 0x0059275B
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06012D60 RID: 77152 RVA: 0x00594570 File Offset: 0x00592770
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), 24);
		}
	}
}
