using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015BC RID: 5564
	public class WorkGiver_EnterSubcoreScanner : WorkGiver_EnterBuilding
	{
		// Token: 0x17001420 RID: 5152
		// (get) Token: 0x06008388 RID: 33672 RVA: 0x0027513D File Offset: 0x0027333D
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.SubcoreScanner);
			}
		}

		// Token: 0x06008389 RID: 33673 RVA: 0x0027511F File Offset: 0x0027331F
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.BiotechActive;
		}
	}
}
