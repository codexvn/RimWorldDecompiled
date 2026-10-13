using System;

namespace RimWorld
{
	// Token: 0x02001E2F RID: 7727
	public class StorytellerCompProperties_CategoryIndividualMTBByBiome : StorytellerCompProperties
	{
		// Token: 0x0600AC0F RID: 44047 RVA: 0x003135A7 File Offset: 0x003117A7
		public StorytellerCompProperties_CategoryIndividualMTBByBiome()
		{
			this.compClass = typeof(StorytellerComp_CategoryIndividualMTBByBiome);
		}

		// Token: 0x0400764F RID: 30287
		public IncidentCategoryDef category;

		// Token: 0x04007650 RID: 30288
		public bool applyCaravanVisibility;
	}
}
