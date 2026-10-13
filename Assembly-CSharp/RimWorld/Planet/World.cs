using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x02003CCE RID: 15566
	public sealed class World : IThingHolder, IExposable, IIncidentTarget, ILoadReferenceable, IDisposable
	{
		// Token: 0x170034BE RID: 13502
		// (get) Token: 0x0601528E RID: 86670 RVA: 0x006553C5 File Offset: 0x006535C5
		public float PlanetCoverage
		{
			get
			{
				return this.info.planetCoverage;
			}
		}

		// Token: 0x170034BF RID: 13503
		// (get) Token: 0x0601528F RID: 86671 RVA: 0x00002C42 File Offset: 0x00000E42
		public IThingHolder ParentHolder
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170034C0 RID: 13504
		// (get) Token: 0x06015290 RID: 86672 RVA: 0x006553D2 File Offset: 0x006535D2
		public PlanetTile Tile
		{
			get
			{
				return PlanetTile.Invalid;
			}
		}

		// Token: 0x170034C1 RID: 13505
		// (get) Token: 0x06015291 RID: 86673 RVA: 0x006553D9 File Offset: 0x006535D9
		public StoryState StoryState
		{
			get
			{
				return this.storyState;
			}
		}

		// Token: 0x170034C2 RID: 13506
		// (get) Token: 0x06015292 RID: 86674 RVA: 0x006553E1 File Offset: 0x006535E1
		public GameConditionManager GameConditionManager
		{
			get
			{
				return this.gameConditionManager;
			}
		}

		// Token: 0x170034C3 RID: 13507
		// (get) Token: 0x06015293 RID: 86675 RVA: 0x006553EC File Offset: 0x006535EC
		public float PlayerWealthForStoryteller
		{
			get
			{
				float num = 0f;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					num += maps[i].PlayerWealthForStoryteller;
				}
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				for (int j = 0; j < caravans.Count; j++)
				{
					num += caravans[j].PlayerWealthForStoryteller;
				}
				return num;
			}
		}

		// Token: 0x170034C4 RID: 13508
		// (get) Token: 0x06015294 RID: 86676 RVA: 0x00655456 File Offset: 0x00653656
		public IEnumerable<Pawn> PlayerPawnsForStoryteller
		{
			get
			{
				return PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction;
			}
		}

		// Token: 0x170034C5 RID: 13509
		// (get) Token: 0x06015295 RID: 86677 RVA: 0x00091D22 File Offset: 0x0008FF22
		public FloatRange IncidentPointsRandomFactorRange
		{
			get
			{
				return FloatRange.One;
			}
		}

		// Token: 0x06015296 RID: 86678 RVA: 0x0065545D File Offset: 0x0065365D
		public IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			yield return IncidentTargetTagDefOf.World;
			yield break;
		}

		// Token: 0x06015297 RID: 86679 RVA: 0x00655468 File Offset: 0x00653668
		public void ExposeData()
		{
			Scribe_Deep.Look<WorldInfo>(ref this.info, "info", Array.Empty<object>());
			Scribe_Deep.Look<WorldGrid>(ref this.grid, "grid", Array.Empty<object>());
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				this.ExposeComponents();
				return;
			}
			if (this.grid == null || !this.grid.HasWorldData)
			{
				WorldGenerator.GenerateWithoutWorldData(this.info.seedString);
				return;
			}
			WorldGenerator.GenerateFromScribe(this.info.seedString);
		}

		// Token: 0x06015298 RID: 86680 RVA: 0x006554E4 File Offset: 0x006536E4
		public void ExposeComponents()
		{
			Scribe_Deep.Look<FactionManager>(ref this.factionManager, "factionManager", Array.Empty<object>());
			Scribe_Deep.Look<IdeoManager>(ref this.ideoManager, "ideoManager", Array.Empty<object>());
			Scribe_Deep.Look<WorldPawns>(ref this.worldPawns, "worldPawns", Array.Empty<object>());
			Scribe_Deep.Look<WorldObjectsHolder>(ref this.worldObjects, "worldObjects", Array.Empty<object>());
			Scribe_Deep.Look<GameConditionManager>(ref this.gameConditionManager, "gameConditionManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<StoryState>(ref this.storyState, "storyState", new object[]
			{
				this
			});
			Scribe_Deep.Look<WorldFeatures>(ref this.features, "features", Array.Empty<object>());
			Scribe_Deep.Look<WorldLandmarks>(ref this.landmarks, "landmarks", Array.Empty<object>());
			Scribe_Collections.Look<WorldComponent>(ref this.components, "components", LookMode.Deep, new object[]
			{
				this
			});
			Scribe_Collections.Look<PocketMapParent>(ref this.pocketMaps, "pocketMaps", LookMode.Deep, Array.Empty<object>());
			this.FillComponents();
			BackCompatibility.PostExposeData(this);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.pocketMaps == null)
				{
					this.pocketMaps = new List<PocketMapParent>();
				}
				if (this.landmarks == null)
				{
					this.landmarks = new WorldLandmarks();
				}
				if (ModsConfig.OdysseyActive && this.landmarks.landmarks.NullOrEmpty<PlanetTile, Landmark>())
				{
					foreach (Tile tile in this.grid.Surface.Tiles)
					{
						List<TileMutatorDef> mutatorsNullable = tile.mutatorsNullable;
						if (mutatorsNullable != null)
						{
							mutatorsNullable.Clear();
						}
					}
					WorldGenStep_Mutators.AddMutatorsFromTile(this.grid.Surface);
					new WorldGenStep_Landmarks().GenerateFresh(this.info.seedString, this.grid.Surface);
				}
			}
		}

		// Token: 0x06015299 RID: 86681 RVA: 0x006556B8 File Offset: 0x006538B8
		public void ConstructComponents()
		{
			this.worldObjects = new WorldObjectsHolder();
			this.factionManager = new FactionManager();
			this.ideoManager = new IdeoManager();
			this.worldPawns = new WorldPawns();
			this.gameConditionManager = new GameConditionManager(this);
			this.storyState = new StoryState(this);
			this.renderer = new WorldRenderer();
			this.UI = new WorldInterface();
			this.debugDrawer = new WorldDebugDrawer();
			this.dynamicDrawManager = new WorldDynamicDrawManager();
			this.pathPool = new WorldPathPool();
			this.reachability = new WorldReachability();
			this.ticksAbsCache = new ConfiguredTicksAbsAtGameStartCache();
			this.tilesInRandomOrder = new WorldTilesInRandomOrder();
			this.components.Clear();
			this.FillComponents();
		}

		// Token: 0x0601529A RID: 86682 RVA: 0x00655774 File Offset: 0x00653974
		private void FillComponents()
		{
			this.components.RemoveAll((WorldComponent component) => component == null);
			foreach (Type type in typeof(WorldComponent).AllSubclassesNonAbstract())
			{
				if (this.GetComponent(type) == null)
				{
					try
					{
						WorldComponent item = (WorldComponent)Activator.CreateInstance(type, new object[]
						{
							this
						});
						this.components.Add(item);
					}
					catch (Exception ex)
					{
						string str = "Could not instantiate a WorldComponent of type ";
						Type type2 = type;
						string str2 = (type2 != null) ? type2.ToString() : null;
						string str3 = ": ";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
			this.tileTemperatures = this.GetComponent<TileTemperaturesComp>();
			this.genData = this.GetComponent<WorldGenData>();
		}

		// Token: 0x0601529B RID: 86683 RVA: 0x00655878 File Offset: 0x00653A78
		public void FinalizeInit(bool fromLoad)
		{
			this.pathGrid.RecalculateAllLayersPathCosts();
			AmbientSoundManager.EnsureWorldAmbientSoundCreated();
			WorldComponentUtility.FinalizeInit(this, fromLoad);
		}

		// Token: 0x0601529C RID: 86684 RVA: 0x00655894 File Offset: 0x00653A94
		public void WorldTick()
		{
			this.worldPawns.WorldPawnsTick();
			this.factionManager.FactionManagerTick();
			this.worldObjects.WorldObjectsHolderTick();
			this.debugDrawer.WorldDebugDrawerTick();
			this.pathGrid.WorldPathGridTick();
			WorldComponentUtility.WorldComponentTick(this);
			this.ideoManager.IdeoManagerTick();
		}

		// Token: 0x0601529D RID: 86685 RVA: 0x006558EC File Offset: 0x00653AEC
		public void WorldPostTick()
		{
			try
			{
				this.gameConditionManager.GameConditionManagerTick();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
			}
		}

		// Token: 0x0601529E RID: 86686 RVA: 0x00655924 File Offset: 0x00653B24
		public void WorldUpdate()
		{
			bool worldRendered = WorldRendererUtility.WorldRendered;
			this.renderer.CheckActivateWorldCamera();
			if (worldRendered)
			{
				ExpandableWorldObjectsUtility.ExpandableWorldObjectsUpdate();
				if (ModsConfig.OdysseyActive)
				{
					ExpandableLandmarksUtility.ExpandableLandmarksUpdate();
				}
				this.renderer.DrawWorldLayers();
				this.dynamicDrawManager.DrawDynamicWorldObjects();
				this.features.UpdateFeatures();
				NoiseDebugUI.RenderPlanetNoise();
			}
			WorldComponentUtility.WorldComponentUpdate(this);
			if (ModsConfig.BiotechActive)
			{
				try
				{
					CompDissolutionEffect_Goodwill.WorldUpdate();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
				try
				{
					CompDissolutionEffect_Pollution.WorldUpdate();
				}
				catch (Exception ex2)
				{
					Log.Error(ex2.ToString());
				}
			}
		}

		// Token: 0x0601529F RID: 86687 RVA: 0x006559CC File Offset: 0x00653BCC
		public void WorldOnGUI()
		{
			WorldComponentUtility.WorldComponentOnGUI(this);
		}

		// Token: 0x060152A0 RID: 86688 RVA: 0x006559D4 File Offset: 0x00653BD4
		public T GetComponent<T>() where T : WorldComponent
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

		// Token: 0x060152A1 RID: 86689 RVA: 0x00655A24 File Offset: 0x00653C24
		public WorldComponent GetComponent(Type type)
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

		// Token: 0x060152A2 RID: 86690 RVA: 0x00655A6C File Offset: 0x00653C6C
		public float? CoastAngleAt(PlanetTile tile, BiomeDef waterBiome)
		{
			World.tmpNeighbors.Clear();
			Find.World.grid.GetTileNeighbors(tile, World.tmpNeighbors);
			IEnumerable<PlanetTile> source = from t in World.tmpNeighbors
			where t.Tile.PrimaryBiome == waterBiome
			select t;
			if (!source.Any<PlanetTile>())
			{
				return null;
			}
			float num = GenMath.MeanAngle((from t in source
			select Find.WorldGrid.GetHeadingFromTo(t, tile)).ToList<float>());
			return new float?((450f - num) % 360f);
		}

		// Token: 0x060152A3 RID: 86691 RVA: 0x00655B0C File Offset: 0x00653D0C
		public Rot4 CoastDirectionAt(PlanetTile tile)
		{
			if (!tile.Valid)
			{
				return Rot4.Invalid;
			}
			if (!tile.Tile.PrimaryBiome.canBuildBase)
			{
				return Rot4.Invalid;
			}
			World.tTpOceanDirs.Clear();
			this.grid.GetTileNeighbors(tile, World.tmpNeighbors);
			int i = 0;
			int count = World.tmpNeighbors.Count;
			while (i < count)
			{
				if (this.grid[World.tmpNeighbors[i]].PrimaryBiome == BiomeDefOf.Ocean)
				{
					Rot4 rotFromTo = this.grid.GetRotFromTo(tile, World.tmpNeighbors[i]);
					if (!World.tTpOceanDirs.Contains(rotFromTo))
					{
						World.tTpOceanDirs.Add(rotFromTo);
					}
				}
				i++;
			}
			if (World.tTpOceanDirs.Count == 0)
			{
				return Rot4.Invalid;
			}
			Rand.PushState();
			Rand.Seed = tile.GetHashCode();
			int index = Rand.Range(0, World.tTpOceanDirs.Count);
			Rand.PopState();
			return World.tTpOceanDirs[index];
		}

		// Token: 0x060152A4 RID: 86692 RVA: 0x00655C10 File Offset: 0x00653E10
		public Rot4 LakeDirectionAt(PlanetTile tile)
		{
			if (!tile.Valid)
			{
				return Rot4.Invalid;
			}
			if (!tile.Tile.PrimaryBiome.canBuildBase)
			{
				return Rot4.Invalid;
			}
			World.tTpOceanDirs.Clear();
			this.grid.GetTileNeighbors(tile, World.tmpNeighbors);
			int i = 0;
			int count = World.tmpNeighbors.Count;
			while (i < count)
			{
				if (this.grid[World.tmpNeighbors[i]].PrimaryBiome == BiomeDefOf.Lake)
				{
					Rot4 rotFromTo = this.grid.GetRotFromTo(tile, World.tmpNeighbors[i]);
					if (!World.tTpOceanDirs.Contains(rotFromTo))
					{
						World.tTpOceanDirs.Add(rotFromTo);
					}
				}
				i++;
			}
			if (World.tTpOceanDirs.Count == 0)
			{
				return Rot4.Invalid;
			}
			Rand.PushState();
			Rand.Seed = tile.GetHashCode();
			int index = Rand.Range(0, World.tTpOceanDirs.Count);
			Rand.PopState();
			return World.tTpOceanDirs[index];
		}

		// Token: 0x060152A5 RID: 86693 RVA: 0x00655D13 File Offset: 0x00653F13
		public bool HasCaves(PlanetTile tile)
		{
			return tile.Tile.Mutators.Any((TileMutatorDef m) => m.IsCave);
		}

		// Token: 0x060152A6 RID: 86694 RVA: 0x00655D48 File Offset: 0x00653F48
		public IEnumerable<ThingDef> NaturalRockTypesIn(PlanetTile tile)
		{
			if (tile.Valid)
			{
				List<ThingDef> forceRockTypes = tile.Tile.PrimaryBiome.forceRockTypes;
				if (forceRockTypes != null)
				{
					return forceRockTypes;
				}
			}
			Rand.PushState();
			Rand.Seed = tile.GetHashCode();
			if (this.allNaturalRockDefs == null)
			{
				this.allNaturalRockDefs = (from def in DefDatabase<ThingDef>.AllDefs
				where def.IsNonResourceNaturalRock
				select def).ToList<ThingDef>();
			}
			int num = Rand.RangeInclusive(2, 3);
			if (num > this.allNaturalRockDefs.Count)
			{
				num = this.allNaturalRockDefs.Count;
			}
			World.tmpNaturalRockDefs.Clear();
			World.tmpNaturalRockDefs.AddRange(this.allNaturalRockDefs);
			IEnumerable<ThingDef> result = (from def in World.tmpNaturalRockDefs
			where World.RockAllowedInBiome(def, tile)
			select def).TakeRandomDistinct(num);
			Rand.PopState();
			return result;
		}

		// Token: 0x060152A7 RID: 86695 RVA: 0x00655E3E File Offset: 0x0065403E
		private static bool RockAllowedInBiome(ThingDef def, PlanetTile tile)
		{
			return !def.building.biomeSpecific || (tile.Valid && tile.Tile.PrimaryBiome.extraRockTypes.NotNullAndContains(def));
		}

		// Token: 0x060152A8 RID: 86696 RVA: 0x00655E71 File Offset: 0x00654071
		public bool Impassable(PlanetTile tileID)
		{
			return !this.pathGrid.Passable(tileID);
		}

		// Token: 0x060152A9 RID: 86697 RVA: 0x00002C42 File Offset: 0x00000E42
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x060152AA RID: 86698 RVA: 0x00655E84 File Offset: 0x00654084
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			List<WorldObject> allWorldObjects = this.worldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				IThingHolder thingHolder = allWorldObjects[i] as IThingHolder;
				if (thingHolder != null)
				{
					outChildren.Add(thingHolder);
				}
				List<WorldObjectComp> allComps = allWorldObjects[i].AllComps;
				for (int j = 0; j < allComps.Count; j++)
				{
					IThingHolder thingHolder2 = allComps[j] as IThingHolder;
					if (thingHolder2 != null)
					{
						outChildren.Add(thingHolder2);
					}
				}
			}
			for (int k = 0; k < this.components.Count; k++)
			{
				IThingHolder thingHolder3 = this.components[k] as IThingHolder;
				if (thingHolder3 != null)
				{
					outChildren.Add(thingHolder3);
				}
			}
		}

		// Token: 0x060152AB RID: 86699 RVA: 0x00655F48 File Offset: 0x00654148
		public string GetUniqueLoadID()
		{
			return "World";
		}

		// Token: 0x170034C6 RID: 13510
		// (get) Token: 0x060152AC RID: 86700 RVA: 0x00655F4F File Offset: 0x0065414F
		public int ConstantRandSeed
		{
			get
			{
				return this.info.persistentRandomValue;
			}
		}

		// Token: 0x060152AD RID: 86701 RVA: 0x00655F5C File Offset: 0x0065415C
		public override string ToString()
		{
			return "World-" + this.info.name;
		}

		// Token: 0x060152AE RID: 86702 RVA: 0x00655F73 File Offset: 0x00654173
		public void Dispose()
		{
			WorldGrid worldGrid = this.grid;
			if (worldGrid == null)
			{
				return;
			}
			worldGrid.Dispose();
		}

		// Token: 0x0400EC10 RID: 60432
		public WorldInfo info = new WorldInfo();

		// Token: 0x0400EC11 RID: 60433
		public List<WorldComponent> components = new List<WorldComponent>();

		// Token: 0x0400EC12 RID: 60434
		public FactionManager factionManager;

		// Token: 0x0400EC13 RID: 60435
		public IdeoManager ideoManager;

		// Token: 0x0400EC14 RID: 60436
		public WorldPawns worldPawns;

		// Token: 0x0400EC15 RID: 60437
		public WorldObjectsHolder worldObjects;

		// Token: 0x0400EC16 RID: 60438
		public GameConditionManager gameConditionManager;

		// Token: 0x0400EC17 RID: 60439
		public StoryState storyState;

		// Token: 0x0400EC18 RID: 60440
		public WorldFeatures features;

		// Token: 0x0400EC19 RID: 60441
		public WorldLandmarks landmarks;

		// Token: 0x0400EC1A RID: 60442
		public List<PocketMapParent> pocketMaps = new List<PocketMapParent>();

		// Token: 0x0400EC1B RID: 60443
		public WorldGrid grid;

		// Token: 0x0400EC1C RID: 60444
		public WorldPathGrid pathGrid;

		// Token: 0x0400EC1D RID: 60445
		public WorldRenderer renderer;

		// Token: 0x0400EC1E RID: 60446
		public WorldInterface UI;

		// Token: 0x0400EC1F RID: 60447
		public WorldDebugDrawer debugDrawer;

		// Token: 0x0400EC20 RID: 60448
		public WorldDynamicDrawManager dynamicDrawManager;

		// Token: 0x0400EC21 RID: 60449
		public WorldPathPool pathPool;

		// Token: 0x0400EC22 RID: 60450
		public WorldReachability reachability;

		// Token: 0x0400EC23 RID: 60451
		public WorldTilesInRandomOrder tilesInRandomOrder;

		// Token: 0x0400EC24 RID: 60452
		public ConfiguredTicksAbsAtGameStartCache ticksAbsCache;

		// Token: 0x0400EC25 RID: 60453
		public TileTemperaturesComp tileTemperatures;

		// Token: 0x0400EC26 RID: 60454
		public WorldGenData genData;

		// Token: 0x0400EC27 RID: 60455
		private List<ThingDef> allNaturalRockDefs;

		// Token: 0x0400EC28 RID: 60456
		private static readonly List<ThingDef> tmpNaturalRockDefs = new List<ThingDef>();

		// Token: 0x0400EC29 RID: 60457
		private static readonly List<PlanetTile> tmpNeighbors = new List<PlanetTile>();

		// Token: 0x0400EC2A RID: 60458
		private static readonly List<Rot4> tTpOceanDirs = new List<Rot4>();
	}
}
