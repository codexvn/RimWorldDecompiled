using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E5B RID: 7771
	public class StorytellerComp_RandomEpicQuest : StorytellerComp_OnOffCycle
	{
		// Token: 0x0600ACC0 RID: 44224 RVA: 0x003153B3 File Offset: 0x003135B3
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].root.IsEpic && (questsListForReading[i].State == QuestState.NotYetAccepted || questsListForReading[i].State == QuestState.Ongoing))
				{
					yield break;
				}
			}
			foreach (FiringIncident firingIncident in base.MakeIntervalIncidents(target))
			{
				yield return firingIncident;
			}
			IEnumerator<FiringIncident> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600ACC1 RID: 44225 RVA: 0x003153CC File Offset: 0x003135CC
		public override IncidentParms GenerateParms(IncidentCategoryDef incCat, IIncidentTarget target)
		{
			IncidentParms parms = base.GenerateParms(incCat, target);
			QuestScriptDef questScriptDef;
			if ((from x in DefDatabase<QuestScriptDef>.AllDefs
			where x.IsEpic && x.CanRun(parms.points, target)
			select x).TryRandomElement(out questScriptDef))
			{
				parms.questScriptDef = questScriptDef;
			}
			return parms;
		}
	}
}
