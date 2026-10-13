using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02003464 RID: 13412
	public class PawnColumnWorker_FoodRestriction : PawnColumnWorker
	{
		// Token: 0x06012DC2 RID: 77250 RVA: 0x00596D34 File Offset: 0x00594F34
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
			if (Widgets.ButtonText(new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f), "ManageFoodPolicies".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_ManageFoodPolicies(null));
			}
		}

		// Token: 0x06012DC3 RID: 77251 RVA: 0x00596DB8 File Offset: 0x00594FB8
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.foodRestriction == null)
			{
				return;
			}
			this.DoAssignFoodRestrictionButtons(rect, pawn);
		}

		// Token: 0x06012DC4 RID: 77252 RVA: 0x00596DCB File Offset: 0x00594FCB
		private IEnumerable<Widgets.DropdownMenuElement<FoodPolicy>> Button_GenerateMenu(Pawn pawn)
		{
			using (List<FoodPolicy>.Enumerator enumerator = Current.Game.foodRestrictionDatabase.AllFoodRestrictions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FoodPolicy foodRestriction = enumerator.Current;
					yield return new Widgets.DropdownMenuElement<FoodPolicy>
					{
						option = new FloatMenuOption(foodRestriction.label, delegate()
						{
							pawn.foodRestriction.CurrentFoodPolicy = foodRestriction;
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
						payload = foodRestriction
					};
				}
			}
			List<FoodPolicy>.Enumerator enumerator = default(List<FoodPolicy>.Enumerator);
			yield return new Widgets.DropdownMenuElement<FoodPolicy>
			{
				option = new FloatMenuOption(string.Format("{0}...", "AssignTabEdit".Translate()), delegate()
				{
					Find.WindowStack.Add(new Dialog_ManageFoodPolicies(pawn.foodRestriction.CurrentFoodPolicy));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0)
			};
			yield break;
			yield break;
		}

		// Token: 0x06012DC5 RID: 77253 RVA: 0x00596898 File Offset: 0x00594A98
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(194f));
		}

		// Token: 0x06012DC6 RID: 77254 RVA: 0x005968B0 File Offset: 0x00594AB0
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(Mathf.CeilToInt(251f), this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x06012DC7 RID: 77255 RVA: 0x00595E9D File Offset: 0x0059409D
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x06012DC8 RID: 77256 RVA: 0x00596DDC File Offset: 0x00594FDC
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06012DC9 RID: 77257 RVA: 0x00596DFF File Offset: 0x00594FFF
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.foodRestriction != null && pawn.foodRestriction.CurrentFoodPolicy != null)
			{
				return pawn.foodRestriction.CurrentFoodPolicy.id;
			}
			return int.MinValue;
		}

		// Token: 0x06012DCA RID: 77258 RVA: 0x00596E2C File Offset: 0x0059502C
		private void DoAssignFoodRestrictionButtons(Rect rect, Pawn pawn)
		{
			Rect rect2 = rect.ContractedBy(0f, 2f);
			Widgets.Dropdown<Pawn, FoodPolicy>(rect2, pawn, (Pawn p) => p.foodRestriction.CurrentFoodPolicy, new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<FoodPolicy>>>(this.Button_GenerateMenu), pawn.foodRestriction.CurrentFoodPolicy.label.Truncate(rect2.width, null), null, pawn.foodRestriction.CurrentFoodPolicy.label, null, null, true);
		}

		// Token: 0x0400C39C RID: 50076
		private const int TopAreaHeight = 65;

		// Token: 0x0400C39D RID: 50077
		public const int ManageFoodRestrictionsButtonHeight = 32;
	}
}
