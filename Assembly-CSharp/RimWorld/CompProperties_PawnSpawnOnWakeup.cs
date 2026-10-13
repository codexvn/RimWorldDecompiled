using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D99 RID: 11673
	public class CompProperties_PawnSpawnOnWakeup : CompProperties
	{
		// Token: 0x060106A2 RID: 67234 RVA: 0x004BC8D4 File Offset: 0x004BAAD4
		public CompProperties_PawnSpawnOnWakeup()
		{
			this.compClass = typeof(CompPawnSpawnOnWakeup);
		}

		// Token: 0x0400AB40 RID: 43840
		public List<PawnKindDef> spawnablePawnKinds;

		// Token: 0x0400AB41 RID: 43841
		public SoundDef spawnSound;

		// Token: 0x0400AB42 RID: 43842
		public EffecterDef spawnEffecter;

		// Token: 0x0400AB43 RID: 43843
		public Type lordJob;

		// Token: 0x0400AB44 RID: 43844
		public bool shouldJoinParentLord;

		// Token: 0x0400AB45 RID: 43845
		[NoTranslate]
		public string activatedMessageKey;

		// Token: 0x0400AB46 RID: 43846
		public FloatRange points;

		// Token: 0x0400AB47 RID: 43847
		public IntRange pawnSpawnRadius = new IntRange(2, 2);

		// Token: 0x0400AB48 RID: 43848
		public bool aggressive = true;

		// Token: 0x0400AB49 RID: 43849
		public bool dropInPods;

		// Token: 0x0400AB4A RID: 43850
		public float defendRadius = 21f;

		// Token: 0x0400AB4B RID: 43851
		public MentalStateDef mentalState;

		// Token: 0x0400AB4C RID: 43852
		public bool destroyAfterSpawn;
	}
}
