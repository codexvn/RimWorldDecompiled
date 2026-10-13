using System;

namespace RimWorld
{
	// Token: 0x02002252 RID: 8786
	public class CompProperties_AbilityOffsetPrisonerResistance : CompProperties_AbilityEffect
	{
		// Token: 0x0600BFBD RID: 49085 RVA: 0x00379640 File Offset: 0x00377840
		public CompProperties_AbilityOffsetPrisonerResistance()
		{
			this.compClass = typeof(CompAbilityEffect_OffsetPrisonerResistance);
		}

		// Token: 0x04008381 RID: 33665
		public float offset;
	}
}
