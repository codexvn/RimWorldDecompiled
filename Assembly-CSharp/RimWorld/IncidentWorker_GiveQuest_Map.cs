using System;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E21 RID: 7713
	public class IncidentWorker_GiveQuest_Map : IncidentWorker_GiveQuest
	{
		// Token: 0x0600AB92 RID: 43922 RVA: 0x00311074 File Offset: 0x0030F274
		protected override void GiveQuest(IncidentParms parms, QuestScriptDef questDef)
		{
			Slate slate = new Slate();
			slate.Set<float>("points", parms.points, false);
			slate.Set<Map>("map", (Map)parms.target, false);
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(questDef, slate);
			if (quest.root.sendAvailableLetter)
			{
				QuestUtility.SendLetterQuestAvailable(quest, null);
			}
		}
	}
}
