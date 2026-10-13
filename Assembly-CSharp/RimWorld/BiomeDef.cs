using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A00 RID: 6656
	public class BiomeDef : Def
	{
		// Token: 0x170016DF RID: 5855
		// (get) Token: 0x06009620 RID: 38432 RVA: 0x002BF07C File Offset: 0x002BD27C
		public BiomeWorker Worker
		{
			get
			{
				BiomeWorker result;
				if ((result = this.worker) == null)
				{
					result = (this.worker = GenWorker<BiomeWorker>.Get(this.workerClass));
				}
				return result;
			}
		}

		// Token: 0x170016E0 RID: 5856
		// (get) Token: 0x06009621 RID: 38433 RVA: 0x002BF0A8 File Offset: 0x002BD2A8
		public Material DrawMaterial
		{
			get
			{
				if (this.cachedMat == null)
				{
					if (this.texture.NullOrEmpty())
					{
						return null;
					}
					if (this == BiomeDefOf.Ocean || this == BiomeDefOf.Lake)
					{
						this.cachedMat = MaterialAllocator.Create(WorldMaterials.WorldOcean);
					}
					else if (!this.allowRoads && !this.allowRivers)
					{
						this.cachedMat = MaterialAllocator.Create(WorldMaterials.WorldIce);
					}
					else
					{
						this.cachedMat = MaterialAllocator.Create(WorldMaterials.WorldTerrain);
					}
					this.cachedMat.mainTexture = ContentFinder<Texture2D>.Get(this.texture, true);
				}
				return this.cachedMat;
			}
		}

		// Token: 0x170016E1 RID: 5857
		// (get) Token: 0x06009622 RID: 38434 RVA: 0x002BF144 File Offset: 0x002BD344
		public List<ThingDef> AllWildPlants
		{
			get
			{
				if (this.cachedWildPlants == null)
				{
					this.cachedWildPlants = new List<ThingDef>();
					foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
					{
						if (thingDef.category == ThingCategory.Plant && this.CommonalityOfPlant(thingDef) > 0f)
						{
							this.cachedWildPlants.Add(thingDef);
						}
					}
				}
				return this.cachedWildPlants;
			}
		}

		// Token: 0x170016E2 RID: 5858
		// (get) Token: 0x06009623 RID: 38435 RVA: 0x002BF1CC File Offset: 0x002BD3CC
		public int MaxWildAndCavePlantsClusterRadius
		{
			get
			{
				if (this.cachedMaxWildPlantsClusterRadius == null)
				{
					this.cachedMaxWildPlantsClusterRadius = new int?(0);
					List<ThingDef> allWildPlants = this.AllWildPlants;
					for (int i = 0; i < allWildPlants.Count; i++)
					{
						if (allWildPlants[i].plant.GrowsInClusters)
						{
							this.cachedMaxWildPlantsClusterRadius = new int?(Mathf.Max(this.cachedMaxWildPlantsClusterRadius.Value, allWildPlants[i].plant.wildClusterRadius));
						}
					}
					List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int j = 0; j < allDefsListForReading.Count; j++)
					{
						if (allDefsListForReading[j].category == ThingCategory.Plant && allDefsListForReading[j].plant.cavePlant)
						{
							this.cachedMaxWildPlantsClusterRadius = new int?(Mathf.Max(this.cachedMaxWildPlantsClusterRadius.Value, allDefsListForReading[j].plant.wildClusterRadius));
						}
					}
				}
				return this.cachedMaxWildPlantsClusterRadius.Value;
			}
		}

		// Token: 0x170016E3 RID: 5859
		// (get) Token: 0x06009624 RID: 38436 RVA: 0x002BF2BC File Offset: 0x002BD4BC
		public float LowestWildAndCavePlantOrder
		{
			get
			{
				if (this.cachedLowestWildPlantOrder == null)
				{
					this.cachedLowestWildPlantOrder = new float?((float)int.MaxValue);
					List<ThingDef> allWildPlants = this.AllWildPlants;
					for (int i = 0; i < allWildPlants.Count; i++)
					{
						this.cachedLowestWildPlantOrder = new float?(Mathf.Min(this.cachedLowestWildPlantOrder.Value, allWildPlants[i].plant.wildOrder));
					}
					List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int j = 0; j < allDefsListForReading.Count; j++)
					{
						if (allDefsListForReading[j].category == ThingCategory.Plant && allDefsListForReading[j].plant.cavePlant)
						{
							this.cachedLowestWildPlantOrder = new float?(Mathf.Min(this.cachedLowestWildPlantOrder.Value, allDefsListForReading[j].plant.wildOrder));
						}
					}
				}
				return this.cachedLowestWildPlantOrder.Value;
			}
		}

		// Token: 0x170016E4 RID: 5860
		// (get) Token: 0x06009625 RID: 38437 RVA: 0x002BF39E File Offset: 0x002BD59E
		public IEnumerable<PawnKindDef> AllWildAnimals
		{
			get
			{
				foreach (PawnKindDef pawnKindDef in DefDatabase<PawnKindDef>.AllDefs)
				{
					if (this.CommonalityOfAnimal(pawnKindDef) > 0f || this.CommonalityOfPollutionAnimal(pawnKindDef) > 0f || this.CommonalityOfCoastalAnimal(pawnKindDef) > 0f)
					{
						yield return pawnKindDef;
					}
				}
				IEnumerator<PawnKindDef> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x170016E5 RID: 5861
		// (get) Token: 0x06009626 RID: 38438 RVA: 0x002BF3B0 File Offset: 0x002BD5B0
		public float TreeDensity
		{
			get
			{
				float num = 0f;
				float num2 = 0f;
				this.CachePlantCommonalitiesIfShould();
				foreach (KeyValuePair<ThingDef, float> keyValuePair in this.cachedPlantCommonalities)
				{
					num += keyValuePair.Value;
					if (keyValuePair.Key.plant.IsTree)
					{
						num2 += keyValuePair.Value;
					}
				}
				if (num == 0f)
				{
					return 0f;
				}
				return num2 / num * this.plantDensity;
			}
		}

		// Token: 0x170016E6 RID: 5862
		// (get) Token: 0x06009627 RID: 38439 RVA: 0x002BF450 File Offset: 0x002BD650
		public int TreeSightingsPerHourFromCaravan
		{
			get
			{
				return Mathf.FloorToInt(this.TreeDensity * 25f);
			}
		}

		// Token: 0x06009628 RID: 38440 RVA: 0x002BF464 File Offset: 0x002BD664
		public TerrainDef TerrainForAffordance(TerrainAffordanceDef affordance)
		{
			foreach (TerrainThreshold terrainThreshold in this.terrainsByFertility)
			{
				if (terrainThreshold.terrain.affordances.Contains(affordance))
				{
					return terrainThreshold.terrain;
				}
			}
			return TerrainDefOf.Soil;
		}

		// Token: 0x06009629 RID: 38441 RVA: 0x002BF4D4 File Offset: 0x002BD6D4
		public float CommonalityOfAnimal(PawnKindDef animalDef)
		{
			if (this.cachedAnimalCommonalities == null)
			{
				this.cachedAnimalCommonalities = new Dictionary<PawnKindDef, float>();
				for (int i = 0; i < this.wildAnimals.Count; i++)
				{
					this.cachedAnimalCommonalities.Add(this.wildAnimals[i].animal, this.wildAnimals[i].commonality);
				}
				foreach (PawnKindDef pawnKindDef in DefDatabase<PawnKindDef>.AllDefs)
				{
					if (pawnKindDef.RaceProps.wildBiomes != null)
					{
						for (int j = 0; j < pawnKindDef.RaceProps.wildBiomes.Count; j++)
						{
							if (pawnKindDef.RaceProps.wildBiomes[j].biome == this)
							{
								this.cachedAnimalCommonalities.Add(pawnKindDef, pawnKindDef.RaceProps.wildBiomes[j].commonality);
							}
						}
					}
				}
			}
			float result;
			if (this.cachedAnimalCommonalities.TryGetValue(animalDef, out result))
			{
				return result;
			}
			return 0f;
		}

		// Token: 0x0600962A RID: 38442 RVA: 0x002BF5F4 File Offset: 0x002BD7F4
		public float CommonalityOfPollutionAnimal(PawnKindDef animalDef)
		{
			if (!ModsConfig.BiotechActive)
			{
				return 0f;
			}
			if (this.cachedPollutionAnimalCommonalities == null)
			{
				this.cachedPollutionAnimalCommonalities = new Dictionary<PawnKindDef, float>();
				for (int i = 0; i < this.pollutionWildAnimals.Count; i++)
				{
					this.cachedPollutionAnimalCommonalities.Add(this.pollutionWildAnimals[i].animal, this.pollutionWildAnimals[i].commonality);
				}
			}
			float result;
			if (this.cachedPollutionAnimalCommonalities.TryGetValue(animalDef, out result))
			{
				return result;
			}
			return 0f;
		}

		// Token: 0x0600962B RID: 38443 RVA: 0x002BF67C File Offset: 0x002BD87C
		public float CommonalityOfCoastalAnimal(PawnKindDef animalDef)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return 0f;
			}
			if (this.cachedCoastalAnimalCommonalities == null)
			{
				this.cachedCoastalAnimalCommonalities = new Dictionary<PawnKindDef, float>();
				for (int i = 0; i < this.coastalWildAnimals.Count; i++)
				{
					this.cachedCoastalAnimalCommonalities.Add(this.coastalWildAnimals[i].animal, this.coastalWildAnimals[i].commonality);
				}
			}
			float result;
			if (this.cachedCoastalAnimalCommonalities.TryGetValue(animalDef, out result))
			{
				return result;
			}
			return 0f;
		}

		// Token: 0x0600962C RID: 38444 RVA: 0x002BF703 File Offset: 0x002BD903
		public bool ShouldSpawnAnimalOnCoast(PawnKindDef animalDef)
		{
			return this.CommonalityOfCoastalAnimal(animalDef) > this.CommonalityOfAnimal(animalDef);
		}

		// Token: 0x0600962D RID: 38445 RVA: 0x002BF718 File Offset: 0x002BD918
		public float CommonalityOfPlant(ThingDef plantDef)
		{
			this.CachePlantCommonalitiesIfShould();
			float result;
			if (this.cachedPlantCommonalities.TryGetValue(plantDef, out result))
			{
				return result;
			}
			return 0f;
		}

		// Token: 0x0600962E RID: 38446 RVA: 0x002BF744 File Offset: 0x002BD944
		public float CommonalityOfDisease(IncidentDef diseaseInc)
		{
			if (this.cachedDiseaseCommonalities == null)
			{
				this.cachedDiseaseCommonalities = new Dictionary<IncidentDef, float>();
				for (int i = 0; i < this.diseases.Count; i++)
				{
					this.cachedDiseaseCommonalities.Add(this.diseases[i].diseaseInc, this.diseases[i].commonality);
				}
				foreach (IncidentDef incidentDef in DefDatabase<IncidentDef>.AllDefs)
				{
					if (incidentDef.diseaseBiomeRecords != null)
					{
						for (int j = 0; j < incidentDef.diseaseBiomeRecords.Count; j++)
						{
							if (incidentDef.diseaseBiomeRecords[j].biome == this)
							{
								this.cachedDiseaseCommonalities.Add(incidentDef.diseaseBiomeRecords[j].diseaseInc, incidentDef.diseaseBiomeRecords[j].commonality);
							}
						}
					}
				}
			}
			float result;
			if (this.cachedDiseaseCommonalities.TryGetValue(diseaseInc, out result))
			{
				return result;
			}
			return 0f;
		}

		// Token: 0x0600962F RID: 38447 RVA: 0x002BF860 File Offset: 0x002BDA60
		private void CachePlantCommonalitiesIfShould()
		{
			if (this.cachedPlantCommonalities == null)
			{
				this.cachedPlantCommonalities = new Dictionary<ThingDef, float>();
				for (int i = 0; i < this.wildPlants.Count; i++)
				{
					if (this.wildPlants[i].plant != null)
					{
						this.cachedPlantCommonalities.Add(this.wildPlants[i].plant, this.wildPlants[i].commonality);
					}
				}
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
				{
					if (thingDef.plant != null && thingDef.plant.wildBiomes != null)
					{
						for (int j = 0; j < thingDef.plant.wildBiomes.Count; j++)
						{
							if (thingDef.plant.wildBiomes[j].biome == this)
							{
								this.cachedPlantCommonalities.Add(thingDef, thingDef.plant.wildBiomes[j].commonality);
							}
						}
					}
				}
			}
		}

		// Token: 0x06009630 RID: 38448 RVA: 0x002BF97C File Offset: 0x002BDB7C
		public bool IsPackAnimalAllowed(ThingDef pawn)
		{
			return this.allowedPackAnimals.Contains(pawn);
		}

		// Token: 0x06009631 RID: 38449 RVA: 0x002BF98A File Offset: 0x002BDB8A
		public static BiomeDef Named(string defName)
		{
			return DefDatabase<BiomeDef>.GetNamed(defName, true);
		}

		// Token: 0x06009632 RID: 38450 RVA: 0x002BF993 File Offset: 0x002BDB93
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (Prefs.DevMode)
			{
				using (List<BiomeAnimalRecord>.Enumerator enumerator2 = this.wildAnimals.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						BiomeAnimalRecord wa = enumerator2.Current;
						if (this.wildAnimals.Count((BiomeAnimalRecord a) => a.animal == wa.animal) > 1)
						{
							yield return "Duplicate animal record: " + wa.animal.defName;
						}
					}
				}
				List<BiomeAnimalRecord>.Enumerator enumerator2 = default(List<BiomeAnimalRecord>.Enumerator);
				if (ModsConfig.BiotechActive)
				{
					using (List<BiomeAnimalRecord>.Enumerator enumerator2 = this.pollutionWildAnimals.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							BiomeAnimalRecord pa = enumerator2.Current;
							if (this.pollutionWildAnimals.Count((BiomeAnimalRecord a) => a.animal == pa.animal) > 1)
							{
								yield return "Duplicate pollution animal record: " + pa.animal.defName;
							}
						}
					}
					enumerator2 = default(List<BiomeAnimalRecord>.Enumerator);
				}
				if (ModsConfig.OdysseyActive)
				{
					using (List<BiomeAnimalRecord>.Enumerator enumerator2 = this.coastalWildAnimals.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							BiomeAnimalRecord ca = enumerator2.Current;
							if (this.coastalWildAnimals.Count((BiomeAnimalRecord a) => a.animal == ca.animal) > 1)
							{
								yield return "Duplicate coastal animal record: " + ca.animal.defName;
							}
						}
					}
					enumerator2 = default(List<BiomeAnimalRecord>.Enumerator);
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x04006230 RID: 25136
		public Type workerClass;

		// Token: 0x04006231 RID: 25137
		public bool implemented = true;

		// Token: 0x04006232 RID: 25138
		public bool generatesNaturally = true;

		// Token: 0x04006233 RID: 25139
		public bool canBuildBase = true;

		// Token: 0x04006234 RID: 25140
		public bool canAutoChoose = true;

		// Token: 0x04006235 RID: 25141
		public bool allowRoads = true;

		// Token: 0x04006236 RID: 25142
		public bool allowRivers = true;

		// Token: 0x04006237 RID: 25143
		public bool allowFarmingCamps = true;

		// Token: 0x04006238 RID: 25144
		public float animalDensity;

		// Token: 0x04006239 RID: 25145
		public float plantDensity;

		// Token: 0x0400623A RID: 25146
		public float diseaseMtbDays = 60f;

		// Token: 0x0400623B RID: 25147
		public float settlementSelectionWeight = 1f;

		// Token: 0x0400623C RID: 25148
		public float campSelectionWeight = 1f;

		// Token: 0x0400623D RID: 25149
		public float pollutionOffset;

		// Token: 0x0400623E RID: 25150
		public bool impassable;

		// Token: 0x0400623F RID: 25151
		public bool hasVirtualPlants = true;

		// Token: 0x04006240 RID: 25152
		public float forageability;

		// Token: 0x04006241 RID: 25153
		public ThingDef foragedFood;

		// Token: 0x04006242 RID: 25154
		public bool wildPlantsCareAboutLocalFertility = true;

		// Token: 0x04006243 RID: 25155
		public bool wildPlantsAreCavePlants;

		// Token: 0x04006244 RID: 25156
		public float wildPlantRegrowDays = 25f;

		// Token: 0x04006245 RID: 25157
		public float movementDifficulty = 1f;

		// Token: 0x04006246 RID: 25158
		public List<WeatherCommonalityRecord> baseWeatherCommonalities = new List<WeatherCommonalityRecord>();

		// Token: 0x04006247 RID: 25159
		public List<TerrainThreshold> terrainsByFertility = new List<TerrainThreshold>();

		// Token: 0x04006248 RID: 25160
		public List<SoundDef> soundsAmbient = new List<SoundDef>();

		// Token: 0x04006249 RID: 25161
		public List<TerrainPatchMaker> terrainPatchMakers = new List<TerrainPatchMaker>();

		// Token: 0x0400624A RID: 25162
		public List<BiomePlantRecord> wildPlants = new List<BiomePlantRecord>();

		// Token: 0x0400624B RID: 25163
		private List<BiomeAnimalRecord> wildAnimals = new List<BiomeAnimalRecord>();

		// Token: 0x0400624C RID: 25164
		private List<BiomeAnimalRecord> pollutionWildAnimals = new List<BiomeAnimalRecord>();

		// Token: 0x0400624D RID: 25165
		private List<BiomeAnimalRecord> coastalWildAnimals = new List<BiomeAnimalRecord>();

		// Token: 0x0400624E RID: 25166
		private List<BiomeDiseaseRecord> diseases = new List<BiomeDiseaseRecord>();

		// Token: 0x0400624F RID: 25167
		private List<ThingDef> allowedPackAnimals = new List<ThingDef>();

		// Token: 0x04006250 RID: 25168
		public bool hasBedrock = true;

		// Token: 0x04006251 RID: 25169
		public bool isExtremeBiome;

		// Token: 0x04006252 RID: 25170
		public bool isWaterBiome;

		// Token: 0x04006253 RID: 25171
		public bool allowPollution = true;

		// Token: 0x04006254 RID: 25172
		public bool wildAnimalsCanWanderInto = true;

		// Token: 0x04006255 RID: 25173
		public bool noAmbientWind;

		// Token: 0x04006256 RID: 25174
		public bool inVacuum;

		// Token: 0x04006257 RID: 25175
		public bool disableSkyLighting;

		// Token: 0x04006258 RID: 25176
		public bool disableShadows;

		// Token: 0x04006259 RID: 25177
		public bool noGravel;

		// Token: 0x0400625A RID: 25178
		public bool canExitMap = true;

		// Token: 0x0400625B RID: 25179
		public bool onlyAllowWhitelistedArrivalModes;

		// Token: 0x0400625C RID: 25180
		public bool isBackgroundBiome;

		// Token: 0x0400625D RID: 25181
		public float wildAnimalScariaChance;

		// Token: 0x0400625E RID: 25182
		public float geyserCountFactor = 1f;

		// Token: 0x0400625F RID: 25183
		public float? constantOutdoorTemperature;

		// Token: 0x04006260 RID: 25184
		public TerrainDef coastalBeachTerrain;

		// Token: 0x04006261 RID: 25185
		public TerrainDef lakeBeachTerrain;

		// Token: 0x04006262 RID: 25186
		public TerrainDef riverbankTerrain;

		// Token: 0x04006263 RID: 25187
		public TerrainDef mudTerrain;

		// Token: 0x04006264 RID: 25188
		public TerrainDef gravelTerrain;

		// Token: 0x04006265 RID: 25189
		public TerrainDef waterShallowTerrain;

		// Token: 0x04006266 RID: 25190
		public TerrainDef waterDeepTerrain;

		// Token: 0x04006267 RID: 25191
		public TerrainDef oceanShallowTerrain;

		// Token: 0x04006268 RID: 25192
		public TerrainDef oceanDeepTerrain;

		// Token: 0x04006269 RID: 25193
		public TerrainDef waterMovingShallowTerrain;

		// Token: 0x0400626A RID: 25194
		public TerrainDef waterMovingChestDeepTerrain;

		// Token: 0x0400626B RID: 25195
		public IntRange riverbankSizeRange;

		// Token: 0x0400626C RID: 25196
		public List<GameConditionDef> biomeMapConditions = new List<GameConditionDef>();

		// Token: 0x0400626D RID: 25197
		public List<GenStepDef> extraGenSteps = new List<GenStepDef>();

		// Token: 0x0400626E RID: 25198
		public List<GenStepDef> preventGenSteps = new List<GenStepDef>();

		// Token: 0x0400626F RID: 25199
		public List<ThingDef> extraRockTypes;

		// Token: 0x04006270 RID: 25200
		public List<ThingDef> forceRockTypes;

		// Token: 0x04006271 RID: 25201
		public float maxFishPopulation;

		// Token: 0x04006272 RID: 25202
		public BiomeFishTypes fishTypes;

		// Token: 0x04006273 RID: 25203
		public List<PlanetLayerDef> layerWhitelist;

		// Token: 0x04006274 RID: 25204
		public List<PlanetLayerDef> layerBlacklist;

		// Token: 0x04006275 RID: 25205
		[MustTranslate]
		public string settleWarning;

		// Token: 0x04006276 RID: 25206
		public Color? fogOfWarColor;

		// Token: 0x04006277 RID: 25207
		public OrbitalDebrisDef orbitalDebris;

		// Token: 0x04006278 RID: 25208
		[NoTranslate]
		public string texture;

		// Token: 0x04006279 RID: 25209
		[Unsaved(false)]
		private Dictionary<PawnKindDef, float> cachedAnimalCommonalities;

		// Token: 0x0400627A RID: 25210
		[Unsaved(false)]
		private Dictionary<PawnKindDef, float> cachedPollutionAnimalCommonalities;

		// Token: 0x0400627B RID: 25211
		[Unsaved(false)]
		private Dictionary<PawnKindDef, float> cachedCoastalAnimalCommonalities;

		// Token: 0x0400627C RID: 25212
		[Unsaved(false)]
		private Dictionary<ThingDef, float> cachedPlantCommonalities;

		// Token: 0x0400627D RID: 25213
		[Unsaved(false)]
		private Dictionary<IncidentDef, float> cachedDiseaseCommonalities;

		// Token: 0x0400627E RID: 25214
		[Unsaved(false)]
		private Material cachedMat;

		// Token: 0x0400627F RID: 25215
		[Unsaved(false)]
		private List<ThingDef> cachedWildPlants;

		// Token: 0x04006280 RID: 25216
		[Unsaved(false)]
		private float? cachedLowestWildPlantOrder;

		// Token: 0x04006281 RID: 25217
		[Unsaved(false)]
		private int? cachedMaxWildPlantsClusterRadius;

		// Token: 0x04006282 RID: 25218
		private BiomeWorker worker;
	}
}
