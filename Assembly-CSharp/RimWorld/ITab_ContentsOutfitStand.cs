using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003398 RID: 13208
	public class ITab_ContentsOutfitStand : ITab_ContentsBase
	{
		// Token: 0x1700308B RID: 12427
		// (get) Token: 0x0601290A RID: 76042 RVA: 0x0057AFB8 File Offset: 0x005791B8
		public override IList<Thing> container
		{
			get
			{
				return this.OutfitStand.HeldItems.ToList<Thing>();
			}
		}

		// Token: 0x1700308C RID: 12428
		// (get) Token: 0x0601290B RID: 76043 RVA: 0x0057A170 File Offset: 0x00578370
		public override bool IsVisible
		{
			get
			{
				return base.SelThing != null && base.IsVisible;
			}
		}

		// Token: 0x1700308D RID: 12429
		// (get) Token: 0x0601290C RID: 76044 RVA: 0x0057AFCA File Offset: 0x005791CA
		public Building_OutfitStand OutfitStand
		{
			get
			{
				return base.SelThing as Building_OutfitStand;
			}
		}

		// Token: 0x1700308E RID: 12430
		// (get) Token: 0x0601290D RID: 76045 RVA: 0x00002501 File Offset: 0x00000701
		public override bool VisibleInBlueprintMode
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0601290E RID: 76046 RVA: 0x0057A18F File Offset: 0x0057838F
		public ITab_ContentsOutfitStand()
		{
			this.labelKey = "TabCasketContents";
			this.containedItemsKey = "TabCasketContents";
		}

		// Token: 0x0601290F RID: 76047 RVA: 0x0057AFD7 File Offset: 0x005791D7
		protected override void DoItemsLists(Rect inRect, ref float curY)
		{
			this.ListContainedApparel(inRect, this.container, ref curY);
		}

		// Token: 0x06012910 RID: 76048 RVA: 0x0057AFE8 File Offset: 0x005791E8
		private void ListContainedApparel(Rect inRect, IList<Thing> apparel, ref float curY)
		{
			GUI.BeginGroup(inRect);
			Widgets.ListSeparator(ref curY, inRect.width, this.containedItemsKey.Translate());
			bool flag = false;
			for (int i = 0; i < apparel.Count; i++)
			{
				Thing thing = apparel[i];
				if (thing != null)
				{
					flag = true;
					this.DoRow(thing, inRect.width, i, ref curY);
				}
			}
			if (!flag)
			{
				Widgets.NoneLabel(ref curY, inRect.width, null);
			}
			GUI.EndGroup();
		}

		// Token: 0x06012911 RID: 76049 RVA: 0x0057B060 File Offset: 0x00579260
		private void DoRow(Thing thing, float width, int i, ref float curY)
		{
			Rect rect = new Rect(0f, curY, width, 28f);
			Widgets.InfoCardButton(0f, curY, thing);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlightSelected(rect);
			}
			else if (i % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			Rect rect2 = new Rect(rect.width - 24f, curY, 24f, 24f);
			if (Widgets.ButtonImage(rect2, ITab_ContentsOutfitStand.DropTex.Texture, true, null))
			{
				IntVec3 position;
				if (!(from x in this.OutfitStand.OccupiedRect().AdjacentCells
				where x.Walkable(this.OutfitStand.Map)
				select x).TryRandomElement(out position))
				{
					position = this.OutfitStand.Position;
				}
				Thing thing2;
				this.OutfitStand.TryDrop(thing, position, ThingPlaceMode.Near, 1, out thing2);
				CompForbiddable compForbiddable;
				if (thing2.TryGetComp(out compForbiddable))
				{
					compForbiddable.Forbidden = true;
				}
			}
			else if (Widgets.ButtonInvisible(rect, true))
			{
				Find.Selector.ClearSelection();
				Find.Selector.Select(thing, true, true);
			}
			TooltipHandler.TipRegionByKey(rect2, "EjectApparelTooltip");
			Widgets.ThingIcon(new Rect(24f, curY, 28f, 28f), thing, 1f, null, false, 1f, false);
			Rect rect3 = new Rect(60f, curY, rect.width - 36f, rect.height);
			rect3.xMax = rect2.xMin;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect3, thing.LabelCap.Truncate(rect3.width, null));
			Text.Anchor = TextAnchor.UpperLeft;
			if (Mouse.IsOver(rect))
			{
				TargetHighlighter.Highlight(thing, true, false, false);
				TooltipHandler.TipRegion(rect, thing.DescriptionDetailed);
			}
			curY += 28f;
		}

		// Token: 0x0400C0D9 RID: 49369
		private static readonly CachedTexture DropTex = new CachedTexture("UI/Buttons/Drop");
	}
}
