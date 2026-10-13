using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02002842 RID: 10306
	public class SitePartWorker_ConditionCauser_WeatherController : SitePartWorker_ConditionCauser
	{
		// Token: 0x0600E178 RID: 57720 RVA: 0x00421B54 File Offset: 0x0041FD54
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			WeatherDef weather = part.conditionCauser.TryGetComp<CompCauseGameCondition_ForceWeather>().weather;
			outExtraDescriptionRules.AddRange(GrammarUtility.RulesForDef("weather", weather));
		}
	}
}
