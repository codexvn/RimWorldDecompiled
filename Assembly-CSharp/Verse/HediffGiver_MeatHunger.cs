using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200077E RID: 1918
	public class HediffGiver_MeatHunger : HediffGiver
	{
		// Token: 0x06002EF0 RID: 12016 RVA: 0x000F1450 File Offset: 0x000EF650
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			Need need;
			if (pawn.needs != null && pawn.needs.TryGetNeed(NeedDefOf.Food, out need) && need.CurLevel <= 0f)
			{
				HealthUtility.AdjustSeverity(pawn, this.hediff, 0.00090000004f);
				return;
			}
			HealthUtility.AdjustSeverity(pawn, this.hediff, -0.024f);
		}

		// Token: 0x040024B2 RID: 9394
		private const float BaseMeatHungerSeverityIncreasePerDay = 0.9f;

		// Token: 0x040024B3 RID: 9395
		private const float BaseMeatHungerSeverityDecreasePerDay = 24f;

		// Token: 0x040024B4 RID: 9396
		private const float BaseMeatHungerSeverityIncreasePerInterval = 0.00090000004f;

		// Token: 0x040024B5 RID: 9397
		private const float BaseMeatHungerSeverityDecreasePerInterval = 0.024f;
	}
}
