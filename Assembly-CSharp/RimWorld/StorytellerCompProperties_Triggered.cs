using System;

namespace RimWorld
{
	// Token: 0x02001E75 RID: 7797
	public class StorytellerCompProperties_Triggered : StorytellerCompProperties
	{
		// Token: 0x0600AD2A RID: 44330 RVA: 0x0031652F File Offset: 0x0031472F
		public StorytellerCompProperties_Triggered()
		{
			this.compClass = typeof(StorytellerComp_Triggered);
		}

		// Token: 0x0400771B RID: 30491
		public IncidentDef incident;

		// Token: 0x0400771C RID: 30492
		public int delayTicks = 60;
	}
}
