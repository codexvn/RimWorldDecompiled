using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200290B RID: 10507
	public class HorrorRelaxTransition : MusicTransition
	{
		// Token: 0x0600E5E6 RID: 58854 RVA: 0x0043135C File Offset: 0x0042F55C
		public override bool IsTransitionSatisfied()
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (!base.IsTransitionSatisfied())
			{
				return false;
			}
			foreach (Map map in Find.Maps)
			{
				if (map.weatherManager.curWeather == WeatherDefOf.GrayPall || map.gameConditionManager.ConditionIsActive(GameConditionDefOf.GrayPall))
				{
					return true;
				}
			}
			foreach (PocketMapParent pocketMapParent in Find.World.pocketMaps)
			{
				if (pocketMapParent.Map != null && HorrorRelaxTransition.IsValidPocketMap(pocketMapParent))
				{
					return true;
				}
			}
			using (List<Map>.Enumerator enumerator = Find.Maps.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (HorrorRelaxTransition.IsValidMap(enumerator.Current))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600E5E7 RID: 58855 RVA: 0x00431484 File Offset: 0x0042F684
		private static bool IsValidPocketMap(PocketMapParent pocketMap)
		{
			return pocketMap.Map.generatorDef == MapGeneratorDefOf.Labyrinth || pocketMap.Map.generatorDef == MapGeneratorDefOf.Undercave;
		}

		// Token: 0x0600E5E8 RID: 58856 RVA: 0x004314AC File Offset: 0x0042F6AC
		private static bool IsValidMap(Map map)
		{
			return map.listerThings.AnyThingWithDef(ThingDefOf.PitGate) || map.listerThings.AnyThingWithDef(ThingDefOf.FleshmassHeart) || map.listerThings.AnyThingWithDef(ThingDefOf.Noctolith);
		}
	}
}
