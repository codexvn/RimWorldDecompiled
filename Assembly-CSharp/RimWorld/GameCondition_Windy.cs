using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D55 RID: 7509
	public class GameCondition_Windy : GameCondition
	{
		// Token: 0x0600A853 RID: 43091 RVA: 0x00302CA4 File Offset: 0x00300EA4
		public override float MinWindSpeed()
		{
			return 1.5f;
		}

		// Token: 0x0600A854 RID: 43092 RVA: 0x00302CAB File Offset: 0x00300EAB
		public override float WeatherCommonalityFactor(WeatherDef weather, Map map)
		{
			if (weather == WeatherDefOf.Fog || weather == WeatherDefOf.FoggyRain)
			{
				return 0f;
			}
			return 1f;
		}
	}
}
