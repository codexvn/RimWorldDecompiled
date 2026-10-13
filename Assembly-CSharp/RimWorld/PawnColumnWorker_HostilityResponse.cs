using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003469 RID: 13417
	public class PawnColumnWorker_HostilityResponse : PawnColumnWorker
	{
		// Token: 0x06012DDC RID: 77276 RVA: 0x0059717B File Offset: 0x0059537B
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (!pawn.RaceProps.Humanlike)
			{
				return;
			}
			HostilityResponseModeUtility.DrawResponseButton(rect, pawn, true);
		}

		// Token: 0x06012DDD RID: 77277 RVA: 0x00597193 File Offset: 0x00595393
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), Mathf.CeilToInt(24f) + 3);
		}

		// Token: 0x06012DDE RID: 77278 RVA: 0x00595706 File Offset: 0x00593906
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 24);
		}

		// Token: 0x06012DDF RID: 77279 RVA: 0x0059455B File Offset: 0x0059275B
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06012DE0 RID: 77280 RVA: 0x005971B0 File Offset: 0x005953B0
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06012DE1 RID: 77281 RVA: 0x005971D3 File Offset: 0x005953D3
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.playerSettings == null)
			{
				return int.MinValue;
			}
			return (int)pawn.playerSettings.hostilityResponse;
		}

		// Token: 0x0400C3AA RID: 50090
		private const int TopPadding = 3;

		// Token: 0x0400C3AB RID: 50091
		private const int Width = 24;
	}
}
