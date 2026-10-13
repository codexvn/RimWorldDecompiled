using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200224E RID: 8782
	public class CompProperties_AbilityMustBeCapableOf : CompProperties_AbilityEffect
	{
		// Token: 0x0600BFB0 RID: 49072 RVA: 0x00378E8B File Offset: 0x0037708B
		public CompProperties_AbilityMustBeCapableOf()
		{
			this.compClass = typeof(CompAbilityEffect_MustBeCapableOf);
		}

		// Token: 0x04008377 RID: 33655
		public WorkTags workTags;
	}
}
