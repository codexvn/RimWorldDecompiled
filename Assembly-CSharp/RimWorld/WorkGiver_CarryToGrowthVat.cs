using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200159F RID: 5535
	public class WorkGiver_CarryToGrowthVat : WorkGiver_CarryToBuilding
	{
		// Token: 0x17001407 RID: 5127
		// (get) Token: 0x060082EB RID: 33515 RVA: 0x00275113 File Offset: 0x00273313
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.GrowthVat);
			}
		}

		// Token: 0x060082EC RID: 33516 RVA: 0x0027511F File Offset: 0x0027331F
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.BiotechActive;
		}
	}
}
