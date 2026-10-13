using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002EE0 RID: 12000
	public class CompProperties_DissolutionEffectPollution : CompProperties
	{
		// Token: 0x06010F1A RID: 69402 RVA: 0x004E0583 File Offset: 0x004DE783
		public CompProperties_DissolutionEffectPollution()
		{
			this.compClass = typeof(CompDissolutionEffect_Pollution);
		}

		// Token: 0x0400B031 RID: 45105
		public int cellsToPollutePerDissolution = 4;

		// Token: 0x0400B032 RID: 45106
		public float tilePollutionPerDissolution = 0.0005f;

		// Token: 0x0400B033 RID: 45107
		public float waterTilePollutionFactor = 8f;

		// Token: 0x0400B034 RID: 45108
		public SimpleCurve goodWillFactorOverDistanceCurce;
	}
}
