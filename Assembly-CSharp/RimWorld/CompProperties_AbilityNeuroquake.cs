using System;

namespace RimWorld
{
	// Token: 0x02002250 RID: 8784
	public class CompProperties_AbilityNeuroquake : CompProperties_AbilityEffect
	{
		// Token: 0x0600BFB4 RID: 49076 RVA: 0x00378F54 File Offset: 0x00377154
		public CompProperties_AbilityNeuroquake()
		{
			this.compClass = typeof(CompAbilityEffect_Neuroquake);
		}

		// Token: 0x04008378 RID: 33656
		public int goodwillImpactForNeuroquake;

		// Token: 0x04008379 RID: 33657
		public int goodwillImpactForBerserk;

		// Token: 0x0400837A RID: 33658
		public int worldRangeTiles;

		// Token: 0x0400837B RID: 33659
		public float mentalStateRadius;
	}
}
