using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003451 RID: 13393
	public class PawnColumnWorker_Overseer : PawnColumnWorker_Label
	{
		// Token: 0x17003144 RID: 12612
		// (get) Token: 0x06012D67 RID: 77159 RVA: 0x000E9647 File Offset: 0x000E7847
		protected override TextAnchor LabelAlignment
		{
			get
			{
				return TextAnchor.MiddleCenter;
			}
		}

		// Token: 0x06012D68 RID: 77160 RVA: 0x00595C5C File Offset: 0x00593E5C
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Pawn overseer = pawn.GetOverseer();
			if (overseer == null)
			{
				return;
			}
			base.DoCell(rect, overseer, table);
		}

		// Token: 0x06012D69 RID: 77161 RVA: 0x00595C80 File Offset: 0x00593E80
		public override bool CanGroupWith(Pawn pawn, Pawn other)
		{
			Pawn overseer = pawn.GetOverseer();
			return overseer != null && other.GetOverseer() == overseer;
		}
	}
}
