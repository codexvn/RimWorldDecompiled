using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200284B RID: 10315
	public class SitePartWorker_RaidSource : SitePartWorker_Outpost
	{
		// Token: 0x0600E195 RID: 57749 RVA: 0x004220F0 File Offset: 0x004202F0
		public override void SitePartWorkerTick(SitePart sitePart)
		{
			base.SitePartWorkerTick(sitePart);
			if (sitePart.lastRaidTick == -1 || (float)Find.TickManager.TicksGame > (float)sitePart.lastRaidTick + 90000f)
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome && sitePart.site.IsHashIntervalTick(2500) && Rand.MTBEventOccurs(QuestTuning.PointsToRaidSourceRaidsMTBDaysCurve.Evaluate(sitePart.parms.threatPoints), 60000f, 2500f))
					{
						this.StartRaid(maps[i], sitePart);
					}
				}
			}
		}

		// Token: 0x0600E196 RID: 57750 RVA: 0x00422194 File Offset: 0x00420394
		private void StartRaid(Map map, SitePart sitePart)
		{
			IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, map);
			incidentParms.forced = true;
			incidentParms.points *= 0.6f;
			if (IncidentDefOf.RaidEnemy.Worker.CanFireNow(incidentParms))
			{
				IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms);
				sitePart.lastRaidTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x0600E197 RID: 57751 RVA: 0x004221FC File Offset: 0x004203FC
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			int enemiesCount = base.GetEnemiesCount(part.site, part.parms);
			float num = QuestTuning.PointsToRaidSourceRaidsMTBDaysCurve.Evaluate(part.parms.threatPoints);
			outExtraDescriptionRules.Add(new Rule_String("enemiesCount", enemiesCount.ToString()));
			outExtraDescriptionRules.Add(new Rule_String("mtbDays", ((int)(num * 60000f)).ToStringTicksToPeriod(true, false, false, true, false)));
			outExtraDescriptionRules.Add(new Rule_String("enemiesLabel", base.GetEnemiesLabel(part.site, enemiesCount)));
		}
	}
}
