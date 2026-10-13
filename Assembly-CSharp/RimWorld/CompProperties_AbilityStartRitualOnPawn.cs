using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002289 RID: 8841
	public class CompProperties_AbilityStartRitualOnPawn : CompProperties_AbilityStartRitual
	{
		// Token: 0x0600C085 RID: 49285 RVA: 0x0037C282 File Offset: 0x0037A482
		public CompProperties_AbilityStartRitualOnPawn()
		{
			this.compClass = typeof(CompAbilityEffect_StartRitualOnPawn);
		}

		// Token: 0x040083D8 RID: 33752
		[NoTranslate]
		public string targetRoleId;
	}
}
