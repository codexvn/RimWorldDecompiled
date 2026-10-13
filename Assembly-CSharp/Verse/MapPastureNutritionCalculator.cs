using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020003B7 RID: 951
	public class MapPastureNutritionCalculator
	{
		// Token: 0x0600184A RID: 6218 RVA: 0x0007B50C File Offset: 0x0007970C
		public void Reset(Map map)
		{
			this.map = map;
			this.cachedSeasonalDetailed.Clear();
			this.cachedSeasonalByTerrain.Clear();
		}

		// Token: 0x0600184B RID: 6219 RVA: 0x0007B52C File Offset: 0x0007972C
		public MapPastureNutritionCalculator.NutritionPerDayPerQuadrum CalculateAverageNutritionPerDay(TerrainDef terrain)
		{
			MapPastureNutritionCalculator.NutritionPerDayPerQuadrum nutritionPerDayPerQuadrum;
			if (!this.cachedSeasonalByTerrain.TryGetValue(terrain, out nutritionPerDayPerQuadrum))
			{
				nutritionPerDayPerQuadrum = default(MapPastureNutritionCalculator.NutritionPerDayPerQuadrum);
				foreach (ThingDef plantDef in this.map.plantGrowthRateCalculator.WildGrazingPlants)
				{
					MapPastureNutritionCalculator.NutritionPerDayPerQuadrum other = this.CalculateAverageNutritionPerDay(plantDef, terrain);
					nutritionPerDayPerQuadrum.AddFrom(other);
				}
				this.cachedSeasonalByTerrain.TryAdd(terrain, nutritionPerDayPerQuadrum);
			}
			return nutritionPerDayPerQuadrum;
		}

		// Token: 0x0600184C RID: 6220 RVA: 0x0007B5BC File Offset: 0x000797BC
		private MapPastureNutritionCalculator.NutritionPerDayPerQuadrum CalculateAverageNutritionPerDay(ThingDef plantDef, TerrainDef terrain)
		{
			MapPastureNutritionCalculator.NutritionPerDayPerQuadrum result;
			using (ProfilerBlock.Scope("CalculateAverageNutritionPerDay"))
			{
				Dictionary<TerrainDef, MapPastureNutritionCalculator.NutritionPerDayPerQuadrum> dictionary;
				if (!this.cachedSeasonalDetailed.TryGetValue(plantDef, out dictionary))
				{
					dictionary = new Dictionary<TerrainDef, MapPastureNutritionCalculator.NutritionPerDayPerQuadrum>();
					this.cachedSeasonalDetailed.Add(plantDef, dictionary);
				}
				MapPastureNutritionCalculator.NutritionPerDayPerQuadrum nutritionPerDayPerQuadrum;
				if (!dictionary.TryGetValue(terrain, out nutritionPerDayPerQuadrum))
				{
					nutritionPerDayPerQuadrum = default(MapPastureNutritionCalculator.NutritionPerDayPerQuadrum);
					dictionary.Add(terrain, nutritionPerDayPerQuadrum);
					nutritionPerDayPerQuadrum[Quadrum.Aprimay] = this.GetAverageNutritionPerDay(Quadrum.Aprimay, plantDef, terrain);
					nutritionPerDayPerQuadrum[Quadrum.Decembary] = this.GetAverageNutritionPerDay(Quadrum.Decembary, plantDef, terrain);
					nutritionPerDayPerQuadrum[Quadrum.Jugust] = this.GetAverageNutritionPerDay(Quadrum.Jugust, plantDef, terrain);
					nutritionPerDayPerQuadrum[Quadrum.Septober] = this.GetAverageNutritionPerDay(Quadrum.Septober, plantDef, terrain);
				}
				result = nutritionPerDayPerQuadrum;
			}
			return result;
		}

		// Token: 0x0600184D RID: 6221 RVA: 0x0007B67C File Offset: 0x0007987C
		public float GetAverageNutritionPerDayToday(TerrainDef terrainDef)
		{
			float result;
			using (ProfilerBlock.Scope("GetAverageNutritionPerDayToday"))
			{
				float num = 0f;
				foreach (ThingDef plantDef in this.map.plantGrowthRateCalculator.WildGrazingPlants)
				{
					num += this.GetAverageNutritionPerDayToday(plantDef, terrainDef);
				}
				result = num;
			}
			return result;
		}

		// Token: 0x0600184E RID: 6222 RVA: 0x0007B710 File Offset: 0x00079910
		private float GetAverageNutritionPerDayToday(ThingDef plantDef, TerrainDef terrainDef)
		{
			if (terrainDef.fertility <= 0f)
			{
				return 0f;
			}
			float result;
			using (ProfilerBlock.Scope("GetAverageNutritionPerDayToday Plant/Terrain"))
			{
				int ticksAbs = Find.TickManager.TicksAbs;
				int nowTicks = ticksAbs - ticksAbs % 60000;
				float growthRate = this.map.plantGrowthRateCalculator.GrowthRateForDay(nowTicks, plantDef, terrainDef);
				result = this.ComputeNutritionProducedPerDay(plantDef, growthRate);
			}
			return result;
		}

		// Token: 0x0600184F RID: 6223 RVA: 0x0007B78C File Offset: 0x0007998C
		public float GetAverageNutritionPerDay(Quadrum quadrum, TerrainDef terrainDef)
		{
			float num = 0f;
			foreach (ThingDef plantDef in this.map.plantGrowthRateCalculator.WildGrazingPlants)
			{
				num += this.GetAverageNutritionPerDay(quadrum, plantDef, terrainDef);
			}
			return num;
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x0007B7F8 File Offset: 0x000799F8
		public float GetAverageNutritionPerDay(Quadrum quadrum, ThingDef plantDef, TerrainDef terrainDef)
		{
			float growthRate = this.map.plantGrowthRateCalculator.QuadrumGrowthRateFor(quadrum, plantDef, terrainDef);
			return this.ComputeNutritionProducedPerDay(plantDef, growthRate);
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x0007B821 File Offset: 0x00079A21
		private float ComputeNutritionProducedPerDay(ThingDef plantDef, float growthRate)
		{
			return SimplifiedPastureNutritionSimulator.NutritionProducedPerDay(this.map, plantDef, growthRate, this.map.wildPlantSpawner.CachedChanceFromDensity);
		}

		// Token: 0x040015AB RID: 5547
		public Map map;

		// Token: 0x040015AC RID: 5548
		private readonly Dictionary<ThingDef, Dictionary<TerrainDef, MapPastureNutritionCalculator.NutritionPerDayPerQuadrum>> cachedSeasonalDetailed = new Dictionary<ThingDef, Dictionary<TerrainDef, MapPastureNutritionCalculator.NutritionPerDayPerQuadrum>>();

		// Token: 0x040015AD RID: 5549
		private readonly Dictionary<TerrainDef, MapPastureNutritionCalculator.NutritionPerDayPerQuadrum> cachedSeasonalByTerrain = new Dictionary<TerrainDef, MapPastureNutritionCalculator.NutritionPerDayPerQuadrum>();

		// Token: 0x020003B8 RID: 952
		public struct NutritionPerDayPerQuadrum
		{
			// Token: 0x06001853 RID: 6227 RVA: 0x0007B85E File Offset: 0x00079A5E
			public float ForQuadrum(Quadrum q)
			{
				return this[q];
			}

			// Token: 0x06001854 RID: 6228 RVA: 0x0007B868 File Offset: 0x00079A68
			public void AddFrom(MapPastureNutritionCalculator.NutritionPerDayPerQuadrum other)
			{
				this[0] = this[0] + other[0];
				this[3] = this[3] + other[3];
				this[1] = this[1] + other[1];
				this[2] = this[2] + other[2];
			}

			// Token: 0x17000469 RID: 1129
			public float this[int key]
			{
				get
				{
					return this.GetValue(key);
				}
				set
				{
					this.SetValue(key, value);
				}
			}

			// Token: 0x1700046A RID: 1130
			public float this[Quadrum key]
			{
				get
				{
					return this.GetValue((int)key);
				}
				set
				{
					this.SetValue((int)key, value);
				}
			}

			// Token: 0x06001859 RID: 6233 RVA: 0x0007B8E4 File Offset: 0x00079AE4
			public float GetValue(int key)
			{
				if (key == 0)
				{
					return this.quadrumOne;
				}
				if (key == 1)
				{
					return this.quadrumTwo;
				}
				if (key == 2)
				{
					return this.quadrumThree;
				}
				if (key == 3)
				{
					return this.quadrumFour;
				}
				return -1f;
			}

			// Token: 0x0600185A RID: 6234 RVA: 0x0007B916 File Offset: 0x00079B16
			public void SetValue(int key, float value)
			{
				if (key == 0)
				{
					this.quadrumOne = value;
					return;
				}
				if (key == 1)
				{
					this.quadrumTwo = value;
					return;
				}
				if (key == 2)
				{
					this.quadrumThree = value;
					return;
				}
				if (key == 3)
				{
					this.quadrumFour = value;
				}
			}

			// Token: 0x040015AE RID: 5550
			private float quadrumOne;

			// Token: 0x040015AF RID: 5551
			private float quadrumTwo;

			// Token: 0x040015B0 RID: 5552
			private float quadrumThree;

			// Token: 0x040015B1 RID: 5553
			private float quadrumFour;
		}
	}
}
