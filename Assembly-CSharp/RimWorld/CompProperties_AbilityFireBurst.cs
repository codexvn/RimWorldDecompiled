using System;

namespace RimWorld
{
	// Token: 0x02002223 RID: 8739
	public class CompProperties_AbilityFireBurst : CompProperties_AbilityEffect
	{
		// Token: 0x0600BF12 RID: 48914 RVA: 0x00376B13 File Offset: 0x00374D13
		public CompProperties_AbilityFireBurst()
		{
			this.compClass = typeof(CompAbilityEffect_FireBurst);
		}

		// Token: 0x04008333 RID: 33587
		public float radius = 6f;
	}
}
