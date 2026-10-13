using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018A6 RID: 6310
	public class ThoughtWorker_NoPersonalBedroom : ThoughtWorker
	{
		// Token: 0x06009051 RID: 36945 RVA: 0x002A8F90 File Offset: 0x002A7190
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.royalty == null || p.MapHeld == null || !p.MapHeld.IsPlayerHome || p.royalty.HighestTitleWithBedroomRequirements() == null || (MoveColonyUtility.TitleAndRoleRequirementsGracePeriodActive && !p.IsQuestLodger()))
			{
				return false;
			}
			return !p.royalty.HasPersonalBedroom();
		}

		// Token: 0x06009052 RID: 36946 RVA: 0x002A8FF0 File Offset: 0x002A71F0
		public override string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(p.royalty.HighestTitleWithBedroomRequirements().Named("TITLE")).CapitalizeFirst();
		}
	}
}
