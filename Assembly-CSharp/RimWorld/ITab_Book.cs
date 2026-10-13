using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200338E RID: 13198
	public class ITab_Book : ITab
	{
		// Token: 0x1700307A RID: 12410
		// (get) Token: 0x060128D3 RID: 75987 RVA: 0x0057997F File Offset: 0x00577B7F
		public override bool IsVisible
		{
			get
			{
				return base.SelThing is Book;
			}
		}

		// Token: 0x060128D4 RID: 75988 RVA: 0x0057998F File Offset: 0x00577B8F
		public ITab_Book()
		{
			this.size = new Vector2(Mathf.Min(610f, (float)UI.screenWidth), 350f);
			this.labelKey = "TabBookContents";
		}

		// Token: 0x060128D5 RID: 75989 RVA: 0x005799C4 File Offset: 0x00577BC4
		protected override void FillTab()
		{
			Book book = base.SelThing as Book;
			if (book == null)
			{
				return;
			}
			Rect rect = new Rect(0f, 20f, this.size.x, this.size.y - 20f).ContractedBy(10f);
			Rect rect2 = rect;
			rect2.y = 10f;
			rect2.height = 30f;
			Rect rect3 = rect;
			rect3.xMax = rect.center.x - 17f;
			rect3.y = rect2.yMax + 17f;
			rect3.yMax = rect.yMax;
			Rect rect4 = rect;
			rect4.x = rect3.xMax + 20f;
			rect4.xMax = rect.xMax + 10f;
			rect4.y = rect2.yMax + 26f;
			rect4.yMax = rect.yMax;
			BookUIUtility.DrawTitle(rect2, book);
			BookUIUtility.DrawBookInfoPanel(rect3, book);
			BookUIUtility.DrawBookDescPanel(rect4, book, ref this.infoScroll);
		}

		// Token: 0x0400C0B8 RID: 49336
		private Vector2 infoScroll;

		// Token: 0x0400C0B9 RID: 49337
		protected const float TopPadding = 20f;

		// Token: 0x0400C0BA RID: 49338
		protected const float InitialHeight = 350f;

		// Token: 0x0400C0BB RID: 49339
		public const float TitleHeight = 30f;

		// Token: 0x0400C0BC RID: 49340
		protected const float InitialWidth = 610f;
	}
}
