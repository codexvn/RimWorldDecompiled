using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020004D7 RID: 1239
	public static class CaravanPollutionUtility
	{
		// Token: 0x06002007 RID: 8199 RVA: 0x000A78A0 File Offset: 0x000A5AA0
		public static void CheckDamageFromPollution(Caravan caravan, int delta)
		{
			if (!caravan.IsHashIntervalTick(3451, delta))
			{
				return;
			}
			if (Find.WorldGrid[caravan.Tile].PollutionLevel() < PollutionLevel.Moderate)
			{
				return;
			}
			float extraFactor = CaravanPollutionUtility.ToxicDamagePollutionFactor(caravan.Tile);
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				ToxicUtility.DoPawnToxicDamage(pawnsListForReading[i], extraFactor);
			}
		}

		// Token: 0x06002008 RID: 8200 RVA: 0x000A7906 File Offset: 0x000A5B06
		public static float ToxicDamagePollutionFactor(PlanetTile tile)
		{
			if (Find.WorldGrid[tile].PollutionLevel() == PollutionLevel.Moderate)
			{
				return 0.5f;
			}
			return 1f;
		}

		// Token: 0x04001B69 RID: 7017
		private const float ModeratePollutionToxicDamageFactor = 0.5f;
	}
}
