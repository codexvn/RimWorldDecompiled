using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D54 RID: 3412
	public class PrefsData
	{
		// Token: 0x060056E4 RID: 22244 RVA: 0x001BEFFC File Offset: 0x001BD1FC
		public void Apply()
		{
			if (!UnityData.IsInMainThread)
			{
				return;
			}
			if (this.customCursorEnabled)
			{
				CustomCursor.Activate();
			}
			else
			{
				CustomCursor.Deactivate();
			}
			AudioListener.volume = this.volumeMaster;
			Application.runInBackground = this.runInBackground;
			if (this.screenWidth == 0 || this.screenHeight == 0)
			{
				ResolutionUtility.SetNativeResolutionRaw();
				return;
			}
			ResolutionUtility.SetResolutionRaw(this.screenWidth, this.screenHeight, !ResolutionUtility.BorderlessFullscreen && this.fullscreen);
		}

		// Token: 0x04003F06 RID: 16134
		public float volumeMaster = 0.8f;

		// Token: 0x04003F07 RID: 16135
		public float volumeGame = 1f;

		// Token: 0x04003F08 RID: 16136
		public float volumeMusic = 0.4f;

		// Token: 0x04003F09 RID: 16137
		public float volumeAmbient = 1f;

		// Token: 0x04003F0A RID: 16138
		public float volumeUI = 1f;

		// Token: 0x04003F0B RID: 16139
		public int screenWidth;

		// Token: 0x04003F0C RID: 16140
		public int screenHeight;

		// Token: 0x04003F0D RID: 16141
		public bool fullscreen;

		// Token: 0x04003F0E RID: 16142
		public float uiScale = 1f;

		// Token: 0x04003F0F RID: 16143
		public bool customCursorEnabled = true;

		// Token: 0x04003F10 RID: 16144
		public bool hatsOnlyOnMap;

		// Token: 0x04003F11 RID: 16145
		public bool plantWindSway = true;

		// Token: 0x04003F12 RID: 16146
		public float screenShakeIntensity = 1f;

		// Token: 0x04003F13 RID: 16147
		public bool textureCompression = true;

		// Token: 0x04003F14 RID: 16148
		public bool showRealtimeClock;

		// Token: 0x04003F15 RID: 16149
		public bool disableTinyText;

		// Token: 0x04003F16 RID: 16150
		public AnimalNameDisplayMode animalNameMode;

		// Token: 0x04003F17 RID: 16151
		public MechNameDisplayMode mechNameMode = MechNameDisplayMode.WhileDrafted;

		// Token: 0x04003F18 RID: 16152
		public string backgroundExpansionId;

		// Token: 0x04003F19 RID: 16153
		public bool randomBackground;

		// Token: 0x04003F1A RID: 16154
		public ShowWeaponsUnderPortraitMode showWeaponsUnderPortraitMode = ShowWeaponsUnderPortraitMode.WhileDrafted;

		// Token: 0x04003F1B RID: 16155
		public DotHighlightDisplayMode dotHighlightDisplayMode = DotHighlightDisplayMode.HighlightAll;

		// Token: 0x04003F1C RID: 16156
		public HighlightStyleMode highlightStyleMode = HighlightStyleMode.Silhouettes;

		// Token: 0x04003F1D RID: 16157
		public bool visibleMood = true;

		// Token: 0x04003F1E RID: 16158
		public bool twelveHourClock;

		// Token: 0x04003F1F RID: 16159
		public bool adaptiveTrainingEnabled = true;

		// Token: 0x04003F20 RID: 16160
		public bool steamDeckKeyboardMode;

		// Token: 0x04003F21 RID: 16161
		public List<string> preferredNames = new List<string>();

		// Token: 0x04003F22 RID: 16162
		public bool resourceReadoutCategorized;

		// Token: 0x04003F23 RID: 16163
		public bool runInBackground;

		// Token: 0x04003F24 RID: 16164
		public bool edgeScreenScroll = true;

		// Token: 0x04003F25 RID: 16165
		public bool rememberDrawStyles;

		// Token: 0x04003F26 RID: 16166
		public bool zoomSwitchWorldLayer = true;

		// Token: 0x04003F27 RID: 16167
		public bool zoomToMouse;

		// Token: 0x04003F28 RID: 16168
		public TemperatureDisplayMode temperatureMode;

		// Token: 0x04003F29 RID: 16169
		public float autosaveIntervalDays = 1f;

		// Token: 0x04003F2A RID: 16170
		public bool testMapSizes;

		// Token: 0x04003F2B RID: 16171
		[LoadAlias("maxNumberOfPlayerHomes")]
		public int maxNumberOfPlayerSettlements = 1;

		// Token: 0x04003F2C RID: 16172
		public bool pauseOnLoad;

		// Token: 0x04003F2D RID: 16173
		public AutomaticPauseMode automaticPauseMode = AutomaticPauseMode.MajorThreat;

		// Token: 0x04003F2E RID: 16174
		public float mapDragSensitivity = 1.3f;

		// Token: 0x04003F2F RID: 16175
		public bool smoothCameraJumps = true;

		// Token: 0x04003F30 RID: 16176
		public int autosavesCount = 5;

		// Token: 0x04003F31 RID: 16177
		[Unsaved(true)]
		public bool? pauseOnUrgentLetter;

		// Token: 0x04003F32 RID: 16178
		[Obsolete]
		public bool extremeDifficultyUnlocked;

		// Token: 0x04003F33 RID: 16179
		[Obsolete]
		public bool pauseOnError;

		// Token: 0x04003F34 RID: 16180
		public bool devMode;

		// Token: 0x04003F35 RID: 16181
		public List<string> debugActionPalette = new List<string>();

		// Token: 0x04003F36 RID: 16182
		public Vector2 devPalettePosition;

		// Token: 0x04003F37 RID: 16183
		public string langFolderName = "unknown";

		// Token: 0x04003F38 RID: 16184
		public bool logVerbose;

		// Token: 0x04003F39 RID: 16185
		public bool openLogOnWarnings;

		// Token: 0x04003F3A RID: 16186
		public bool closeLogWindowOnEscape = true;

		// Token: 0x04003F3B RID: 16187
		public bool disableQuickStartCryptoSickness = true;

		// Token: 0x04003F3C RID: 16188
		public bool quickStartDevPaletteOn;

		// Token: 0x04003F3D RID: 16189
		public bool resetModsConfigOnCrash = true;

		// Token: 0x04003F3E RID: 16190
		[Obsolete]
		public bool simulateNotOwningRoyalty;

		// Token: 0x04003F3F RID: 16191
		[Obsolete]
		public bool simulateNotOwningIdeology;

		// Token: 0x04003F40 RID: 16192
		[Obsolete]
		public bool simulateNotOwningBiotech;

		// Token: 0x04003F41 RID: 16193
		[Obsolete]
		public bool simulateNotOwningAnomaly;
	}
}
