using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003BD RID: 957
	public static class SimplifiedPastureNutritionSimulator
	{
		// Token: 0x06001875 RID: 6261 RVA: 0x0007BF94 File Offset: 0x0007A194
		public static float NutritionProducedPerDay(Map map, ThingDef plantDef, float averageGrowthRate, float mapRespawnChance)
		{
			float result;
			using (ProfilerBlock.Scope("NutritionProducedPerDay"))
			{
				if (Mathf.Approximately(averageGrowthRate, 0f))
				{
					result = 0f;
				}
				else
				{
					if (SimplifiedPastureNutritionSimulator.cachedGame != Current.Game)
					{
						SimplifiedPastureNutritionSimulator.cachedGame = Current.Game;
						SimplifiedPastureNutritionSimulator.nutritionCache.Clear();
					}
					SimplifiedPastureNutritionSimulator.CacheKey key = new SimplifiedPastureNutritionSimulator.CacheKey
					{
						map = map,
						mapRespawnChance = mapRespawnChance,
						plantDef = plantDef
					};
					ValueTuple<float, float> valueTuple;
					if (SimplifiedPastureNutritionSimulator.nutritionCache.TryGetValue(key, out valueTuple) && Mathf.Abs(valueTuple.Item1 - averageGrowthRate) <= 0.05f)
					{
						result = valueTuple.Item2;
					}
					else
					{
						using (ProfilerBlock.Scope("Cache_NutritionProducedPerDay"))
						{
							float num = map.Biome.wildPlantRegrowDays / mapRespawnChance;
							float num2 = plantDef.plant.growDays / averageGrowthRate * plantDef.plant.harvestMinGrowth;
							float num3 = plantDef.GetStatValueAbstract(StatDefOf.Nutrition, null) * PlantUtility.NutritionFactorFromGrowth(plantDef, plantDef.plant.harvestMinGrowth) / (num + num2) * map.wildPlantSpawner.GetCommonalityPctOfPlant(plantDef) * 0.85f;
							SimplifiedPastureNutritionSimulator.nutritionCache[key] = new ValueTuple<float, float>(averageGrowthRate, num3);
							result = num3;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x0007C114 File Offset: 0x0007A314
		public static float NutritionConsumedPerDay(Pawn animal)
		{
			return SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(animal.def, animal.ageTracker.CurLifeStage);
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x0007C12C File Offset: 0x0007A32C
		public static float NutritionConsumedPerDay(ThingDef animalDef)
		{
			LifeStageAge lifeStageAge = animalDef.race.lifeStageAges.Last<LifeStageAge>();
			return SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(animalDef, lifeStageAge.def);
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x0007C156 File Offset: 0x0007A356
		public static float NutritionConsumedPerDay(ThingDef animalDef, LifeStageDef lifeStageDef)
		{
			return Need_Food.BaseHungerRate(lifeStageDef, animalDef) * 60000f;
		}

		// Token: 0x040015C2 RID: 5570
		private const float UnderEstimateNutritionFactor = 0.85f;

		// Token: 0x040015C3 RID: 5571
		private const float CacheGrowthRateRange = 0.05f;

		// Token: 0x040015C4 RID: 5572
		private static Game cachedGame;

		// Token: 0x040015C5 RID: 5573
		[TupleElementNames(new string[]
		{
			"growthRate",
			"perDay"
		})]
		private static readonly Dictionary<SimplifiedPastureNutritionSimulator.CacheKey, ValueTuple<float, float>> nutritionCache = new Dictionary<SimplifiedPastureNutritionSimulator.CacheKey, ValueTuple<float, float>>();

		// Token: 0x020003BE RID: 958
		private struct CacheKey : IEquatable<SimplifiedPastureNutritionSimulator.CacheKey>
		{
			// Token: 0x0600187A RID: 6266 RVA: 0x0007C171 File Offset: 0x0007A371
			public bool Equals(SimplifiedPastureNutritionSimulator.CacheKey other)
			{
				return object.Equals(this.map, other.map) && object.Equals(this.plantDef, other.plantDef) && this.mapRespawnChance.Equals(other.mapRespawnChance);
			}

			// Token: 0x0600187B RID: 6267 RVA: 0x0007C1AC File Offset: 0x0007A3AC
			public override bool Equals(object obj)
			{
				if (obj is SimplifiedPastureNutritionSimulator.CacheKey)
				{
					SimplifiedPastureNutritionSimulator.CacheKey other = (SimplifiedPastureNutritionSimulator.CacheKey)obj;
					return this.Equals(other);
				}
				return false;
			}

			// Token: 0x0600187C RID: 6268 RVA: 0x0007C1D1 File Offset: 0x0007A3D1
			public override int GetHashCode()
			{
				return HashCode.Combine<Map, ThingDef, float>(this.map, this.plantDef, this.mapRespawnChance);
			}

			// Token: 0x040015C6 RID: 5574
			public Map map;

			// Token: 0x040015C7 RID: 5575
			public ThingDef plantDef;

			// Token: 0x040015C8 RID: 5576
			public float mapRespawnChance;
		}
	}
}
