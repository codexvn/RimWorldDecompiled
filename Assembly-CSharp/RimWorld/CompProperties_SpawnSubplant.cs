using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E4A RID: 11850
	public class CompProperties_SpawnSubplant : CompProperties
	{
		// Token: 0x0400ADDE RID: 44510
		public ThingDef subplant;

		// Token: 0x0400ADDF RID: 44511
		[NoTranslate]
		public string saveKeysPrefix;

		// Token: 0x0400ADE0 RID: 44512
		public SoundDef spawnSound;

		// Token: 0x0400ADE1 RID: 44513
		public float maxRadius;

		// Token: 0x0400ADE2 RID: 44514
		public float subplantSpawnDays;

		// Token: 0x0400ADE3 RID: 44515
		public float minGrowthForSpawn = 0.2f;

		// Token: 0x0400ADE4 RID: 44516
		public FloatRange? initialGrowthRange;

		// Token: 0x0400ADE5 RID: 44517
		public bool canSpawnOverPlayerSownPlants = true;

		// Token: 0x0400ADE6 RID: 44518
		public List<ThingDef> plantsToNotOverwrite;

		// Token: 0x0400ADE7 RID: 44519
		public SimpleCurve chanceOverDistance;

		// Token: 0x0400ADE8 RID: 44520
		public ThingDef dontWipePlant;

		// Token: 0x0400ADE9 RID: 44521
		public int maxPlants = -1;

		// Token: 0x0400ADEA RID: 44522
		public int checkRespawnIntervalHours = 18;
	}
}
