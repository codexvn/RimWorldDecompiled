using System;

namespace RimWorld
{
	// Token: 0x02002287 RID: 8839
	public class CompProperties_AbilityStartRitual : CompProperties_AbilityEffect
	{
		// Token: 0x0600C07C RID: 49276 RVA: 0x0037BFF6 File Offset: 0x0037A1F6
		public CompProperties_AbilityStartRitual()
		{
			this.compClass = typeof(CompAbilityEffect_StartRitual);
		}

		// Token: 0x040083D5 RID: 33749
		public PreceptDef ritualDef;

		// Token: 0x040083D6 RID: 33750
		public bool allowedForChild = true;
	}
}
