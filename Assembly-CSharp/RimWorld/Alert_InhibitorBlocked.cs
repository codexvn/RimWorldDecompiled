using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200305A RID: 12378
	public class Alert_InhibitorBlocked : Alert
	{
		// Token: 0x06011626 RID: 71206 RVA: 0x004FCBCC File Offset: 0x004FADCC
		public Alert_InhibitorBlocked()
		{
			this.defaultLabel = "AlertInhibitorBlocked".Translate();
			this.defaultExplanation = "AlertInhibitorBlockedDesc".Translate();
			this.requireAnomaly = true;
		}

		// Token: 0x06011627 RID: 71207 RVA: 0x004FCC1B File Offset: 0x004FAE1B
		public override AlertReport GetReport()
		{
			this.GetTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x06011628 RID: 71208 RVA: 0x004FCC30 File Offset: 0x004FAE30
		private void GetTargets()
		{
			this.targets.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				foreach (Building building in maps[i].listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.ElectricInhibitor))
				{
					if (building.HasComp<CompFacility>() && ContainmentUtility.IsLinearBuildingBlocked(building.def, building.Position, building.Rotation, building.Map, null))
					{
						this.targets.Add(building);
					}
				}
			}
		}

		// Token: 0x0400B3F3 RID: 46067
		public readonly List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
