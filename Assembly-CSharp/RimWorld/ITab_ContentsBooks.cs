using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003392 RID: 13202
	public class ITab_ContentsBooks : ITab_ContentsBase
	{
		// Token: 0x1700307F RID: 12415
		// (get) Token: 0x060128E5 RID: 76005 RVA: 0x0057A15E File Offset: 0x0057835E
		public override IList<Thing> container
		{
			get
			{
				return this.Bookcase.GetDirectlyHeldThings().ToList<Thing>();
			}
		}

		// Token: 0x17003080 RID: 12416
		// (get) Token: 0x060128E6 RID: 76006 RVA: 0x0057A170 File Offset: 0x00578370
		public override bool IsVisible
		{
			get
			{
				return base.SelThing != null && base.IsVisible;
			}
		}

		// Token: 0x17003081 RID: 12417
		// (get) Token: 0x060128E7 RID: 76007 RVA: 0x0057A182 File Offset: 0x00578382
		public Building_Bookcase Bookcase
		{
			get
			{
				return base.SelThing as Building_Bookcase;
			}
		}

		// Token: 0x17003082 RID: 12418
		// (get) Token: 0x060128E8 RID: 76008 RVA: 0x00002501 File Offset: 0x00000701
		public override bool VisibleInBlueprintMode
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060128E9 RID: 76009 RVA: 0x0057A18F File Offset: 0x0057838F
		public ITab_ContentsBooks()
		{
			this.labelKey = "TabCasketContents";
			this.containedItemsKey = "TabCasketContents";
		}

		// Token: 0x060128EA RID: 76010 RVA: 0x0057A1AD File Offset: 0x005783AD
		protected override void DoItemsLists(Rect inRect, ref float curY)
		{
			this.ListContainedBooks(inRect, this.container, ref curY);
		}

		// Token: 0x060128EB RID: 76011 RVA: 0x0057A1C0 File Offset: 0x005783C0
		private void ListContainedBooks(Rect inRect, IList<Thing> books, ref float curY)
		{
			GUI.BeginGroup(inRect);
			float num = curY;
			Widgets.ListSeparator(ref curY, inRect.width, this.containedItemsKey.Translate());
			Rect rect = new Rect(0f, num, inRect.width, curY - num - 3f);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				TooltipHandler.TipRegionByKey(rect, "ContainedBooksDesc");
			}
			bool flag = false;
			for (int i = 0; i < books.Count; i++)
			{
				Book book = books[i] as Book;
				if (book != null)
				{
					flag = true;
					this.DoRow(book, inRect.width, i, ref curY);
				}
			}
			if (!flag)
			{
				Widgets.NoneLabel(ref curY, inRect.width, null);
			}
			GUI.EndGroup();
		}

		// Token: 0x060128EC RID: 76012 RVA: 0x0057A278 File Offset: 0x00578478
		private void DoRow(Book book, float width, int i, ref float curY)
		{
			Rect rect = new Rect(0f, curY, width, 28f);
			Widgets.InfoCardButton(0f, curY, book);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlightSelected(rect);
			}
			else if (i % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			Rect rect2 = new Rect(rect.width - 24f, curY, 24f, 24f);
			if (Widgets.ButtonImage(rect2, ITab_ContentsBooks.DropTex.Texture, true, null))
			{
				IntVec3 position;
				if (!(from x in this.Bookcase.OccupiedRect().AdjacentCells
				where x.Walkable(this.Bookcase.Map)
				select x).TryRandomElement(out position))
				{
					position = this.Bookcase.Position;
				}
				Thing thing;
				this.Bookcase.GetDirectlyHeldThings().TryDrop(book, position, this.Bookcase.Map, ThingPlaceMode.Near, 1, out thing, null, null);
				CompForbiddable compForbiddable;
				if (thing.TryGetComp(out compForbiddable))
				{
					compForbiddable.Forbidden = true;
				}
			}
			else if (Widgets.ButtonInvisible(rect, true))
			{
				Find.Selector.ClearSelection();
				Find.Selector.Select(book, true, true);
				InspectPaneUtility.OpenTab(typeof(ITab_Book));
			}
			TooltipHandler.TipRegionByKey(rect2, "EjectBookTooltip");
			Widgets.ThingIcon(new Rect(24f, curY, 28f, 28f), book, 1f, null, false, 1f, false);
			Rect rect3 = new Rect(60f, curY, rect.width - 36f, rect.height);
			rect3.xMax = rect2.xMin;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect3, book.LabelCap.Truncate(rect3.width, null));
			Text.Anchor = TextAnchor.UpperLeft;
			if (Mouse.IsOver(rect))
			{
				TargetHighlighter.Highlight(book, true, false, false);
				TooltipHandler.TipRegion(rect, book.DescriptionDetailed);
			}
			curY += 28f;
		}

		// Token: 0x0400C0CE RID: 49358
		private static readonly CachedTexture DropTex = new CachedTexture("UI/Buttons/Drop");
	}
}
