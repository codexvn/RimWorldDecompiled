using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020020F0 RID: 8432
	public class TileMutatorWorker_IncreaseWeatherFrequency : TileMutatorWorker
	{
		// Token: 0x0600B829 RID: 47145 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_IncreaseWeatherFrequency(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B82A RID: 47146 RVA: 0x00351FB4 File Offset: 0x003501B4
		public override void MutateWeatherCommonalityFor(WeatherDef weather, PlanetTile tile, ref float commonality)
		{
			if (this.def.weathersToAffect.Contains(weather))
			{
				commonality += this.def.weatherFrequencyOffset;
				commonality *= this.def.weatherFrequencyFactor;
			}
		}
	}
}
