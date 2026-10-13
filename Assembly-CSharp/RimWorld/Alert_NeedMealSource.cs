using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003017 RID: 12311
	public class Alert_NeedMealSource : Alert
	{
		// Token: 0x06011522 RID: 70946 RVA: 0x004F7958 File Offset: 0x004F5B58
		public Alert_NeedMealSource()
		{
			this.defaultLabel = "NeedMealSource".Translate();
			this.defaultExplanation = "NeedMealSourceDesc".Translate();
		}

		// Token: 0x06011523 RID: 70947 RVA: 0x004F798C File Offset: 0x004F5B8C
		public override AlertReport GetReport()
		{
			if (GenDate.DaysPassed < 2)
			{
				return false;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (this.NeedMealSource(maps[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06011524 RID: 70948 RVA: 0x004F79DC File Offset: 0x004F5BDC
		private bool NeedMealSource(Map map)
		{
			if (!map.IsPlayerHome)
			{
				return false;
			}
			if (!map.mapPawns.AnyColonistSpawned)
			{
				return false;
			}
			List<Building> allBuildingsColonist = map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				if (allBuildingsColonist[i].def.building.isMealSource)
				{
					return false;
				}
			}
			return true;
		}
	}
}
