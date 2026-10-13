using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200228D RID: 8845
	public class CompProperties_StopManhunter : CompProperties_AbilityEffect
	{
		// Token: 0x0600C08F RID: 49295 RVA: 0x0037C450 File Offset: 0x0037A650
		public CompProperties_StopManhunter()
		{
			this.compClass = typeof(CompAbilityEffect_StopManhunter);
		}

		// Token: 0x040083DB RID: 33755
		[MustTranslate]
		public string successMessage;
	}
}
