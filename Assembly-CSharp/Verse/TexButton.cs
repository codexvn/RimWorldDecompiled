using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C0B RID: 3083
	[StaticConstructorOnStartup]
	public class TexButton
	{
		// Token: 0x040037F1 RID: 14321
		public static readonly Texture2D CloseXBig = ContentFinder<Texture2D>.Get("UI/Widgets/CloseX", true);

		// Token: 0x040037F2 RID: 14322
		public static readonly Texture2D CloseXSmall = ContentFinder<Texture2D>.Get("UI/Widgets/CloseXSmall", true);

		// Token: 0x040037F3 RID: 14323
		public static readonly Texture2D NextBig = ContentFinder<Texture2D>.Get("UI/Widgets/NextArrow", true);

		// Token: 0x040037F4 RID: 14324
		public static readonly Texture2D Delete = ContentFinder<Texture2D>.Get("UI/Buttons/Dismiss", true);

		// Token: 0x040037F5 RID: 14325
		public static readonly Texture2D ReorderUp = ContentFinder<Texture2D>.Get("UI/Buttons/ReorderUp", true);

		// Token: 0x040037F6 RID: 14326
		public static readonly Texture2D ReorderDown = ContentFinder<Texture2D>.Get("UI/Buttons/ReorderDown", true);

		// Token: 0x040037F7 RID: 14327
		public static readonly Texture2D Plus = ContentFinder<Texture2D>.Get("UI/Buttons/Plus", true);

		// Token: 0x040037F8 RID: 14328
		public static readonly Texture2D Minus = ContentFinder<Texture2D>.Get("UI/Buttons/Minus", true);

		// Token: 0x040037F9 RID: 14329
		public static readonly Texture2D Suspend = ContentFinder<Texture2D>.Get("UI/Buttons/Suspend", true);

		// Token: 0x040037FA RID: 14330
		public static readonly Texture2D PlaceholderImage = ContentFinder<Texture2D>.Get("PlaceholderImage", true);

		// Token: 0x040037FB RID: 14331
		public static readonly Texture2D SelectOverlappingNext = ContentFinder<Texture2D>.Get("UI/Buttons/SelectNextOverlapping", true);

		// Token: 0x040037FC RID: 14332
		public static readonly Texture2D Info = ContentFinder<Texture2D>.Get("UI/Buttons/InfoButton", true);

		// Token: 0x040037FD RID: 14333
		public static readonly Texture2D Rename = ContentFinder<Texture2D>.Get("UI/Buttons/Rename", true);

		// Token: 0x040037FE RID: 14334
		public static readonly Texture2D Banish = ContentFinder<Texture2D>.Get("UI/Buttons/Banish", true);

		// Token: 0x040037FF RID: 14335
		public static readonly Texture2D OpenStatsReport = ContentFinder<Texture2D>.Get("UI/Buttons/OpenStatsReport", true);

		// Token: 0x04003800 RID: 14336
		public static readonly Texture2D RenounceTitle = ContentFinder<Texture2D>.Get("UI/Buttons/Renounce", true);

		// Token: 0x04003801 RID: 14337
		public static readonly Texture2D ExecuteColonist = ContentFinder<Texture2D>.Get("UI/Buttons/Execute", true);

		// Token: 0x04003802 RID: 14338
		public static readonly Texture2D Copy = ContentFinder<Texture2D>.Get("UI/Buttons/Copy", true);

		// Token: 0x04003803 RID: 14339
		public static readonly Texture2D Paste = ContentFinder<Texture2D>.Get("UI/Buttons/Paste", true);

		// Token: 0x04003804 RID: 14340
		public static readonly Texture2D Drop = ContentFinder<Texture2D>.Get("UI/Buttons/Drop", true);

		// Token: 0x04003805 RID: 14341
		public static readonly Texture2D Ingest = ContentFinder<Texture2D>.Get("UI/Buttons/Ingest", true);

		// Token: 0x04003806 RID: 14342
		public static readonly Texture2D DragHash = ContentFinder<Texture2D>.Get("UI/Buttons/DragHash", true);

		// Token: 0x04003807 RID: 14343
		public static readonly Texture2D Infinity = ContentFinder<Texture2D>.Get("UI/Buttons/Infinity", true);

		// Token: 0x04003808 RID: 14344
		public static readonly Texture2D Search = ContentFinder<Texture2D>.Get("UI/Widgets/Search", true);

		// Token: 0x04003809 RID: 14345
		public static readonly Texture2D SearchButton = ContentFinder<Texture2D>.Get("UI/Widgets/SearchToggle", true);

		// Token: 0x0400380A RID: 14346
		public static readonly Texture2D CodexButton = ContentFinder<Texture2D>.Get("UI/Buttons/OpenCodex", true);

		// Token: 0x0400380B RID: 14347
		public static readonly Texture2D ToggleLog = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/ToggleLog", true);

		// Token: 0x0400380C RID: 14348
		public static readonly Texture2D OpenDebugActionsMenu = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/OpenDebugActionsMenu", true);

		// Token: 0x0400380D RID: 14349
		public static readonly Texture2D OpenInspector = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/OpenInspector", true);

		// Token: 0x0400380E RID: 14350
		public static readonly Texture2D OpenInspectSettings = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/OpenInspectSettings", true);

		// Token: 0x0400380F RID: 14351
		public static readonly Texture2D GodModeDisabled = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/GodModeDisabled", true);

		// Token: 0x04003810 RID: 14352
		public static readonly Texture2D GodModeEnabled = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/GodModeEnabled", true);

		// Token: 0x04003811 RID: 14353
		public static readonly Texture2D ToggleTweak = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/ToggleTweak", true);

		// Token: 0x04003812 RID: 14354
		public static readonly Texture2D ToggleDevPalette = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/ToggleDevPalette", true);

		// Token: 0x04003813 RID: 14355
		public static readonly Texture2D HotReloadDefs = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/HotReloadDefs", true);

		// Token: 0x04003814 RID: 14356
		public static readonly Texture2D Add = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Add", true);

		// Token: 0x04003815 RID: 14357
		public static readonly Texture2D NewItem = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/NewItem", true);

		// Token: 0x04003816 RID: 14358
		public static readonly Texture2D Reveal = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Reveal", true);

		// Token: 0x04003817 RID: 14359
		public static readonly Texture2D Collapse = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Collapse", true);

		// Token: 0x04003818 RID: 14360
		public static readonly Texture2D Empty = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Empty", true);

		// Token: 0x04003819 RID: 14361
		public static readonly Texture2D Save = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Save", true);

		// Token: 0x0400381A RID: 14362
		public static readonly Texture2D NewFile = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/NewFile", true);

		// Token: 0x0400381B RID: 14363
		public static readonly Texture2D RenameDev = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Rename", true);

		// Token: 0x0400381C RID: 14364
		public static readonly Texture2D Reload = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Reload", true);

		// Token: 0x0400381D RID: 14365
		public static readonly Texture2D Play = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Play", true);

		// Token: 0x0400381E RID: 14366
		public static readonly Texture2D Stop = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/Stop", true);

		// Token: 0x0400381F RID: 14367
		public static readonly Texture2D RangeMatch = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/RangeMatch", true);

		// Token: 0x04003820 RID: 14368
		public static readonly Texture2D CenterOnPointsTex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/CenterOnPoints", true);

		// Token: 0x04003821 RID: 14369
		public static readonly Texture2D CurveResetTex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/CurveReset", true);

		// Token: 0x04003822 RID: 14370
		public static readonly Texture2D QuickZoomHor1Tex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomHor1", true);

		// Token: 0x04003823 RID: 14371
		public static readonly Texture2D QuickZoomHor100Tex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomHor100", true);

		// Token: 0x04003824 RID: 14372
		public static readonly Texture2D QuickZoomHor20kTex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomHor20k", true);

		// Token: 0x04003825 RID: 14373
		public static readonly Texture2D QuickZoomVer1Tex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomVer1", true);

		// Token: 0x04003826 RID: 14374
		public static readonly Texture2D QuickZoomVer100Tex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomVer100", true);

		// Token: 0x04003827 RID: 14375
		public static readonly Texture2D QuickZoomVer20kTex = ContentFinder<Texture2D>.Get("UI/Buttons/Dev/QuickZoomVer20k", true);

		// Token: 0x04003828 RID: 14376
		public static readonly Texture2D IconBlog = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Blog", true);

		// Token: 0x04003829 RID: 14377
		public static readonly Texture2D IconForums = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Forums", true);

		// Token: 0x0400382A RID: 14378
		public static readonly Texture2D IconTwitter = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Twitter", true);

		// Token: 0x0400382B RID: 14379
		public static readonly Texture2D IconBook = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Book", true);

		// Token: 0x0400382C RID: 14380
		public static readonly Texture2D IconSoundtrack = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Soundtrack", true);

		// Token: 0x0400382D RID: 14381
		public static readonly Texture2D ShowLearningHelper = ContentFinder<Texture2D>.Get("UI/Buttons/ShowLearningHelper", true);

		// Token: 0x0400382E RID: 14382
		public static readonly Texture2D ShowZones = ContentFinder<Texture2D>.Get("UI/Buttons/ShowZones", true);

		// Token: 0x0400382F RID: 14383
		public static readonly Texture2D ShowFertilityOverlay = ContentFinder<Texture2D>.Get("UI/Buttons/ShowFertilityOverlay", true);

		// Token: 0x04003830 RID: 14384
		public static readonly Texture2D ShowPollutionOverlay = ContentFinder<Texture2D>.Get("UI/Buttons/ShowPollutionOverlay", true);

		// Token: 0x04003831 RID: 14385
		public static readonly Texture2D ShowTerrainAffordanceOverlay = ContentFinder<Texture2D>.Get("UI/Buttons/ShowTerrainAffordanceOverlay", true);

		// Token: 0x04003832 RID: 14386
		public static readonly Texture2D ShowBeauty = ContentFinder<Texture2D>.Get("UI/Buttons/ShowBeauty", true);

		// Token: 0x04003833 RID: 14387
		public static readonly Texture2D ShowRoomStats = ContentFinder<Texture2D>.Get("UI/Buttons/ShowRoomStats", true);

		// Token: 0x04003834 RID: 14388
		public static readonly Texture2D ShowColonistBar = ContentFinder<Texture2D>.Get("UI/Buttons/ShowColonistBar", true);

		// Token: 0x04003835 RID: 14389
		public static readonly Texture2D ShowRoofOverlay = ContentFinder<Texture2D>.Get("UI/Buttons/ShowRoofOverlay", true);

		// Token: 0x04003836 RID: 14390
		public static readonly Texture2D AutoHomeArea = ContentFinder<Texture2D>.Get("UI/Buttons/AutoHomeArea", true);

		// Token: 0x04003837 RID: 14391
		public static readonly Texture2D AutoRebuild = ContentFinder<Texture2D>.Get("UI/Buttons/AutoRebuild", true);

		// Token: 0x04003838 RID: 14392
		public static readonly Texture2D CategorizedResourceReadout = ContentFinder<Texture2D>.Get("UI/Buttons/ResourceReadoutCategorized", true);

		// Token: 0x04003839 RID: 14393
		public static readonly Texture2D LockNorthUp = ContentFinder<Texture2D>.Get("UI/Buttons/LockNorthUp", true);

		// Token: 0x0400383A RID: 14394
		public static readonly Texture2D UsePlanetDayNightSystem = ContentFinder<Texture2D>.Get("UI/Buttons/UsePlanetDayNightSystem", true);

		// Token: 0x0400383B RID: 14395
		public static readonly Texture2D ShowExpandingIcons = ContentFinder<Texture2D>.Get("UI/Buttons/ShowExpandingIcons", true);

		// Token: 0x0400383C RID: 14396
		public static readonly Texture2D ShowWorldFeatures = ContentFinder<Texture2D>.Get("UI/Buttons/ShowWorldFeatures", true);

		// Token: 0x0400383D RID: 14397
		public static readonly Texture2D ShowTemperatureOverlay = ContentFinder<Texture2D>.Get("UI/Buttons/ToggleHeatOverlay", true);

		// Token: 0x0400383E RID: 14398
		public static readonly Texture2D ShowVacuumOverlay = ContentFinder<Texture2D>.Get("UI/Buttons/ToggleVacuumOverlay", true);

		// Token: 0x0400383F RID: 14399
		public static readonly Texture2D ShowLandmarkIcons = ContentFinder<Texture2D>.Get("UI/Buttons/DisplayLandmarks", true);

		// Token: 0x04003840 RID: 14400
		public static readonly Texture2D ShowOtherFactionBases = ContentFinder<Texture2D>.Get("UI/Buttons/DisplayOtherFactionBases", true);

		// Token: 0x04003841 RID: 14401
		public static readonly Texture2D ShowImportantLocations = ContentFinder<Texture2D>.Get("UI/Buttons/DisplayImportantLocations", true);

		// Token: 0x04003842 RID: 14402
		public static readonly Texture2D[] SpeedButtonTextures = new Texture2D[]
		{
			ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Pause", true),
			ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Normal", true),
			ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Fast", true),
			ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Superfast", true),
			ContentFinder<Texture2D>.Get("UI/TimeControls/TimeSpeedButton_Superfast", true)
		};
	}
}
