using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002B5 RID: 693
	public class PawnCapacityWorker
	{
		// Token: 0x060010FE RID: 4350 RVA: 0x0004FC72 File Offset: 0x0004DE72
		public virtual float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			return 1f;
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanHaveCapacity(BodyDef body)
		{
			return true;
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x0004FC7C File Offset: 0x0004DE7C
		protected float CalculateCapacityAndRecord(HediffSet diffSet, PawnCapacityDef capacity, List<PawnCapacityUtility.CapacityImpactor> impactors)
		{
			float level = diffSet.pawn.health.capacities.GetLevel(capacity);
			if (impactors != null && level != 1f)
			{
				impactors.Add(new PawnCapacityUtility.CapacityImpactorCapacity
				{
					capacity = capacity
				});
			}
			return level;
		}
	}
}
