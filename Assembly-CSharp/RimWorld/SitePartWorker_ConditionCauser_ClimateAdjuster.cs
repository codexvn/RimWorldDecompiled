using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02002843 RID: 10307
	public class SitePartWorker_ConditionCauser_ClimateAdjuster : SitePartWorker_ConditionCauser
	{
		// Token: 0x0600E17A RID: 57722 RVA: 0x00421B98 File Offset: 0x0041FD98
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			string output = part.conditionCauser.TryGetComp<CompCauseGameCondition_TemperatureOffset>().temperatureOffset.ToStringTemperatureOffset("F1");
			outExtraDescriptionRules.Add(new Rule_String("temperatureOffset", output));
		}
	}
}
