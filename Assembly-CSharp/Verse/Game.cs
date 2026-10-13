using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using Verse.Profile;

namespace Verse
{
	// Token: 0x02000333 RID: 819
	public class Game : IExposable, IDisposable
	{
		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x0600149A RID: 5274 RVA: 0x00060B5F File Offset: 0x0005ED5F
		// (set) Token: 0x0600149B RID: 5275 RVA: 0x00060B67 File Offset: 0x0005ED67
		public Scenario Scenario
		{
			get
			{
				return this.scenarioInt;
			}
			set
			{
				this.scenarioInt = value;
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x0600149C RID: 5276 RVA: 0x00060B70 File Offset: 0x0005ED70
		// (set) Token: 0x0600149D RID: 5277 RVA: 0x00060B78 File Offset: 0x0005ED78
		public World World
		{
			get
			{
				return this.worldInt;
			}
			set
			{
				if (this.worldInt == value)
				{
					return;
				}
				this.worldInt = value;
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x0600149E RID: 5278 RVA: 0x00060B8B File Offset: 0x0005ED8B
		// (set) Token: 0x0600149F RID: 5279 RVA: 0x00060BAC File Offset: 0x0005EDAC
		public Map CurrentMap
		{
			get
			{
				if (this.currentMapIndex < 0)
				{
					return null;
				}
				return this.maps[(int)this.currentMapIndex];
			}
			set
			{
				int num;
				if (value == null)
				{
					num = -1;
				}
				else
				{
					num = this.maps.IndexOf(value);
					if (num < 0)
					{
						Log.Error("Could not set current map because it does not exist.");
						return;
					}
				}
				if ((int)this.currentMapIndex != num)
				{
					this.currentMapIndex = (sbyte)num;
					Find.MapUI.Notify_SwitchedMap();
					AmbientSoundManager.Notify_SwitchedMap();
				}
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x060014A0 RID: 5280 RVA: 0x00060BFC File Offset: 0x0005EDFC
		public Map AnyPlayerHomeMap
		{
			get
			{
				if (Faction.OfPlayerSilentFail == null)
				{
					return null;
				}
				for (int i = 0; i < this.maps.Count; i++)
				{
					if (this.maps[i].IsPlayerHome)
					{
						return this.maps[i];
					}
				}
				if (ModsConfig.OdysseyActive)
				{
					for (int j = 0; j < this.maps.Count; j++)
					{
						if (GravshipUtility.PlayerHasGravEngine(this.maps[j]))
						{
							return this.maps[j];
						}
					}
				}
				return null;
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x060014A1 RID: 5281 RVA: 0x00060C86 File Offset: 0x0005EE86
		public bool PlayerHasControl
		{
			get
			{
				return !ScreenFader.IsFading() && (!WorldComponent_GravshipController.CutsceneInProgress || Find.CameraDriver.config.gravshipFreeCam);
			}
		}

		// Token: 0x060014A2 RID: 5282 RVA: 0x00060CAC File Offset: 0x0005EEAC
		public bool IsPlayerTile(PlanetTile tile)
		{
			for (int i = 0; i < this.maps.Count; i++)
			{
				Map map = this.maps[i];
				if (map.Tile == tile && map.IsPlayerHome)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x060014A3 RID: 5283 RVA: 0x00060CF8 File Offset: 0x0005EEF8
		public IReadOnlyList<Map> PlayerHomeMaps
		{
			get
			{
				if (Faction.OfPlayerSilentFail == null)
				{
					return null;
				}
				Game.tmpPlayerHomeMaps.Clear();
				for (int i = 0; i < this.maps.Count; i++)
				{
					Map map = this.maps[i];
					if (map.IsPlayerHome)
					{
						Game.tmpPlayerHomeMaps.Add(map);
					}
				}
				return Game.tmpPlayerHomeMaps;
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x060014A4 RID: 5284 RVA: 0x00060D54 File Offset: 0x0005EF54
		public Map RandomPlayerHomeMap
		{
			get
			{
				if (Faction.OfPlayerSilentFail == null)
				{
					return null;
				}
				Game.tmpPlayerHomeMaps.Clear();
				for (int i = 0; i < this.maps.Count; i++)
				{
					Map map = this.maps[i];
					if (map.IsPlayerHome)
					{
						Game.tmpPlayerHomeMaps.Add(map);
					}
				}
				if (Game.tmpPlayerHomeMaps.Any<Map>())
				{
					Map result = Game.tmpPlayerHomeMaps.RandomElement<Map>();
					Game.tmpPlayerHomeMaps.Clear();
					return result;
				}
				return null;
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x060014A5 RID: 5285 RVA: 0x00060DCC File Offset: 0x0005EFCC
		public Map RandomRootSurfacePlayerHomeMap
		{
			get
			{
				if (Faction.OfPlayerSilentFail == null)
				{
					return null;
				}
				Game.tmpPlayerHomeMaps.Clear();
				for (int i = 0; i < this.maps.Count; i++)
				{
					Map map = this.maps[i];
					if (map.IsPlayerHome && map.Tile.Layer.IsRootSurface)
					{
						Game.tmpPlayerHomeMaps.Add(map);
					}
				}
				if (Game.tmpPlayerHomeMaps.Any<Map>())
				{
					Map result = Game.tmpPlayerHomeMaps.RandomElement<Map>();
					Game.tmpPlayerHomeMaps.Clear();
					return result;
				}
				return null;
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x060014A6 RID: 5286 RVA: 0x00060E5C File Offset: 0x0005F05C
		public Map RandomSurfacePlayerHomeMap
		{
			get
			{
				if (Faction.OfPlayerSilentFail == null)
				{
					return null;
				}
				Game.tmpPlayerHomeMaps.Clear();
				for (int i = 0; i < this.maps.Count; i++)
				{
					Map map = this.maps[i];
					if (map.IsPlayerHome && map.Tile.LayerDef.SurfaceTiles)
					{
						Game.tmpPlayerHomeMaps.Add(map);
					}
				}
				if (Game.tmpPlayerHomeMaps.Any<Map>())
				{
					Map result = Game.tmpPlayerHomeMaps.RandomElement<Map>();
					Game.tmpPlayerHomeMaps.Clear();
					return result;
				}
				return null;
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x060014A7 RID: 5287 RVA: 0x00060EE9 File Offset: 0x0005F0E9
		public List<Map> Maps
		{
			get
			{
				return this.maps;
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x060014A8 RID: 5288 RVA: 0x00060EF1 File Offset: 0x0005F0F1
		// (set) Token: 0x060014A9 RID: 5289 RVA: 0x00060EF9 File Offset: 0x0005F0F9
		public GameInitData InitData
		{
			get
			{
				return this.initData;
			}
			set
			{
				this.initData = value;
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x060014AA RID: 5290 RVA: 0x00060F02 File Offset: 0x0005F102
		public GameInfo Info
		{
			get
			{
				return this.info;
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x060014AB RID: 5291 RVA: 0x00060F0A File Offset: 0x0005F10A
		public GameRules Rules
		{
			get
			{
				return this.rules;
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x060014AC RID: 5292 RVA: 0x00060F12 File Offset: 0x0005F112
		// (set) Token: 0x060014AD RID: 5293 RVA: 0x00060F1A File Offset: 0x0005F11A
		public Gravship Gravship
		{
			get
			{
				return this.gravshipInt;
			}
			set
			{
				this.gravshipInt = value;
			}
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x00060F24 File Offset: 0x0005F124
		public Game()
		{
			this.FillComponents();
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x000610B0 File Offset: 0x0005F2B0
		public void AddMap(Map map)
		{
			if (map == null)
			{
				Log.Error("Tried to add null map.");
				return;
			}
			if (this.maps.Contains(map))
			{
				Log.Error("Tried to add map but it's already here.");
				return;
			}
			if (this.maps.Count > 127)
			{
				Log.Error("Can't add map. Reached maps count limit (" + sbyte.MaxValue.ToString() + ").");
				return;
			}
			this.maps.Add(map);
			Find.ColonistBar.MarkColonistsDirty();
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x00061128 File Offset: 0x0005F328
		public Map FindMap(MapParent mapParent)
		{
			for (int i = 0; i < this.maps.Count; i++)
			{
				if (this.maps[i].info.parent == mapParent)
				{
					return this.maps[i];
				}
			}
			return null;
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x00061174 File Offset: 0x0005F374
		public Map FindMap(PlanetTile tile)
		{
			for (int i = 0; i < this.maps.Count; i++)
			{
				if (this.maps[i].Tile == tile)
				{
					return this.maps[i];
				}
			}
			return null;
		}

		// Token: 0x060014B2 RID: 5298 RVA: 0x000611C0 File Offset: 0x0005F3C0
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Log.Error("You must use special LoadData method to load Game.");
				return;
			}
			Scribe_Values.Look<sbyte>(ref this.currentMapIndex, "currentMapIndex", -1, false);
			this.ExposeSmallComponents();
			Scribe_Deep.Look<World>(ref this.worldInt, "world", Array.Empty<object>());
			Scribe_Collections.Look<Map>(ref this.maps, "maps", LookMode.Deep, Array.Empty<object>());
			Find.CameraDriver.Expose();
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x00061230 File Offset: 0x0005F430
		private void ExposeSmallComponents()
		{
			Scribe_Deep.Look<GameInfo>(ref this.info, "info", Array.Empty<object>());
			Scribe_Deep.Look<GameRules>(ref this.rules, "rules", Array.Empty<object>());
			Scribe_Deep.Look<Scenario>(ref this.scenarioInt, "scenario", Array.Empty<object>());
			Scribe_Deep.Look<TickManager>(ref this.tickManager, "tickManager", Array.Empty<object>());
			Scribe_Deep.Look<PlaySettings>(ref this.playSettings, "playSettings", Array.Empty<object>());
			Scribe_Deep.Look<StoryWatcher>(ref this.storyWatcher, "storyWatcher", Array.Empty<object>());
			Scribe_Deep.Look<GameEnder>(ref this.gameEnder, "gameEnder", Array.Empty<object>());
			Scribe_Deep.Look<LetterStack>(ref this.letterStack, "letterStack", Array.Empty<object>());
			Scribe_Deep.Look<ResearchManager>(ref this.researchManager, "researchManager", Array.Empty<object>());
			Scribe_Deep.Look<AnalysisManager>(ref this.analysisManager, "analysisManager", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.analysisManager == null)
			{
				this.analysisManager = new AnalysisManager();
			}
			Scribe_Deep.Look<Storyteller>(ref this.storyteller, "storyteller", Array.Empty<object>());
			Scribe_Deep.Look<History>(ref this.history, "history", Array.Empty<object>());
			Scribe_Deep.Look<TaleManager>(ref this.taleManager, "taleManager", Array.Empty<object>());
			Scribe_Deep.Look<PlayLog>(ref this.playLog, "playLog", Array.Empty<object>());
			Scribe_Deep.Look<BattleLog>(ref this.battleLog, "battleLog", Array.Empty<object>());
			Scribe_Deep.Look<OutfitDatabase>(ref this.outfitDatabase, "outfitDatabase", Array.Empty<object>());
			Scribe_Deep.Look<DrugPolicyDatabase>(ref this.drugPolicyDatabase, "drugPolicyDatabase", Array.Empty<object>());
			Scribe_Deep.Look<FoodRestrictionDatabase>(ref this.foodRestrictionDatabase, "foodRestrictionDatabase", Array.Empty<object>());
			Scribe_Deep.Look<ReadingPolicyDatabase>(ref this.readingPolicyDatabase, "readingPolicyDatabase", Array.Empty<object>());
			Scribe_Deep.Look<Tutor>(ref this.tutor, "tutor", Array.Empty<object>());
			Scribe_Deep.Look<DateNotifier>(ref this.dateNotifier, "dateNotifier", Array.Empty<object>());
			Scribe_Deep.Look<UniqueIDsManager>(ref this.uniqueIDsManager, "uniqueIDsManager", Array.Empty<object>());
			Scribe_Deep.Look<QuestManager>(ref this.questManager, "questManager", Array.Empty<object>());
			Scribe_Deep.Look<TransportShipManager>(ref this.transportShipManager, "transportShipManager", Array.Empty<object>());
			Scribe_Deep.Look<StudyManager>(ref this.studyManager, "studyManager", Array.Empty<object>());
			Scribe_Deep.Look<CustomXenogermDatabase>(ref this.customXenogermDatabase, "customXenogermDatabase", Array.Empty<object>());
			Scribe_Deep.Look<CustomXenotypeDatabase>(ref this.customXenotypeDatabase, "customXenotypeDatabase", Array.Empty<object>());
			Scribe_Deep.Look<RelationshipRecords>(ref this.relationshipRecords, "relationshipRecords", Array.Empty<object>());
			Scribe_Deep.Look<HiddenItemsManager>(ref this.hiddenItemsManager, "hiddenItemsManager", Array.Empty<object>());
			Scribe_Deep.Look<EntityCodex>(ref this.entityCodex, "entityCodex", Array.Empty<object>());
			Scribe_Collections.Look<GameComponent>(ref this.components, "components", LookMode.Deep, new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.FillComponents();
				if (this.rules == null)
				{
					Log.Warning("Save game was missing rules. Replacing with a blank GameRules.");
					this.rules = new GameRules();
				}
				if (this.relationshipRecords == null)
				{
					this.relationshipRecords = new RelationshipRecords();
				}
				if (this.readingPolicyDatabase == null)
				{
					this.readingPolicyDatabase = new ReadingPolicyDatabase();
				}
				if (this.hiddenItemsManager == null)
				{
					this.hiddenItemsManager = new HiddenItemsManager();
				}
				if (this.entityCodex == null)
				{
					this.entityCodex = new EntityCodex();
				}
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x060014B4 RID: 5300 RVA: 0x00061568 File Offset: 0x0005F768
		private void FillComponents()
		{
			this.components.RemoveAll((GameComponent component) => component == null);
			foreach (Type type in typeof(GameComponent).AllSubclassesNonAbstract())
			{
				if (this.GetComponent(type) == null)
				{
					try
					{
						GameComponent item = (GameComponent)Activator.CreateInstance(type, new object[]
						{
							this
						});
						this.components.Add(item);
					}
					catch (Exception ex)
					{
						string str = "Could not instantiate a GameComponent of type ";
						Type type2 = type;
						string str2 = (type2 != null) ? type2.ToString() : null;
						string str3 = ": ";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
		}

		// Token: 0x060014B5 RID: 5301 RVA: 0x00061654 File Offset: 0x0005F854
		public void InitNewGame()
		{
			string str = (from mod in LoadedModManager.RunningMods
			select mod.PackageIdPlayerFacing + ((!mod.ModMetaData.VersionCompatible) ? " (incompatible version)" : "")).ToLineList("  - ", false);
			Log.Message("Initializing new game with mods:\n" + str);
			if (this.maps.Any<Map>())
			{
				Log.Error("Called InitNewGame() but there already is a map. There should be 0 maps...");
				return;
			}
			if (this.initData == null)
			{
				Log.Error("Called InitNewGame() but init data is null. Create it first.");
				return;
			}
			Game.ClearCaches();
			MemoryUtility.UnloadUnusedUnityAssets();
			try
			{
				Current.ProgramState = ProgramState.MapInitializing;
				IntVec3 intVec = new IntVec3(this.initData.mapSize, 1, this.initData.mapSize);
				Settlement settlement = null;
				List<Settlement> settlements = Find.WorldObjects.Settlements;
				for (int i = 0; i < settlements.Count; i++)
				{
					if (settlements[i].Faction == Faction.OfPlayer)
					{
						settlement = settlements[i];
						break;
					}
				}
				if (settlement == null)
				{
					Log.Error("Could not generate starting map because there is no any player faction base.");
				}
				this.tickManager.gameStartAbsTick = GenTicks.ConfiguredTicksAbsAtGameStart;
				this.info.startingTile = this.initData.startingTile;
				this.info.startingAndOptionalPawns = this.initData.startingAndOptionalPawns;
				Map currentMap = MapGenerator.GenerateMap(intVec, settlement, this.initData.mapGeneratorDef ?? settlement.MapGeneratorDef, settlement.ExtraGenStepDefs, null, false, false);
				this.worldInt.info.initialMapSize = intVec;
				if (this.initData.permadeath)
				{
					this.info.permadeathMode = true;
					this.info.permadeathModeUniqueName = PermadeathModeUtility.GeneratePermadeathSaveName();
				}
				PawnUtility.GiveAllStartingPlayerPawnsThought(ThoughtDefOf.NewColonyOptimism);
				this.FinalizeInit();
				Current.Game.CurrentMap = currentMap;
				Find.CameraDriver.JumpToCurrentMapLoc(MapGenerator.PlayerStartSpot);
				Find.CameraDriver.ResetSize();
				if (Prefs.PauseOnLoad && this.initData.startedFromEntry)
				{
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						this.tickManager.DoSingleTick();
						this.tickManager.CurTimeSpeed = TimeSpeed.Paused;
					});
				}
				Find.Scenario.PostGameStart();
				this.history.FinalizeInit();
				ResearchUtility.ApplyPlayerStartingResearch();
				GameComponentUtility.StartedNewGame();
				this.initData = null;
			}
			finally
			{
			}
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x0006188C File Offset: 0x0005FA8C
		public void LoadGame()
		{
			if (this.maps.Any<Map>())
			{
				Log.Error("Called LoadGame() but there already is a map. There should be 0 maps...");
				return;
			}
			Game.ClearCaches();
			MemoryUtility.UnloadUnusedUnityAssets();
			BackCompatibility.PreLoadSavegame(ScribeMetaHeaderUtility.loadedGameVersion);
			Current.ProgramState = ProgramState.MapInitializing;
			this.ExposeSmallComponents();
			LongEventHandler.SetCurrentEventText("LoadingWorld".Translate());
			if (Scribe.EnterNode("world"))
			{
				try
				{
					this.World = new World();
					this.World.ExposeData();
					goto IL_81;
				}
				finally
				{
					Scribe.ExitNode();
				}
				goto IL_76;
				IL_81:
				DeepProfiler.Start("World.FinalizeInit");
				this.World.FinalizeInit(true);
				DeepProfiler.End();
				LongEventHandler.SetCurrentEventText("LoadingMap".Translate());
				Scribe_Collections.Look<Map>(ref this.maps, "maps", LookMode.Deep, Array.Empty<object>());
				if (this.maps.RemoveAll((Map x) => x == null) != 0)
				{
					Log.Warning("Some maps were null after loading.");
				}
				int num = -1;
				Scribe_Values.Look<int>(ref num, "currentMapIndex", -1, false);
				if (num < 0 && this.maps.Any<Map>())
				{
					Log.Error("Current map is null after loading but there are maps available. Setting current map to [0].");
					num = 0;
				}
				if (num >= this.maps.Count)
				{
					Log.Error("Current map index out of bounds after loading.");
					if (this.maps.Any<Map>())
					{
						num = 0;
					}
					else
					{
						num = -1;
					}
				}
				this.currentMapIndex = sbyte.MinValue;
				this.CurrentMap = ((num >= 0) ? this.maps[num] : null);
				LongEventHandler.SetCurrentEventText("InitializingGame".Translate());
				Find.CameraDriver.Expose();
				DeepProfiler.Start("Scribe.loader.FinalizeLoading");
				Scribe.loader.FinalizeLoading();
				DeepProfiler.End();
				LongEventHandler.SetCurrentEventText("SpawningAllThings".Translate());
				DeepProfiler.Start("maps.FinalizeLoading");
				for (int i = 0; i < this.maps.Count; i++)
				{
					try
					{
						this.maps[i].FinalizeLoading();
					}
					catch (Exception ex)
					{
						string str = "Error in Map.FinalizeLoading(): ";
						Exception ex2 = ex;
						Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
					}
					try
					{
						MapParent parent = this.maps[i].Parent;
						if (parent != null)
						{
							parent.FinalizeLoading();
						}
					}
					catch (Exception ex3)
					{
						string str2 = "Error in MapParent.FinalizeLoading(): ";
						Exception ex4 = ex3;
						Log.Error(str2 + ((ex4 != null) ? ex4.ToString() : null));
					}
				}
				DeepProfiler.End();
				DeepProfiler.Start("Game.FinalizeInit");
				this.FinalizeInit();
				DeepProfiler.End();
				if (Prefs.PauseOnLoad)
				{
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						Find.TickManager.DoSingleTick();
						Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
					});
				}
				GameComponentUtility.LoadedGame();
				BackCompatibility.PostLoadSavegame(ScribeMetaHeaderUtility.loadedGameVersion);
				return;
			}
			IL_76:
			Log.Error("Could not find world XML node.");
		}

		// Token: 0x060014B7 RID: 5303 RVA: 0x00061B6C File Offset: 0x0005FD6C
		public void UpdateEntry()
		{
			GameComponentUtility.GameComponentUpdate();
		}

		// Token: 0x060014B8 RID: 5304 RVA: 0x00061B74 File Offset: 0x0005FD74
		public void UpdatePlay()
		{
			try
			{
				Find.LetterStack.OpenAutomaticLetters();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
			}
			this.tickManager.TickManagerUpdate();
			this.letterStack.LetterStackUpdate();
			this.World.WorldUpdate();
			for (int i = 0; i < this.maps.Count; i++)
			{
				this.maps[i].MapUpdate();
			}
			this.Info.GameInfoUpdate();
			GameComponentUtility.GameComponentUpdate();
			this.signalManager.SignalManagerUpdate();
			GlobalTextureAtlasManager.GlobalTextureAtlasManagerUpdate();
		}

		// Token: 0x060014B9 RID: 5305 RVA: 0x00061C14 File Offset: 0x0005FE14
		public T GetComponent<T>() where T : GameComponent
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				T t = this.components[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x00061C64 File Offset: 0x0005FE64
		public GameComponent GetComponent(Type type)
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				if (type.IsInstanceOfType(this.components[i]))
				{
					return this.components[i];
				}
			}
			return null;
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x00061CAC File Offset: 0x0005FEAC
		public void FinalizeInit()
		{
			LogSimple.FlushToFileAndOpen();
			this.researchManager.ReapplyAllMods();
			MessagesRepeatAvoider.Reset();
			GameComponentUtility.FinalizeInit();
			Current.ProgramState = ProgramState.Playing;
			Current.Game.World.ideoManager.Notify_GameStarted();
			RecipeDefGenerator.ResetRecipeIngredientsForDifficulty();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				DebugSettings.devPalette = Prefs.StartDevPaletteOn;
				Find.UIRoot.debugWindowOpener.TryOpenOrClosePalette();
			});
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x00061D18 File Offset: 0x0005FF18
		public void DeinitAndRemoveMap(Map map, bool notifyPlayer)
		{
			if (map == null)
			{
				Log.Error("Tried to remove null map.");
				return;
			}
			if (!this.maps.Contains(map))
			{
				Log.Error("Tried to remove map " + ((map != null) ? map.ToString() : null) + " but it's not here.");
				return;
			}
			if (map.Parent != null)
			{
				map.Parent.Notify_MyMapAboutToBeRemoved();
			}
			Map currentMap = this.CurrentMap;
			MapDeiniter.Deinit(map, notifyPlayer);
			this.maps.Remove(map);
			if (currentMap != null)
			{
				sbyte b = (sbyte)this.maps.IndexOf(currentMap);
				if (b < 0)
				{
					if (this.maps.Any<Map>())
					{
						this.CurrentMap = this.maps[0];
					}
					else
					{
						this.CurrentMap = null;
					}
					Find.World.renderer.wantedMode = WorldRenderMode.Planet;
				}
				else
				{
					this.currentMapIndex = b;
				}
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
			MapComponentUtility.MapRemoved(map);
			Find.Scenario.MapRemoved(map);
			if (map.Parent != null)
			{
				map.Parent.Notify_MyMapRemoved(map);
			}
			foreach (PocketMapParent pocketMapParent in Find.World.pocketMaps.ToList<PocketMapParent>())
			{
				if (pocketMapParent.sourceMap == map && pocketMapParent.Map.generatorDef.pocketMapProperties.destroyOnParentMapAbandoned)
				{
					PocketMapUtility.DestroyPocketMap(pocketMapParent.Map);
				}
			}
			map.Dispose();
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x00061E98 File Offset: 0x00060098
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Game debug data:");
			stringBuilder.AppendLine("initData:");
			if (this.initData == null)
			{
				stringBuilder.AppendLine("   null");
			}
			else
			{
				stringBuilder.AppendLine(this.initData.ToString());
			}
			stringBuilder.AppendLine("Scenario:");
			if (this.scenarioInt == null)
			{
				stringBuilder.AppendLine("   null");
			}
			else
			{
				StringBuilder stringBuilder2 = stringBuilder;
				string str = "   ";
				Scenario scenario = this.scenarioInt;
				stringBuilder2.AppendLine(str + ((scenario != null) ? scenario.ToString() : null));
			}
			stringBuilder.AppendLine("World:");
			if (this.worldInt == null)
			{
				stringBuilder.AppendLine("   null");
			}
			else
			{
				stringBuilder.AppendLine("   name: " + this.worldInt.info.name);
			}
			stringBuilder.AppendLine("Maps count: " + this.maps.Count.ToString());
			for (int i = 0; i < this.maps.Count; i++)
			{
				stringBuilder.AppendLine("   Map " + this.maps[i].Index.ToString() + ":");
				StringBuilder stringBuilder3 = stringBuilder;
				string str2 = "      tile: ";
				Tile tileInfo = this.maps[i].TileInfo;
				stringBuilder3.AppendLine(str2 + ((tileInfo != null) ? tileInfo.ToString() : null));
			}
			stringBuilder.AppendLine("Game components:");
			for (int j = 0; j < this.components.Count; j++)
			{
				this.components[j].AppendDebugString(stringBuilder);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x00062044 File Offset: 0x00060244
		public void Dispose()
		{
			for (int i = 0; i < this.maps.Count; i++)
			{
				this.maps[i].Dispose();
			}
			World world = this.worldInt;
			if (world != null)
			{
				world.Dispose();
			}
			SteadyEnvironmentEffects.Reset();
		}

		// Token: 0x060014BF RID: 5311 RVA: 0x00062090 File Offset: 0x00060290
		public static void ClearCaches()
		{
			Find.ClearCache();
			ChildcareUtility.ClearCache();
			SlaveRebellionUtility.ClearCache();
			Alert_NeedMeditationSpot.ClearCache();
			BuildCopyCommandUtility.ClearCache();
			MechanitorUtility.ClearCache();
			SocialCardUtility.ClearCaches();
			foreach (StatDef statDef in DefDatabase<StatDef>.AllDefsListForReading)
			{
				statDef.Worker.TryClearCache();
			}
		}

		// Token: 0x04001349 RID: 4937
		private GameInitData initData;

		// Token: 0x0400134A RID: 4938
		private Gravship gravshipInt;

		// Token: 0x0400134B RID: 4939
		public sbyte currentMapIndex = -1;

		// Token: 0x0400134C RID: 4940
		private GameInfo info = new GameInfo();

		// Token: 0x0400134D RID: 4941
		public List<GameComponent> components = new List<GameComponent>();

		// Token: 0x0400134E RID: 4942
		private GameRules rules = new GameRules();

		// Token: 0x0400134F RID: 4943
		private Scenario scenarioInt;

		// Token: 0x04001350 RID: 4944
		private World worldInt;

		// Token: 0x04001351 RID: 4945
		private List<Map> maps = new List<Map>();

		// Token: 0x04001352 RID: 4946
		public PlaySettings playSettings = new PlaySettings();

		// Token: 0x04001353 RID: 4947
		public StoryWatcher storyWatcher = new StoryWatcher();

		// Token: 0x04001354 RID: 4948
		public LetterStack letterStack = new LetterStack();

		// Token: 0x04001355 RID: 4949
		public ResearchManager researchManager = new ResearchManager();

		// Token: 0x04001356 RID: 4950
		public AnalysisManager analysisManager = new AnalysisManager();

		// Token: 0x04001357 RID: 4951
		public GameEnder gameEnder = new GameEnder();

		// Token: 0x04001358 RID: 4952
		public Storyteller storyteller = new Storyteller();

		// Token: 0x04001359 RID: 4953
		public History history = new History();

		// Token: 0x0400135A RID: 4954
		public TaleManager taleManager = new TaleManager();

		// Token: 0x0400135B RID: 4955
		public PlayLog playLog = new PlayLog();

		// Token: 0x0400135C RID: 4956
		public BattleLog battleLog = new BattleLog();

		// Token: 0x0400135D RID: 4957
		public OutfitDatabase outfitDatabase = new OutfitDatabase();

		// Token: 0x0400135E RID: 4958
		public DrugPolicyDatabase drugPolicyDatabase = new DrugPolicyDatabase();

		// Token: 0x0400135F RID: 4959
		public ReadingPolicyDatabase readingPolicyDatabase = new ReadingPolicyDatabase();

		// Token: 0x04001360 RID: 4960
		public FoodRestrictionDatabase foodRestrictionDatabase = new FoodRestrictionDatabase();

		// Token: 0x04001361 RID: 4961
		public TickManager tickManager = new TickManager();

		// Token: 0x04001362 RID: 4962
		public Tutor tutor = new Tutor();

		// Token: 0x04001363 RID: 4963
		public Autosaver autosaver = new Autosaver();

		// Token: 0x04001364 RID: 4964
		public DateNotifier dateNotifier = new DateNotifier();

		// Token: 0x04001365 RID: 4965
		public SignalManager signalManager = new SignalManager();

		// Token: 0x04001366 RID: 4966
		public UniqueIDsManager uniqueIDsManager = new UniqueIDsManager();

		// Token: 0x04001367 RID: 4967
		public QuestManager questManager = new QuestManager();

		// Token: 0x04001368 RID: 4968
		public TransportShipManager transportShipManager = new TransportShipManager();

		// Token: 0x04001369 RID: 4969
		public StudyManager studyManager = new StudyManager();

		// Token: 0x0400136A RID: 4970
		public CustomXenogermDatabase customXenogermDatabase = new CustomXenogermDatabase();

		// Token: 0x0400136B RID: 4971
		public CustomXenotypeDatabase customXenotypeDatabase = new CustomXenotypeDatabase();

		// Token: 0x0400136C RID: 4972
		public RelationshipRecords relationshipRecords = new RelationshipRecords();

		// Token: 0x0400136D RID: 4973
		public HiddenItemsManager hiddenItemsManager = new HiddenItemsManager();

		// Token: 0x0400136E RID: 4974
		public EntityCodex entityCodex = new EntityCodex();

		// Token: 0x0400136F RID: 4975
		private static readonly List<Map> tmpPlayerHomeMaps = new List<Map>();
	}
}
