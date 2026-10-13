using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200232D RID: 9005
	public class PawnCapacityWorker_Hearing : PawnCapacityWorker
	{
		// Token: 0x0600C37C RID: 50044 RVA: 0x0038B010 File Offset: 0x00389210
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, BodyPartTagDefOf.HearingSource, float.MaxValue, default(FloatRange), impactors, 0.75f);
		}

		// Token: 0x0600C37D RID: 50045 RVA: 0x0038B03E File Offset: 0x0038923E
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.HearingSource);
		}
	}
}
