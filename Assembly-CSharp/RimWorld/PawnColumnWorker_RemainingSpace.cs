using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003448 RID: 13384
	public class PawnColumnWorker_RemainingSpace : PawnColumnWorker
	{
		// Token: 0x06012D3E RID: 77118 RVA: 0x000026EA File Offset: 0x000008EA
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
		}

		// Token: 0x06012D3F RID: 77119 RVA: 0x00002501 File Offset: 0x00000701
		public override int GetMinWidth(PawnTable table)
		{
			return 0;
		}

		// Token: 0x06012D40 RID: 77120 RVA: 0x002C8DC2 File Offset: 0x002C6FC2
		public override int GetMaxWidth(PawnTable table)
		{
			return 1000000;
		}

		// Token: 0x06012D41 RID: 77121 RVA: 0x00595658 File Offset: 0x00593858
		public override int GetOptimalWidth(PawnTable table)
		{
			return this.GetMaxWidth(table);
		}

		// Token: 0x06012D42 RID: 77122 RVA: 0x00002501 File Offset: 0x00000701
		public override int GetMinCellHeight(Pawn pawn)
		{
			return 0;
		}
	}
}
