using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003026 RID: 12326
	public class Alert_CannotBeUsedRoofed : Alert
	{
		// Token: 0x17002CEF RID: 11503
		// (get) Token: 0x06011557 RID: 70999 RVA: 0x004F89A8 File Offset: 0x004F6BA8
		private List<Thing> UnusableBuildings
		{
			get
			{
				this.unusableBuildingsResult.Clear();
				if (this.thingDefsToCheck == null)
				{
					this.thingDefsToCheck = new List<ThingDef>();
					foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
					{
						if (!thingDef.canBeUsedUnderRoof)
						{
							this.thingDefsToCheck.Add(thingDef);
						}
					}
				}
				List<Map> maps = Find.Maps;
				Faction ofPlayer = Faction.OfPlayer;
				for (int i = 0; i < this.thingDefsToCheck.Count; i++)
				{
					for (int j = 0; j < maps.Count; j++)
					{
						List<Thing> list = maps[j].listerThings.ThingsOfDef(this.thingDefsToCheck[i]);
						for (int k = 0; k < list.Count; k++)
						{
							if (list[k].Faction == ofPlayer && RoofUtility.IsAnyCellUnderRoof(list[k]))
							{
								this.unusableBuildingsResult.Add(list[k]);
							}
						}
					}
				}
				return this.unusableBuildingsResult;
			}
		}

		// Token: 0x06011558 RID: 71000 RVA: 0x004F8AD8 File Offset: 0x004F6CD8
		public Alert_CannotBeUsedRoofed()
		{
			this.defaultLabel = "BuildingCantBeUsedRoofed".Translate();
			this.defaultExplanation = "BuildingCantBeUsedRoofedDesc".Translate();
		}

		// Token: 0x06011559 RID: 71001 RVA: 0x004F8B15 File Offset: 0x004F6D15
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.UnusableBuildings);
		}

		// Token: 0x0400B38C RID: 45964
		private List<ThingDef> thingDefsToCheck;

		// Token: 0x0400B38D RID: 45965
		private List<Thing> unusableBuildingsResult = new List<Thing>();
	}
}
