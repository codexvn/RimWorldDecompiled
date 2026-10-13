using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003456 RID: 13398
	public class PawnColumnWorker_Carry : PawnColumnWorker
	{
		// Token: 0x06012D82 RID: 77186 RVA: 0x005961A8 File Offset: 0x005943A8
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.inventoryStock == null)
			{
				return;
			}
			float num = rect.width - 4f;
			int num2 = Mathf.FloorToInt(num * 0.33333334f);
			float x = rect.x;
			InventoryStockGroupDef group = InventoryStockGroupDefOf.Medicine;
			Widgets.Dropdown<Pawn, ThingDef>(new Rect(x, rect.y + 2f, (float)num2, rect.height - 4f), pawn, (Pawn p) => p.inventoryStock.GetDesiredThingForGroup(group), (Pawn p) => this.GenerateThingButtons(p, group), null, pawn.inventoryStock.GetDesiredThingForGroup(group).uiIcon, null, null, null, true);
			float x2 = x + (float)num2 + 4f;
			int num3 = Mathf.FloorToInt(num * 0.6666667f);
			Widgets.Dropdown<Pawn, int>(new Rect(x2, rect.y + 2f, (float)num3, rect.height - 4f), pawn, (Pawn p) => p.inventoryStock.GetDesiredCountForGroup(group), (Pawn p) => this.GenerateCountButtons(p, group), pawn.inventoryStock.GetDesiredCountForGroup(group).ToString(), null, null, null, null, true);
		}

		// Token: 0x06012D83 RID: 77187 RVA: 0x005962C7 File Offset: 0x005944C7
		private IEnumerable<Widgets.DropdownMenuElement<ThingDef>> GenerateThingButtons(Pawn pawn, InventoryStockGroupDef group)
		{
			using (List<ThingDef>.Enumerator enumerator = group.thingDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef thing = enumerator.Current;
					yield return new Widgets.DropdownMenuElement<ThingDef>
					{
						option = new FloatMenuOption(thing.LabelCap, delegate()
						{
							pawn.inventoryStock.SetThingForGroup(group, thing);
						}, thing, null, false, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, null),
						payload = thing
					};
				}
			}
			List<ThingDef>.Enumerator enumerator = default(List<ThingDef>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06012D84 RID: 77188 RVA: 0x005962DE File Offset: 0x005944DE
		private IEnumerable<Widgets.DropdownMenuElement<int>> GenerateCountButtons(Pawn pawn, InventoryStockGroupDef group)
		{
			int num;
			for (int i = group.min; i <= group.max; i = num + 1)
			{
				int localI = i;
				yield return new Widgets.DropdownMenuElement<int>
				{
					option = new FloatMenuOption(i.ToString(), delegate()
					{
						pawn.inventoryStock.SetCountForGroup(group, localI);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
					payload = i
				};
				num = i;
			}
			yield break;
		}

		// Token: 0x06012D85 RID: 77189 RVA: 0x005962F5 File Offset: 0x005944F5
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(54f));
		}

		// Token: 0x06012D86 RID: 77190 RVA: 0x0059630D File Offset: 0x0059450D
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(Mathf.CeilToInt(104f), this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x06012D87 RID: 77191 RVA: 0x00595E9D File Offset: 0x0059409D
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x06012D88 RID: 77192 RVA: 0x0059632C File Offset: 0x0059452C
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06012D89 RID: 77193 RVA: 0x0059634F File Offset: 0x0059454F
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.inventoryStock != null)
			{
				return pawn.inventoryStock.GetDesiredCountForGroup(InventoryStockGroupDefOf.Medicine);
			}
			return int.MinValue;
		}

		// Token: 0x0400C36F RID: 50031
		private const int TopAreaHeight = 65;
	}
}
