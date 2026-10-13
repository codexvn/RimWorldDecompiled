using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02003473 RID: 13427
	public class PawnColumnWorker_Reading : PawnColumnWorker
	{
		// Token: 0x06012E18 RID: 77336 RVA: 0x00597CC4 File Offset: 0x00595EC4
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
			Rect rect2 = new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f);
			if (Widgets.ButtonText(rect2, "ManageReadingPolicies".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_ManageReadingPolicies(null));
			}
			UIHighlighter.HighlightOpportunity(rect2, "ManageReadingAssignments");
			UIHighlighter.HighlightOpportunity(rect2, "ReadingAssignPolicy");
		}

		// Token: 0x06012E19 RID: 77337 RVA: 0x00597D5E File Offset: 0x00595F5E
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.reading == null)
			{
				return;
			}
			ReadingColumnUIUtility.DoAssignReadingButtons(rect, pawn);
		}

		// Token: 0x06012E1A RID: 77338 RVA: 0x00596898 File Offset: 0x00594A98
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(194f));
		}

		// Token: 0x06012E1B RID: 77339 RVA: 0x005968B0 File Offset: 0x00594AB0
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(Mathf.CeilToInt(251f), this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x06012E1C RID: 77340 RVA: 0x00595E9D File Offset: 0x0059409D
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x06012E1D RID: 77341 RVA: 0x00597D70 File Offset: 0x00595F70
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06012E1E RID: 77342 RVA: 0x00597D93 File Offset: 0x00595F93
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.reading != null && pawn.reading.CurrentPolicy != null)
			{
				return pawn.reading.CurrentPolicy.id;
			}
			return int.MinValue;
		}

		// Token: 0x0400C3BF RID: 50111
		private const int TopAreaHeight = 65;

		// Token: 0x0400C3C0 RID: 50112
		private const int ManageReadingButtonHeight = 32;
	}
}
