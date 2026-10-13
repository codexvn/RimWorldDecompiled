using System;
using System.Collections.Generic;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x02003CC4 RID: 15556
	public class TileTemperaturesComp : WorldComponent
	{
		// Token: 0x0601525B RID: 86619 RVA: 0x00654821 File Offset: 0x00652A21
		public TileTemperaturesComp(World world) : base(world)
		{
			this.ClearCaches();
		}

		// Token: 0x0601525C RID: 86620 RVA: 0x00654830 File Offset: 0x00652A30
		public override void WorldComponentTick()
		{
			foreach (KeyValuePair<PlanetLayerDef, List<int>> keyValuePair in this.usedSlots)
			{
				PlanetLayerDef planetLayerDef;
				List<int> list;
				keyValuePair.Deconstruct(out planetLayerDef, out list);
				PlanetLayerDef key = planetLayerDef;
				foreach (int num in list)
				{
					this.cache[key][num].CheckCache();
				}
			}
			if (Find.TickManager.TicksGame % 300 == 84)
			{
				foreach (KeyValuePair<PlanetLayerDef, List<int>> keyValuePair in this.usedSlots)
				{
					PlanetLayerDef planetLayerDef;
					List<int> list;
					keyValuePair.Deconstruct(out planetLayerDef, out list);
					PlanetLayerDef key2 = planetLayerDef;
					List<int> list2 = list;
					if (list2.Any<int>())
					{
						this.cache[key2][list2[0]] = null;
						list2.RemoveAt(0);
					}
				}
			}
		}

		// Token: 0x0601525D RID: 86621 RVA: 0x00654960 File Offset: 0x00652B60
		public float GetOutdoorTemp(PlanetTile tile)
		{
			return this.RetrieveCachedData(tile).GetOutdoorTemp();
		}

		// Token: 0x0601525E RID: 86622 RVA: 0x0065496E File Offset: 0x00652B6E
		public float GetSeasonalTemp(PlanetTile tile)
		{
			return this.RetrieveCachedData(tile).GetSeasonalTemp();
		}

		// Token: 0x0601525F RID: 86623 RVA: 0x0065497C File Offset: 0x00652B7C
		public float OutdoorTemperatureAt(PlanetTile tile, int absTick)
		{
			return this.RetrieveCachedData(tile).OutdoorTemperatureAt(absTick);
		}

		// Token: 0x06015260 RID: 86624 RVA: 0x0065498B File Offset: 0x00652B8B
		public float OffsetFromDailyRandomVariation(PlanetTile tile, int absTick)
		{
			return this.RetrieveCachedData(tile).OffsetFromDailyRandomVariation(absTick);
		}

		// Token: 0x06015261 RID: 86625 RVA: 0x0065499A File Offset: 0x00652B9A
		public float AverageTemperatureForTwelfth(PlanetTile tile, Twelfth twelfth)
		{
			return this.RetrieveCachedData(tile).AverageTemperatureForTwelfth(twelfth);
		}

		// Token: 0x06015262 RID: 86626 RVA: 0x006549AC File Offset: 0x00652BAC
		public bool SeasonAcceptableFor(PlanetTile tile, ThingDef animalRace)
		{
			if (!tile.Valid)
			{
				return true;
			}
			float seasonalTemp = this.GetSeasonalTemp(tile);
			return seasonalTemp > animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) && seasonalTemp < animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null);
		}

		// Token: 0x06015263 RID: 86627 RVA: 0x006549EC File Offset: 0x00652BEC
		public bool OutdoorTemperatureAcceptableFor(PlanetTile tile, ThingDef animalRace)
		{
			if (!tile.Valid)
			{
				return true;
			}
			float outdoorTemp = this.GetOutdoorTemp(tile);
			return outdoorTemp > animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) && outdoorTemp < animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null);
		}

		// Token: 0x06015264 RID: 86628 RVA: 0x00654A2C File Offset: 0x00652C2C
		public bool SeasonAndOutdoorTemperatureAcceptableFor(PlanetTile tile, ThingDef animalRace)
		{
			return this.SeasonAcceptableFor(tile, animalRace) && this.OutdoorTemperatureAcceptableFor(tile, animalRace);
		}

		// Token: 0x06015265 RID: 86629 RVA: 0x00654A42 File Offset: 0x00652C42
		public void ClearCaches()
		{
			this.cache = new Dictionary<PlanetLayerDef, TileTemperaturesComp.CachedTileTemperatureData[]>();
			this.usedSlots = new Dictionary<PlanetLayerDef, List<int>>();
		}

		// Token: 0x06015266 RID: 86630 RVA: 0x00654A5C File Offset: 0x00652C5C
		private TileTemperaturesComp.CachedTileTemperatureData RetrieveCachedData(PlanetTile tile)
		{
			if (!this.cache.ContainsKey(tile.LayerDef))
			{
				this.cache[tile.LayerDef] = new TileTemperaturesComp.CachedTileTemperatureData[tile.Layer.TilesCount];
			}
			if (!this.usedSlots.ContainsKey(tile.LayerDef))
			{
				this.usedSlots[tile.LayerDef] = new List<int>();
			}
			if (this.cache[tile.LayerDef][tile.tileId] != null)
			{
				return this.cache[tile.LayerDef][tile.tileId];
			}
			this.cache[tile.LayerDef][tile.tileId] = new TileTemperaturesComp.CachedTileTemperatureData(tile);
			this.usedSlots[tile.LayerDef].Add(tile.tileId);
			return this.cache[tile.LayerDef][tile.tileId];
		}

		// Token: 0x0400EBF6 RID: 60406
		private Dictionary<PlanetLayerDef, TileTemperaturesComp.CachedTileTemperatureData[]> cache;

		// Token: 0x0400EBF7 RID: 60407
		private Dictionary<PlanetLayerDef, List<int>> usedSlots;

		// Token: 0x02003CC5 RID: 15557
		private class CachedTileTemperatureData
		{
			// Token: 0x06015267 RID: 86631 RVA: 0x00654B58 File Offset: 0x00652D58
			public CachedTileTemperatureData(PlanetTile tile)
			{
				this.tile = tile;
				int seed = Gen.HashCombineInt(tile.GetHashCode(), 199372327);
				this.dailyVariationPerlinCached = new Perlin(4.999999873689376E-06, 2.0, 0.5, 3, seed, QualityMode.Medium);
				this.twelfthlyTempAverages = new float[12];
				for (int i = 0; i < 12; i++)
				{
					this.twelfthlyTempAverages[i] = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, (Twelfth)i);
				}
				this.CheckCache();
			}

			// Token: 0x06015268 RID: 86632 RVA: 0x00654C04 File Offset: 0x00652E04
			public float GetOutdoorTemp()
			{
				return this.cachedOutdoorTemp;
			}

			// Token: 0x06015269 RID: 86633 RVA: 0x00654C0C File Offset: 0x00652E0C
			public float GetSeasonalTemp()
			{
				return this.cachedSeasonalTemp;
			}

			// Token: 0x0601526A RID: 86634 RVA: 0x00654C14 File Offset: 0x00652E14
			public float OutdoorTemperatureAt(int absTick)
			{
				return this.CalculateOutdoorTemperatureAtTile(absTick, true);
			}

			// Token: 0x0601526B RID: 86635 RVA: 0x00654C1E File Offset: 0x00652E1E
			public float OffsetFromDailyRandomVariation(int absTick)
			{
				return (float)this.dailyVariationPerlinCached.GetValue((double)absTick, 0.0, 0.0) * 7f;
			}

			// Token: 0x0601526C RID: 86636 RVA: 0x00654C46 File Offset: 0x00652E46
			public float AverageTemperatureForTwelfth(Twelfth twelfth)
			{
				return this.twelfthlyTempAverages[(int)twelfth];
			}

			// Token: 0x0601526D RID: 86637 RVA: 0x00654C50 File Offset: 0x00652E50
			public void CheckCache()
			{
				if (this.tickCachesNeedReset <= Find.TickManager.TicksGame)
				{
					this.tickCachesNeedReset = Find.TickManager.TicksGame + 60;
					Map map = Current.Game.FindMap(this.tile);
					this.cachedOutdoorTemp = this.OutdoorTemperatureAt(Find.TickManager.TicksAbs);
					if (map != null)
					{
						this.cachedOutdoorTemp += map.gameConditionManager.AggregateTemperatureOffset();
					}
					this.cachedSeasonalTemp = this.CalculateOutdoorTemperatureAtTile(Find.TickManager.TicksAbs, false);
				}
			}

			// Token: 0x0601526E RID: 86638 RVA: 0x00654CDC File Offset: 0x00652EDC
			private float CalculateOutdoorTemperatureAtTile(int absTick, bool includeDailyVariations)
			{
				if (absTick == 0)
				{
					absTick = 1;
				}
				float num = Find.WorldGrid[this.tile].temperature + GenTemperature.OffsetFromSeasonCycle(absTick, this.tile);
				if (includeDailyVariations)
				{
					num += this.OffsetFromDailyRandomVariation(absTick) + GenTemperature.OffsetFromSunCycle(absTick, this.tile);
				}
				return num;
			}

			// Token: 0x0400EBF8 RID: 60408
			private readonly PlanetTile tile;

			// Token: 0x0400EBF9 RID: 60409
			private int tickCachesNeedReset = int.MinValue;

			// Token: 0x0400EBFA RID: 60410
			private float cachedOutdoorTemp = float.MinValue;

			// Token: 0x0400EBFB RID: 60411
			private float cachedSeasonalTemp = float.MinValue;

			// Token: 0x0400EBFC RID: 60412
			private float[] twelfthlyTempAverages;

			// Token: 0x0400EBFD RID: 60413
			private Perlin dailyVariationPerlinCached;

			// Token: 0x0400EBFE RID: 60414
			private const int CachedTempUpdateInterval = 60;
		}
	}
}
