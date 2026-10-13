using System;

namespace RimWorld
{
	// Token: 0x02001E68 RID: 7784
	public class StorytellerCompProperties_RefiringUniqueQuest : StorytellerCompProperties
	{
		// Token: 0x0600ACF5 RID: 44277 RVA: 0x00315D2B File Offset: 0x00313F2B
		public StorytellerCompProperties_RefiringUniqueQuest()
		{
			this.compClass = typeof(StorytellerComp_RefiringUniqueQuest);
		}

		// Token: 0x040076F6 RID: 30454
		public IncidentDef incident;

		// Token: 0x040076F7 RID: 30455
		public float refireEveryDays = -1f;

		// Token: 0x040076F8 RID: 30456
		public int minColonyWealth = -1;
	}
}
