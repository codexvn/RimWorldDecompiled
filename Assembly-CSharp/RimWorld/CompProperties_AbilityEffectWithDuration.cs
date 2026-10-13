using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020022AC RID: 8876
	public class CompProperties_AbilityEffectWithDuration : CompProperties_AbilityEffect
	{
		// Token: 0x0400841D RID: 33821
		public StatDef durationMultiplier;

		// Token: 0x0400841E RID: 33822
		public FloatRange durationSecondsOverride = FloatRange.Zero;
	}
}
