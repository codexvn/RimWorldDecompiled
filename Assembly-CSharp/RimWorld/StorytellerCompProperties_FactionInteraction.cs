using System;

namespace RimWorld
{
	// Token: 0x02001E42 RID: 7746
	public class StorytellerCompProperties_FactionInteraction : StorytellerCompProperties
	{
		// Token: 0x0600AC55 RID: 44117 RVA: 0x0031410B File Offset: 0x0031230B
		public StorytellerCompProperties_FactionInteraction()
		{
			this.compClass = typeof(StorytellerComp_FactionInteraction);
		}

		// Token: 0x0400767D RID: 30333
		public IncidentDef incident;

		// Token: 0x0400767E RID: 30334
		public float baseIncidentsPerYear;

		// Token: 0x0400767F RID: 30335
		public float minSpacingDays;

		// Token: 0x04007680 RID: 30336
		public StoryDanger minDanger;

		// Token: 0x04007681 RID: 30337
		public bool fullAlliesOnly;

		// Token: 0x04007682 RID: 30338
		public float minWealth;
	}
}
