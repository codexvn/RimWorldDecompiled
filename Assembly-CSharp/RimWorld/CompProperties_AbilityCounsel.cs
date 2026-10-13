using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200220A RID: 8714
	public class CompProperties_AbilityCounsel : CompProperties_AbilityEffect
	{
		// Token: 0x0600BEB9 RID: 48825 RVA: 0x00374EEC File Offset: 0x003730EC
		public CompProperties_AbilityCounsel()
		{
			this.compClass = typeof(CompAbilityEffect_Counsel);
		}

		// Token: 0x040082F5 RID: 33525
		public float minMoodOffset = -10f;

		// Token: 0x040082F6 RID: 33526
		[MustTranslate]
		public string successMessage;

		// Token: 0x040082F7 RID: 33527
		[MustTranslate]
		public string successMessageNoNegativeThought;

		// Token: 0x040082F8 RID: 33528
		[MustTranslate]
		public string failMessage;

		// Token: 0x040082F9 RID: 33529
		public ThoughtDef failedThoughtRecipient;
	}
}
