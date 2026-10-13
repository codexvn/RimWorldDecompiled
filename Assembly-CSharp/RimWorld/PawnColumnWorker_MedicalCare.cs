using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200346C RID: 13420
	public class PawnColumnWorker_MedicalCare : PawnColumnWorker
	{
		// Token: 0x06012DF7 RID: 77303 RVA: 0x005976A2 File Offset: 0x005958A2
		public override void DoHeader(Rect rect, PawnTable table)
		{
			MouseoverSounds.DoRegion(rect);
			base.DoHeader(rect, table);
		}

		// Token: 0x06012DF8 RID: 77304 RVA: 0x0059454B File Offset: 0x0059274B
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 28);
		}

		// Token: 0x06012DF9 RID: 77305 RVA: 0x0059455B File Offset: 0x0059275B
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06012DFA RID: 77306 RVA: 0x005976B2 File Offset: 0x005958B2
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			MedicalCareUtility.MedicalCareSelectButton(rect, pawn);
		}

		// Token: 0x06012DFB RID: 77307 RVA: 0x005976BB File Offset: 0x005958BB
		public override int Compare(Pawn a, Pawn b)
		{
			return a.playerSettings.medCare.CompareTo(b.playerSettings.medCare);
		}
	}
}
