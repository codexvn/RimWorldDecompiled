using System;

namespace RimWorld
{
	// Token: 0x02001E45 RID: 7749
	public class StorytellerCompProperties_GauranlenPodSpawn : StorytellerCompProperties
	{
		// Token: 0x0600AC62 RID: 44130 RVA: 0x00314377 File Offset: 0x00312577
		public StorytellerCompProperties_GauranlenPodSpawn()
		{
			this.compClass = typeof(StorytellerComp_GauranlenPodSpawn);
		}

		// Token: 0x0400768B RID: 30347
		public float daysBetweenPodSpawns;

		// Token: 0x0400768C RID: 30348
		public int countdownFactorAnyConnectors = 1;

		// Token: 0x0400768D RID: 30349
		public IncidentDef incident;
	}
}
