using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x0200054D RID: 1357
	public class WeatherWorker_TorrentialRain : WeatherWorker
	{
		// Token: 0x0600232F RID: 9007 RVA: 0x000B7209 File Offset: 0x000B5409
		public WeatherWorker_TorrentialRain(WeatherDef def) : base(def)
		{
		}

		// Token: 0x06002330 RID: 9008 RVA: 0x000B7212 File Offset: 0x000B5412
		public override void OnWeatherStart(Map map)
		{
			base.OnWeatherStart(map);
			GenSpawn.Spawn(ThingDefOf.TorrentialRainFlood, map.Center, map, WipeMode.Vanish);
		}

		// Token: 0x06002331 RID: 9009 RVA: 0x000B722E File Offset: 0x000B542E
		public override void OnWeatherEnd(Map map)
		{
			base.OnWeatherEnd(map);
			Thing thing = map.listerThings.ThingsOfDef(ThingDefOf.TorrentialRainFlood).FirstOrDefault<Thing>();
			if (thing == null)
			{
				return;
			}
			thing.Destroy(DestroyMode.Vanish);
		}
	}
}
