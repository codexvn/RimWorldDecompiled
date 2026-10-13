using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002229 RID: 8745
	public class CompProperties_AbilityFireSpew : CompProperties_AbilityEffect
	{
		// Token: 0x0600BF2B RID: 48939 RVA: 0x0037701E File Offset: 0x0037521E
		public CompProperties_AbilityFireSpew()
		{
			this.compClass = typeof(CompAbilityEffect_FireSpew);
		}

		// Token: 0x0400833A RID: 33594
		public float range;

		// Token: 0x0400833B RID: 33595
		public float lineWidthEnd;

		// Token: 0x0400833C RID: 33596
		public ThingDef filthDef;

		// Token: 0x0400833D RID: 33597
		public int damAmount = -1;

		// Token: 0x0400833E RID: 33598
		public EffecterDef effecterDef;

		// Token: 0x0400833F RID: 33599
		public bool canHitFilledCells;
	}
}
