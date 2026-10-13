using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002329 RID: 9001
	public class PawnCapacityWorker_BloodPumping : PawnCapacityWorker
	{
		// Token: 0x0600C370 RID: 50032 RVA: 0x0038ADC4 File Offset: 0x00388FC4
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.BloodPumpingSource, float.MaxValue, default(FloatRange), impactors, -1f);
		}

		// Token: 0x0600C371 RID: 50033 RVA: 0x0038ADF2 File Offset: 0x00388FF2
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.BloodPumpingSource);
		}
	}
}
