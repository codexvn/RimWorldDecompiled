using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200198A RID: 6538
	public class CompProperties_UseEffect : CompProperties
	{
		// Token: 0x0600944B RID: 37963 RVA: 0x002B906B File Offset: 0x002B726B
		public CompProperties_UseEffect()
		{
			this.compClass = typeof(CompUseEffect);
		}

		// Token: 0x04005FE2 RID: 24546
		public bool doCameraShake;

		// Token: 0x04005FE3 RID: 24547
		public ThingDef moteOnUsed;

		// Token: 0x04005FE4 RID: 24548
		public float moteOnUsedScale = 1f;

		// Token: 0x04005FE5 RID: 24549
		public FleckDef fleckOnUsed;

		// Token: 0x04005FE6 RID: 24550
		public float fleckOnUsedScale = 1f;

		// Token: 0x04005FE7 RID: 24551
		public EffecterDef effecterOnUsed;

		// Token: 0x04005FE8 RID: 24552
		public EffecterDef warmupEffecter;
	}
}
