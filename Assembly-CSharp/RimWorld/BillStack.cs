using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010F5 RID: 4341
	public class BillStack : IExposable
	{
		// Token: 0x17001059 RID: 4185
		// (get) Token: 0x060069B2 RID: 27058 RVA: 0x0021488D File Offset: 0x00212A8D
		public List<Bill> Bills
		{
			get
			{
				return this.bills;
			}
		}

		// Token: 0x060069B3 RID: 27059 RVA: 0x00214895 File Offset: 0x00212A95
		public IEnumerator<Bill> GetEnumerator()
		{
			return this.bills.GetEnumerator();
		}

		// Token: 0x1700105A RID: 4186
		public Bill this[int index]
		{
			get
			{
				return this.bills[index];
			}
		}

		// Token: 0x1700105B RID: 4187
		// (get) Token: 0x060069B5 RID: 27061 RVA: 0x002148B5 File Offset: 0x00212AB5
		public int Count
		{
			get
			{
				return this.bills.Count;
			}
		}

		// Token: 0x1700105C RID: 4188
		// (get) Token: 0x060069B6 RID: 27062 RVA: 0x002148C4 File Offset: 0x00212AC4
		public Bill FirstShouldDoNow
		{
			get
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this.bills[i].ShouldDoNow())
					{
						return this.bills[i];
					}
				}
				return null;
			}
		}

		// Token: 0x1700105D RID: 4189
		// (get) Token: 0x060069B7 RID: 27063 RVA: 0x00214904 File Offset: 0x00212B04
		public bool AnyShouldDoNow
		{
			get
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this.bills[i].ShouldDoNow())
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060069B8 RID: 27064 RVA: 0x00214938 File Offset: 0x00212B38
		public BillStack(IBillGiver giver)
		{
			this.billGiver = giver;
		}

		// Token: 0x060069B9 RID: 27065 RVA: 0x00214952 File Offset: 0x00212B52
		public void AddBill(Bill bill)
		{
			bill.billStack = this;
			this.bills.Add(bill);
		}

		// Token: 0x060069BA RID: 27066 RVA: 0x00214967 File Offset: 0x00212B67
		public void Delete(Bill bill)
		{
			bill.deleted = true;
			this.bills.Remove(bill);
			this.billGiver.Notify_BillDeleted(bill);
		}

		// Token: 0x060069BB RID: 27067 RVA: 0x00214989 File Offset: 0x00212B89
		public void Clear()
		{
			this.bills.Clear();
		}

		// Token: 0x060069BC RID: 27068 RVA: 0x00214998 File Offset: 0x00212B98
		public void Reorder(Bill bill, int offset)
		{
			int num = this.bills.IndexOf(bill);
			num += offset;
			if (num >= 0)
			{
				this.bills.Remove(bill);
				this.bills.Insert(num, bill);
			}
		}

		// Token: 0x060069BD RID: 27069 RVA: 0x002149D4 File Offset: 0x00212BD4
		public void RemoveIncompletableBills()
		{
			for (int i = this.bills.Count - 1; i >= 0; i--)
			{
				if (!this.bills[i].CompletableEver)
				{
					Bill bill = this.bills[i];
					this.bills.Remove(bill);
					this.billGiver.Notify_BillDeleted(bill);
				}
			}
		}

		// Token: 0x060069BE RID: 27070 RVA: 0x00214A32 File Offset: 0x00212C32
		public int IndexOf(Bill bill)
		{
			return this.bills.IndexOf(bill);
		}

		// Token: 0x060069BF RID: 27071 RVA: 0x00214A40 File Offset: 0x00212C40
		public void ExposeData()
		{
			Scribe_Collections.Look<Bill>(ref this.bills, "bills", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				if (this.bills.RemoveAll((Bill x) => x == null) != 0)
				{
					Log.Error("Some bills were null after loading.");
				}
				if (this.bills.RemoveAll((Bill x) => x.recipe == null) != 0)
				{
					Log.Error("Some bills had null recipe after loading.");
				}
				for (int i = 0; i < this.bills.Count; i++)
				{
					this.bills[i].billStack = this;
				}
			}
		}

		// Token: 0x060069C0 RID: 27072 RVA: 0x00214B04 File Offset: 0x00212D04
		public Bill DoListing(Rect rect, Func<List<FloatMenuOption>> recipeOptionsMaker, ref Vector2 scrollPosition, ref float viewHeight)
		{
			Bill result = null;
			Widgets.BeginGroup(rect);
			Text.Font = GameFont.Small;
			if (this.Count < 15)
			{
				Rect rect2 = new Rect(0f, 0f, 150f, 29f);
				if (Widgets.ButtonText(rect2, "AddBill".Translate(), true, true, true, null))
				{
					Find.WindowStack.Add(new FloatMenu(recipeOptionsMaker()));
				}
				UIHighlighter.HighlightOpportunity(rect2, "AddBill");
			}
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
			Rect outRect = new Rect(0f, 35f, rect.width, rect.height - 35f);
			Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, viewHeight);
			Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect, true);
			float num = 0f;
			for (int i = 0; i < this.Count; i++)
			{
				Bill bill = this.bills[i];
				Rect rect3 = bill.DoInterface(0f, num, viewRect.width, i);
				if (!bill.DeletedOrDereferenced && Mouse.IsOver(rect3))
				{
					result = bill;
				}
				num += rect3.height + 6f;
			}
			if (Event.current.type == EventType.Layout)
			{
				viewHeight = num + 60f;
			}
			Widgets.EndScrollView();
			Widgets.EndGroup();
			return result;
		}

		// Token: 0x04004AC1 RID: 19137
		[Unsaved(false)]
		public IBillGiver billGiver;

		// Token: 0x04004AC2 RID: 19138
		private List<Bill> bills = new List<Bill>();

		// Token: 0x04004AC3 RID: 19139
		public const int MaxCount = 15;

		// Token: 0x04004AC4 RID: 19140
		private const float TopAreaHeight = 35f;

		// Token: 0x04004AC5 RID: 19141
		private const float BillInterfaceSpacing = 6f;

		// Token: 0x04004AC6 RID: 19142
		private const float ExtraViewHeight = 60f;
	}
}
