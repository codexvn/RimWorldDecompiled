using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E22 RID: 11810
	public class CompProperties_SpawnEffecterOnDestroy : CompProperties
	{
		// Token: 0x06010A1B RID: 68123 RVA: 0x004CC030 File Offset: 0x004CA230
		public CompProperties_SpawnEffecterOnDestroy()
		{
			this.compClass = typeof(CompSpawnEffecterOnDestroy);
		}

		// Token: 0x0400AD56 RID: 44374
		public EffecterDef effect;
	}
}
