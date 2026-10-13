using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FE7 RID: 12263
	public class Alert_NeedDefenses : Alert
	{
		// Token: 0x06011462 RID: 70754 RVA: 0x004F2D04 File Offset: 0x004F0F04
		public Alert_NeedDefenses()
		{
			this.defaultLabel = "NeedDefenses".Translate();
			this.defaultExplanation = "NeedDefensesDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06011463 RID: 70755 RVA: 0x004F2D40 File Offset: 0x004F0F40
		public override AlertReport GetReport()
		{
			if (GenDate.DaysPassed < 2 || GenDate.DaysPassed > 5)
			{
				return false;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (this.NeedDefenses(maps[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06011464 RID: 70756 RVA: 0x004F2D98 File Offset: 0x004F0F98
		private bool NeedDefenses(Map map)
		{
			if (!map.IsPlayerHome)
			{
				return false;
			}
			if (!map.mapPawns.AnyColonistSpawned && !map.listerBuildings.allBuildingsColonist.Any<Building>())
			{
				return false;
			}
			List<Building> allBuildingsColonist = map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				Building building = allBuildingsColonist[i];
				if ((building.def.building != null && (building.def.building.IsTurret || building.def.building.isTrap)) || building.def == ThingDefOf.Sandbags || building.def == ThingDefOf.Barricade)
				{
					return false;
				}
			}
			return true;
		}
	}
}
