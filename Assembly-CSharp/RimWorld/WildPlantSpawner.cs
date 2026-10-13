using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020021C1 RID: 8641
	public class WildPlantSpawner : IExposable
	{
		// Token: 0x17001CAD RID: 7341
		// (get) Token: 0x0600BCA0 RID: 48288 RVA: 0x0036AD12 File Offset: 0x00368F12
		public float CurrentPlantDensityFactor
		{
			get
			{
				return this.map.TileInfo.PlantDensityFactor * this.map.gameConditionManager.AggregatePlantDensityFactor(this.map);
			}
		}

		// Token: 0x17001CAE RID: 7342
		// (get) Token: 0x0600BCA1 RID: 48289 RVA: 0x0036AD3C File Offset: 0x00368F3C
		public float CurrentWholeMapNumDesiredPlants
		{
			get
			{
				CellRect cellRect = CellRect.WholeMap(this.map);
				float currentPlantDensityFactor = this.CurrentPlantDensityFactor;
				float num = 0f;
				foreach (IntVec3 forCell in cellRect)
				{
					num += this.GetDesiredPlantsCountAt(forCell, currentPlantDensityFactor);
				}
				return num;
			}
		}

		// Token: 0x17001CAF RID: 7343
		// (get) Token: 0x0600BCA2 RID: 48290 RVA: 0x0036ADAC File Offset: 0x00368FAC
		public int CurrentWholeMapNumNonZeroFertilityCells
		{
			get
			{
				CellRect cellRect = CellRect.WholeMap(this.map);
				int num = 0;
				using (CellRect.Enumerator enumerator = cellRect.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.GetFertility(this.map) > 0f)
						{
							num++;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x17001CB0 RID: 7344
		// (get) Token: 0x0600BCA3 RID: 48291 RVA: 0x0036AE18 File Offset: 0x00369018
		public float CachedChanceFromDensity
		{
			get
			{
				this.CacheWholeMapNumDesiredPlants();
				return this.calculatedWholeMapNumDesiredPlants / (float)this.calculatedWholeMapNumNonZeroFertilityCells;
			}
		}

		// Token: 0x17001CB1 RID: 7345
		// (get) Token: 0x0600BCA4 RID: 48292 RVA: 0x0036AE30 File Offset: 0x00369030
		public SimpleCurve OverrideDensityForFertilityCurve
		{
			get
			{
				if (this.cachedOverrideDensityForFertilityCurve != null)
				{
					return this.cachedOverrideDensityForFertilityCurve;
				}
				this.cachedOverrideDensityForFertilityCurve = new SimpleCurve
				{
					{
						new CurvePoint(0f, -1f),
						true
					}
				};
				foreach (TileMutatorDef tileMutatorDef in this.map.TileInfo.Mutators)
				{
					if (tileMutatorDef.overrideDensityForFertilityCurve != null)
					{
						this.cachedOverrideDensityForFertilityCurve = tileMutatorDef.overrideDensityForFertilityCurve;
					}
				}
				return this.cachedOverrideDensityForFertilityCurve;
			}
		}

		// Token: 0x17001CB2 RID: 7346
		// (get) Token: 0x0600BCA5 RID: 48293 RVA: 0x0036AECC File Offset: 0x003690CC
		public List<ThingDef> AllWildPlants
		{
			get
			{
				if (this.cachedWildPlants != null)
				{
					return this.cachedWildPlants;
				}
				this.cachedWildPlants = new List<ThingDef>();
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
				{
					if (thingDef.category == ThingCategory.Plant && this.GetCommonalityOfPlant(thingDef) > 0f)
					{
						this.cachedWildPlants.Add(thingDef);
					}
				}
				return this.cachedWildPlants;
			}
		}

		// Token: 0x17001CB3 RID: 7347
		// (get) Token: 0x0600BCA6 RID: 48294 RVA: 0x0036AF5C File Offset: 0x0036915C
		public List<ThingDef> MutatorWildPlants
		{
			get
			{
				if (this.cachedMutatorWildPlants != null)
				{
					return this.cachedMutatorWildPlants;
				}
				this.cachedMutatorWildPlants = new List<ThingDef>();
				foreach (TileMutatorDef tileMutatorDef in this.map.TileInfo.Mutators)
				{
					TileMutatorWorker worker = tileMutatorDef.Worker;
					foreach (BiomePlantRecord biomePlantRecord in (((worker != null) ? worker.AdditionalWildPlants(this.map.Tile) : null) ?? Enumerable.Empty<BiomePlantRecord>()))
					{
						if (!this.cachedMutatorWildPlants.Contains(biomePlantRecord.plant))
						{
							this.cachedMutatorWildPlants.Add(biomePlantRecord.plant);
						}
					}
				}
				return this.cachedMutatorWildPlants;
			}
		}

		// Token: 0x17001CB4 RID: 7348
		// (get) Token: 0x0600BCA7 RID: 48295 RVA: 0x0036B044 File Offset: 0x00369244
		private bool HaveAnyPlantsWhichIgnoreFertility
		{
			get
			{
				if (this.cachedHaveAnyPlantsWhichIgnoreFertility == null)
				{
					this.cachedHaveAnyPlantsWhichIgnoreFertility = new bool?(false);
					bool? flag = this.cachedHaveAnyPlantsWhichIgnoreFertility;
					this.cachedHaveAnyPlantsWhichIgnoreFertility = (flag | this.AllWildPlants.Any((ThingDef p) => p.plant.completelyIgnoreFertility));
				}
				return this.cachedHaveAnyPlantsWhichIgnoreFertility.Value;
			}
		}

		// Token: 0x17001CB5 RID: 7349
		// (get) Token: 0x0600BCA8 RID: 48296 RVA: 0x0036B0B8 File Offset: 0x003692B8
		public float PlantsCommonalitiesSum
		{
			get
			{
				if (this.cachedPlantsCommonalitiesSum == null)
				{
					this.cachedPlantsCommonalitiesSum = new float?(0f);
					for (int i = 0; i < this.AllWildPlants.Count; i++)
					{
						this.cachedPlantsCommonalitiesSum += this.GetCommonalityOfPlant(this.AllWildPlants[i]);
					}
				}
				return this.cachedPlantsCommonalitiesSum.Value;
			}
		}

		// Token: 0x17001CB6 RID: 7350
		// (get) Token: 0x0600BCA9 RID: 48297 RVA: 0x0036B148 File Offset: 0x00369348
		public float CavePlantsCommonalitiesSum
		{
			get
			{
				if (this.cachedCavePlantsCommonalitiesSum == null)
				{
					this.cachedCavePlantsCommonalitiesSum = new float?(0f);
					for (int i = 0; i < WildPlantSpawner.allCavePlants.Count; i++)
					{
						this.cachedCavePlantsCommonalitiesSum += this.GetCommonalityOfPlant(WildPlantSpawner.allCavePlants[i]);
					}
				}
				return this.cachedCavePlantsCommonalitiesSum.Value;
			}
		}

		// Token: 0x0600BCAA RID: 48298 RVA: 0x0036B1D3 File Offset: 0x003693D3
		public WildPlantSpawner(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600BCAB RID: 48299 RVA: 0x0036B1E2 File Offset: 0x003693E2
		public static void ResetStaticData()
		{
			WildPlantSpawner.allCavePlants.Clear();
			WildPlantSpawner.allCavePlants.AddRange(from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.category == ThingCategory.Plant && x.plant.cavePlant && x.plant.cavePlantWeight > 0f
			select x);
		}

		// Token: 0x0600BCAC RID: 48300 RVA: 0x0036B224 File Offset: 0x00369424
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.cycleIndex, "cycleIndex", 0, false);
			Scribe_Values.Look<float>(ref this.calculatedWholeMapNumDesiredPlants, "calculatedWholeMapNumDesiredPlants", 0f, false);
			Scribe_Values.Look<float>(ref this.calculatedWholeMapNumDesiredPlantsTmp, "calculatedWholeMapNumDesiredPlantsTmp", 0f, false);
			Scribe_Values.Look<bool>(ref this.hasWholeMapNumDesiredPlantsCalculated, "hasWholeMapNumDesiredPlantsCalculated", true, false);
			Scribe_Values.Look<int>(ref this.calculatedWholeMapNumNonZeroFertilityCells, "calculatedWholeMapNumNonZeroFertilityCells", 0, false);
			Scribe_Values.Look<int>(ref this.calculatedWholeMapNumNonZeroFertilityCellsTmp, "calculatedWholeMapNumNonZeroFertilityCellsTmp", 0, false);
		}

		// Token: 0x0600BCAD RID: 48301 RVA: 0x0036B2A8 File Offset: 0x003694A8
		public void WildPlantSpawnerTick()
		{
			if (DebugSettings.fastEcology || DebugSettings.fastEcologyRegrowRateOnly)
			{
				for (int i = 0; i < 2000; i++)
				{
					this.WildPlantSpawnerTickInternal();
				}
				return;
			}
			this.WildPlantSpawnerTickInternal();
		}

		// Token: 0x0600BCAE RID: 48302 RVA: 0x0036B2E0 File Offset: 0x003694E0
		private void WildPlantSpawnerTickInternal()
		{
			int area = this.map.Area;
			int num = Mathf.CeilToInt((float)area * 0.0001f);
			float currentPlantDensityFactor = this.CurrentPlantDensityFactor;
			this.CacheWholeMapNumDesiredPlants();
			int num2 = Mathf.CeilToInt(10000f);
			float cachedChanceFromDensity = this.CachedChanceFromDensity;
			for (int i = 0; i < num; i++)
			{
				if (this.cycleIndex >= area)
				{
					this.calculatedWholeMapNumDesiredPlants = this.calculatedWholeMapNumDesiredPlantsTmp;
					this.calculatedWholeMapNumDesiredPlantsTmp = 0f;
					this.calculatedWholeMapNumNonZeroFertilityCells = this.calculatedWholeMapNumNonZeroFertilityCellsTmp;
					this.calculatedWholeMapNumNonZeroFertilityCellsTmp = 0;
					this.cycleIndex = 0;
				}
				IntVec3 intVec = this.map.cellsInRandomOrder.Get(this.cycleIndex);
				this.calculatedWholeMapNumDesiredPlantsTmp += this.GetDesiredPlantsCountAt(intVec, currentPlantDensityFactor);
				if (this.map.fertilityGrid.FertilityAt(intVec) > 0f)
				{
					this.calculatedWholeMapNumNonZeroFertilityCellsTmp++;
				}
				float mtb = this.GoodRoofForCavePlant(intVec) ? 130f : this.map.BiomeAt(intVec).wildPlantRegrowDays;
				float num3 = this.OverrideDensityForFertilityCurve.Evaluate(this.map.fertilityGrid.FertilityAt(intVec));
				if (Rand.Chance((num3 > 0f) ? num3 : cachedChanceFromDensity) && Rand.MTBEventOccurs(mtb, 60000f, (float)num2) && this.CanRegrowAt(intVec))
				{
					this.CheckSpawnWildPlantAt(intVec, currentPlantDensityFactor, this.calculatedWholeMapNumDesiredPlants, false);
				}
				this.cycleIndex++;
			}
		}

		// Token: 0x0600BCAF RID: 48303 RVA: 0x0036B460 File Offset: 0x00369660
		private void CachePlantCommonalitiesIfShould()
		{
			if (this.cachedPlantCommonalities != null)
			{
				return;
			}
			this.cachedPlantCommonalities = new Dictionary<ThingDef, float>();
			foreach (BiomeDef biomeDef in this.map.Biomes)
			{
				foreach (BiomePlantRecord biomePlantRecord in biomeDef.wildPlants)
				{
					if (biomePlantRecord.plant != null)
					{
						if (this.cachedPlantCommonalities.ContainsKey(biomePlantRecord.plant))
						{
							this.cachedPlantCommonalities[biomePlantRecord.plant] = (this.cachedPlantCommonalities[biomePlantRecord.plant] + biomePlantRecord.commonality) / 2f;
						}
						else
						{
							this.cachedPlantCommonalities.Add(biomePlantRecord.plant, biomePlantRecord.commonality);
						}
					}
				}
			}
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.plant != null && thingDef.plant.wildBiomes != null)
				{
					for (int i = 0; i < thingDef.plant.wildBiomes.Count; i++)
					{
						if (this.map.Biomes.Contains(thingDef.plant.wildBiomes[i].biome))
						{
							this.cachedPlantCommonalities.Add(thingDef, thingDef.plant.wildBiomes[i].commonality);
						}
					}
				}
			}
			foreach (TileMutatorDef tileMutatorDef in this.map.TileInfo.Mutators)
			{
				TileMutatorWorker worker = tileMutatorDef.Worker;
				foreach (BiomePlantRecord biomePlantRecord2 in (((worker != null) ? worker.AdditionalWildPlants(this.map.Tile) : null) ?? Enumerable.Empty<BiomePlantRecord>()))
				{
					if (biomePlantRecord2.plant != null)
					{
						if (this.cachedPlantCommonalities.ContainsKey(biomePlantRecord2.plant))
						{
							this.cachedPlantCommonalities[biomePlantRecord2.plant] = Mathf.Max(this.cachedPlantCommonalities[biomePlantRecord2.plant], biomePlantRecord2.commonality);
						}
						else
						{
							this.cachedPlantCommonalities.Add(biomePlantRecord2.plant, biomePlantRecord2.commonality);
						}
					}
				}
			}
			foreach (ThingDef thingDef2 in this.cachedPlantCommonalities.Keys.ToList<ThingDef>())
			{
				float num = this.cachedPlantCommonalities[thingDef2];
				int num2 = this.map.Biomes.Count<BiomeDef>();
				foreach (BiomeDef biomeDef2 in this.map.Biomes)
				{
					if (thingDef2.plant.cavePlant && !biomeDef2.wildPlantsAreCavePlants)
					{
						num += thingDef2.plant.cavePlantWeight / (float)num2;
					}
					else
					{
						num += this.cachedPlantCommonalities.GetWithFallback(thingDef2, 0f) / (float)num2;
					}
					foreach (TileMutatorDef tileMutatorDef2 in this.map.TileInfo.Mutators)
					{
						if (tileMutatorDef2.Worker != null)
						{
							num *= tileMutatorDef2.Worker.PlantCommonalityFactorFor(thingDef2, this.map.Tile);
						}
					}
				}
				this.cachedPlantCommonalities[thingDef2] = num;
			}
		}

		// Token: 0x0600BCB0 RID: 48304 RVA: 0x0036B900 File Offset: 0x00369B00
		private void CacheWholeMapNumDesiredPlants()
		{
			if (!this.hasWholeMapNumDesiredPlantsCalculated)
			{
				this.calculatedWholeMapNumDesiredPlants = this.CurrentWholeMapNumDesiredPlants;
				this.calculatedWholeMapNumNonZeroFertilityCells = this.CurrentWholeMapNumNonZeroFertilityCells;
				this.hasWholeMapNumDesiredPlantsCalculated = true;
			}
		}

		// Token: 0x0600BCB1 RID: 48305 RVA: 0x0036B92C File Offset: 0x00369B2C
		public bool CheckSpawnWildPlantAt(IntVec3 c, float plantDensityFactor, float wholeMapNumDesiredPlants, bool setRandomGrowth = false)
		{
			if (plantDensityFactor <= 0f || c.GetPlant(this.map) != null || c.GetCover(this.map) != null || c.GetEdifice(this.map) != null || (!this.HaveAnyPlantsWhichIgnoreFertility && this.map.fertilityGrid.FertilityAt(c) <= 0f) || !PlantUtility.SnowAllowsPlanting(c, this.map) || !PlantUtility.SandAllowsPlanting(c, this.map))
			{
				return false;
			}
			bool cavePlants = this.GoodRoofForCavePlant(c);
			float num = this.OverrideDensityForFertilityCurve.Evaluate(this.map.fertilityGrid.FertilityAt(c));
			if (num > 0f)
			{
				if (!Rand.Chance(num))
				{
					return false;
				}
			}
			else if (this.SaturatedAt(c, plantDensityFactor, cavePlants, wholeMapNumDesiredPlants))
			{
				return false;
			}
			this.CalculatePlantsWhichCanGrowAt(c, WildPlantSpawner.tmpPossiblePlants, cavePlants, plantDensityFactor);
			if (!WildPlantSpawner.tmpPossiblePlants.Any<ThingDef>())
			{
				return false;
			}
			this.CalculateDistancesToNearbyClusters(c);
			WildPlantSpawner.tmpPossiblePlantsWithWeight.Clear();
			foreach (ThingDef thingDef in WildPlantSpawner.tmpPossiblePlants)
			{
				float value = this.PlantChoiceWeight(thingDef, c, WildPlantSpawner.distanceSqToNearbyClusters, wholeMapNumDesiredPlants, plantDensityFactor);
				WildPlantSpawner.tmpPossiblePlantsWithWeight.Add(new KeyValuePair<ThingDef, float>(thingDef, value));
			}
			KeyValuePair<ThingDef, float> keyValuePair;
			if (!WildPlantSpawner.tmpPossiblePlantsWithWeight.TryRandomElementByWeight((KeyValuePair<ThingDef, float> x) => x.Value, out keyValuePair))
			{
				return false;
			}
			if (keyValuePair.Key.plant.wildPlantUseDistanceToShore && !Rand.Chance(this.GetWaterPlantDistanceToShoreWeight(c)))
			{
				return false;
			}
			WildPlantSpawner.SpawnPlant(keyValuePair.Key, this.map, c, setRandomGrowth);
			return true;
		}

		// Token: 0x0600BCB2 RID: 48306 RVA: 0x0036BAE8 File Offset: 0x00369CE8
		public static Plant SpawnPlant(ThingDef plant, Map map, IntVec3 cell, bool setRandomGrowth)
		{
			Plant plant2 = (Plant)ThingMaker.MakeThing(plant, null);
			if (setRandomGrowth)
			{
				plant2.Growth = Mathf.Clamp01(WildPlantSpawner.InitialGrowthRandomRange.RandomInRange);
				if (plant2.def.plant.LimitedLifespan)
				{
					plant2.Age = Rand.Range(0, Mathf.Max(plant2.def.plant.LifespanTicks - 50, 0));
				}
			}
			GenSpawn.Spawn(plant2, cell, map, WipeMode.Vanish);
			return plant2;
		}

		// Token: 0x0600BCB3 RID: 48307 RVA: 0x0036BB60 File Offset: 0x00369D60
		private float PlantChoiceWeight(ThingDef plantDef, IntVec3 c, Dictionary<ThingDef, float> distanceSqToNearbyClusters, float wholeMapNumDesiredPlants, float plantDensityFactor)
		{
			float commonalityOfPlant = this.GetCommonalityOfPlant(plantDef);
			float num = this.GetCommonalityPctOfPlant(plantDef);
			if (Current.ProgramState == ProgramState.Playing)
			{
				num *= plantDef.plant.plantRespawningCommonalityFactor;
			}
			float num2 = commonalityOfPlant;
			if (num2 <= 0f)
			{
				return num2;
			}
			float num3 = 0.5f;
			if ((float)this.map.listerThings.ThingsInGroup(ThingRequestGroup.NonStumpPlant).Count > wholeMapNumDesiredPlants / 2f && (this.map.BiomeAt(c).wildPlantsAreCavePlants || !plantDef.plant.cavePlant))
			{
				num3 = (float)this.map.listerThings.ThingsOfDef(plantDef).Count / (float)this.map.listerThings.ThingsInGroup(ThingRequestGroup.NonStumpPlant).Count / num;
				num2 *= WildPlantSpawner.GlobalPctSelectionWeightBias.Evaluate(num3);
			}
			if (plantDef.plant.GrowsInClusters && num3 < 1.1f)
			{
				float num4 = (plantDef.plant.cavePlant && !this.map.BiomeAt(c).wildPlantsAreCavePlants) ? this.CavePlantsCommonalitiesSum : this.PlantsCommonalitiesSum;
				float x = commonalityOfPlant * plantDef.plant.wildClusterWeight / (num4 - commonalityOfPlant + commonalityOfPlant * plantDef.plant.wildClusterWeight);
				float num5 = 1f / (3.1415927f * (float)plantDef.plant.wildClusterRadius * (float)plantDef.plant.wildClusterRadius);
				num5 = GenMath.LerpDoubleClamped(num, 1f, 1f, num5, x);
				float f;
				if (distanceSqToNearbyClusters.TryGetValue(plantDef, out f))
				{
					float x2 = Mathf.Sqrt(f);
					num2 *= GenMath.LerpDoubleClamped((float)plantDef.plant.wildClusterRadius * 0.9f, (float)plantDef.plant.wildClusterRadius * 1.1f, plantDef.plant.wildClusterWeight, num5, x2);
				}
				else
				{
					num2 *= num5;
				}
			}
			if (plantDef.plant.wildEqualLocalDistribution)
			{
				float f2 = wholeMapNumDesiredPlants * num;
				float num6 = (float)Mathf.Max(this.map.Size.x, this.map.Size.z) / Mathf.Sqrt(f2) * 2f;
				if (plantDef.plant.GrowsInClusters)
				{
					num6 = Mathf.Max(num6, (float)plantDef.plant.wildClusterRadius * 1.6f);
				}
				num6 = Mathf.Max(num6, 7f);
				if (num6 <= 25f)
				{
					num2 *= this.LocalPlantProportionsWeightFactor(c, num, plantDensityFactor, num6, plantDef);
				}
			}
			return num2;
		}

		// Token: 0x0600BCB4 RID: 48308 RVA: 0x0036BDC4 File Offset: 0x00369FC4
		private float LocalPlantProportionsWeightFactor(IntVec3 c, float commonalityPct, float plantDensityFactor, float radiusToScan, ThingDef plantDef)
		{
			float numDesiredPlantsLocally = 0f;
			int numPlants = 0;
			int numPlantsThisDef = 0;
			RegionTraverser.BreadthFirstTraverse(c, this.map, (Region from, Region to) => c.InHorDistOf(to.extentsClose.ClosestCellTo(c), radiusToScan), delegate(Region reg)
			{
				numDesiredPlantsLocally += this.GetDesiredPlantsCountIn(reg, c, plantDensityFactor);
				numPlants += reg.ListerThings.ThingsInGroup(ThingRequestGroup.NonStumpPlant).Count;
				numPlantsThisDef += reg.ListerThings.ThingsOfDef(plantDef).Count;
				return false;
			}, 999999, RegionType.Set_Passable);
			if (numDesiredPlantsLocally * commonalityPct < 2f)
			{
				return 1f;
			}
			if ((float)numPlants <= numDesiredPlantsLocally * 0.5f)
			{
				return 1f;
			}
			float t = (float)numPlantsThisDef / (float)numPlants / commonalityPct;
			return Mathf.Lerp(7f, 1f, t);
		}

		// Token: 0x0600BCB5 RID: 48309 RVA: 0x0036BE98 File Offset: 0x0036A098
		private void CalculatePlantsWhichCanGrowAt(IntVec3 c, List<ThingDef> outPlants, bool cavePlants, float plantDensityFactor)
		{
			outPlants.Clear();
			if (cavePlants)
			{
				for (int i = 0; i < WildPlantSpawner.allCavePlants.Count; i++)
				{
					if (WildPlantSpawner.allCavePlants[i].CanEverPlantAt(c, this.map, false, true))
					{
						outPlants.Add(WildPlantSpawner.allCavePlants[i]);
					}
				}
				return;
			}
			WildPlantSpawner.tmpWildPlants.Clear();
			WildPlantSpawner.tmpWildPlants.AddRange(this.map.BiomeAt(c).AllWildPlants);
			WildPlantSpawner.tmpWildPlants.AddRange(this.MutatorWildPlants);
			foreach (ThingDef thingDef in WildPlantSpawner.tmpWildPlants)
			{
				if (!thingDef.IsDeadPlant && thingDef.CanEverPlantAt(c, this.map, false, true))
				{
					if (!Mathf.Approximately(thingDef.plant.wildOrder, this.map.BiomeAt(c).LowestWildAndCavePlantOrder))
					{
						float num = 7f;
						if (thingDef.plant.GrowsInClusters)
						{
							num = Math.Max(num, (float)thingDef.plant.wildClusterRadius * 1.5f);
						}
						if (!this.EnoughLowerOrderPlantsNearby(c, plantDensityFactor, num, thingDef))
						{
							continue;
						}
					}
					outPlants.Add(thingDef);
				}
			}
		}

		// Token: 0x0600BCB6 RID: 48310 RVA: 0x0036BFE8 File Offset: 0x0036A1E8
		private bool EnoughLowerOrderPlantsNearby(IntVec3 c, float plantDensityFactor, float radiusToScan, ThingDef plantDef)
		{
			float num = 0f;
			WildPlantSpawner.tmpPlantDefsLowerOrder.Clear();
			List<BiomePlantRecord> wildPlants = this.map.BiomeAt(c).wildPlants;
			for (int i = 0; i < wildPlants.Count; i++)
			{
				if (wildPlants[i].plant.plant.wildOrder < plantDef.plant.wildOrder)
				{
					num += this.GetCommonalityPctOfPlant(wildPlants[i].plant);
					WildPlantSpawner.tmpPlantDefsLowerOrder.Add(wildPlants[i].plant);
				}
			}
			float numDesiredPlantsLocally = 0f;
			int numPlantsLowerOrder = 0;
			RegionTraverser.BreadthFirstTraverse(c, this.map, (Region from, Region to) => c.InHorDistOf(to.extentsClose.ClosestCellTo(c), radiusToScan), delegate(Region reg)
			{
				numDesiredPlantsLocally += this.GetDesiredPlantsCountIn(reg, c, plantDensityFactor);
				for (int j = 0; j < WildPlantSpawner.tmpPlantDefsLowerOrder.Count; j++)
				{
					numPlantsLowerOrder += reg.ListerThings.ThingsOfDef(WildPlantSpawner.tmpPlantDefsLowerOrder[j]).Count;
				}
				return false;
			}, 999999, RegionType.Set_Passable);
			float num2 = numDesiredPlantsLocally * num;
			return num2 < 4f || (float)numPlantsLowerOrder / num2 >= 0.57f;
		}

		// Token: 0x0600BCB7 RID: 48311 RVA: 0x0036C110 File Offset: 0x0036A310
		private bool SaturatedAt(IntVec3 c, float plantDensityFactor, bool cavePlants, float wholeMapNumDesiredPlants)
		{
			int num = GenRadial.NumCellsInRadius(20f);
			if (wholeMapNumDesiredPlants * ((float)num / (float)this.map.Area) <= 4f || (!MapGenUtility.IsMixedBiome(this.map) && !this.map.BiomeAt(c).wildPlantsCareAboutLocalFertility))
			{
				return (float)this.map.listerThings.ThingsInGroup(ThingRequestGroup.NonStumpPlant).Count >= wholeMapNumDesiredPlants;
			}
			float numDesiredPlantsLocally = 0f;
			int numPlants = 0;
			RegionTraverser.BreadthFirstTraverse(c, this.map, (Region from, Region to) => c.InHorDistOf(to.extentsClose.ClosestCellTo(c), 20f), delegate(Region reg)
			{
				numDesiredPlantsLocally += this.GetDesiredPlantsCountIn(reg, c, plantDensityFactor);
				numPlants += reg.ListerThings.ThingsInGroup(ThingRequestGroup.NonStumpPlant).Count;
				return false;
			}, 999999, RegionType.Set_Passable);
			return (float)numPlants >= numDesiredPlantsLocally;
		}

		// Token: 0x0600BCB8 RID: 48312 RVA: 0x0036C1F8 File Offset: 0x0036A3F8
		private void CalculateDistancesToNearbyClusters(IntVec3 c)
		{
			WildPlantSpawner.nearbyClusters.Clear();
			WildPlantSpawner.nearbyClustersList.Clear();
			int num = GenRadial.NumCellsInRadius((float)(this.map.BiomeAt(c).MaxWildAndCavePlantsClusterRadius * 2));
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = c + GenRadial.RadialPattern[i];
				if (intVec.InBounds(this.map))
				{
					List<Thing> list = this.map.thingGrid.ThingsListAtFast(intVec);
					for (int j = 0; j < list.Count; j++)
					{
						Thing thing = list[j];
						if (thing.def.category == ThingCategory.Plant && thing.def.plant.GrowsInClusters)
						{
							float item = (float)intVec.DistanceToSquared(c);
							List<float> list2;
							if (!WildPlantSpawner.nearbyClusters.TryGetValue(thing.def, out list2))
							{
								list2 = SimplePool<List<float>>.Get();
								WildPlantSpawner.nearbyClusters.Add(thing.def, list2);
								WildPlantSpawner.nearbyClustersList.Add(new KeyValuePair<ThingDef, List<float>>(thing.def, list2));
							}
							list2.Add(item);
						}
					}
				}
			}
			WildPlantSpawner.distanceSqToNearbyClusters.Clear();
			for (int k = 0; k < WildPlantSpawner.nearbyClustersList.Count; k++)
			{
				List<float> value = WildPlantSpawner.nearbyClustersList[k].Value;
				value.Sort();
				WildPlantSpawner.distanceSqToNearbyClusters.Add(WildPlantSpawner.nearbyClustersList[k].Key, value[value.Count / 2]);
				value.Clear();
				SimplePool<List<float>>.Return(value);
			}
		}

		// Token: 0x0600BCB9 RID: 48313 RVA: 0x0036C399 File Offset: 0x0036A599
		private bool CanRegrowAt(IntVec3 c)
		{
			return !c.Roofed(this.map) || this.GoodRoofForCavePlant(c);
		}

		// Token: 0x0600BCBA RID: 48314 RVA: 0x0036C3B4 File Offset: 0x0036A5B4
		private bool GoodRoofForCavePlant(IntVec3 c)
		{
			RoofDef roof = c.GetRoof(this.map);
			return roof != null && roof.isNatural;
		}

		// Token: 0x0600BCBB RID: 48315 RVA: 0x0036C3DC File Offset: 0x0036A5DC
		private float GetCommonalityOfPlant(ThingDef plant)
		{
			this.CachePlantCommonalitiesIfShould();
			if (plant.plant.cavePlant && !this.map.Biome.wildPlantsAreCavePlants)
			{
				return plant.plant.cavePlantWeight;
			}
			return this.cachedPlantCommonalities.GetValueOrDefault(plant, 0f);
		}

		// Token: 0x0600BCBC RID: 48316 RVA: 0x0036C42B File Offset: 0x0036A62B
		public float GetCommonalityPctOfPlant(ThingDef plant)
		{
			if (!plant.plant.cavePlant || this.map.Biome.wildPlantsAreCavePlants)
			{
				return this.GetCommonalityOfPlant(plant) / this.PlantsCommonalitiesSum;
			}
			return this.GetCommonalityOfPlant(plant) / this.CavePlantsCommonalitiesSum;
		}

		// Token: 0x0600BCBD RID: 48317 RVA: 0x0036C46C File Offset: 0x0036A66C
		public float GetBaseDesiredPlantsCountAt(IntVec3 c)
		{
			float num = this.map.fertilityGrid.FertilityAt(c);
			if (this.GoodRoofForCavePlant(c))
			{
				num *= 0.5f;
			}
			if (this.HaveAnyPlantsWhichIgnoreFertility && c.GetTerrain(this.map).IsWater && num <= 0f)
			{
				num = 0.1f;
			}
			return num;
		}

		// Token: 0x0600BCBE RID: 48318 RVA: 0x0036C4C8 File Offset: 0x0036A6C8
		public float GetDesiredPlantsCountAt(IntVec3 forCell, float plantDensityFactor)
		{
			float num = this.map.fertilityGrid.FertilityAt(forCell);
			if (num <= 0f && this.HaveAnyPlantsWhichIgnoreFertility)
			{
				num = 1f;
			}
			float num2 = this.map.BiomeAt(forCell).plantDensity * plantDensityFactor * num;
			return Mathf.Min(this.GetBaseDesiredPlantsCountAt(forCell) * num2, 1f);
		}

		// Token: 0x0600BCBF RID: 48319 RVA: 0x0036C528 File Offset: 0x0036A728
		public float GetDesiredPlantsCountIn(Region reg, IntVec3 forCell, float plantDensityFactor)
		{
			float num = this.map.fertilityGrid.FertilityAt(forCell);
			if (num <= 0f && this.HaveAnyPlantsWhichIgnoreFertility)
			{
				num = 1f;
			}
			float num2 = this.map.BiomeAt(forCell).plantDensity * plantDensityFactor * num;
			return Mathf.Min(reg.GetBaseDesiredPlantsCount(true) * num2, (float)reg.CellCount);
		}

		// Token: 0x0600BCC0 RID: 48320 RVA: 0x0036C588 File Offset: 0x0036A788
		public float GetWaterPlantDistanceToShoreWeight(IntVec3 rootCell)
		{
			if (!rootCell.GetTerrain(this.map).IsWater)
			{
				return 1f;
			}
			float distance = float.MaxValue;
			this.map.floodFiller.FloodFill(rootCell, (IntVec3 c) => true, delegate(IntVec3 c)
			{
				if (this.map.terrainGrid.BaseTerrainAt(c).IsWater)
				{
					return false;
				}
				distance = c.DistanceTo(rootCell);
				return true;
			}, int.MaxValue, false, null);
			return Mathf.Clamp01(1f - distance / 4f);
		}

		// Token: 0x040081DF RID: 33247
		private readonly Map map;

		// Token: 0x040081E0 RID: 33248
		private int cycleIndex;

		// Token: 0x040081E1 RID: 33249
		private float calculatedWholeMapNumDesiredPlants;

		// Token: 0x040081E2 RID: 33250
		private float calculatedWholeMapNumDesiredPlantsTmp;

		// Token: 0x040081E3 RID: 33251
		private int calculatedWholeMapNumNonZeroFertilityCells;

		// Token: 0x040081E4 RID: 33252
		private int calculatedWholeMapNumNonZeroFertilityCellsTmp;

		// Token: 0x040081E5 RID: 33253
		private bool hasWholeMapNumDesiredPlantsCalculated;

		// Token: 0x040081E6 RID: 33254
		[Unsaved(false)]
		private List<ThingDef> cachedWildPlants;

		// Token: 0x040081E7 RID: 33255
		[Unsaved(false)]
		private List<ThingDef> cachedMutatorWildPlants;

		// Token: 0x040081E8 RID: 33256
		[Unsaved(false)]
		private Dictionary<ThingDef, float> cachedPlantCommonalities;

		// Token: 0x040081E9 RID: 33257
		[Unsaved(false)]
		private float? cachedPlantsCommonalitiesSum;

		// Token: 0x040081EA RID: 33258
		[Unsaved(false)]
		private float? cachedCavePlantsCommonalitiesSum;

		// Token: 0x040081EB RID: 33259
		[Unsaved(false)]
		private bool? cachedHaveAnyPlantsWhichIgnoreFertility;

		// Token: 0x040081EC RID: 33260
		[Unsaved(false)]
		private SimpleCurve cachedOverrideDensityForFertilityCurve;

		// Token: 0x040081ED RID: 33261
		private static readonly List<ThingDef> allCavePlants = new List<ThingDef>();

		// Token: 0x040081EE RID: 33262
		private static readonly List<ThingDef> tmpPossiblePlants = new List<ThingDef>();

		// Token: 0x040081EF RID: 33263
		private static readonly List<KeyValuePair<ThingDef, float>> tmpPossiblePlantsWithWeight = new List<KeyValuePair<ThingDef, float>>();

		// Token: 0x040081F0 RID: 33264
		private static readonly Dictionary<ThingDef, float> distanceSqToNearbyClusters = new Dictionary<ThingDef, float>();

		// Token: 0x040081F1 RID: 33265
		private static readonly Dictionary<ThingDef, List<float>> nearbyClusters = new Dictionary<ThingDef, List<float>>();

		// Token: 0x040081F2 RID: 33266
		private static readonly List<KeyValuePair<ThingDef, List<float>>> nearbyClustersList = new List<KeyValuePair<ThingDef, List<float>>>();

		// Token: 0x040081F3 RID: 33267
		public static readonly FloatRange InitialGrowthRandomRange = new FloatRange(0.15f, 1.5f);

		// Token: 0x040081F4 RID: 33268
		private const float CavePlantsDensityFactor = 0.5f;

		// Token: 0x040081F5 RID: 33269
		private const float WaterPlantsDensityFactor = 0.1f;

		// Token: 0x040081F6 RID: 33270
		private const int PlantSaturationScanRadius = 20;

		// Token: 0x040081F7 RID: 33271
		private const float MapFractionCheckPerTick = 0.0001f;

		// Token: 0x040081F8 RID: 33272
		private const float ChanceToRegrow = 0.012f;

		// Token: 0x040081F9 RID: 33273
		private const float CavePlantChanceToRegrow = 0.0001f;

		// Token: 0x040081FA RID: 33274
		private const float BaseLowerOrderScanRadius = 7f;

		// Token: 0x040081FB RID: 33275
		private const float LowerOrderScanRadiusWildClusterRadiusFactor = 1.5f;

		// Token: 0x040081FC RID: 33276
		private const float MinDesiredLowerOrderPlantsToConsiderSkipping = 4f;

		// Token: 0x040081FD RID: 33277
		private const float MinLowerOrderPlantsPct = 0.57f;

		// Token: 0x040081FE RID: 33278
		private const float LocalPlantProportionsMaxScanRadius = 25f;

		// Token: 0x040081FF RID: 33279
		private const float MaxLocalProportionsBias = 7f;

		// Token: 0x04008200 RID: 33280
		private const float CavePlantRegrowDays = 130f;

		// Token: 0x04008201 RID: 33281
		private static readonly SimpleCurve GlobalPctSelectionWeightBias = new SimpleCurve
		{
			{
				new CurvePoint(0f, 3f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			},
			{
				new CurvePoint(1.5f, 0.25f),
				true
			},
			{
				new CurvePoint(3f, 0.02f),
				true
			}
		};

		// Token: 0x04008202 RID: 33282
		private static readonly List<ThingDef> tmpWildPlants = new List<ThingDef>();

		// Token: 0x04008203 RID: 33283
		private static readonly List<ThingDef> tmpPlantDefsLowerOrder = new List<ThingDef>();
	}
}
