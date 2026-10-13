using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000CC9 RID: 3273
	public static class Find
	{
		// Token: 0x17000CD4 RID: 3284
		// (get) Token: 0x06005399 RID: 21401 RVA: 0x001AEF26 File Offset: 0x001AD126
		public static Root Root
		{
			get
			{
				return Current.Root;
			}
		}

		// Token: 0x17000CD5 RID: 3285
		// (get) Token: 0x0600539A RID: 21402 RVA: 0x001AEF2D File Offset: 0x001AD12D
		public static SoundRoot SoundRoot
		{
			get
			{
				return Current.Root.soundRoot;
			}
		}

		// Token: 0x17000CD6 RID: 3286
		// (get) Token: 0x0600539B RID: 21403 RVA: 0x001AEF39 File Offset: 0x001AD139
		public static UIRoot UIRoot
		{
			get
			{
				if (!(Current.Root != null))
				{
					return null;
				}
				return Current.Root.uiRoot;
			}
		}

		// Token: 0x17000CD7 RID: 3287
		// (get) Token: 0x0600539C RID: 21404 RVA: 0x001AEF54 File Offset: 0x001AD154
		public static MusicManagerEntry MusicManagerEntry
		{
			get
			{
				return ((Root_Entry)Current.Root).musicManagerEntry;
			}
		}

		// Token: 0x17000CD8 RID: 3288
		// (get) Token: 0x0600539D RID: 21405 RVA: 0x001AEF65 File Offset: 0x001AD165
		public static MusicManagerPlay MusicManagerPlay
		{
			get
			{
				return ((Root_Play)Current.Root).musicManagerPlay;
			}
		}

		// Token: 0x17000CD9 RID: 3289
		// (get) Token: 0x0600539E RID: 21406 RVA: 0x001AEF76 File Offset: 0x001AD176
		public static LanguageWorker ActiveLanguageWorker
		{
			get
			{
				return LanguageDatabase.activeLanguage.Worker;
			}
		}

		// Token: 0x17000CDA RID: 3290
		// (get) Token: 0x0600539F RID: 21407 RVA: 0x001AEF82 File Offset: 0x001AD182
		public static Camera Camera
		{
			get
			{
				return Current.Camera;
			}
		}

		// Token: 0x17000CDB RID: 3291
		// (get) Token: 0x060053A0 RID: 21408 RVA: 0x001AEF89 File Offset: 0x001AD189
		public static CameraDriver CameraDriver
		{
			get
			{
				return Current.CameraDriver;
			}
		}

		// Token: 0x17000CDC RID: 3292
		// (get) Token: 0x060053A1 RID: 21409 RVA: 0x001AEF90 File Offset: 0x001AD190
		public static ColorCorrectionCurves CameraColor
		{
			get
			{
				return Current.ColorCorrectionCurves;
			}
		}

		// Token: 0x17000CDD RID: 3293
		// (get) Token: 0x060053A2 RID: 21410 RVA: 0x001AEF97 File Offset: 0x001AD197
		public static Camera PawnCacheCamera
		{
			get
			{
				return PawnCacheCameraManager.PawnCacheCamera;
			}
		}

		// Token: 0x17000CDE RID: 3294
		// (get) Token: 0x060053A3 RID: 21411 RVA: 0x001AEF9E File Offset: 0x001AD19E
		public static PawnCacheRenderer PawnCacheRenderer
		{
			get
			{
				return PawnCacheCameraManager.PawnCacheRenderer;
			}
		}

		// Token: 0x17000CDF RID: 3295
		// (get) Token: 0x060053A4 RID: 21412 RVA: 0x001AEFA5 File Offset: 0x001AD1A5
		public static Camera WorldCamera
		{
			get
			{
				return WorldCameraManager.WorldCamera;
			}
		}

		// Token: 0x17000CE0 RID: 3296
		// (get) Token: 0x060053A5 RID: 21413 RVA: 0x001AEFAC File Offset: 0x001AD1AC
		public static WorldCameraDriver WorldCameraDriver
		{
			get
			{
				return WorldCameraManager.WorldCameraDriver;
			}
		}

		// Token: 0x17000CE1 RID: 3297
		// (get) Token: 0x060053A6 RID: 21414 RVA: 0x001AEFB3 File Offset: 0x001AD1B3
		public static WindowStack WindowStack
		{
			get
			{
				UIRoot uiroot = Find.UIRoot;
				if (uiroot == null)
				{
					return null;
				}
				return uiroot.windows;
			}
		}

		// Token: 0x17000CE2 RID: 3298
		// (get) Token: 0x060053A7 RID: 21415 RVA: 0x001AEFC5 File Offset: 0x001AD1C5
		public static ScreenshotModeHandler ScreenshotModeHandler
		{
			get
			{
				UIRoot uiroot = Find.UIRoot;
				if (uiroot == null)
				{
					return null;
				}
				return uiroot.screenshotMode;
			}
		}

		// Token: 0x17000CE3 RID: 3299
		// (get) Token: 0x060053A8 RID: 21416 RVA: 0x001AEFD7 File Offset: 0x001AD1D7
		public static MainButtonsRoot MainButtonsRoot
		{
			get
			{
				return ((UIRoot_Play)Find.UIRoot).mainButtonsRoot;
			}
		}

		// Token: 0x17000CE4 RID: 3300
		// (get) Token: 0x060053A9 RID: 21417 RVA: 0x001AEFE8 File Offset: 0x001AD1E8
		public static MainTabsRoot MainTabsRoot
		{
			get
			{
				return Find.MainButtonsRoot.tabs;
			}
		}

		// Token: 0x17000CE5 RID: 3301
		// (get) Token: 0x060053AA RID: 21418 RVA: 0x001AEFF4 File Offset: 0x001AD1F4
		public static MapInterface MapUI
		{
			get
			{
				return ((UIRoot_Play)Find.UIRoot).mapUI;
			}
		}

		// Token: 0x17000CE6 RID: 3302
		// (get) Token: 0x060053AB RID: 21419 RVA: 0x001AF005 File Offset: 0x001AD205
		public static AlertsReadout Alerts
		{
			get
			{
				return ((UIRoot_Play)Find.UIRoot).alerts;
			}
		}

		// Token: 0x17000CE7 RID: 3303
		// (get) Token: 0x060053AC RID: 21420 RVA: 0x001AF016 File Offset: 0x001AD216
		public static Selector Selector
		{
			get
			{
				return Find.MapUI.selector;
			}
		}

		// Token: 0x17000CE8 RID: 3304
		// (get) Token: 0x060053AD RID: 21421 RVA: 0x001AF022 File Offset: 0x001AD222
		public static Targeter Targeter
		{
			get
			{
				return Find.MapUI.targeter;
			}
		}

		// Token: 0x17000CE9 RID: 3305
		// (get) Token: 0x060053AE RID: 21422 RVA: 0x001AF02E File Offset: 0x001AD22E
		public static ColonistBar ColonistBar
		{
			get
			{
				return Find.MapUI.colonistBar;
			}
		}

		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x060053AF RID: 21423 RVA: 0x001AF03A File Offset: 0x001AD23A
		public static DesignatorManager DesignatorManager
		{
			get
			{
				return Find.MapUI.designatorManager;
			}
		}

		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x060053B0 RID: 21424 RVA: 0x001AF046 File Offset: 0x001AD246
		public static ReverseDesignatorDatabase ReverseDesignatorDatabase
		{
			get
			{
				return Find.MapUI.reverseDesignatorDatabase;
			}
		}

		// Token: 0x17000CEC RID: 3308
		// (get) Token: 0x060053B1 RID: 21425 RVA: 0x001AF052 File Offset: 0x001AD252
		public static GameInitData GameInitData
		{
			get
			{
				Game game = Current.Game;
				if (game == null)
				{
					return null;
				}
				return game.InitData;
			}
		}

		// Token: 0x17000CED RID: 3309
		// (get) Token: 0x060053B2 RID: 21426 RVA: 0x001AF064 File Offset: 0x001AD264
		public static GameInfo GameInfo
		{
			get
			{
				return Current.Game.Info;
			}
		}

		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x060053B3 RID: 21427 RVA: 0x001AF070 File Offset: 0x001AD270
		public static Scenario Scenario
		{
			get
			{
				if (Current.Game != null && Current.Game.Scenario != null)
				{
					return Current.Game.Scenario;
				}
				if (ScenarioMaker.GeneratingScenario != null)
				{
					return ScenarioMaker.GeneratingScenario;
				}
				if (Find.UIRoot != null)
				{
					Page_ScenarioEditor page_ScenarioEditor = Find.WindowStack.WindowOfType<Page_ScenarioEditor>();
					if (page_ScenarioEditor != null)
					{
						return page_ScenarioEditor.EditingScenario;
					}
				}
				return null;
			}
		}

		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x060053B4 RID: 21428 RVA: 0x001AF0C5 File Offset: 0x001AD2C5
		public static World World
		{
			get
			{
				if (Current.Game == null || Current.Game.World == null)
				{
					return Current.CreatingWorld;
				}
				return Current.Game.World;
			}
		}

		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x060053B5 RID: 21429 RVA: 0x001AF0EA File Offset: 0x001AD2EA
		public static List<Map> Maps
		{
			get
			{
				Game game = Current.Game;
				if (game == null)
				{
					return null;
				}
				return game.Maps;
			}
		}

		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x060053B6 RID: 21430 RVA: 0x001AF0FC File Offset: 0x001AD2FC
		public static Map CurrentMap
		{
			get
			{
				Game game = Current.Game;
				if (game == null)
				{
					return null;
				}
				return game.CurrentMap;
			}
		}

		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x060053B7 RID: 21431 RVA: 0x001AF10E File Offset: 0x001AD30E
		public static Map AnyPlayerHomeMap
		{
			get
			{
				Game game = Current.Game;
				if (game == null)
				{
					return null;
				}
				return game.AnyPlayerHomeMap;
			}
		}

		// Token: 0x17000CF3 RID: 3315
		// (get) Token: 0x060053B8 RID: 21432 RVA: 0x001AF120 File Offset: 0x001AD320
		public static Map RandomPlayerHomeMap
		{
			get
			{
				Game game = Current.Game;
				if (game == null)
				{
					return null;
				}
				return game.RandomPlayerHomeMap;
			}
		}

		// Token: 0x17000CF4 RID: 3316
		// (get) Token: 0x060053B9 RID: 21433 RVA: 0x001AF132 File Offset: 0x001AD332
		public static Map RandomRootSurfacePlayerHomeMap
		{
			get
			{
				Game game = Current.Game;
				if (game == null)
				{
					return null;
				}
				return game.RandomRootSurfacePlayerHomeMap;
			}
		}

		// Token: 0x17000CF5 RID: 3317
		// (get) Token: 0x060053BA RID: 21434 RVA: 0x001AF132 File Offset: 0x001AD332
		public static Map RandomSurfacePlayerHomeMap
		{
			get
			{
				Game game = Current.Game;
				if (game == null)
				{
					return null;
				}
				return game.RandomRootSurfacePlayerHomeMap;
			}
		}

		// Token: 0x17000CF6 RID: 3318
		// (get) Token: 0x060053BB RID: 21435 RVA: 0x001AF144 File Offset: 0x001AD344
		public static StoryWatcher StoryWatcher
		{
			get
			{
				return Current.Game.storyWatcher;
			}
		}

		// Token: 0x17000CF7 RID: 3319
		// (get) Token: 0x060053BC RID: 21436 RVA: 0x001AF150 File Offset: 0x001AD350
		public static ResearchManager ResearchManager
		{
			get
			{
				return Current.Game.researchManager;
			}
		}

		// Token: 0x17000CF8 RID: 3320
		// (get) Token: 0x060053BD RID: 21437 RVA: 0x001AF15C File Offset: 0x001AD35C
		public static AnalysisManager AnalysisManager
		{
			get
			{
				return Current.Game.analysisManager;
			}
		}

		// Token: 0x17000CF9 RID: 3321
		// (get) Token: 0x060053BE RID: 21438 RVA: 0x001AF168 File Offset: 0x001AD368
		public static Storyteller Storyteller
		{
			get
			{
				Game game = Current.Game;
				if (game == null)
				{
					return null;
				}
				return game.storyteller;
			}
		}

		// Token: 0x17000CFA RID: 3322
		// (get) Token: 0x060053BF RID: 21439 RVA: 0x001AF17A File Offset: 0x001AD37A
		public static GameEnder GameEnder
		{
			get
			{
				return Current.Game.gameEnder;
			}
		}

		// Token: 0x17000CFB RID: 3323
		// (get) Token: 0x060053C0 RID: 21440 RVA: 0x001AF186 File Offset: 0x001AD386
		public static LetterStack LetterStack
		{
			get
			{
				return Current.Game.letterStack;
			}
		}

		// Token: 0x17000CFC RID: 3324
		// (get) Token: 0x060053C1 RID: 21441 RVA: 0x001AF192 File Offset: 0x001AD392
		public static Archive Archive
		{
			get
			{
				History history = Find.History;
				if (history == null)
				{
					return null;
				}
				return history.archive;
			}
		}

		// Token: 0x17000CFD RID: 3325
		// (get) Token: 0x060053C2 RID: 21442 RVA: 0x001AF1A4 File Offset: 0x001AD3A4
		public static PlaySettings PlaySettings
		{
			get
			{
				return Current.Game.playSettings;
			}
		}

		// Token: 0x17000CFE RID: 3326
		// (get) Token: 0x060053C3 RID: 21443 RVA: 0x001AF1B0 File Offset: 0x001AD3B0
		public static History History
		{
			get
			{
				Game game = Current.Game;
				if (game == null)
				{
					return null;
				}
				return game.history;
			}
		}

		// Token: 0x17000CFF RID: 3327
		// (get) Token: 0x060053C4 RID: 21444 RVA: 0x001AF1C2 File Offset: 0x001AD3C2
		public static TaleManager TaleManager
		{
			get
			{
				return Current.Game.taleManager;
			}
		}

		// Token: 0x17000D00 RID: 3328
		// (get) Token: 0x060053C5 RID: 21445 RVA: 0x001AF1CE File Offset: 0x001AD3CE
		public static PlayLog PlayLog
		{
			get
			{
				return Current.Game.playLog;
			}
		}

		// Token: 0x17000D01 RID: 3329
		// (get) Token: 0x060053C6 RID: 21446 RVA: 0x001AF1DA File Offset: 0x001AD3DA
		public static BattleLog BattleLog
		{
			get
			{
				return Current.Game.battleLog;
			}
		}

		// Token: 0x17000D02 RID: 3330
		// (get) Token: 0x060053C7 RID: 21447 RVA: 0x001AF1E6 File Offset: 0x001AD3E6
		public static TickManager TickManager
		{
			get
			{
				return Current.Game.tickManager;
			}
		}

		// Token: 0x17000D03 RID: 3331
		// (get) Token: 0x060053C8 RID: 21448 RVA: 0x001AF1F2 File Offset: 0x001AD3F2
		public static Tutor Tutor
		{
			get
			{
				Game game = Current.Game;
				if (game == null)
				{
					return null;
				}
				return game.tutor;
			}
		}

		// Token: 0x17000D04 RID: 3332
		// (get) Token: 0x060053C9 RID: 21449 RVA: 0x001AF204 File Offset: 0x001AD404
		public static TutorialState TutorialState
		{
			get
			{
				return Current.Game.tutor.tutorialState;
			}
		}

		// Token: 0x17000D05 RID: 3333
		// (get) Token: 0x060053CA RID: 21450 RVA: 0x001AF215 File Offset: 0x001AD415
		public static ActiveLessonHandler ActiveLesson
		{
			get
			{
				Game game = Current.Game;
				if (game == null)
				{
					return null;
				}
				return game.tutor.activeLesson;
			}
		}

		// Token: 0x17000D06 RID: 3334
		// (get) Token: 0x060053CB RID: 21451 RVA: 0x001AF22C File Offset: 0x001AD42C
		public static Autosaver Autosaver
		{
			get
			{
				return Current.Game.autosaver;
			}
		}

		// Token: 0x17000D07 RID: 3335
		// (get) Token: 0x060053CC RID: 21452 RVA: 0x001AF238 File Offset: 0x001AD438
		public static DateNotifier DateNotifier
		{
			get
			{
				return Current.Game.dateNotifier;
			}
		}

		// Token: 0x17000D08 RID: 3336
		// (get) Token: 0x060053CD RID: 21453 RVA: 0x001AF244 File Offset: 0x001AD444
		public static SignalManager SignalManager
		{
			get
			{
				return Current.Game.signalManager;
			}
		}

		// Token: 0x17000D09 RID: 3337
		// (get) Token: 0x060053CE RID: 21454 RVA: 0x001AF250 File Offset: 0x001AD450
		public static UniqueIDsManager UniqueIDsManager
		{
			get
			{
				Game game = Current.Game;
				if (game == null)
				{
					return null;
				}
				return game.uniqueIDsManager;
			}
		}

		// Token: 0x17000D0A RID: 3338
		// (get) Token: 0x060053CF RID: 21455 RVA: 0x001AF262 File Offset: 0x001AD462
		public static QuestManager QuestManager
		{
			get
			{
				return Current.Game.questManager;
			}
		}

		// Token: 0x17000D0B RID: 3339
		// (get) Token: 0x060053D0 RID: 21456 RVA: 0x001AF26E File Offset: 0x001AD46E
		public static RelationshipRecords RelationshipRecords
		{
			get
			{
				return Current.Game.relationshipRecords;
			}
		}

		// Token: 0x17000D0C RID: 3340
		// (get) Token: 0x060053D1 RID: 21457 RVA: 0x001AF27A File Offset: 0x001AD47A
		public static TransportShipManager TransportShipManager
		{
			get
			{
				return Current.Game.transportShipManager;
			}
		}

		// Token: 0x17000D0D RID: 3341
		// (get) Token: 0x060053D2 RID: 21458 RVA: 0x001AF286 File Offset: 0x001AD486
		public static GameComponent_Bossgroup BossgroupManager
		{
			get
			{
				return Current.Game.GetComponent<GameComponent_Bossgroup>();
			}
		}

		// Token: 0x17000D0E RID: 3342
		// (get) Token: 0x060053D3 RID: 21459 RVA: 0x001AF292 File Offset: 0x001AD492
		public static GameComponent_Anomaly Anomaly
		{
			get
			{
				GameComponent_Anomaly result;
				if ((result = Find.cachedAnomaly) == null)
				{
					result = (Find.cachedAnomaly = Current.Game.GetComponent<GameComponent_Anomaly>());
				}
				return result;
			}
		}

		// Token: 0x17000D0F RID: 3343
		// (get) Token: 0x060053D4 RID: 21460 RVA: 0x001AF2AD File Offset: 0x001AD4AD
		public static StudyManager StudyManager
		{
			get
			{
				return Current.Game.studyManager;
			}
		}

		// Token: 0x17000D10 RID: 3344
		// (get) Token: 0x060053D5 RID: 21461 RVA: 0x001AF2B9 File Offset: 0x001AD4B9
		public static CustomXenogermDatabase CustomXenogermDatabase
		{
			get
			{
				return Current.Game.customXenogermDatabase;
			}
		}

		// Token: 0x17000D11 RID: 3345
		// (get) Token: 0x060053D6 RID: 21462 RVA: 0x001AF2C5 File Offset: 0x001AD4C5
		public static GameComponent_PsychicRitualManager PsychicRitualManager
		{
			get
			{
				return Current.Game.GetComponent<GameComponent_PsychicRitualManager>();
			}
		}

		// Token: 0x17000D12 RID: 3346
		// (get) Token: 0x060053D7 RID: 21463 RVA: 0x001AF2D1 File Offset: 0x001AD4D1
		public static GameComponent_PawnDuplicator PawnDuplicator
		{
			get
			{
				return Current.Game.GetComponent<GameComponent_PawnDuplicator>();
			}
		}

		// Token: 0x17000D13 RID: 3347
		// (get) Token: 0x060053D8 RID: 21464 RVA: 0x001AF2DD File Offset: 0x001AD4DD
		public static HiddenItemsManager HiddenItemsManager
		{
			get
			{
				return Current.Game.hiddenItemsManager;
			}
		}

		// Token: 0x17000D14 RID: 3348
		// (get) Token: 0x060053D9 RID: 21465 RVA: 0x001AF2E9 File Offset: 0x001AD4E9
		public static EntityCodex EntityCodex
		{
			get
			{
				return Current.Game.entityCodex;
			}
		}

		// Token: 0x17000D15 RID: 3349
		// (get) Token: 0x060053DA RID: 21466 RVA: 0x001AF2F5 File Offset: 0x001AD4F5
		public static Gravship CurrentGravship
		{
			get
			{
				return Current.Game.Gravship;
			}
		}

		// Token: 0x17000D16 RID: 3350
		// (get) Token: 0x060053DB RID: 21467 RVA: 0x001AF301 File Offset: 0x001AD501
		public static FactionManager FactionManager
		{
			get
			{
				return Find.World.factionManager;
			}
		}

		// Token: 0x17000D17 RID: 3351
		// (get) Token: 0x060053DC RID: 21468 RVA: 0x001AF30D File Offset: 0x001AD50D
		public static IdeoManager IdeoManager
		{
			get
			{
				return Find.World.ideoManager;
			}
		}

		// Token: 0x17000D18 RID: 3352
		// (get) Token: 0x060053DD RID: 21469 RVA: 0x001AF319 File Offset: 0x001AD519
		public static WorldPawns WorldPawns
		{
			get
			{
				return Find.World.worldPawns;
			}
		}

		// Token: 0x17000D19 RID: 3353
		// (get) Token: 0x060053DE RID: 21470 RVA: 0x001AF325 File Offset: 0x001AD525
		public static WorldObjectsHolder WorldObjects
		{
			get
			{
				return Find.World.worldObjects;
			}
		}

		// Token: 0x17000D1A RID: 3354
		// (get) Token: 0x060053DF RID: 21471 RVA: 0x001AF331 File Offset: 0x001AD531
		public static WorldGrid WorldGrid
		{
			get
			{
				return Find.World.grid;
			}
		}

		// Token: 0x17000D1B RID: 3355
		// (get) Token: 0x060053E0 RID: 21472 RVA: 0x001AF33D File Offset: 0x001AD53D
		public static WorldDebugDrawer WorldDebugDrawer
		{
			get
			{
				return Find.World.debugDrawer;
			}
		}

		// Token: 0x17000D1C RID: 3356
		// (get) Token: 0x060053E1 RID: 21473 RVA: 0x001AF349 File Offset: 0x001AD549
		public static WorldPathGrid WorldPathGrid
		{
			get
			{
				return Find.World.pathGrid;
			}
		}

		// Token: 0x17000D1D RID: 3357
		// (get) Token: 0x060053E2 RID: 21474 RVA: 0x001AF355 File Offset: 0x001AD555
		public static WorldDynamicDrawManager WorldDynamicDrawManager
		{
			get
			{
				return Find.World.dynamicDrawManager;
			}
		}

		// Token: 0x17000D1E RID: 3358
		// (get) Token: 0x060053E3 RID: 21475 RVA: 0x001AF361 File Offset: 0x001AD561
		public static WorldPathPool WorldPathPool
		{
			get
			{
				return Find.World.pathPool;
			}
		}

		// Token: 0x17000D1F RID: 3359
		// (get) Token: 0x060053E4 RID: 21476 RVA: 0x001AF36D File Offset: 0x001AD56D
		public static WorldReachability WorldReachability
		{
			get
			{
				return Find.World.reachability;
			}
		}

		// Token: 0x17000D20 RID: 3360
		// (get) Token: 0x060053E5 RID: 21477 RVA: 0x001AF379 File Offset: 0x001AD579
		public static WorldFeatures WorldFeatures
		{
			get
			{
				return Find.World.features;
			}
		}

		// Token: 0x17000D21 RID: 3361
		// (get) Token: 0x060053E6 RID: 21478 RVA: 0x001AF385 File Offset: 0x001AD585
		public static WorldComponent_GravshipController GravshipController
		{
			get
			{
				return Find.World.GetComponent<WorldComponent_GravshipController>();
			}
		}

		// Token: 0x17000D22 RID: 3362
		// (get) Token: 0x060053E7 RID: 21479 RVA: 0x001AF391 File Offset: 0x001AD591
		public static WorldInterface WorldInterface
		{
			get
			{
				return Find.World.UI;
			}
		}

		// Token: 0x17000D23 RID: 3363
		// (get) Token: 0x060053E8 RID: 21480 RVA: 0x001AF39D File Offset: 0x001AD59D
		public static WorldSelector WorldSelector
		{
			get
			{
				return Find.WorldInterface.selector;
			}
		}

		// Token: 0x17000D24 RID: 3364
		// (get) Token: 0x060053E9 RID: 21481 RVA: 0x001AF3A9 File Offset: 0x001AD5A9
		public static WorldTargeter WorldTargeter
		{
			get
			{
				return Find.WorldInterface.targeter;
			}
		}

		// Token: 0x17000D25 RID: 3365
		// (get) Token: 0x060053EA RID: 21482 RVA: 0x001AF3B5 File Offset: 0x001AD5B5
		public static WorldRoutePlanner WorldRoutePlanner
		{
			get
			{
				return Find.WorldInterface.routePlanner;
			}
		}

		// Token: 0x17000D26 RID: 3366
		// (get) Token: 0x060053EB RID: 21483 RVA: 0x001AF3C1 File Offset: 0x001AD5C1
		public static TilePicker TilePicker
		{
			get
			{
				return Find.WorldInterface.tilePicker;
			}
		}

		// Token: 0x17000D27 RID: 3367
		// (get) Token: 0x060053EC RID: 21484 RVA: 0x001AF3CD File Offset: 0x001AD5CD
		public static HistoryEventsManager HistoryEventsManager
		{
			get
			{
				return Find.History.historyEventsManager;
			}
		}

		// Token: 0x17000D28 RID: 3368
		// (get) Token: 0x060053ED RID: 21485 RVA: 0x001AF3D9 File Offset: 0x001AD5D9
		public static GoodwillSituationManager GoodwillSituationManager
		{
			get
			{
				return Find.FactionManager.goodwillSituationManager;
			}
		}

		// Token: 0x060053EE RID: 21486 RVA: 0x001AF3E5 File Offset: 0x001AD5E5
		public static void ClearCache()
		{
			Find.cachedAnomaly = null;
		}

		// Token: 0x04003C4D RID: 15437
		private static GameComponent_Anomaly cachedAnomaly;
	}
}
