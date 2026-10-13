using System;

namespace RimWorld
{
	// Token: 0x020021FA RID: 8698
	public class CompProperties_AbilityBrainDamageChance : CompProperties_AbilityEffect
	{
		// Token: 0x0600BE7B RID: 48763 RVA: 0x00373B2B File Offset: 0x00371D2B
		public CompProperties_AbilityBrainDamageChance()
		{
			this.compClass = typeof(CompAbilityEffect_BrainDamageChance);
		}

		// Token: 0x040082DA RID: 33498
		public float brainDamageChance = 0.3f;
	}
}
