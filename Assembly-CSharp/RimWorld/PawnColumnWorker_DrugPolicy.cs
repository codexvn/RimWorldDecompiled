using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200345E RID: 13406
	public class PawnColumnWorker_DrugPolicy : PawnColumnWorker
	{
		// Token: 0x06012DA7 RID: 77223 RVA: 0x005967EC File Offset: 0x005949EC
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
			Rect rect2 = new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f);
			if (Widgets.ButtonText(rect2, "ManageDrugPolicies".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_ManageDrugPolicies(null));
			}
			UIHighlighter.HighlightOpportunity(rect2, "ManageDrugPolicies");
			UIHighlighter.HighlightOpportunity(rect2, "ButtonAssignDrugs");
		}

		// Token: 0x06012DA8 RID: 77224 RVA: 0x00596886 File Offset: 0x00594A86
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.drugs == null)
			{
				return;
			}
			DrugPolicyUIUtility.DoAssignDrugPolicyButtons(rect, pawn);
		}

		// Token: 0x06012DA9 RID: 77225 RVA: 0x00596898 File Offset: 0x00594A98
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(194f));
		}

		// Token: 0x06012DAA RID: 77226 RVA: 0x005968B0 File Offset: 0x00594AB0
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(Mathf.CeilToInt(251f), this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x06012DAB RID: 77227 RVA: 0x00595E9D File Offset: 0x0059409D
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x06012DAC RID: 77228 RVA: 0x005968D0 File Offset: 0x00594AD0
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06012DAD RID: 77229 RVA: 0x005968F3 File Offset: 0x00594AF3
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.drugs != null && pawn.drugs.CurrentPolicy != null)
			{
				return pawn.drugs.CurrentPolicy.id;
			}
			return int.MinValue;
		}

		// Token: 0x0400C38C RID: 50060
		private const int TopAreaHeight = 65;

		// Token: 0x0400C38D RID: 50061
		public const int ManageDrugPoliciesButtonHeight = 32;
	}
}
