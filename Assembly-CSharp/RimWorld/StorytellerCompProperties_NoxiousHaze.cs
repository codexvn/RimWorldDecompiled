using System;

namespace RimWorld
{
	// Token: 0x02001E52 RID: 7762
	public class StorytellerCompProperties_NoxiousHaze : StorytellerCompProperties
	{
		// Token: 0x0600AC99 RID: 44185 RVA: 0x00314B9F File Offset: 0x00312D9F
		public StorytellerCompProperties_NoxiousHaze()
		{
			this.compClass = typeof(StorytellerComp_NoxiousHaze);
		}

		// Token: 0x040076B2 RID: 30386
		public float lastFireMinMTBThreshold = 0.5f;

		// Token: 0x040076B3 RID: 30387
		public float lastFireMaxMTBThreshold = 2f;
	}
}
