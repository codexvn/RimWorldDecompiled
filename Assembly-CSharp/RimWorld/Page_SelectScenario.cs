using System;
using System.Collections.Generic;
using System.Linq;
using Steamworks;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x0200329E RID: 12958
	[StaticConstructorOnStartup]
	public class Page_SelectScenario : Page
	{
		// Token: 0x17002F07 RID: 12039
		// (get) Token: 0x060123BA RID: 74682 RVA: 0x0055C1C9 File Offset: 0x0055A3C9
		public override string PageTitle
		{
			get
			{
				return "ChooseScenario".Translate();
			}
		}

		// Token: 0x060123BB RID: 74683 RVA: 0x0055C1DA File Offset: 0x0055A3DA
		public override void PreOpen()
		{
			base.PreOpen();
			this.infoScrollPosition = Vector2.zero;
			ScenarioLister.MarkDirty();
			this.EnsureValidSelection();
		}

		// Token: 0x060123BC RID: 74684 RVA: 0x0055C1F8 File Offset: 0x0055A3F8
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			Rect mainRect = base.GetMainRect(rect, 0f, false);
			Widgets.BeginGroup(mainRect);
			Rect rect2 = new Rect(0f, 0f, mainRect.width * 0.35f, mainRect.height).Rounded();
			this.DoScenarioSelectionList(rect2);
			ScenarioUI.DrawScenarioInfo(new Rect(rect2.xMax + 17f, 0f, mainRect.width - rect2.width - 17f, mainRect.height).Rounded(), this.curScen, ref this.infoScrollPosition);
			Widgets.EndGroup();
			base.DoBottomButtons(rect, null, "ScenarioEditor".Translate(), new Action(this.GoToScenarioEditor), true, true);
		}

		// Token: 0x060123BD RID: 74685 RVA: 0x0055C2C3 File Offset: 0x0055A4C3
		private bool CanEditScenario(Scenario scen)
		{
			return scen.Category == ScenarioCategory.CustomLocal || scen.CanToUploadToWorkshop();
		}

		// Token: 0x060123BE RID: 74686 RVA: 0x0055C2DC File Offset: 0x0055A4DC
		private void GoToScenarioEditor()
		{
			Page_ScenarioEditor page_ScenarioEditor = new Page_ScenarioEditor(this.CanEditScenario(this.curScen) ? this.curScen : this.curScen.CopyForEditing());
			page_ScenarioEditor.prev = this;
			Find.WindowStack.Add(page_ScenarioEditor);
			this.Close(true);
		}

		// Token: 0x060123BF RID: 74687 RVA: 0x0055C32C File Offset: 0x0055A52C
		private void DoScenarioSelectionList(Rect rect)
		{
			rect.xMax += 2f;
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f - 2f, this.totalScenarioListHeight + 250f);
			Widgets.BeginScrollView(rect, ref this.scenariosScrollPosition, rect2, true);
			Rect rect3 = rect2.AtZero();
			rect3.height = 999999f;
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = rect2.width;
			listing_Standard.Begin(rect3);
			Text.Font = GameFont.Small;
			this.ListScenariosOnListing(listing_Standard, ScenarioLister.ScenariosInCategory(ScenarioCategory.FromDef));
			listing_Standard.Gap(12f);
			Text.Font = GameFont.Small;
			listing_Standard.Label("ScenariosCustom".Translate(), -1f, null);
			this.ListScenariosOnListing(listing_Standard, ScenarioLister.ScenariosInCategory(ScenarioCategory.CustomLocal));
			listing_Standard.Gap(12f);
			Text.Font = GameFont.Small;
			listing_Standard.Label("ScenariosSteamWorkshop".Translate(), -1f, null);
			if (listing_Standard.ButtonText("OpenSteamWorkshop".Translate(), null, 1f))
			{
				SteamUtility.OpenSteamWorkshopPage();
			}
			this.ListScenariosOnListing(listing_Standard, ScenarioLister.ScenariosInCategory(ScenarioCategory.SteamWorkshop));
			listing_Standard.End();
			this.totalScenarioListHeight = listing_Standard.CurHeight;
			Widgets.EndScrollView();
		}

		// Token: 0x060123C0 RID: 74688 RVA: 0x0055C470 File Offset: 0x0055A670
		private void ListScenariosOnListing(Listing_Standard listing, IEnumerable<Scenario> scenarios)
		{
			bool flag = false;
			foreach (Scenario scenario in scenarios)
			{
				if (scenario.showInUI)
				{
					if (flag)
					{
						listing.Gap(6f);
					}
					Scenario scen = scenario;
					Rect rect = listing.GetRect(68f, 1f).ContractedBy(4f);
					this.DoScenarioListEntry(rect, scen);
					flag = true;
				}
			}
			if (!flag)
			{
				GUI.color = new Color(1f, 1f, 1f, 0.5f);
				listing.Label("(" + "NoneLower".Translate() + ")", -1f, null);
				GUI.color = Color.white;
			}
		}

		// Token: 0x060123C1 RID: 74689 RVA: 0x0055C548 File Offset: 0x0055A748
		private void DoScenarioListEntry(Rect rect, Scenario scen)
		{
			bool flag = this.curScen == scen;
			if (!scen.valid)
			{
				TooltipHandler.TipRegion(rect, "ScenPart_Error".Translate().Colorize(ColorLibrary.RedReadable));
			}
			Widgets.DrawOptionBackground(rect, flag);
			MouseoverSounds.DoRegion(rect);
			Rect rect2 = rect.ContractedBy(4f);
			Text.Font = GameFont.Small;
			Rect rect3 = rect2;
			rect3.height = Text.CalcHeight(scen.name, rect3.width);
			Widgets.Label(rect3, scen.name);
			Text.Font = GameFont.Tiny;
			Rect rect4 = rect2;
			rect4.yMin = rect3.yMax;
			if (!Text.TinyFontSupported)
			{
				rect4.yMin -= 6f;
				rect4.height += 6f;
			}
			Widgets.Label(rect4, scen.GetSummary());
			if (!scen.enabled)
			{
				return;
			}
			WidgetRow widgetRow = new WidgetRow(rect.xMax, rect.y, UIDirection.LeftThenDown, 99999f, 4f);
			if (scen.Category == ScenarioCategory.CustomLocal && widgetRow.ButtonIcon(TexButton.Delete, "Delete".Translate(), new Color?(GenUI.SubtleMouseoverColor), null, null, true, -1f))
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmDelete".Translate(scen.File.Name), delegate()
				{
					scen.File.Delete();
					ScenarioLister.MarkDirty();
				}, true, null, WindowLayer.Dialog));
			}
			if (scen.Category == ScenarioCategory.SteamWorkshop && widgetRow.ButtonIcon(TexButton.Delete, "Unsubscribe".Translate(), new Color?(GenUI.SubtleMouseoverColor), null, null, true, -1f))
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmUnsubscribeFrom".Translate(scen.File.Name), delegate()
				{
					scen.enabled = false;
					if (this.curScen == scen)
					{
						this.curScen = null;
						this.EnsureValidSelection();
					}
					Workshop.Unsubscribe(scen);
				}, true, null, WindowLayer.Dialog));
			}
			if (scen.GetPublishedFileId() != PublishedFileId_t.Invalid)
			{
				if (widgetRow.ButtonIcon(ContentSource.SteamWorkshop.GetIcon(), "WorkshopPage".Translate(), null, null, null, true, -1f))
				{
					SteamUtility.OpenWorkshopPage(scen.GetPublishedFileId());
				}
				if (scen.CanToUploadToWorkshop())
				{
					widgetRow.Icon(Page_SelectScenario.CanUploadIcon, "CanBeUpdatedOnWorkshop".Translate(), 0f);
				}
			}
			if (!scen.valid)
			{
				widgetRow.Icon(Page_SelectScenario.WarningIcon, null, 3f);
			}
			if (!flag && Widgets.ButtonInvisible(rect, true))
			{
				SoundDefOf.Click.PlayOneShotOnCamera(null);
				if (!scen.valid)
				{
					PreLoadUtility.CheckVersionAndLoad(scen.File.FullName, ScribeMetaHeaderUtility.ScribeHeaderMode.Scenario, delegate
					{
						this.curScen = scen;
					}, false);
					return;
				}
				this.curScen = scen;
			}
		}

		// Token: 0x060123C2 RID: 74690 RVA: 0x0055C89A File Offset: 0x0055AA9A
		protected override bool CanDoNext()
		{
			if (!base.CanDoNext())
			{
				return false;
			}
			if (this.curScen == null)
			{
				return false;
			}
			Page_SelectScenario.BeginScenarioConfiguration(this.curScen, this);
			return true;
		}

		// Token: 0x060123C3 RID: 74691 RVA: 0x0055C8C0 File Offset: 0x0055AAC0
		public static void BeginScenarioConfiguration(Scenario scen, Page originPage)
		{
			Game.ClearCaches();
			Current.Game = new Game();
			Current.Game.InitData = new GameInitData();
			Current.Game.Scenario = scen;
			Current.Game.Scenario.PreConfigure();
			Find.GameInitData.startedFromEntry = true;
			Page firstConfigPage = Current.Game.Scenario.GetFirstConfigPage();
			if (firstConfigPage == null)
			{
				PageUtility.InitGameStart();
				return;
			}
			originPage.next = firstConfigPage;
			firstConfigPage.prev = originPage;
		}

		// Token: 0x060123C4 RID: 74692 RVA: 0x0055C937 File Offset: 0x0055AB37
		private void EnsureValidSelection()
		{
			if (this.curScen == null || !ScenarioLister.ScenarioIsListedAnywhere(this.curScen))
			{
				this.curScen = ScenarioLister.ScenariosInCategory(ScenarioCategory.FromDef).FirstOrDefault<Scenario>();
			}
		}

		// Token: 0x060123C5 RID: 74693 RVA: 0x0055C960 File Offset: 0x0055AB60
		internal void Notify_ScenarioListChanged()
		{
			PublishedFileId_t selModId = this.curScen.GetPublishedFileId();
			this.curScen = ScenarioLister.AllScenarios().FirstOrDefault((Scenario sc) => sc.GetPublishedFileId() == selModId);
			this.EnsureValidSelection();
		}

		// Token: 0x060123C6 RID: 74694 RVA: 0x0055C9A6 File Offset: 0x0055ABA6
		internal void Notify_SteamItemUnsubscribed(PublishedFileId_t pfid)
		{
			if (this.curScen != null && this.curScen.GetPublishedFileId() == pfid)
			{
				this.curScen = null;
			}
			this.EnsureValidSelection();
		}

		// Token: 0x0400BDC1 RID: 48577
		private Scenario curScen;

		// Token: 0x0400BDC2 RID: 48578
		private Vector2 infoScrollPosition = Vector2.zero;

		// Token: 0x0400BDC3 RID: 48579
		private const float ScenarioEntryHeight = 68f;

		// Token: 0x0400BDC4 RID: 48580
		private static readonly Texture2D CanUploadIcon = ContentFinder<Texture2D>.Get("UI/Icons/ContentSources/CanUpload", true);

		// Token: 0x0400BDC5 RID: 48581
		private static readonly Texture2D WarningIcon = Resources.Load<Texture2D>("Textures/UI/Widgets/YellowWarning");

		// Token: 0x0400BDC6 RID: 48582
		private Vector2 scenariosScrollPosition = Vector2.zero;

		// Token: 0x0400BDC7 RID: 48583
		private float totalScenarioListHeight;
	}
}
