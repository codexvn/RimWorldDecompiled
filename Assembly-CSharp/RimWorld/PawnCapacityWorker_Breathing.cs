using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200232A RID: 9002
	public class PawnCapacityWorker_Breathing : PawnCapacityWorker
	{
		// Token: 0x0600C373 RID: 50035 RVA: 0x0038AE00 File Offset: 0x00389000
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BreathingSource, float.MaxValue, default(FloatRange), impactors, -1f) * PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BreathingPathway, 1f, default(FloatRange), impactors, -1f) * PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BreathingSourceCage, 1f, default(FloatRange), impactors, -1f);
		}

		// Token: 0x0600C374 RID: 50036 RVA: 0x0038AE72 File Offset: 0x00389072
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.BreathingSource);
		}
	}
}
