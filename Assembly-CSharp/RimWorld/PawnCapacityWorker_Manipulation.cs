using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200232E RID: 9006
	public class PawnCapacityWorker_Manipulation : PawnCapacityWorker
	{
		// Token: 0x0600C37F RID: 50047 RVA: 0x0038B04C File Offset: 0x0038924C
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			float num = 0f;
			return PawnCapacityUtility.CalculateLimbEfficiency(diffSet, BodyPartTagDefOf.ManipulationLimbCore, BodyPartTagDefOf.ManipulationLimbSegment, BodyPartTagDefOf.ManipulationLimbDigit, 0.8f, out num, impactors) * base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.Consciousness, impactors);
		}

		// Token: 0x0600C380 RID: 50048 RVA: 0x0038B08A File Offset: 0x0038928A
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.ManipulationLimbCore);
		}
	}
}
