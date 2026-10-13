using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015BB RID: 5563
	public class WorkGiver_EnterGrowthVat : WorkGiver_EnterBuilding
	{
		// Token: 0x1700141F RID: 5151
		// (get) Token: 0x06008385 RID: 33669 RVA: 0x00275113 File Offset: 0x00273313
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.GrowthVat);
			}
		}

		// Token: 0x06008386 RID: 33670 RVA: 0x0027511F File Offset: 0x0027331F
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.BiotechActive;
		}
	}
}
