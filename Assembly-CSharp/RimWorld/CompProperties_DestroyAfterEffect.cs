using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001960 RID: 6496
	public class CompProperties_DestroyAfterEffect : CompProperties
	{
		// Token: 0x0600939E RID: 37790 RVA: 0x002B6D9A File Offset: 0x002B4F9A
		public CompProperties_DestroyAfterEffect()
		{
			this.compClass = typeof(CompDestroyAfterEffect);
		}

		// Token: 0x04005EFC RID: 24316
		public EffecterDef effecterDef;
	}
}
