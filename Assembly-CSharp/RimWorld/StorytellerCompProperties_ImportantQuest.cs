using System;

namespace RimWorld
{
	// Token: 0x02001E4A RID: 7754
	public class StorytellerCompProperties_ImportantQuest : StorytellerCompProperties
	{
		// Token: 0x0600AC7C RID: 44156 RVA: 0x003146F7 File Offset: 0x003128F7
		public StorytellerCompProperties_ImportantQuest()
		{
			this.compClass = typeof(StorytellerComp_ImportantQuest);
		}

		// Token: 0x0400769A RID: 30362
		public IncidentDef questIncident;

		// Token: 0x0400769B RID: 30363
		public QuestScriptDef questDef;

		// Token: 0x0400769C RID: 30364
		public int fireAfterDaysPassed;
	}
}
