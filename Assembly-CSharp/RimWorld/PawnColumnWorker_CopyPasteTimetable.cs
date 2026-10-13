using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003437 RID: 13367
	public class PawnColumnWorker_CopyPasteTimetable : PawnColumnWorker_CopyPaste
	{
		// Token: 0x1700313C RID: 12604
		// (get) Token: 0x06012CE9 RID: 77033 RVA: 0x00594C71 File Offset: 0x00592E71
		protected override bool AnythingInClipboard
		{
			get
			{
				return PawnColumnWorker_CopyPasteTimetable.clipboard != null;
			}
		}

		// Token: 0x06012CEA RID: 77034 RVA: 0x00594C7B File Offset: 0x00592E7B
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.timetable == null)
			{
				return;
			}
			if (pawn.IsSubhuman)
			{
				return;
			}
			base.DoCell(rect, pawn, table);
		}

		// Token: 0x06012CEB RID: 77035 RVA: 0x00594C98 File Offset: 0x00592E98
		protected override void CopyFrom(Pawn p)
		{
			PawnColumnWorker_CopyPasteTimetable.clipboard = p.timetable.times.ToList<TimeAssignmentDef>();
		}

		// Token: 0x06012CEC RID: 77036 RVA: 0x00594CB0 File Offset: 0x00592EB0
		protected override void PasteTo(Pawn p)
		{
			for (int i = 0; i < 24; i++)
			{
				p.timetable.times[i] = PawnColumnWorker_CopyPasteTimetable.clipboard[i];
			}
		}

		// Token: 0x0400C34E RID: 49998
		private static List<TimeAssignmentDef> clipboard;
	}
}
