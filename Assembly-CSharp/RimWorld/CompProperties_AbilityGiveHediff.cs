using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002238 RID: 8760
	public class CompProperties_AbilityGiveHediff : CompProperties_AbilityEffectWithDuration
	{
		// Token: 0x04008352 RID: 33618
		public HediffDef hediffDef;

		// Token: 0x04008353 RID: 33619
		public bool onlyBrain;

		// Token: 0x04008354 RID: 33620
		public bool applyToSelf;

		// Token: 0x04008355 RID: 33621
		public bool onlyApplyToSelf;

		// Token: 0x04008356 RID: 33622
		public bool applyToTarget = true;

		// Token: 0x04008357 RID: 33623
		public bool replaceExisting;

		// Token: 0x04008358 RID: 33624
		public float severity = -1f;

		// Token: 0x04008359 RID: 33625
		public bool ignoreSelf;
	}
}
