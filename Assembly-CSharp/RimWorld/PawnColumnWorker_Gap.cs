using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003447 RID: 13383
	public class PawnColumnWorker_Gap : PawnColumnWorker
	{
		// Token: 0x17003141 RID: 12609
		// (get) Token: 0x06012D38 RID: 77112 RVA: 0x00595623 File Offset: 0x00593823
		protected virtual int Width
		{
			get
			{
				return this.def.gap;
			}
		}

		// Token: 0x06012D39 RID: 77113 RVA: 0x000026EA File Offset: 0x000008EA
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
		}

		// Token: 0x06012D3A RID: 77114 RVA: 0x00595630 File Offset: 0x00593830
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), this.Width);
		}

		// Token: 0x06012D3B RID: 77115 RVA: 0x00595644 File Offset: 0x00593844
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.Width);
		}

		// Token: 0x06012D3C RID: 77116 RVA: 0x00002501 File Offset: 0x00000701
		public override int GetMinCellHeight(Pawn pawn)
		{
			return 0;
		}
	}
}
