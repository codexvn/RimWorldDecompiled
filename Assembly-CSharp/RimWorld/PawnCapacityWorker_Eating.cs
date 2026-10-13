using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200232C RID: 9004
	public class PawnCapacityWorker_Eating : PawnCapacityWorker
	{
		// Token: 0x0600C379 RID: 50041 RVA: 0x0038AF7C File Offset: 0x0038917C
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.EatingSource, float.MaxValue, default(FloatRange), impactors, -1f) * PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.EatingPathway, 1f, default(FloatRange), impactors, -1f) * PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.Tongue, float.MaxValue, new FloatRange(0.5f, 1f), impactors, -1f) * base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.Consciousness, impactors);
		}

		// Token: 0x0600C37A RID: 50042 RVA: 0x0038B000 File Offset: 0x00389200
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.EatingSource);
		}
	}
}
