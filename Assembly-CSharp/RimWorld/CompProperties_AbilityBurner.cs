using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020022B2 RID: 8882
	public class CompProperties_AbilityBurner : CompProperties_AbilityEffect
	{
		// Token: 0x0600C135 RID: 49461 RVA: 0x0037F214 File Offset: 0x0037D414
		public CompProperties_AbilityBurner()
		{
			this.compClass = typeof(CompAbilityEffect_Burner);
		}

		// Token: 0x04008421 RID: 33825
		public ThingDef moteDef;

		// Token: 0x04008422 RID: 33826
		public int numStreams;

		// Token: 0x04008423 RID: 33827
		public float coneSizeDegrees;

		// Token: 0x04008424 RID: 33828
		public float range;

		// Token: 0x04008425 RID: 33829
		public float rangeNoise;

		// Token: 0x04008426 RID: 33830
		public float barrelOffsetDistance;

		// Token: 0x04008427 RID: 33831
		public int lifespanNoise;

		// Token: 0x04008428 RID: 33832
		public float sizeReductionDistanceThreshold;

		// Token: 0x04008429 RID: 33833
		public EffecterDef effecterDef;
	}
}
