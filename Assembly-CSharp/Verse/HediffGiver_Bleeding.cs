using System;

namespace Verse
{
	// Token: 0x02000777 RID: 1911
	public class HediffGiver_Bleeding : HediffGiver
	{
		// Token: 0x06002EE0 RID: 12000 RVA: 0x000F0E54 File Offset: 0x000EF054
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			HediffSet hediffSet = pawn.health.hediffSet;
			if (hediffSet.BleedRateTotal >= 0.1f)
			{
				HealthUtility.AdjustSeverity(pawn, this.hediff, hediffSet.BleedRateTotal * 0.001f);
				return;
			}
			HealthUtility.AdjustSeverity(pawn, this.hediff, -0.00033333333f);
		}
	}
}
