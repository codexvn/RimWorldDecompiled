using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020025FA RID: 9722
	[StaticConstructorOnStartup]
	public static class IdeoUIUtility
	{
		// Token: 0x170020B9 RID: 8377
		// (get) Token: 0x0600D548 RID: 54600 RVA: 0x003F0250 File Offset: 0x003EE450
		public static Ideo FallbackSelectedIdeo
		{
			get
			{
				Faction ofPlayerSilentFail = Faction.OfPlayerSilentFail;
				Ideo ideo;
				if (ofPlayerSilentFail == null)
				{
					ideo = null;
				}
				else
				{
					FactionIdeosTracker ideos = ofPlayerSilentFail.ideos;
					ideo = ((ideos != null) ? ideos.PrimaryIdeo : null);
				}
				return ideo ?? Find.IdeoManager.IdeosInViewOrder.FirstOrFallback(null);
			}
		}

		// Token: 0x170020BA RID: 8378
		// (get) Token: 0x0600D549 RID: 54601 RVA: 0x003F0284 File Offset: 0x003EE484
		public static string ClickToEdit
		{
			get
			{
				return "ClickToEdit".Translate().CapitalizeFirst().Colorize(ColorLibrary.Green);
			}
		}

		// Token: 0x170020BB RID: 8379
		// (get) Token: 0x0600D54A RID: 54602 RVA: 0x003F02AD File Offset: 0x003EE4AD
		private static Texture2D InitialPlayerIdeoTex
		{
			get
			{
				return ContentFinder<Texture2D>.Get("UI/Icons/InitialPlayerIdeo", true);
			}
		}

		// Token: 0x170020BC RID: 8380
		// (get) Token: 0x0600D54B RID: 54603 RVA: 0x003F02BA File Offset: 0x003EE4BA
		public static bool DevEditMode
		{
			get
			{
				return Prefs.DevMode && IdeoUIUtility.devEditMode;
			}
		}

		// Token: 0x170020BD RID: 8381
		// (get) Token: 0x0600D54C RID: 54604 RVA: 0x003F02CA File Offset: 0x003EE4CA
		public static Texture2D FluidIdeoIcon
		{
			get
			{
				if (IdeoUIUtility.fluidIdeoIcon == null)
				{
					IdeoUIUtility.fluidIdeoIcon = ContentFinder<Texture2D>.Get("UI/Icons/FluidIdeo", true);
				}
				return IdeoUIUtility.fluidIdeoIcon;
			}
		}

		// Token: 0x170020BE RID: 8382
		// (get) Token: 0x0600D54D RID: 54605 RVA: 0x003F02F0 File Offset: 0x003EE4F0
		public static bool PlayerPrimaryIdeoNotShared
		{
			get
			{
				if (Faction.OfPlayer.ideos.PrimaryIdeo == null)
				{
					return false;
				}
				foreach (Faction faction in Find.FactionManager.AllFactions)
				{
					if (!faction.IsPlayer && faction.ideos != null && faction.ideos.Has(Faction.OfPlayer.ideos.PrimaryIdeo))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x0600D54E RID: 54606 RVA: 0x003F0380 File Offset: 0x003EE580
		public static bool TutorAllowsInteraction(IdeoEditMode editMode)
		{
			return editMode == IdeoEditMode.Dev || TutorSystem.AllowAction("ConfiguringIdeo");
		}

		// Token: 0x0600D54F RID: 54607 RVA: 0x003F0397 File Offset: 0x003EE597
		public static void SetSelected(Ideo ideo)
		{
			IdeoUIUtility.selected = ideo;
		}

		// Token: 0x0600D550 RID: 54608 RVA: 0x003F039F File Offset: 0x003EE59F
		public static void UnselectCurrent()
		{
			IdeoUIUtility.selected = null;
		}

		// Token: 0x0600D551 RID: 54609 RVA: 0x003F03A8 File Offset: 0x003EE5A8
		public static void DoIdeoIcon(Rect rect, Ideo ideo, bool doTooltip = true, Action extraAction = null)
		{
			if (ideo == null)
			{
				return;
			}
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				if (doTooltip)
				{
					TooltipHandler.TipRegion(rect, ideo.name);
				}
			}
			ideo.DrawIcon(rect);
			if (extraAction != null && Widgets.ButtonInvisible(rect, true))
			{
				extraAction();
			}
		}

		// Token: 0x0600D552 RID: 54610 RVA: 0x003F03F4 File Offset: 0x003EE5F4
		public static void OpenIdeoInfo(Ideo ideo)
		{
			IdeoUIUtility.selected = ideo;
			if (Current.ProgramState == ProgramState.Playing && !Find.WindowStack.AnyWindowAbsorbingAllInput)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Ideos, true);
				return;
			}
			Find.WindowStack.Add(new Dialog_IdeosDuringLanding());
		}

		// Token: 0x0600D553 RID: 54611 RVA: 0x003F0430 File Offset: 0x003EE630
		public static void DoIdeoListAndDetails(Rect fillRect, ref Vector2 scrollPosition_list, ref float scrollViewHeight_list, ref Vector2 scrollPosition_details, ref float scrollViewHeight_details, bool editMode = false, bool showCreateIdeoButton = false, List<Pawn> pawns = null, Ideo onlyEditIdeo = null, Action createCustomBtnActOverride = null, bool forArchonexusRestart = false, Func<Pawn, Ideo> pawnIdeoGetter = null, Action<Ideo> ideoLoadedFromFile = null, bool showLoadExistingIdeoBtn = false, bool allowLoad = true, Action createFluidBtnAct = null)
		{
			Text.Font = GameFont.Small;
			if (Prefs.DevMode)
			{
				Widgets.CheckboxLabeled(new Rect(fillRect.width - 310f, 0f, 150f, 24f), "DEV: Show all", ref IdeoUIUtility.showAll, false, null, null, false, false);
				Widgets.CheckboxLabeled(new Rect(fillRect.width - 155f, 0f, 150f, 24f), "DEV: Edit mode", ref IdeoUIUtility.devEditMode, false, null, null, false, false);
			}
			else
			{
				IdeoUIUtility.showAll = false;
			}
			Ideo ideo = null;
			Rect fillRect2 = new Rect(fillRect.x, fillRect.y, (float)Mathf.FloorToInt(fillRect.width * 0.25f), fillRect.height);
			IdeoUIUtility.DoIdeoList(fillRect2, ref scrollPosition_list, ref scrollViewHeight_list, out ideo, showCreateIdeoButton, pawns, createCustomBtnActOverride, forArchonexusRestart, pawnIdeoGetter, showLoadExistingIdeoBtn, createFluidBtnAct);
			if (IdeoUIUtility.selected != null && !Find.IdeoManager.IdeosListForReading.Contains(IdeoUIUtility.selected))
			{
				IdeoUIUtility.selected = null;
			}
			Ideo ideo2;
			if ((ideo2 = IdeoUIUtility.selected) == null)
			{
				ideo2 = (ideo ?? (showCreateIdeoButton ? null : IdeoUIUtility.FallbackSelectedIdeo));
			}
			Ideo ideo3 = ideo2;
			Rect rect = new Rect(fillRect2.xMax, fillRect.y, fillRect.width - fillRect2.width, fillRect.height);
			if (ideo3 != null)
			{
				Rect inRect = rect.ContractedBy(17f);
				inRect.yMax += 8f;
				bool editMode2 = editMode && (onlyEditIdeo == null || onlyEditIdeo == ideo3);
				IdeoUIUtility.DoIdeoDetails(inRect, ideo3, ref scrollPosition_details, ref scrollViewHeight_details, editMode2, ideoLoadedFromFile, allowLoad, true, false, forArchonexusRestart);
				return;
			}
			if (showCreateIdeoButton)
			{
				IdeoUIUtility.DoInitialIdeoSelection(rect.ContractedBy(17f));
			}
		}

		// Token: 0x0600D554 RID: 54612 RVA: 0x003F05D0 File Offset: 0x003EE7D0
		public static void MakeLoadedIdeoPrimary(Ideo loadIdeo)
		{
			Faction.OfPlayer.ideos.SetPrimary(loadIdeo);
			Find.IdeoManager.RemoveUnusedStartingIdeos();
			IdeoUIUtility.SetSelected(loadIdeo);
			Page_ConfigureIdeo page_ConfigureIdeo = Find.WindowStack.WindowOfType<Page_ConfigureIdeo>();
			if (page_ConfigureIdeo != null)
			{
				page_ConfigureIdeo.SelectOrMakeNewIdeo(loadIdeo);
			}
		}

		// Token: 0x0600D555 RID: 54613 RVA: 0x003F0614 File Offset: 0x003EE814
		private static void DoInitialIdeoSelection(Rect fillRect)
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			Rect rect = new Rect(fillRect.center.x - IdeoUIUtility.ButtonSize.x, fillRect.center.y, IdeoUIUtility.ButtonSize.x * 2f, IdeoUIUtility.ButtonSize.y * 2f);
			if (GenFilePaths.AllCustomIdeoFiles.Any<FileInfo>())
			{
				float num = IdeoUIUtility.ButtonSize.y + 2f;
				if (Widgets.ButtonText(new Rect(fillRect.center.x - IdeoUIUtility.ButtonSize.x, fillRect.center.y + num, IdeoUIUtility.ButtonSize.x * 2f, IdeoUIUtility.ButtonSize.y * 2f), "LoadExistingIdeoligion".Translate() + "...", true, true, true, null))
				{
					Find.WindowStack.Add(new Dialog_IdeoList_Load(delegate(Ideo ideo)
					{
						Page_ConfigureIdeo page_ConfigureIdeo2 = Find.WindowStack.WindowOfType<Page_ConfigureIdeo>();
						if (page_ConfigureIdeo2 != null)
						{
							page_ConfigureIdeo2.SelectOrMakeNewIdeo(ideo);
						}
					}));
				}
				rect.y -= num;
			}
			if (Widgets.ButtonText(rect, "CreateCustomIdeoligion".Translate() + "...", true, true, true, null))
			{
				Page_ConfigureIdeo page_ConfigureIdeo = Find.WindowStack.WindowOfType<Page_ConfigureIdeo>();
				if (page_ConfigureIdeo != null)
				{
					page_ConfigureIdeo.SelectOrMakeNewIdeo(IdeoUIUtility.selected);
				}
			}
			Widgets.Label(new Rect(fillRect.x + 26f, rect.y - Text.LineHeight, fillRect.width - 52f, Text.LineHeight), "SelectAnIdeo".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0600D556 RID: 54614 RVA: 0x003F07D0 File Offset: 0x003EE9D0
		public static void DoIdeoList(Rect fillRect, ref Vector2 scrollPosition, ref float scrollViewHeight, out Ideo mouseoverIdeo, bool showCreateNewButton, List<Pawn> pawns = null, Action createCustomBtnActOverride = null, bool forArchonexusRestart = false, Func<Pawn, Ideo> pawnIdeoGetter = null, bool showLoadExistingIdeoBtn = false, Action createFluidBtnAct = null)
		{
			IdeoUIUtility.<>c__DisplayClass61_0 CS$<>8__locals1 = new IdeoUIUtility.<>c__DisplayClass61_0();
			CS$<>8__locals1.fillRect = fillRect;
			mouseoverIdeo = null;
			CS$<>8__locals1.gameStartConfig = Find.WindowStack.WindowOfType<Page_ConfigureIdeo>();
			bool flag = CS$<>8__locals1.gameStartConfig is Page_ConfigureFluidIdeo;
			Widgets.BeginGroup(CS$<>8__locals1.fillRect);
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			Rect outRect = CS$<>8__locals1.fillRect.AtZero();
			outRect.yMin += 17f;
			IdeoUIUtility.<>c__DisplayClass61_1 CS$<>8__locals2;
			CS$<>8__locals2.viewRect = new Rect(0f, 0f, CS$<>8__locals1.fillRect.width - 16f, scrollViewHeight);
			Widgets.BeginScrollView(outRect, ref scrollPosition, CS$<>8__locals2.viewRect, true);
			IdeoUIUtility.<>c__DisplayClass61_2 CS$<>8__locals3;
			CS$<>8__locals3.curY = 0f;
			int num = 0;
			if (CS$<>8__locals1.gameStartConfig != null)
			{
				CS$<>8__locals1.<DoIdeoList>g__DrawIdeoligionSectionHeader|0("CustomIdeoligionSectionHeader".Translate(), ref CS$<>8__locals2, ref CS$<>8__locals3);
			}
			if (showCreateNewButton)
			{
				if (Widgets.ButtonText(new Rect(0f, CS$<>8__locals3.curY + 5f, CS$<>8__locals2.viewRect.width, 36f), (forArchonexusRestart ? "CreateNew" : "CreateCustom").Translate() + "...", true, true, true, null))
				{
					if (createCustomBtnActOverride != null)
					{
						createCustomBtnActOverride();
					}
					else if (CS$<>8__locals1.gameStartConfig != null)
					{
						CS$<>8__locals1.gameStartConfig.SelectOrMakeNewIdeo(null);
						IdeoUIUtility.selected = CS$<>8__locals1.gameStartConfig.ideo;
					}
				}
				CS$<>8__locals3.curY += 46f;
				num++;
				if (forArchonexusRestart && createFluidBtnAct != null)
				{
					if (Widgets.ButtonText(new Rect(0f, CS$<>8__locals3.curY, CS$<>8__locals2.viewRect.width, 36f), "CreateFluid".Translate(), true, true, true, null))
					{
						createFluidBtnAct();
					}
					CS$<>8__locals3.curY += 46f;
					num++;
				}
				if (showLoadExistingIdeoBtn && GenFilePaths.AllCustomIdeoFiles.Any<FileInfo>())
				{
					if (Widgets.ButtonText(new Rect(0f, CS$<>8__locals3.curY, CS$<>8__locals2.viewRect.width, 36f), "LoadExisting".Translate() + "...", true, true, true, null))
					{
						Find.WindowStack.Add(new Dialog_IdeoList_Load(delegate(Ideo ideo)
						{
							if (CS$<>8__locals1.gameStartConfig != null)
							{
								CS$<>8__locals1.gameStartConfig.SelectOrMakeNewIdeo(ideo);
							}
						}));
					}
					CS$<>8__locals3.curY += 46f;
					num++;
				}
			}
			if (CS$<>8__locals1.gameStartConfig != null && !IdeoUIUtility.PlayerPrimaryIdeoNotShared)
			{
				CS$<>8__locals1.<DoIdeoList>g__DrawIdeoligionSectionHeader|0("FactionIdeoligionSectionHeader".Translate(), ref CS$<>8__locals2, ref CS$<>8__locals3);
			}
			foreach (Ideo ideo2 in Find.IdeoManager.IdeosInViewOrder)
			{
				if (pawns != null)
				{
					IdeoUIUtility.tmpPawns.Clear();
					for (int i = 0; i < pawns.Count; i++)
					{
						if (((pawnIdeoGetter != null) ? pawnIdeoGetter(pawns[i]) : pawns[i].Ideo) == ideo2)
						{
							IdeoUIUtility.tmpPawns.Add(pawns[i]);
						}
					}
				}
				bool flag2;
				IdeoUIUtility.DrawIdeoRow(ideo2, ref CS$<>8__locals3.curY, CS$<>8__locals2.viewRect, out flag2, num, IdeoUIUtility.tmpPawns);
				if (flag2)
				{
					mouseoverIdeo = ideo2;
				}
				num++;
				if (CS$<>8__locals1.gameStartConfig != null && IdeoUIUtility.PlayerPrimaryIdeoNotShared && ideo2 == Faction.OfPlayer.ideos.PrimaryIdeo)
				{
					if (!flag)
					{
						CS$<>8__locals3.curY += 4f;
						if (Widgets.ButtonText(new Rect(0f, CS$<>8__locals3.curY, CS$<>8__locals2.viewRect.width, 36f), "DeleteCustom".Translate(), true, true, true, null))
						{
							Ideo removeCustomIdeo = Faction.OfPlayer.ideos.PrimaryIdeo;
							Faction.OfPlayer.ideos.SetPrimary(Find.IdeoManager.IdeosInViewOrder.First((Ideo newIdeo) => newIdeo != removeCustomIdeo));
							IdeoUIUtility.selected = Faction.OfPlayer.ideos.PrimaryIdeo;
							Page_ConfigureIdeo gameStartConfig = CS$<>8__locals1.gameStartConfig;
							if (gameStartConfig != null)
							{
								gameStartConfig.SelectOrMakeNewIdeo(IdeoUIUtility.selected);
							}
							Find.IdeoManager.Remove(removeCustomIdeo);
						}
						CS$<>8__locals3.curY += 46f;
					}
					else
					{
						CS$<>8__locals3.curY += 17f;
					}
					CS$<>8__locals1.<DoIdeoList>g__DrawIdeoligionSectionHeader|0("FactionIdeoligionSectionHeader".Translate(), ref CS$<>8__locals2, ref CS$<>8__locals3);
				}
			}
			if (Event.current.type == EventType.Layout)
			{
				scrollViewHeight = CS$<>8__locals3.curY;
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
			IdeoUIUtility.tmpPawns.Clear();
		}

		// Token: 0x0600D557 RID: 54615 RVA: 0x003F0CEC File Offset: 0x003EEEEC
		public static void DoIdeoSaveLoad(ref float curY, float width, Ideo ideo, Action<Ideo> loadIdeo, bool allowLoad = true, bool allowSave = true, bool forArchonexusRestart = false)
		{
			float num = width / 2f;
			float num2 = Mathf.Max(width - (width - IdeoUIUtility.PreceptBoxSize.x * 3f - 16f) / 2f - IdeoUIUtility.SaveLoadButtonSize.x, num + 10f);
			curY += 4f;
			float num3 = num2;
			if (allowSave)
			{
				Rect rect = new Rect(num3, curY, IdeoUIUtility.SaveLoadButtonSize.x, IdeoUIUtility.SaveLoadButtonSize.y);
				num3 -= IdeoUIUtility.SaveLoadButtonSize.x + 4f;
				if (Widgets.ButtonText(rect, "Save".Translate(), true, true, true, null))
				{
					Find.WindowStack.Add(new Dialog_IdeoList_Save(ideo));
				}
			}
			if (allowLoad)
			{
				Rect rect2 = new Rect(num3, curY, IdeoUIUtility.SaveLoadButtonSize.x, IdeoUIUtility.SaveLoadButtonSize.y);
				num3 -= IdeoUIUtility.SaveLoadButtonSize.x;
				if (Widgets.ButtonText(rect2, "Load".Translate(), true, true, true, null))
				{
					if (forArchonexusRestart)
					{
						Find.WindowStack.Add(new Dialog_IdeoList_Load(loadIdeo));
					}
					else
					{
						Find.WindowStack.Add(new Dialog_IdeoList_Load(loadIdeo, Find.IdeoManager.GetFactionsWithIdeo(ideo, true, true), IdeoUIUtility.DevEditMode));
					}
				}
			}
			curY += IdeoUIUtility.SaveLoadButtonSize.y + 4f;
		}

		// Token: 0x0600D558 RID: 54616 RVA: 0x003F0E48 File Offset: 0x003EF048
		private static void DrawIdeoRow(Ideo ideo, ref float curY, Rect fillRect, out bool mouseover, int row, List<Pawn> pawns = null)
		{
			Rect rect = new Rect(44f, curY, 0f, 46f);
			rect.width = fillRect.width - rect.x;
			Rect rect2 = new Rect(7f, curY + 7f, 30f, 30f);
			float num = pawns.NullOrEmpty<Pawn>() ? 0f : 32f;
			float height = 46f + num;
			Rect rect3 = new Rect(0f, curY, fillRect.width, height);
			if (row % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect3);
			}
			if (ideo == IdeoUIUtility.selected)
			{
				Widgets.DrawHighlightSelected(rect3);
			}
			else
			{
				Widgets.DrawHighlightIfMouseover(rect3);
			}
			Page_ConfigureIdeo page_ConfigureIdeo = Find.WindowStack.WindowOfType<Page_ConfigureIdeo>();
			bool flag = page_ConfigureIdeo is Page_ConfigureFluidIdeo;
			Text.Font = GameFont.Small;
			IdeoUIUtility.DoIdeoIcon(rect2, ideo, true, null);
			Rect rect4 = rect;
			rect4.y += 3f;
			Widgets.Label(rect4, ideo.name.Truncate(rect4.width, null));
			float num2 = rect.y + rect.height / 2f - 2f;
			IdeoUIUtility.DoFactionIcons(ideo, rect.x, ref num2, rect.width, 18f, 18f, null, Find.WindowStack.WindowOfType<Page_ConfigureIdeo>() == null, page_ConfigureIdeo != null && !flag);
			curY += 46f;
			if (pawns != null && pawns.Count > 0)
			{
				num2 = curY;
				IdeoUIUtility.DoPawnIcons(pawns, rect2.x, ref num2, rect.width, 22f);
				curY += num;
			}
			mouseover = Mouse.IsOver(rect3);
			if (IdeoUIUtility.selected != ideo && Widgets.ButtonInvisible(rect3, true) && TutorSystem.AllowAction("ConfiguringIdeo"))
			{
				IdeoUIUtility.selected = ideo;
				SoundDefOf.DialogBoxAppear.PlayOneShotOnCamera(null);
				if (page_ConfigureIdeo != null && !flag)
				{
					page_ConfigureIdeo.SelectOrMakeNewIdeo(IdeoUIUtility.selected);
				}
			}
		}

		// Token: 0x0600D559 RID: 54617 RVA: 0x003F103C File Offset: 0x003EF23C
		public static void DoIdeoDetails(Rect inRect, Ideo ideo, ref Vector2 scrollPosition, ref float viewHeight, bool editMode = false, Action<Ideo> ideoLoadedFromFile = null, bool allowLoad = true, bool allowSave = true, bool reform = false, bool forArchonexusRestart = false)
		{
			if (!ModLister.CheckIdeology("Ideoligion"))
			{
				return;
			}
			IdeoEditMode ideoEditMode;
			if (IdeoUIUtility.DevEditMode)
			{
				ideoEditMode = IdeoEditMode.Dev;
			}
			else if (editMode)
			{
				ideoEditMode = (reform ? IdeoEditMode.Reform : IdeoEditMode.GameStart);
			}
			else
			{
				ideoEditMode = IdeoEditMode.None;
			}
			if (ideo.createdFromNoExpansionGame && ideoEditMode == IdeoEditMode.None && !ideo.memes.Any<MemeDef>())
			{
				Widgets.NoneLabelCenteredVertically(inRect, null);
				return;
			}
			Rect viewRect = new Rect(0f, 0f, inRect.width - 16f, viewHeight);
			Widgets.BeginScrollView(inRect, ref scrollPosition, viewRect, true);
			float num = 0f;
			float b = 0f;
			if (allowLoad || allowSave)
			{
				IdeoUIUtility.DoIdeoSaveLoad(ref num, viewRect.width, ideo, delegate(Ideo loadedIdeo)
				{
					if (ideoLoadedFromFile != null)
					{
						ideoLoadedFromFile(loadedIdeo);
						return;
					}
					foreach (Faction faction in Find.IdeoManager.GetFactionsWithIdeo(ideo, true, true))
					{
						faction.ideos.SetPrimary(loadedIdeo);
					}
					IdeoUIUtility.MakeLoadedIdeoPrimary(loadedIdeo);
				}, allowLoad, allowSave, forArchonexusRestart);
			}
			b = num;
			IdeoUIUtility.DoName(ref num, viewRect.width, ideo, ideoEditMode);
			IdeoUIUtility.DoFactions(ref b, viewRect.width, ideo, ideoEditMode);
			if (ideo.Fluid)
			{
				IdeoUIUtility.DoFluidIdeo(ref num, viewRect.width, ideo, ideoEditMode);
			}
			num += 34f;
			num = Mathf.Max(num, b);
			bool flag = false;
			using (List<MemeDef>.Enumerator enumerator = ideo.memes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.category != MemeCategory.Structure)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag || editMode)
			{
				IdeoUIUtility.DoMemes(ref num, viewRect.width, ideo, ideoEditMode);
			}
			IdeoUIUtility.DoDescription(ref num, viewRect.width, ideo, ideoEditMode);
			if (ideo.foundation != null)
			{
				IdeoUIUtility.DoFoundationInfo(ref num, viewRect.width, ideo, ideoEditMode);
			}
			IdeoUIUtility.tmpMouseOverPrecept = null;
			IdeoUIUtility.DoPrecepts(ref num, viewRect.width, ideo, ideoEditMode);
			IdeoUIUtility.DoAppearanceItems(ideo, ideoEditMode, ref num, viewRect.width);
			if (Prefs.DevMode)
			{
				IdeoUIUtility.DoDebugButtons(ref num, viewRect.width, ideo);
			}
			if (Event.current.type == EventType.Layout)
			{
				viewHeight = num + inRect.height / 2f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x0600D55A RID: 54618 RVA: 0x003F128C File Offset: 0x003EF48C
		public static void DoName(ref float curY, float width, Ideo ideo, IdeoEditMode editMode)
		{
			IdeoUIUtility.<>c__DisplayClass68_0 CS$<>8__locals1 = new IdeoUIUtility.<>c__DisplayClass68_0();
			CS$<>8__locals1.ideo = ideo;
			float b = (editMode != IdeoEditMode.None) ? 220f : 275f;
			Text.Font = GameFont.Medium;
			float num = Text.CalcSize(CS$<>8__locals1.ideo.name).x;
			Text.Font = GameFont.Small;
			string text = CS$<>8__locals1.ideo.adjective.CapitalizeFirst() + " / " + CS$<>8__locals1.ideo.memberName.CapitalizeFirst();
			num = Mathf.Max(num, Text.CalcSize(text).x);
			num = Mathf.Min(num, b);
			float num2 = 87f + num;
			float x = (width - IdeoUIUtility.PreceptBoxSize.x * 3f - 16f) / 2f;
			float num3 = width - (width - IdeoUIUtility.PreceptBoxSize.x * 3f - 16f) / 2f;
			float num4 = (width - num2) / 2f;
			MemeDef structureMeme = CS$<>8__locals1.ideo.StructureMeme;
			Rect rect = new Rect(x, curY, 35f, 35f);
			if (structureMeme != null)
			{
				GUI.DrawTexture(rect, structureMeme.Icon);
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
					TooltipHandler.TipRegion(rect, IdeoUIUtility.StructureTooltip(structureMeme, editMode));
				}
				if (editMode != IdeoEditMode.None && Widgets.ButtonInvisible(rect, true) && IdeoUIUtility.TutorAllowsInteraction(editMode))
				{
					Find.WindowStack.Add(new Dialog_ChooseMemes(CS$<>8__locals1.ideo, MemeCategory.Structure, false, null, null, false));
				}
			}
			Rect rect2 = new Rect(rect.xMax + 4f, curY, 35f, 35f);
			if (CS$<>8__locals1.ideo.culture != null)
			{
				GUI.color = CS$<>8__locals1.ideo.culture.iconColor;
				GUI.DrawTexture(rect2, CS$<>8__locals1.ideo.culture.Icon);
				GUI.color = Color.white;
				if (editMode != IdeoEditMode.None && Widgets.ButtonInvisible(rect2, true) && IdeoUIUtility.TutorAllowsInteraction(editMode))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (CultureDef culture2 in from c in DefDatabase<CultureDef>.AllDefs
					orderby c.label
					select c)
					{
						CultureDef culture = culture2;
						string label = culture.LabelCap;
						list.Add(new FloatMenuOption(label, delegate()
						{
							if (CS$<>8__locals1.ideo.culture != culture)
							{
								CS$<>8__locals1.ideo.culture = culture;
								CS$<>8__locals1.ideo.foundation.RandomizeStyles();
								CS$<>8__locals1.ideo.style.RecalculateAvailableStyleItems();
								IdeoFoundation_Deity ideoFoundation_Deity = CS$<>8__locals1.ideo.foundation as IdeoFoundation_Deity;
								if (ideoFoundation_Deity != null)
								{
									ideoFoundation_Deity.GenerateDeities();
								}
								CS$<>8__locals1.ideo.RegenerateDescription(true);
							}
						}, culture.Icon, culture.iconColor, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
					}
					Find.WindowStack.Add(new FloatMenu(list, "ChooseCulture".Translate(), false));
				}
			}
			if (CS$<>8__locals1.ideo.culture != null && Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
				TaggedString str = string.Concat(new string[]
				{
					("Culture".Translate() + ": " + CS$<>8__locals1.ideo.culture.LabelCap).Colorize(ColoredText.TipSectionTitleColor),
					"\n\n",
					CS$<>8__locals1.ideo.culture.description,
					"\n\n",
					"CultureTip".Translate().Resolve().Colorize(ColoredText.SubtleGrayColor),
					(editMode != IdeoEditMode.None) ? ("\n\n" + IdeoUIUtility.ClickToEdit) : string.Empty
				});
				TooltipHandler.TipRegion(rect2, str);
			}
			float num5 = rect.xMin;
			float num6 = rect.yMax + 10f;
			IdeoUIUtility.DoStyles(ref num6, ref num5, width, CS$<>8__locals1.ideo, editMode, 28);
			float yMax = rect.yMax;
			if (Current.ProgramState == ProgramState.Entry)
			{
				if (IdeoUIUtility.currentRitualAmbiencePreviewIdeo != CS$<>8__locals1.ideo || (IdeoUIUtility.currentRitualAmbiencePreview != null && IdeoUIUtility.currentRitualAmbiencePreview.Ended))
				{
					IdeoUIUtility.currentRitualAmbiencePreview = null;
				}
				if (CS$<>8__locals1.ideo.SoundOngoingRitual != null)
				{
					num5 += 10f;
					Rect rect3 = new Rect(num5, rect.yMax + 10f, 32f, 32f);
					TooltipHandler.TipRegion(rect3, () => "TipPreviewRitualAmbienceSound".Translate(), 45834593);
					if (Widgets.ButtonImage(rect3, (IdeoUIUtility.currentRitualAmbiencePreview != null) ? IdeoUIUtility.PreviewRitualAmbienceOn : IdeoUIUtility.PreviewRitualAmbience, true, null))
					{
						if (IdeoUIUtility.currentRitualAmbiencePreview == null)
						{
							CS$<>8__locals1.<DoName>g__StartPlay|0();
						}
						else
						{
							IdeoUIUtility.currentRitualAmbiencePreview = null;
						}
					}
					num5 += 32f;
				}
				if (IdeoUIUtility.currentRitualAmbiencePreview != null)
				{
					if (IdeoUIUtility.currentRitualAmbiencePreview.def != CS$<>8__locals1.ideo.SoundOngoingRitual)
					{
						CS$<>8__locals1.<DoName>g__StartPlay|0();
					}
					IdeoUIUtility.currentRitualAmbiencePreview.Maintain();
					Find.MusicManagerEntry.MaintainSilence();
				}
			}
			float num7 = curY;
			IdeoUIUtility.DoNameAndSymbol(ref curY, width, CS$<>8__locals1.ideo, editMode);
			if (editMode != IdeoEditMode.None)
			{
				float x2 = Mathf.Max(num3 - IdeoUIUtility.AddPreceptButtonSize.x, num4 + num2 + 10f);
				Rect rect4 = new Rect(x2, num7, IdeoUIUtility.AddPreceptButtonSize.x, IdeoUIUtility.AddPreceptButtonSize.y);
				TooltipHandler.TipRegion(rect4, "RandomizeSymbolsTooltip".Translate());
				if (Widgets.ButtonText(rect4, "RandomizeSymbols".Translate(), true, true, true, null) && IdeoUIUtility.TutorAllowsInteraction(editMode))
				{
					CS$<>8__locals1.ideo.MakeMemeberNamePluralDirty();
					CS$<>8__locals1.ideo.foundation.RandomizePlace();
					CS$<>8__locals1.ideo.foundation.GenerateTextSymbols();
					CS$<>8__locals1.ideo.foundation.GenerateLeaderTitle();
					CS$<>8__locals1.ideo.foundation.RandomizeIcon();
					CS$<>8__locals1.ideo.RegenerateAllPreceptNames();
					CS$<>8__locals1.ideo.RegenerateDescription(true);
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				GUI.color = ColoredText.SubtleGrayColor;
				Text.Anchor = TextAnchor.UpperCenter;
				string text2 = "ClickAnyElementToEditIt".Translate();
				float height = Text.CalcHeight(text2, IdeoUIUtility.AddPreceptButtonSize.x);
				Widgets.Label(new Rect(x2, num7 + IdeoUIUtility.AddPreceptButtonSize.y + 4f, IdeoUIUtility.AddPreceptButtonSize.x, height), text2);
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
			}
		}

		// Token: 0x0600D55B RID: 54619 RVA: 0x003F18F8 File Offset: 0x003EFAF8
		public static void DoNameAndSymbol(ref float curY, float width, Ideo ideo, IdeoEditMode editMode)
		{
			float b = (editMode != IdeoEditMode.None) ? 220f : 275f;
			Text.Font = GameFont.Medium;
			float num = Text.CalcSize(ideo.name).x;
			Text.Font = GameFont.Small;
			string text = ideo.adjective.CapitalizeFirst() + " / " + ideo.memberName.CapitalizeFirst();
			num = Mathf.Max(num, Text.CalcSize(text).x);
			num = Mathf.Min(num, b);
			float num2 = 87f + num;
			float num3 = (width - IdeoUIUtility.PreceptBoxSize.x * 3f - 16f) / 2f;
			float num4 = (width - IdeoUIUtility.PreceptBoxSize.x * 3f - 16f) / 2f;
			float x = (width - num2) / 2f;
			Rect rect = new Rect(x, curY, 70f, 70f);
			GUI.color = ideo.Color;
			GUI.DrawTexture(rect, ideo.Icon);
			GUI.color = Color.white;
			if (editMode != IdeoEditMode.None && Widgets.ButtonInvisible(rect, true) && IdeoUIUtility.TutorAllowsInteraction(editMode))
			{
				Find.WindowStack.Add(new Dialog_ChooseIdeoSymbols(ideo));
			}
			float num5 = curY;
			Rect rect2 = new Rect(rect.xMax + 17f, num5 + 5f, 999f, 30f);
			Text.Font = GameFont.Medium;
			Widgets.Label(rect2, ideo.name.Truncate(num, null));
			Text.Font = GameFont.Small;
			num5 += 39f;
			GUI.color = new Color(0.65f, 0.65f, 0.65f);
			Widgets.Label(new Rect(rect.xMax + 17f, num5, num, 30f), text.Truncate(num, null));
			num5 += 18f;
			GUI.color = Color.white;
			Rect rect3 = new Rect(rect.xMin, rect.yMin, num + 70f + 17f + 4f, 70f);
			if (Mouse.IsOver(rect3))
			{
				Widgets.DrawHighlight(rect3);
				string str = string.Concat(new string[]
				{
					("Name".Translate() + ": ").Colorize(ColoredText.TipSectionTitleColor),
					ideo.name,
					"\n",
					("Adjective".Translate() + ": ").Colorize(ColoredText.TipSectionTitleColor),
					ideo.adjective.CapitalizeFirst(),
					"\n",
					("IdeoMembers".Translate() + ": ").Colorize(ColoredText.TipSectionTitleColor),
					ideo.memberName.CapitalizeFirst(),
					"\n",
					("LeaderTitle".Translate() + ": ").Colorize(ColoredText.TipSectionTitleColor),
					ideo.leaderTitleMale.CapitalizeFirst(),
					"\n",
					("WorshipRoom".Translate() + ": ").Colorize(ColoredText.TipSectionTitleColor),
					ideo.WorshipRoomLabel.CapitalizeFirst(),
					"\n",
					(ideo.leaderTitleFemale != ideo.leaderTitleMale) ? (" (" + ideo.leaderTitleFemale.CapitalizeFirst() + ")") : "",
					(editMode != IdeoEditMode.None) ? ("\n" + IdeoUIUtility.ClickToEdit) : string.Empty
				});
				TooltipHandler.TipRegion(rect3, str);
			}
			if (editMode != IdeoEditMode.None && Widgets.ButtonInvisible(rect3, true) && IdeoUIUtility.TutorAllowsInteraction(editMode))
			{
				Find.WindowStack.Add(new Dialog_ChooseIdeoSymbols(ideo));
			}
			curY += 70f;
			curY += 10f;
		}

		// Token: 0x0600D55C RID: 54620 RVA: 0x003F1CBC File Offset: 0x003EFEBC
		public static void DoStyles(ref float curY, ref float curX, float width, Ideo ideo, IdeoEditMode editMode, int styleIconSize = 28)
		{
			IdeoUIUtility.tmpStyleCategories.Clear();
			IdeoUIUtility.tmpStyleCategories.AddRange(ideo.thingStyleCategories);
			Func<StyleCategoryDef, bool> <>9__1;
			Func<StyleCategoryDef, bool> <>9__4;
			for (int i = 0; i < 3; i++)
			{
				Rect rect = new Rect(curX, curY, (float)styleIconSize, (float)styleIconSize);
				curX += (float)styleIconSize;
				int index = i;
				if (i < IdeoUIUtility.tmpStyleCategories.Count)
				{
					GUI.DrawTexture(rect.ContractedBy(4f), IdeoUIUtility.tmpStyleCategories[i].category.Icon);
					if (Mouse.IsOver(rect))
					{
						Widgets.DrawHighlight(rect);
						TooltipHandler.TipRegion(rect, IdeoUIUtility.StyleTooltip(IdeoUIUtility.tmpStyleCategories[i].category, editMode, ideo, IdeoUIUtility.tmpStyleCategories, false));
					}
					if (editMode != IdeoEditMode.None && Widgets.ButtonInvisible(rect, true) && IdeoUIUtility.TutorAllowsInteraction(editMode))
					{
						List<FloatMenuOption> list = new List<FloatMenuOption>();
						list.Add(new FloatMenuOption("Remove".Translate(), delegate()
						{
							ideo.thingStyleCategories.RemoveAt(index);
							ideo.SortStyleCategories();
							ideo.style.ResetStylesForThingDef();
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						IEnumerable<StyleCategoryDef> allDefs = DefDatabase<StyleCategoryDef>.AllDefs;
						Func<StyleCategoryDef, bool> predicate;
						if ((predicate = <>9__1) == null)
						{
							predicate = (<>9__1 = ((StyleCategoryDef x) => (!x.fixedIdeoOnly || editMode == IdeoEditMode.Dev) && !ideo.thingStyleCategories.Any((ThingStyleCategoryWithPriority y) => y.category == x)));
						}
						using (IEnumerator<StyleCategoryDef> enumerator = allDefs.Where(predicate).GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								StyleCategoryDef s = enumerator.Current;
								list.Add(new FloatMenuOption(s.LabelCap, delegate()
								{
									ideo.thingStyleCategories.RemoveAt(index);
									ideo.thingStyleCategories.Insert(index, new ThingStyleCategoryWithPriority(s, (float)(3 - index)));
									ideo.SortStyleCategories();
									ideo.style.ResetStylesForThingDef();
								}, s.Icon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
							}
						}
						Find.WindowStack.Add(new FloatMenu(list));
					}
				}
				else if (editMode != IdeoEditMode.None)
				{
					GUI.DrawTexture(rect.ContractedBy(4f), IdeoUIUtility.PlusTex);
					if (Mouse.IsOver(rect))
					{
						Widgets.DrawHighlight(rect);
						TooltipHandler.TipRegion(rect, "AddStyleCategory".Translate() + "\n\n" + "StyleCategoryDescription".Translate(ideo.Named("IDEO")).Resolve().Colorize(ColoredText.SubtleGrayColor) + "\n\n" + IdeoUIUtility.ClickToEdit);
					}
					if (!Widgets.ButtonInvisible(rect, true) || !IdeoUIUtility.TutorAllowsInteraction(editMode))
					{
						break;
					}
					List<FloatMenuOption> list2 = new List<FloatMenuOption>();
					IEnumerable<StyleCategoryDef> allDefs2 = DefDatabase<StyleCategoryDef>.AllDefs;
					Func<StyleCategoryDef, bool> predicate2;
					if ((predicate2 = <>9__4) == null)
					{
						predicate2 = (<>9__4 = ((StyleCategoryDef x) => (!x.fixedIdeoOnly || editMode == IdeoEditMode.Dev) && !ideo.thingStyleCategories.Any((ThingStyleCategoryWithPriority y) => y.category == x)));
					}
					using (IEnumerator<StyleCategoryDef> enumerator = allDefs2.Where(predicate2).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							StyleCategoryDef s = enumerator.Current;
							list2.Add(new FloatMenuOption(s.LabelCap, delegate()
							{
								ideo.thingStyleCategories.Add(new ThingStyleCategoryWithPriority(s, (float)(3 - index)));
								ideo.SortStyleCategories();
								ideo.style.ResetStylesForThingDef();
							}, s.Icon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
						}
					}
					if (list2.Any<FloatMenuOption>())
					{
						Find.WindowStack.Add(new FloatMenu(list2));
						return;
					}
					break;
				}
			}
		}

		// Token: 0x0600D55D RID: 54621 RVA: 0x003F20B8 File Offset: 0x003F02B8
		public static TaggedString StructureTooltip(MemeDef structureMeme, IdeoEditMode editMode)
		{
			return string.Concat(new string[]
			{
				("StructureMeme".Translate() + ": " + structureMeme.LabelCap).Colorize(ColoredText.TipSectionTitleColor),
				"\n\n",
				structureMeme.description,
				"\n\n",
				"StructureMemeTip".Translate().Resolve().Colorize(ColoredText.SubtleGrayColor),
				(editMode != IdeoEditMode.None) ? ("\n\n" + IdeoUIUtility.ClickToEdit) : string.Empty
			});
		}

		// Token: 0x0600D55E RID: 54622 RVA: 0x003F2158 File Offset: 0x003F0358
		public static TaggedString StyleTooltip(StyleCategoryDef cat, IdeoEditMode editMode, Ideo ideo, List<ThingStyleCategoryWithPriority> selectedStyles, bool skipDominanceDesc = false)
		{
			string text = "\n\n" + ((ideo != null && !ideo.classicMode) ? "StyleCategoryDescStyleDominance".Translate(ideo.Named("IDEO")) : "StyleCategoryDescStyleDominanceAbstract".Translate()).Resolve();
			if (skipDominanceDesc)
			{
				text = string.Empty;
			}
			return string.Concat(new string[]
			{
				("StyleCategory".Translate() + ": " + cat.LabelCap).Colorize(ColoredText.TipSectionTitleColor),
				text,
				"\n\n",
				IdeoUIUtility.GetStyleCategoryDescription(cat, ideo, selectedStyles ?? IdeoUIUtility.tmpStyleCategories),
				(editMode != IdeoEditMode.None) ? ("\n\n" + IdeoUIUtility.ClickToEdit) : string.Empty,
				"\n\n",
				((ideo != null) ? "StyleCategoryDescription".Translate(ideo.Named("IDEO")) : "StyleCategoryDescriptionAbstract".Translate()).Resolve().Colorize(ColoredText.SubtleGrayColor)
			});
		}

		// Token: 0x0600D55F RID: 54623 RVA: 0x003F2268 File Offset: 0x003F0468
		public static string GetStyleCategoryDescription(StyleCategoryDef cat, Ideo ideo, List<ThingStyleCategoryWithPriority> selectedStyles)
		{
			IdeoUIUtility.<>c__DisplayClass73_0 CS$<>8__locals1;
			CS$<>8__locals1.cat = cat;
			IdeoUIUtility.tmpOverrides.Clear();
			CS$<>8__locals1.desc = "";
			if (!CS$<>8__locals1.cat.addDesignators.NullOrEmpty<BuildableDef>() || !CS$<>8__locals1.cat.addDesignatorGroups.NullOrEmpty<DesignatorDropdownGroupDef>())
			{
				CS$<>8__locals1.desc += ("IdeoMakesBuildingBuildable".Translate() + ": ").Colorize(ColoredText.TipSectionTitleColor);
				CS$<>8__locals1.desc += IdeoUIUtility.SortedLabelCaps<BuildableDef, DesignatorDropdownGroupDef>(CS$<>8__locals1.cat.addDesignators, CS$<>8__locals1.cat.addDesignatorGroups, "\n  - ");
			}
			CS$<>8__locals1.showedDetails = false;
			for (int i = 0; i < CS$<>8__locals1.cat.thingDefStyles.Count; i++)
			{
				ThingDef thingDef = CS$<>8__locals1.cat.thingDefStyles[i].ThingDef;
				if (thingDef.canGenerateDefaultDesignator || (!CS$<>8__locals1.cat.addDesignators.NullOrEmpty<BuildableDef>() && CS$<>8__locals1.cat.addDesignators.Contains(thingDef)))
				{
					bool flag = false;
					int num = 0;
					Predicate<ThingDefStyle> <>9__1;
					while (num < selectedStyles.Count && selectedStyles[num].category != CS$<>8__locals1.cat)
					{
						List<ThingDefStyle> thingDefStyles = selectedStyles[num].category.thingDefStyles;
						Predicate<ThingDefStyle> predicate;
						if ((predicate = <>9__1) == null)
						{
							predicate = (<>9__1 = ((ThingDefStyle s) => s.ThingDef == thingDef));
						}
						if (thingDefStyles.Any(predicate))
						{
							IdeoUIUtility.tmpOverrides.Add(new IdeoUIUtility.StyleCatOverride(thingDef.LabelCap, selectedStyles[num].category));
							flag = true;
							break;
						}
						num++;
					}
					if (!flag)
					{
						if (!CS$<>8__locals1.showedDetails)
						{
							IdeoUIUtility.<GetStyleCategoryDescription>g__AppendDetails|73_0(ref CS$<>8__locals1);
						}
						CS$<>8__locals1.desc = CS$<>8__locals1.desc + "\n  - " + thingDef.LabelCap.Resolve();
					}
				}
			}
			if (ideo != null)
			{
				SoundDef soundOngoingRitual = ideo.SoundOngoingRitual;
				if (CS$<>8__locals1.cat.soundOngoingRitual != null)
				{
					if (soundOngoingRitual == CS$<>8__locals1.cat.soundOngoingRitual)
					{
						if (!CS$<>8__locals1.showedDetails)
						{
							IdeoUIUtility.<GetStyleCategoryDescription>g__AppendDetails|73_0(ref CS$<>8__locals1);
						}
						CS$<>8__locals1.desc = CS$<>8__locals1.desc + "\n  - " + "RitualAmbienceSound".Translate().Resolve();
					}
					else
					{
						StyleCategoryDef style = null;
						foreach (ThingStyleCategoryWithPriority thingStyleCategoryWithPriority in ideo.thingStyleCategories)
						{
							if (thingStyleCategoryWithPriority.category.soundOngoingRitual == soundOngoingRitual)
							{
								style = thingStyleCategoryWithPriority.category;
								break;
							}
						}
						IdeoUIUtility.tmpOverrides.Add(new IdeoUIUtility.StyleCatOverride("RitualAmbienceSound".Translate(), style));
					}
				}
			}
			if (IdeoUIUtility.tmpOverrides.Count > 0)
			{
				IdeoUIUtility.tmpOverrides.SortBy((IdeoUIUtility.StyleCatOverride ot) => ot.style.index);
				int num2 = -1;
				foreach (IdeoUIUtility.StyleCatOverride styleCatOverride in IdeoUIUtility.tmpOverrides)
				{
					if (num2 == -1 || num2 != (int)styleCatOverride.style.index)
					{
						if (!CS$<>8__locals1.desc.NullOrEmpty())
						{
							CS$<>8__locals1.desc += "\n\n";
						}
						CS$<>8__locals1.desc += ("OverriddenByStyle".Translate(styleCatOverride.style.LabelCap) + ": ").Colorize(ColoredText.SubtleGrayColor);
						num2 = (int)styleCatOverride.style.index;
					}
					CS$<>8__locals1.desc += ("\n  - " + styleCatOverride.label).Colorize(ColoredText.SubtleGrayColor);
				}
			}
			return CS$<>8__locals1.desc;
		}

		// Token: 0x0600D560 RID: 54624 RVA: 0x003F269C File Offset: 0x003F089C
		private static void DoFactions(ref float curY, float width, Ideo ideo, IdeoEditMode editMode)
		{
			if (Find.World == null)
			{
				return;
			}
			if (Find.WindowStack.WindowOfType<Page_ConfigureIdeo>() != null)
			{
				return;
			}
			float startX = width - 150f;
			if (editMode == IdeoEditMode.Dev)
			{
				curY += 60f;
			}
			IdeoUIUtility.DoFactionIcons(ideo, startX, ref curY, 9999f, 30f, 20f, "IdeoligionOf".Translate() + ":", false, false);
		}

		// Token: 0x0600D561 RID: 54625 RVA: 0x003F2708 File Offset: 0x003F0908
		private static void DoFluidIdeo(ref float curY, float width, Ideo ideo, IdeoEditMode editMode)
		{
			string text = ideo.development.Points.ToString() + " / " + ideo.development.NextReformationDevelopmentPoints.ToString();
			float x = Text.CalcSize(text).x;
			float x2 = (width - IdeoUIUtility.PreceptBoxSize.x * 3f - 16f) / 2f;
			Rect position = new Rect(x2, curY, 25f, 25f);
			GUI.DrawTexture(position, IdeoUIUtility.FluidIdeoIcon);
			Text.Anchor = TextAnchor.MiddleCenter;
			Rect rect = new Rect(position.xMax + 4f, curY, x, 25f);
			Widgets.Label(rect, text);
			bool canReformNow = ideo.development.CanReformNow;
			if (editMode == IdeoEditMode.Dev && !canReformNow && Widgets.ButtonText(new Rect(rect.xMax + 10f, rect.y, 25f, 25f), "+", true, true, true, null))
			{
				ideo.development.points = ideo.development.NextReformationDevelopmentPoints;
			}
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect2 = new Rect(position.x, position.y, rect.xMax - position.x, 25f);
			if (Mouse.IsOver(rect2))
			{
				TaggedString taggedString = string.Concat(new string[]
				{
					("CurrentDevelopmentPoints".Translate() + ": ").Colorize(ColoredText.TipSectionTitleColor),
					ideo.development.Points.ToString(),
					"\n",
					("ReformDevelopmentPoints".Translate().Colorize(ColoredText.TipSectionTitleColor) + ": ").Colorize(ColoredText.TipSectionTitleColor),
					ideo.development.NextReformationDevelopmentPoints.ToString(),
					"\n\n"
				}) + "FluidIdeoTip".Translate() + "\n\n" + ("FluidIdeoTipGetPoints".Translate() + ": ").Colorize(ColoredText.TipSectionTitleColor) + "\n";
				taggedString += "\n- " + "FluidIdeoTipGetPoinsByConversion".Translate();
				IdeoUIUtility.fluidIdeoRituals.Clear();
				IdeoDevelopmentUtility.GetAllRitualsThatGiveDevelopmentPoints(ideo, IdeoUIUtility.fluidIdeoRituals);
				for (int i = 0; i < IdeoUIUtility.fluidIdeoRituals.Count; i++)
				{
					taggedString += "\n- " + IdeoUIUtility.fluidIdeoRituals[i].LabelCap + " (" + "Ritual".Translate() + ")";
				}
				IdeoUIUtility.fluidIdeoRituals.Clear();
				IdeoUIUtility.fluidIdeoQuestSuccessEvents.Clear();
				IdeoDevelopmentUtility.GetAllQuestSuccessEventsThatGiveDevelopmentPoints(ideo, IdeoUIUtility.fluidIdeoQuestSuccessEvents);
				for (int j = 0; j < IdeoUIUtility.fluidIdeoQuestSuccessEvents.Count; j++)
				{
					taggedString += "\n- " + IdeoUIUtility.fluidIdeoQuestSuccessEvents[j].LabelCap + " (" + "QuestLower".Translate() + ")";
				}
				IdeoUIUtility.fluidIdeoQuestSuccessEvents.Clear();
				TooltipHandler.TipRegion(rect2, taggedString);
				Widgets.DrawHighlight(rect2);
			}
			if (canReformNow && Widgets.ButtonText(new Rect(rect.xMax + 10f, curY, 100f, 25f), "ReformIdeo".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_ReformIdeo(ideo));
			}
			curY += 25f;
		}

		// Token: 0x0600D562 RID: 54626 RVA: 0x003F2AC8 File Offset: 0x003F0CC8
		private static float DoFactionIcons(Ideo ideo, float startX, ref float curY, float width, float iconSize, float iconSizeMinor, string label = null, bool showInitialIcon = false, bool showSelectedAsPlayerIdeo = false)
		{
			IdeoUIUtility.tmpFactions.Clear();
			bool flag = showSelectedAsPlayerIdeo && IdeoUIUtility.selected != null;
			foreach (Faction faction in Find.FactionManager.AllFactionsInViewOrder)
			{
				if ((!faction.Hidden || IdeoUIUtility.showAll) && (!flag || !faction.IsPlayer) && faction.ideos != null && (faction.ideos.IsPrimary(ideo) || faction.ideos.IsMinor(ideo)))
				{
					IdeoUIUtility.tmpFactions.Add(faction);
				}
			}
			if (flag && ideo == IdeoUIUtility.selected)
			{
				IdeoUIUtility.tmpFactions.Add(Faction.OfPlayer);
			}
			float num = startX;
			if (showInitialIcon && ideo.initialPlayerIdeo)
			{
				Rect rect = new Rect(num, curY, iconSize, iconSize);
				GUI.DrawTexture(rect, IdeoUIUtility.InitialPlayerIdeoTex);
				if (Mouse.IsOver(rect))
				{
					TooltipHandler.TipRegion(rect, "InitialPlayerIdeo".Translate());
					Widgets.DrawHighlight(rect);
				}
				num += iconSize + 4f;
			}
			if (IdeoUIUtility.tmpFactions.Any<Faction>())
			{
				IdeoUIUtility.tmpFactions.SortByDescending((Faction x) => x.ideos.IsPrimary(ideo));
				if (!label.NullOrEmpty())
				{
					Widgets.Label(startX, ref curY, width, label, default(TipSignal));
				}
				for (int i = 0; i < IdeoUIUtility.tmpFactions.Count; i++)
				{
					float num2 = IdeoUIUtility.tmpFactions[i].ideos.IsPrimary(ideo) ? iconSize : iconSizeMinor;
					if (num + num2 > width - startX)
					{
						num = startX;
						curY += num2 + 4f;
					}
					FactionUIUtility.DrawFactionIconWithTooltip(new Rect(num, curY, num2, num2), IdeoUIUtility.tmpFactions[i]);
					num += num2 + 4f;
				}
				curY += iconSize + 17f;
				return num;
			}
			return startX;
		}

		// Token: 0x0600D563 RID: 54627 RVA: 0x003F2CE4 File Offset: 0x003F0EE4
		private static void DoPawnIcons(List<Pawn> pawns, float startX, ref float curY, float width, float iconSize)
		{
			float num = startX;
			for (int i = 0; i < pawns.Count; i++)
			{
				Rect rect = new Rect(num, curY, iconSize, iconSize);
				Widgets.DrawHighlightIfMouseover(rect);
				Widgets.ThingIcon(rect, pawns[i], 1f, null, false, 1f, false);
				TooltipHandler.TipRegion(rect, pawns[i].LabelCap);
				num += iconSize + 4f;
			}
		}

		// Token: 0x0600D564 RID: 54628 RVA: 0x003F2D5C File Offset: 0x003F0F5C
		public static void DoDescription(ref float curY, float width, Ideo ideo, IdeoEditMode editMode)
		{
			float num = (width - IdeoUIUtility.PreceptBoxSize.x * 3f - 16f) / 2f;
			if (editMode != IdeoEditMode.None)
			{
				Rect rect = new Rect(num, curY, Text.LineHeight, Text.LineHeight);
				if (Widgets.ButtonImage(rect, ideo.descriptionLocked ? IdeoUIUtility.LockedTex : IdeoUIUtility.UnlockedTex, true, null))
				{
					ideo.descriptionLocked = !ideo.descriptionLocked;
					if (ideo.descriptionLocked)
					{
						SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
					}
					else
					{
						SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
					}
				}
				GUI.color = Color.white;
				if (Mouse.IsOver(rect))
				{
					string str = "LockButtonDesc".Translate() + "\n\n" + (ideo.descriptionLocked ? "LockInOn" : "LockInOff").Translate("Narrative".Translate(), "NarrativeLower".Translate());
					TooltipHandler.TipRegion(rect, str);
				}
			}
			float yMin = curY;
			Widgets.Label(num + Text.LineHeight + 4f, ref curY, width, "CoreNarrative".Translate(), default(TipSignal));
			float width2 = width - num - 80f;
			int num2 = (int)Mathf.Max(70f, Text.CalcHeight(ideo.description, width2));
			Rect rect2 = new Rect(num + 40f, curY, width2, (float)num2);
			Widgets.Label(rect2, ideo.description);
			if (editMode != IdeoEditMode.None)
			{
				Rect rect3 = rect2;
				rect3.yMin = yMin;
				rect3.xMin = num + Text.LineHeight + 4f;
				if (Mouse.IsOver(rect3))
				{
					Widgets.DrawHighlight(rect3);
					TooltipHandler.TipRegion(rect2, "CoreNarrativeDesc".Translate() + "\n\n" + IdeoUIUtility.ClickToEdit);
				}
				if (Widgets.ButtonInvisible(rect3, true) && IdeoUIUtility.TutorAllowsInteraction(editMode))
				{
					Find.WindowStack.Add(new Dialog_EditIdeoDescription(ideo));
				}
			}
			curY += (float)num2;
			curY += 17f;
		}

		// Token: 0x0600D565 RID: 54629 RVA: 0x003F2F6A File Offset: 0x003F116A
		public static void DoFoundationInfo(ref float curY, float width, Ideo ideo, IdeoEditMode editMode)
		{
			ideo.foundation.DoInfo(ref curY, width, editMode);
			curY += 17f;
		}

		// Token: 0x0600D566 RID: 54630 RVA: 0x003F2F84 File Offset: 0x003F1184
		private static void DoMemes(ref float curY, float width, Ideo ideo, IdeoEditMode editMode)
		{
			float num = (width - IdeoUIUtility.PreceptBoxSize.x * 3f - 16f) / 2f;
			float num2 = curY;
			Widgets.Label(num, ref num2, width, "Memes".Translate(), default(TipSignal));
			IdeoUIUtility.tmpMemesToShow.Clear();
			for (int i = 0; i < ideo.memes.Count; i++)
			{
				if (ideo.memes[i].category != MemeCategory.Structure)
				{
					IdeoUIUtility.tmpMemesToShow.Add(ideo.memes[i]);
				}
			}
			float num3 = (float)IdeoUIUtility.tmpMemesToShow.Count * IdeoUIUtility.MemeBoxSize.x + (float)((IdeoUIUtility.tmpMemesToShow.Count - 1) * 8);
			float num4 = (width - num3) / 2f;
			if (editMode == IdeoEditMode.GameStart && IdeoUIUtility.tmpMemesToShow.Any<MemeDef>())
			{
				IdeoUIUtility.DrawKnowledgeTip(ConceptDefOf.EditingMemes, curY + Text.LineHeight, num);
			}
			IdeoUIUtility.tmpMouseOverMeme = null;
			for (int j = 0; j < IdeoUIUtility.tmpMemesToShow.Count; j++)
			{
				Rect rect = new Rect(num4 + (float)j * IdeoUIUtility.MemeBoxSize.x + (float)(j * 8), curY, IdeoUIUtility.MemeBoxSize.x, IdeoUIUtility.MemeBoxSize.y);
				if (editMode == IdeoEditMode.GameStart)
				{
					UIHighlighter.HighlightOpportunity(rect, "MemeBox");
				}
				IdeoUIUtility.DoMeme(rect, IdeoUIUtility.tmpMemesToShow[j], ideo, editMode, true, null);
			}
			curY += IdeoUIUtility.MemeBoxSize.y;
			curY += 17f;
		}

		// Token: 0x0600D567 RID: 54631 RVA: 0x003F310C File Offset: 0x003F130C
		private static void DrawKnowledgeTip(ConceptDef conceptDef, float curY, float labelAlignOffset)
		{
			if (Find.World == null && TutorSystem.AdaptiveTrainingEnabled && !PlayerKnowledgeDatabase.IsComplete(conceptDef))
			{
				Rect rect = new Rect(new Rect(0f, curY + Text.LineHeight, labelAlignOffset / 2f + 26f, Text.LineHeight * 2f));
				GUI.color = IdeoUIUtility.TutorArrowColor;
				GUI.DrawTexture(new Rect(rect.xMax - 1f, rect.y, rect.height, rect.height), IdeoUIUtility.ArrowTex);
				GUI.color = Color.white;
				Widgets.DrawWindowBackgroundTutor(rect);
				Text.Anchor = TextAnchor.MiddleCenter;
				GUI.color = Color.white;
				Widgets.Label(rect.ContractedBy(2f), IdeoUIUtility.ClickToEdit);
				Text.Anchor = TextAnchor.UpperLeft;
			}
		}

		// Token: 0x0600D568 RID: 54632 RVA: 0x003F31E0 File Offset: 0x003F13E0
		public static void DoMeme(Rect memeBox, MemeDef meme, Ideo ideo = null, IdeoEditMode editMode = IdeoEditMode.None, bool drawHighlight = true, Action selectedOverride = null)
		{
			if (!ModLister.CheckIdeology("Memes"))
			{
				return;
			}
			if (drawHighlight)
			{
				Widgets.DrawLightHighlight(memeBox);
				GUI.color = new Color(1f, 1f, 1f, 0.04f);
				Widgets.DrawBox(memeBox, 1, null);
				GUI.color = Color.white;
			}
			if (Mouse.IsOver(memeBox))
			{
				Widgets.DrawHighlight(memeBox);
				TooltipHandler.TipRegion(memeBox, IdeoUIUtility.GetMemeTip(meme, ideo) + ((editMode != IdeoEditMode.None) ? ("\n\n" + IdeoUIUtility.ClickToEdit) : string.Empty));
				IdeoUIUtility.tmpMouseOverMeme = meme;
			}
			else if (IdeoUIUtility.tmpMouseOverPrecept != null && IdeoUIUtility.IsPreceptRelatedForUI(meme, IdeoUIUtility.tmpMouseOverPrecept))
			{
				Widgets.DrawHighlight(memeBox);
			}
			GUI.DrawTexture(new Rect(memeBox.x + (memeBox.width - 80f) / 2f, memeBox.y + 8f, 80f, 80f), meme.Icon);
			if (meme.impact > 0)
			{
				Rect rect = memeBox.RightPartPixels(18f).TopPartPixels(18f);
				rect.x -= 4f;
				rect.y += 4f;
				IdeoImpactUtility.DrawImpactIcon(rect, meme.impact);
			}
			Rect rect2 = new Rect(memeBox.x, memeBox.yMax - Text.LineHeight * 2f + 4f, memeBox.width, Text.LineHeight * 2f - 4f).ContractedBy(10f, 0f);
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect2, meme.LabelCap);
			GenUI.ResetLabelAlign();
			if (editMode != IdeoEditMode.None && Widgets.ButtonInvisible(memeBox, true) && IdeoUIUtility.TutorAllowsInteraction(editMode))
			{
				if (selectedOverride != null)
				{
					selectedOverride();
					return;
				}
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.EditingMemes, KnowledgeAmount.Total);
				Find.WindowStack.Add(new Dialog_ChooseMemes(ideo, meme.category, false, null, null, false));
			}
		}

		// Token: 0x0600D569 RID: 54633 RVA: 0x003F33D0 File Offset: 0x003F15D0
		private static string GetMemeTip(MemeDef meme, Ideo ideo = null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(meme.LabelCap.Colorize(ColoredText.TipSectionTitleColor));
			if (meme.impact > 0)
			{
				stringBuilder.AppendLine(("IdeoImpact".Translate() + ": " + IdeoImpactUtility.MemeImpactLabel(meme.impact).CapitalizeFirst()).Colorize(ColoredText.ImpactColor));
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(meme.description);
			List<PreceptDef> allDefsListForReading = DefDatabase<PreceptDef>.AllDefsListForReading;
			if (!meme.requireOne.NullOrEmpty<List<PreceptDef>>())
			{
				IdeoUIUtility.tmpRequiredPrecepts.Clear();
				for (int i = 0; i < meme.requireOne.Count; i++)
				{
					List<PreceptDef> list = meme.requireOne[i];
					if (list.Count == 1)
					{
						IdeoUIUtility.tmpRequiredPrecepts.Add(list[0]);
					}
				}
				if (IdeoUIUtility.tmpRequiredPrecepts.Count > 0)
				{
					stringBuilder.AppendInNewLine(("RequiredPrecepts".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
					stringBuilder.AppendLine();
					for (int j = 0; j < IdeoUIUtility.tmpRequiredPrecepts.Count; j++)
					{
						stringBuilder.AppendLine("  - " + IdeoUIUtility.tmpRequiredPrecepts[j].issue.LabelCap + ": " + IdeoUIUtility.tmpRequiredPrecepts[j].LabelCap);
					}
					IdeoUIUtility.tmpRequiredPrecepts.Clear();
				}
				for (int k = 0; k < meme.requireOne.Count; k++)
				{
					List<PreceptDef> list2 = meme.requireOne[k];
					if (list2.Count > 1)
					{
						stringBuilder.AppendInNewLine(("RequiresOnePrecept".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
						stringBuilder.AppendLine();
						for (int l = 0; l < list2.Count; l++)
						{
							stringBuilder.AppendLine("  - " + list2[l].issue.LabelCap + ": " + list2[l].LabelCap);
						}
					}
				}
			}
			if (meme.selectOneOrNone != null && meme.selectOneOrNone.preceptThingPairs.Count > 0)
			{
				stringBuilder.AppendInNewLine(("ChanceToHavePrecept".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
				stringBuilder.AppendLine();
				List<PreceptThingPair> preceptThingPairs = meme.selectOneOrNone.preceptThingPairs;
				for (int m = 0; m < preceptThingPairs.Count; m++)
				{
					stringBuilder.AppendLine("  - " + preceptThingPairs[m].thing.LabelCap + ": " + preceptThingPairs[m].precept.LabelCap);
				}
			}
			if (!meme.requiredRituals.NullOrEmpty<RequiredRitualAndBuilding>())
			{
				stringBuilder.AppendInNewLine(("RequiredRituals".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
				stringBuilder.AppendLine();
				for (int n = 0; n < meme.requiredRituals.Count; n++)
				{
					stringBuilder.AppendLine("  - " + (meme.requiredRituals[n].pattern.shortDescOverride.CapitalizeFirst() ?? meme.requiredRituals[n].precept.LabelCap));
				}
			}
			List<string> list3 = meme.UnlockedRoles(ideo);
			if (!list3.NullOrEmpty<string>())
			{
				stringBuilder.AppendInNewLine(("MemeUnlocksRoles".Translate() + ": ").Colorize(ColoredText.TipSectionTitleColor));
				stringBuilder.AppendInNewLine(list3.ToLineList("  - "));
				stringBuilder.AppendLine();
			}
			List<string> list4 = meme.UnlockedRituals();
			if (!list4.NullOrEmpty<string>())
			{
				stringBuilder.AppendInNewLine(("MemeUnlocksRituals".Translate() + ": ").Colorize(ColoredText.TipSectionTitleColor));
				stringBuilder.AppendInNewLine(list4.ToLineList("  - "));
				stringBuilder.AppendLine();
			}
			if (!meme.thingStyleCategories.NullOrEmpty<ThingStyleCategoryWithPriority>())
			{
				stringBuilder.AppendInNewLine(("Styles".Translate() + ": ").Colorize(ColoredText.TipSectionTitleColor));
				stringBuilder.AppendLine();
				for (int num = 0; num < meme.thingStyleCategories.Count; num++)
				{
					stringBuilder.AppendLine("  - " + meme.thingStyleCategories[num].category.LabelCap);
				}
			}
			IEnumerable<RecipeDef> source = from r in DefDatabase<RecipeDef>.AllDefsListForReading
			where r.memePrerequisitesAny != null && r.memePrerequisitesAny.Contains(meme)
			select r;
			if (source.Any<RecipeDef>())
			{
				stringBuilder.AppendInNewLine(("UnlockedRecipes".Translate() + ":").CapitalizeFirst().Colorize(ColoredText.TipSectionTitleColor));
				List<ThingDef> defs = (from r in source
				select r.ProducedThingDef).Distinct<ThingDef>().ToList<ThingDef>();
				stringBuilder.AppendLine(IdeoUIUtility.SortedLabelCaps<ThingDef>(defs, "\n  - "));
			}
			if (!meme.addDesignators.NullOrEmpty<BuildableDef>() || !meme.addDesignatorGroups.NullOrEmpty<DesignatorDropdownGroupDef>())
			{
				stringBuilder.AppendInNewLine(("IdeoMakesBuildingBuildable".Translate() + ": ").Colorize(ColoredText.TipSectionTitleColor));
				stringBuilder.AppendLine(IdeoUIUtility.SortedLabelCaps<BuildableDef, DesignatorDropdownGroupDef>(meme.addDesignators, meme.addDesignatorGroups, "\n  - "));
			}
			if (meme.startingResearchProjects.Any<ResearchProjectDef>())
			{
				stringBuilder.AppendInNewLine(("IdeoStartWithResearch".Translate() + ": ").Colorize(ColoredText.TipSectionTitleColor));
				stringBuilder.AppendLine(IdeoUIUtility.SortedLabelCaps<ResearchProjectDef>(meme.startingResearchProjects, "\n  - "));
			}
			if (!meme.agreeableTraits.NullOrEmpty<TraitRequirement>())
			{
				stringBuilder.AppendInNewLine(("AgreeableTraits".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
				stringBuilder.AppendInNewLine(meme.agreeableTraits.Select(delegate(TraitRequirement x)
				{
					if (x.degree == null)
					{
						return x.def.degreeDatas.First<TraitDegreeData>().label;
					}
					return x.def.DataAtDegree(x.degree.Value).label;
				}).ToLineList("  - ", true));
				stringBuilder.AppendLine();
			}
			if (!meme.disagreeableTraits.NullOrEmpty<TraitRequirement>())
			{
				stringBuilder.AppendInNewLine(("DisagreeableTraits".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
				stringBuilder.AppendInNewLine(meme.disagreeableTraits.Select(delegate(TraitRequirement x)
				{
					if (x.degree == null)
					{
						return x.def.degreeDatas.First<TraitDegreeData>().label;
					}
					return x.def.DataAtDegree(x.degree.Value).label;
				}).ToLineList("  - ", true));
				stringBuilder.AppendLine();
			}
			int num2 = 0;
			for (int num3 = 0; num3 < allDefsListForReading.Count; num3++)
			{
				if (allDefsListForReading[num3].conflictingMemes.Contains(meme))
				{
					num2++;
				}
			}
			if (num2 > 0)
			{
				stringBuilder.AppendInNewLine(("PreventsPrecepts".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
				stringBuilder.AppendLine();
				for (int num4 = 0; num4 < allDefsListForReading.Count; num4++)
				{
					if (allDefsListForReading[num4].conflictingMemes.Contains(meme))
					{
						stringBuilder.AppendLine("  - " + allDefsListForReading[num4].issue.LabelCap + ": " + allDefsListForReading[num4].LabelCap);
					}
				}
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600D56A RID: 54634 RVA: 0x003F3C56 File Offset: 0x003F1E56
		private static string SortedLabelCaps<T>(List<T> defs, string prefix) where T : Def
		{
			return IdeoUIUtility.SortedLabelCaps<T, Def>(defs, null, prefix);
		}

		// Token: 0x0600D56B RID: 54635 RVA: 0x003F3C60 File Offset: 0x003F1E60
		private static string SortedLabelCaps<T1, T2>(List<T1> defs1, List<T2> defs2, string prefix) where T1 : Def where T2 : Def
		{
			IdeoUIUtility.tmpSortedLabelCaps.Clear();
			if (!defs1.NullOrEmpty<T1>())
			{
				foreach (T1 t in defs1)
				{
					if (t != null)
					{
						IdeoUIUtility.tmpSortedLabelCaps.Add(t.LabelCap);
					}
				}
			}
			if (!defs2.NullOrEmpty<T2>())
			{
				foreach (T2 t2 in defs2)
				{
					if (t2 != null)
					{
						IdeoUIUtility.tmpSortedLabelCaps.Add(t2.LabelCap);
					}
				}
			}
			if (IdeoUIUtility.tmpSortedLabelCaps.NullOrEmpty<string>())
			{
				return "";
			}
			IdeoUIUtility.tmpSortedLabelCaps.Sort();
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string value in IdeoUIUtility.tmpSortedLabelCaps)
			{
				stringBuilder.Append(prefix).Append(value);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600D56C RID: 54636 RVA: 0x003F3DB4 File Offset: 0x003F1FB4
		public static void DoPrecepts(ref float curY, float width, Ideo ideo, IdeoEditMode editMode)
		{
			IdeoUIUtility.DoPreceptsInt("Precepts".Translate(), "Precept".Translate(), true, ideo, editMode, ref curY, width, (PreceptDef p) => p.preceptClass == typeof(Precept), true);
			IdeoUIUtility.DoPreceptsInt("IdeoRoles".Translate(), "Role".Translate(), false, ideo, editMode, ref curY, width, (PreceptDef p) => typeof(Precept_Role).IsAssignableFrom(p.preceptClass), false);
			IdeoUIUtility.DoPreceptsInt("Rituals".Translate(), "Ritual".Translate(), false, ideo, editMode, ref curY, width, (PreceptDef p) => p.preceptClass == typeof(Precept_Ritual), false);
			IdeoUIUtility.DoPreceptsInt("IdeoBuildings".Translate(), "IdeoBuilding".Translate(), false, ideo, editMode, ref curY, width, (PreceptDef p) => p.preceptClass == typeof(Precept_Building) || p.preceptClass == typeof(Precept_RitualSeat), false);
			IdeoUIUtility.DoPreceptsInt("IdeoRelics".Translate(), "IdeoRelic".Translate(), false, ideo, editMode, ref curY, width, (PreceptDef p) => p.preceptClass == typeof(Precept_Relic), false);
			IdeoUIUtility.DoPreceptsInt("IdeoWeapons".Translate(), "IdeoWeapon".Translate(), false, ideo, editMode, ref curY, width, (PreceptDef p) => p.preceptClass == typeof(Precept_Weapon), false);
			IdeoUIUtility.DoPreceptsInt("VeneratedAnimals".Translate(), "Animal".Translate(), false, ideo, editMode, ref curY, width, (PreceptDef p) => p.preceptClass == typeof(Precept_Animal), false);
			if (ModsConfig.BiotechActive)
			{
				IdeoUIUtility.DoPreceptsInt("PreferredXenotypes".Translate(), "Xenotype".Translate().ToString().UncapitalizeFirst(), false, ideo, editMode, ref curY, width, (PreceptDef p) => p.preceptClass == typeof(Precept_Xenotype), false);
			}
			IdeoUIUtility.DoPreceptsInt("IdeoApparel".Translate(), "IdeoApparelDesire".Translate(), false, ideo, editMode, ref curY, width, (PreceptDef p) => p.preceptClass == typeof(Precept_Apparel), false);
		}

		// Token: 0x0600D56D RID: 54637 RVA: 0x003F4060 File Offset: 0x003F2260
		private static void DoPreceptsInt(string categoryLabel, string addPreceptLabel, bool mainPrecepts, Ideo ideo, IdeoEditMode editMode, ref float curY, float width, Func<PreceptDef, bool> filter, bool group = false)
		{
			IdeoUIUtility.<>c__DisplayClass101_0 CS$<>8__locals1 = new IdeoUIUtility.<>c__DisplayClass101_0();
			CS$<>8__locals1.ideo = ideo;
			IdeoUIUtility.tmpPrecepts.Clear();
			List<Precept> preceptsListForReading = CS$<>8__locals1.ideo.PreceptsListForReading;
			for (int i = 0; i < preceptsListForReading.Count; i++)
			{
				if ((IdeoUIUtility.showAll || preceptsListForReading[i].def.visible) && filter(preceptsListForReading[i].def))
				{
					IdeoUIUtility.tmpPrecepts.Add(preceptsListForReading[i]);
				}
			}
			if (!mainPrecepts && preceptsListForReading.Count == 0)
			{
				return;
			}
			IdeoUIUtility.tmpUsedThingDefs.Clear();
			foreach (Precept precept in CS$<>8__locals1.ideo.PreceptsListForReading)
			{
				Precept_ThingDef precept_ThingDef = precept as Precept_ThingDef;
				if (precept_ThingDef != null)
				{
					IdeoUIUtility.tmpUsedThingDefs.Add(precept_ThingDef.ThingDef);
				}
			}
			curY += 4f;
			float num = (width - IdeoUIUtility.PreceptBoxSize.x * 3f - 16f) / 2f;
			Widgets.Label(num, ref curY, width, categoryLabel, default(TipSignal));
			if (editMode == IdeoEditMode.GameStart && mainPrecepts && IdeoUIUtility.tmpPrecepts.Any<Precept>())
			{
				IdeoUIUtility.DrawKnowledgeTip(ConceptDefOf.EditingPrecepts, curY, num);
			}
			if (editMode != IdeoEditMode.None)
			{
				float num2 = width - (width - IdeoUIUtility.PreceptBoxSize.x * 3f - 16f) / 2f;
				Rect rect = new Rect(num2 - IdeoUIUtility.AddPreceptButtonSize.x, curY - IdeoUIUtility.AddPreceptButtonSize.y, IdeoUIUtility.AddPreceptButtonSize.x, IdeoUIUtility.AddPreceptButtonSize.y);
				if (Widgets.ButtonText(rect, "AddPrecept".Translate(addPreceptLabel).CapitalizeFirst() + "...", true, true, true, null) && IdeoUIUtility.TutorAllowsInteraction(editMode))
				{
					IdeoUIUtility.AddPrecept(CS$<>8__locals1.ideo, editMode, filter, group);
				}
				if (mainPrecepts)
				{
					Rect rect2 = rect;
					rect2.x = rect.xMin - rect.width - 10f;
					if (Widgets.ButtonText(rect2, "RandomizePrecepts".Translate(), true, true, true, null) && IdeoUIUtility.TutorAllowsInteraction(editMode))
					{
						if (CS$<>8__locals1.ideo.anyPreceptEdited)
						{
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ChangesRandomizePrecepts".Translate("RandomizePrecepts".Translate()), new Action(CS$<>8__locals1.<DoPreceptsInt>g__RandomizePrecepts|0), false, null, WindowLayer.Dialog));
						}
						else
						{
							CS$<>8__locals1.<DoPreceptsInt>g__RandomizePrecepts|0();
						}
					}
				}
			}
			curY += 4f;
			PreceptImpact preceptImpact = IdeoUIUtility.tmpPrecepts.Any<Precept>() ? IdeoUIUtility.tmpPrecepts[0].def.impact : PreceptImpact.Low;
			float num3 = (width - 3f * IdeoUIUtility.PreceptBoxSize.x - 16f) / 2f;
			int num4 = 0;
			int num5 = 0;
			if (!IdeoUIUtility.tmpPrecepts.Any<Precept>())
			{
				GUI.color = Color.grey;
				Widgets.Label(new Rect(num + 36f, curY + 10f, 999f, Text.LineHeight), "(" + "NoneLower".Translate() + ")");
				GUI.color = Color.white;
			}
			for (int j = 0; j < IdeoUIUtility.tmpPrecepts.Count; j++)
			{
				if (preceptImpact != IdeoUIUtility.tmpPrecepts[j].def.impact)
				{
					preceptImpact = IdeoUIUtility.tmpPrecepts[j].def.impact;
					num5 = 0;
					num4++;
				}
				else if (num5 >= 2)
				{
					num5 = 0;
					num4++;
				}
				else if (j > 0)
				{
					num5++;
				}
				Rect rect3 = new Rect(num3 + (float)num5 * IdeoUIUtility.PreceptBoxSize.x + (float)(num5 * 8), curY + (float)num4 * IdeoUIUtility.PreceptBoxSize.y + (float)(num4 * 8), IdeoUIUtility.PreceptBoxSize.x, IdeoUIUtility.PreceptBoxSize.y);
				if (mainPrecepts && editMode == IdeoEditMode.GameStart)
				{
					UIHighlighter.HighlightOpportunity(rect3, "PreceptBox");
				}
				IdeoUIUtility.tmpPrecepts[j].DrawPreceptBox(rect3, editMode, IdeoUIUtility.tmpMouseOverMeme != null && IdeoUIUtility.IsPreceptRelatedForUI(IdeoUIUtility.tmpMouseOverMeme, IdeoUIUtility.tmpPrecepts[j].def));
				if (Mouse.IsOver(rect3))
				{
					IdeoUIUtility.tmpMouseOverPrecept = IdeoUIUtility.tmpPrecepts[j].def;
				}
				GUI.color = Color.white;
			}
			curY += (float)(num4 + 1) * IdeoUIUtility.PreceptBoxSize.y + (float)(num4 * 8);
			curY += 17f;
			IdeoUIUtility.tmpPrecepts.Clear();
		}

		// Token: 0x0600D56E RID: 54638 RVA: 0x003F4540 File Offset: 0x003F2740
		private static void RegisterPreceptGroup(Ideo ideo, PreceptDef def, IdeoEditMode mode, ThingDef thing = null)
		{
			if (def.issue == null)
			{
				return;
			}
			List<FloatMenuOption> options;
			if (IdeoUIUtility.issueGroupsTmp.TryGetValue(def.issue, out options))
			{
				IdeoUIUtility.AddPreceptOption(ideo, def, mode, options, null);
				return;
			}
			PreceptDef def2;
			if (IdeoUIUtility.singlePreceptsTmp.Remove(def.issue, out def2))
			{
				options = (IdeoUIUtility.issueGroupsTmp[def.issue] = new List<FloatMenuOption>());
				IdeoUIUtility.AddPreceptOption(ideo, def2, mode, options, null);
				IdeoUIUtility.AddPreceptOption(ideo, def, mode, options, null);
				return;
			}
			IdeoUIUtility.singlePreceptsTmp[def.issue] = def;
		}

		// Token: 0x0600D56F RID: 54639 RVA: 0x003F45C8 File Offset: 0x003F27C8
		private static void PushPreceptGroups(Ideo ideo, List<FloatMenuOption> options, IdeoEditMode mode)
		{
			foreach (KeyValuePair<IssueDef, List<FloatMenuOption>> keyValuePair in IdeoUIUtility.issueGroupsTmp)
			{
				IssueDef issue2;
				List<FloatMenuOption> list2;
				keyValuePair.Deconstruct(out issue2, out list2);
				IssueDef issue = issue2;
				List<FloatMenuOption> list = list2;
				if (!list.Empty<FloatMenuOption>())
				{
					options.Add(new FloatMenuOption(issue.LabelCap, delegate()
					{
						Find.WindowStack.Add(new FloatMenu(list, issue.LabelCap, false));
					}, issue.Icon ?? ideo.Icon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
				}
			}
			foreach (KeyValuePair<IssueDef, PreceptDef> keyValuePair2 in IdeoUIUtility.singlePreceptsTmp)
			{
				IssueDef issue2;
				PreceptDef preceptDef;
				keyValuePair2.Deconstruct(out issue2, out preceptDef);
				PreceptDef def = preceptDef;
				IdeoUIUtility.AddPreceptOption(ideo, def, mode, options, null);
			}
			IdeoUIUtility.singlePreceptsTmp.Clear();
			IdeoUIUtility.issueGroupsTmp.Clear();
		}

		// Token: 0x0600D570 RID: 54640 RVA: 0x003F4700 File Offset: 0x003F2900
		private static void AddPrecept(Ideo ideo, IdeoEditMode editMode, Func<PreceptDef, bool> filter, bool group)
		{
			IdeoUIUtility.<>c__DisplayClass107_0 CS$<>8__locals1 = new IdeoUIUtility.<>c__DisplayClass107_0();
			CS$<>8__locals1.ideo = ideo;
			CS$<>8__locals1.editMode = editMode;
			CS$<>8__locals1.opts = new List<FloatMenuOption>();
			List<PreceptDef> list = DefDatabase<PreceptDef>.AllDefs.Where(filter).ToList<PreceptDef>();
			bool flag = list.Any((PreceptDef p) => p.preceptClass == typeof(Precept_Ritual) && (IdeoUIUtility.showAll || p.visible) && IdeoUIUtility.CanListPrecept(CS$<>8__locals1.ideo, p, CS$<>8__locals1.editMode));
			int num = CS$<>8__locals1.ideo.PreceptsListForReading.Count((Precept p) => p is Precept_Ritual && (IdeoUIUtility.showAll || p.def.visible));
			IdeoUIUtility.addedPatternDefsTmp.Clear();
			IdeoUIUtility.thingGroupsTmp.Clear();
			using (List<PreceptDef>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IdeoUIUtility.<>c__DisplayClass107_1 CS$<>8__locals2 = new IdeoUIUtility.<>c__DisplayClass107_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					CS$<>8__locals2.p = enumerator.Current;
					IdeoUIUtility.<>c__DisplayClass107_2 CS$<>8__locals3;
					CS$<>8__locals3.acceptance = IdeoUIUtility.CanListPrecept(CS$<>8__locals2.CS$<>8__locals1.ideo, CS$<>8__locals2.p, CS$<>8__locals2.CS$<>8__locals1.editMode);
					if (CS$<>8__locals3.acceptance || !string.IsNullOrWhiteSpace(CS$<>8__locals3.acceptance.Reason))
					{
						int preceptCountOfDef = CS$<>8__locals2.CS$<>8__locals1.ideo.GetPreceptCountOfDef(CS$<>8__locals2.p);
						int num2 = CS$<>8__locals2.p.maxCount;
						if (CS$<>8__locals2.p.preceptInstanceCountCurve != null)
						{
							num2 = Mathf.Max(num2, Mathf.RoundToInt(CS$<>8__locals2.p.preceptInstanceCountCurve.Last<CurvePoint>().y));
						}
						if (preceptCountOfDef < num2 || CS$<>8__locals2.p.ignoreLimitsInEditMode)
						{
							IdeoUIUtility.<>c__DisplayClass107_8 CS$<>8__locals4 = new IdeoUIUtility.<>c__DisplayClass107_8();
							CS$<>8__locals4.CS$<>8__locals7 = CS$<>8__locals2;
							if (!CS$<>8__locals4.CS$<>8__locals7.p.useChoicesFromBuildingDefs || CS$<>8__locals4.CS$<>8__locals7.p.Worker.ThingDefsForIdeo(CS$<>8__locals4.CS$<>8__locals7.CS$<>8__locals1.ideo, null).EnumerableNullOrEmpty<PreceptThingChance>())
							{
								if (CS$<>8__locals4.CS$<>8__locals7.p.preceptClass == typeof(Precept_Weapon))
								{
									CS$<>8__locals4.CS$<>8__locals7.CS$<>8__locals1.<AddPrecept>g__AddWeaponPreceptOption|5(CS$<>8__locals4.CS$<>8__locals7.p, ref CS$<>8__locals3);
								}
								else if (CS$<>8__locals4.CS$<>8__locals7.p.preceptClass == typeof(Precept_Xenotype))
								{
									CS$<>8__locals4.CS$<>8__locals7.CS$<>8__locals1.<AddPrecept>g__AddXenotypePreceptOption|7(CS$<>8__locals4.CS$<>8__locals7.p, ref CS$<>8__locals3);
								}
								else if (group)
								{
									IdeoUIUtility.RegisterPreceptGroup(CS$<>8__locals4.CS$<>8__locals7.CS$<>8__locals1.ideo, CS$<>8__locals4.CS$<>8__locals7.p, CS$<>8__locals4.CS$<>8__locals7.CS$<>8__locals1.editMode, null);
								}
								else
								{
									IdeoUIUtility.AddPreceptOption(CS$<>8__locals4.CS$<>8__locals7.CS$<>8__locals1.ideo, CS$<>8__locals4.CS$<>8__locals7.p, CS$<>8__locals4.CS$<>8__locals7.CS$<>8__locals1.editMode, CS$<>8__locals4.CS$<>8__locals7.CS$<>8__locals1.opts, null);
								}
							}
							else
							{
								IdeoUIUtility.tmpAllowedThingDefs.Clear();
								IdeoUIUtility.tmpAllThingDefs.Clear();
								IdeoUIUtility.tmpAllowedThingDefs.AddRange(from td in CS$<>8__locals4.CS$<>8__locals7.p.Worker.ThingDefsForIdeo(CS$<>8__locals4.CS$<>8__locals7.CS$<>8__locals1.ideo, null)
								select td.def);
								IdeoUIUtility.tmpAllThingDefs.AddRange(from bDef in CS$<>8__locals4.CS$<>8__locals7.p.Worker.ThingDefs
								orderby CS$<>8__locals4.CS$<>8__locals7.p.Worker.GetThingOrder(bDef), bDef.chance descending, bDef.def.LabelCap
								select bDef into bd
								select bd.def);
								if (CS$<>8__locals4.CS$<>8__locals7.p.preceptClass == typeof(Precept_Building))
								{
									foreach (MemeDef memeDef in CS$<>8__locals4.CS$<>8__locals7.CS$<>8__locals1.ideo.memes)
									{
										if (!memeDef.consumableBuildings.NullOrEmpty<ThingDef>())
										{
											foreach (ThingDef item in memeDef.consumableBuildings)
											{
												if (!IdeoUIUtility.tmpAllowedThingDefs.Contains(item))
												{
													IdeoUIUtility.tmpAllowedThingDefs.Add(item);
												}
												if (!IdeoUIUtility.tmpAllThingDefs.Contains(item))
												{
													IdeoUIUtility.tmpAllThingDefs.Add(item);
												}
											}
										}
									}
								}
								using (List<ThingDef>.Enumerator enumerator3 = IdeoUIUtility.tmpAllThingDefs.GetEnumerator())
								{
									while (enumerator3.MoveNext())
									{
										ThingDef b = enumerator3.Current;
										TaggedString taggedString = b.LabelCap;
										if (CS$<>8__locals4.CS$<>8__locals7.p.preceptClass == typeof(Precept_Apparel))
										{
											taggedString += ": " + CS$<>8__locals4.CS$<>8__locals7.p.LabelCap;
										}
										FloatMenuOption floatMenuOption = null;
										if ((!CS$<>8__locals4.CS$<>8__locals7.p.canUseAlreadyUsedThingDef && IdeoUIUtility.tmpUsedThingDefs.Contains(b)) || CS$<>8__locals4.CS$<>8__locals7.p.Worker.ShouldSkipThing(CS$<>8__locals4.CS$<>8__locals7.CS$<>8__locals1.ideo, b) || !IdeoUIUtility.tmpAllowedThingDefs.Contains(b))
										{
											if (!CS$<>8__locals4.CS$<>8__locals7.p.canUseAlreadyUsedThingDef && IdeoUIUtility.tmpUsedThingDefs.Contains(b))
											{
												floatMenuOption = null;
											}
											else
											{
												AcceptanceReport report = CS$<>8__locals4.CS$<>8__locals7.p.Worker.CanUse(b, CS$<>8__locals4.CS$<>8__locals7.CS$<>8__locals1.ideo, null);
												if (!report)
												{
													floatMenuOption = new FloatMenuOption(string.IsNullOrWhiteSpace(report.Reason) ? taggedString : (taggedString + " (" + report.Reason + ")"), null, b, null, false, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, null);
													floatMenuOption.thingStyle = CS$<>8__locals4.CS$<>8__locals7.CS$<>8__locals1.ideo.GetStyleFor(b);
												}
											}
										}
										else
										{
											floatMenuOption = new FloatMenuOption(taggedString, delegate()
											{
												Precept precept = PreceptMaker.MakePrecept(CS$<>8__locals4.CS$<>8__locals7.p);
												Precept_Apparel precept_Apparel = precept as Precept_Apparel;
												if (precept_Apparel != null)
												{
													precept_Apparel.apparelDef = b;
												}
												CS$<>8__locals4.CS$<>8__locals7.CS$<>8__locals1.ideo.AddPrecept(precept, true, null, null);
												CS$<>8__locals4.CS$<>8__locals7.CS$<>8__locals1.ideo.anyPreceptEdited = true;
												Precept_ThingDef precept_ThingDef = precept as Precept_ThingDef;
												if (precept_ThingDef != null)
												{
													precept_ThingDef.ThingDef = b;
													precept_ThingDef.RegenerateName();
												}
											}, b, null, false, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, null);
											floatMenuOption.thingStyle = CS$<>8__locals4.CS$<>8__locals7.CS$<>8__locals1.ideo.GetStyleFor(b);
											if (CS$<>8__locals4.CS$<>8__locals7.p.preceptClass == typeof(Precept_Apparel))
											{
												floatMenuOption.forceThingColor = new Color?(CS$<>8__locals4.CS$<>8__locals7.CS$<>8__locals1.ideo.ApparelColor);
											}
										}
										if (floatMenuOption != null)
										{
											if (group)
											{
												CS$<>8__locals4.CS$<>8__locals7.CS$<>8__locals1.<AddPrecept>g__GetOptionList|22(b).Add(IdeoUIUtility.<AddPrecept>g__PostProcessOption|107_4(floatMenuOption, ref CS$<>8__locals3));
											}
											else
											{
												CS$<>8__locals4.CS$<>8__locals7.CS$<>8__locals1.opts.Add(IdeoUIUtility.<AddPrecept>g__PostProcessOption|107_4(floatMenuOption, ref CS$<>8__locals3));
											}
										}
									}
								}
							}
							IdeoUIUtility.<>c__DisplayClass107_8 CS$<>8__locals6 = CS$<>8__locals4;
							RitualPatternDef ritualPatternBase = CS$<>8__locals4.CS$<>8__locals7.p.ritualPatternBase;
							CS$<>8__locals6.groupTag = ((ritualPatternBase != null) ? ritualPatternBase.patternGroupTag : null);
							if (CS$<>8__locals4.groupTag.NullOrEmpty())
							{
								continue;
							}
							IEnumerable<RitualPatternDef> allDefs = DefDatabase<RitualPatternDef>.AllDefs;
							Func<RitualPatternDef, bool> predicate;
							if ((predicate = CS$<>8__locals4.<>9__25) == null)
							{
								predicate = (CS$<>8__locals4.<>9__25 = ((RitualPatternDef d) => d.patternGroupTag == CS$<>8__locals4.groupTag && d != CS$<>8__locals4.CS$<>8__locals7.p.ritualPatternBase));
							}
							using (IEnumerator<RitualPatternDef> enumerator4 = allDefs.Where(predicate).GetEnumerator())
							{
								while (enumerator4.MoveNext())
								{
									RitualPatternDef patternDef = enumerator4.Current;
									IdeoUIUtility.AddPreceptOption(CS$<>8__locals4.CS$<>8__locals7.CS$<>8__locals1.ideo, CS$<>8__locals4.CS$<>8__locals7.p, CS$<>8__locals4.CS$<>8__locals7.CS$<>8__locals1.editMode, CS$<>8__locals4.CS$<>8__locals7.CS$<>8__locals1.opts, patternDef);
								}
								continue;
							}
						}
						if (CS$<>8__locals2.p.preceptClass == typeof(Precept_Relic))
						{
							CS$<>8__locals2.CS$<>8__locals1.opts.Add(new FloatMenuOption("MaxRelicCount".Translate(num2), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
						else if (preceptCountOfDef >= num2 && CS$<>8__locals2.p.issue.allowMultiplePrecepts)
						{
							CS$<>8__locals2.CS$<>8__locals1.opts.Add(new FloatMenuOption(CS$<>8__locals2.p.LabelCap + " (" + "MaxPreceptCount".Translate(num2) + ")", null, CS$<>8__locals2.p.Icon ?? CS$<>8__locals2.CS$<>8__locals1.ideo.Icon, CS$<>8__locals2.CS$<>8__locals1.ideo.Color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
						}
					}
				}
			}
			IdeoUIUtility.PushPreceptGroups(CS$<>8__locals1.ideo, CS$<>8__locals1.opts, CS$<>8__locals1.editMode);
			CS$<>8__locals1.opts = CS$<>8__locals1.opts.OrderBy(delegate(FloatMenuOption x)
			{
				if (!x.Label.EndsWith("..."))
				{
					return 0;
				}
				return 1;
			}).ThenBy((FloatMenuOption x) => x.Label).ToList<FloatMenuOption>();
			if (num < 6)
			{
				using (List<MemeDef>.Enumerator enumerator2 = CS$<>8__locals1.ideo.memes.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						MemeDef memeDef2 = enumerator2.Current;
						if (!memeDef2.replacementPatterns.NullOrEmpty<RitualPatternDef>())
						{
							foreach (PreceptDef preceptDef in DefDatabase<PreceptDef>.AllDefs.Where(filter))
							{
								if (preceptDef.ritualPatternBase != null && !preceptDef.ritualPatternBase.tags.NullOrEmpty<string>() && memeDef2.replaceRitualsWithTags.Any(new Predicate<string>(preceptDef.ritualPatternBase.tags.Contains)) && !preceptDef.classicModeOnly)
								{
									foreach (RitualPatternDef ritualPatternDef in memeDef2.replacementPatterns)
									{
										if (IdeoUIUtility.CanAddRitualPattern(CS$<>8__locals1.ideo, ritualPatternDef, CS$<>8__locals1.editMode))
										{
											IdeoUIUtility.AddPreceptOption(CS$<>8__locals1.ideo, preceptDef, CS$<>8__locals1.editMode, CS$<>8__locals1.opts, ritualPatternDef);
										}
									}
								}
							}
						}
					}
					goto IL_BE9;
				}
			}
			if (flag)
			{
				CS$<>8__locals1.opts.Clear();
				CS$<>8__locals1.opts.Add(new FloatMenuOption("MaxRitualCount".Translate(6), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			}
			IL_BE9:
			if (!CS$<>8__locals1.opts.Any<FloatMenuOption>())
			{
				CS$<>8__locals1.opts.Add(new FloatMenuOption("NoChoicesAvailable".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			}
			IdeoUIUtility.thingGroupsTmp.Clear();
			Find.WindowStack.Add(new FloatMenu(CS$<>8__locals1.opts));
		}

		// Token: 0x0600D571 RID: 54641 RVA: 0x003F5414 File Offset: 0x003F3614
		private static void AddPreceptOption(Ideo ideo, PreceptDef def, IdeoEditMode editMode, List<FloatMenuOption> options, RitualPatternDef patternDef = null)
		{
			IdeoUIUtility.<>c__DisplayClass108_0 CS$<>8__locals1 = new IdeoUIUtility.<>c__DisplayClass108_0();
			CS$<>8__locals1.def = def;
			CS$<>8__locals1.ideo = ideo;
			IdeoUIUtility.tempRequiredMemes.Clear();
			if (!CS$<>8__locals1.def.visibleOnAddFloatMenu && editMode != IdeoEditMode.Dev)
			{
				return;
			}
			CS$<>8__locals1.pat = (patternDef ?? CS$<>8__locals1.def.ritualPatternBase);
			if (CS$<>8__locals1.pat != null && (!IdeoUIUtility.CanAddRitualPattern(CS$<>8__locals1.ideo, CS$<>8__locals1.pat, editMode) || IdeoUIUtility.addedPatternDefsTmp.Contains(CS$<>8__locals1.pat)))
			{
				return;
			}
			string text = IdeoUIUtility.GetPreceptLabel(CS$<>8__locals1.def, patternDef);
			bool flag = IdeoUIUtility.IsBasicPrecept(CS$<>8__locals1.def);
			if (flag)
			{
				text = CS$<>8__locals1.def.issue.LabelCap + ": " + text;
			}
			Action action = new Action(CS$<>8__locals1.<AddPreceptOption>g__AddPreceptAction|0);
			if (editMode != IdeoEditMode.Dev)
			{
				AcceptanceReport acceptanceReport = IdeoUIUtility.CanAddPrecept(CS$<>8__locals1.def, CS$<>8__locals1.pat, CS$<>8__locals1.ideo);
				if (!acceptanceReport.Accepted)
				{
					action = null;
					text = text + " (" + acceptanceReport.Reason + ")";
				}
				else if (CS$<>8__locals1.ideo.HasMaxPreceptsForIssue(CS$<>8__locals1.def.issue))
				{
					return;
				}
			}
			string label = text;
			Action action2 = action;
			Texture2D iconTex;
			if ((iconTex = ((patternDef != null) ? patternDef.Icon : null)) == null)
			{
				iconTex = (CS$<>8__locals1.def.Icon ?? CS$<>8__locals1.ideo.Icon);
			}
			options.Add(new FloatMenuOption(label, action2, iconTex, flag ? IdeoUIUtility.GetIconAndLabelColor(CS$<>8__locals1.def.impact) : CS$<>8__locals1.ideo.Color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
			if (CS$<>8__locals1.pat != null)
			{
				IdeoUIUtility.addedPatternDefsTmp.Add(CS$<>8__locals1.pat);
			}
		}

		// Token: 0x0600D572 RID: 54642 RVA: 0x003F55C1 File Offset: 0x003F37C1
		private static bool IsBasicPrecept(PreceptDef def)
		{
			return def.preceptClass == typeof(Precept) || def.issue.forceWriteLabelInPreceptFloatMenuOption;
		}

		// Token: 0x0600D573 RID: 54643 RVA: 0x003F55E7 File Offset: 0x003F37E7
		private static string GetPreceptLabel(PreceptDef def, RitualPatternDef patternDef)
		{
			return ((patternDef != null) ? patternDef.shortDescOverride.CapitalizeFirst() : null) ?? def.LabelCap;
		}

		// Token: 0x0600D574 RID: 54644 RVA: 0x003F560C File Offset: 0x003F380C
		private static AcceptanceReport CanAddPrecept(PreceptDef def, RitualPatternDef pat, Ideo ideo)
		{
			AcceptanceReport acceptanceReport = ideo.CanAddPreceptAllFactions(def);
			if (!acceptanceReport)
			{
				return acceptanceReport;
			}
			if (pat != null && !pat.ritualObligationTargetFilter.thingDefs.NullOrEmpty<ThingDef>() && !pat.ignoreConsumableBuildingRequirement)
			{
				List<ThingDef> things = pat.ritualObligationTargetFilter.thingDefs;
				bool flag = false;
				Predicate<ThingDef> <>9__0;
				foreach (MemeDef memeDef in DefDatabase<MemeDef>.AllDefs)
				{
					if (!memeDef.consumableBuildings.NullOrEmpty<ThingDef>())
					{
						List<ThingDef> consumableBuildings = memeDef.consumableBuildings;
						Predicate<ThingDef> predicate;
						if ((predicate = <>9__0) == null)
						{
							predicate = (<>9__0 = ((ThingDef x) => things.Contains(x)));
						}
						if (consumableBuildings.Any(predicate))
						{
							if (ideo.HasMeme(memeDef))
							{
								flag = true;
								break;
							}
							IdeoUIUtility.tempRequiredMemes.Add(memeDef.label);
						}
					}
				}
				if (IdeoUIUtility.tempRequiredMemes.Any<string>() && !flag)
				{
					if (IdeoUIUtility.tempRequiredMemes.Count == 1)
					{
						return "RequiresMeme".Translate() + ": " + IdeoUIUtility.tempRequiredMemes[0].CapitalizeFirst();
					}
					return "RequiresOneOfMemes".Translate() + ": " + IdeoUIUtility.tempRequiredMemes.ToCommaList(false, false).CapitalizeFirst();
				}
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x0600D575 RID: 54645 RVA: 0x003F5780 File Offset: 0x003F3980
		public static AcceptanceReport CanListPrecept(Ideo ideo, PreceptDef precept, IdeoEditMode editMode)
		{
			if (!precept.visible && !IdeoUIUtility.showAll)
			{
				return false;
			}
			if (editMode == IdeoEditMode.Dev)
			{
				return true;
			}
			return ideo.CanAddPreceptAllFactions(precept);
		}

		// Token: 0x0600D576 RID: 54646 RVA: 0x003F57AA File Offset: 0x003F39AA
		private static bool CanAddRitualPattern(Ideo ideo, RitualPatternDef pattern, IdeoEditMode editMode)
		{
			return editMode == IdeoEditMode.Dev || pattern.CanFactionUse(IdeoUIUtility.FactionForRandomization(ideo));
		}

		// Token: 0x0600D577 RID: 54647 RVA: 0x003F57C4 File Offset: 0x003F39C4
		public static void DoAppearanceItems(Ideo ideo, IdeoEditMode editMode, ref float curY, float width)
		{
			IdeoUIUtility.<>c__DisplayClass114_0 CS$<>8__locals1;
			CS$<>8__locals1.editMode = editMode;
			CS$<>8__locals1.ideo = ideo;
			Widgets.Label((width - IdeoUIUtility.PreceptBoxSize.x * 3f - 16f) / 2f, ref curY, width, "Appearance".Translate(), default(TipSignal));
			IdeoUIUtility.<DoAppearanceItems>g__DrawAppearanceItem|114_0(4f, curY, StyleItemTab.HairAndBeard, CS$<>8__locals1.ideo.style.DisplayedHairDef, ref CS$<>8__locals1);
			IdeoUIUtility.<DoAppearanceItems>g__DrawAppearanceItem|114_0(4f + IdeoUIUtility.PreceptBoxSize.x + 8f, curY, StyleItemTab.Tattoo, CS$<>8__locals1.ideo.style.DisplayedTattooDef, ref CS$<>8__locals1);
			curY += IdeoUIUtility.PreceptBoxSize.y + 17f;
		}

		// Token: 0x0600D578 RID: 54648 RVA: 0x003F5884 File Offset: 0x003F3A84
		public static FactionDef FactionForRandomization(Ideo ideo)
		{
			FactionDef result = Find.Scenario.playerFaction.factionDef;
			if (Find.World != null)
			{
				foreach (Faction faction in Find.FactionManager.AllFactionsVisible)
				{
					if (faction.ideos != null && faction.ideos.IsPrimary(ideo))
					{
						result = faction.def;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x0600D579 RID: 54649 RVA: 0x003F5908 File Offset: 0x003F3B08
		private static void DoDebugButtons(ref float curY, float width, Ideo ideo)
		{
			curY += 17f;
			if (Widgets.ButtonText(new Rect(0f, curY, 200f, 40f), "DEV: Single precept", true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				List<PreceptDef> allDefsListForReading = DefDatabase<PreceptDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					PreceptDef defLocal = allDefsListForReading[i];
					list.Add(new FloatMenuOption(defLocal.issue.LabelCap + ": " + (defLocal.LabelCap.NullOrEmpty() ? defLocal.defName : defLocal.LabelCap), delegate()
					{
						ideo.ClearPrecepts();
						ideo.AddPrecept(PreceptMaker.MakePrecept(defLocal), true, null, null);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			if (Widgets.ButtonText(new Rect(210f, curY, 200f, 40f), "DEV: test descriptions...", true, true, true, null))
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int j = 0; j < 100; j++)
				{
					ideo.RegenerateDescription(true);
					stringBuilder.Append("template: " + ideo.descriptionTemplate).AppendLine();
					stringBuilder.Append("text: " + ideo.description).AppendLine().AppendLine();
				}
				Log.Message(stringBuilder.ToString());
			}
			if (Widgets.ButtonText(new Rect(420f, curY, 200f, 40f), "DEV: Test names...", true, true, true, null))
			{
				List<FloatMenuOption> list2 = new List<FloatMenuOption>();
				list2.Add(new FloatMenuOption("Ideo names (replace)", delegate()
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					for (int k = 0; k < 200; k++)
					{
						ideo.foundation.GenerateTextSymbols();
						ideo.foundation.GenerateLeaderTitle();
						stringBuilder2.AppendLine(string.Concat(new string[]
						{
							ideo.name,
							"\n- ",
							ideo.adjective,
							"\n- ",
							ideo.memberName,
							"\n- ",
							ideo.leaderTitleMale
						}));
						stringBuilder2.AppendLine();
					}
					Log.Message(stringBuilder2.ToString());
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				using (List<Precept>.Enumerator enumerator = ideo.PreceptsListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Precept precept = enumerator.Current;
						PreceptDef def = precept.def;
						if (def.preceptClass == typeof(Precept_Ritual) || typeof(Precept_Role).IsAssignableFrom(def.preceptClass) || def.preceptClass == typeof(Precept_Building))
						{
							list2.Add(new FloatMenuOption(def.issue.LabelCap + ": " + precept.LabelCap, delegate()
							{
								StringBuilder stringBuilder2 = new StringBuilder();
								for (int k = 0; k < 200; k++)
								{
									stringBuilder2.AppendLine(precept.GenerateNameRaw());
								}
								Log.Message(stringBuilder2.ToString());
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
					}
				}
				Find.WindowStack.Add(new FloatMenu(list2));
			}
			curY += 50f;
		}

		// Token: 0x0600D57A RID: 54650 RVA: 0x003F5C54 File Offset: 0x003F3E54
		public static void DrawExtraThoughtInfoFromIdeo(Pawn pawn, ref Rect rect)
		{
			if (!ModLister.IdeologyInstalled || pawn.Ideo == null)
			{
				return;
			}
			if (pawn.Ideo.IdeoCausesHumanMeatCravings())
			{
				string value = (Find.TickManager.TicksGame - pawn.mindState.lastHumanMeatIngestedTick).ToStringTicksToPeriod(true, false, true, true, false);
				TaggedString taggedString = "LastHumanMeat".Translate() + ": " + "TimeAgo".Translate(value);
				float num = Text.CalcHeight(taggedString, rect.width);
				Rect rect2 = new Rect(rect.x, rect.yMax - num, rect.width, num);
				Widgets.Label(rect2, taggedString);
				if (Mouse.IsOver(rect2))
				{
					Widgets.DrawHighlight(rect2);
					TooltipHandler.TipRegion(rect2, "LastHumanMeatDesc".Translate(pawn.Named("PAWN"), pawn.Ideo.Named("IDEO"), 8.Named("DURATION")).Resolve());
				}
				rect.yMax -= num;
			}
		}

		// Token: 0x0600D57B RID: 54651 RVA: 0x003F5D68 File Offset: 0x003F3F68
		public static Color GetBackgroundColor(PreceptImpact impact)
		{
			switch (impact)
			{
			case PreceptImpact.Low:
				return new Color(0.13f, 0.13f, 0.13f);
			case PreceptImpact.Medium:
				return new Color(0.18f, 0.18f, 0.18f);
			case PreceptImpact.High:
				return new Color(0.24f, 0.24f, 0.24f);
			default:
				return Color.white;
			}
		}

		// Token: 0x0600D57C RID: 54652 RVA: 0x003F5DD0 File Offset: 0x003F3FD0
		public static Color GetIconAndLabelColor(PreceptImpact impact)
		{
			switch (impact)
			{
			case PreceptImpact.Low:
				return new Color(0.7f, 0.7f, 0.7f);
			case PreceptImpact.Medium:
				return new Color(1f, 1f, 1f);
			case PreceptImpact.High:
				return new Color(1f, 1f, 0.5f);
			default:
				return Color.white;
			}
		}

		// Token: 0x0600D57D RID: 54653 RVA: 0x003F5E38 File Offset: 0x003F4038
		public static bool IsPreceptRelatedForUI(MemeDef meme, PreceptDef precept)
		{
			return precept.requiredMemes.Contains(meme) || precept.associatedMemes.Contains(meme) || (meme.requireOne != null && meme.requireOne.Any((List<PreceptDef> pl) => pl.Contains(precept)));
		}

		// Token: 0x0600D57E RID: 54654 RVA: 0x003F5E9C File Offset: 0x003F409C
		public static void DrawImpactInfo(Rect rect, List<MemeDef> memes)
		{
			int num = IdeoUIUtility.ImpactOf(memes);
			if (num == 0)
			{
				return;
			}
			string text = num.ToStringCached();
			Text.Font = GameFont.Medium;
			float num2 = Mathf.Max(16f, Text.CalcSize(text).x + 2f);
			Rect rect2 = rect;
			Rect rect3 = rect.TopPartPixels(rect.height - 12f);
			Rect rect4;
			Rect rect5;
			rect3.SplitVertically(rect3.width - num2, out rect4, out rect5);
			rect5.y += 1f;
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("IdeoImpactOverallDesc".Translate());
				foreach (MemeDef memeDef in memes)
				{
					if (memeDef.impact != 0)
					{
						stringBuilder.Append("\n  - ").Append(memeDef.LabelCap).Append(": ").Append(memeDef.impact.ToStringCached());
					}
				}
				TooltipHandler.TipRegion(rect, stringBuilder.ToString());
			}
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.LowerRight;
			Widgets.Label(rect5, text);
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.LowerRight;
			Widgets.Label(rect2, IdeoImpactUtility.OverallImpactLabel(num).CapitalizeFirst());
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.LowerRight;
			Widgets.Label(rect4, "IdeoImpactOverall".Translate() + ":");
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0600D57F RID: 54655 RVA: 0x003F6040 File Offset: 0x003F4240
		private static int ImpactOf(List<MemeDef> memes)
		{
			int num = 0;
			foreach (MemeDef memeDef in memes)
			{
				num += memeDef.impact;
			}
			return num;
		}

		// Token: 0x0600D580 RID: 54656 RVA: 0x003F6094 File Offset: 0x003F4294
		public static void DrawIdeoPlate(Rect r, Ideo ideo, Pawn pawn = null)
		{
			Widgets.DrawHighlightIfMouseover(r);
			Rect rect = new Rect(r.x, r.y, r.width, r.height);
			Rect rect2 = new Rect(r.x + 1f, r.y + 1f, 20f, 20f);
			ideo.DrawIcon(rect2);
			Widgets.Label(new Rect(rect.x + rect.height + 5f, rect.y, rect.width - 10f, rect.height), ideo.name);
			if (Widgets.ButtonInvisible(r, true))
			{
				IdeoUIUtility.OpenIdeoInfo(ideo);
			}
			if (Mouse.IsOver(r))
			{
				TaggedString t = ideo.name.Colorize(ColoredText.TipSectionTitleColor);
				if (pawn != null)
				{
					t += "\n" + "Certainty".Translate().CapitalizeFirst() + ": " + pawn.ideo.Certainty.ToStringPercent();
				}
				t += "\n\n" + "ClickForMoreInfo".Translate().Colorize(ColoredText.SubtleGrayColor);
				if (pawn != null && pawn.ideo.PreviousIdeos.Any<Ideo>())
				{
					t += "\n\n" + "Formerly".Translate().CapitalizeFirst() + ": \n" + (from x in pawn.ideo.PreviousIdeos
					select x.name).ToLineList("  - ", false);
				}
				TooltipHandler.TipRegion(r, t.Resolve());
			}
		}

		// Token: 0x0600D582 RID: 54658 RVA: 0x003F6418 File Offset: 0x003F4618
		[CompilerGenerated]
		internal static void <GetStyleCategoryDescription>g__AppendDetails|73_0(ref IdeoUIUtility.<>c__DisplayClass73_0 A_0)
		{
			if (!A_0.desc.NullOrEmpty())
			{
				A_0.desc += "\n\n";
			}
			A_0.desc += ("StyleCategoryDetails".Translate(A_0.cat.label).CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor);
			A_0.showedDetails = true;
		}

		// Token: 0x0600D583 RID: 54659 RVA: 0x003F6496 File Offset: 0x003F4696
		[CompilerGenerated]
		internal static FloatMenuOption <AddPrecept>g__PostProcessOption|107_4(FloatMenuOption option, ref IdeoUIUtility.<>c__DisplayClass107_2 A_1)
		{
			if (!A_1.acceptance)
			{
				option.action = null;
				option.Label = option.Label + " (" + A_1.acceptance.Reason + ")";
			}
			return option;
		}

		// Token: 0x0600D584 RID: 54660 RVA: 0x003F64D4 File Offset: 0x003F46D4
		[CompilerGenerated]
		internal static void <DoAppearanceItems>g__DrawAppearanceItem|114_0(float xOffset, float y, StyleItemTab tab, StyleItemDef defToDisplay, ref IdeoUIUtility.<>c__DisplayClass114_0 A_4)
		{
			Rect rect = new Rect(xOffset, y, IdeoUIUtility.PreceptBoxSize.x, IdeoUIUtility.PreceptBoxSize.y);
			Rect butRect = rect;
			Color backgroundColor = IdeoUIUtility.GetBackgroundColor(PreceptImpact.Medium);
			Widgets.DrawRectFast(rect, backgroundColor, null);
			GUI.color = new Color(backgroundColor.r + 0.05f, backgroundColor.g + 0.05f, backgroundColor.b + 0.05f);
			Widgets.DrawBox(rect, 1, null);
			GUI.color = Color.white;
			string text = ((tab == StyleItemTab.HairAndBeard) ? "HairAndBeards" : "Tattoos").Translate();
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				TooltipHandler.TipRegion(rect, text.Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + ((tab == StyleItemTab.HairAndBeard) ? "HairAndBeardsDesc" : "TattoosDesc").Translate() + ((A_4.editMode != IdeoEditMode.None) ? ("\n\n" + IdeoUIUtility.ClickToEdit) : string.Empty));
			}
			rect = rect.ContractedBy(4f);
			Rect rect2 = new Rect(rect.x, rect.y, 50f, 50f);
			if (defToDisplay != null)
			{
				GUI.color = PawnHairColors.ReddishBrown;
				Widgets.DefIcon(rect2, defToDisplay, null, 1.25f, null, false, null, null, null, 1f);
				GUI.color = Color.white;
				rect.xMin = rect2.xMax + 10f;
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			GUI.color = new Color(0.8f, 0.8f, 0.8f);
			Widgets.Label(new Rect(rect.x, rect.y, rect.width, rect.height / 2f), text);
			GUI.color = Color.white;
			Widgets.Label(new Rect(rect.x, rect.y + rect.height / 2f, rect.width, rect.height / 2f), "NumAvailable".Translate(((tab == StyleItemTab.HairAndBeard) ? A_4.ideo.style.NumHairAndBeardStylesAvailable : A_4.ideo.style.NumTattooStylesAvailable).ToString()));
			Text.Anchor = TextAnchor.UpperLeft;
			if (Widgets.ButtonInvisible(butRect, true) && IdeoUIUtility.TutorAllowsInteraction(A_4.editMode))
			{
				Find.WindowStack.Add(new Dialog_EditIdeoStyleItems(A_4.ideo, tab, A_4.editMode));
			}
		}

		// Token: 0x040090DF RID: 37087
		private static bool showAll;

		// Token: 0x040090E0 RID: 37088
		public static bool devEditMode;

		// Token: 0x040090E1 RID: 37089
		public static Ideo selected;

		// Token: 0x040090E2 RID: 37090
		private static Ideo currentRitualAmbiencePreviewIdeo;

		// Token: 0x040090E3 RID: 37091
		private static Sustainer currentRitualAmbiencePreview;

		// Token: 0x040090E4 RID: 37092
		private const float IdeoIconRectSize = 30f;

		// Token: 0x040090E5 RID: 37093
		private const float IdeoIconRectGapX = 7f;

		// Token: 0x040090E6 RID: 37094
		private const float IdeoIconRectGapY = 7f;

		// Token: 0x040090E7 RID: 37095
		private const float RowMinHeight = 46f;

		// Token: 0x040090E8 RID: 37096
		private const int IconSize = 70;

		// Token: 0x040090E9 RID: 37097
		public const int CultureIconSize = 35;

		// Token: 0x040090EA RID: 37098
		private const int StyleIconSize = 28;

		// Token: 0x040090EB RID: 37099
		public const int ApparelRequirementIconSize = 24;

		// Token: 0x040090EC RID: 37100
		public const int ApparelRequirementIconPad = 2;

		// Token: 0x040090ED RID: 37101
		public const float IdeoListWidthPct = 0.25f;

		// Token: 0x040090EE RID: 37102
		private const float IdeoDetailsPad = 17f;

		// Token: 0x040090EF RID: 37103
		public const float CellPad = 6f;

		// Token: 0x040090F0 RID: 37104
		public const float ExtraSpaceBetweenRows = 4f;

		// Token: 0x040090F1 RID: 37105
		public const float TextTopPad = 2f;

		// Token: 0x040090F2 RID: 37106
		public static readonly Vector2 MemeBoxSize = new Vector2(122f, 120f);

		// Token: 0x040090F3 RID: 37107
		private const float MemeIconSize = 80f;

		// Token: 0x040090F4 RID: 37108
		private const int MemeIconMargin = 8;

		// Token: 0x040090F5 RID: 37109
		public static readonly Vector2 PreceptBoxSize = new Vector2(220f, 60f);

		// Token: 0x040090F6 RID: 37110
		public const float PreceptIconSize = 50f;

		// Token: 0x040090F7 RID: 37111
		public const int MaxPreceptsPerRow = 3;

		// Token: 0x040090F8 RID: 37112
		public const int GapBetweenBoxes = 8;

		// Token: 0x040090F9 RID: 37113
		private const int DescriptionTextBoxHeight = 70;

		// Token: 0x040090FA RID: 37114
		private const int DescriptionTextBoxPadding = 40;

		// Token: 0x040090FB RID: 37115
		public static readonly Vector2 ButtonSize = new Vector2(145f, 30f);

		// Token: 0x040090FC RID: 37116
		private static readonly Vector2 AddPreceptButtonSize = new Vector2(175f, 30f);

		// Token: 0x040090FD RID: 37117
		private static readonly Color TutorArrowColor = new Color(0.937f, 0.847f, 0f);

		// Token: 0x040090FE RID: 37118
		private static readonly Texture2D ArrowTex = ContentFinder<Texture2D>.Get("UI/Overlays/TutorArrowRight", true);

		// Token: 0x040090FF RID: 37119
		private static readonly Texture2D PlusTex = ContentFinder<Texture2D>.Get("UI/Buttons/Plus", true);

		// Token: 0x04009100 RID: 37120
		public static readonly Texture2D UnlockedTex = ContentFinder<Texture2D>.Get("UI/Overlays/LockedMonochrome", true);

		// Token: 0x04009101 RID: 37121
		public static readonly Texture2D LockedTex = ContentFinder<Texture2D>.Get("UI/Overlays/Locked", true);

		// Token: 0x04009102 RID: 37122
		public static readonly Texture2D PreviewRitualAmbience = ContentFinder<Texture2D>.Get("UI/Buttons/PreviewSound_NotPlaying", true);

		// Token: 0x04009103 RID: 37123
		public static readonly Texture2D PreviewRitualAmbienceOn = ContentFinder<Texture2D>.Get("UI/Buttons/PreviewSound_Playing", true);

		// Token: 0x04009104 RID: 37124
		private static readonly Vector2 SaveLoadButtonSize = new Vector2(70f, 30f);

		// Token: 0x04009105 RID: 37125
		private const int FluidIconSize = 25;

		// Token: 0x04009106 RID: 37126
		private static Texture2D fluidIdeoIcon;

		// Token: 0x04009107 RID: 37127
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x04009108 RID: 37128
		private static List<ThingStyleCategoryWithPriority> tmpStyleCategories = new List<ThingStyleCategoryWithPriority>();

		// Token: 0x04009109 RID: 37129
		private static List<IdeoUIUtility.StyleCatOverride> tmpOverrides = new List<IdeoUIUtility.StyleCatOverride>();

		// Token: 0x0400910A RID: 37130
		private static List<Precept_Ritual> fluidIdeoRituals = new List<Precept_Ritual>();

		// Token: 0x0400910B RID: 37131
		private static List<HistoryEventDef> fluidIdeoQuestSuccessEvents = new List<HistoryEventDef>();

		// Token: 0x0400910C RID: 37132
		private static List<Faction> tmpFactions = new List<Faction>();

		// Token: 0x0400910D RID: 37133
		private static List<MemeDef> tmpMemesToShow = new List<MemeDef>();

		// Token: 0x0400910E RID: 37134
		private static MemeDef tmpMouseOverMeme = null;

		// Token: 0x0400910F RID: 37135
		private static List<PreceptDef> tmpRequiredPrecepts = new List<PreceptDef>();

		// Token: 0x04009110 RID: 37136
		private static List<string> tmpSortedLabelCaps = new List<string>();

		// Token: 0x04009111 RID: 37137
		private static List<Precept> tmpPrecepts = new List<Precept>();

		// Token: 0x04009112 RID: 37138
		private static List<ThingDef> tmpUsedThingDefs = new List<ThingDef>();

		// Token: 0x04009113 RID: 37139
		private static List<ThingDef> tmpAllowedThingDefs = new List<ThingDef>();

		// Token: 0x04009114 RID: 37140
		private static List<ThingDef> tmpAllThingDefs = new List<ThingDef>();

		// Token: 0x04009115 RID: 37141
		private static List<string> tempRequiredMemes = new List<string>();

		// Token: 0x04009116 RID: 37142
		private static PreceptDef tmpMouseOverPrecept = null;

		// Token: 0x04009117 RID: 37143
		private static List<RitualPatternDef> addedPatternDefsTmp = new List<RitualPatternDef>();

		// Token: 0x04009118 RID: 37144
		private static readonly Dictionary<IssueDef, List<FloatMenuOption>> issueGroupsTmp = new Dictionary<IssueDef, List<FloatMenuOption>>();

		// Token: 0x04009119 RID: 37145
		private static readonly Dictionary<IssueDef, PreceptDef> singlePreceptsTmp = new Dictionary<IssueDef, PreceptDef>();

		// Token: 0x0400911A RID: 37146
		private static readonly Dictionary<ThingDef, List<FloatMenuOption>> thingGroupsTmp = new Dictionary<ThingDef, List<FloatMenuOption>>();

		// Token: 0x020025FB RID: 9723
		private struct StyleCatOverride
		{
			// Token: 0x0600D585 RID: 54661 RVA: 0x003F675B File Offset: 0x003F495B
			public StyleCatOverride(string label, StyleCategoryDef style)
			{
				this.label = label;
				this.style = style;
			}

			// Token: 0x0400911B RID: 37147
			public string label;

			// Token: 0x0400911C RID: 37148
			public StyleCategoryDef style;
		}
	}
}
