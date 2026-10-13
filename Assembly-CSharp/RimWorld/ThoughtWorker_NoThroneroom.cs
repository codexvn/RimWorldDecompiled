using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018A5 RID: 6309
	public class ThoughtWorker_NoThroneroom : ThoughtWorker
	{
		// Token: 0x0600904F RID: 36943 RVA: 0x002A8F30 File Offset: 0x002A7130
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.royalty == null || p.MapHeld == null || !p.MapHeld.IsPlayerHome || p.royalty.HighestTitleWithThroneRoomRequirements() == null || (MoveColonyUtility.TitleAndRoleRequirementsGracePeriodActive && !p.IsQuestLodger()))
			{
				return false;
			}
			return p.ownership.AssignedThrone == null;
		}
	}
}
