using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015A1 RID: 5537
	public class WorkGiver_CarryToSubcoreScanner : WorkGiver_CarryToBuilding
	{
		// Token: 0x17001409 RID: 5129
		// (get) Token: 0x060082F1 RID: 33521 RVA: 0x0027513D File Offset: 0x0027333D
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.SubcoreScanner);
			}
		}

		// Token: 0x060082F2 RID: 33522 RVA: 0x0027511F File Offset: 0x0027331F
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.BiotechActive;
		}
	}
}
