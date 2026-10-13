using System;

namespace RimWorld
{
	// Token: 0x02001E71 RID: 7793
	public class StorytellerCompProperties_SingleOnceFixed : StorytellerCompProperties
	{
		// Token: 0x0600AD1C RID: 44316 RVA: 0x0031632F File Offset: 0x0031452F
		public StorytellerCompProperties_SingleOnceFixed()
		{
			this.compClass = typeof(StorytellerComp_SingleOnceFixed);
		}

		// Token: 0x0400770E RID: 30478
		public IncidentDef incident;

		// Token: 0x0400770F RID: 30479
		public int fireAfterDaysPassed;

		// Token: 0x04007710 RID: 30480
		public RoyalTitleDef skipIfColonistHasMinTitle;

		// Token: 0x04007711 RID: 30481
		public bool skipIfOnExtremeBiome;

		// Token: 0x04007712 RID: 30482
		public int minColonistCount;
	}
}
