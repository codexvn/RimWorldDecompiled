using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E45 RID: 11845
	public class CompProperties_SpawnPawnsOnDamaged : CompProperties
	{
		// Token: 0x06010ACC RID: 68300 RVA: 0x004CE8AB File Offset: 0x004CCAAB
		public CompProperties_SpawnPawnsOnDamaged()
		{
			this.compClass = typeof(CompSpawnPawnsOnDamaged);
		}

		// Token: 0x0400ADD3 RID: 44499
		public float chance = 1f;

		// Token: 0x0400ADD4 RID: 44500
		public int cooldownTicks;

		// Token: 0x0400ADD5 RID: 44501
		public IntRange spawnPawnCountRange = new IntRange(1, 1);

		// Token: 0x0400ADD6 RID: 44502
		public List<PawnKindDef> spawnPawnKindOptions = new List<PawnKindDef>();
	}
}
