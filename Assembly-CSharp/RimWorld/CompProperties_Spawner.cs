using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E24 RID: 11812
	public class CompProperties_Spawner : CompProperties
	{
		// Token: 0x06010A1F RID: 68127 RVA: 0x004CC079 File Offset: 0x004CA279
		public CompProperties_Spawner()
		{
			this.compClass = typeof(CompSpawner);
		}

		// Token: 0x0400AD57 RID: 44375
		public ThingDef thingToSpawn;

		// Token: 0x0400AD58 RID: 44376
		public int spawnCount = 1;

		// Token: 0x0400AD59 RID: 44377
		public IntRange spawnIntervalRange = new IntRange(100, 100);

		// Token: 0x0400AD5A RID: 44378
		public int spawnMaxAdjacent = -1;

		// Token: 0x0400AD5B RID: 44379
		public bool spawnForbidden;

		// Token: 0x0400AD5C RID: 44380
		public bool requiresPower;

		// Token: 0x0400AD5D RID: 44381
		public bool writeTimeLeftToSpawn;

		// Token: 0x0400AD5E RID: 44382
		public bool showMessageIfOwned;

		// Token: 0x0400AD5F RID: 44383
		[NoTranslate]
		public string saveKeysPrefix;

		// Token: 0x0400AD60 RID: 44384
		public bool inheritFaction;
	}
}
