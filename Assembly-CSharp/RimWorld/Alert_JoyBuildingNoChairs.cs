using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003023 RID: 12323
	public abstract class Alert_JoyBuildingNoChairs : Alert
	{
		// Token: 0x17002CEB RID: 11499
		// (get) Token: 0x0601154E RID: 70990
		protected abstract JoyGiverDef JoyGiver { get; }

		// Token: 0x17002CEC RID: 11500
		// (get) Token: 0x0601154F RID: 70991 RVA: 0x004F87D0 File Offset: 0x004F69D0
		private List<Thing> BadBuildings
		{
			get
			{
				this.badBuildingsResult.Clear();
				List<Map> maps = Find.Maps;
				foreach (ThingDef def in this.JoyGiver.thingDefs)
				{
					for (int i = 0; i < maps.Count; i++)
					{
						foreach (Thing thing in maps[i].listerThings.ThingsOfDef(def))
						{
							if (thing.Faction == Faction.OfPlayer && !this.JoyBuildingUsable(thing))
							{
								this.badBuildingsResult.Add(thing);
							}
						}
					}
				}
				return this.badBuildingsResult;
			}
		}

		// Token: 0x06011550 RID: 70992 RVA: 0x004F88BC File Offset: 0x004F6ABC
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadBuildings);
		}

		// Token: 0x06011551 RID: 70993 RVA: 0x004F88CC File Offset: 0x004F6ACC
		private bool JoyBuildingUsable(Thing t)
		{
			List<IntVec3> list = GenAdjFast.AdjacentCellsCardinal(t);
			for (int i = 0; i < list.Count; i++)
			{
				Building edifice = list[i].GetEdifice(t.Map);
				if (edifice != null && edifice.def.building.isSittable)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400B38B RID: 45963
		private List<Thing> badBuildingsResult = new List<Thing>();
	}
}
