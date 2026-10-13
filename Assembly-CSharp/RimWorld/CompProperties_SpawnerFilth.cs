using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E29 RID: 11817
	public class CompProperties_SpawnerFilth : CompProperties
	{
		// Token: 0x06010A44 RID: 68164 RVA: 0x004CC930 File Offset: 0x004CAB30
		public CompProperties_SpawnerFilth()
		{
			this.compClass = typeof(CompSpawnerFilth);
		}

		// Token: 0x0400AD69 RID: 44393
		public ThingDef filthDef;

		// Token: 0x0400AD6A RID: 44394
		public int spawnCountOnSpawn = 5;

		// Token: 0x0400AD6B RID: 44395
		public float spawnMtbHours = 12f;

		// Token: 0x0400AD6C RID: 44396
		public float spawnRadius = 3f;

		// Token: 0x0400AD6D RID: 44397
		public float spawnEveryDays = -1f;

		// Token: 0x0400AD6E RID: 44398
		public RotStage? requiredRotStage;
	}
}
