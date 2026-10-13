using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200232F RID: 9007
	public class PawnCapacityWorker_Metabolism : PawnCapacityWorker
	{
		// Token: 0x0600C382 RID: 50050 RVA: 0x0038B098 File Offset: 0x00389298
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.MetabolismSource, float.MaxValue, default(FloatRange), impactors, -1f);
		}

		// Token: 0x0600C383 RID: 50051 RVA: 0x0038B0C6 File Offset: 0x003892C6
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.MetabolismSource);
		}
	}
}
