using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002331 RID: 9009
	public class PawnCapacityWorker_Sight : PawnCapacityWorker
	{
		// Token: 0x0600C388 RID: 50056 RVA: 0x0038B1D8 File Offset: 0x003893D8
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartTagDef sightSource = BodyPartTagDefOf.SightSource;
			float maxValue = float.MaxValue;
			float partEfficiencySpecialWeight = PawnCapacityWorker_Sight.PartEfficiencySpecialWeight;
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, sightSource, maxValue, default(FloatRange), impactors, partEfficiencySpecialWeight);
		}

		// Token: 0x0600C389 RID: 50057 RVA: 0x0038B208 File Offset: 0x00389408
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.SightSource);
		}

		// Token: 0x0400853A RID: 34106
		public static float PartEfficiencySpecialWeight = 0.75f;
	}
}
