using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003B9 RID: 953
	public class MapPlantGrowthRateCalculator
	{
		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x0600185B RID: 6235 RVA: 0x0007B946 File Offset: 0x00079B46
		public List<TerrainDef> TerrainDefs
		{
			get
			{
				this.ComputeIfDirty();
				return this.terrainDefs;
			}
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x0600185C RID: 6236 RVA: 0x0007B954 File Offset: 0x00079B54
		public List<ThingDef> WildGrazingPlants
		{
			get
			{
				this.ComputeIfDirty();
				return this.wildGrazingPlants;
			}
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x0600185D RID: 6237 RVA: 0x0007B962 File Offset: 0x00079B62
		public List<ThingDef> GrazingAnimals
		{
			get
			{
				this.ComputeIfDirty();
				return this.includeAnimalTypes;
			}
		}

		// Token: 0x0600185E RID: 6238 RVA: 0x0007B970 File Offset: 0x00079B70
		public void BuildFor(PlanetTile tile)
		{
			this.tile = tile;
			this.longLat = Find.WorldGrid.LongLatOf(tile);
			this.biome = Find.WorldGrid[tile].PrimaryBiome;
			this.ComputeIfDirty();
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x0007B9A6 File Offset: 0x00079BA6
		public void BuildFor(Map map)
		{
			this.tile = map.Tile;
			this.longLat = Find.WorldGrid.LongLatOf(map.Tile);
			this.biome = map.Biome;
			this.ComputeIfDirty();
		}

		// Token: 0x06001860 RID: 6240 RVA: 0x0007B9DC File Offset: 0x00079BDC
		public float GrowthRateForDay(int nowTicks, ThingDef plantDef, TerrainDef terrainDef)
		{
			float result;
			using (ProfilerBlock.Scope("GrowthRateForDay"))
			{
				this.ComputeIfDirty();
				int index = nowTicks / 60000 % 60;
				result = MapPlantGrowthRateCalculator.ComputeGrowthRate(plantDef, terrainDef, this.dailyGrowthRates[index]);
			}
			return result;
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x0007BA3C File Offset: 0x00079C3C
		public float QuadrumGrowthRateFor(Quadrum quadrum, ThingDef plantDef, TerrainDef terrainDef)
		{
			this.ComputeIfDirty();
			return MapPlantGrowthRateCalculator.ComputeGrowthRate(plantDef, terrainDef, this.seasonalGrowthRates[quadrum]);
		}

		// Token: 0x06001862 RID: 6242 RVA: 0x0007BA58 File Offset: 0x00079C58
		private static float ComputeGrowthRate(ThingDef plantDef, TerrainDef terrainDef, MapPlantGrowthRateCalculator.PlantGrowthRates rates)
		{
			if (!plantDef.plant.completelyIgnoreFertility && terrainDef.fertility < plantDef.plant.fertilityMin)
			{
				return 0f;
			}
			MapPlantGrowthRateCalculator.GrowthRateAccumulator growthRateAccumulator = rates.For(plantDef);
			return PlantUtility.GrowthRateFactorFor_Fertility(plantDef, terrainDef.fertility) * growthRateAccumulator.GrowthRateForTemperature * growthRateAccumulator.GrowthRateForGlow;
		}

		// Token: 0x06001863 RID: 6243 RVA: 0x0007BAB0 File Offset: 0x00079CB0
		private void ComputeIfDirty()
		{
			if (!this.dirty)
			{
				return;
			}
			using (ProfilerBlock.Scope("MapPlantGrowthRateCalculator.ComputeIfDirty"))
			{
				this.dirty = false;
				this.includeAnimalTypes.Clear();
				this.seasonalGrowthRates.Clear();
				this.dailyGrowthRates.Clear();
				this.seasonalGrowthRates.Add(Quadrum.Aprimay, new MapPlantGrowthRateCalculator.PlantGrowthRates());
				this.seasonalGrowthRates.Add(Quadrum.Decembary, new MapPlantGrowthRateCalculator.PlantGrowthRates());
				this.seasonalGrowthRates.Add(Quadrum.Jugust, new MapPlantGrowthRateCalculator.PlantGrowthRates());
				this.seasonalGrowthRates.Add(Quadrum.Septober, new MapPlantGrowthRateCalculator.PlantGrowthRates());
				this.AddIncludedAnimals();
				this.terrainDefs = DefDatabase<TerrainDef>.AllDefsListForReading;
				this.wildGrazingPlants = this.biome.AllWildPlants.Where(new Func<ThingDef, bool>(MapPlantGrowthRateCalculator.IsEdibleByPastureAnimals)).ToList<ThingDef>();
				this.CalculateDailyFertility();
				this.CalculateSeasonalFertility();
			}
		}

		// Token: 0x06001864 RID: 6244 RVA: 0x0007BBA4 File Offset: 0x00079DA4
		private void CalculateDailyFertility()
		{
			for (int i = 0; i < 60; i++)
			{
				int num = i * 60000;
				int nowTicks = num - num % 60000;
				MapPlantGrowthRateCalculator.PlantGrowthRates plantGrowthRates = new MapPlantGrowthRateCalculator.PlantGrowthRates();
				this.dailyGrowthRates.Add(plantGrowthRates);
				foreach (ThingDef plantDef in this.wildGrazingPlants)
				{
					this.SimulateGrowthRateForDay(nowTicks, plantGrowthRates.For(plantDef));
				}
			}
		}

		// Token: 0x06001865 RID: 6245 RVA: 0x0007BC30 File Offset: 0x00079E30
		private void CalculateSeasonalFertility()
		{
			for (int i = 0; i < this.dailyGrowthRates.Count; i++)
			{
				Quadrum key = GenDate.Quadrum((long)(i * 60000), this.longLat.x);
				MapPlantGrowthRateCalculator.PlantGrowthRates plantGrowthRates = this.seasonalGrowthRates[key];
				foreach (KeyValuePair<ThingDef, MapPlantGrowthRateCalculator.GrowthRateAccumulator> keyValuePair in this.dailyGrowthRates[i].byPlant)
				{
					plantGrowthRates.For(keyValuePair.Value.plantDef).Accumulate(keyValuePair.Value);
				}
			}
		}

		// Token: 0x06001866 RID: 6246 RVA: 0x0007BCE8 File Offset: 0x00079EE8
		private void AddIncludedAnimals()
		{
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (MapPlantGrowthRateCalculator.IsPastureAnimal(thingDef))
				{
					this.includeAnimalTypes.Add(thingDef);
				}
			}
			this.includeAnimalTypes.Sort((ThingDef a, ThingDef b) => string.CompareOrdinal(a.label, b.label));
		}

		// Token: 0x06001867 RID: 6247 RVA: 0x0007BD6C File Offset: 0x00079F6C
		public static bool IsPastureAnimal(ThingDef td)
		{
			return !td.IsCorpse && td.race != null && td.race.Animal && td.race.Roamer;
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x0007BD98 File Offset: 0x00079F98
		public static bool IsEdibleByPastureAnimals(ThingDef foodDef)
		{
			return foodDef.ingestible != null && foodDef.ingestible.preferability != FoodPreferability.Undefined && (FoodTypeFlags.VegetarianRoughAnimal & foodDef.ingestible.foodType) > FoodTypeFlags.None;
		}

		// Token: 0x06001869 RID: 6249 RVA: 0x0007BDC8 File Offset: 0x00079FC8
		private void SimulateGrowthRateForDay(int nowTicks, MapPlantGrowthRateCalculator.GrowthRateAccumulator growthRates)
		{
			int num = nowTicks - nowTicks % 60000;
			int num2 = 24;
			int num3 = 60000 / num2;
			for (int i = 0; i < num2; i++)
			{
				int num4 = num + i * num3;
				float cellTemp = Find.World.tileTemperatures.OutdoorTemperatureAt(this.tile, num4);
				float glow = GenCelestial.CelestialSunGlow(this.tile, num4);
				float grTemp = PlantUtility.GrowthRateFactorFor_Temperature(growthRates.plantDef, cellTemp);
				float grGlow = PlantUtility.GrowthRateFactorFor_Light(growthRates.plantDef, glow);
				growthRates.Accumulate(grTemp, grGlow);
			}
		}

		// Token: 0x040015B2 RID: 5554
		private Vector2 longLat;

		// Token: 0x040015B3 RID: 5555
		private PlanetTile tile;

		// Token: 0x040015B4 RID: 5556
		private BiomeDef biome;

		// Token: 0x040015B5 RID: 5557
		private bool dirty = true;

		// Token: 0x040015B6 RID: 5558
		private readonly List<ThingDef> includeAnimalTypes = new List<ThingDef>();

		// Token: 0x040015B7 RID: 5559
		private List<TerrainDef> terrainDefs;

		// Token: 0x040015B8 RID: 5560
		private List<ThingDef> wildGrazingPlants;

		// Token: 0x040015B9 RID: 5561
		private readonly Dictionary<Quadrum, MapPlantGrowthRateCalculator.PlantGrowthRates> seasonalGrowthRates = new Dictionary<Quadrum, MapPlantGrowthRateCalculator.PlantGrowthRates>();

		// Token: 0x040015BA RID: 5562
		private readonly List<MapPlantGrowthRateCalculator.PlantGrowthRates> dailyGrowthRates = new List<MapPlantGrowthRateCalculator.PlantGrowthRates>();

		// Token: 0x020003BA RID: 954
		private class GrowthRateAccumulator
		{
			// Token: 0x0600186B RID: 6251 RVA: 0x0007BE7D File Offset: 0x0007A07D
			public GrowthRateAccumulator(ThingDef plantDef)
			{
				this.plantDef = plantDef;
			}

			// Token: 0x1700046E RID: 1134
			// (get) Token: 0x0600186C RID: 6252 RVA: 0x0007BE8C File Offset: 0x0007A08C
			public float GrowthRateForTemperature
			{
				get
				{
					if (this.numSamples != 0)
					{
						return this.sumGrowthRateForTemperature / (float)this.numSamples;
					}
					return 0f;
				}
			}

			// Token: 0x1700046F RID: 1135
			// (get) Token: 0x0600186D RID: 6253 RVA: 0x0007BEAA File Offset: 0x0007A0AA
			public float GrowthRateForGlow
			{
				get
				{
					if (this.numSamples != 0)
					{
						return this.sumGrowthRateForGlow / (float)this.numSamples;
					}
					return 0f;
				}
			}

			// Token: 0x0600186E RID: 6254 RVA: 0x0007BEC8 File Offset: 0x0007A0C8
			public void Accumulate(float grTemp, float grGlow)
			{
				this.sumGrowthRateForTemperature += grTemp;
				this.sumGrowthRateForGlow += grGlow;
				this.numSamples++;
			}

			// Token: 0x0600186F RID: 6255 RVA: 0x0007BEF4 File Offset: 0x0007A0F4
			public void Accumulate(MapPlantGrowthRateCalculator.GrowthRateAccumulator other)
			{
				this.sumGrowthRateForTemperature += other.sumGrowthRateForTemperature;
				this.sumGrowthRateForGlow += other.sumGrowthRateForGlow;
				this.numSamples += other.numSamples;
			}

			// Token: 0x040015BB RID: 5563
			public readonly ThingDef plantDef;

			// Token: 0x040015BC RID: 5564
			private float sumGrowthRateForTemperature;

			// Token: 0x040015BD RID: 5565
			private float sumGrowthRateForGlow;

			// Token: 0x040015BE RID: 5566
			private int numSamples;
		}

		// Token: 0x020003BB RID: 955
		private class PlantGrowthRates
		{
			// Token: 0x06001870 RID: 6256 RVA: 0x0007BF30 File Offset: 0x0007A130
			public MapPlantGrowthRateCalculator.GrowthRateAccumulator For(ThingDef plantDef)
			{
				MapPlantGrowthRateCalculator.GrowthRateAccumulator growthRateAccumulator;
				if (!this.byPlant.TryGetValue(plantDef, out growthRateAccumulator))
				{
					growthRateAccumulator = new MapPlantGrowthRateCalculator.GrowthRateAccumulator(plantDef);
					this.byPlant.Add(plantDef, growthRateAccumulator);
				}
				return growthRateAccumulator;
			}

			// Token: 0x040015BF RID: 5567
			public readonly Dictionary<ThingDef, MapPlantGrowthRateCalculator.GrowthRateAccumulator> byPlant = new Dictionary<ThingDef, MapPlantGrowthRateCalculator.GrowthRateAccumulator>();
		}
	}
}
