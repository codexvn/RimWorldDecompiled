using System;

namespace RimWorld
{
	// Token: 0x02001E4E RID: 7758
	public class StorytellerCompProperties_MonolithMigration : StorytellerCompProperties
	{
		// Token: 0x0600AC8A RID: 44170 RVA: 0x003149C7 File Offset: 0x00312BC7
		public StorytellerCompProperties_MonolithMigration()
		{
			this.compClass = typeof(StorytellerComp_MonolithMigration);
		}

		// Token: 0x040076A8 RID: 30376
		public float mtbDays;

		// Token: 0x040076A9 RID: 30377
		public IncidentDef incident;
	}
}
