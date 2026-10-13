using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FEA RID: 12266
	public class Alert_NeedMiner : Alert
	{
		// Token: 0x0601146F RID: 70767 RVA: 0x004F324F File Offset: 0x004F144F
		public Alert_NeedMiner()
		{
			this.defaultLabel = "NeedMiner".Translate();
			this.defaultExplanation = "NeedMinerDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06011470 RID: 70768 RVA: 0x004F3288 File Offset: 0x004F1488
		public override AlertReport GetReport()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map.IsPlayerHome)
				{
					Alert_NeedMiner.tmpDesignations.Clear();
					Alert_NeedMiner.tmpDesignations.AddRange(map.designationManager.designationsByDef[DesignationDefOf.Mine]);
					Alert_NeedMiner.tmpDesignations.AddRange(map.designationManager.designationsByDef[DesignationDefOf.MineVein]);
					if (!Alert_NeedMiner.tmpDesignations.NullOrEmpty<Designation>())
					{
						bool flag = false;
						foreach (Pawn pawn in map.mapPawns.PawnsInFaction(Faction.OfPlayer))
						{
							if ((pawn.Spawned || pawn.BrieflyDespawned()) && !pawn.Downed)
							{
								if (pawn.IsFreeColonist && pawn.workSettings != null && pawn.workSettings.GetPriority(WorkTypeDefOf.Mining) > 0)
								{
									flag = true;
									break;
								}
								if (pawn.IsColonyMechPlayerControlled && pawn.RaceProps.mechEnabledWorkTypes.Contains(WorkTypeDefOf.Mining) && pawn.RaceProps.mechFixedSkillLevel > 0)
								{
									flag = true;
									break;
								}
							}
						}
						if (!flag)
						{
							return AlertReport.CulpritIs(new GlobalTargetInfo(Alert_NeedMiner.tmpDesignations[0].target.Cell, map, false));
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0400B323 RID: 45859
		private static List<Designation> tmpDesignations = new List<Designation>();
	}
}
