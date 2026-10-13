using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02003048 RID: 12360
	public class Alert_MechChargerFull : Alert
	{
		// Token: 0x060115E2 RID: 71138 RVA: 0x004FB974 File Offset: 0x004F9B74
		public Alert_MechChargerFull()
		{
			this.defaultLabel = "AlertRechargerFull".Translate();
			this.defaultExplanation = "AlertRechargerFullDesc".Translate();
			this.requireBiotech = true;
		}

		// Token: 0x060115E3 RID: 71139 RVA: 0x004FB9C3 File Offset: 0x004F9BC3
		public override AlertReport GetReport()
		{
			this.GetTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x060115E4 RID: 71140 RVA: 0x004FB9D8 File Offset: 0x004F9BD8
		private void GetTargets()
		{
			this.targets.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				List<Thing> list = maps[i].listerThings.ThingsInGroup(ThingRequestGroup.MechCharger);
				for (int j = 0; j < list.Count; j++)
				{
					Building_MechCharger building_MechCharger = (Building_MechCharger)list[j];
					if (building_MechCharger.Faction == Faction.OfPlayer && building_MechCharger.IsFullOfWaste)
					{
						this.targets.Add(building_MechCharger);
					}
				}
			}
		}

		// Token: 0x0400B3D5 RID: 46037
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
