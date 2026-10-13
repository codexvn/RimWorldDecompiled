using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003022 RID: 12322
	public class Alert_BilliardsTableOnWall : Alert
	{
		// Token: 0x17002CEA RID: 11498
		// (get) Token: 0x0601154B RID: 70987 RVA: 0x004F86EC File Offset: 0x004F68EC
		private List<Thing> BadTables
		{
			get
			{
				this.badTablesResult.Clear();
				List<Map> maps = Find.Maps;
				Faction ofPlayer = Faction.OfPlayer;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThingDefOf.BilliardsTable);
					for (int j = 0; j < list.Count; j++)
					{
						if (list[j].Faction == ofPlayer && !JoyGiver_PlayBilliards.ThingHasStandableSpaceOnAllSides(list[j]))
						{
							this.badTablesResult.Add(list[j]);
						}
					}
				}
				return this.badTablesResult;
			}
		}

		// Token: 0x0601154C RID: 70988 RVA: 0x004F8785 File Offset: 0x004F6985
		public Alert_BilliardsTableOnWall()
		{
			this.defaultLabel = "BilliardsNeedsSpace".Translate();
			this.defaultExplanation = "BilliardsNeedsSpaceDesc".Translate();
		}

		// Token: 0x0601154D RID: 70989 RVA: 0x004F87C2 File Offset: 0x004F69C2
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadTables);
		}

		// Token: 0x0400B38A RID: 45962
		private List<Thing> badTablesResult = new List<Thing>();
	}
}
