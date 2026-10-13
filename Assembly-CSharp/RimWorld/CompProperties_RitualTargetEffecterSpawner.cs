using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F4F RID: 12111
	public class CompProperties_RitualTargetEffecterSpawner : CompProperties
	{
		// Token: 0x0601113C RID: 69948 RVA: 0x004E7A21 File Offset: 0x004E5C21
		public CompProperties_RitualTargetEffecterSpawner()
		{
			this.compClass = typeof(CompRitualTargetEffecterSpawner);
		}

		// Token: 0x0400B154 RID: 45396
		public EffecterDef effecter;

		// Token: 0x0400B155 RID: 45397
		public float minRitualProgress;
	}
}
