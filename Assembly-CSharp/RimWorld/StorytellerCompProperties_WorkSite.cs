using System;

namespace RimWorld
{
	// Token: 0x02001E77 RID: 7799
	public class StorytellerCompProperties_WorkSite : StorytellerCompProperties
	{
		// Token: 0x0600AD2F RID: 44335 RVA: 0x003166CD File Offset: 0x003148CD
		public StorytellerCompProperties_WorkSite()
		{
			this.compClass = typeof(StorytellerComp_WorkSite);
		}

		// Token: 0x0400771D RID: 30493
		public IncidentDef incident;

		// Token: 0x0400771E RID: 30494
		public float baseMtbDays = 5f;
	}
}
