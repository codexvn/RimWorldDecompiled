using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020021A1 RID: 8609
	public class WeatherDecider : IExposable
	{
		// Token: 0x17001C93 RID: 7315
		// (get) Token: 0x0600BC13 RID: 48147 RVA: 0x00368178 File Offset: 0x00366378
		public WeatherDef ForcedWeather
		{
			get
			{
				WeatherDecider.allConditionsTmp.Clear();
				this.map.gameConditionManager.GetAllGameConditionsAffectingMap(this.map, WeatherDecider.allConditionsTmp);
				WeatherDef result = null;
				foreach (GameCondition gameCondition in WeatherDecider.allConditionsTmp)
				{
					WeatherDef weatherDef = gameCondition.ForcedWeather();
					if (weatherDef != null)
					{
						result = weatherDef;
					}
				}
				return result;
			}
		}

		// Token: 0x17001C94 RID: 7316
		// (get) Token: 0x0600BC14 RID: 48148 RVA: 0x003681F8 File Offset: 0x003663F8
		public IEnumerable<WeatherCommonalityRecord> WeatherCommonalities
		{
			get
			{
				return this.map.Biome.baseWeatherCommonalities;
			}
		}

		// Token: 0x17001C95 RID: 7317
		// (get) Token: 0x0600BC15 RID: 48149 RVA: 0x0036820A File Offset: 0x0036640A
		public bool ClearWeatherAllowed
		{
			get
			{
				if (!this.WeatherCommonalities.EnumerableNullOrEmpty<WeatherCommonalityRecord>())
				{
					return this.WeatherCommonalities.Any((WeatherCommonalityRecord w) => w.weather == WeatherDefOf.Clear);
				}
				return false;
			}
		}

		// Token: 0x0600BC16 RID: 48150 RVA: 0x00368245 File Offset: 0x00366445
		public WeatherDecider(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600BC17 RID: 48151 RVA: 0x0036825F File Offset: 0x0036645F
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.curWeatherDuration, "curWeatherDuration", 0, true);
			Scribe_Values.Look<int>(ref this.ticksWhenRainAllowedAgain, "ticksWhenRainAllowedAgain", 0, false);
		}

		// Token: 0x0600BC18 RID: 48152 RVA: 0x00368288 File Offset: 0x00366488
		public void WeatherDeciderTick()
		{
			WeatherDef forcedWeather = this.ForcedWeather;
			int num = this.curWeatherDuration;
			if (this.map.fireWatcher.LargeFireDangerPresent || !this.map.weatherManager.curWeather.temperatureRange.Includes(this.map.mapTemperature.OutdoorTemp) || (ModsConfig.AnomalyActive && this.map.gameConditionManager.BrightnessChanging))
			{
				num = (int)((float)num * 0.25f);
			}
			if (forcedWeather != null && this.map.weatherManager.curWeather != forcedWeather)
			{
				if (ModsConfig.AnomalyActive && forcedWeather.transitionTicksOverride != 2147483647)
				{
					num = forcedWeather.transitionTicksOverride;
				}
				else
				{
					num = 4000;
				}
			}
			if (this.map.weatherManager.curWeatherAge > num)
			{
				this.StartNextWeather();
			}
		}

		// Token: 0x0600BC19 RID: 48153 RVA: 0x0036835C File Offset: 0x0036655C
		public void StartNextWeather()
		{
			WeatherDef weatherDef = this.ChooseNextWeather();
			this.map.weatherManager.TransitionTo(weatherDef);
			this.curWeatherDuration = weatherDef.durationRange.RandomInRange;
		}

		// Token: 0x0600BC1A RID: 48154 RVA: 0x00368394 File Offset: 0x00366594
		public void StartInitialWeather()
		{
			if (Find.GameInitData != null)
			{
				if (this.ClearWeatherAllowed)
				{
					this.map.weatherManager.curWeather = WeatherDefOf.Clear;
				}
				else
				{
					this.map.weatherManager.curWeather = this.WeatherCommonalities.RandomElement<WeatherCommonalityRecord>().weather;
				}
				this.curWeatherDuration = 10000;
				this.map.weatherManager.lastWeather = this.map.weatherManager.curWeather;
				this.map.weatherManager.curWeatherAge = 0;
			}
			else
			{
				this.map.weatherManager.curWeather = null;
				WeatherDef weatherDef = this.ChooseNextWeather();
				WeatherDef lastWeather = this.ChooseNextWeather();
				this.map.weatherManager.curWeather = weatherDef;
				this.map.weatherManager.lastWeather = lastWeather;
				this.curWeatherDuration = weatherDef.durationRange.RandomInRange;
				this.map.weatherManager.curWeatherAge = Rand.Range(0, this.curWeatherDuration);
			}
			this.map.weatherManager.ResetSkyTargetLerpCache();
		}

		// Token: 0x0600BC1B RID: 48155 RVA: 0x003684A4 File Offset: 0x003666A4
		private WeatherDef ChooseNextWeather()
		{
			if (TutorSystem.TutorialMode && this.ClearWeatherAllowed)
			{
				return WeatherDefOf.Clear;
			}
			WeatherDef forcedWeather = this.ForcedWeather;
			if (forcedWeather != null)
			{
				return forcedWeather;
			}
			WeatherDef result;
			if (DefDatabase<WeatherDef>.AllDefs.TryRandomElementByWeight(new Func<WeatherDef, float>(this.CurrentWeatherCommonality), out result))
			{
				return result;
			}
			Log.Warning("All weather commonalities were zero. Defaulting to " + WeatherDefOf.Clear.defName + ".");
			if (!this.WeatherCommonalities.EnumerableNullOrEmpty<WeatherCommonalityRecord>())
			{
				return this.WeatherCommonalities.RandomElement<WeatherCommonalityRecord>().weather;
			}
			return WeatherDefOf.Clear;
		}

		// Token: 0x0600BC1C RID: 48156 RVA: 0x0036852F File Offset: 0x0036672F
		public void DisableRainFor(int ticks)
		{
			this.ticksWhenRainAllowedAgain = Find.TickManager.TicksGame + ticks;
		}

		// Token: 0x0600BC1D RID: 48157 RVA: 0x00368544 File Offset: 0x00366744
		private float CurrentWeatherCommonality(WeatherDef weather)
		{
			if (this.map.weatherManager.curWeather != null && !this.map.weatherManager.curWeather.repeatable && weather == this.map.weatherManager.curWeather)
			{
				return 0f;
			}
			if (!weather.temperatureRange.Includes(this.map.mapTemperature.OutdoorTemp))
			{
				return 0f;
			}
			if (weather.favorability < Favorability.Neutral && GenDate.DaysPassedSinceSettle < 8)
			{
				return 0f;
			}
			if (weather.rainRate > 0.1f && Find.TickManager.TicksGame < this.ticksWhenRainAllowedAgain)
			{
				return 0f;
			}
			if (weather.rainRate > 0.1f)
			{
				if (this.map.gameConditionManager.ActiveConditions.Any((GameCondition x) => x.def.preventRain))
				{
					return 0f;
				}
			}
			if (ModsConfig.AnomalyActive && weather.minMonolithLevel > Find.Anomaly.HighestLevelReached && (!weather.canOccurInAmbientHorror || !Find.Anomaly.AmbientHorrorMode))
			{
				return 0f;
			}
			BiomeDef biome = this.map.Biome;
			float result = 0f;
			for (int i = 0; i < biome.baseWeatherCommonalities.Count; i++)
			{
				WeatherCommonalityRecord weatherCommonalityRecord = biome.baseWeatherCommonalities[i];
				if (weatherCommonalityRecord.weather == weather)
				{
					float num = weatherCommonalityRecord.commonality;
					if (this.map.fireWatcher.LargeFireDangerPresent && weather.rainRate > 0.1f)
					{
						num *= 15f;
					}
					if (weatherCommonalityRecord.weather.commonalityRainfallFactor != null)
					{
						num *= weatherCommonalityRecord.weather.commonalityRainfallFactor.Evaluate(this.map.TileInfo.rainfall);
					}
					foreach (GameCondition gameCondition in this.map.gameConditionManager.ActiveConditions)
					{
						num *= gameCondition.WeatherCommonalityFactor(weather, this.map);
					}
					result = num;
					break;
				}
			}
			PlanetTile tile = this.map.Tile;
			for (int j = 0; j < this.map.TileInfo.Mutators.Count; j++)
			{
				TileMutatorWorker worker = this.map.TileInfo.Mutators[j].Worker;
				if (worker != null)
				{
					worker.MutateWeatherCommonalityFor(weather, tile, ref result);
				}
			}
			return result;
		}

		// Token: 0x0600BC1E RID: 48158 RVA: 0x003687DC File Offset: 0x003669DC
		public void LogWeatherChances()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (WeatherDef weatherDef in from w in DefDatabase<WeatherDef>.AllDefs
			orderby this.CurrentWeatherCommonality(w) descending
			select w)
			{
				stringBuilder.AppendLine(weatherDef.label + " - " + this.CurrentWeatherCommonality(weatherDef).ToString());
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04008187 RID: 33159
		private Map map;

		// Token: 0x04008188 RID: 33160
		private int curWeatherDuration = 10000;

		// Token: 0x04008189 RID: 33161
		private int ticksWhenRainAllowedAgain;

		// Token: 0x0400818A RID: 33162
		private const int FirstWeatherDuration = 10000;

		// Token: 0x0400818B RID: 33163
		private const float ChanceFactorRainOnFire = 15f;

		// Token: 0x0400818C RID: 33164
		private static List<GameCondition> allConditionsTmp = new List<GameCondition>();
	}
}
