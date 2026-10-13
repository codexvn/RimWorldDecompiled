using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200145B RID: 5211
	public class JobGiver_HaulToPortal : ThinkNode_JobGiver
	{
		// Token: 0x06007DF9 RID: 32249 RVA: 0x0025F0A0 File Offset: 0x0025D2A0
		protected override Job TryGiveJob(Pawn pawn)
		{
			MapPortal portal = pawn.mindState.duty.focus.Thing as MapPortal;
			if (EnterPortalUtility.HasJobOnPortal(pawn, portal))
			{
				return EnterPortalUtility.JobOnPortal(pawn, portal);
			}
			return null;
		}
	}
}
