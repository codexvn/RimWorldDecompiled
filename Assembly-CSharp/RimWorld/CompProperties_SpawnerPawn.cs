using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E38 RID: 11832
	public class CompProperties_SpawnerPawn : CompProperties
	{
		// Token: 0x06010A89 RID: 68233 RVA: 0x004CD6A4 File Offset: 0x004CB8A4
		public CompProperties_SpawnerPawn()
		{
			this.compClass = typeof(CompSpawnerPawn);
		}

		// Token: 0x0400AD9A RID: 44442
		public List<PawnKindDef> spawnablePawnKinds;

		// Token: 0x0400AD9B RID: 44443
		public SoundDef spawnSound;

		// Token: 0x0400AD9C RID: 44444
		[NoTranslate]
		public string spawnMessageKey;

		// Token: 0x0400AD9D RID: 44445
		[NoTranslate]
		public string noPawnsLeftToSpawnKey;

		// Token: 0x0400AD9E RID: 44446
		[NoTranslate]
		public string pawnsLeftToSpawnKey;

		// Token: 0x0400AD9F RID: 44447
		public bool showNextSpawnInInspect;

		// Token: 0x0400ADA0 RID: 44448
		public bool shouldJoinParentLord;

		// Token: 0x0400ADA1 RID: 44449
		public Type lordJob;

		// Token: 0x0400ADA2 RID: 44450
		public float defendRadius = 21f;

		// Token: 0x0400ADA3 RID: 44451
		public float lordJoinRadius = 2.1474836E+09f;

		// Token: 0x0400ADA4 RID: 44452
		public int initialPawnsCount;

		// Token: 0x0400ADA5 RID: 44453
		public float initialPawnsPoints;

		// Token: 0x0400ADA6 RID: 44454
		public float maxSpawnedPawnsPoints = -1f;

		// Token: 0x0400ADA7 RID: 44455
		public FloatRange pawnSpawnIntervalDays = new FloatRange(0.85f, 1.15f);

		// Token: 0x0400ADA8 RID: 44456
		public int pawnSpawnRadius = 2;

		// Token: 0x0400ADA9 RID: 44457
		public IntRange maxPawnsToSpawn = IntRange.Zero;

		// Token: 0x0400ADAA RID: 44458
		public bool chooseSingleTypeToSpawn;

		// Token: 0x0400ADAB RID: 44459
		[NoTranslate]
		public string nextSpawnInspectStringKey;

		// Token: 0x0400ADAC RID: 44460
		[NoTranslate]
		public string nextSpawnInspectStringKeyDormant;
	}
}
