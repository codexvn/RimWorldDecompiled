using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003449 RID: 13385
	public class PawnColumnWorker_AutoRepair : PawnColumnWorker
	{
		// Token: 0x06012D44 RID: 77124 RVA: 0x00595664 File Offset: 0x00593864
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.Faction == Faction.OfPlayer && pawn.RaceProps.IsMechanoid && pawn.GetOverseer() == null)
			{
				return;
			}
			CompMechRepairable comp = pawn.GetComp<CompMechRepairable>();
			if (comp == null)
			{
				return;
			}
			rect.xMin += (rect.width - 24f) / 2f;
			rect.yMin += (rect.height - 24f) / 2f;
			Widgets.Checkbox(rect.position, ref comp.autoRepair, 24f, false, this.def.paintable, null, null);
		}

		// Token: 0x06012D45 RID: 77125 RVA: 0x00595706 File Offset: 0x00593906
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 24);
		}

		// Token: 0x06012D46 RID: 77126 RVA: 0x0059455B File Offset: 0x0059275B
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06012D47 RID: 77127 RVA: 0x00594570 File Offset: 0x00592770
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), 24);
		}
	}
}
