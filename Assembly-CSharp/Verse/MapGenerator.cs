using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004B8 RID: 1208
	public static class MapGenerator
	{
		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06001F54 RID: 8020 RVA: 0x000A4ACD File Offset: 0x000A2CCD
		public static MapGenFloatGrid Elevation
		{
			get
			{
				return MapGenerator.FloatGridNamed("Elevation");
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06001F55 RID: 8021 RVA: 0x000A4AD9 File Offset: 0x000A2CD9
		public static MapGenFloatGrid Fertility
		{
			get
			{
				return MapGenerator.FloatGridNamed("Fertility");
			}
		}

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06001F56 RID: 8022 RVA: 0x000A4AE5 File Offset: 0x000A2CE5
		public static MapGenFloatGrid Caves
		{
			get
			{
				return MapGenerator.FloatGridNamed("Caves");
			}
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06001F57 RID: 8023 RVA: 0x000A4AF1 File Offset: 0x000A2CF1
		public static List<CellRect> UsedRects
		{
			get
			{
				return MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			}
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06001F58 RID: 8024 RVA: 0x000A4AFD File Offset: 0x000A2CFD
		// (set) Token: 0x06001F59 RID: 8025 RVA: 0x000A4B1B File Offset: 0x000A2D1B
		public static IntVec3 PlayerStartSpot
		{
			get
			{
				if (!MapGenerator.PlayerStartSpotValid)
				{
					Log.Error("Accessing player start spot before setting it.");
					return IntVec3.Invalid;
				}
				return MapGenerator.playerStartSpotInt;
			}
			set
			{
				MapGenerator.playerStartSpotInt = value;
			}
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06001F5A RID: 8026 RVA: 0x000A4B23 File Offset: 0x000A2D23
		public static bool PlayerStartSpotValid
		{
			get
			{
				return MapGenerator.playerStartSpotInt.IsValid;
			}
		}

		// Token: 0x06001F5B RID: 8027 RVA: 0x000A4B30 File Offset: 0x000A2D30
		public static Map GenerateMap(IntVec3 mapSize, MapParent parent, MapGeneratorDef mapGenerator, IEnumerable<GenStepWithParams> extraGenStepDefs = null, Action<Map> extraInitBeforeContentGen = null, bool isPocketMap = false, bool stepDebugger = false)
		{
			ProgramState programState = Current.ProgramState;
			Current.ProgramState = ProgramState.MapInitializing;
			MapGenerator.ClearWorkingData();
			MapGenerator.playerStartSpotInt = IntVec3.Invalid;
			MapGenerator.rootsToUnfog.Clear();
			MapGenerator.mapBeingGenerated = null;
			MapGenerator.gravship = null;
			DeepProfiler.Start("InitNewGeneratedMap");
			Rand.PushState();
			int seed = Gen.HashCombineInt(Find.World.info.Seed, (parent != null) ? parent.Tile.GetHashCode() : 0);
			if (isPocketMap)
			{
				seed = Gen.HashCombineInt(Find.World.info.Seed, (parent != null) ? parent.ID : Rand.Int);
			}
			Rand.Seed = seed;
			if (stepDebugger)
			{
				MapGenerator.debugMode = true;
				MapGenerator.debugSeed = seed;
				MapGenerator.debugGenStepIndex = 0;
			}
			Map map2;
			try
			{
				if (parent != null && parent.HasMap)
				{
					Log.Error(string.Format("Tried to generate a new map and set {0} as its parent, but this world object already has a map. One world object can't have more than 1 map.", parent));
					parent = null;
				}
				DeepProfiler.Start("Set up map");
				Map map = new Map();
				map.uniqueID = Find.UniqueIDsManager.GetNextMapID();
				map.generationTick = GenTicks.TicksGame;
				map.events = new MapEvents(map);
				MapGenerator.mapBeingGenerated = map;
				map.info.Size = mapSize;
				map.info.parent = parent;
				if (ModsConfig.OdysseyActive && extraGenStepDefs != null)
				{
					if (extraGenStepDefs.Any((GenStepWithParams step) => step.def == GenStepDefOf.ReserveGravshipArea))
					{
						map.wasSpawnedViaGravShipLanding = true;
					}
				}
				if (mapGenerator == null)
				{
					Log.Error("Attempted to generate map without generator; falling back on encounter map");
					mapGenerator = MapGeneratorDefOf.Encounter;
				}
				map.generatorDef = mapGenerator;
				map.info.disableSunShadows = mapGenerator.disableShadows;
				if (isPocketMap)
				{
					map.info.isPocketMap = true;
					map.pocketTileInfo = new Tile
					{
						PrimaryBiome = mapGenerator.pocketMapProperties.biome
					};
					foreach (TileMutatorDef mutator in mapGenerator.pocketMapProperties.tileMutators)
					{
						map.TileInfo.AddMutator(mutator);
					}
				}
				map.ConstructComponents();
				foreach (TileMutatorDef tileMutatorDef in map.TileInfo.Mutators)
				{
					TileMutatorWorker worker = tileMutatorDef.Worker;
					if (worker != null)
					{
						worker.Init(map);
					}
				}
				DeepProfiler.End();
				Current.Game.AddMap(map);
				if (mapGenerator.isUnderground)
				{
					foreach (IntVec3 c in map.AllCells)
					{
						map.roofGrid.SetRoof(c, mapGenerator.roofDef ?? RoofDefOf.RoofRockThick);
					}
				}
				if (extraInitBeforeContentGen != null)
				{
					extraInitBeforeContentGen(map);
				}
				IEnumerable<GenStepWithParams> enumerable = mapGenerator.genSteps.Where(new Func<GenStepDef, bool>(MapGenerator.<GenerateMap>g__IsValidBiome|32_1)).Select(new Func<GenStepDef, GenStepWithParams>(MapGenerator.<GenerateMap>g__GetGenStepParms|32_2));
				foreach (TileMutatorDef tileMutatorDef2 in map.TileInfo.Mutators)
				{
					if (tileMutatorDef2.extraGenSteps.Any<GenStepDef>())
					{
						enumerable = enumerable.Concat(tileMutatorDef2.extraGenSteps.Select(new Func<GenStepDef, GenStepWithParams>(MapGenerator.<GenerateMap>g__GetGenStepParms|32_2)));
					}
				}
				if (map.Biome.extraGenSteps.Any<GenStepDef>())
				{
					enumerable = enumerable.Concat(map.Biome.extraGenSteps.Where(new Func<GenStepDef, bool>(MapGenerator.<GenerateMap>g__IsValidBiome|32_1)).Select(new Func<GenStepDef, GenStepWithParams>(MapGenerator.<GenerateMap>g__GetGenStepParms|32_2)));
				}
				if (map.Biome.preventGenSteps.Any<GenStepDef>())
				{
					enumerable = from step in enumerable
					where !map.Biome.preventGenSteps.Contains(step.def)
					select step;
				}
				using (IEnumerator<TileMutatorDef> enumerator2 = map.TileInfo.Mutators.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						TileMutatorDef mut = enumerator2.Current;
						if (mut.preventGenSteps.Any<GenStepDef>())
						{
							enumerable = from step in enumerable
							where !mut.preventGenSteps.Contains(step.def)
							select step;
						}
					}
				}
				if (extraGenStepDefs != null)
				{
					enumerable = enumerable.Concat(extraGenStepDefs);
				}
				enumerable = enumerable.Distinct<GenStepWithParams>();
				map.areaManager.AddStartingAreas();
				map.weatherDecider.StartInitialWeather();
				DeepProfiler.Start("Generate contents into map");
				MapGenerator.GenerateContentsIntoMap(enumerable, map, seed, stepDebugger);
				DeepProfiler.End();
				Find.Scenario.PostMapGenerate(map);
				DeepProfiler.Start("Finalize map init");
				map.FinalizeInit();
				DeepProfiler.End();
				DeepProfiler.Start("MapComponent.MapGenerated()");
				MapComponentUtility.MapGenerated(map);
				DeepProfiler.End();
				if (parent != null)
				{
					parent.PostMapGenerate();
				}
				DeepProfiler.Start("Map generator post init");
				if (!stepDebugger)
				{
					MapGenerator.MapGeneratorPostInit(enumerable, map);
				}
				DeepProfiler.End();
				if (MapGenerator.gravship != null && !stepDebugger && map.Parent.CanBeSettled)
				{
					GravshipUtility.SettleTile(map);
				}
				if (map.TileInfo.Layer.Def.isSpace)
				{
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.Orbit, OpportunityType.Critical);
				}
				map2 = map;
			}
			finally
			{
				DeepProfiler.End();
				if (!stepDebugger)
				{
					MapGenerator.ClearWorkingData();
					MapGenerator.mapBeingGenerated = null;
					MapGenerator.gravship = null;
				}
				Current.ProgramState = programState;
				Rand.PopState();
			}
			return map2;
		}

		// Token: 0x06001F5C RID: 8028 RVA: 0x000A51BC File Offset: 0x000A33BC
		public static bool DebugDoNextGenStep(Map map)
		{
			if (MapGenerator.debugGenStepIndex >= MapGenerator.tmpGenSteps.Count)
			{
				MapGenerator.ClearDebugMode();
				return false;
			}
			GenStepWithParams genStepWithParams = MapGenerator.tmpGenSteps[MapGenerator.debugGenStepIndex];
			Log.Message("Doing gen step " + genStepWithParams.def.defName);
			Rand.PushState(Gen.HashCombineInt(MapGenerator.debugSeed, MapGenerator.GetSeedPart(MapGenerator.tmpGenSteps, MapGenerator.debugGenStepIndex)));
			try
			{
				GenStepParams parms = genStepWithParams.parms;
				if (MapGenerator.gravship != null)
				{
					parms.gravship = MapGenerator.gravship;
				}
				genStepWithParams.def.genStep.Generate(map, parms);
				genStepWithParams.def.genStep.PostMapInitialized(map, parms);
			}
			catch (Exception arg)
			{
				Log.Error(string.Format("Error stepping GenStep {0}: {1}", genStepWithParams.def.defName, arg));
			}
			Rand.PopState();
			map.FinalizeInit();
			MapGenerator.debugGenStepIndex++;
			return true;
		}

		// Token: 0x06001F5D RID: 8029 RVA: 0x000A52B0 File Offset: 0x000A34B0
		public static void ClearDebugMode()
		{
			MapGenerator.debugMode = false;
			MapGenerator.debugSeed = 0;
			MapGenerator.debugGenStepIndex = 0;
		}

		// Token: 0x06001F5E RID: 8030 RVA: 0x000A52C4 File Offset: 0x000A34C4
		public static void MapGeneratorPostInit(IEnumerable<GenStepWithParams> genStepDefs, Map map)
		{
			MapGenerator.tmpGenSteps.Clear();
			MapGenerator.tmpGenSteps.AddRange(from x in genStepDefs
			orderby x.def.order, x.def.index
			select x);
			MapGenerator.tmpGenSteps.RemoveWhere((GenStepWithParams a) => MapGenerator.tmpGenSteps.Any((GenStepWithParams b) => b.def.preventsGenSteps != null && b.def.preventsGenSteps.Contains(a.def)));
			for (int i = 0; i < MapGenerator.tmpGenSteps.Count; i++)
			{
				GenStepParams parms = MapGenerator.tmpGenSteps[i].parms;
				if (MapGenerator.gravship != null)
				{
					parms.gravship = MapGenerator.gravship;
				}
				MapGenerator.tmpGenSteps[i].def.genStep.PostMapInitialized(map, parms);
			}
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x000A53AC File Offset: 0x000A35AC
		public static void GenerateContentsIntoMap(IEnumerable<GenStepWithParams> genStepDefs, Map map, int seed, bool stepDebugger = false)
		{
			MapGenerator.ClearWorkingData();
			if (ModsConfig.OdysseyActive)
			{
				foreach (GenStepWithParams genStepWithParams in genStepDefs)
				{
					if (genStepWithParams.def == GenStepDefOf.GravshipMarker)
					{
						MapGenerator.gravship = genStepWithParams.parms.gravship;
						break;
					}
				}
			}
			Rand.PushState();
			try
			{
				Rand.Seed = seed;
				RockNoises.Init(map);
				MapGenerator.tmpGenSteps.Clear();
				MapGenerator.tmpGenSteps.AddRange(from x in genStepDefs
				orderby x.def.order, x.def.index
				select x);
				MapGenerator.tmpGenSteps.RemoveWhere((GenStepWithParams a) => MapGenerator.tmpGenSteps.Any((GenStepWithParams b) => b.def.preventsGenSteps != null && b.def.preventsGenSteps.Contains(a.def)));
				if (!stepDebugger)
				{
					for (int i = 0; i < MapGenerator.tmpGenSteps.Count; i++)
					{
						string str = "GenStep - ";
						GenStepDef def = MapGenerator.tmpGenSteps[i].def;
						DeepProfiler.Start(str + ((def != null) ? def.ToString() : null));
						try
						{
							GenStepParams parms = MapGenerator.tmpGenSteps[i].parms;
							if (MapGenerator.gravship != null)
							{
								parms.gravship = MapGenerator.gravship;
							}
							Rand.PushState();
							Rand.Seed = Gen.HashCombineInt(seed, MapGenerator.GetSeedPart(MapGenerator.tmpGenSteps, i));
							MapGenerator.tmpGenSteps[i].def.genStep.Generate(map, parms);
							if (map.pathing.IncrementalDirtyingDisabled)
							{
								Log.Error(string.Format("Genstep [{0}] {1} ended with path incremental dirtying disabled, for safety reasons it must be reenabled before returning.", i, MapGenerator.tmpGenSteps[i].def));
								map.pathing.ReEnableIncrementalDirtying();
							}
							Rand.PopState();
						}
						catch (Exception arg)
						{
							Log.Error(string.Format("Error in GenStep: {0}", arg));
						}
						finally
						{
							DeepProfiler.End();
						}
					}
				}
			}
			finally
			{
				if (!stepDebugger)
				{
					Rand.PopState();
					RockNoises.Reset();
				}
			}
		}

		// Token: 0x06001F60 RID: 8032 RVA: 0x000A561C File Offset: 0x000A381C
		private static void ClearWorkingData()
		{
			List<CellRect> source;
			if (Prefs.DevMode && MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out source))
			{
				MapGenerator.cachedUsedRects = source.ToList<CellRect>();
			}
			foreach (KeyValuePair<string, object> keyValuePair in MapGenerator.data)
			{
				string text;
				object obj;
				keyValuePair.Deconstruct(out text, out obj);
				IDisposable disposable = obj as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			MapGenerator.data.Clear();
		}

		// Token: 0x06001F61 RID: 8033 RVA: 0x000A56B0 File Offset: 0x000A38B0
		public static void DebugDraw()
		{
			if (DebugViewSettings.drawUsedRects)
			{
				float y = AltitudeLayer.MetaOverlays.AltitudeFor();
				foreach (CellRect cellRect in MapGenerator.cachedUsedRects)
				{
					GenDraw.DrawLineBetween(new Vector3((float)cellRect.minX, y, (float)cellRect.minZ), new Vector3((float)cellRect.minX, y, (float)(cellRect.maxZ + 1)), SimpleColor.Red, 0.2f);
					GenDraw.DrawLineBetween(new Vector3((float)(cellRect.maxX + 1), y, (float)cellRect.minZ), new Vector3((float)(cellRect.maxX + 1), y, (float)(cellRect.maxZ + 1)), SimpleColor.Red, 0.2f);
					GenDraw.DrawLineBetween(new Vector3((float)cellRect.minX, y, (float)cellRect.minZ), new Vector3((float)(cellRect.maxX + 1), y, (float)cellRect.minZ), SimpleColor.Red, 0.2f);
					GenDraw.DrawLineBetween(new Vector3((float)cellRect.minX, y, (float)(cellRect.maxZ + 1)), new Vector3((float)(cellRect.maxX + 1), y, (float)(cellRect.maxZ + 1)), SimpleColor.Red, 0.2f);
				}
			}
		}

		// Token: 0x06001F62 RID: 8034 RVA: 0x000A57F0 File Offset: 0x000A39F0
		public static T GetVar<T>(string name)
		{
			object obj;
			if (MapGenerator.data.TryGetValue(name, out obj))
			{
				return (T)((object)obj);
			}
			return default(T);
		}

		// Token: 0x06001F63 RID: 8035 RVA: 0x000A581C File Offset: 0x000A3A1C
		public static bool TryGetVar<T>(string name, out T var)
		{
			object obj;
			if (MapGenerator.data.TryGetValue(name, out obj))
			{
				var = (T)((object)obj);
				return true;
			}
			var = default(T);
			return false;
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x000A5850 File Offset: 0x000A3A50
		public static T GetOrGenerateVar<T>(string name)
		{
			T t = MapGenerator.GetVar<T>(name);
			if (t != null)
			{
				return t;
			}
			t = (T)((object)Activator.CreateInstance(typeof(T)));
			MapGenerator.SetVar<T>(name, t);
			return t;
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x000A588B File Offset: 0x000A3A8B
		public static void SetVar<T>(string name, T var)
		{
			MapGenerator.data[name] = var;
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x000A58A0 File Offset: 0x000A3AA0
		public static MapGenFloatGrid FloatGridNamed(string name)
		{
			MapGenFloatGrid var = MapGenerator.GetVar<MapGenFloatGrid>(name);
			if (var != null)
			{
				return var;
			}
			MapGenFloatGrid mapGenFloatGrid = new MapGenFloatGrid(MapGenerator.mapBeingGenerated);
			MapGenerator.SetVar<MapGenFloatGrid>(name, mapGenFloatGrid);
			return mapGenFloatGrid;
		}

		// Token: 0x06001F67 RID: 8039 RVA: 0x000A58CC File Offset: 0x000A3ACC
		private static int GetSeedPart(List<GenStepWithParams> genSteps, int index)
		{
			int seedPart = genSteps[index].def.genStep.SeedPart;
			int num = 0;
			for (int i = 0; i < index; i++)
			{
				if (MapGenerator.tmpGenSteps[i].def.genStep.SeedPart == seedPart)
				{
					num++;
				}
			}
			return seedPart + num;
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x000A597C File Offset: 0x000A3B7C
		[CompilerGenerated]
		internal static bool <GenerateMap>g__IsValidBiome|32_1(GenStepDef g)
		{
			return !Find.Scenario.parts.Any((ScenPart p) => typeof(ScenPart_DisableMapGen).IsAssignableFrom(p.def.scenPartClass) && p.def.genStep == g);
		}

		// Token: 0x06001F6A RID: 8042 RVA: 0x000A59B4 File Offset: 0x000A3BB4
		[CompilerGenerated]
		internal static GenStepWithParams <GenerateMap>g__GetGenStepParms|32_2(GenStepDef x)
		{
			return new GenStepWithParams(x, default(GenStepParams));
		}

		// Token: 0x04001B03 RID: 6915
		public static Map mapBeingGenerated;

		// Token: 0x04001B04 RID: 6916
		private static IntVec3 playerStartSpotInt = IntVec3.Invalid;

		// Token: 0x04001B05 RID: 6917
		private static Gravship gravship;

		// Token: 0x04001B06 RID: 6918
		public static List<IntVec3> rootsToUnfog = new List<IntVec3>();

		// Token: 0x04001B07 RID: 6919
		private static Dictionary<string, object> data = new Dictionary<string, object>();

		// Token: 0x04001B08 RID: 6920
		private static List<GenStepWithParams> tmpGenSteps = new List<GenStepWithParams>();

		// Token: 0x04001B09 RID: 6921
		private static int debugSeed;

		// Token: 0x04001B0A RID: 6922
		public static bool debugMode = false;

		// Token: 0x04001B0B RID: 6923
		private static int debugGenStepIndex = 0;

		// Token: 0x04001B0C RID: 6924
		private static List<CellRect> cachedUsedRects = new List<CellRect>();

		// Token: 0x04001B0D RID: 6925
		public const string ElevationName = "Elevation";

		// Token: 0x04001B0E RID: 6926
		public const string FertilityName = "Fertility";

		// Token: 0x04001B0F RID: 6927
		public const string CavesName = "Caves";

		// Token: 0x04001B10 RID: 6928
		public const string RectOfInterestName = "RectOfInterest";

		// Token: 0x04001B11 RID: 6929
		public const string UsedRectsName = "UsedRects";

		// Token: 0x04001B12 RID: 6930
		public const string RectOfInterestTurretsGenStepsCount = "RectOfInterestTurretsGenStepsCount";

		// Token: 0x04001B13 RID: 6931
		public const string DontGenerateClearedGravShipTerrain = "DontGenerateClearedGravShipTerrain";

		// Token: 0x04001B14 RID: 6932
		public const string GravshipSpawnSet = "GravshipSpawnSet";

		// Token: 0x04001B15 RID: 6933
		public const string SpawnRectName = "SpawnRect";

		// Token: 0x04001B16 RID: 6934
		public static List<MapGenUtility.CellData> tmpDataDump = new List<MapGenUtility.CellData>();
	}
}
