using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using LudeonTK;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D45 RID: 3397
	public static class Prefs
	{
		// Token: 0x17000D6B RID: 3435
		// (get) Token: 0x06005669 RID: 22121 RVA: 0x001BE27B File Offset: 0x001BC47B
		// (set) Token: 0x0600566A RID: 22122 RVA: 0x001BE287 File Offset: 0x001BC487
		public static float VolumeMaster
		{
			get
			{
				return Prefs.data.volumeMaster;
			}
			set
			{
				if (Prefs.data.volumeMaster == value)
				{
					return;
				}
				Prefs.data.volumeMaster = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D6C RID: 3436
		// (get) Token: 0x0600566B RID: 22123 RVA: 0x001BE2A7 File Offset: 0x001BC4A7
		// (set) Token: 0x0600566C RID: 22124 RVA: 0x001BE2B3 File Offset: 0x001BC4B3
		public static float VolumeGame
		{
			get
			{
				return Prefs.data.volumeGame;
			}
			set
			{
				if (Prefs.data.volumeGame == value)
				{
					return;
				}
				Prefs.data.volumeGame = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D6D RID: 3437
		// (get) Token: 0x0600566D RID: 22125 RVA: 0x001BE2D3 File Offset: 0x001BC4D3
		// (set) Token: 0x0600566E RID: 22126 RVA: 0x001BE2DF File Offset: 0x001BC4DF
		public static float VolumeMusic
		{
			get
			{
				return Prefs.data.volumeMusic;
			}
			set
			{
				if (Prefs.data.volumeMusic == value)
				{
					return;
				}
				Prefs.data.volumeMusic = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D6E RID: 3438
		// (get) Token: 0x0600566F RID: 22127 RVA: 0x001BE2FF File Offset: 0x001BC4FF
		// (set) Token: 0x06005670 RID: 22128 RVA: 0x001BE30B File Offset: 0x001BC50B
		public static float VolumeAmbient
		{
			get
			{
				return Prefs.data.volumeAmbient;
			}
			set
			{
				if (Prefs.data.volumeAmbient == value)
				{
					return;
				}
				Prefs.data.volumeAmbient = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D6F RID: 3439
		// (get) Token: 0x06005671 RID: 22129 RVA: 0x001BE32B File Offset: 0x001BC52B
		// (set) Token: 0x06005672 RID: 22130 RVA: 0x001BE337 File Offset: 0x001BC537
		public static float VolumeUI
		{
			get
			{
				return Prefs.data.volumeUI;
			}
			set
			{
				if (Prefs.data.volumeUI == value)
				{
					return;
				}
				Prefs.data.volumeUI = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D70 RID: 3440
		// (get) Token: 0x06005673 RID: 22131 RVA: 0x001BE357 File Offset: 0x001BC557
		// (set) Token: 0x06005674 RID: 22132 RVA: 0x001BE363 File Offset: 0x001BC563
		public static bool AdaptiveTrainingEnabled
		{
			get
			{
				return Prefs.data.adaptiveTrainingEnabled;
			}
			set
			{
				if (Prefs.data.adaptiveTrainingEnabled == value)
				{
					return;
				}
				Prefs.data.adaptiveTrainingEnabled = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D71 RID: 3441
		// (get) Token: 0x06005675 RID: 22133 RVA: 0x001BE383 File Offset: 0x001BC583
		// (set) Token: 0x06005676 RID: 22134 RVA: 0x001BE38F File Offset: 0x001BC58F
		public static bool SteamDeckKeyboardMode
		{
			get
			{
				return Prefs.data.steamDeckKeyboardMode;
			}
			set
			{
				if (Prefs.data.steamDeckKeyboardMode == value)
				{
					return;
				}
				Prefs.data.steamDeckKeyboardMode = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D72 RID: 3442
		// (get) Token: 0x06005677 RID: 22135 RVA: 0x001BE3AF File Offset: 0x001BC5AF
		// (set) Token: 0x06005678 RID: 22136 RVA: 0x001BE3BB File Offset: 0x001BC5BB
		public static bool EdgeScreenScroll
		{
			get
			{
				return Prefs.data.edgeScreenScroll;
			}
			set
			{
				if (Prefs.data.edgeScreenScroll == value)
				{
					return;
				}
				Prefs.data.edgeScreenScroll = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D73 RID: 3443
		// (get) Token: 0x06005679 RID: 22137 RVA: 0x001BE3DB File Offset: 0x001BC5DB
		// (set) Token: 0x0600567A RID: 22138 RVA: 0x001BE3E7 File Offset: 0x001BC5E7
		public static bool RememberDrawStlyes
		{
			get
			{
				return Prefs.data.rememberDrawStyles;
			}
			set
			{
				if (Prefs.data.rememberDrawStyles == value)
				{
					return;
				}
				Prefs.data.rememberDrawStyles = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D74 RID: 3444
		// (get) Token: 0x0600567B RID: 22139 RVA: 0x001BE407 File Offset: 0x001BC607
		// (set) Token: 0x0600567C RID: 22140 RVA: 0x001BE413 File Offset: 0x001BC613
		public static bool ZoomSwitchWorldLayer
		{
			get
			{
				return Prefs.data.zoomSwitchWorldLayer;
			}
			set
			{
				if (Prefs.data.zoomSwitchWorldLayer == value)
				{
					return;
				}
				Prefs.data.zoomSwitchWorldLayer = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D75 RID: 3445
		// (get) Token: 0x0600567D RID: 22141 RVA: 0x001BE433 File Offset: 0x001BC633
		// (set) Token: 0x0600567E RID: 22142 RVA: 0x001BE43F File Offset: 0x001BC63F
		public static bool ZoomToMouse
		{
			get
			{
				return Prefs.data.zoomToMouse;
			}
			set
			{
				if (Prefs.data.zoomToMouse == value)
				{
					return;
				}
				Prefs.data.zoomToMouse = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D76 RID: 3446
		// (get) Token: 0x0600567F RID: 22143 RVA: 0x001BE45F File Offset: 0x001BC65F
		// (set) Token: 0x06005680 RID: 22144 RVA: 0x001BE46B File Offset: 0x001BC66B
		public static float ScreenShakeIntensity
		{
			get
			{
				return Prefs.data.screenShakeIntensity;
			}
			set
			{
				if (Prefs.data.screenShakeIntensity == value)
				{
					return;
				}
				Prefs.data.screenShakeIntensity = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D77 RID: 3447
		// (get) Token: 0x06005681 RID: 22145 RVA: 0x001BE48B File Offset: 0x001BC68B
		// (set) Token: 0x06005682 RID: 22146 RVA: 0x001BE497 File Offset: 0x001BC697
		public static bool RunInBackground
		{
			get
			{
				return Prefs.data.runInBackground;
			}
			set
			{
				if (Prefs.data.runInBackground == value)
				{
					return;
				}
				Prefs.data.runInBackground = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D78 RID: 3448
		// (get) Token: 0x06005683 RID: 22147 RVA: 0x001BE4B7 File Offset: 0x001BC6B7
		// (set) Token: 0x06005684 RID: 22148 RVA: 0x001BE4C3 File Offset: 0x001BC6C3
		public static TemperatureDisplayMode TemperatureMode
		{
			get
			{
				return Prefs.data.temperatureMode;
			}
			set
			{
				if (Prefs.data.temperatureMode == value)
				{
					return;
				}
				Prefs.data.temperatureMode = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D79 RID: 3449
		// (get) Token: 0x06005685 RID: 22149 RVA: 0x001BE4E3 File Offset: 0x001BC6E3
		// (set) Token: 0x06005686 RID: 22150 RVA: 0x001BE4EF File Offset: 0x001BC6EF
		public static float AutosaveIntervalDays
		{
			get
			{
				return Prefs.data.autosaveIntervalDays;
			}
			set
			{
				if (Prefs.data.autosaveIntervalDays == value)
				{
					return;
				}
				Prefs.data.autosaveIntervalDays = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D7A RID: 3450
		// (get) Token: 0x06005687 RID: 22151 RVA: 0x001BE50F File Offset: 0x001BC70F
		// (set) Token: 0x06005688 RID: 22152 RVA: 0x001BE51B File Offset: 0x001BC71B
		public static bool CustomCursorEnabled
		{
			get
			{
				return Prefs.data.customCursorEnabled;
			}
			set
			{
				if (Prefs.data.customCursorEnabled == value)
				{
					return;
				}
				Prefs.data.customCursorEnabled = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D7B RID: 3451
		// (get) Token: 0x06005689 RID: 22153 RVA: 0x001BE53B File Offset: 0x001BC73B
		// (set) Token: 0x0600568A RID: 22154 RVA: 0x001BE547 File Offset: 0x001BC747
		public static DotHighlightDisplayMode DotHighlightDisplayMode
		{
			get
			{
				return Prefs.data.dotHighlightDisplayMode;
			}
			set
			{
				if (Prefs.data.dotHighlightDisplayMode == value)
				{
					return;
				}
				Prefs.data.dotHighlightDisplayMode = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D7C RID: 3452
		// (get) Token: 0x0600568B RID: 22155 RVA: 0x001BE567 File Offset: 0x001BC767
		// (set) Token: 0x0600568C RID: 22156 RVA: 0x001BE573 File Offset: 0x001BC773
		public static HighlightStyleMode HighlightStyleMode
		{
			get
			{
				return Prefs.data.highlightStyleMode;
			}
			set
			{
				if (Prefs.data.highlightStyleMode == value)
				{
					return;
				}
				Prefs.data.highlightStyleMode = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D7D RID: 3453
		// (get) Token: 0x0600568D RID: 22157 RVA: 0x001BE593 File Offset: 0x001BC793
		// (set) Token: 0x0600568E RID: 22158 RVA: 0x001BE59F File Offset: 0x001BC79F
		public static AnimalNameDisplayMode AnimalNameMode
		{
			get
			{
				return Prefs.data.animalNameMode;
			}
			set
			{
				if (Prefs.data.animalNameMode == value)
				{
					return;
				}
				Prefs.data.animalNameMode = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D7E RID: 3454
		// (get) Token: 0x0600568F RID: 22159 RVA: 0x001BE5BF File Offset: 0x001BC7BF
		// (set) Token: 0x06005690 RID: 22160 RVA: 0x001BE5CB File Offset: 0x001BC7CB
		public static MechNameDisplayMode MechNameMode
		{
			get
			{
				return Prefs.data.mechNameMode;
			}
			set
			{
				if (Prefs.data.mechNameMode == value)
				{
					return;
				}
				Prefs.data.mechNameMode = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D7F RID: 3455
		// (get) Token: 0x06005691 RID: 22161 RVA: 0x001BE5EB File Offset: 0x001BC7EB
		// (set) Token: 0x06005692 RID: 22162 RVA: 0x001BE5F7 File Offset: 0x001BC7F7
		public static ShowWeaponsUnderPortraitMode ShowWeaponsUnderPortraitMode
		{
			get
			{
				return Prefs.data.showWeaponsUnderPortraitMode;
			}
			set
			{
				if (Prefs.data.showWeaponsUnderPortraitMode == value)
				{
					return;
				}
				Prefs.data.showWeaponsUnderPortraitMode = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D80 RID: 3456
		// (get) Token: 0x06005693 RID: 22163 RVA: 0x001BE617 File Offset: 0x001BC817
		// (set) Token: 0x06005694 RID: 22164 RVA: 0x001BE623 File Offset: 0x001BC823
		public static bool VisibleMood
		{
			get
			{
				return Prefs.data.visibleMood;
			}
			set
			{
				if (Prefs.data.visibleMood == value)
				{
					return;
				}
				Prefs.data.visibleMood = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D81 RID: 3457
		// (get) Token: 0x06005695 RID: 22165 RVA: 0x001BE643 File Offset: 0x001BC843
		// (set) Token: 0x06005696 RID: 22166 RVA: 0x001BE658 File Offset: 0x001BC858
		public static bool DevMode
		{
			get
			{
				return Prefs.data == null || Prefs.data.devMode;
			}
			set
			{
				if (Prefs.data.devMode == value)
				{
					return;
				}
				Prefs.data.devMode = value;
				if (!Prefs.data.devMode)
				{
					Prefs.data.logVerbose = false;
					Prefs.data.resetModsConfigOnCrash = true;
					DebugSettings.godMode = false;
					Find.WindowStack.TryRemove(typeof(Dialog_DevPalette), true);
				}
				Prefs.Apply();
			}
		}

		// Token: 0x17000D82 RID: 3458
		// (get) Token: 0x06005697 RID: 22167 RVA: 0x001BE6C1 File Offset: 0x001BC8C1
		// (set) Token: 0x06005698 RID: 22168 RVA: 0x001BE6D6 File Offset: 0x001BC8D6
		public static bool ResetModsConfigOnCrash
		{
			get
			{
				return Prefs.data == null || Prefs.data.resetModsConfigOnCrash;
			}
			set
			{
				if (Prefs.data.resetModsConfigOnCrash == value)
				{
					return;
				}
				Prefs.data.resetModsConfigOnCrash = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D83 RID: 3459
		// (get) Token: 0x06005699 RID: 22169 RVA: 0x001BE6F6 File Offset: 0x001BC8F6
		// (set) Token: 0x0600569A RID: 22170 RVA: 0x001BE702 File Offset: 0x001BC902
		public static List<string> PreferredNames
		{
			get
			{
				return Prefs.data.preferredNames;
			}
			set
			{
				if (Prefs.data.preferredNames == value)
				{
					return;
				}
				Prefs.data.preferredNames = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D84 RID: 3460
		// (get) Token: 0x0600569B RID: 22171 RVA: 0x001BE722 File Offset: 0x001BC922
		// (set) Token: 0x0600569C RID: 22172 RVA: 0x001BE72E File Offset: 0x001BC92E
		public static string LangFolderName
		{
			get
			{
				return Prefs.data.langFolderName;
			}
			set
			{
				if (Prefs.data.langFolderName == value)
				{
					return;
				}
				Prefs.data.langFolderName = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D85 RID: 3461
		// (get) Token: 0x0600569D RID: 22173 RVA: 0x001BE753 File Offset: 0x001BC953
		// (set) Token: 0x0600569E RID: 22174 RVA: 0x001BE75F File Offset: 0x001BC95F
		public static bool LogVerbose
		{
			get
			{
				return Prefs.data.logVerbose;
			}
			set
			{
				if (Prefs.data.logVerbose == value)
				{
					return;
				}
				Prefs.data.logVerbose = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D86 RID: 3462
		// (get) Token: 0x0600569F RID: 22175 RVA: 0x001BE77F File Offset: 0x001BC97F
		// (set) Token: 0x060056A0 RID: 22176 RVA: 0x001BE78B File Offset: 0x001BC98B
		public static bool DisableQuickStartCryptoSickness
		{
			get
			{
				return Prefs.data.disableQuickStartCryptoSickness;
			}
			set
			{
				if (Prefs.data.disableQuickStartCryptoSickness == value)
				{
					return;
				}
				Prefs.data.disableQuickStartCryptoSickness = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D87 RID: 3463
		// (get) Token: 0x060056A1 RID: 22177 RVA: 0x001BE7AB File Offset: 0x001BC9AB
		// (set) Token: 0x060056A2 RID: 22178 RVA: 0x001BE7B7 File Offset: 0x001BC9B7
		public static bool StartDevPaletteOn
		{
			get
			{
				return Prefs.data.quickStartDevPaletteOn;
			}
			set
			{
				if (Prefs.data.quickStartDevPaletteOn == value)
				{
					return;
				}
				Prefs.data.quickStartDevPaletteOn = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D88 RID: 3464
		// (get) Token: 0x060056A3 RID: 22179 RVA: 0x001BE7D7 File Offset: 0x001BC9D7
		// (set) Token: 0x060056A4 RID: 22180 RVA: 0x001BE7E3 File Offset: 0x001BC9E3
		public static bool PauseOnLoad
		{
			get
			{
				return Prefs.data.pauseOnLoad;
			}
			set
			{
				Prefs.data.pauseOnLoad = value;
			}
		}

		// Token: 0x17000D89 RID: 3465
		// (get) Token: 0x060056A5 RID: 22181 RVA: 0x001BE7F0 File Offset: 0x001BC9F0
		// (set) Token: 0x060056A6 RID: 22182 RVA: 0x001BE7FC File Offset: 0x001BC9FC
		public static AutomaticPauseMode AutomaticPauseMode
		{
			get
			{
				return Prefs.data.automaticPauseMode;
			}
			set
			{
				Prefs.data.automaticPauseMode = value;
			}
		}

		// Token: 0x17000D8A RID: 3466
		// (get) Token: 0x060056A7 RID: 22183 RVA: 0x001BE809 File Offset: 0x001BCA09
		// (set) Token: 0x060056A8 RID: 22184 RVA: 0x001BE815 File Offset: 0x001BCA15
		public static bool ShowRealtimeClock
		{
			get
			{
				return Prefs.data.showRealtimeClock;
			}
			set
			{
				Prefs.data.showRealtimeClock = value;
			}
		}

		// Token: 0x17000D8B RID: 3467
		// (get) Token: 0x060056A9 RID: 22185 RVA: 0x001BE822 File Offset: 0x001BCA22
		// (set) Token: 0x060056AA RID: 22186 RVA: 0x001BE82E File Offset: 0x001BCA2E
		public static bool TwelveHourClockMode
		{
			get
			{
				return Prefs.data.twelveHourClock;
			}
			set
			{
				Prefs.data.twelveHourClock = value;
			}
		}

		// Token: 0x17000D8C RID: 3468
		// (get) Token: 0x060056AB RID: 22187 RVA: 0x001BE83B File Offset: 0x001BCA3B
		// (set) Token: 0x060056AC RID: 22188 RVA: 0x001BE847 File Offset: 0x001BCA47
		public static bool DisableTinyText
		{
			get
			{
				return Prefs.data.disableTinyText;
			}
			set
			{
				Prefs.data.disableTinyText = value;
			}
		}

		// Token: 0x17000D8D RID: 3469
		// (get) Token: 0x060056AD RID: 22189 RVA: 0x001BE854 File Offset: 0x001BCA54
		// (set) Token: 0x060056AE RID: 22190 RVA: 0x001BE860 File Offset: 0x001BCA60
		public static bool TestMapSizes
		{
			get
			{
				return Prefs.data.testMapSizes;
			}
			set
			{
				Prefs.data.testMapSizes = value;
			}
		}

		// Token: 0x17000D8E RID: 3470
		// (get) Token: 0x060056AF RID: 22191 RVA: 0x001BE86D File Offset: 0x001BCA6D
		// (set) Token: 0x060056B0 RID: 22192 RVA: 0x001BE879 File Offset: 0x001BCA79
		public static int MaxNumberOfPlayerSettlements
		{
			get
			{
				return Prefs.data.maxNumberOfPlayerSettlements;
			}
			set
			{
				Prefs.data.maxNumberOfPlayerSettlements = value;
			}
		}

		// Token: 0x17000D8F RID: 3471
		// (get) Token: 0x060056B1 RID: 22193 RVA: 0x001BE886 File Offset: 0x001BCA86
		// (set) Token: 0x060056B2 RID: 22194 RVA: 0x001BE892 File Offset: 0x001BCA92
		public static bool PlantWindSway
		{
			get
			{
				return Prefs.data.plantWindSway;
			}
			set
			{
				Prefs.data.plantWindSway = value;
			}
		}

		// Token: 0x17000D90 RID: 3472
		// (get) Token: 0x060056B3 RID: 22195 RVA: 0x001BE89F File Offset: 0x001BCA9F
		// (set) Token: 0x060056B4 RID: 22196 RVA: 0x001BE8AB File Offset: 0x001BCAAB
		public static bool TextureCompression
		{
			get
			{
				return Prefs.data.textureCompression;
			}
			set
			{
				Prefs.data.textureCompression = value;
			}
		}

		// Token: 0x17000D91 RID: 3473
		// (get) Token: 0x060056B5 RID: 22197 RVA: 0x001BE8B8 File Offset: 0x001BCAB8
		// (set) Token: 0x060056B6 RID: 22198 RVA: 0x001BE8C4 File Offset: 0x001BCAC4
		public static bool ResourceReadoutCategorized
		{
			get
			{
				return Prefs.data.resourceReadoutCategorized;
			}
			set
			{
				if (value == Prefs.data.resourceReadoutCategorized)
				{
					return;
				}
				Prefs.data.resourceReadoutCategorized = value;
				Prefs.Save();
			}
		}

		// Token: 0x17000D92 RID: 3474
		// (get) Token: 0x060056B7 RID: 22199 RVA: 0x001BE8E4 File Offset: 0x001BCAE4
		// (set) Token: 0x060056B8 RID: 22200 RVA: 0x001BE8F0 File Offset: 0x001BCAF0
		public static float UIScale
		{
			get
			{
				return Prefs.data.uiScale;
			}
			set
			{
				Prefs.data.uiScale = value;
			}
		}

		// Token: 0x17000D93 RID: 3475
		// (get) Token: 0x060056B9 RID: 22201 RVA: 0x001BE8FD File Offset: 0x001BCAFD
		// (set) Token: 0x060056BA RID: 22202 RVA: 0x001BE909 File Offset: 0x001BCB09
		public static int ScreenWidth
		{
			get
			{
				return Prefs.data.screenWidth;
			}
			set
			{
				Prefs.data.screenWidth = value;
			}
		}

		// Token: 0x17000D94 RID: 3476
		// (get) Token: 0x060056BB RID: 22203 RVA: 0x001BE916 File Offset: 0x001BCB16
		// (set) Token: 0x060056BC RID: 22204 RVA: 0x001BE922 File Offset: 0x001BCB22
		public static int ScreenHeight
		{
			get
			{
				return Prefs.data.screenHeight;
			}
			set
			{
				Prefs.data.screenHeight = value;
			}
		}

		// Token: 0x17000D95 RID: 3477
		// (get) Token: 0x060056BD RID: 22205 RVA: 0x001BE92F File Offset: 0x001BCB2F
		// (set) Token: 0x060056BE RID: 22206 RVA: 0x001BE93B File Offset: 0x001BCB3B
		public static bool FullScreen
		{
			get
			{
				return Prefs.data.fullscreen;
			}
			set
			{
				Prefs.data.fullscreen = value;
			}
		}

		// Token: 0x17000D96 RID: 3478
		// (get) Token: 0x060056BF RID: 22207 RVA: 0x001BE948 File Offset: 0x001BCB48
		// (set) Token: 0x060056C0 RID: 22208 RVA: 0x001BE954 File Offset: 0x001BCB54
		public static bool HatsOnlyOnMap
		{
			get
			{
				return Prefs.data.hatsOnlyOnMap;
			}
			set
			{
				if (Prefs.data.hatsOnlyOnMap == value)
				{
					return;
				}
				Prefs.data.hatsOnlyOnMap = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D97 RID: 3479
		// (get) Token: 0x060056C1 RID: 22209 RVA: 0x001BE974 File Offset: 0x001BCB74
		// (set) Token: 0x060056C2 RID: 22210 RVA: 0x001BE980 File Offset: 0x001BCB80
		public static float MapDragSensitivity
		{
			get
			{
				return Prefs.data.mapDragSensitivity;
			}
			set
			{
				Prefs.data.mapDragSensitivity = value;
			}
		}

		// Token: 0x17000D98 RID: 3480
		// (get) Token: 0x060056C3 RID: 22211 RVA: 0x001BE990 File Offset: 0x001BCB90
		// (set) Token: 0x060056C4 RID: 22212 RVA: 0x001BE9D7 File Offset: 0x001BCBD7
		public static ExpansionDef BackgroundImageExpansion
		{
			get
			{
				if (Prefs.data.backgroundExpansionId != null)
				{
					ExpansionDef expansionWithIdentifier = ModLister.GetExpansionWithIdentifier(Prefs.data.backgroundExpansionId);
					if (expansionWithIdentifier != null && expansionWithIdentifier.Status != ExpansionStatus.NotInstalled)
					{
						return expansionWithIdentifier;
					}
				}
				ExpansionDef lastInstalledExpansion = ModsConfig.LastInstalledExpansion;
				if (lastInstalledExpansion != null)
				{
					return lastInstalledExpansion;
				}
				return ExpansionDefOf.Core;
			}
			set
			{
				Prefs.data.backgroundExpansionId = ((value != null) ? value.linkedMod : null);
				((UI_BackgroundMain)UIMenuBackgroundManager.background).overrideBGImage = ((value != null) ? value.BackgroundImage : null);
			}
		}

		// Token: 0x17000D99 RID: 3481
		// (get) Token: 0x060056C5 RID: 22213 RVA: 0x001BEA0A File Offset: 0x001BCC0A
		// (set) Token: 0x060056C6 RID: 22214 RVA: 0x001BEA16 File Offset: 0x001BCC16
		public static bool RandomBackgroundImage
		{
			get
			{
				return Prefs.data.randomBackground;
			}
			set
			{
				Prefs.data.randomBackground = value;
			}
		}

		// Token: 0x17000D9A RID: 3482
		// (get) Token: 0x060056C7 RID: 22215 RVA: 0x001BEA23 File Offset: 0x001BCC23
		// (set) Token: 0x060056C8 RID: 22216 RVA: 0x001BEA2F File Offset: 0x001BCC2F
		public static List<string> DebugActionsPalette
		{
			get
			{
				return Prefs.data.debugActionPalette;
			}
			set
			{
				if (Prefs.data.debugActionPalette != value)
				{
					Prefs.data.debugActionPalette = value;
					Prefs.Save();
				}
			}
		}

		// Token: 0x17000D9B RID: 3483
		// (get) Token: 0x060056C9 RID: 22217 RVA: 0x001BEA4E File Offset: 0x001BCC4E
		// (set) Token: 0x060056CA RID: 22218 RVA: 0x001BEA5A File Offset: 0x001BCC5A
		public static Vector2 DevPalettePosition
		{
			get
			{
				return Prefs.data.devPalettePosition;
			}
			set
			{
				if (Prefs.data.devPalettePosition != value)
				{
					Prefs.data.devPalettePosition = value;
					Prefs.Save();
				}
			}
		}

		// Token: 0x17000D9C RID: 3484
		// (get) Token: 0x060056CB RID: 22219 RVA: 0x001BEA7E File Offset: 0x001BCC7E
		// (set) Token: 0x060056CC RID: 22220 RVA: 0x001BEA8A File Offset: 0x001BCC8A
		public static bool SmoothCameraJumps
		{
			get
			{
				return Prefs.data.smoothCameraJumps;
			}
			set
			{
				if (Prefs.data.smoothCameraJumps == value)
				{
					return;
				}
				Prefs.data.smoothCameraJumps = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D9D RID: 3485
		// (get) Token: 0x060056CD RID: 22221 RVA: 0x001BEAAA File Offset: 0x001BCCAA
		// (set) Token: 0x060056CE RID: 22222 RVA: 0x001BEABC File Offset: 0x001BCCBC
		public static bool OpenLogOnWarnings
		{
			get
			{
				PrefsData prefsData = Prefs.data;
				return prefsData != null && prefsData.openLogOnWarnings;
			}
			set
			{
				if (Prefs.data.openLogOnWarnings == value)
				{
					return;
				}
				Prefs.data.openLogOnWarnings = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D9E RID: 3486
		// (get) Token: 0x060056CF RID: 22223 RVA: 0x001BEADC File Offset: 0x001BCCDC
		// (set) Token: 0x060056D0 RID: 22224 RVA: 0x001BEAE8 File Offset: 0x001BCCE8
		public static bool CloseLogWindowOnEscape
		{
			get
			{
				return Prefs.data.closeLogWindowOnEscape;
			}
			set
			{
				if (Prefs.data.closeLogWindowOnEscape == value)
				{
					return;
				}
				Prefs.data.closeLogWindowOnEscape = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000D9F RID: 3487
		// (get) Token: 0x060056D1 RID: 22225 RVA: 0x001BEB08 File Offset: 0x001BCD08
		// (set) Token: 0x060056D2 RID: 22226 RVA: 0x001BEB14 File Offset: 0x001BCD14
		public static int AutosavesCount
		{
			get
			{
				return Prefs.data.autosavesCount;
			}
			set
			{
				if (Prefs.data.autosavesCount == value)
				{
					return;
				}
				Prefs.data.autosavesCount = value;
				Prefs.Apply();
			}
		}

		// Token: 0x060056D3 RID: 22227 RVA: 0x001BEB34 File Offset: 0x001BCD34
		public static void Init()
		{
			bool flag = !new FileInfo(GenFilePaths.PrefsFilePath).Exists;
			Prefs.data = new PrefsData();
			Prefs.data = DirectXmlLoader.ItemFromXmlFile<PrefsData>(GenFilePaths.PrefsFilePath, true);
			BackCompatibility.PrefsDataPostLoad(Prefs.data);
			if (flag)
			{
				Prefs.data.langFolderName = LanguageDatabase.SystemLanguageFolderName();
				Prefs.data.uiScale = ResolutionUtility.GetRecommendedUIScale(Prefs.data.screenWidth, Prefs.data.screenHeight);
			}
			if (DevModePermanentlyDisabledUtility.Disabled)
			{
				Prefs.DevMode = false;
			}
			Prefs.Apply();
		}

		// Token: 0x060056D4 RID: 22228 RVA: 0x001BEBC0 File Offset: 0x001BCDC0
		public static void Save()
		{
			try
			{
				XDocument xdocument = new XDocument();
				XElement content = DirectXmlSaver.XElementFromObject(Prefs.data, typeof(PrefsData));
				xdocument.Add(content);
				xdocument.Save(GenFilePaths.PrefsFilePath);
			}
			catch (Exception ex)
			{
				GenUI.ErrorDialog("ProblemSavingFile".Translate(GenFilePaths.PrefsFilePath, ex.ToString()));
				string str = "Exception saving prefs: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x060056D5 RID: 22229 RVA: 0x001BEC54 File Offset: 0x001BCE54
		public static void Apply()
		{
			Prefs.data.Apply();
		}

		// Token: 0x060056D6 RID: 22230 RVA: 0x001BEC60 File Offset: 0x001BCE60
		public static void Notify_NewExpansion()
		{
			Prefs.data.backgroundExpansionId = null;
		}

		// Token: 0x060056D7 RID: 22231 RVA: 0x001BEC70 File Offset: 0x001BCE70
		public static NameTriple RandomPreferredName()
		{
			string rawName;
			if ((from name in Prefs.PreferredNames
			where !name.NullOrEmpty()
			select name).TryRandomElement(out rawName))
			{
				return NameTriple.FromString(rawName, false);
			}
			return null;
		}

		// Token: 0x04003EE7 RID: 16103
		private static PrefsData data;
	}
}
