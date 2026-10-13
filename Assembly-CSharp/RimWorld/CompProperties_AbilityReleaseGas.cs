using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002268 RID: 8808
	public class CompProperties_AbilityReleaseGas : CompProperties_AbilityEffect
	{
		// Token: 0x0600C006 RID: 49158 RVA: 0x0037A53F File Offset: 0x0037873F
		public CompProperties_AbilityReleaseGas()
		{
			this.compClass = typeof(CompAbilityEffect_ReleaseGas);
		}

		// Token: 0x0400839C RID: 33692
		public GasType gasType;

		// Token: 0x0400839D RID: 33693
		public int cellsToFill;
	}
}
