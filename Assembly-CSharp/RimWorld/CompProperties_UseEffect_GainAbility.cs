using System;

namespace RimWorld
{
	// Token: 0x02002F5E RID: 12126
	public class CompProperties_UseEffect_GainAbility : CompProperties_UseEffect
	{
		// Token: 0x06011177 RID: 70007 RVA: 0x004E8613 File Offset: 0x004E6813
		public CompProperties_UseEffect_GainAbility()
		{
			this.compClass = typeof(CompUseEffect_GainAbility);
		}

		// Token: 0x0400B16F RID: 45423
		public AbilityDef ability;
	}
}
