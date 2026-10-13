using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E69 RID: 15977
	public class WITab_Caravan_Items : WITab
	{
		// Token: 0x06015DB0 RID: 89520 RVA: 0x00689448 File Offset: 0x00687648
		public WITab_Caravan_Items()
		{
			this.labelKey = "TabCaravanItems";
		}

		// Token: 0x06015DB1 RID: 89521 RVA: 0x00689468 File Offset: 0x00687668
		protected override void FillTab()
		{
			this.CheckCreateSorters();
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y);
			if (Widgets.ButtonText(new Rect(rect.x + 10f, rect.y + 10f, 200f, 27f), "AssignDrugPolicies".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_AssignCaravanDrugPolicies(base.SelCaravan));
			}
			rect.yMin += 37f;
			Widgets.BeginGroup(rect.ContractedBy(10f));
			TransferableUIUtility.DoTransferableSorters(this.sorter1, this.sorter2, delegate(TransferableSorterDef x)
			{
				this.sorter1 = x;
				this.CacheItems();
			}, delegate(TransferableSorterDef x)
			{
				this.sorter2 = x;
				this.CacheItems();
			});
			Widgets.EndGroup();
			rect.yMin += 25f;
			Widgets.BeginGroup(rect);
			this.CheckCacheItems();
			CaravanItemsTabUtility.DoRows(rect.size, this.cachedItems, base.SelCaravan, ref this.scrollPosition, ref this.scrollViewHeight);
			Widgets.EndGroup();
		}

		// Token: 0x06015DB2 RID: 89522 RVA: 0x0068959B File Offset: 0x0068779B
		protected override void UpdateSize()
		{
			base.UpdateSize();
			this.CheckCacheItems();
			this.size = CaravanItemsTabUtility.GetSize(this.cachedItems, this.PaneTopY, true);
		}

		// Token: 0x06015DB3 RID: 89523 RVA: 0x006895C4 File Offset: 0x006877C4
		private void CheckCacheItems()
		{
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(base.SelCaravan);
			if (list.Count != this.cachedItemsCount)
			{
				this.CacheItems();
				return;
			}
			int num = base.SelCaravan.GetHashCode();
			for (int i = 0; i < list.Count; i++)
			{
				num = Gen.HashCombineInt(num, list[i].GetHashCode());
			}
			if (num != this.cachedItemsHash)
			{
				this.CacheItems();
			}
		}

		// Token: 0x06015DB4 RID: 89524 RVA: 0x00689634 File Offset: 0x00687834
		private void CacheItems()
		{
			this.CheckCreateSorters();
			this.cachedItems.Clear();
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(base.SelCaravan);
			int seed = 0;
			for (int i = 0; i < list.Count; i++)
			{
				TransferableImmutable transferableImmutable = TransferableUtility.TransferableMatching<TransferableImmutable>(list[i], this.cachedItems, TransferAsOneMode.Normal);
				if (transferableImmutable == null)
				{
					transferableImmutable = new TransferableImmutable();
					this.cachedItems.Add(transferableImmutable);
				}
				transferableImmutable.things.Add(list[i]);
				seed = Gen.HashCombineInt(seed, list[i].GetHashCode());
			}
			this.cachedItems = this.cachedItems.OrderBy((TransferableImmutable tr) => tr, this.sorter1.Comparer).ThenBy((TransferableImmutable tr) => tr, this.sorter2.Comparer).ThenBy((TransferableImmutable tr) => TransferableUIUtility.DefaultListOrderPriority(tr)).ToList<TransferableImmutable>();
			this.cachedItemsCount = list.Count;
			this.cachedItemsHash = seed;
		}

		// Token: 0x06015DB5 RID: 89525 RVA: 0x00689765 File Offset: 0x00687965
		private void CheckCreateSorters()
		{
			if (this.sorter1 == null)
			{
				this.sorter1 = TransferableSorterDefOf.Category;
			}
			if (this.sorter2 == null)
			{
				this.sorter2 = TransferableSorterDefOf.MarketValue;
			}
		}

		// Token: 0x0400F27A RID: 62074
		private Vector2 scrollPosition;

		// Token: 0x0400F27B RID: 62075
		private float scrollViewHeight;

		// Token: 0x0400F27C RID: 62076
		private TransferableSorterDef sorter1;

		// Token: 0x0400F27D RID: 62077
		private TransferableSorterDef sorter2;

		// Token: 0x0400F27E RID: 62078
		private List<TransferableImmutable> cachedItems = new List<TransferableImmutable>();

		// Token: 0x0400F27F RID: 62079
		private int cachedItemsHash;

		// Token: 0x0400F280 RID: 62080
		private int cachedItemsCount;

		// Token: 0x0400F281 RID: 62081
		private const float SortersSpace = 25f;

		// Token: 0x0400F282 RID: 62082
		private const float AssignDrugPoliciesButtonHeight = 27f;
	}
}
