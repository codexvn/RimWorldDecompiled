using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C57 RID: 11351
	public class CompProperties_DestroyNearbyPlantsOnSpawn : CompProperties
	{
		// Token: 0x0600FE8E RID: 65166 RVA: 0x0049C961 File Offset: 0x0049AB61
		public CompProperties_DestroyNearbyPlantsOnSpawn()
		{
			this.compClass = typeof(CompDestroyNearbyPlantsOnSpawn);
		}

		// Token: 0x0400A6C6 RID: 42694
		public int radius;
	}
}
