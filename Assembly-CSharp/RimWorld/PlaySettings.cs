using System;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x020037DE RID: 14302
	public sealed class PlaySettings : IExposable
	{
		// Token: 0x06013D14 RID: 81172 RVA: 0x005E19B0 File Offset: 0x005DFBB0
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.showLearningHelper, "showLearningHelper", false, false);
			Scribe_Values.Look<bool>(ref this.showZones, "showZones", false, false);
			Scribe_Values.Look<bool>(ref this.showBeauty, "showBeauty", false, false);
			Scribe_Values.Look<bool>(ref this.showRoomStats, "showRoomStats", false, false);
			Scribe_Values.Look<bool>(ref this.showColonistBar, "showColonistBar", false, false);
			Scribe_Values.Look<bool>(ref this.showRoofOverlay, "showRoofOverlay", false, false);
			Scribe_Values.Look<bool>(ref this.showFertilityOverlay, "showFertilityOverlay", false, false);
			Scribe_Values.Look<bool>(ref this.showTerrainAffordanceOverlay, "showTerrainAffordanceOverlay", false, false);
			Scribe_Values.Look<bool>(ref this.showPollutionOverlay, "showPollutionOverlay", false, false);
			Scribe_Values.Look<bool>(ref this.showVacuumOverlay, "showVacuumOverlay", false, false);
			Scribe_Values.Look<bool>(ref this.autoHomeArea, "autoHomeArea", false, false);
			Scribe_Values.Look<bool>(ref this.autoRebuild, "autoRebuild", false, false);
			Scribe_Values.Look<bool>(ref this.lockNorthUp, "lockNorthUp", false, false);
			Scribe_Values.Look<bool>(ref this.usePlanetDayNightSystem, "usePlanetDayNightSystem", false, false);
			Scribe_Values.Look<bool>(ref this.showImportantExpandingIcons, "showImportantExpandingIcons", true, false);
			Scribe_Values.Look<bool>(ref this.showBasesExpandingIcons, "showBasesExpandingIcons", true, false);
			Scribe_Values.Look<bool>(ref this.showExpandingLandmarks, "showExpandingLandmarks", false, false);
			Scribe_Values.Look<bool>(ref this.showWorldFeatures, "showWorldFeatures", false, false);
			Scribe_Values.Look<bool>(ref this.showTemperatureOverlay, "showTemperatureOverlay", false, false);
			Scribe_Values.Look<bool>(ref this.useWorkPriorities, "useWorkPriorities", false, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForColonist, "defaultCareForColonist", MedicalCareCategory.Best, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForTamedAnimal, "defaultCareForTamedAnimal", MedicalCareCategory.HerbalOrWorse, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForPrisoner, "defaultCareForPrisoner", MedicalCareCategory.HerbalOrWorse, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForSlave, "defaultCareForSlave", MedicalCareCategory.HerbalOrWorse, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForNeutralFaction, "defaultCareForNeutralFaction", MedicalCareCategory.HerbalOrWorse, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForWildlife, "defaultCareForWildlife", MedicalCareCategory.HerbalOrWorse, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForHostileFaction, "defaultCareForHumanlikeEnemies", MedicalCareCategory.HerbalOrWorse, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForFriendlyFaction, "defaultCareForFriendlyFaction", MedicalCareCategory.HerbalOrWorse, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForNoFaction, "defaultCareForNoFaction", MedicalCareCategory.HerbalOrWorse, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForEntities, "defaultCareForEntities", MedicalCareCategory.NoMeds, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.defaultCareForGhouls, "defaultCareForGhouls", MedicalCareCategory.NoMeds, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06013D15 RID: 81173 RVA: 0x005E1BF1 File Offset: 0x005DFDF1
		public void DoPlaySettingsGlobalControls(WidgetRow row, bool worldView)
		{
			bool flag = this.showColonistBar;
			if (worldView)
			{
				this.DoWorldViewControls(row);
			}
			else
			{
				this.DoMapControls(row);
			}
			if (flag != this.showColonistBar)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
		}

		// Token: 0x06013D16 RID: 81174 RVA: 0x005E1C20 File Offset: 0x005DFE20
		private void DoWorldViewControls(WidgetRow row)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				row.ToggleableIcon(ref this.showColonistBar, TexButton.ShowColonistBar, SteamDeck.IsSteamDeckInNonKeyboardMode ? "ShowColonistBarToggleButtonController".Translate() : "ShowColonistBarToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			}
			bool flag = this.lockNorthUp;
			row.ToggleableIcon(ref this.lockNorthUp, TexButton.LockNorthUp, "LockNorthUpToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			if (flag != this.lockNorthUp && this.lockNorthUp)
			{
				Find.WorldCameraDriver.RotateSoNorthIsUp(true);
			}
			row.ToggleableIcon(ref this.showImportantExpandingIcons, TexButton.ShowImportantLocations, "ShowImportantExpandingIconsToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			row.ToggleableIcon(ref this.showBasesExpandingIcons, TexButton.ShowOtherFactionBases, "ShowBasesExpandingIconsToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			if (ModsConfig.OdysseyActive)
			{
				row.ToggleableIcon(ref this.showExpandingLandmarks, TexButton.ShowLandmarkIcons, "ShowExpandingLandmarksToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			}
			row.ToggleableIcon(ref this.usePlanetDayNightSystem, TexButton.UsePlanetDayNightSystem, "UsePlanetDayNightSystemToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			row.ToggleableIcon(ref this.showWorldFeatures, TexButton.ShowWorldFeatures, "ShowWorldFeaturesToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			string arg = KeyPrefs.KeyPrefsData.GetBoundKeyCode(KeyBindingDefOf.OpenMapSearch, KeyPrefs.BindingSlot.A).ToStringReadable();
			string tooltip = string.Format("{0}: {1}\n\n{2}", "HotKeyTip".Translate(), arg, "SearchTheWorldDesc".Translate());
			if (row.ButtonIcon(TexButton.SearchButton, tooltip, null, null, null, true, -1f) || (KeyBindingDefOf.OpenMapSearch.JustPressed && Event.current.type == EventType.KeyDown))
			{
				Find.WindowStack.Add(new Dialog_WorldSearch());
			}
		}

		// Token: 0x06013D17 RID: 81175 RVA: 0x005E1E14 File Offset: 0x005E0014
		private void DoMapControls(WidgetRow row)
		{
			row.ToggleableIcon(ref this.showLearningHelper, TexButton.ShowLearningHelper, "ShowLearningHelperWhenEmptyToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			row.ToggleableIcon(ref this.showZones, TexButton.ShowZones, "ZoneVisibilityToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			string arg = KeyPrefs.KeyPrefsData.GetBoundKeyCode(KeyBindingDefOf.ToggleBeautyDisplay, KeyPrefs.BindingSlot.A).ToStringReadable();
			string tooltip = string.Format("{0}: {1}\n\n{2}", "HotKeyTip".Translate(), arg, "ShowBeautyToggleButton".Translate());
			row.ToggleableIcon(ref this.showBeauty, TexButton.ShowBeauty, tooltip, SoundDefOf.Mouseover_ButtonToggle, null);
			PlaySettings.CheckKeyBindingToggle(KeyBindingDefOf.ToggleBeautyDisplay, ref this.showBeauty);
			string arg2 = KeyPrefs.KeyPrefsData.GetBoundKeyCode(KeyBindingDefOf.ToggleRoomStatsDisplay, KeyPrefs.BindingSlot.A).ToStringReadable();
			string tooltip2 = string.Format("{0}: {1}\n\n{2}", "HotKeyTip".Translate(), arg2, "ShowRoomStatsToggleButton".Translate());
			row.ToggleableIcon(ref this.showRoomStats, TexButton.ShowRoomStats, tooltip2, SoundDefOf.Mouseover_ButtonToggle, null);
			PlaySettings.CheckKeyBindingToggle(KeyBindingDefOf.ToggleRoomStatsDisplay, ref this.showRoomStats);
			row.ToggleableIcon(ref this.showColonistBar, TexButton.ShowColonistBar, SteamDeck.IsSteamDeckInNonKeyboardMode ? "ShowColonistBarToggleButtonController".Translate() : "ShowColonistBarToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			row.ToggleableIcon(ref this.showRoofOverlay, TexButton.ShowRoofOverlay, "ShowRoofOverlayToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			row.ToggleableIcon(ref this.showFertilityOverlay, TexButton.ShowFertilityOverlay, "ShowFertilityOverlayToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			row.ToggleableIcon(ref this.showTerrainAffordanceOverlay, TexButton.ShowTerrainAffordanceOverlay, "ShowTerrainAffordanceOverlayToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			row.ToggleableIcon(ref this.autoHomeArea, TexButton.AutoHomeArea, "AutoHomeAreaToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			row.ToggleableIcon(ref this.autoRebuild, TexButton.AutoRebuild, "AutoRebuildButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			row.ToggleableIcon(ref this.showTemperatureOverlay, TexButton.ShowTemperatureOverlay, "ShowTemperatureOverlayToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			bool resourceReadoutCategorized = Prefs.ResourceReadoutCategorized;
			bool flag = resourceReadoutCategorized;
			row.ToggleableIcon(ref resourceReadoutCategorized, TexButton.CategorizedResourceReadout, "CategorizedResourceReadoutToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			if (resourceReadoutCategorized != flag)
			{
				Prefs.ResourceReadoutCategorized = resourceReadoutCategorized;
			}
			if (ModsConfig.BiotechActive)
			{
				row.ToggleableIcon(ref this.showPollutionOverlay, TexButton.ShowPollutionOverlay, "ShowPollutionOverlayToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			}
			if (ModsConfig.OdysseyActive && Find.CurrentMap != null && Find.CurrentMap.Biome.inVacuum)
			{
				row.ToggleableIcon(ref this.showVacuumOverlay, TexButton.ShowVacuumOverlay, "ShowVacuumOverlayToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			}
			else
			{
				this.showVacuumOverlay = false;
			}
			string arg3 = KeyPrefs.KeyPrefsData.GetBoundKeyCode(KeyBindingDefOf.OpenMapSearch, KeyPrefs.BindingSlot.A).ToStringReadable();
			string tooltip3 = string.Format("{0}: {1}\n\n{2}", "HotKeyTip".Translate(), arg3, "SearchTheMapDesc".Translate());
			if (row.ButtonIcon(TexButton.SearchButton, tooltip3, null, null, null, true, -1f) || (KeyBindingDefOf.OpenMapSearch.JustPressed && Event.current.type == EventType.KeyDown))
			{
				Event.current.Use();
				Find.WindowStack.Add(new Dialog_MapSearch(Find.CurrentMap));
			}
			if (ModsConfig.AnomalyActive && Find.Anomaly.AnomalyStudyEnabled)
			{
				UIHighlighter.HighlightOpportunity(row.ButtonIconRect(-1f), "EntityCodex");
				if (row.ButtonIcon(TexButton.CodexButton, "EntityCodexGizmoTip".Translate(), null, null, null, true, -1f))
				{
					Find.WindowStack.Add(new Dialog_EntityCodex(null));
				}
			}
		}

		// Token: 0x06013D18 RID: 81176 RVA: 0x005E2234 File Offset: 0x005E0434
		private static void CheckKeyBindingToggle(KeyBindingDef keyBinding, ref bool value)
		{
			if (!keyBinding.KeyDownEvent)
			{
				return;
			}
			value = !value;
			if (value)
			{
				SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				return;
			}
			SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
		}

		// Token: 0x0400DB7B RID: 56187
		public bool showLearningHelper = true;

		// Token: 0x0400DB7C RID: 56188
		public bool showZones = true;

		// Token: 0x0400DB7D RID: 56189
		public bool showBeauty;

		// Token: 0x0400DB7E RID: 56190
		public bool showRoomStats;

		// Token: 0x0400DB7F RID: 56191
		public bool showColonistBar = true;

		// Token: 0x0400DB80 RID: 56192
		public bool showRoofOverlay;

		// Token: 0x0400DB81 RID: 56193
		public bool showFertilityOverlay;

		// Token: 0x0400DB82 RID: 56194
		public bool showTerrainAffordanceOverlay;

		// Token: 0x0400DB83 RID: 56195
		public bool showPollutionOverlay;

		// Token: 0x0400DB84 RID: 56196
		public bool showVacuumOverlay;

		// Token: 0x0400DB85 RID: 56197
		public bool autoHomeArea = true;

		// Token: 0x0400DB86 RID: 56198
		public bool autoRebuild;

		// Token: 0x0400DB87 RID: 56199
		public bool showTemperatureOverlay;

		// Token: 0x0400DB88 RID: 56200
		public bool lockNorthUp = true;

		// Token: 0x0400DB89 RID: 56201
		public bool usePlanetDayNightSystem = true;

		// Token: 0x0400DB8A RID: 56202
		public bool showImportantExpandingIcons = true;

		// Token: 0x0400DB8B RID: 56203
		public bool showBasesExpandingIcons = true;

		// Token: 0x0400DB8C RID: 56204
		public bool showExpandingLandmarks;

		// Token: 0x0400DB8D RID: 56205
		public bool showWorldFeatures = true;

		// Token: 0x0400DB8E RID: 56206
		public bool useWorkPriorities;

		// Token: 0x0400DB8F RID: 56207
		public MedicalCareCategory defaultCareForColonist = MedicalCareCategory.Best;

		// Token: 0x0400DB90 RID: 56208
		public MedicalCareCategory defaultCareForPrisoner = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x0400DB91 RID: 56209
		public MedicalCareCategory defaultCareForSlave = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x0400DB92 RID: 56210
		public MedicalCareCategory defaultCareForTamedAnimal = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x0400DB93 RID: 56211
		public MedicalCareCategory defaultCareForFriendlyFaction = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x0400DB94 RID: 56212
		public MedicalCareCategory defaultCareForNeutralFaction = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x0400DB95 RID: 56213
		public MedicalCareCategory defaultCareForHostileFaction = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x0400DB96 RID: 56214
		public MedicalCareCategory defaultCareForNoFaction = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x0400DB97 RID: 56215
		public MedicalCareCategory defaultCareForWildlife = MedicalCareCategory.HerbalOrWorse;

		// Token: 0x0400DB98 RID: 56216
		public MedicalCareCategory defaultCareForEntities = MedicalCareCategory.NoMeds;

		// Token: 0x0400DB99 RID: 56217
		public MedicalCareCategory defaultCareForGhouls = MedicalCareCategory.NoMeds;
	}
}
