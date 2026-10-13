using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x020031A1 RID: 12705
	[StaticConstructorOnStartup]
	public class Dialog_Options : Window
	{
		// Token: 0x17002E75 RID: 11893
		// (get) Token: 0x06011E01 RID: 73217 RVA: 0x00530955 File Offset: 0x0052EB55
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(650f, 600f);
			}
		}

		// Token: 0x06011E02 RID: 73218 RVA: 0x00530966 File Offset: 0x0052EB66
		public Dialog_Options(OptionCategoryDef initialCategory) : this()
		{
			this.selectedCategory = initialCategory;
			this.selectedMod = null;
		}

		// Token: 0x06011E03 RID: 73219 RVA: 0x0053097C File Offset: 0x0052EB7C
		public Dialog_Options(Mod initialMod) : this()
		{
			this.selectedCategory = null;
			this.selectedMod = initialMod;
		}

		// Token: 0x06011E04 RID: 73220 RVA: 0x00530994 File Offset: 0x0052EB94
		public Dialog_Options() : base(null)
		{
			this.doCloseX = true;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06011E05 RID: 73221 RVA: 0x005309EC File Offset: 0x0052EBEC
		public override void PostOpen()
		{
			base.PostOpen();
			this.quickSearchWidget.Reset();
			this.cachedModsWithSettings = from mod in LoadedModManager.ModHandles
			where !mod.SettingsCategory().NullOrEmpty()
			orderby mod.SettingsCategory()
			select mod;
			foreach (OptionCategoryDef optionCategoryDef in DefDatabase<OptionCategoryDef>.AllDefs)
			{
				if (!optionCategoryDef.modContentPack.IsOfficialMod)
				{
					Log.Error("Unofficial OptionCategoryDef: " + optionCategoryDef.label + " found, ignoring");
				}
			}
			this.hasModSettings = LoadedModManager.ModHandles.Any((Mod mod) => !mod.SettingsCategory().NullOrEmpty());
		}

		// Token: 0x06011E06 RID: 73222 RVA: 0x00530AEC File Offset: 0x0052ECEC
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			IEnumerable<OptionCategoryDef> allDefs = DefDatabase<OptionCategoryDef>.AllDefs;
			Text.Anchor = TextAnchor.MiddleLeft;
			int num = 0;
			foreach (OptionCategoryDef optionCategoryDef in allDefs)
			{
				if ((Prefs.DevMode || !optionCategoryDef.isDev) && optionCategoryDef.modContentPack.IsOfficialMod && (optionCategoryDef != OptionCategoryDefOf.Mods || this.hasModSettings))
				{
					Rect rect = new Rect(0f, (float)num * 50f, 160f, 48f);
					this.DoCategoryRow(rect.ContractedBy(4f), optionCategoryDef);
					num++;
				}
			}
			Text.Anchor = TextAnchor.UpperLeft;
			float num2 = 60f;
			Rect inRect2 = new Rect(177f, 0f, inRect.width - 160f - 17f, inRect.height - num2);
			this.DoOptions(this.selectedCategory, inRect2);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			if (Widgets.ButtonText(new Rect(inRect.width / 2f - Dialog_Options.OkayButtonSize.x / 2f, inRect.yMax - Dialog_Options.OkayButtonSize.y, Dialog_Options.OkayButtonSize.x, Dialog_Options.OkayButtonSize.y), "OK".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06011E07 RID: 73223 RVA: 0x00530C70 File Offset: 0x0052EE70
		private void DoCategoryRow(Rect r, OptionCategoryDef optionCategory)
		{
			Widgets.DrawOptionBackground(r, optionCategory == this.selectedCategory);
			if (Widgets.ButtonInvisible(r, true))
			{
				this.selectedCategory = optionCategory;
				this.selectedMod = null;
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			float num = r.x + 10f;
			Rect position = new Rect(num, r.y + (r.height - 20f) / 2f, 20f, 20f);
			Texture2D image = ContentFinder<Texture2D>.Get(optionCategory.texPath, true);
			GUI.DrawTexture(position, image);
			num += 30f;
			Widgets.Label(new Rect(num, r.y, r.width - num, r.height), optionCategory.label.CapitalizeFirst());
		}

		// Token: 0x06011E08 RID: 73224 RVA: 0x00530D30 File Offset: 0x0052EF30
		private void DoOptions(OptionCategoryDef category, Rect inRect)
		{
			bool flag = this.optionsViewRectHeight > inRect.height;
			Rect outRect = new Rect(inRect);
			Rect viewRect = new Rect(outRect.x, outRect.y, outRect.width - (flag ? 26f : 0f), this.optionsViewRectHeight);
			Widgets.BeginScrollView(outRect, ref this.optionsScrollPosition, viewRect, true);
			Listing_Standard listing_Standard = new Listing_Standard();
			Rect rect = new Rect(viewRect.x, viewRect.y, viewRect.width, 999999f);
			listing_Standard.Begin(rect);
			listing_Standard.verticalSpacing = 5f;
			listing_Standard.Gap(12f);
			if (category == OptionCategoryDefOf.General)
			{
				this.DoGeneralOptions(listing_Standard);
			}
			else if (category == OptionCategoryDefOf.Graphics)
			{
				this.DoVideoOptions(listing_Standard);
			}
			else if (category == OptionCategoryDefOf.Audio)
			{
				this.DoAudioOptions(listing_Standard);
			}
			else if (category == OptionCategoryDefOf.Gameplay)
			{
				this.DoGameplayOptions(listing_Standard);
			}
			else if (category == OptionCategoryDefOf.Interface)
			{
				this.DoUIOptions(listing_Standard);
			}
			else if (category == OptionCategoryDefOf.Controls)
			{
				this.DoControlsOptions(listing_Standard);
			}
			else if (category == OptionCategoryDefOf.Dev)
			{
				this.DoDevOptions(listing_Standard);
			}
			else if (category == OptionCategoryDefOf.Mods)
			{
				this.DoModOptions(listing_Standard);
			}
			if (this.selectedMod != null)
			{
				float num = 15f;
				Rect inRect2 = new Rect(0f, num, viewRect.width, inRect.height - num);
				Mod mod = this.selectedMod;
				if (mod != null)
				{
					mod.DoSettingsWindowContents(inRect2);
				}
			}
			this.optionsViewRectHeight = listing_Standard.CurHeight;
			listing_Standard.End();
			Widgets.EndScrollView();
		}

		// Token: 0x06011E09 RID: 73225 RVA: 0x00530EB8 File Offset: 0x0052F0B8
		private void DoGeneralOptions(Listing_Standard listing)
		{
			if (listing.ButtonTextLabeledPct("ChooseLanguage".Translate(), LanguageDatabase.activeLanguage.DisplayName, 0.6f, TextAnchor.MiddleLeft, null, null, Dialog_Options.LanguageIconTex))
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					Messages.Message("ChangeLanguageFromMainMenu".Translate(), MessageTypeDefOf.RejectInput, false);
					SoundDefOf.Click.PlayOneShotOnCamera(null);
				}
				else
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (LoadedLanguage localLang2 in LanguageDatabase.AllLoadedLanguages)
					{
						LoadedLanguage localLang = localLang2;
						list.Add(new FloatMenuOption(localLang.DisplayName, delegate()
						{
							LanguageDatabase.SelectLanguage(localLang);
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
			}
			float autosaveIntervalDays = Prefs.AutosaveIntervalDays;
			string text = "Days".Translate();
			string text2 = "day".Translate();
			if (listing.ButtonTextLabeledPct("AutosaveInterval".Translate(), autosaveIntervalDays.ToString() + " " + ((autosaveIntervalDays == 1f) ? text2 : text), 0.6f, TextAnchor.MiddleLeft, null, "AutosaveIntervalTooltip".Translate(), null))
			{
				List<FloatMenuOption> list2 = new List<FloatMenuOption>();
				if (Prefs.DevMode)
				{
					list2.Add(new FloatMenuOption("0.05 " + text + "(debug)", delegate()
					{
						Prefs.AutosaveIntervalDays = 0.05f;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					list2.Add(new FloatMenuOption("0.075 " + text + "(debug)", delegate()
					{
						Prefs.AutosaveIntervalDays = 0.075f;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					list2.Add(new FloatMenuOption("0.1 " + text + "(debug)", delegate()
					{
						Prefs.AutosaveIntervalDays = 0.1f;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					list2.Add(new FloatMenuOption("0.125 " + text + "(debug)", delegate()
					{
						Prefs.AutosaveIntervalDays = 0.125f;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					list2.Add(new FloatMenuOption("0.25 " + text + "(debug)", delegate()
					{
						Prefs.AutosaveIntervalDays = 0.25f;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				list2.Add(new FloatMenuOption("0.5 " + text, delegate()
				{
					Prefs.AutosaveIntervalDays = 0.5f;
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				list2.Add(new FloatMenuOption(1.ToString() + " " + text2, delegate()
				{
					Prefs.AutosaveIntervalDays = 1f;
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				list2.Add(new FloatMenuOption(3.ToString() + " " + text, delegate()
				{
					Prefs.AutosaveIntervalDays = 3f;
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				list2.Add(new FloatMenuOption(7.ToString() + " " + text, delegate()
				{
					Prefs.AutosaveIntervalDays = 7f;
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				list2.Add(new FloatMenuOption(14.ToString() + " " + text, delegate()
				{
					Prefs.AutosaveIntervalDays = 14f;
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				Find.WindowStack.Add(new FloatMenu(list2));
			}
			if (Current.ProgramState == ProgramState.Playing && Current.Game.Info.permadeathMode && Prefs.AutosaveIntervalDays > 1f)
			{
				GUI.color = Color.red;
				listing.Label("MaxPermadeathAutosaveIntervalInfo".Translate(1f), -1f, null);
				GUI.color = Color.white;
			}
			int autosavesCount = Prefs.AutosavesCount;
			Prefs.AutosavesCount = Mathf.RoundToInt(listing.SliderLabeled("AutosavesCount".Translate(autosavesCount), (float)autosavesCount, 1f, 25f, 0.6f, null));
			bool runInBackground = Prefs.RunInBackground;
			listing.CheckboxLabeled("RunInBackground".Translate(), ref runInBackground, null, 30f, 0.6f);
			Prefs.RunInBackground = runInBackground;
			if (!DevModePermanentlyDisabledUtility.Disabled || Prefs.DevMode)
			{
				bool devMode = Prefs.DevMode;
				listing.CheckboxLabeled("DevelopmentMode".Translate(), ref devMode, null, 30f, 0.6f);
				Prefs.DevMode = devMode;
			}
			if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
			{
				listing.verticalSpacing = 0f;
				if (listing.ButtonTextLabeledPct("SaveGameDataFolder".Translate(), "OpenFolder".Translate(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
				{
					Application.OpenURL(GenFilePaths.SavedGamesFolderPath);
					SoundDefOf.Click.PlayOneShotOnCamera(null);
				}
				listing.SubLabel(GenFilePaths.SavedGamesFolderPath, 0.6f);
				listing.Gap(12f);
				if (listing.ButtonTextLabeledPct("LogFileFolder".Translate(), "OpenFolder".Translate(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
				{
					Application.OpenURL(Application.persistentDataPath);
					SoundDefOf.Click.PlayOneShotOnCamera(null);
				}
				listing.SubLabel(Application.persistentDataPath, 0.6f);
				listing.Gap(12f);
				listing.verticalSpacing = 5f;
			}
			else
			{
				if (listing.ButtonTextLabeledPct("SaveGameDataLocation".Translate(), "ShowFolder".Translate(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
				{
					Find.WindowStack.Add(new Dialog_MessageBox(Path.GetFullPath(GenFilePaths.SaveDataFolderPath), null, null, null, null, null, false, null, null, WindowLayer.Dialog));
				}
				if (listing.ButtonTextLabeledPct("LogFileLocation".Translate(), "ShowFolder".Translate(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
				{
					Find.WindowStack.Add(new Dialog_MessageBox(Path.GetFullPath(Application.consoleLogPath), null, null, null, null, null, false, null, null, WindowLayer.Dialog));
				}
			}
			if (listing.ButtonTextLabeledPct("RestoreToDefaultSettingsLabel".Translate(), "RestoreToDefaultSettings".Translate(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				WindowStack windowStack = Find.WindowStack;
				TaggedString text3 = "ResetAndRestartConfirmationDialog".Translate();
				Action buttonAAction = new Action(this.RestoreToDefaultSettings);
				windowStack.Add(new Dialog_MessageBox(text3, "Yes".Translate(), buttonAAction, "No".Translate(), null, null, false, null, null, WindowLayer.Dialog));
			}
		}

		// Token: 0x06011E0A RID: 73226 RVA: 0x00531648 File Offset: 0x0052F848
		private void DoVideoOptions(Listing_Standard listing)
		{
			if (listing.ButtonTextLabeledPct("Resolution".Translate(), Dialog_Options.ResToString(Screen.width, Screen.height), 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				using (IEnumerator<Resolution> enumerator = (from x in UnityGUIBugsFixer.ScreenResolutionsWithoutDuplicates
				where x.width >= 1024 && x.height >= 768
				orderby x.width, x.height
				select x).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Resolution res = enumerator.Current;
						list.Add(new FloatMenuOption(Dialog_Options.ResToString(res.width, res.height), delegate()
						{
							if (!ResolutionUtility.UIScaleSafeWithResolution(Prefs.UIScale, res.width, res.height))
							{
								Messages.Message("MessageScreenResTooSmallForUIScale".Translate(), MessageTypeDefOf.RejectInput, false);
								return;
							}
							ResolutionUtility.SafeSetResolution(res);
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
				}
				if (!list.Any<FloatMenuOption>())
				{
					list.Add(new FloatMenuOption("NoneBrackets".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			if (ResolutionUtility.BorderlessFullscreen)
			{
				listing.Label("BorderlessFullscreen".Translate(), -1f, null);
			}
			else
			{
				bool fullScreen = Screen.fullScreen;
				bool flag = fullScreen;
				listing.CheckboxLabeled("Fullscreen".Translate(), ref fullScreen, null, 30f, 0.6f);
				if (fullScreen != flag)
				{
					ResolutionUtility.SafeSetFullscreen(fullScreen);
				}
				if (!SteamDeck.IsSteamDeck && listing.ButtonText("BorderlessFullscreen".Translate(), null, 1f))
				{
					Find.WindowStack.Add(new Dialog_MessageBox("BorderlessFullscreenInstructions".Translate(), null, null, null, null, null, false, null, null, WindowLayer.Dialog));
				}
			}
			bool textureCompression = Prefs.TextureCompression;
			listing.CheckboxLabeled("TextureCompression".Translate(), ref textureCompression, "TextureCompression_Tooltip".Translate(), 30f, 0.6f);
			if (textureCompression != Prefs.TextureCompression)
			{
				Find.WindowStack.Add(new Dialog_MessageBox("ChangedTextureCompressionRestart".Translate(), "Yes".Translate(), delegate()
				{
					Prefs.TextureCompression = textureCompression;
					Prefs.Save();
					GenCommandLine.Restart();
				}, "No".Translate(), null, null, false, null, null, WindowLayer.Dialog));
			}
			Prefs.TextureCompression = textureCompression;
			bool plantWindSway = Prefs.PlantWindSway;
			listing.CheckboxLabeled("PlantWindSway".Translate(), ref plantWindSway, null, 30f, 0.6f);
			Prefs.PlantWindSway = plantWindSway;
			float screenShakeIntensity = Prefs.ScreenShakeIntensity;
			Prefs.ScreenShakeIntensity = (float)Math.Round((double)listing.SliderLabeled(string.Format("{0}: {1}", "ScreenShakeIntensity".Translate(), screenShakeIntensity.ToStringPercent("N0")), screenShakeIntensity, 0f, 2f, 0.6f, null), 1);
			bool smoothCameraJumps = Prefs.SmoothCameraJumps;
			listing.CheckboxLabeled("SmoothCameraJumps".Translate(), ref smoothCameraJumps, "SmoothCameraJumpsDesc".Translate(), 30f, 0.6f);
			Prefs.SmoothCameraJumps = smoothCameraJumps;
		}

		// Token: 0x06011E0B RID: 73227 RVA: 0x005319C0 File Offset: 0x0052FBC0
		private void DoAudioOptions(Listing_Standard listing)
		{
			Prefs.VolumeMaster = listing.SliderLabeled(string.Format("{0}: {1}", "MasterVolume".Translate(), Prefs.VolumeMaster.ToStringPercent()), Prefs.VolumeMaster, 0f, 1f, 0.6f, "MasterVolumeTooltip".Translate());
			Prefs.VolumeGame = listing.SliderLabeled(string.Format("{0}: {1}", "GameVolume".Translate(), Prefs.VolumeGame.ToStringPercent()), Prefs.VolumeGame, 0f, 1f, 0.6f, "GameVolumeTooltip".Translate());
			Prefs.VolumeMusic = listing.SliderLabeled(string.Format("{0}: {1}", "MusicVolume".Translate(), Prefs.VolumeMusic.ToStringPercent()), Prefs.VolumeMusic, 0f, 1f, 0.6f, "MusicVolumeTooltip".Translate());
			Prefs.VolumeAmbient = listing.SliderLabeled(string.Format("{0}: {1}", "AmbientVolume".Translate(), Prefs.VolumeAmbient.ToStringPercent()), Prefs.VolumeAmbient, 0f, 1f, 0.6f, "AmbientVolumeTooltip".Translate());
			Prefs.VolumeUI = listing.SliderLabeled(string.Format("{0}: {1}", "UIVolume".Translate(), Prefs.VolumeUI.ToStringPercent()), Prefs.VolumeUI, 0f, 1f, 0.6f, "UIVolumeTooltip".Translate());
		}

		// Token: 0x06011E0C RID: 73228 RVA: 0x00531B64 File Offset: 0x0052FD64
		private void DoGameplayOptions(Listing_Standard listing)
		{
			if (Current.ProgramState == ProgramState.Playing && listing.ButtonTextLabeledPct("ChangeStoryteller".Translate(), "Modify".Translate(), 0.6f, TextAnchor.UpperLeft, "OptionsButton-ChooseStoryteller", null, null) && TutorSystem.AllowAction("ChooseStoryteller"))
			{
				Find.WindowStack.Add(new Page_SelectStorytellerInGame());
			}
			int maxNumberOfPlayerSettlements = Prefs.MaxNumberOfPlayerSettlements;
			int num = Mathf.RoundToInt(listing.SliderLabeled("MaxNumberOfPlayerSettlements".Translate(maxNumberOfPlayerSettlements), (float)maxNumberOfPlayerSettlements, 1f, 5f, 0.6f, null));
			Prefs.MaxNumberOfPlayerSettlements = num;
			if (maxNumberOfPlayerSettlements != num && num > 1)
			{
				TutorUtility.DoModalDialogIfNotKnown(ConceptDefOf.MaxNumberOfPlayerSettlements, Array.Empty<string>());
			}
			bool pauseOnLoad = Prefs.PauseOnLoad;
			listing.CheckboxLabeled("PauseOnLoad".Translate(), ref pauseOnLoad, null, 30f, 0.6f);
			Prefs.PauseOnLoad = pauseOnLoad;
			AutomaticPauseMode automaticPauseMode = Prefs.AutomaticPauseMode;
			if (listing.ButtonTextLabeledPct("AutomaticPauseModeSetting".Translate(), Prefs.AutomaticPauseMode.ToStringHuman(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (object obj in Enum.GetValues(typeof(AutomaticPauseMode)))
				{
					AutomaticPauseMode localPmode2 = (AutomaticPauseMode)obj;
					AutomaticPauseMode localPmode = localPmode2;
					list.Add(new FloatMenuOption(localPmode.ToStringHuman(), delegate()
					{
						Prefs.AutomaticPauseMode = localPmode;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			Prefs.AutomaticPauseMode = automaticPauseMode;
			bool adaptiveTrainingEnabled = Prefs.AdaptiveTrainingEnabled;
			listing.CheckboxLabeled("LearningHelper".Translate(), ref adaptiveTrainingEnabled, "LearningHelperTooltip".Translate(), 30f, 0.6f);
			Prefs.AdaptiveTrainingEnabled = adaptiveTrainingEnabled;
			if (listing.ButtonTextLabeledPct("ResetAdaptiveTutor".Translate(), "Reset".Translate(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmResetLearningHelper".Translate(), delegate()
				{
					Messages.Message("AdaptiveTutorIsReset".Translate(), MessageTypeDefOf.TaskCompletion, false);
					PlayerKnowledgeDatabase.ResetPersistent();
				}, true, null, WindowLayer.Dialog));
			}
			listing.verticalSpacing = 0f;
			if (Prefs.PreferredNames.Count < 6)
			{
				if (listing.ButtonTextLabeledPct("NamesYouWantToSee".Translate(), "AddName".Translate() + "...", 0.6f, TextAnchor.MiddleLeft, null, null, null))
				{
					Find.WindowStack.Add(new Dialog_AddPreferredName());
				}
			}
			else
			{
				listing.Label("NamesYouWantToSee".Translate(), -1f, null);
			}
			listing.SubLabel("NamesYouWantToSeeSubText".Translate(), 0.6f);
			listing.verticalSpacing = 5f;
			Prefs.PreferredNames.RemoveAll((string n) => n.NullOrEmpty());
			float num2 = 20f;
			listing.Indent(num2);
			for (int i = 0; i < Prefs.PreferredNames.Count; i++)
			{
				string name = Prefs.PreferredNames[i];
				PawnBio pawnBio = (from b in SolidBioDatabase.allBios
				where b.name.ToString() == name
				select b).FirstOrDefault<PawnBio>();
				if (pawnBio == null)
				{
					name += " [N]";
				}
				else
				{
					PawnBioType bioType = pawnBio.BioType;
					if (bioType != PawnBioType.BackstoryInGame)
					{
						if (bioType == PawnBioType.PirateKing)
						{
							name += " [PK]";
						}
					}
					else
					{
						name += " [B]";
					}
				}
				Rect rect = listing.GetRect(24f, 1f);
				rect.width -= num2;
				if (i % 2 == 0)
				{
					Widgets.DrawLightHighlight(rect);
				}
				Widgets.Label(new Rect(rect.x + 4f, rect.y, rect.width - 4f, rect.height), name);
				if (Widgets.ButtonImage(new Rect(rect.xMax - 24f, rect.y, 24f, 24f), TexButton.Delete, Color.white, GenUI.SubtleMouseoverColor, true, null))
				{
					Prefs.PreferredNames.RemoveAt(i);
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
			}
			listing.Outdent(num2);
			listing.Gap(12f);
		}

		// Token: 0x06011E0D RID: 73229 RVA: 0x00532044 File Offset: 0x00530244
		private void DoUIOptions(Listing_Standard listing)
		{
			if (listing.ButtonTextLabeledPct("UIScale".Translate(), Prefs.UIScale.ToString() + "x", 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				for (int i = 0; i < Dialog_Options.UIScales.Length; i++)
				{
					float scale = Dialog_Options.UIScales[i];
					list.Add(new FloatMenuOption(Dialog_Options.UIScales[i].ToString() + "x", delegate()
					{
						if (scale != 1f && !ResolutionUtility.UIScaleSafeWithResolution(scale, Screen.width, Screen.height))
						{
							Messages.Message("MessageScreenResTooSmallForUIScale".Translate(), MessageTypeDefOf.RejectInput, false);
							return;
						}
						ResolutionUtility.SafeSetUIScale(scale);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			if (listing.ButtonTextLabeledPct("TemperatureMode".Translate(), Prefs.TemperatureMode.ToStringHuman(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				List<FloatMenuOption> list2 = new List<FloatMenuOption>();
				foreach (object obj in Enum.GetValues(typeof(TemperatureDisplayMode)))
				{
					TemperatureDisplayMode localTmode2 = (TemperatureDisplayMode)obj;
					TemperatureDisplayMode localTmode = localTmode2;
					list2.Add(new FloatMenuOption(localTmode.ToStringHuman(), delegate()
					{
						Prefs.TemperatureMode = localTmode;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list2));
			}
			if (listing.ButtonTextLabeledPct("ShowWeaponsUnderPortrait".Translate(), Prefs.ShowWeaponsUnderPortraitMode.ToStringHuman(), 0.6f, TextAnchor.MiddleLeft, null, "ShowWeaponsUnderPortraitTooltip".Translate(), null))
			{
				List<FloatMenuOption> list3 = new List<FloatMenuOption>();
				using (IEnumerator enumerator = Enum.GetValues(typeof(ShowWeaponsUnderPortraitMode)).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ShowWeaponsUnderPortraitMode mode = (ShowWeaponsUnderPortraitMode)enumerator.Current;
						list3.Add(new FloatMenuOption(mode.ToStringHuman(), delegate()
						{
							Prefs.ShowWeaponsUnderPortraitMode = mode;
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list3));
			}
			if (listing.ButtonTextLabeledPct("ShowAnimalNames".Translate(), Prefs.AnimalNameMode.ToStringHuman(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				List<FloatMenuOption> list4 = new List<FloatMenuOption>();
				foreach (object obj2 in Enum.GetValues(typeof(AnimalNameDisplayMode)))
				{
					AnimalNameDisplayMode localMode5 = (AnimalNameDisplayMode)obj2;
					AnimalNameDisplayMode localMode = localMode5;
					list4.Add(new FloatMenuOption(localMode.ToStringHuman(), delegate()
					{
						Prefs.AnimalNameMode = localMode;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list4));
			}
			if (ModsConfig.BiotechActive && listing.ButtonTextLabeledPct("ShowMechNames".Translate(), Prefs.MechNameMode.ToStringHuman(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				List<FloatMenuOption> list5 = new List<FloatMenuOption>();
				foreach (object obj3 in Enum.GetValues(typeof(MechNameDisplayMode)))
				{
					MechNameDisplayMode localMode2 = (MechNameDisplayMode)obj3;
					MechNameDisplayMode localMode = localMode2;
					list5.Add(new FloatMenuOption(localMode.ToStringHuman(), delegate()
					{
						Prefs.MechNameMode = localMode;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list5));
			}
			if (listing.ButtonTextLabeledPct("DotHighlightDisplayMode".Translate(), Prefs.DotHighlightDisplayMode.ToStringHuman(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				List<FloatMenuOption> list6 = new List<FloatMenuOption>();
				foreach (object obj4 in Enum.GetValues(typeof(DotHighlightDisplayMode)))
				{
					DotHighlightDisplayMode localMode3 = (DotHighlightDisplayMode)obj4;
					DotHighlightDisplayMode localMode = localMode3;
					list6.Add(new FloatMenuOption(localMode.ToStringHuman(), delegate()
					{
						Prefs.DotHighlightDisplayMode = localMode;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list6));
			}
			bool flag = Prefs.DotHighlightDisplayMode > DotHighlightDisplayMode.None;
			if (!flag)
			{
				GUI.color = Color.gray;
			}
			if (listing.ButtonTextLabeledPct("HighlightStyleMode".Translate(), Prefs.HighlightStyleMode.ToStringHuman(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				List<FloatMenuOption> list7 = new List<FloatMenuOption>();
				foreach (object obj5 in Enum.GetValues(typeof(HighlightStyleMode)))
				{
					HighlightStyleMode localMode4 = (HighlightStyleMode)obj5;
					HighlightStyleMode localMode = localMode4;
					list7.Add(new FloatMenuOption(localMode.ToStringHuman(), delegate()
					{
						Prefs.HighlightStyleMode = localMode;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				if (flag)
				{
					Find.WindowStack.Add(new FloatMenu(list7));
				}
			}
			if (!flag)
			{
				GUI.color = Color.white;
			}
			TaggedString taggedString = Prefs.RandomBackgroundImage ? "Random".Translate() : Prefs.BackgroundImageExpansion.LabelCap;
			if ((from e in ModLister.AllExpansions
			where !e.isCore && e.Status != ExpansionStatus.NotInstalled
			select e).Any<ExpansionDef>() && listing.ButtonTextLabeledPct("SetBackgroundImage".Translate(), taggedString, 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				List<FloatMenuOption> list8 = new List<FloatMenuOption>();
				foreach (ExpansionDef expansionDef in ModLister.AllExpansions)
				{
					if (expansionDef.Status != ExpansionStatus.NotInstalled)
					{
						ExpansionDef localExpansion = expansionDef;
						list8.Add(new FloatMenuOption(expansionDef.label, delegate()
						{
							Prefs.BackgroundImageExpansion = localExpansion;
							Prefs.RandomBackgroundImage = false;
						}, expansionDef.Icon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
					}
				}
				list8.Add(new FloatMenuOption("Random".Translate(), delegate()
				{
					Prefs.RandomBackgroundImage = true;
					((UI_BackgroundMain)UIMenuBackgroundManager.background).overrideBGImage = (from exp in ModLister.AllExpansions
					where exp.Status == ExpansionStatus.Active
					select exp).RandomElement<ExpansionDef>().BackgroundImage;
				}, Dialog_Options.RandomBackgroundTex, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
				Find.WindowStack.Add(new FloatMenu(list8));
			}
			bool showRealtimeClock = Prefs.ShowRealtimeClock;
			listing.CheckboxLabeled("ShowRealtimeClock".Translate(), ref showRealtimeClock, null, 30f, 0.6f);
			Prefs.ShowRealtimeClock = showRealtimeClock;
			bool twelveHourClockMode = Prefs.TwelveHourClockMode;
			listing.CheckboxLabeled("TwelveHourClockMode".Translate(), ref twelveHourClockMode, null, 30f, 0.6f);
			Prefs.TwelveHourClockMode = twelveHourClockMode;
			bool hatsOnlyOnMap = Prefs.HatsOnlyOnMap;
			listing.CheckboxLabeled("HatsShownOnlyOnMap".Translate(), ref hatsOnlyOnMap, null, 30f, 0.6f);
			if (hatsOnlyOnMap != Prefs.HatsOnlyOnMap)
			{
				PortraitsCache.Clear();
			}
			Prefs.HatsOnlyOnMap = hatsOnlyOnMap;
			if (!SteamDeck.IsSteamDeck)
			{
				bool disableTinyText = Prefs.DisableTinyText;
				listing.CheckboxLabeled("DisableTinyText".Translate(), ref disableTinyText, null, 30f, 0.6f);
				if (Prefs.DisableTinyText != disableTinyText)
				{
					Prefs.DisableTinyText = disableTinyText;
					Widgets.ClearLabelCache();
					GenUI.ClearLabelWidthCache();
					if (Current.ProgramState == ProgramState.Playing)
					{
						Find.ColonistBar.drawer.ClearLabelCache();
					}
				}
			}
			bool flag2 = !Prefs.CustomCursorEnabled;
			listing.CheckboxLabeled("CustomCursor".Translate(), ref flag2, null, 30f, 0.6f);
			Prefs.CustomCursorEnabled = !flag2;
			bool visibleMood = Prefs.VisibleMood;
			listing.CheckboxLabeled("VisibleMood".Translate(), ref visibleMood, "VisibleMoodDesc".Translate(), 30f, 0.6f);
			Prefs.VisibleMood = visibleMood;
		}

		// Token: 0x06011E0E RID: 73230 RVA: 0x0053293C File Offset: 0x00530B3C
		private void DoControlsOptions(Listing_Standard listing)
		{
			if (!SteamDeck.IsSteamDeckInNonKeyboardMode && listing.ButtonTextLabeledPct("KeyboardConfig".Translate(), "ModifyConfig".Translate(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				Find.WindowStack.Add(new Dialog_KeyBindings());
			}
			if (SteamDeck.IsSteamDeck && listing.ButtonTextLabeledPct("ControllerConfig".Translate(), "ModifyConfig".Translate(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				SteamDeck.ShowConfigPage();
			}
			if (SteamDeck.IsSteamDeck)
			{
				bool steamDeckKeyboardMode = Prefs.SteamDeckKeyboardMode;
				listing.CheckboxLabeled("SteamDeckKeyboardMode".Translate(), ref steamDeckKeyboardMode, null, 30f, 0.6f);
				Prefs.SteamDeckKeyboardMode = steamDeckKeyboardMode;
			}
			float mapDragSensitivity = Prefs.MapDragSensitivity;
			Prefs.MapDragSensitivity = (float)Math.Round((double)listing.SliderLabeled("MapDragSensitivity".Translate() + ": " + mapDragSensitivity.ToStringPercent("F0"), mapDragSensitivity, 0.8f, 2.5f, 0.6f, null), 2);
			bool edgeScreenScroll = Prefs.EdgeScreenScroll;
			listing.CheckboxLabeled("EdgeScreenScroll".Translate(), ref edgeScreenScroll, null, 30f, 0.6f);
			Prefs.EdgeScreenScroll = edgeScreenScroll;
			bool zoomToMouse = Prefs.ZoomToMouse;
			listing.CheckboxLabeled("ZoomToMouse".Translate(), ref zoomToMouse, null, 30f, 0.6f);
			Prefs.ZoomToMouse = zoomToMouse;
			bool zoomSwitchWorldLayer = Prefs.ZoomSwitchWorldLayer;
			listing.CheckboxLabeled("ZoomSwitchLayer".Translate(), ref zoomSwitchWorldLayer, "ZoomSwitchLayer_Tooltip".Translate(), 30f, 0.6f);
			Prefs.ZoomSwitchWorldLayer = zoomSwitchWorldLayer;
			bool rememberDrawStlyes = Prefs.RememberDrawStlyes;
			listing.CheckboxLabeled("RememberDrawStyle".Translate(), ref rememberDrawStlyes, "RememberDrawStyle_Tooltip".Translate(), 30f, 0.6f);
			Prefs.RememberDrawStlyes = rememberDrawStlyes;
		}

		// Token: 0x06011E0F RID: 73231 RVA: 0x00532B2C File Offset: 0x00530D2C
		private void DoDevOptions(Listing_Standard listing)
		{
			bool testMapSizes = Prefs.TestMapSizes;
			listing.CheckboxLabeled("EnableTestMapSizes".Translate(), ref testMapSizes, null, 30f, 0.6f);
			Prefs.TestMapSizes = testMapSizes;
			bool logVerbose = Prefs.LogVerbose;
			listing.CheckboxLabeled("LogVerbose".Translate(), ref logVerbose, null, 30f, 0.6f);
			Prefs.LogVerbose = logVerbose;
			bool resetModsConfigOnCrash = Prefs.ResetModsConfigOnCrash;
			listing.CheckboxLabeled("ResetModsConfigOnCrash".Translate(), ref resetModsConfigOnCrash, null, 30f, 0.6f);
			Prefs.ResetModsConfigOnCrash = resetModsConfigOnCrash;
			bool disableQuickStartCryptoSickness = Prefs.DisableQuickStartCryptoSickness;
			listing.CheckboxLabeled("DisableQuickStartCryptoSickness".Translate(), ref disableQuickStartCryptoSickness, null, 30f, 0.6f);
			Prefs.DisableQuickStartCryptoSickness = disableQuickStartCryptoSickness;
			bool startDevPaletteOn = Prefs.StartDevPaletteOn;
			listing.CheckboxLabeled("StartDevPaletteOn".Translate(), ref startDevPaletteOn, null, 30f, 0.6f);
			Prefs.StartDevPaletteOn = startDevPaletteOn;
			bool openLogOnWarnings = Prefs.OpenLogOnWarnings;
			listing.CheckboxLabeled("OpenLogOnWarnings".Translate(), ref openLogOnWarnings, null, 30f, 0.6f);
			Prefs.OpenLogOnWarnings = openLogOnWarnings;
			bool closeLogWindowOnEscape = Prefs.CloseLogWindowOnEscape;
			listing.CheckboxLabeled("CloseLogWindowOnEscape".Translate(), ref closeLogWindowOnEscape, null, 30f, 0.6f);
			Prefs.CloseLogWindowOnEscape = closeLogWindowOnEscape;
			if (!DevModePermanentlyDisabledUtility.Disabled && listing.ButtonTextLabeledPct("PermanentlyDisableDevMode".Translate(), "Disable".Translate(), 0.6f, TextAnchor.MiddleLeft, null, null, null))
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmPermanentlyDisableDevMode".Translate(), new Action(DevModePermanentlyDisabledUtility.Disable), true, null, WindowLayer.Dialog));
			}
		}

		// Token: 0x06011E10 RID: 73232 RVA: 0x00532CE0 File Offset: 0x00530EE0
		private void DoModOptions(Listing_Standard listing)
		{
			Rect rect = listing.GetRect(30f, 1f);
			this.quickSearchWidget.OnGUI(rect, null, null);
			this.modFilter = this.quickSearchWidget.filter.Text;
			listing.Gap(12f);
			Rect rect2 = default(Rect);
			int num = 0;
			foreach (Mod mod in this.cachedModsWithSettings)
			{
				if (mod.SettingsCategory().ToLower().Contains(this.modFilter.ToLower()) || mod.Content.Name.ToLower().Contains(this.modFilter.ToLower()))
				{
					if (num % 2 == 0)
					{
						rect2 = listing.GetRect(40f, 1f);
					}
					Rect rect3 = ((num % 2 == 0) ? rect2.LeftHalf() : rect2.RightHalf()).ContractedBy(2f);
					Text.Anchor = TextAnchor.MiddleLeft;
					Widgets.DrawOptionBackground(rect3, false);
					if (Widgets.ButtonInvisible(rect3, true))
					{
						Find.WindowStack.Add(new Dialog_ModSettings(mod));
					}
					if (Mouse.IsOver(rect3) && Text.CalcSize(mod.SettingsCategory()).x > rect3.width - rect3.height)
					{
						TooltipHandler.TipRegion(rect3, mod.SettingsCategory());
					}
					if (!mod.Content.ModMetaData.Icon.NullOrBad())
					{
						GUI.DrawTexture(new Rect(rect3.x, rect3.y, rect3.height, rect3.height).ContractedBy(2f), mod.Content.ModMetaData.Icon);
					}
					rect3.xMin += rect3.height;
					Widgets.Label(rect3, mod.SettingsCategory().Truncate(rect3.width, this.modOptionTruncationCache));
					Text.Anchor = TextAnchor.UpperLeft;
					num++;
				}
			}
		}

		// Token: 0x06011E11 RID: 73233 RVA: 0x00532EFC File Offset: 0x005310FC
		public override void PreClose()
		{
			base.PreClose();
			Prefs.Save();
			if (this.selectedMod != null)
			{
				this.selectedMod.WriteSettings();
			}
		}

		// Token: 0x06011E12 RID: 73234 RVA: 0x00532F1C File Offset: 0x0053111C
		public void RestoreToDefaultSettings()
		{
			foreach (FileInfo fileInfo in new DirectoryInfo(GenFilePaths.ConfigFolderPath).GetFiles("*.xml"))
			{
				try
				{
					fileInfo.Delete();
				}
				catch (SystemException)
				{
				}
			}
			Find.WindowStack.Add(new Dialog_MessageBox("ResetAndRestart".Translate(), null, new Action(GenCommandLine.Restart), null, null, null, false, null, null, WindowLayer.Dialog));
		}

		// Token: 0x06011E13 RID: 73235 RVA: 0x00532F98 File Offset: 0x00531198
		public static string ResToString(int width, int height)
		{
			string text = width.ToString() + "x" + height.ToString();
			if (width == 1280 && height == 720)
			{
				text += " (720p)";
			}
			if (width == 1920 && height == 1080)
			{
				text += " (1080p)";
			}
			return text;
		}

		// Token: 0x0400B8DA RID: 47322
		public OptionCategoryDef selectedCategory = OptionCategoryDefOf.General;

		// Token: 0x0400B8DB RID: 47323
		public Mod selectedMod;

		// Token: 0x0400B8DC RID: 47324
		private bool hasModSettings;

		// Token: 0x0400B8DD RID: 47325
		private Vector2 categoryScrollPosition;

		// Token: 0x0400B8DE RID: 47326
		private Vector2 optionsScrollPosition;

		// Token: 0x0400B8DF RID: 47327
		private float optionsViewRectHeight;

		// Token: 0x0400B8E0 RID: 47328
		private QuickSearchWidget quickSearchWidget = new QuickSearchWidget();

		// Token: 0x0400B8E1 RID: 47329
		private string modFilter = "";

		// Token: 0x0400B8E2 RID: 47330
		private IEnumerable<Mod> cachedModsWithSettings;

		// Token: 0x0400B8E3 RID: 47331
		private const float CategoryListWidth = 160f;

		// Token: 0x0400B8E4 RID: 47332
		private const float CategoryRowHeight = 48f;

		// Token: 0x0400B8E5 RID: 47333
		private const float CategoryRowSpacing = 2f;

		// Token: 0x0400B8E6 RID: 47334
		private const float CategoryIconSize = 20f;

		// Token: 0x0400B8E7 RID: 47335
		private const float BottomRowHeight = 50f;

		// Token: 0x0400B8E8 RID: 47336
		private const float ListingVerticalSpacing = 5f;

		// Token: 0x0400B8E9 RID: 47337
		private const float SearchBarHeight = 30f;

		// Token: 0x0400B8EA RID: 47338
		private const float ModIconSize = 32f;

		// Token: 0x0400B8EB RID: 47339
		private static readonly Vector2 OkayButtonSize = new Vector2(160f, 40f);

		// Token: 0x0400B8EC RID: 47340
		public static readonly float[] UIScales = new float[]
		{
			1f,
			1.25f,
			1.5f,
			1.75f,
			2f,
			2.5f,
			3f,
			3.5f,
			4f
		};

		// Token: 0x0400B8ED RID: 47341
		public static readonly Texture2D RandomBackgroundTex = ContentFinder<Texture2D>.Get("UI/Icons/RandomBackground", true);

		// Token: 0x0400B8EE RID: 47342
		public static readonly Texture2D LanguageIconTex = ContentFinder<Texture2D>.Get("UI/Icons/Language", true);

		// Token: 0x0400B8EF RID: 47343
		private Dictionary<string, string> modOptionTruncationCache = new Dictionary<string, string>();
	}
}
