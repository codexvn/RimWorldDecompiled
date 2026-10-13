using System;

namespace RimWorld
{
	// Token: 0x0200227A RID: 8826
	public class CompProperties_AbilitySmokepop : CompProperties_AbilityEffect
	{
		// Token: 0x0600C056 RID: 49238 RVA: 0x0037B58E File Offset: 0x0037978E
		public CompProperties_AbilitySmokepop()
		{
			this.compClass = typeof(CompAbilityEffect_Smokepop);
		}

		// Token: 0x040083C6 RID: 33734
		public float smokeRadius;
	}
}
