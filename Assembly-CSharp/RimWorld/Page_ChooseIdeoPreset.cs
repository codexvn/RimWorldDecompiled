using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02003274 RID: 12916
	[StaticConstructorOnStartup]
	public class Page_ChooseIdeoPreset : Page
	{
		// Token: 0x17002EF9 RID: 12025
		// (get) Token: 0x060122BE RID: 74430 RVA: 0x0055418E File Offset: 0x0055238E
		public override string PageTitle
		{
			get
			{
				return "ChooseYourIdeoligion".Translate();
			}
		}

		// Token: 0x17002EFA RID: 12026
		// (get) Token: 0x060122BF RID: 74431 RVA: 0x0055419F File Offset: 0x0055239F
		public Texture2D RandomIcon
		{
			get
			{
				if (this.randomStructureIcon == null)
				{
					this.randomStructureIcon = ContentFinder<Texture2D>.Get("UI/Structures/Random", true);
				}
				return this.randomStructureIcon;
			}
		}

		// Token: 0x060122C0 RID: 74432 RVA: 0x005541C8 File Offset: 0x005523C8
		public override void PostOpen()
		{
			base.PostOpen();
			IdeoGenerationParms genParms = new IdeoGenerationParms(Find.FactionManager.OfPlayer.def, false, null, null, null, false, false, false, false, "", null, null, false, "", false);
			CultureDef culture;
			if (!(from x in DefDatabase<CultureDef>.AllDefs
			where Find.FactionManager.OfPlayer.def.allowedCultures.Contains(x)
			select x).TryRandomElement(out culture))
			{
				culture = DefDatabase<CultureDef>.AllDefs.RandomElement<CultureDef>();
			}
			this.classicIdeo = IdeoGenerator.GenerateClassicIdeo(culture, genParms, false);
			Find.IdeoManager.classicMode = false;
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (faction != Faction.OfPlayer && faction.ideos != null && faction.ideos.PrimaryIdeo.memes.NullOrEmpty<MemeDef>())
				{
					FactionDef def = faction.def;
					if (faction.def.fixedIdeo)
					{
						FactionDef forFaction = def;
						bool forceNoExpansionIdeo = false;
						List<PreceptDef> disallowedPrecepts = null;
						List<MemeDef> disallowedMemes = null;
						string ideoName = def.ideoName;
						List<StyleCategoryDef> styles = def.styles;
						List<DeityPreset> deityPresets = def.deityPresets;
						bool hiddenIdeo = def.hiddenIdeo;
						string ideoDescription = def.ideoDescription;
						IdeoGenerationParms parms = new IdeoGenerationParms(forFaction, forceNoExpansionIdeo, disallowedPrecepts, disallowedMemes, def.forcedMemes, false, false, false, true, ideoName, styles, deityPresets, hiddenIdeo, ideoDescription, def.requiredPreceptsOnly);
						faction.ideos.ChooseOrGenerateIdeo(parms);
					}
					else
					{
						faction.ideos.ChooseOrGenerateIdeo(new IdeoGenerationParms(faction.def, false, null, null, null, false, false, false, false, "", null, null, false, "", false));
					}
				}
			}
			this.AssignIdeoToPlayer(this.classicIdeo);
			Faction.OfPlayer.ideos.SetPrimary(this.classicIdeo);
			Find.IdeoManager.RemoveUnusedStartingIdeos();
			if (Find.Storyteller.def.tutorialMode)
			{
				this.DoNext();
			}
		}

		// Token: 0x060122C1 RID: 74433 RVA: 0x005543B8 File Offset: 0x005525B8
		public override void DoWindowContents(Rect inRect)
		{
			base.DrawPageTitle(inRect);
			float num = 0f;
			Rect mainRect = base.GetMainRect(inRect, 0f, false);
			TaggedString taggedString = "ChooseYourIdeoligionDesc".Translate();
			float num2 = Text.CalcHeight(taggedString, mainRect.width);
			Rect rect = mainRect;
			rect.yMin += num;
			Widgets.Label(rect, taggedString);
			num += num2 + 10f;
			this.DrawStructureAndStyleSelection(inRect);
			Rect outRect = mainRect;
			outRect.width = 954f;
			outRect.yMin += num;
			float num3 = (this.InitialSize.x - 937f) / 2f;
			float x = (inRect.width - Page_ChooseIdeoPreset.ButtonSize.x - 10f - 500f - 16f) / 2f - num3;
			Rect viewRect = new Rect(-num3, 0f, 921f, this.totalCategoryListHeight + 100f);
			Widgets.BeginScrollView(outRect, ref this.leftScrollPosition, viewRect, true);
			num = 0f;
			this.lastCategoryGroupLabel = "";
			float a = 0f;
			Rect rect2 = new Rect(x, num + Text.LineHeight, Page_ChooseIdeoPreset.ButtonSize.x, Page_ChooseIdeoPreset.ButtonSize.y);
			Page_ChooseIdeoPreset.<DoWindowContents>g__DrawSplitCategoryInfo|26_0(IdeoPresetCategoryDefOf.Classic, rect2);
			this.DrawSelectable(rect2, "PlayClassic".Translate(), null, TextAnchor.MiddleCenter, this.presetSelection == Page_ChooseIdeoPreset.PresetSelection.Classic, true, null, delegate
			{
				this.selectedIdeo = null;
				this.presetSelection = Page_ChooseIdeoPreset.PresetSelection.Classic;
			});
			num = Mathf.Max(a, rect2.yMax) + Text.LineHeight;
			Widgets.Label(new Rect(0f, num, 300f, Text.LineHeight), "CustomIdeoligions".Translate());
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			Widgets.DrawLineHorizontal(0f, num + Text.LineHeight + 2f, 901f);
			GUI.color = Color.white;
			num += 12f;
			float a2 = num;
			Rect rect3 = new Rect(x, num + Text.LineHeight, Page_ChooseIdeoPreset.ButtonSize.x, Page_ChooseIdeoPreset.ButtonSize.y);
			Page_ChooseIdeoPreset.<DoWindowContents>g__DrawSplitCategoryInfo|26_0(IdeoPresetCategoryDefOf.Fluid, rect3);
			this.DrawSelectable(rect3, "CreateCustomFluid".Translate(), null, TextAnchor.MiddleCenter, this.presetSelection == Page_ChooseIdeoPreset.PresetSelection.CustomFluid, true, null, delegate
			{
				this.selectedIdeo = null;
				this.presetSelection = Page_ChooseIdeoPreset.PresetSelection.CustomFluid;
			});
			float num4 = Mathf.Max(a2, rect3.yMax);
			num = num4 + 10f;
			Rect rect4 = new Rect(x, num + Text.LineHeight, Page_ChooseIdeoPreset.ButtonSize.x, Page_ChooseIdeoPreset.ButtonSize.y);
			Page_ChooseIdeoPreset.<DoWindowContents>g__DrawSplitCategoryInfo|26_0(IdeoPresetCategoryDefOf.Custom, rect4);
			this.DrawSelectable(rect4, "CreateCustomFixed".Translate(), null, TextAnchor.MiddleCenter, this.presetSelection == Page_ChooseIdeoPreset.PresetSelection.CustomFixed, true, null, delegate
			{
				this.selectedIdeo = null;
				this.presetSelection = Page_ChooseIdeoPreset.PresetSelection.CustomFixed;
			});
			Rect rect5 = new Rect(rect4.xMax - Page_ChooseIdeoPreset.ButtonSizeSmall.x, rect4.yMax + 10f, Page_ChooseIdeoPreset.ButtonSizeSmall.x, Page_ChooseIdeoPreset.ButtonSizeSmall.y);
			this.DrawSelectable(rect5, "LoadSaved".Translate() + "...", null, TextAnchor.MiddleCenter, this.presetSelection == Page_ChooseIdeoPreset.PresetSelection.Load, true, null, delegate
			{
				this.selectedIdeo = null;
				this.presetSelection = Page_ChooseIdeoPreset.PresetSelection.Load;
			});
			num = Mathf.Max(num4, rect5.yMax) + 10f;
			foreach (IdeoPresetCategoryDef category in from c in DefDatabase<IdeoPresetCategoryDef>.AllDefsListForReading
			where c != IdeoPresetCategoryDefOf.Classic && c != IdeoPresetCategoryDefOf.Custom && c != IdeoPresetCategoryDefOf.Fluid
			select c)
			{
				this.DrawCategory(category, ref num);
			}
			this.totalCategoryListHeight = num;
			Widgets.EndScrollView();
			base.DoBottomButtons(inRect, null, null, null, true, true);
		}

		// Token: 0x060122C2 RID: 74434 RVA: 0x00554798 File Offset: 0x00552998
		private void DrawCategory(IdeoPresetCategoryDef category, ref float curY)
		{
			float num = 0f;
			if (category.groupLabel != this.lastCategoryGroupLabel)
			{
				curY += 16f;
				Widgets.Label(new Rect(0f, curY, 300f, Text.LineHeight), category.groupLabel);
				GUI.color = new Color(1f, 1f, 1f, 0.5f);
				Widgets.DrawLineHorizontal(0f, curY + Text.LineHeight + 2f, 901f);
				GUI.color = Color.white;
				num += Text.LineHeight + 12f;
			}
			if (category.LabelCap != category.groupLabel)
			{
				Rect rect = new Rect(0f, curY + num, 300f, Text.LineHeight);
				num += Text.LineHeight;
				Widgets.Label(rect, category.LabelCap);
			}
			float height = Text.CalcHeight(category.description, 300f);
			Widgets.Label(new Rect(0f, curY + num, 300f, height), category.description);
			curY += num;
			this.ideosPerRow.Clear();
			int num2 = 0;
			float num3 = 0f;
			this.ideosPerRow.Add(0);
			IEnumerable<IdeoPresetDef> allDefs = DefDatabase<IdeoPresetDef>.AllDefs;
			Func<IdeoPresetDef, bool> <>9__0;
			Func<IdeoPresetDef, bool> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = ((IdeoPresetDef i) => i.categoryDef == category));
			}
			foreach (IdeoPresetDef ideo in allDefs.Where(predicate))
			{
				float num4 = this.RectForIdeo(ideo).width + 17f;
				if (num3 + num4 <= 620f)
				{
					List<int> list = this.ideosPerRow;
					int index = num2;
					int num5 = list[index];
					list[index] = num5 + 1;
					num3 += num4;
				}
				else
				{
					num2++;
					this.ideosPerRow.Add(1);
					num3 = num4;
				}
			}
			num2 = 0;
			num3 = 0f;
			int num6 = 0;
			IEnumerable<IdeoPresetDef> allDefs2 = DefDatabase<IdeoPresetDef>.AllDefs;
			Func<IdeoPresetDef, bool> <>9__1;
			Func<IdeoPresetDef, bool> predicate2;
			if ((predicate2 = <>9__1) == null)
			{
				predicate2 = (<>9__1 = ((IdeoPresetDef i) => i.categoryDef == category));
			}
			foreach (IdeoPresetDef ideo2 in allDefs2.Where(predicate2))
			{
				Rect rect2 = this.RectForIdeo(ideo2);
				float num7 = rect2.width + 10f;
				this.DrawIdeo(new Vector2(num3 + 300f + 17f, curY), ideo2);
				num3 += num7;
				num6++;
				if (num2 < this.ideosPerRow.Count && num6 >= this.ideosPerRow[num2])
				{
					num2++;
					num3 = 0f;
					num6 = 0;
					curY += rect2.height + 10f;
				}
			}
			curY += 8f;
			this.lastCategoryGroupLabel = category.groupLabel;
		}

		// Token: 0x060122C3 RID: 74435 RVA: 0x00554ADC File Offset: 0x00552CDC
		private void DrawSelectable(Rect rect, string label, string tooltip, TextAnchor textAnchor, bool active, bool tutorAllows, Action iconDrawer, Action onSelect)
		{
			TextAnchor anchor = Text.Anchor;
			Widgets.DrawOptionBackground(rect, active);
			Rect rect2 = rect.ContractedBy(2f);
			rect2.xMin += 4f;
			rect2.xMax -= 4f;
			Text.Anchor = textAnchor;
			Widgets.Label(rect2, label);
			Text.Anchor = anchor;
			if (iconDrawer != null)
			{
				iconDrawer();
			}
			if (Mouse.IsOver(rect) && !tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, new TipSignal(tooltip, 351941375, TooltipPriority.Ideo));
			}
			if (Widgets.ButtonInvisible(rect, true) && tutorAllows)
			{
				if (onSelect != null)
				{
					onSelect();
				}
				SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x060122C4 RID: 74436 RVA: 0x00554B8C File Offset: 0x00552D8C
		private void DrawIdeo(Vector2 offset, IdeoPresetDef ideo)
		{
			Page_ChooseIdeoPreset.<>c__DisplayClass30_0 CS$<>8__locals1 = new Page_ChooseIdeoPreset.<>c__DisplayClass30_0();
			CS$<>8__locals1.ideo = ideo;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.rect = this.RectForIdeo(CS$<>8__locals1.ideo);
			Page_ChooseIdeoPreset.<>c__DisplayClass30_0 CS$<>8__locals2 = CS$<>8__locals1;
			CS$<>8__locals2.rect.x = CS$<>8__locals2.rect.x + offset.x;
			Page_ChooseIdeoPreset.<>c__DisplayClass30_0 CS$<>8__locals3 = CS$<>8__locals1;
			CS$<>8__locals3.rect.y = CS$<>8__locals3.rect.y + offset.y;
			this.DrawSelectable(CS$<>8__locals1.rect, CS$<>8__locals1.ideo.LabelCap, CS$<>8__locals1.ideo.LabelCap.Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + CS$<>8__locals1.ideo.description, Text.Anchor, this.selectedIdeo == CS$<>8__locals1.ideo, !TutorSystem.TutorialMode || TutorSystem.AllowAction("IdeoPresetSelectIdeo"), delegate
			{
				float num = 5f;
				float num2 = 55f * (float)((CS$<>8__locals1.ideo.Icon != null) ? 1 : CS$<>8__locals1.ideo.memes.Count) + 5f;
				float num3 = CS$<>8__locals1.rect.width / 2f - num2 / 2f;
				if (CS$<>8__locals1.ideo.Icon != null)
				{
					GUI.DrawTexture(new Rect(CS$<>8__locals1.rect.x + num + num3, CS$<>8__locals1.rect.y + Text.LineHeight + 5f, 50f, 50f), CS$<>8__locals1.ideo.Icon);
					return;
				}
				foreach (MemeDef meme in CS$<>8__locals1.ideo.memes)
				{
					Rect rect = new Rect(CS$<>8__locals1.rect.x + num + num3, CS$<>8__locals1.rect.y + Text.LineHeight + 5f, 50f, 50f);
					CS$<>8__locals1.<>4__this.DrawMeme(rect, meme);
					num += 55f;
				}
			}, delegate
			{
				CS$<>8__locals1.<>4__this.presetSelection = Page_ChooseIdeoPreset.PresetSelection.Preset;
				CS$<>8__locals1.<>4__this.selectedIdeo = CS$<>8__locals1.ideo;
			});
		}

		// Token: 0x060122C5 RID: 74437 RVA: 0x00554C7C File Offset: 0x00552E7C
		private Rect RectForIdeo(IdeoPresetDef ideo)
		{
			float a = Text.CalcSize(ideo.LabelCap).x + 5f + 2f;
			float b = (float)((ideo.Icon != null) ? 1 : ideo.memes.Count) * 55f;
			return new Rect
			{
				width = Mathf.Max(Mathf.Max(a, b) + 5f, 110f),
				height = Text.LineHeight + 50f + 10f
			};
		}

		// Token: 0x060122C6 RID: 74438 RVA: 0x00554D10 File Offset: 0x00552F10
		private void DrawMeme(Rect rect, MemeDef meme)
		{
			Widgets.DrawLightHighlight(rect);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				TooltipHandler.TipRegion(rect, meme.LabelCap.Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + meme.description);
			}
			GUI.DrawTexture(rect, meme.Icon);
		}

		// Token: 0x060122C7 RID: 74439 RVA: 0x00554D68 File Offset: 0x00552F68
		private void DrawStructureAndStyleSelection(Rect rect)
		{
			Page_ChooseIdeoPreset.<>c__DisplayClass33_0 CS$<>8__locals1 = new Page_ChooseIdeoPreset.<>c__DisplayClass33_0();
			CS$<>8__locals1.rect = rect;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.numStyles = this.selectedStyles.Count;
			bool flag = CS$<>8__locals1.numStyles < 3;
			CS$<>8__locals1.curX = 10f;
			if (flag)
			{
				Rect rect2 = CS$<>8__locals1.<DrawStructureAndStyleSelection>g__GetRect|0();
				GUI.DrawTexture(rect2.ContractedBy(4f), Page_ChooseIdeoPreset.PlusTex);
				if (Mouse.IsOver(rect2))
				{
					Widgets.DrawHighlight(rect2);
					TooltipHandler.TipRegion(rect2, "AddStyleCategory".Translate() + "\n\n" + "StyleCategoryDescriptionAbstract".Translate().Resolve().Colorize(ColoredText.SubtleGrayColor));
				}
				if (Widgets.ButtonInvisible(rect2, true) && (!TutorSystem.TutorialMode || TutorSystem.AllowAction("IdeoPresetEditStyle")))
				{
					List<FloatMenuOption> options = new List<FloatMenuOption>();
					CS$<>8__locals1.<DrawStructureAndStyleSelection>g__FillAllAvailableStyles|1(ref options, -1);
					Find.WindowStack.Add(new FloatMenu(options));
				}
			}
			for (int i = this.selectedStyles.Count - 1; i >= 0; i--)
			{
				StyleCategoryDef styleCategoryDef = this.selectedStyles[i];
				Rect rect3 = CS$<>8__locals1.<DrawStructureAndStyleSelection>g__GetRect|0();
				GUI.DrawTexture(rect3.ContractedBy(2f), (styleCategoryDef != null) ? styleCategoryDef.Icon : this.RandomIcon);
				if (Mouse.IsOver(rect3))
				{
					Widgets.DrawHighlight(rect3);
					TooltipHandler.TipRegion(rect3, (styleCategoryDef != null) ? IdeoUIUtility.StyleTooltip(styleCategoryDef, IdeoEditMode.None, null, this.selectedStylesWithPriority, false) : "RandomStyleTip".Translate());
				}
				if (Widgets.ButtonInvisible(rect3, true) && (!TutorSystem.TutorialMode || TutorSystem.AllowAction("IdeoPresetEditStyle")))
				{
					List<FloatMenuOption> options2 = new List<FloatMenuOption>();
					CS$<>8__locals1.<DrawStructureAndStyleSelection>g__FillAllAvailableStyles|1(ref options2, i);
					Find.WindowStack.Add(new FloatMenu(options2));
				}
			}
			CS$<>8__locals1.curX += 4f;
			TaggedString taggedString = "Styles".Translate();
			float x = Text.CalcSize(taggedString).x;
			CS$<>8__locals1.curX += x;
			Rect rect4 = new Rect(CS$<>8__locals1.rect.xMax - CS$<>8__locals1.curX, CS$<>8__locals1.rect.yMin + 6f, x, Text.LineHeight);
			if (Mouse.IsOver(rect4))
			{
				Widgets.DrawHighlight(rect4.ExpandedBy(4f, 2f));
				TooltipHandler.TipRegion(rect4, "StyleCategoryDescriptionAbstract".Translate());
			}
			Widgets.Label(rect4, taggedString);
			CS$<>8__locals1.curX += 17f;
			Rect rect5 = CS$<>8__locals1.<DrawStructureAndStyleSelection>g__GetRect|0();
			GUI.DrawTexture(rect5.ContractedBy(2f), (this.selectedStructure != null) ? this.selectedStructure.Icon : this.RandomIcon);
			if (Mouse.IsOver(rect5))
			{
				Widgets.DrawHighlight(rect5);
				TooltipHandler.TipRegion(rect5, (this.selectedStructure != null) ? IdeoUIUtility.StructureTooltip(this.selectedStructure, IdeoEditMode.None) : "RandomStructureTip".Translate());
			}
			if (Widgets.ButtonInvisible(rect5, true) && (!TutorSystem.TutorialMode || TutorSystem.AllowAction("IdeoPresetEditStructure")))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				if (this.selectedStructure != null)
				{
					list.Add(new FloatMenuOption("Random".Translate(), delegate()
					{
						CS$<>8__locals1.<>4__this.selectedStructure = null;
					}, this.RandomIcon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
				}
				using (List<MemeDef>.Enumerator enumerator = DefDatabase<MemeDef>.AllDefsListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MemeDef meme = enumerator.Current;
						if (this.selectedStructure != meme && meme.category == MemeCategory.Structure && IdeoUtility.IsMemeAllowedFor(meme, Find.Scenario.playerFaction.factionDef))
						{
							list.Add(new FloatMenuOption(meme.LabelCap, delegate()
							{
								CS$<>8__locals1.<>4__this.selectedStructure = meme;
							}, meme.Icon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
						}
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			CS$<>8__locals1.curX += 4f;
			TaggedString taggedString2 = "Structure".Translate();
			float x2 = Text.CalcSize(taggedString2).x;
			CS$<>8__locals1.curX += x2;
			Rect rect6 = new Rect(CS$<>8__locals1.rect.xMax - CS$<>8__locals1.curX, CS$<>8__locals1.rect.yMin + 6f, x2, Text.LineHeight);
			if (Mouse.IsOver(rect6))
			{
				Widgets.DrawHighlight(rect6.ExpandedBy(4f, 2f));
				TooltipHandler.TipRegion(rect6, "StructureMemeTip".Translate());
			}
			Widgets.Label(rect6, taggedString2);
		}

		// Token: 0x060122C8 RID: 74440 RVA: 0x00555280 File Offset: 0x00553480
		private void DoClassic()
		{
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (faction.ideos != null)
				{
					faction.ideos.RemoveAll();
					faction.ideos.SetPrimary(this.classicIdeo);
				}
			}
			Find.IdeoManager.RemoveUnusedStartingIdeos();
			Find.Scenario.PostIdeoChosen();
			if (Find.Storyteller.def.tutorialMode)
			{
				this.next.prev = this.prev;
			}
			else
			{
				this.next.prev = this;
			}
			base.DoNext();
		}

		// Token: 0x060122C9 RID: 74441 RVA: 0x00555338 File Offset: 0x00553538
		private void DoCustomize(bool fluid = false)
		{
			if (!TutorSystem.TutorialMode || TutorSystem.AllowAction("IdeoPresetCustomizeIdeo"))
			{
				foreach (Ideo ideo in Find.IdeoManager.IdeosListForReading)
				{
					ideo.initialPlayerIdeo = false;
				}
				Faction.OfPlayer.ideos.RemoveAll();
				Find.IdeoManager.RemoveUnusedStartingIdeos();
				Page_ConfigureIdeo page_ConfigureIdeo;
				if (fluid)
				{
					page_ConfigureIdeo = new Page_ConfigureFluidIdeo();
					page_ConfigureIdeo.SelectOrMakeNewIdeo(null);
					page_ConfigureIdeo.ideo.Fluid = true;
				}
				else
				{
					page_ConfigureIdeo = new Page_ConfigureIdeo();
				}
				page_ConfigureIdeo.prev = this;
				page_ConfigureIdeo.next = this.next;
				this.next.prev = page_ConfigureIdeo;
				this.ResetSelection();
				Find.WindowStack.Add(page_ConfigureIdeo);
				this.Close(true);
			}
		}

		// Token: 0x060122CA RID: 74442 RVA: 0x0055541C File Offset: 0x0055361C
		private void DoLoad()
		{
			Dialog_IdeoList_Load window = new Dialog_IdeoList_Load(delegate(Ideo ideo)
			{
				this.AssignIdeoToPlayer(ideo);
				Find.IdeoManager.RemoveUnusedStartingIdeos();
				Find.Scenario.PostIdeoChosen();
				this.next.prev = this;
				this.ResetSelection();
				base.DoNext();
			});
			Find.WindowStack.Add(window);
		}

		// Token: 0x060122CB RID: 74443 RVA: 0x00555448 File Offset: 0x00553648
		private void DoPreset()
		{
			List<MemeDef> list = this.selectedIdeo.memes.ToList<MemeDef>();
			if (this.selectedStructure != null)
			{
				list.RemoveAll((MemeDef x) => x.category == MemeCategory.Structure);
				list.Add(this.selectedStructure);
			}
			else if (!list.Any((MemeDef x) => x.category == MemeCategory.Structure))
			{
				MemeDef item;
				if ((from m in DefDatabase<MemeDef>.AllDefsListForReading
				where m.category == MemeCategory.Structure && IdeoUtility.IsMemeAllowedFor(m, Find.Scenario.playerFaction.factionDef)
				select m).TryRandomElement(out item))
				{
					list.Add(item);
				}
			}
			Ideo ideo = IdeoGenerator.GenerateIdeo(new IdeoGenerationParms(Find.FactionManager.OfPlayer.def, false, null, null, list, this.selectedIdeo.classicPlus, true, false, false, "", null, null, false, "", false));
			this.ApplySelectedStylesToIdeo(ideo);
			this.AssignIdeoToPlayer(ideo);
			Find.IdeoManager.RemoveUnusedStartingIdeos();
			Find.Scenario.PostIdeoChosen();
			base.DoNext();
		}

		// Token: 0x060122CC RID: 74444 RVA: 0x00555564 File Offset: 0x00553764
		protected override void DoNext()
		{
			Find.IdeoManager.classicMode = (this.presetSelection == Page_ChooseIdeoPreset.PresetSelection.Classic);
			switch (this.presetSelection)
			{
			case Page_ChooseIdeoPreset.PresetSelection.Classic:
				this.DoClassic();
				return;
			case Page_ChooseIdeoPreset.PresetSelection.CustomFluid:
				this.DoCustomize(true);
				return;
			case Page_ChooseIdeoPreset.PresetSelection.CustomFixed:
				this.DoCustomize(false);
				return;
			case Page_ChooseIdeoPreset.PresetSelection.Load:
				this.DoLoad();
				return;
			case Page_ChooseIdeoPreset.PresetSelection.Preset:
				this.DoPreset();
				return;
			default:
				return;
			}
		}

		// Token: 0x060122CD RID: 74445 RVA: 0x005555CC File Offset: 0x005537CC
		private void ApplySelectedStylesToIdeo(Ideo ideo)
		{
			if (this.selectedStyles.Count == 1 && this.selectedStyles[0] == null)
			{
				ideo.foundation.RandomizeStyles();
				return;
			}
			List<StyleCategoryDef> finalizedStyles = this.selectedStyles.ToList<StyleCategoryDef>();
			Func<StyleCategoryDef, bool> <>9__0;
			for (int i = finalizedStyles.Count - 1; i >= 0; i--)
			{
				if (finalizedStyles[i] == null)
				{
					IEnumerable<StyleCategoryDef> allDefs = DefDatabase<StyleCategoryDef>.AllDefs;
					Func<StyleCategoryDef, bool> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((StyleCategoryDef x) => !x.fixedIdeoOnly && !finalizedStyles.Contains(x)));
					}
					StyleCategoryDef value;
					if (allDefs.Where(predicate).TryRandomElement(out value))
					{
						finalizedStyles[i] = value;
					}
					else
					{
						finalizedStyles.RemoveAt(i);
					}
				}
			}
			ideo.thingStyleCategories.Clear();
			for (int j = 0; j < finalizedStyles.Count; j++)
			{
				StyleCategoryDef category = finalizedStyles[j];
				ideo.thingStyleCategories.Add(new ThingStyleCategoryWithPriority(category, (float)(3 - j)));
			}
			ideo.SortStyleCategories();
		}

		// Token: 0x060122CE RID: 74446 RVA: 0x005556DC File Offset: 0x005538DC
		private void AssignIdeoToPlayer(Ideo ideo)
		{
			Faction.OfPlayer.ideos.SetPrimary(ideo);
			foreach (Ideo ideo2 in Find.IdeoManager.IdeosListForReading)
			{
				ideo2.initialPlayerIdeo = false;
			}
			ideo.initialPlayerIdeo = true;
			Find.IdeoManager.Add(ideo);
		}

		// Token: 0x060122CF RID: 74447 RVA: 0x00555754 File Offset: 0x00553954
		private void RecacheStyleCategoriesWithPriority()
		{
			this.selectedStylesWithPriority.Clear();
			for (int i = 0; i < this.selectedStyles.Count; i++)
			{
				StyleCategoryDef styleCategoryDef = this.selectedStyles[i];
				if (styleCategoryDef != null)
				{
					this.selectedStylesWithPriority.Add(new ThingStyleCategoryWithPriority(styleCategoryDef, (float)(3 - i)));
				}
			}
		}

		// Token: 0x060122D0 RID: 74448 RVA: 0x005557A7 File Offset: 0x005539A7
		private void ResetSelection()
		{
			this.selectedIdeo = null;
			this.selectedStructure = null;
			this.selectedStyles = new List<StyleCategoryDef>
			{
				null
			};
		}

		// Token: 0x060122D3 RID: 74451 RVA: 0x0055583A File Offset: 0x00553A3A
		[CompilerGenerated]
		internal static void <DoWindowContents>g__DrawSplitCategoryInfo|26_0(IdeoPresetCategoryDef cat, Rect buttonRect)
		{
			Rect rect = new Rect(buttonRect.xMax + 10f, buttonRect.y, 500f, buttonRect.height);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect, cat.description);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0400BCF1 RID: 48369
		private IdeoPresetDef selectedIdeo;

		// Token: 0x0400BCF2 RID: 48370
		private Ideo classicIdeo;

		// Token: 0x0400BCF3 RID: 48371
		private MemeDef selectedStructure;

		// Token: 0x0400BCF4 RID: 48372
		private List<StyleCategoryDef> selectedStyles = new List<StyleCategoryDef>
		{
			null
		};

		// Token: 0x0400BCF5 RID: 48373
		private List<ThingStyleCategoryWithPriority> selectedStylesWithPriority = new List<ThingStyleCategoryWithPriority>
		{
			null
		};

		// Token: 0x0400BCF6 RID: 48374
		private Page_ChooseIdeoPreset.PresetSelection presetSelection;

		// Token: 0x0400BCF7 RID: 48375
		private Vector2 leftScrollPosition;

		// Token: 0x0400BCF8 RID: 48376
		private float totalCategoryListHeight;

		// Token: 0x0400BCF9 RID: 48377
		private string lastCategoryGroupLabel;

		// Token: 0x0400BCFA RID: 48378
		private Texture2D randomStructureIcon;

		// Token: 0x0400BCFB RID: 48379
		private const float IdeosRowWidth = 620f;

		// Token: 0x0400BCFC RID: 48380
		private const float CategoryDescRowWidth = 300f;

		// Token: 0x0400BCFD RID: 48381
		private const float CategoryDescRowWidthSmall = 500f;

		// Token: 0x0400BCFE RID: 48382
		private const float CategoryRowWidth = 937f;

		// Token: 0x0400BCFF RID: 48383
		private const float MemeIconSize = 50f;

		// Token: 0x0400BD00 RID: 48384
		private const float IdeoBoxMargin = 5f;

		// Token: 0x0400BD01 RID: 48385
		private const float IdeoBoxWidthMin = 110f;

		// Token: 0x0400BD02 RID: 48386
		private static readonly Vector2 ButtonSize = new Vector2(160f, 60f);

		// Token: 0x0400BD03 RID: 48387
		private static readonly Vector2 ButtonSizeSmall = new Vector2(120f, 40f);

		// Token: 0x0400BD04 RID: 48388
		private static readonly Texture2D PlusTex = ContentFinder<Texture2D>.Get("UI/Buttons/Plus", true);

		// Token: 0x0400BD05 RID: 48389
		private List<int> ideosPerRow = new List<int>();

		// Token: 0x02003275 RID: 12917
		private enum PresetSelection
		{
			// Token: 0x0400BD07 RID: 48391
			Classic,
			// Token: 0x0400BD08 RID: 48392
			CustomFluid,
			// Token: 0x0400BD09 RID: 48393
			CustomFixed,
			// Token: 0x0400BD0A RID: 48394
			Load,
			// Token: 0x0400BD0B RID: 48395
			Preset
		}
	}
}
