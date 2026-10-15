using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld;

public class BiomeDef : Def
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass109_0
	{
		public BiomeAnimalRecord wa;

		internal bool _003CConfigErrors_003Eb__0(BiomeAnimalRecord a)
		{
			return a.animal == wa.animal;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass109_1
	{
		public BiomeAnimalRecord pa;

		internal bool _003CConfigErrors_003Eb__1(BiomeAnimalRecord a)
		{
			return a.animal == pa.animal;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass109_2
	{
		public BiomeAnimalRecord ca;

		internal bool _003CConfigErrors_003Eb__2(BiomeAnimalRecord a)
		{
			return a.animal == ca.animal;
		}
	}

	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__109 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public BiomeDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		private List<BiomeAnimalRecord>.Enumerator _003C_003E7__wrap2;

		string IEnumerator<string>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CConfigErrors_003Ed__109(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -5:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			case -6:
			case 4:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally4();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = default(List<BiomeAnimalRecord>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				BiomeDef biomeDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)biomeDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007a;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007a;
				case 2:
					_003C_003E1__state = -4;
					goto IL_0124;
				case 3:
					_003C_003E1__state = -5;
					goto IL_01d3;
				case 4:
					{
						_003C_003E1__state = -6;
						goto IL_027f;
					}
					IL_007a:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (!Prefs.DevMode)
					{
						break;
					}
					_003C_003E7__wrap2 = biomeDef.wildAnimals.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0124;
					IL_01d3:
					while (_003C_003E7__wrap2.MoveNext())
					{
						_003C_003Ec__DisplayClass109_1 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass109_1
						{
							pa = _003C_003E7__wrap2.Current
						};
						if (biomeDef.pollutionWildAnimals.Count((BiomeAnimalRecord a) => a.animal == CS_0024_003C_003E8__locals2.pa.animal) > 1)
						{
							_003C_003E2__current = "Duplicate pollution animal record: " + CS_0024_003C_003E8__locals2.pa.animal.defName;
							_003C_003E1__state = 3;
							return true;
						}
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap2 = default(List<BiomeAnimalRecord>.Enumerator);
					goto IL_01f2;
					IL_01f2:
					if (!ModsConfig.OdysseyActive)
					{
						break;
					}
					_003C_003E7__wrap2 = biomeDef.coastalWildAnimals.GetEnumerator();
					_003C_003E1__state = -6;
					goto IL_027f;
					IL_0124:
					while (_003C_003E7__wrap2.MoveNext())
					{
						_003C_003Ec__DisplayClass109_0 CS_0024_003C_003E8__locals5 = new _003C_003Ec__DisplayClass109_0
						{
							wa = _003C_003E7__wrap2.Current
						};
						if (biomeDef.wildAnimals.Count((BiomeAnimalRecord a) => a.animal == CS_0024_003C_003E8__locals5.wa.animal) > 1)
						{
							_003C_003E2__current = "Duplicate animal record: " + CS_0024_003C_003E8__locals5.wa.animal.defName;
							_003C_003E1__state = 2;
							return true;
						}
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<BiomeAnimalRecord>.Enumerator);
					if (ModsConfig.BiotechActive)
					{
						_003C_003E7__wrap2 = biomeDef.pollutionWildAnimals.GetEnumerator();
						_003C_003E1__state = -5;
						goto IL_01d3;
					}
					goto IL_01f2;
					IL_027f:
					while (_003C_003E7__wrap2.MoveNext())
					{
						_003C_003Ec__DisplayClass109_2 CS_0024_003C_003E8__locals8 = new _003C_003Ec__DisplayClass109_2
						{
							ca = _003C_003E7__wrap2.Current
						};
						if (biomeDef.coastalWildAnimals.Count((BiomeAnimalRecord a) => a.animal == CS_0024_003C_003E8__locals8.ca.animal) > 1)
						{
							_003C_003E2__current = "Duplicate coastal animal record: " + CS_0024_003C_003E8__locals8.ca.animal.defName;
							_003C_003E1__state = 4;
							return true;
						}
					}
					_003C_003Em__Finally4();
					_003C_003E7__wrap2 = default(List<BiomeAnimalRecord>.Enumerator);
					break;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally4()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__109 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__109(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_AllWildAnimals_003Ed__94 : IEnumerable<PawnKindDef>, IEnumerable, IEnumerator<PawnKindDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private PawnKindDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public BiomeDef _003C_003E4__this;

		private IEnumerator<PawnKindDef> _003C_003E7__wrap1;

		PawnKindDef IEnumerator<PawnKindDef>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003Cget_AllWildAnimals_003Ed__94(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				BiomeDef biomeDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = DefDatabase<PawnKindDef>.AllDefs.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					PawnKindDef current = _003C_003E7__wrap1.Current;
					if (biomeDef.CommonalityOfAnimal(current) > 0f || biomeDef.CommonalityOfPollutionAnimal(current) > 0f || biomeDef.CommonalityOfCoastalAnimal(current) > 0f)
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<PawnKindDef> IEnumerable<PawnKindDef>.GetEnumerator()
		{
			_003Cget_AllWildAnimals_003Ed__94 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_AllWildAnimals_003Ed__94(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<PawnKindDef>)this).GetEnumerator();
		}
	}

	public Type workerClass;

	public bool implemented = true;

	public bool generatesNaturally = true;

	public bool canBuildBase = true;

	public bool canAutoChoose = true;

	public bool allowRoads = true;

	public bool allowRivers = true;

	public bool allowFarmingCamps = true;

	public float animalDensity;

	public float plantDensity;

	public float diseaseMtbDays = 60f;

	public float settlementSelectionWeight = 1f;

	public float campSelectionWeight = 1f;

	public float pollutionOffset;

	public bool impassable;

	public bool hasVirtualPlants = true;

	public float forageability;

	public ThingDef foragedFood;

	public bool wildPlantsCareAboutLocalFertility = true;

	public bool wildPlantsAreCavePlants;

	public float wildPlantRegrowDays = 25f;

	public float movementDifficulty = 1f;

	public List<WeatherCommonalityRecord> baseWeatherCommonalities = new List<WeatherCommonalityRecord>();

	public List<TerrainThreshold> terrainsByFertility = new List<TerrainThreshold>();

	public List<SoundDef> soundsAmbient = new List<SoundDef>();

	public List<TerrainPatchMaker> terrainPatchMakers = new List<TerrainPatchMaker>();

	public List<BiomePlantRecord> wildPlants = new List<BiomePlantRecord>();

	private List<BiomeAnimalRecord> wildAnimals = new List<BiomeAnimalRecord>();

	private List<BiomeAnimalRecord> pollutionWildAnimals = new List<BiomeAnimalRecord>();

	private List<BiomeAnimalRecord> coastalWildAnimals = new List<BiomeAnimalRecord>();

	private List<BiomeDiseaseRecord> diseases = new List<BiomeDiseaseRecord>();

	private List<ThingDef> allowedPackAnimals = new List<ThingDef>();

	public bool hasBedrock = true;

	public bool isExtremeBiome;

	public bool isWaterBiome;

	public bool allowPollution = true;

	public bool wildAnimalsCanWanderInto = true;

	public bool noAmbientWind;

	public bool inVacuum;

	public bool disableSkyLighting;

	public bool disableShadows;

	public bool noGravel;

	public bool canExitMap = true;

	public bool onlyAllowWhitelistedArrivalModes;

	public bool isBackgroundBiome;

	public float wildAnimalScariaChance;

	public float geyserCountFactor = 1f;

	public float? constantOutdoorTemperature;

	public TerrainDef coastalBeachTerrain;

	public TerrainDef lakeBeachTerrain;

	public TerrainDef riverbankTerrain;

	public TerrainDef mudTerrain;

	public TerrainDef gravelTerrain;

	public TerrainDef waterShallowTerrain;

	public TerrainDef waterDeepTerrain;

	public TerrainDef oceanShallowTerrain;

	public TerrainDef oceanDeepTerrain;

	public TerrainDef waterMovingShallowTerrain;

	public TerrainDef waterMovingChestDeepTerrain;

	public IntRange riverbankSizeRange;

	public List<GameConditionDef> biomeMapConditions = new List<GameConditionDef>();

	public List<GenStepDef> extraGenSteps = new List<GenStepDef>();

	public List<GenStepDef> preventGenSteps = new List<GenStepDef>();

	public List<ThingDef> extraRockTypes;

	public List<ThingDef> forceRockTypes;

	public float maxFishPopulation;

	public BiomeFishTypes fishTypes;

	public List<PlanetLayerDef> layerWhitelist;

	public List<PlanetLayerDef> layerBlacklist;

	[MustTranslate]
	public string settleWarning;

	public Color? fogOfWarColor;

	public OrbitalDebrisDef orbitalDebris;

	[NoTranslate]
	public string texture;

	[Unsaved(false)]
	private Dictionary<PawnKindDef, float> cachedAnimalCommonalities;

	[Unsaved(false)]
	private Dictionary<PawnKindDef, float> cachedPollutionAnimalCommonalities;

	[Unsaved(false)]
	private Dictionary<PawnKindDef, float> cachedCoastalAnimalCommonalities;

	[Unsaved(false)]
	private Dictionary<ThingDef, float> cachedPlantCommonalities;

	[Unsaved(false)]
	private Dictionary<IncidentDef, float> cachedDiseaseCommonalities;

	[Unsaved(false)]
	private Material cachedMat;

	[Unsaved(false)]
	private List<ThingDef> cachedWildPlants;

	[Unsaved(false)]
	private float? cachedLowestWildPlantOrder;

	[Unsaved(false)]
	private int? cachedMaxWildPlantsClusterRadius;

	private BiomeWorker worker;

	public BiomeWorker Worker => worker ?? (worker = GenWorker<BiomeWorker>.Get(workerClass));

	public Material DrawMaterial
	{
		get
		{
			if (cachedMat == null)
			{
				if (texture.NullOrEmpty())
				{
					return null;
				}
				if (this == BiomeDefOf.Ocean || this == BiomeDefOf.Lake)
				{
					cachedMat = MaterialAllocator.Create(WorldMaterials.WorldOcean);
				}
				else if (!allowRoads && !allowRivers)
				{
					cachedMat = MaterialAllocator.Create(WorldMaterials.WorldIce);
				}
				else
				{
					cachedMat = MaterialAllocator.Create(WorldMaterials.WorldTerrain);
				}
				cachedMat.mainTexture = ContentFinder<Texture2D>.Get(texture);
			}
			return cachedMat;
		}
	}

	public List<ThingDef> AllWildPlants
	{
		get
		{
			if (cachedWildPlants == null)
			{
				cachedWildPlants = new List<ThingDef>();
				foreach (ThingDef item in DefDatabase<ThingDef>.AllDefsListForReading)
				{
					if (item.category == ThingCategory.Plant && CommonalityOfPlant(item) > 0f)
					{
						cachedWildPlants.Add(item);
					}
				}
			}
			return cachedWildPlants;
		}
	}

	public int MaxWildAndCavePlantsClusterRadius
	{
		get
		{
			if (!cachedMaxWildPlantsClusterRadius.HasValue)
			{
				cachedMaxWildPlantsClusterRadius = 0;
				List<ThingDef> allWildPlants = AllWildPlants;
				for (int i = 0; i < allWildPlants.Count; i++)
				{
					if (allWildPlants[i].plant.GrowsInClusters)
					{
						cachedMaxWildPlantsClusterRadius = Mathf.Max(cachedMaxWildPlantsClusterRadius.Value, allWildPlants[i].plant.wildClusterRadius);
					}
				}
				List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int j = 0; j < allDefsListForReading.Count; j++)
				{
					if (allDefsListForReading[j].category == ThingCategory.Plant && allDefsListForReading[j].plant.cavePlant)
					{
						cachedMaxWildPlantsClusterRadius = Mathf.Max(cachedMaxWildPlantsClusterRadius.Value, allDefsListForReading[j].plant.wildClusterRadius);
					}
				}
			}
			return cachedMaxWildPlantsClusterRadius.Value;
		}
	}

	public float LowestWildAndCavePlantOrder
	{
		get
		{
			if (!cachedLowestWildPlantOrder.HasValue)
			{
				cachedLowestWildPlantOrder = 2.1474836E+09f;
				List<ThingDef> allWildPlants = AllWildPlants;
				for (int i = 0; i < allWildPlants.Count; i++)
				{
					cachedLowestWildPlantOrder = Mathf.Min(cachedLowestWildPlantOrder.Value, allWildPlants[i].plant.wildOrder);
				}
				List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int j = 0; j < allDefsListForReading.Count; j++)
				{
					if (allDefsListForReading[j].category == ThingCategory.Plant && allDefsListForReading[j].plant.cavePlant)
					{
						cachedLowestWildPlantOrder = Mathf.Min(cachedLowestWildPlantOrder.Value, allDefsListForReading[j].plant.wildOrder);
					}
				}
			}
			return cachedLowestWildPlantOrder.Value;
		}
	}

	public IEnumerable<PawnKindDef> AllWildAnimals
	{
		[IteratorStateMachine(typeof(_003Cget_AllWildAnimals_003Ed__94))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_AllWildAnimals_003Ed__94(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public float TreeDensity
	{
		get
		{
			float num = 0f;
			float num2 = 0f;
			CachePlantCommonalitiesIfShould();
			foreach (KeyValuePair<ThingDef, float> cachedPlantCommonality in cachedPlantCommonalities)
			{
				num += cachedPlantCommonality.Value;
				if (cachedPlantCommonality.Key.plant.IsTree)
				{
					num2 += cachedPlantCommonality.Value;
				}
			}
			if (num == 0f)
			{
				return 0f;
			}
			return num2 / num * plantDensity;
		}
	}

	public int TreeSightingsPerHourFromCaravan => Mathf.FloorToInt(TreeDensity * 25f);

	public TerrainDef TerrainForAffordance(TerrainAffordanceDef affordance)
	{
		foreach (TerrainThreshold item in terrainsByFertility)
		{
			if (item.terrain.affordances.Contains(affordance))
			{
				return item.terrain;
			}
		}
		return TerrainDefOf.Soil;
	}

	public float CommonalityOfAnimal(PawnKindDef animalDef)
	{
		if (cachedAnimalCommonalities == null)
		{
			cachedAnimalCommonalities = new Dictionary<PawnKindDef, float>();
			for (int i = 0; i < wildAnimals.Count; i++)
			{
				cachedAnimalCommonalities.Add(wildAnimals[i].animal, wildAnimals[i].commonality);
			}
			foreach (PawnKindDef allDef in DefDatabase<PawnKindDef>.AllDefs)
			{
				if (allDef.RaceProps.wildBiomes == null)
				{
					continue;
				}
				for (int j = 0; j < allDef.RaceProps.wildBiomes.Count; j++)
				{
					if (allDef.RaceProps.wildBiomes[j].biome == this)
					{
						cachedAnimalCommonalities.Add(allDef, allDef.RaceProps.wildBiomes[j].commonality);
					}
				}
			}
		}
		if (cachedAnimalCommonalities.TryGetValue(animalDef, out var value))
		{
			return value;
		}
		return 0f;
	}

	public float CommonalityOfPollutionAnimal(PawnKindDef animalDef)
	{
		if (!ModsConfig.BiotechActive)
		{
			return 0f;
		}
		if (cachedPollutionAnimalCommonalities == null)
		{
			cachedPollutionAnimalCommonalities = new Dictionary<PawnKindDef, float>();
			for (int i = 0; i < pollutionWildAnimals.Count; i++)
			{
				cachedPollutionAnimalCommonalities.Add(pollutionWildAnimals[i].animal, pollutionWildAnimals[i].commonality);
			}
		}
		if (cachedPollutionAnimalCommonalities.TryGetValue(animalDef, out var value))
		{
			return value;
		}
		return 0f;
	}

	public float CommonalityOfCoastalAnimal(PawnKindDef animalDef)
	{
		if (!ModsConfig.OdysseyActive)
		{
			return 0f;
		}
		if (cachedCoastalAnimalCommonalities == null)
		{
			cachedCoastalAnimalCommonalities = new Dictionary<PawnKindDef, float>();
			for (int i = 0; i < coastalWildAnimals.Count; i++)
			{
				cachedCoastalAnimalCommonalities.Add(coastalWildAnimals[i].animal, coastalWildAnimals[i].commonality);
			}
		}
		if (cachedCoastalAnimalCommonalities.TryGetValue(animalDef, out var value))
		{
			return value;
		}
		return 0f;
	}

	public bool ShouldSpawnAnimalOnCoast(PawnKindDef animalDef)
	{
		return CommonalityOfCoastalAnimal(animalDef) > CommonalityOfAnimal(animalDef);
	}

	public float CommonalityOfPlant(ThingDef plantDef)
	{
		CachePlantCommonalitiesIfShould();
		if (cachedPlantCommonalities.TryGetValue(plantDef, out var value))
		{
			return value;
		}
		return 0f;
	}

	public float CommonalityOfDisease(IncidentDef diseaseInc)
	{
		if (cachedDiseaseCommonalities == null)
		{
			cachedDiseaseCommonalities = new Dictionary<IncidentDef, float>();
			for (int i = 0; i < diseases.Count; i++)
			{
				cachedDiseaseCommonalities.Add(diseases[i].diseaseInc, diseases[i].commonality);
			}
			foreach (IncidentDef allDef in DefDatabase<IncidentDef>.AllDefs)
			{
				if (allDef.diseaseBiomeRecords == null)
				{
					continue;
				}
				for (int j = 0; j < allDef.diseaseBiomeRecords.Count; j++)
				{
					if (allDef.diseaseBiomeRecords[j].biome == this)
					{
						cachedDiseaseCommonalities.Add(allDef.diseaseBiomeRecords[j].diseaseInc, allDef.diseaseBiomeRecords[j].commonality);
					}
				}
			}
		}
		if (cachedDiseaseCommonalities.TryGetValue(diseaseInc, out var value))
		{
			return value;
		}
		return 0f;
	}

	private void CachePlantCommonalitiesIfShould()
	{
		if (cachedPlantCommonalities != null)
		{
			return;
		}
		cachedPlantCommonalities = new Dictionary<ThingDef, float>();
		for (int i = 0; i < wildPlants.Count; i++)
		{
			if (wildPlants[i].plant != null)
			{
				cachedPlantCommonalities.Add(wildPlants[i].plant, wildPlants[i].commonality);
			}
		}
		foreach (ThingDef allDef in DefDatabase<ThingDef>.AllDefs)
		{
			if (allDef.plant == null || allDef.plant.wildBiomes == null)
			{
				continue;
			}
			for (int j = 0; j < allDef.plant.wildBiomes.Count; j++)
			{
				if (allDef.plant.wildBiomes[j].biome == this)
				{
					if (cachedPlantCommonalities.ContainsKey(allDef))
					{
						cachedPlantCommonalities[allDef] = (cachedPlantCommonalities[allDef] + allDef.plant.wildBiomes[j].commonality) / 2f;
					}
					else
					{
						cachedPlantCommonalities.Add(allDef, allDef.plant.wildBiomes[j].commonality);
					}
				}
			}
		}
	}

	public bool IsPackAnimalAllowed(ThingDef pawn)
	{
		return allowedPackAnimals.Contains(pawn);
	}

	public static BiomeDef Named(string defName)
	{
		return DefDatabase<BiomeDef>.GetNamed(defName);
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__109))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__109(-2)
		{
			_003C_003E4__this = this
		};
	}
}
