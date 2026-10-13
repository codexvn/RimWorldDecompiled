using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DA5 RID: 11685
	public class CompProperties_PlantDamager : CompProperties
	{
		// Token: 0x060106E2 RID: 67298 RVA: 0x004BDD8F File Offset: 0x004BBF8F
		public CompProperties_PlantDamager()
		{
			this.compClass = typeof(CompPlantDamager);
		}

		// Token: 0x0400AB65 RID: 43877
		public int ticksBetweenDamage;

		// Token: 0x0400AB66 RID: 43878
		public float radius;

		// Token: 0x0400AB67 RID: 43879
		public bool ignoreAnima = true;

		// Token: 0x0400AB68 RID: 43880
		public float damagePerCycle;

		// Token: 0x0400AB69 RID: 43881
		public int cycleCountOnSpawn;
	}
}
