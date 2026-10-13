using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015A0 RID: 5536
	public class WorkGiver_CarryToGeneExtractor : WorkGiver_CarryToBuilding
	{
		// Token: 0x17001408 RID: 5128
		// (get) Token: 0x060082EE RID: 33518 RVA: 0x00275131 File Offset: 0x00273331
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.GeneExtractor);
			}
		}

		// Token: 0x060082EF RID: 33519 RVA: 0x0027511F File Offset: 0x0027331F
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.BiotechActive;
		}
	}
}
