using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02001E4B RID: 7755
	public class StorytellerCompProperties_MechanitorComplexQuest : StorytellerCompProperties
	{
		// Token: 0x0600AC7D RID: 44157 RVA: 0x0031470F File Offset: 0x0031290F
		public StorytellerCompProperties_MechanitorComplexQuest()
		{
			this.compClass = typeof(StorytellerComp_MechanitorComplexQuest);
		}

		// Token: 0x0400769D RID: 30365
		public IncidentDef incident;

		// Token: 0x0400769E RID: 30366
		public int mtbDays = 60;

		// Token: 0x0400769F RID: 30367
		public float minSpacingDays;

		// Token: 0x040076A0 RID: 30368
		public float existingMechanitorOrMechlinkMTBFactor = 1f;

		// Token: 0x040076A1 RID: 30369
		public List<QuestScriptDef> blockedByQueuedOrActiveQuests;
	}
}
