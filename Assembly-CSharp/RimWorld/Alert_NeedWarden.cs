using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FEB RID: 12267
	public class Alert_NeedWarden : Alert
	{
		// Token: 0x06011472 RID: 70770 RVA: 0x004F3420 File Offset: 0x004F1620
		public Alert_NeedWarden()
		{
			this.defaultLabel = "NeedWarden".Translate();
			this.defaultExplanation = "NeedWardenDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06011473 RID: 70771 RVA: 0x004F345C File Offset: 0x004F165C
		public override AlertReport GetReport()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map.IsPlayerHome && map.mapPawns.PrisonersOfColonySpawned.Any<Pawn>())
				{
					bool flag = false;
					foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
					{
						if ((pawn.Spawned || pawn.BrieflyDespawned()) && !pawn.Downed && pawn.workSettings != null && pawn.workSettings.GetPriority(WorkTypeDefOf.Warden) > 0)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return AlertReport.CulpritIs(map.mapPawns.PrisonersOfColonySpawned[0]);
					}
				}
			}
			return false;
		}
	}
}
