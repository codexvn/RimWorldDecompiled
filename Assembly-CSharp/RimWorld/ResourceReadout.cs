using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003495 RID: 13461
	public class ResourceReadout
	{
		// Token: 0x06012EE3 RID: 77539 RVA: 0x0059B04B File Offset: 0x0059924B
		public ResourceReadout()
		{
			this.RootThingCategories = (from cat in DefDatabase<ThingCategoryDef>.AllDefs
			where cat.resourceReadoutRoot
			select cat).ToList<ThingCategoryDef>();
		}

		// Token: 0x06012EE4 RID: 77540 RVA: 0x0059B088 File Offset: 0x00599288
		public void ResourceReadoutOnGUI()
		{
			if (Event.current.type == EventType.Layout)
			{
				return;
			}
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			if (Find.MainTabsRoot.OpenTab == MainButtonDefOf.Menu)
			{
				return;
			}
			GenUI.DrawTextWinterShadow(new Rect(256f, 512f, -256f, -512f));
			Text.Font = GameFont.Small;
			Rect rect = Prefs.ResourceReadoutCategorized ? new Rect(2f, 7f, 124f, (float)(UI.screenHeight - 7) - 200f) : new Rect(7f, 7f, 110f, (float)(UI.screenHeight - 7) - 200f);
			Rect rect2 = new Rect(0f, 0f, rect.width, this.lastDrawnHeight);
			bool flag = rect2.height > rect.height;
			if (flag)
			{
				Widgets.BeginScrollView(rect, ref this.scrollPosition, rect2, false);
			}
			else
			{
				this.scrollPosition = Vector2.zero;
				Widgets.BeginGroup(rect);
			}
			if (!Prefs.ResourceReadoutCategorized)
			{
				this.DoReadoutSimple(rect2, rect.height);
			}
			else
			{
				this.DoReadoutCategorized(rect2);
			}
			if (flag)
			{
				Widgets.EndScrollView();
				return;
			}
			Widgets.EndGroup();
		}

		// Token: 0x06012EE5 RID: 77541 RVA: 0x0059B1B0 File Offset: 0x005993B0
		private void DoReadoutCategorized(Rect rect)
		{
			Listing_ResourceReadout listing_ResourceReadout = new Listing_ResourceReadout(Find.CurrentMap);
			listing_ResourceReadout.Begin(rect);
			listing_ResourceReadout.nestIndentWidth = 7f;
			listing_ResourceReadout.lineHeight = 24f;
			listing_ResourceReadout.verticalSpacing = 0f;
			for (int i = 0; i < this.RootThingCategories.Count; i++)
			{
				listing_ResourceReadout.DoCategory(this.RootThingCategories[i].treeNode, 0, 32);
			}
			listing_ResourceReadout.End();
			this.lastDrawnHeight = listing_ResourceReadout.CurHeight;
		}

		// Token: 0x06012EE6 RID: 77542 RVA: 0x0059B234 File Offset: 0x00599434
		private void DoReadoutSimple(Rect rect, float outRectHeight)
		{
			Widgets.BeginGroup(rect);
			Text.Anchor = TextAnchor.MiddleLeft;
			float num = 0f;
			foreach (KeyValuePair<ThingDef, int> keyValuePair in Find.CurrentMap.resourceCounter.AllCountedAmounts)
			{
				if (keyValuePair.Value > 0 || keyValuePair.Key.resourceReadoutAlwaysShow)
				{
					Rect rect2 = new Rect(0f, num, 999f, 24f);
					if (rect2.yMax >= this.scrollPosition.y && rect2.y <= this.scrollPosition.y + outRectHeight)
					{
						this.DrawResourceSimple(rect2, keyValuePair.Key);
					}
					num += 24f;
				}
			}
			Text.Anchor = TextAnchor.UpperLeft;
			this.lastDrawnHeight = num;
			Widgets.EndGroup();
		}

		// Token: 0x06012EE7 RID: 77543 RVA: 0x0059B31C File Offset: 0x0059951C
		public void DrawResourceSimple(Rect rect, ThingDef thingDef)
		{
			this.DrawIcon(rect.x, rect.y, thingDef);
			rect.y += 2f;
			int count = Find.CurrentMap.resourceCounter.GetCount(thingDef);
			Widgets.Label(new Rect(34f, rect.y, rect.width - 34f, rect.height), count.ToStringCached());
		}

		// Token: 0x06012EE8 RID: 77544 RVA: 0x0059B394 File Offset: 0x00599594
		private void DrawIcon(float x, float y, ThingDef thingDef)
		{
			Rect rect = new Rect(x, y, 27f, 27f);
			Color color = GUI.color;
			Widgets.ThingIcon(rect, thingDef, null, null, 1f, null, null, 1f);
			GUI.color = color;
			if (Mouse.IsOver(rect))
			{
				TaggedString str = thingDef.LabelCap + ": " + thingDef.description.CapitalizeFirst();
				TooltipHandler.TipRegion(rect, str);
			}
		}

		// Token: 0x0400C415 RID: 50197
		private Vector2 scrollPosition;

		// Token: 0x0400C416 RID: 50198
		private float lastDrawnHeight;

		// Token: 0x0400C417 RID: 50199
		private readonly List<ThingCategoryDef> RootThingCategories;

		// Token: 0x0400C418 RID: 50200
		private const float LineHeightSimple = 24f;

		// Token: 0x0400C419 RID: 50201
		private const float LineHeightCategorized = 24f;

		// Token: 0x0400C41A RID: 50202
		private const float DistFromScreenBottom = 200f;
	}
}
