using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003435 RID: 13365
	public abstract class PawnColumnWorker_CopyPaste : PawnColumnWorker
	{
		// Token: 0x1700313B RID: 12603
		// (get) Token: 0x06012CDF RID: 77023
		protected abstract bool AnythingInClipboard { get; }

		// Token: 0x06012CE0 RID: 77024 RVA: 0x00594BD4 File Offset: 0x00592DD4
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Action pasteAction = null;
			if (this.AnythingInClipboard)
			{
				pasteAction = delegate()
				{
					this.PasteTo(pawn);
				};
			}
			CopyPasteUI.DoCopyPasteButtons(new Rect(rect.x, rect.y, 36f, 30f), delegate
			{
				this.CopyFrom(pawn);
			}, pasteAction);
		}

		// Token: 0x06012CE1 RID: 77025 RVA: 0x00594C3B File Offset: 0x00592E3B
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 36);
		}

		// Token: 0x06012CE2 RID: 77026 RVA: 0x0059455B File Offset: 0x0059275B
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06012CE3 RID: 77027
		protected abstract void CopyFrom(Pawn p);

		// Token: 0x06012CE4 RID: 77028
		protected abstract void PasteTo(Pawn p);
	}
}
