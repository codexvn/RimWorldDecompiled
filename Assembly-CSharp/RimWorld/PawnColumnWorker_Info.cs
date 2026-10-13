using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003441 RID: 13377
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Info : PawnColumnWorker
	{
		// Token: 0x06012D1E RID: 77086 RVA: 0x00595302 File Offset: 0x00593502
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Widgets.InfoCardButtonCentered(rect, pawn);
		}

		// Token: 0x06012D1F RID: 77087 RVA: 0x0059530C File Offset: 0x0059350C
		public override int GetMinWidth(PawnTable table)
		{
			return 24;
		}

		// Token: 0x06012D20 RID: 77088 RVA: 0x0059530C File Offset: 0x0059350C
		public override int GetMaxWidth(PawnTable table)
		{
			return 24;
		}
	}
}
