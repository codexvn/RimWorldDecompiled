using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200346D RID: 13421
	public class PawnColumnWorker_Outfit : PawnColumnWorker
	{
		// Token: 0x06012DFD RID: 77309 RVA: 0x005976E4 File Offset: 0x005958E4
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
			Rect rect2 = new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f);
			if (Widgets.ButtonText(rect2, "ManageApparelPolicies".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_ManageApparelPolicies(null));
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Outfits, KnowledgeAmount.Total);
			}
			UIHighlighter.HighlightOpportunity(rect2, "ManageOutfits");
		}

		// Token: 0x06012DFE RID: 77310 RVA: 0x00597780 File Offset: 0x00595980
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.outfits == null)
			{
				return;
			}
			Rect rect2 = rect.ContractedBy(0f, 2f);
			bool somethingIsForced = pawn.outfits.forcedHandler.SomethingIsForced;
			Rect rect3 = rect2;
			Rect rect4 = default(Rect);
			if (somethingIsForced)
			{
				rect2.SplitVerticallyWithMargin(out rect3, out rect4, 4f);
			}
			if (pawn.IsQuestLodger())
			{
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect3, "Unchangeable".Translate().Truncate(rect3.width, null));
				TooltipHandler.TipRegionByKey(rect3, "QuestRelated_Outfit");
				Text.Anchor = TextAnchor.UpperLeft;
			}
			else
			{
				Widgets.Dropdown<Pawn, ApparelPolicy>(rect3, pawn, (Pawn p) => p.outfits.CurrentApparelPolicy, new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<ApparelPolicy>>>(this.Button_GenerateMenu), pawn.outfits.CurrentApparelPolicy.label.Truncate(rect3.width, null), null, pawn.outfits.CurrentApparelPolicy.label, null, null, true);
			}
			if (somethingIsForced)
			{
				if (Widgets.ButtonText(rect4, "ClearForcedApparel".Translate(), true, true, true, null))
				{
					pawn.outfits.forcedHandler.Reset();
				}
				if (Mouse.IsOver(rect4))
				{
					TooltipHandler.TipRegion(rect4, new TipSignal(delegate()
					{
						string text = "ForcedApparel".Translate() + ":\n";
						foreach (Apparel apparel in pawn.outfits.forcedHandler.ForcedApparel)
						{
							text = text + "\n   " + apparel.LabelCap;
						}
						return text;
					}, pawn.GetHashCode() * 612));
				}
			}
		}

		// Token: 0x06012DFF RID: 77311 RVA: 0x0059790A File Offset: 0x00595B0A
		private IEnumerable<Widgets.DropdownMenuElement<ApparelPolicy>> Button_GenerateMenu(Pawn pawn)
		{
			using (List<ApparelPolicy>.Enumerator enumerator = Current.Game.outfitDatabase.AllOutfits.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ApparelPolicy outfit = enumerator.Current;
					yield return new Widgets.DropdownMenuElement<ApparelPolicy>
					{
						option = new FloatMenuOption(outfit.label, delegate()
						{
							pawn.outfits.CurrentApparelPolicy = outfit;
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
						payload = outfit
					};
				}
			}
			List<ApparelPolicy>.Enumerator enumerator = default(List<ApparelPolicy>.Enumerator);
			yield return new Widgets.DropdownMenuElement<ApparelPolicy>
			{
				option = new FloatMenuOption(string.Format("{0}...", "AssignTabEdit".Translate()), delegate()
				{
					Find.WindowStack.Add(new Dialog_ManageApparelPolicies(pawn.outfits.CurrentApparelPolicy));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0)
			};
			yield break;
			yield break;
		}

		// Token: 0x06012E00 RID: 77312 RVA: 0x00596898 File Offset: 0x00594A98
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(194f));
		}

		// Token: 0x06012E01 RID: 77313 RVA: 0x005968B0 File Offset: 0x00594AB0
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(Mathf.CeilToInt(251f), this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x06012E02 RID: 77314 RVA: 0x00595E9D File Offset: 0x0059409D
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x06012E03 RID: 77315 RVA: 0x0059791C File Offset: 0x00595B1C
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06012E04 RID: 77316 RVA: 0x0059793F File Offset: 0x00595B3F
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.outfits != null && pawn.outfits.CurrentApparelPolicy != null)
			{
				return pawn.outfits.CurrentApparelPolicy.id;
			}
			return int.MinValue;
		}

		// Token: 0x0400C3B0 RID: 50096
		public const int TopAreaHeight = 65;

		// Token: 0x0400C3B1 RID: 50097
		public const int ManageOutfitsButtonHeight = 32;
	}
}
