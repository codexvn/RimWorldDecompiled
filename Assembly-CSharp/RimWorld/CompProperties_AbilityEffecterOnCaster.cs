using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002217 RID: 8727
	public class CompProperties_AbilityEffecterOnCaster : CompProperties_AbilityEffect
	{
		// Token: 0x0600BEDF RID: 48863 RVA: 0x00375A7D File Offset: 0x00373C7D
		public CompProperties_AbilityEffecterOnCaster()
		{
			this.compClass = typeof(CompAbilityEffect_EffecterOnCaster);
		}

		// Token: 0x04008307 RID: 33543
		public EffecterDef effecterDef;

		// Token: 0x04008308 RID: 33544
		public float scale = 1f;

		// Token: 0x04008309 RID: 33545
		public int maintainTicks;
	}
}
