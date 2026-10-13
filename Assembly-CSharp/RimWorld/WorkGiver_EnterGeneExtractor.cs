using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015BA RID: 5562
	public class WorkGiver_EnterGeneExtractor : WorkGiver_EnterBuilding
	{
		// Token: 0x1700141E RID: 5150
		// (get) Token: 0x06008382 RID: 33666 RVA: 0x00275131 File Offset: 0x00273331
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.GeneExtractor);
			}
		}

		// Token: 0x06008383 RID: 33667 RVA: 0x0027511F File Offset: 0x0027331F
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.BiotechActive;
		}
	}
}
