using System;
using System.Collections.Generic;
using System.Linq;
using LudeonTK;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000457 RID: 1111
	public sealed class Map : IIncidentTarget, ILoadReferenceable, IThingHolder, IExposable, IDisposable
	{
		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06001CEB RID: 7403 RVA: 0x0009195F File Offset: 0x0008FB5F
		public int Index
		{
			get
			{
				return Find.Maps.IndexOf(this);
			}
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06001CEC RID: 7404 RVA: 0x0009196C File Offset: 0x0008FB6C
		public IntVec3 Size
		{
			get
			{
				return this.info.Size;
			}
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06001CED RID: 7405 RVA: 0x00091979 File Offset: 0x0008FB79
		public IntVec3 Center
		{
			get
			{
				return new IntVec3(this.Size.x / 2, 0, this.Size.z / 2);
			}
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06001CEE RID: 7406 RVA: 0x0009199B File Offset: 0x0008FB9B
		public Faction ParentFaction
		{
			get
			{
				MapParent parent = this.info.parent;
				if (parent == null)
				{
					return null;
				}
				return parent.Faction;
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06001CEF RID: 7407 RVA: 0x000919B3 File Offset: 0x0008FBB3
		public int Area
		{
			get
			{
				return this.Size.x * this.Size.z;
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06001CF0 RID: 7408 RVA: 0x000919CC File Offset: 0x0008FBCC
		public IThingHolder ParentHolder
		{
			get
			{
				return this.info.parent;
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06001CF1 RID: 7409 RVA: 0x000919D9 File Offset: 0x0008FBD9
		public bool DrawMapClippers
		{
			get
			{
				return !this.generatorDef.disableMapClippers;
			}
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06001CF2 RID: 7410 RVA: 0x000919E9 File Offset: 0x0008FBE9
		public bool CanEverExit
		{
			get
			{
				return !this.info.isPocketMap && this.Biome.canExitMap;
			}
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06001CF3 RID: 7411 RVA: 0x00091A08 File Offset: 0x0008FC08
		// (set) Token: 0x06001CF4 RID: 7412 RVA: 0x00091A32 File Offset: 0x0008FC32
		public Color? FogOfWarColor
		{
			get
			{
				Color? result = this.fogOfWarColor;
				if (result == null)
				{
					return this.Biome.fogOfWarColor;
				}
				return result;
			}
			set
			{
				this.fogOfWarColor = value;
			}
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06001CF5 RID: 7413 RVA: 0x00091A3B File Offset: 0x0008FC3B
		// (set) Token: 0x06001CF6 RID: 7414 RVA: 0x00091A52 File Offset: 0x0008FC52
		public OrbitalDebrisDef OrbitalDebris
		{
			get
			{
				return this.orbitalDebris ?? this.Biome.orbitalDebris;
			}
			set
			{
				this.orbitalDebris = value;
			}
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06001CF7 RID: 7415 RVA: 0x00091A5C File Offset: 0x0008FC5C
		public Material MapEdgeMaterial
		{
			get
			{
				if (ModsConfig.AnomalyActive && this.generatorDef == MapGeneratorDefOf.MetalHell)
				{
					return MapEdgeClipDrawer.ClipMatMetalhell;
				}
				WorldObject parent = this.Parent;
				if (parent != null && parent.def.MapEdgeMaterial != null)
				{
					return parent.def.MapEdgeMaterial;
				}
				return MapEdgeClipDrawer.ClipMat;
			}
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06001CF8 RID: 7416 RVA: 0x00091AB1 File Offset: 0x0008FCB1
		// (set) Token: 0x06001CF9 RID: 7417 RVA: 0x00091AB9 File Offset: 0x0008FCB9
		public bool Disposed { get; private set; }

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06001CFA RID: 7418 RVA: 0x00091AC2 File Offset: 0x0008FCC2
		public IEnumerable<IntVec3> AllCells
		{
			get
			{
				int num;
				for (int z = 0; z < this.Size.z; z = num + 1)
				{
					for (int y = 0; y < this.Size.y; y = num + 1)
					{
						for (int x = 0; x < this.Size.x; x = num + 1)
						{
							yield return new IntVec3(x, y, z);
							num = x;
						}
						num = y;
					}
					num = z;
				}
				yield break;
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06001CFB RID: 7419 RVA: 0x00091AD4 File Offset: 0x0008FCD4
		public bool IsPlayerHome
		{
			get
			{
				if (!this.wasSpawnedViaGravShipLanding)
				{
					MapInfo mapInfo = this.info;
					return ((mapInfo != null) ? mapInfo.parent : null) != null && this.info.parent.Faction == Faction.OfPlayer && this.info.parent.def.canBePlayerHome;
				}
				return true;
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06001CFC RID: 7420 RVA: 0x00091B2D File Offset: 0x0008FD2D
		public bool TreatAsPlayerHomeForThreatPoints
		{
			get
			{
				return this.IsPlayerHome || (this.info.parent != null && this.info.parent.def.treatAsPlayerHome);
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06001CFD RID: 7421 RVA: 0x00091B60 File Offset: 0x0008FD60
		public bool IsTempIncidentMap
		{
			get
			{
				return this.info.parent.def.isTempIncidentMapOwner;
			}
		}

		// Token: 0x06001CFE RID: 7422 RVA: 0x00091B77 File Offset: 0x0008FD77
		public IEnumerator<IntVec3> GetEnumerator()
		{
			foreach (IntVec3 intVec in this.AllCells)
			{
				yield return intVec;
			}
			IEnumerator<IntVec3> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06001CFF RID: 7423 RVA: 0x00091B86 File Offset: 0x0008FD86
		public PlanetTile Tile
		{
			get
			{
				return this.info.Tile;
			}
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06001D00 RID: 7424 RVA: 0x00091B93 File Offset: 0x0008FD93
		public Tile TileInfo
		{
			get
			{
				if (!this.IsPocketMap)
				{
					return Find.WorldGrid[this.Tile];
				}
				return this.pocketTileInfo;
			}
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06001D01 RID: 7425 RVA: 0x00091BB4 File Offset: 0x0008FDB4
		public BiomeDef Biome
		{
			get
			{
				return this.TileInfo.PrimaryBiome;
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06001D02 RID: 7426 RVA: 0x00091BC1 File Offset: 0x0008FDC1
		public IEnumerable<BiomeDef> Biomes
		{
			get
			{
				return this.TileInfo.Biomes;
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06001D03 RID: 7427 RVA: 0x00091BD0 File Offset: 0x0008FDD0
		public MixedBiomeMapComponent MixedBiomeComp
		{
			get
			{
				MixedBiomeMapComponent result;
				if ((result = this.mixedBiomeComp) == null)
				{
					result = (this.mixedBiomeComp = this.GetComponent<MixedBiomeMapComponent>());
				}
				return result;
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06001D04 RID: 7428 RVA: 0x00091BF6 File Offset: 0x0008FDF6
		public bool IsStartingMap
		{
			get
			{
				return Find.GameInfo.startingTile == this.Tile;
			}
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06001D05 RID: 7429 RVA: 0x00091C0D File Offset: 0x0008FE0D
		public bool IsPocketMap
		{
			get
			{
				return this.info.isPocketMap;
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06001D06 RID: 7430 RVA: 0x00091C1A File Offset: 0x0008FE1A
		public StoryState StoryState
		{
			get
			{
				return this.storyState;
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06001D07 RID: 7431 RVA: 0x00091C22 File Offset: 0x0008FE22
		public GameConditionManager GameConditionManager
		{
			get
			{
				return this.gameConditionManager;
			}
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06001D08 RID: 7432 RVA: 0x00091C2C File Offset: 0x0008FE2C
		public float PlayerWealthForStoryteller
		{
			get
			{
				if (!this.TreatAsPlayerHomeForThreatPoints)
				{
					float num = 0f;
					foreach (Pawn pawn in this.mapPawns.PawnsInFaction(Faction.OfPlayer))
					{
						if (pawn.IsFreeColonist)
						{
							num += WealthWatcher.GetEquipmentApparelAndInventoryWealth(pawn);
						}
						if (pawn.IsAnimal)
						{
							num += pawn.MarketValue;
						}
					}
					return num;
				}
				if (Find.Storyteller.difficulty.fixedWealthMode)
				{
					return StorytellerUtility.FixedWealthModeMapWealthFromTimeCurve.Evaluate(this.AgeInDays * Find.Storyteller.difficulty.fixedWealthTimeFactor);
				}
				return this.wealthWatcher.WealthItems + this.wealthWatcher.WealthBuildings * 0.5f + this.wealthWatcher.WealthPawns;
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06001D09 RID: 7433 RVA: 0x00091D10 File Offset: 0x0008FF10
		public IEnumerable<Pawn> PlayerPawnsForStoryteller
		{
			get
			{
				return this.mapPawns.PawnsInFaction(Faction.OfPlayer);
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06001D0A RID: 7434 RVA: 0x00091D22 File Offset: 0x0008FF22
		public FloatRange IncidentPointsRandomFactorRange
		{
			get
			{
				return FloatRange.One;
			}
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06001D0B RID: 7435 RVA: 0x000919CC File Offset: 0x0008FBCC
		public MapParent Parent
		{
			get
			{
				return this.info.parent;
			}
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06001D0C RID: 7436 RVA: 0x00091D29 File Offset: 0x0008FF29
		public PocketMapParent PocketMapParent
		{
			get
			{
				if (!this.IsPocketMap)
				{
					return null;
				}
				return this.Parent as PocketMapParent;
			}
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06001D0D RID: 7437 RVA: 0x00091D40 File Offset: 0x0008FF40
		public IEnumerable<Map> ChildPocketMaps
		{
			get
			{
				foreach (PocketMapParent pocketMapParent in Find.World.pocketMaps)
				{
					if (pocketMapParent.sourceMap == this)
					{
						yield return pocketMapParent.Map;
					}
				}
				List<PocketMapParent>.Enumerator enumerator = default(List<PocketMapParent>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06001D0E RID: 7438 RVA: 0x00091D50 File Offset: 0x0008FF50
		public float AgeInDays
		{
			get
			{
				return (float)(Find.TickManager.TicksGame - this.generationTick) / 60000f;
			}
		}

		// Token: 0x06001D0F RID: 7439 RVA: 0x00091D6A File Offset: 0x0008FF6A
		public IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			MapParent parent = this.info.parent;
			return ((parent != null) ? parent.IncidentTargetTags() : null) ?? Enumerable.Empty<IncidentTargetTagDef>();
		}

		// Token: 0x06001D10 RID: 7440 RVA: 0x00091D8C File Offset: 0x0008FF8C
		public void ConstructComponents()
		{
			this.spawnedThings = new ThingOwner<Thing>(this);
			this.cellIndices = new CellIndices(this);
			this.listerThings = new ListerThings(ListerThingsUse.Global, this.thingListChangedCallbacks);
			this.listerBuildings = new ListerBuildings();
			this.mapPawns = new MapPawns(this);
			this.dynamicDrawManager = new DynamicDrawManager(this);
			this.mapDrawer = new MapDrawer(this);
			this.tooltipGiverList = new TooltipGiverList();
			this.pawnDestinationReservationManager = new PawnDestinationReservationManager();
			this.reservationManager = new ReservationManager(this);
			this.enrouteManager = new EnrouteManager(this);
			this.physicalInteractionReservationManager = new PhysicalInteractionReservationManager();
			this.designationManager = new DesignationManager(this);
			this.lordManager = new LordManager(this);
			this.debugDrawer = new DebugCellDrawer();
			this.passingShipManager = new PassingShipManager(this);
			this.haulDestinationManager = new HaulDestinationManager(this);
			this.gameConditionManager = new GameConditionManager(this);
			this.weatherManager = new WeatherManager(this);
			this.zoneManager = new ZoneManager(this);
			this.planManager = new PlanManager(this);
			this.resourceCounter = new ResourceCounter(this);
			this.mapTemperature = new MapTemperature(this);
			this.TemperatureVacuumCache = new TemperatureVacuumCache(this);
			this.areaManager = new AreaManager(this);
			this.attackTargetsCache = new AttackTargetsCache(this);
			this.attackTargetReservationManager = new AttackTargetReservationManager(this);
			this.lordsStarter = new VoluntarilyJoinableLordsStarter(this);
			this.flecks = new FleckManager(this);
			this.deferredSpawner = new DeferredSpawner(this);
			this.thingGrid = new ThingGrid(this);
			this.coverGrid = new CoverGrid(this);
			this.edificeGrid = new EdificeGrid(this);
			this.blueprintGrid = new BlueprintGrid(this);
			this.fogGrid = new FogGrid(this);
			this.glowGrid = new GlowGrid(this);
			this.regionGrid = new RegionGrid(this);
			this.terrainGrid = new TerrainGrid(this);
			this.pathing = new Pathing(this);
			this.roofGrid = new RoofGrid(this);
			this.fertilityGrid = new FertilityGrid(this);
			this.snowGrid = new SnowGrid(this);
			this.gasGrid = new GasGrid(this);
			this.pollutionGrid = new PollutionGrid(this);
			this.deepResourceGrid = new DeepResourceGrid(this);
			this.exitMapGrid = new ExitMapGrid(this);
			this.avoidGrid = new AvoidGrid(this);
			this.linkGrid = new LinkGrid(this);
			this.powerNetManager = new PowerNetManager(this);
			this.powerNetGrid = new PowerNetGrid(this);
			this.regionMaker = new RegionMaker(this);
			this.pathFinder = new PathFinder(this);
			this.pawnPathPool = new PawnPathPool(this);
			this.regionAndRoomUpdater = new RegionAndRoomUpdater(this);
			this.regionLinkDatabase = new RegionLinkDatabase();
			this.moteCounter = new MoteCounter();
			this.gatherSpotLister = new GatherSpotLister();
			this.windManager = new WindManager(this);
			this.listerBuildingsRepairable = new ListerBuildingsRepairable();
			this.listerHaulables = new ListerHaulables(this);
			this.listerMergeables = new ListerMergeables(this);
			this.listerFilthInHomeArea = new ListerFilthInHomeArea(this);
			this.listerArtificialBuildingsForMeditation = new ListerArtificialBuildingsForMeditation(this);
			this.listerBuldingOfDefInProximity = new ListerBuldingOfDefInProximity(this);
			this.listerBuildingWithTagInProximity = new ListerBuildingWithTagInProximity(this);
			this.reachability = new Reachability(this);
			this.itemAvailability = new ItemAvailability(this);
			this.autoBuildRoofAreaSetter = new AutoBuildRoofAreaSetter(this);
			this.roofCollapseBufferResolver = new RoofCollapseBufferResolver(this);
			this.roofCollapseBuffer = new RoofCollapseBuffer();
			this.wildAnimalSpawner = new WildAnimalSpawner(this);
			this.wildPlantSpawner = new WildPlantSpawner(this);
			this.steadyEnvironmentEffects = new SteadyEnvironmentEffects(this);
			this.tempTerrain = new TempTerrainManager(this);
			this.skyManager = new SkyManager(this);
			this.overlayDrawer = new OverlayDrawer();
			this.floodFiller = new FloodFiller(this);
			this.weatherDecider = new WeatherDecider(this);
			this.fireWatcher = new FireWatcher(this);
			this.dangerWatcher = new DangerWatcher(this);
			this.damageWatcher = new DamageWatcher();
			this.strengthWatcher = new StrengthWatcher(this);
			this.wealthWatcher = new WealthWatcher(this);
			this.regionDirtyer = new RegionDirtyer(this);
			this.cellsInRandomOrder = new MapCellsInRandomOrder(this);
			this.rememberedCameraPos = new RememberedCameraPos(this);
			this.mineStrikeManager = new MineStrikeManager();
			this.storyState = new StoryState(this);
			this.retainedCaravanData = new RetainedCaravanData(this);
			this.temporaryThingDrawer = new TemporaryThingDrawer();
			this.animalPenManager = new AnimalPenManager(this);
			this.plantGrowthRateCalculator = new MapPlantGrowthRateCalculator();
			this.autoSlaughterManager = new AutoSlaughterManager(this);
			this.treeDestructionTracker = new TreeDestructionTracker(this);
			this.storageGroups = new StorageGroupManager(this);
			this.effecterMaintainer = new EffecterMaintainer(this);
			this.postTickVisuals = new PostTickVisuals(this);
			if (ModsConfig.OdysseyActive)
			{
				this.substructureGrid = new SubstructureGrid(this);
				this.waterBodyTracker = new WaterBodyTracker(this);
				this.freezeManager = new FreezeManager(this);
				this.sandGrid = new SandGrid(this);
			}
			this.components.Clear();
			this.FillComponents();
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06001D11 RID: 7441 RVA: 0x00092264 File Offset: 0x00090464
		public int NextGenSeed
		{
			get
			{
				int value = this.TileInfo.tile.Valid ? this.TileInfo.tile.GetHashCode() : this.uniqueID;
				int num = this.generatedId;
				this.generatedId = num + 1;
				return HashCode.Combine<int, int, int>(value, num, Find.World.info.Seed);
			}
		}

		// Token: 0x06001D12 RID: 7442 RVA: 0x000922C8 File Offset: 0x000904C8
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.events = new MapEvents(this);
			}
			Scribe_Values.Look<int>(ref this.uniqueID, "uniqueID", -1, false);
			Scribe_Values.Look<int>(ref this.generationTick, "generationTick", 0, false);
			Scribe_Values.Look<bool>(ref this.wasSpawnedViaGravShipLanding, "wasSpawnedViaGravShipLanding", false, false);
			Scribe_Values.Look<Color?>(ref this.fogOfWarColor, "fogOfWarColor", null, false);
			Scribe_Values.Look<int>(ref this.generatedId, "generatedId", 0, false);
			Scribe_Defs.Look<OrbitalDebrisDef>(ref this.orbitalDebris, "orbitalDebris");
			Scribe_Defs.Look<MapGeneratorDef>(ref this.generatorDef, "generatorDef");
			Scribe_Deep.Look<Tile>(ref this.pocketTileInfo, "pocketTileInfo", Array.Empty<object>());
			Scribe_Deep.Look<MapInfo>(ref this.info, "mapInfo", Array.Empty<object>());
			Scribe_Collections.Look<LayoutStructureSketch>(ref this.layoutStructureSketches, "layoutStructureSketches", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<CellRect>(ref this.landingBlockers, "landingBlockers", LookMode.Undefined, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.compressor = new MapFileCompressor(this);
				this.compressor.BuildCompressedString();
				this.ExposeComponents();
				this.compressor.ExposeData();
				HashSet<string> hashSet = new HashSet<string>();
				if (Scribe.EnterNode("things"))
				{
					try
					{
						using (List<Thing>.Enumerator enumerator = this.listerThings.AllThings.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Thing thing = enumerator.Current;
								try
								{
									if (thing.def.isSaveable && !thing.IsSaveCompressible())
									{
										if (!hashSet.Add(thing.ThingID))
										{
											Log.Error("Saving Thing with already-used ID " + thing.ThingID);
										}
										else
										{
											hashSet.Add(thing.ThingID);
										}
										Thing thing2 = thing;
										Scribe_Deep.Look<Thing>(ref thing2, "thing", Array.Empty<object>());
									}
								}
								catch (OutOfMemoryException)
								{
									throw;
								}
								catch (Exception arg)
								{
									Log.Error(string.Format("Exception saving {0}: {1}", thing, arg));
								}
							}
							goto IL_1F1;
						}
					}
					finally
					{
						Scribe.ExitNode();
					}
				}
				Log.Error("Could not enter the things node while saving.");
				IL_1F1:
				this.compressor = null;
			}
			else
			{
				if (Scribe.mode == LoadSaveMode.LoadingVars)
				{
					this.ConstructComponents();
					this.regionAndRoomUpdater.Enabled = false;
					this.compressor = new MapFileCompressor(this);
				}
				else if (Scribe.mode == LoadSaveMode.PostLoadInit && this.landingBlockers == null)
				{
					this.landingBlockers = new List<CellRect>();
				}
				this.ExposeComponents();
				DeepProfiler.Start("Load compressed things");
				this.compressor.ExposeData();
				DeepProfiler.End();
				DeepProfiler.Start("Load non-compressed things");
				Scribe_Collections.Look<Thing>(ref this.loadedFullThings, "things", LookMode.Deep, Array.Empty<object>());
				DeepProfiler.End();
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06001D13 RID: 7443 RVA: 0x00092594 File Offset: 0x00090794
		private void FillComponents()
		{
			this.components.RemoveAll((MapComponent component) => component == null);
			foreach (Type type in typeof(MapComponent).AllSubclassesNonAbstract())
			{
				if (!typeof(CustomMapComponent).IsAssignableFrom(type) && this.GetComponent(type) == null)
				{
					try
					{
						MapComponent item = (MapComponent)Activator.CreateInstance(type, new object[]
						{
							this
						});
						this.components.Add(item);
					}
					catch (Exception ex)
					{
						string str = "Could not instantiate a MapComponent of type ";
						Type type2 = type;
						string str2 = (type2 != null) ? type2.ToString() : null;
						string str3 = ": ";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
			MapGeneratorDef mapGeneratorDef = this.generatorDef;
			if (((mapGeneratorDef != null) ? mapGeneratorDef.customMapComponents : null) != null)
			{
				foreach (Type type3 in this.generatorDef.customMapComponents)
				{
					if (this.GetComponent(type3) == null)
					{
						try
						{
							MapComponent item2 = (MapComponent)Activator.CreateInstance(type3, new object[]
							{
								this
							});
							this.components.Add(item2);
						}
						catch (Exception ex3)
						{
							string str4 = "Could not instantiate a MapComponent of type ";
							Type type4 = type3;
							string str5 = (type4 != null) ? type4.ToString() : null;
							string str6 = ": ";
							Exception ex4 = ex3;
							Log.Error(str4 + str5 + str6 + ((ex4 != null) ? ex4.ToString() : null));
						}
					}
				}
			}
			this.roadInfo = this.GetComponent<RoadInfo>();
			this.waterInfo = this.GetComponent<WaterInfo>();
		}

		// Token: 0x06001D14 RID: 7444 RVA: 0x00092774 File Offset: 0x00090974
		public void FinalizeLoading()
		{
			this.regionAndRoomUpdater.Enabled = true;
			List<Thing> list = this.compressor.ThingsToSpawnAfterLoad().ToList<Thing>();
			this.compressor = null;
			DeepProfiler.Start("Merge compressed and non-compressed thing lists");
			List<Thing> list2 = new List<Thing>(this.loadedFullThings.Count + list.Count);
			foreach (Thing item in this.loadedFullThings.Concat(list))
			{
				list2.Add(item);
			}
			this.loadedFullThings.Clear();
			DeepProfiler.End();
			DeepProfiler.Start("Spawn everything into the map");
			BackCompatibility.PreCheckSpawnBackCompatibleThingAfterLoading(this);
			foreach (Thing thing in list2)
			{
				if (!(thing is Building))
				{
					try
					{
						if (!BackCompatibility.CheckSpawnBackCompatibleThingAfterLoading(thing, this))
						{
							GenSpawn.Spawn(thing, thing.Position, this, thing.Rotation, WipeMode.FullRefund, true, false);
						}
					}
					catch (Exception ex)
					{
						string str = "Exception spawning loaded thing ";
						string str2 = thing.ToStringSafe<Thing>();
						string str3 = ": ";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
			foreach (Building building in from t in list2.OfType<Building>()
			orderby t.def.size.Magnitude
			select t)
			{
				try
				{
					GenSpawn.SpawnBuildingAsPossible(building, this, true);
				}
				catch (Exception ex3)
				{
					string str4 = "Exception spawning loaded thing ";
					string str5 = building.ToStringSafe<Building>();
					string str6 = ": ";
					Exception ex4 = ex3;
					Log.Error(str4 + str5 + str6 + ((ex4 != null) ? ex4.ToString() : null));
				}
			}
			BackCompatibility.PostCheckSpawnBackCompatibleThingAfterLoading(this);
			DeepProfiler.End();
			this.FinalizeInit();
		}

		// Token: 0x06001D15 RID: 7445 RVA: 0x00092984 File Offset: 0x00090B84
		public void FinalizeInit()
		{
			DeepProfiler.Start("Finalize geometry");
			this.pathing.RecalculateAllPerceivedPathCosts();
			this.regionAndRoomUpdater.Enabled = true;
			this.regionAndRoomUpdater.RebuildAllRegionsAndRooms();
			this.powerNetManager.UpdatePowerNetsAndConnections_First();
			this.TemperatureVacuumCache.TemperatureVacuumSaveLoad.ApplyLoadedDataToRegions();
			this.avoidGrid.Regenerate();
			this.animalPenManager.RebuildAllPens();
			this.plantGrowthRateCalculator.BuildFor(this);
			this.gasGrid.RecalculateEverHadGas();
			DeepProfiler.End();
			DeepProfiler.Start("Thing.PostMapInit()");
			foreach (Thing thing in this.listerThings.AllThings.ToList<Thing>())
			{
				try
				{
					thing.PostMapInit();
				}
				catch (Exception ex)
				{
					string str = "Error in PostMapInit() for ";
					string str2 = thing.ToStringSafe<Thing>();
					string str3 = ": ";
					Exception ex2 = ex;
					Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			DeepProfiler.End();
			DeepProfiler.Start("listerFilthInHomeArea.RebuildAll()");
			this.listerFilthInHomeArea.RebuildAll();
			DeepProfiler.End();
			if (ModsConfig.OdysseyActive)
			{
				this.GetComponent<VacuumComponent>().SetDrawerDirty();
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.mapDrawer.RegenerateEverythingNow();
			});
			DeepProfiler.Start("resourceCounter.UpdateResourceCounts()");
			this.resourceCounter.UpdateResourceCounts();
			DeepProfiler.End();
			DeepProfiler.Start("wealthWatcher.ForceRecount()");
			this.wealthWatcher.ForceRecount(true);
			DeepProfiler.End();
			if (ModsConfig.OdysseyActive)
			{
				using (new ProfilerBlock("WaterBodyTracker.ConstructBodies()"))
				{
					WaterBodyTracker waterBodyTracker = this.waterBodyTracker;
					if (waterBodyTracker != null)
					{
						waterBodyTracker.ConstructBodies();
					}
				}
			}
			MapComponentUtility.FinalizeInit(this);
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				Find.MusicManagerPlay.CheckTransitions();
			});
		}

		// Token: 0x06001D16 RID: 7446 RVA: 0x00092B7C File Offset: 0x00090D7C
		private void ExposeComponents()
		{
			Scribe_Deep.Look<WeatherManager>(ref this.weatherManager, "weatherManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<ReservationManager>(ref this.reservationManager, "reservationManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<EnrouteManager>(ref this.enrouteManager, "enrouteManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<PhysicalInteractionReservationManager>(ref this.physicalInteractionReservationManager, "physicalInteractionReservationManager", Array.Empty<object>());
			Scribe_Deep.Look<PlanManager>(ref this.planManager, "planManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<DesignationManager>(ref this.designationManager, "designationManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<PawnDestinationReservationManager>(ref this.pawnDestinationReservationManager, "pawnDestinationReservationManager", Array.Empty<object>());
			Scribe_Deep.Look<LordManager>(ref this.lordManager, "lordManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<PassingShipManager>(ref this.passingShipManager, "visitorManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<GameConditionManager>(ref this.gameConditionManager, "gameConditionManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<FogGrid>(ref this.fogGrid, "fogGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<RoofGrid>(ref this.roofGrid, "roofGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<TerrainGrid>(ref this.terrainGrid, "terrainGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<ZoneManager>(ref this.zoneManager, "zoneManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<TemperatureVacuumCache>(ref this.TemperatureVacuumCache, "temperatureCache", new object[]
			{
				this
			});
			Scribe_Deep.Look<SnowGrid>(ref this.snowGrid, "snowGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<GasGrid>(ref this.gasGrid, "gasGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<PollutionGrid>(ref this.pollutionGrid, "pollutionGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<WaterBodyTracker>(ref this.waterBodyTracker, "waterBodyTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<AreaManager>(ref this.areaManager, "areaManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<VoluntarilyJoinableLordsStarter>(ref this.lordsStarter, "lordsStarter", new object[]
			{
				this
			});
			Scribe_Deep.Look<AttackTargetReservationManager>(ref this.attackTargetReservationManager, "attackTargetReservationManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<DeepResourceGrid>(ref this.deepResourceGrid, "deepResourceGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<WeatherDecider>(ref this.weatherDecider, "weatherDecider", new object[]
			{
				this
			});
			Scribe_Deep.Look<DamageWatcher>(ref this.damageWatcher, "damageWatcher", Array.Empty<object>());
			Scribe_Deep.Look<RememberedCameraPos>(ref this.rememberedCameraPos, "rememberedCameraPos", new object[]
			{
				this
			});
			Scribe_Deep.Look<MineStrikeManager>(ref this.mineStrikeManager, "mineStrikeManager", Array.Empty<object>());
			Scribe_Deep.Look<RetainedCaravanData>(ref this.retainedCaravanData, "retainedCaravanData", new object[]
			{
				this
			});
			Scribe_Deep.Look<StoryState>(ref this.storyState, "storyState", new object[]
			{
				this
			});
			Scribe_Deep.Look<TempTerrainManager>(ref this.tempTerrain, "tempTerrain", new object[]
			{
				this
			});
			Scribe_Deep.Look<WildPlantSpawner>(ref this.wildPlantSpawner, "wildPlantSpawner", new object[]
			{
				this
			});
			Scribe_Deep.Look<TemporaryThingDrawer>(ref this.temporaryThingDrawer, "temporaryThingDrawer", Array.Empty<object>());
			Scribe_Deep.Look<FleckManager>(ref this.flecks, "flecks", new object[]
			{
				this
			});
			Scribe_Deep.Look<DeferredSpawner>(ref this.deferredSpawner, "deferredSpawner", new object[]
			{
				this
			});
			Scribe_Deep.Look<AutoSlaughterManager>(ref this.autoSlaughterManager, "autoSlaughterManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<TreeDestructionTracker>(ref this.treeDestructionTracker, "treeDestructionTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<StorageGroupManager>(ref this.storageGroups, "storageGroups", new object[]
			{
				this
			});
			Scribe_Deep.Look<SandGrid>(ref this.sandGrid, "sandGrid", new object[]
			{
				this
			});
			Scribe_Collections.Look<MapComponent>(ref this.components, "components", LookMode.Deep, new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.planManager == null)
				{
					this.planManager = new PlanManager(this);
				}
				if (ModsConfig.BiotechActive && this.pollutionGrid == null)
				{
					this.pollutionGrid = new PollutionGrid(this);
				}
				if (ModsConfig.OdysseyActive)
				{
					if (this.sandGrid == null)
					{
						this.sandGrid = new SandGrid(this);
					}
					if (this.substructureGrid == null)
					{
						this.substructureGrid = new SubstructureGrid(this);
					}
					if (this.waterBodyTracker == null)
					{
						this.waterBodyTracker = new WaterBodyTracker(this);
					}
					if (this.freezeManager == null)
					{
						this.freezeManager = new FreezeManager(this);
					}
				}
			}
			this.FillComponents();
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06001D17 RID: 7447 RVA: 0x00093004 File Offset: 0x00091204
		public void MapPreTick()
		{
			this.itemAvailability.Tick();
			this.listerHaulables.ListerHaulablesTick();
			try
			{
				this.autoBuildRoofAreaSetter.AutoBuildRoofAreaSetterTick_First();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
			}
			this.roofCollapseBufferResolver.CollapseRoofsMarkedToCollapse();
			this.windManager.WindManagerTick();
			try
			{
				this.mapTemperature.MapTemperatureTick();
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.ToString());
			}
			this.temporaryThingDrawer.Tick();
			try
			{
				this.pathFinder.PathFinderTick();
			}
			catch (Exception ex3)
			{
				Log.Error(ex3.ToString());
			}
		}

		// Token: 0x06001D18 RID: 7448 RVA: 0x000930BC File Offset: 0x000912BC
		public void MapPostTick()
		{
			try
			{
				this.wildAnimalSpawner.WildAnimalSpawnerTick();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
			}
			try
			{
				this.wildPlantSpawner.WildPlantSpawnerTick();
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.ToString());
			}
			try
			{
				this.powerNetManager.PowerNetsTick();
			}
			catch (Exception ex3)
			{
				Log.Error(ex3.ToString());
			}
			try
			{
				this.steadyEnvironmentEffects.SteadyEnvironmentEffectsTick();
			}
			catch (Exception ex4)
			{
				Log.Error(ex4.ToString());
			}
			try
			{
				this.tempTerrain.Tick();
			}
			catch (Exception ex5)
			{
				Log.Error(ex5.ToString());
			}
			try
			{
				this.gasGrid.Tick();
			}
			catch (Exception ex6)
			{
				Log.Error(ex6.ToString());
			}
			if (ModsConfig.BiotechActive)
			{
				try
				{
					this.pollutionGrid.PollutionTick();
				}
				catch (Exception ex7)
				{
					Log.Error(ex7.ToString());
				}
			}
			try
			{
				this.deferredSpawner.DeferredSpawnerTick();
			}
			catch (Exception ex8)
			{
				Log.Error(ex8.ToString());
			}
			try
			{
				this.lordManager.LordManagerTick();
			}
			catch (Exception ex9)
			{
				Log.Error(ex9.ToString());
			}
			try
			{
				this.passingShipManager.PassingShipManagerTick();
			}
			catch (Exception ex10)
			{
				Log.Error(ex10.ToString());
			}
			try
			{
				this.debugDrawer.DebugDrawerTick();
			}
			catch (Exception ex11)
			{
				Log.Error(ex11.ToString());
			}
			try
			{
				this.lordsStarter.VoluntarilyJoinableLordsStarterTick();
			}
			catch (Exception ex12)
			{
				Log.Error(ex12.ToString());
			}
			try
			{
				this.gameConditionManager.GameConditionManagerTick();
			}
			catch (Exception ex13)
			{
				Log.Error(ex13.ToString());
			}
			try
			{
				this.weatherManager.WeatherManagerTick();
			}
			catch (Exception ex14)
			{
				Log.Error(ex14.ToString());
			}
			try
			{
				this.resourceCounter.ResourceCounterTick();
			}
			catch (Exception ex15)
			{
				Log.Error(ex15.ToString());
			}
			try
			{
				this.weatherDecider.WeatherDeciderTick();
			}
			catch (Exception ex16)
			{
				Log.Error(ex16.ToString());
			}
			try
			{
				this.fireWatcher.FireWatcherTick();
			}
			catch (Exception ex17)
			{
				Log.Error(ex17.ToString());
			}
			if (ModsConfig.OdysseyActive)
			{
				try
				{
					WaterBodyTracker waterBodyTracker = this.waterBodyTracker;
					if (waterBodyTracker != null)
					{
						waterBodyTracker.Tick();
					}
				}
				catch (Exception ex18)
				{
					Log.Error(ex18.ToString());
				}
			}
			try
			{
				this.flecks.FleckManagerTick();
			}
			catch (Exception ex19)
			{
				Log.Error(ex19.ToString());
			}
			try
			{
				this.effecterMaintainer.EffecterMaintainerTick();
			}
			catch (Exception ex20)
			{
				Log.Error(ex20.ToString());
			}
			MapComponentUtility.MapComponentTick(this);
			try
			{
				foreach (TileMutatorDef tileMutatorDef in this.TileInfo.Mutators)
				{
					TileMutatorWorker worker = tileMutatorDef.Worker;
					if (worker != null)
					{
						worker.Tick(this);
					}
				}
			}
			catch (Exception ex21)
			{
				Log.Error(ex21.ToString());
			}
		}

		// Token: 0x06001D19 RID: 7449 RVA: 0x00093548 File Offset: 0x00091748
		public void MapUpdate()
		{
			if (this.Disposed)
			{
				return;
			}
			bool drawingMap = WorldRendererUtility.DrawingMap;
			this.skyManager.SkyManagerUpdate();
			this.powerNetManager.UpdatePowerNetsAndConnections_First();
			this.regionGrid.UpdateClean();
			this.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
			this.glowGrid.GlowGridUpdate_First();
			this.lordManager.LordManagerUpdate();
			this.postTickVisuals.ProcessPostTickVisuals();
			if (drawingMap && Find.CurrentMap == this)
			{
				if (Map.AlwaysRedrawShadows)
				{
					this.mapDrawer.WholeMapChanged(MapMeshFlagDefOf.Things);
				}
				GlobalRendererUtility.UpdateGlobalShadersParams();
				PlantFallColors.SetFallShaderGlobals(this);
				this.waterInfo.SetTextures();
				this.avoidGrid.DebugDrawOnMap();
				BreachingGridDebug.DebugDrawAllOnMap(this);
				this.mapDrawer.MapMeshDrawerUpdate_First();
				this.powerNetGrid.DrawDebugPowerNetGrid();
				DoorsDebugDrawer.DrawDebug();
				this.mapDrawer.DrawMapMesh();
				this.dynamicDrawManager.DrawDynamicThings();
				this.gameConditionManager.GameConditionManagerDraw(this);
				MapEdgeClipDrawer.DrawClippers(this);
				this.designationManager.DrawDesignations();
				this.overlayDrawer.DrawAllOverlays();
				this.temporaryThingDrawer.Draw();
				this.flecks.FleckManagerDraw();
			}
			try
			{
				this.areaManager.AreaManagerUpdate();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
			}
			this.weatherManager.WeatherManagerUpdate();
			try
			{
				this.flecks.FleckManagerUpdate();
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.ToString());
			}
			MapComponentUtility.MapComponentUpdate(this);
		}

		// Token: 0x06001D1A RID: 7450 RVA: 0x000936D4 File Offset: 0x000918D4
		public T GetComponent<T>() where T : MapComponent
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

		// Token: 0x06001D1B RID: 7451 RVA: 0x00093724 File Offset: 0x00091924
		public MapComponent GetComponent(Type type)
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

		// Token: 0x06001D1C RID: 7452 RVA: 0x00093769 File Offset: 0x00091969
		public void MapOnGUI()
		{
			this.DevGUISketches();
			Map.DevRoadPaths();
			this.pathFinder.OnGUI();
		}

		// Token: 0x06001D1D RID: 7453 RVA: 0x00093784 File Offset: 0x00091984
		private static void DevRoadPaths()
		{
			if (!DebugViewSettings.drawRoadPaths)
			{
				return;
			}
			for (int i = 0; i < GenStep_Roads.paths.Count; i++)
			{
				foreach (IntVec3 intVec in GenStep_Roads.paths[i])
				{
					Vector2 vector = intVec.ToVector3Shifted().MapToUIPosition();
					DevGUI.DrawRect(new Rect(vector.x, vector.y, 5f, 5f), (i % 2 == 0) ? Color.yellow : Color.blue);
				}
			}
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x00093834 File Offset: 0x00091A34
		private void DevGUISketches()
		{
			if ((!DebugViewSettings.drawMapGraphs && !DebugViewSettings.drawMapRooms) || this.layoutStructureSketches.NullOrEmpty<LayoutStructureSketch>())
			{
				return;
			}
			foreach (LayoutStructureSketch layoutStructureSketch in this.layoutStructureSketches)
			{
				this.DebugGUILayoutStructure(layoutStructureSketch);
			}
		}

		// Token: 0x06001D1F RID: 7455 RVA: 0x000938A4 File Offset: 0x00091AA4
		private void DebugGUILayoutStructure(LayoutStructureSketch layoutStructureSketch)
		{
			Map.DevDrawOutline(layoutStructureSketch.structureLayout.container, Color.yellow);
			Vector2 pos = (layoutStructureSketch.structureLayout.container.Min - IntVec3.South).ToVector3().MapToUIPosition();
			Map.DevDrawLabel(layoutStructureSketch.layoutDef.defName, pos);
			if (DebugViewSettings.drawMapGraphs)
			{
				StructureLayout structureLayout = layoutStructureSketch.structureLayout;
				if (((structureLayout != null) ? structureLayout.neighbours : null) != null)
				{
					foreach (KeyValuePair<Vector2, List<Vector2>> keyValuePair in layoutStructureSketch.structureLayout.neighbours.connections)
					{
						foreach (Vector2 b in keyValuePair.Value)
						{
							Vector2 a = layoutStructureSketch.center.ToVector2();
							Vector2 vector = a + keyValuePair.Key;
							Vector2 vector2 = a + b;
							Vector2 start = new Vector3(vector.x, 0f, vector.y).MapToUIPosition();
							Vector2 end = new Vector3(vector2.x, 0f, vector2.y).MapToUIPosition();
							DevGUI.DrawLine(start, end, Color.green, 2f);
						}
					}
				}
			}
			if (DebugViewSettings.drawMapRooms)
			{
				StructureLayout structureLayout2 = layoutStructureSketch.structureLayout;
				if (((structureLayout2 != null) ? structureLayout2.Rooms : null) != null)
				{
					foreach (LayoutRoom layoutRoom in layoutStructureSketch.structureLayout.Rooms)
					{
						string name = "NA";
						if (!layoutRoom.defs.NullOrEmpty<LayoutRoomDef>())
						{
							name = (from x in layoutRoom.defs
							select x.defName).ToCommaList(false, false);
						}
						Map.DevDrawLabel(name, layoutRoom.rects[0].CenterVector3.MapToUIPosition());
						foreach (CellRect r in layoutRoom.rects)
						{
							Map.DevDrawOutline(r, Color.blue);
						}
					}
				}
			}
		}

		// Token: 0x06001D20 RID: 7456 RVA: 0x00093B3C File Offset: 0x00091D3C
		private static void DevDrawLabel(string name, Vector2 pos)
		{
			float widthCached = name.GetWidthCached();
			DevGUI.Label(new Rect(pos.x - widthCached / 2f, pos.y, widthCached, 20f), name);
		}

		// Token: 0x06001D21 RID: 7457 RVA: 0x00093B78 File Offset: 0x00091D78
		private static void DevDrawOutline(CellRect r, Color color)
		{
			IntVec3 min = r.Min;
			IntVec3 intVec = r.Max + new IntVec3(1, 0, 1);
			IntVec3 a = new IntVec3(min.x, 0, min.z);
			IntVec3 intVec2 = new IntVec3(intVec.x, 0, min.z);
			IntVec3 intVec3 = new IntVec3(min.x, 0, intVec.z);
			IntVec3 b = new IntVec3(intVec.x, 0, intVec.z);
			Map.DevDrawLine(a, intVec2, color);
			Map.DevDrawLine(a, intVec3, color);
			Map.DevDrawLine(intVec3, b, color);
			Map.DevDrawLine(intVec2, b, color);
		}

		// Token: 0x06001D22 RID: 7458 RVA: 0x00093C14 File Offset: 0x00091E14
		private static void DevDrawLine(IntVec3 a, IntVec3 b, Color color)
		{
			Vector2 start = a.ToVector3().MapToUIPosition();
			Vector2 end = b.ToVector3().MapToUIPosition();
			DevGUI.DrawLine(start, end, color, 2f);
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06001D23 RID: 7459 RVA: 0x00093C46 File Offset: 0x00091E46
		public int ConstantRandSeed
		{
			get
			{
				return this.uniqueID ^ 16622162;
			}
		}

		// Token: 0x06001D24 RID: 7460 RVA: 0x00093C54 File Offset: 0x00091E54
		public string GetUniqueLoadID()
		{
			return "Map_" + this.uniqueID.ToString();
		}

		// Token: 0x06001D25 RID: 7461 RVA: 0x00093C6C File Offset: 0x00091E6C
		public override string ToString()
		{
			string text = "Map-" + this.uniqueID.ToString();
			if (this.IsPlayerHome)
			{
				text += "-PlayerHome";
			}
			return text;
		}

		// Token: 0x06001D26 RID: 7462 RVA: 0x00093CA4 File Offset: 0x00091EA4
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.spawnedThings;
		}

		// Token: 0x06001D27 RID: 7463 RVA: 0x00093CAC File Offset: 0x00091EAC
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.listerThings.ThingsInGroup(ThingRequestGroup.ThingHolder));
			List<PassingShip> passingShips = this.passingShipManager.passingShips;
			for (int i = 0; i < passingShips.Count; i++)
			{
				IThingHolder thingHolder = passingShips[i] as IThingHolder;
				if (thingHolder != null)
				{
					outChildren.Add(thingHolder);
				}
			}
			for (int j = 0; j < this.components.Count; j++)
			{
				IThingHolder thingHolder2 = this.components[j] as IThingHolder;
				if (thingHolder2 != null)
				{
					outChildren.Add(thingHolder2);
				}
			}
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x00093D38 File Offset: 0x00091F38
		public void Dispose()
		{
			if (this.Disposed)
			{
				return;
			}
			this.Disposed = true;
			foreach (MapComponent mapComponent in this.components)
			{
				IDisposable disposable = mapComponent as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			if (this.regionAndRoomUpdater != null)
			{
				this.regionAndRoomUpdater.Enabled = false;
			}
			PathFinder pathFinder = this.pathFinder;
			if (pathFinder != null)
			{
				pathFinder.Dispose();
			}
			LordManager lordManager = this.lordManager;
			if (lordManager != null)
			{
				lordManager.Dispose();
			}
			FogGrid fogGrid = this.fogGrid;
			if (fogGrid != null)
			{
				fogGrid.Dispose();
			}
			SnowGrid snowGrid = this.snowGrid;
			if (snowGrid != null)
			{
				snowGrid.Dispose();
			}
			GlowGrid glowGrid = this.glowGrid;
			if (glowGrid != null)
			{
				glowGrid.Dispose();
			}
			SandGrid sandGrid = this.sandGrid;
			if (sandGrid != null)
			{
				sandGrid.Dispose();
			}
			AvoidGrid avoidGrid = this.avoidGrid;
			if (avoidGrid != null)
			{
				avoidGrid.Dispose();
			}
			ListerBuildings listerBuildings = this.listerBuildings;
			if (listerBuildings != null)
			{
				listerBuildings.Dispose();
			}
			ListerThings listerThings = this.listerThings;
			if (listerThings != null)
			{
				listerThings.Clear();
			}
			RegionDirtyer regionDirtyer = this.regionDirtyer;
			if (regionDirtyer != null)
			{
				regionDirtyer.SetAllDirty();
			}
			RegionGrid regionGrid = this.regionGrid;
			if (regionGrid != null)
			{
				regionGrid.Dispose();
			}
			Pathing pathing = this.pathing;
			if (pathing != null)
			{
				pathing.Dispose();
			}
			MapDrawer mapDrawer = this.mapDrawer;
			if (mapDrawer != null)
			{
				mapDrawer.Dispose();
			}
			Resources.UnloadUnusedAssets();
			MapGenerator.ClearDebugMode();
		}

		// Token: 0x040018F0 RID: 6384
		public MapFileCompressor compressor;

		// Token: 0x040018F1 RID: 6385
		private List<Thing> loadedFullThings;

		// Token: 0x040018F2 RID: 6386
		public MapGeneratorDef generatorDef;

		// Token: 0x040018F3 RID: 6387
		public int uniqueID = -1;

		// Token: 0x040018F4 RID: 6388
		public int generationTick;

		// Token: 0x040018F5 RID: 6389
		public bool wasSpawnedViaGravShipLanding;

		// Token: 0x040018F6 RID: 6390
		private Color? fogOfWarColor;

		// Token: 0x040018F7 RID: 6391
		private OrbitalDebrisDef orbitalDebris;

		// Token: 0x040018F8 RID: 6392
		private int generatedId;

		// Token: 0x040018F9 RID: 6393
		public MapInfo info = new MapInfo();

		// Token: 0x040018FA RID: 6394
		public MapEvents events;

		// Token: 0x040018FB RID: 6395
		public List<MapComponent> components = new List<MapComponent>();

		// Token: 0x040018FC RID: 6396
		public ThingOwner spawnedThings;

		// Token: 0x040018FD RID: 6397
		public CellIndices cellIndices;

		// Token: 0x040018FE RID: 6398
		public ListerThings listerThings;

		// Token: 0x040018FF RID: 6399
		public ListerBuildings listerBuildings;

		// Token: 0x04001900 RID: 6400
		public MapPawns mapPawns;

		// Token: 0x04001901 RID: 6401
		public DynamicDrawManager dynamicDrawManager;

		// Token: 0x04001902 RID: 6402
		public MapDrawer mapDrawer;

		// Token: 0x04001903 RID: 6403
		public PawnDestinationReservationManager pawnDestinationReservationManager;

		// Token: 0x04001904 RID: 6404
		public TooltipGiverList tooltipGiverList;

		// Token: 0x04001905 RID: 6405
		public ReservationManager reservationManager;

		// Token: 0x04001906 RID: 6406
		public EnrouteManager enrouteManager;

		// Token: 0x04001907 RID: 6407
		public PhysicalInteractionReservationManager physicalInteractionReservationManager;

		// Token: 0x04001908 RID: 6408
		public DesignationManager designationManager;

		// Token: 0x04001909 RID: 6409
		public LordManager lordManager;

		// Token: 0x0400190A RID: 6410
		public PassingShipManager passingShipManager;

		// Token: 0x0400190B RID: 6411
		public HaulDestinationManager haulDestinationManager;

		// Token: 0x0400190C RID: 6412
		public DebugCellDrawer debugDrawer;

		// Token: 0x0400190D RID: 6413
		public GameConditionManager gameConditionManager;

		// Token: 0x0400190E RID: 6414
		public WeatherManager weatherManager;

		// Token: 0x0400190F RID: 6415
		public ZoneManager zoneManager;

		// Token: 0x04001910 RID: 6416
		public PlanManager planManager;

		// Token: 0x04001911 RID: 6417
		public ResourceCounter resourceCounter;

		// Token: 0x04001912 RID: 6418
		public MapTemperature mapTemperature;

		// Token: 0x04001913 RID: 6419
		public TemperatureVacuumCache TemperatureVacuumCache;

		// Token: 0x04001914 RID: 6420
		public AreaManager areaManager;

		// Token: 0x04001915 RID: 6421
		public AttackTargetsCache attackTargetsCache;

		// Token: 0x04001916 RID: 6422
		public AttackTargetReservationManager attackTargetReservationManager;

		// Token: 0x04001917 RID: 6423
		public VoluntarilyJoinableLordsStarter lordsStarter;

		// Token: 0x04001918 RID: 6424
		public FleckManager flecks;

		// Token: 0x04001919 RID: 6425
		public DeferredSpawner deferredSpawner;

		// Token: 0x0400191A RID: 6426
		public ThingGrid thingGrid;

		// Token: 0x0400191B RID: 6427
		public CoverGrid coverGrid;

		// Token: 0x0400191C RID: 6428
		public EdificeGrid edificeGrid;

		// Token: 0x0400191D RID: 6429
		public BlueprintGrid blueprintGrid;

		// Token: 0x0400191E RID: 6430
		public FogGrid fogGrid;

		// Token: 0x0400191F RID: 6431
		public RegionGrid regionGrid;

		// Token: 0x04001920 RID: 6432
		public GlowGrid glowGrid;

		// Token: 0x04001921 RID: 6433
		public TerrainGrid terrainGrid;

		// Token: 0x04001922 RID: 6434
		public Pathing pathing;

		// Token: 0x04001923 RID: 6435
		public RoofGrid roofGrid;

		// Token: 0x04001924 RID: 6436
		public FertilityGrid fertilityGrid;

		// Token: 0x04001925 RID: 6437
		public SnowGrid snowGrid;

		// Token: 0x04001926 RID: 6438
		public DeepResourceGrid deepResourceGrid;

		// Token: 0x04001927 RID: 6439
		public ExitMapGrid exitMapGrid;

		// Token: 0x04001928 RID: 6440
		public AvoidGrid avoidGrid;

		// Token: 0x04001929 RID: 6441
		public GasGrid gasGrid;

		// Token: 0x0400192A RID: 6442
		public PollutionGrid pollutionGrid;

		// Token: 0x0400192B RID: 6443
		public SubstructureGrid substructureGrid;

		// Token: 0x0400192C RID: 6444
		public WaterBodyTracker waterBodyTracker;

		// Token: 0x0400192D RID: 6445
		public SandGrid sandGrid;

		// Token: 0x0400192E RID: 6446
		public LinkGrid linkGrid;

		// Token: 0x0400192F RID: 6447
		public PowerNetManager powerNetManager;

		// Token: 0x04001930 RID: 6448
		public PowerNetGrid powerNetGrid;

		// Token: 0x04001931 RID: 6449
		public RegionMaker regionMaker;

		// Token: 0x04001932 RID: 6450
		public PathFinder pathFinder;

		// Token: 0x04001933 RID: 6451
		public PawnPathPool pawnPathPool;

		// Token: 0x04001934 RID: 6452
		public RegionAndRoomUpdater regionAndRoomUpdater;

		// Token: 0x04001935 RID: 6453
		public RegionLinkDatabase regionLinkDatabase;

		// Token: 0x04001936 RID: 6454
		public MoteCounter moteCounter;

		// Token: 0x04001937 RID: 6455
		public GatherSpotLister gatherSpotLister;

		// Token: 0x04001938 RID: 6456
		public WindManager windManager;

		// Token: 0x04001939 RID: 6457
		public ListerBuildingsRepairable listerBuildingsRepairable;

		// Token: 0x0400193A RID: 6458
		public ListerHaulables listerHaulables;

		// Token: 0x0400193B RID: 6459
		public ListerMergeables listerMergeables;

		// Token: 0x0400193C RID: 6460
		public ListerArtificialBuildingsForMeditation listerArtificialBuildingsForMeditation;

		// Token: 0x0400193D RID: 6461
		public ListerBuldingOfDefInProximity listerBuldingOfDefInProximity;

		// Token: 0x0400193E RID: 6462
		public ListerBuildingWithTagInProximity listerBuildingWithTagInProximity;

		// Token: 0x0400193F RID: 6463
		public ListerFilthInHomeArea listerFilthInHomeArea;

		// Token: 0x04001940 RID: 6464
		public Reachability reachability;

		// Token: 0x04001941 RID: 6465
		public ItemAvailability itemAvailability;

		// Token: 0x04001942 RID: 6466
		public AutoBuildRoofAreaSetter autoBuildRoofAreaSetter;

		// Token: 0x04001943 RID: 6467
		public RoofCollapseBufferResolver roofCollapseBufferResolver;

		// Token: 0x04001944 RID: 6468
		public RoofCollapseBuffer roofCollapseBuffer;

		// Token: 0x04001945 RID: 6469
		public WildAnimalSpawner wildAnimalSpawner;

		// Token: 0x04001946 RID: 6470
		public WildPlantSpawner wildPlantSpawner;

		// Token: 0x04001947 RID: 6471
		public SteadyEnvironmentEffects steadyEnvironmentEffects;

		// Token: 0x04001948 RID: 6472
		public TempTerrainManager tempTerrain;

		// Token: 0x04001949 RID: 6473
		public FreezeManager freezeManager;

		// Token: 0x0400194A RID: 6474
		public SkyManager skyManager;

		// Token: 0x0400194B RID: 6475
		public OverlayDrawer overlayDrawer;

		// Token: 0x0400194C RID: 6476
		public FloodFiller floodFiller;

		// Token: 0x0400194D RID: 6477
		public WeatherDecider weatherDecider;

		// Token: 0x0400194E RID: 6478
		public FireWatcher fireWatcher;

		// Token: 0x0400194F RID: 6479
		public DangerWatcher dangerWatcher;

		// Token: 0x04001950 RID: 6480
		public DamageWatcher damageWatcher;

		// Token: 0x04001951 RID: 6481
		public StrengthWatcher strengthWatcher;

		// Token: 0x04001952 RID: 6482
		public WealthWatcher wealthWatcher;

		// Token: 0x04001953 RID: 6483
		public RegionDirtyer regionDirtyer;

		// Token: 0x04001954 RID: 6484
		public MapCellsInRandomOrder cellsInRandomOrder;

		// Token: 0x04001955 RID: 6485
		public RememberedCameraPos rememberedCameraPos;

		// Token: 0x04001956 RID: 6486
		public MineStrikeManager mineStrikeManager;

		// Token: 0x04001957 RID: 6487
		public StoryState storyState;

		// Token: 0x04001958 RID: 6488
		public RoadInfo roadInfo;

		// Token: 0x04001959 RID: 6489
		public WaterInfo waterInfo;

		// Token: 0x0400195A RID: 6490
		public RetainedCaravanData retainedCaravanData;

		// Token: 0x0400195B RID: 6491
		public TemporaryThingDrawer temporaryThingDrawer;

		// Token: 0x0400195C RID: 6492
		public AnimalPenManager animalPenManager;

		// Token: 0x0400195D RID: 6493
		public MapPlantGrowthRateCalculator plantGrowthRateCalculator;

		// Token: 0x0400195E RID: 6494
		public AutoSlaughterManager autoSlaughterManager;

		// Token: 0x0400195F RID: 6495
		public TreeDestructionTracker treeDestructionTracker;

		// Token: 0x04001960 RID: 6496
		public StorageGroupManager storageGroups;

		// Token: 0x04001961 RID: 6497
		public EffecterMaintainer effecterMaintainer;

		// Token: 0x04001962 RID: 6498
		public PostTickVisuals postTickVisuals;

		// Token: 0x04001963 RID: 6499
		public List<LayoutStructureSketch> layoutStructureSketches = new List<LayoutStructureSketch>();

		// Token: 0x04001964 RID: 6500
		public ThingListChangedCallbacks thingListChangedCallbacks = new ThingListChangedCallbacks();

		// Token: 0x04001965 RID: 6501
		public List<CellRect> landingBlockers = new List<CellRect>();

		// Token: 0x04001966 RID: 6502
		public Tile pocketTileInfo;

		// Token: 0x04001967 RID: 6503
		public const string ThingSaveKey = "thing";

		// Token: 0x04001968 RID: 6504
		[TweakValue("Graphics_Shadow", 0f, 100f)]
		private static bool AlwaysRedrawShadows;

		// Token: 0x0400196A RID: 6506
		private MixedBiomeMapComponent mixedBiomeComp;
	}
}
