using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200301A RID: 12314
	public class Alert_NeedBatteries : Alert
	{
		// Token: 0x0601152E RID: 70958 RVA: 0x004F7DF8 File Offset: 0x004F5FF8
		public Alert_NeedBatteries()
		{
			this.defaultLabel = "NeedBatteries".Translate();
			this.defaultExplanation = "NeedBatteriesDesc".Translate();
		}

		// Token: 0x0601152F RID: 70959 RVA: 0x004F7E2C File Offset: 0x004F602C
		public override TaggedString GetExplanation()
		{
			string text = this.defaultExplanation + "\n\n";
			if (ResearchProjectDefOf.Batteries.IsFinished)
			{
				text += "NeedBatteriesResearchedDesc".Translate();
			}
			else
			{
				text += "NeedBatteriesNotResearchedDesc".Translate();
			}
			return text;
		}

		// Token: 0x06011530 RID: 70960 RVA: 0x004F7E8C File Offset: 0x004F608C
		public override AlertReport GetReport()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (this.NeedBatteries(maps[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06011531 RID: 70961 RVA: 0x004F7ECC File Offset: 0x004F60CC
		private bool NeedBatteries(Map map)
		{
			if (!map.IsPlayerHome)
			{
				return false;
			}
			return !map.listerBuildings.ColonistsHaveBuilding((Thing building) => building is Building_Battery) && (map.listerBuildings.ColonistsHaveBuilding(ThingDefOf.SolarGenerator) || map.listerBuildings.ColonistsHaveBuilding(ThingDefOf.WindTurbine)) && !map.listerBuildings.ColonistsHaveBuilding(ThingDefOf.GeothermalGenerator) && !map.listerBuildings.ColonistsHaveBuilding(ThingDefOf.WatermillGenerator);
		}
	}
}
