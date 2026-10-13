using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003394 RID: 13204
	public class ITab_ContentsGenepackHolder : ITab_ContentsBase
	{
		// Token: 0x17003084 RID: 12420
		// (get) Token: 0x060128F1 RID: 76017 RVA: 0x0057A4FE File Offset: 0x005786FE
		public override IList<Thing> container
		{
			get
			{
				return this.ContainerThing.innerContainer;
			}
		}

		// Token: 0x17003085 RID: 12421
		// (get) Token: 0x060128F2 RID: 76018 RVA: 0x0057A50B File Offset: 0x0057870B
		public CompGenepackContainer ContainerThing
		{
			get
			{
				return base.SelThing.TryGetComp<CompGenepackContainer>();
			}
		}

		// Token: 0x060128F3 RID: 76019 RVA: 0x0057A18F File Offset: 0x0057838F
		public ITab_ContentsGenepackHolder()
		{
			this.labelKey = "TabCasketContents";
			this.containedItemsKey = "TabCasketContents";
		}

		// Token: 0x060128F4 RID: 76020 RVA: 0x0057A518 File Offset: 0x00578718
		public override void OnOpen()
		{
			if (!ModLister.CheckBiotech("genepack container"))
			{
				this.CloseTab();
			}
		}

		// Token: 0x060128F5 RID: 76021 RVA: 0x0057A52C File Offset: 0x0057872C
		protected override void DoItemsLists(Rect inRect, ref float curY)
		{
			CompGenepackContainer containerThing = this.ContainerThing;
			bool autoLoad = containerThing.autoLoad;
			Rect rect = new Rect(inRect.x, inRect.y, inRect.width, 24f);
			Widgets.CheckboxLabeled(rect, "AllowAllGenepacks".Translate(), ref containerThing.autoLoad, false, null, null, false, false);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				TooltipHandler.TipRegionByKey(rect, "AllowAllGenepacksDesc");
			}
			if (autoLoad != containerThing.autoLoad)
			{
				containerThing.leftToLoad.Clear();
			}
			curY += 28f;
			this.ListContainedGenepacks(inRect, containerThing, ref curY);
			if (!containerThing.autoLoad)
			{
				this.ListGenepacksToLoad(inRect, containerThing, ref curY);
				this.ListGenepacksOnMap(inRect, containerThing, ref curY);
			}
		}

		// Token: 0x060128F6 RID: 76022 RVA: 0x0057A5E0 File Offset: 0x005787E0
		private void ListContainedGenepacks(Rect inRect, CompGenepackContainer container, ref float curY)
		{
			GUI.BeginGroup(inRect);
			float num = curY;
			Widgets.ListSeparator(ref curY, inRect.width, this.containedItemsKey.Translate());
			Rect rect = new Rect(0f, num, inRect.width, curY - num - 3f);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				TooltipHandler.TipRegionByKey(rect, "ContainedGenepacksDesc");
			}
			List<Genepack> containedGenepacks = container.ContainedGenepacks;
			bool flag = false;
			for (int i = 0; i < containedGenepacks.Count; i++)
			{
				Genepack genepack = containedGenepacks[i];
				if (genepack != null)
				{
					flag = true;
					this.DoRow(genepack, container, inRect.width, ref curY, true);
				}
			}
			if (!flag)
			{
				Widgets.NoneLabel(ref curY, inRect.width, null);
			}
			GUI.EndGroup();
		}

		// Token: 0x060128F7 RID: 76023 RVA: 0x0057A6A0 File Offset: 0x005788A0
		private void ListGenepacksToLoad(Rect inRect, CompGenepackContainer container, ref float curY)
		{
			bool flag = false;
			GUI.BeginGroup(inRect);
			float num = curY;
			Widgets.ListSeparator(ref curY, inRect.width, "GenepacksToLoad".Translate());
			Rect rect = new Rect(0f, num, inRect.width, curY - num - 3f);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				TooltipHandler.TipRegionByKey(rect, "GenepacksToLoadDesc");
			}
			if (container.leftToLoad != null)
			{
				for (int i = container.leftToLoad.Count - 1; i >= 0; i--)
				{
					Genepack genepack = container.leftToLoad[i] as Genepack;
					if (genepack == null || genepack.Destroyed || genepack.MapHeld != container.parent.Map || !genepack.AutoLoad)
					{
						container.leftToLoad.RemoveAt(i);
					}
					else
					{
						this.DoRow(genepack, container, inRect.width, ref curY, false);
						flag = true;
					}
				}
			}
			if (!flag)
			{
				Widgets.NoneLabel(ref curY, inRect.width, null);
			}
			GUI.EndGroup();
		}

		// Token: 0x060128F8 RID: 76024 RVA: 0x0057A7A0 File Offset: 0x005789A0
		private void ListGenepacksOnMap(Rect inRect, CompGenepackContainer container, ref float curY)
		{
			bool flag = false;
			GUI.BeginGroup(inRect);
			float num = curY;
			Widgets.ListSeparator(ref curY, inRect.width, "GenepacksToIgnore".Translate());
			Rect rect = new Rect(0f, num, inRect.width, curY - num - 3f);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				TooltipHandler.TipRegionByKey(rect, "GenepacksIgnoredDesc");
			}
			List<Thing> list = container.parent.Map.listerThings.ThingsOfDef(ThingDefOf.Genepack);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				Thing thing = list[i];
				if (thing != null)
				{
					Genepack genepack = (Genepack)thing;
					if (genepack.targetContainer == null && genepack.AutoLoad)
					{
						this.DoRow((Genepack)thing, container, inRect.width, ref curY, false);
						flag = true;
					}
				}
			}
			if (!flag)
			{
				Widgets.NoneLabel(ref curY, inRect.width, null);
			}
			GUI.EndGroup();
		}

		// Token: 0x060128F9 RID: 76025 RVA: 0x0057A898 File Offset: 0x00578A98
		private void DoRow(Genepack genepack, CompGenepackContainer container, float width, ref float curY, bool insideContainer)
		{
			bool flag = container.leftToLoad.Contains(genepack);
			bool flag2 = flag;
			Rect rect = new Rect(0f, curY, width, 28f);
			Rect rect2 = new Rect(rect.width - 24f, curY, 24f, 24f);
			if (insideContainer)
			{
				if (Widgets.ButtonImage(rect2, ITab_ContentsGenepackHolder.DropTex.Texture, true, null))
				{
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmRemoveGenepack".Translate(genepack.LabelNoCount), delegate()
					{
						this.OnDropThing(genepack, genepack.stackCount);
					}, false, null, WindowLayer.Dialog));
				}
				TooltipHandler.TipRegionByKey(rect2, "EjectGenepackDesc");
			}
			else
			{
				Widgets.Checkbox(rect.width - 24f, curY, ref flag, 24f, false, false, null, null);
				string key = flag ? "RemoveFromLoadingListDesc" : "AddToLoadingListDesc";
				TooltipHandler.TipRegionByKey(rect2, key);
			}
			rect.width -= 24f;
			for (int i = Mathf.Min(genepack.GeneSet.GenesListForReading.Count, 5) - 1; i >= 0; i--)
			{
				GeneDef geneDef = genepack.GeneSet.GenesListForReading[i];
				Rect rect3 = new Rect(rect.xMax - 22f, rect.yMax - rect.height / 2f - 11f, 22f, 22f);
				Widgets.DefIcon(rect3, geneDef, null, 0.75f, null, false, null, null, null, 1f);
				Rect rect4 = rect3;
				rect4.yMin = rect.yMin;
				rect4.yMax = rect.yMax;
				if (Mouse.IsOver(rect4))
				{
					Widgets.DrawHighlight(rect4);
					TooltipHandler.TipRegion(rect4, geneDef.LabelCap + "\n\n" + geneDef.DescriptionFull);
				}
				rect.xMax -= 22f;
			}
			Widgets.InfoCardButton(0f, curY, genepack);
			if (Mouse.IsOver(rect))
			{
				GUI.color = ITab_ContentsBase.ThingHighlightColor;
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			Widgets.ThingIcon(new Rect(24f, curY, 28f, 28f), genepack, 1f, null, false, 1f, false);
			Rect rect5 = new Rect(60f, curY, rect.width - 36f, rect.height);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect5, genepack.LabelCap.Truncate(rect5.width, null));
			Text.Anchor = TextAnchor.UpperLeft;
			if (Mouse.IsOver(rect))
			{
				TargetHighlighter.Highlight(genepack, true, false, false);
				TooltipHandler.TipRegion(rect, genepack.LabelCap);
			}
			curY += 28f;
			if (flag2 != flag)
			{
				if (!flag)
				{
					genepack.targetContainer = null;
					container.leftToLoad.Remove(genepack);
					return;
				}
				if (!container.CanLoadMore)
				{
					Messages.Message("CanOnlyStoreNumGenepacks".Translate(container.parent, container.Props.maxCapacity).CapitalizeFirst(), container.parent, MessageTypeDefOf.RejectInput, false);
					return;
				}
				genepack.targetContainer = container.parent;
				container.leftToLoad.Add(genepack);
			}
		}

		// Token: 0x0400C0D0 RID: 49360
		private static readonly CachedTexture DropTex = new CachedTexture("UI/Buttons/Drop");

		// Token: 0x0400C0D1 RID: 49361
		private const float MiniGeneIconSize = 22f;
	}
}
