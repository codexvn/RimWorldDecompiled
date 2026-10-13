using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E2A RID: 11818
	public class CompProperties_SpawnFilthOnTakeDamage : CompProperties
	{
		// Token: 0x06010A45 RID: 68165 RVA: 0x004CC970 File Offset: 0x004CAB70
		public CompProperties_SpawnFilthOnTakeDamage()
		{
			this.compClass = typeof(CompSpawnerFilthOnTakeDamage);
		}

		// Token: 0x0400AD6F RID: 44399
		public ThingDef filthDef;

		// Token: 0x0400AD70 RID: 44400
		public IntRange filthCountRange = IntRange.One;

		// Token: 0x0400AD71 RID: 44401
		public float chance = 1f;
	}
}
