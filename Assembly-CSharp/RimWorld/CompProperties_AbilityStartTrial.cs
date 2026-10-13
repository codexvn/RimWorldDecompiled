using System;

namespace RimWorld
{
	// Token: 0x0200228B RID: 8843
	public class CompProperties_AbilityStartTrial : CompProperties_AbilityStartRitualOnPawn
	{
		// Token: 0x0600C08A RID: 49290 RVA: 0x0037C33A File Offset: 0x0037A53A
		public CompProperties_AbilityStartTrial()
		{
			this.compClass = typeof(CompAbilityEffect_StartTrial);
		}

		// Token: 0x040083D9 RID: 33753
		public PreceptDef ritualDefForPrisoner;

		// Token: 0x040083DA RID: 33754
		public PreceptDef ritualDefForMentalState;
	}
}
