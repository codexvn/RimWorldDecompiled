using System;

namespace RimWorld
{
	// Token: 0x02001E65 RID: 7781
	public class StorytellerComp_RandomQuest : StorytellerComp_OnOffCycle
	{
		// Token: 0x0600ACE5 RID: 44261 RVA: 0x00315A2C File Offset: 0x00313C2C
		public override IncidentParms GenerateParms(IncidentCategoryDef incCat, IIncidentTarget target)
		{
			IncidentParms incidentParms = base.GenerateParms(incCat, target);
			incidentParms.questScriptDef = NaturalRandomQuestChooser.ChooseNaturalRandomQuest(incidentParms.points, target);
			return incidentParms;
		}
	}
}
