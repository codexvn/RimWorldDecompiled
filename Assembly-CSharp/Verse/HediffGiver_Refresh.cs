using System;

namespace Verse
{
	// Token: 0x02000782 RID: 1922
	public class HediffGiver_Refresh : HediffGiver
	{
		// Token: 0x06002EF9 RID: 12025 RVA: 0x000F1648 File Offset: 0x000EF848
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(this.hediff, false);
			if (firstHediffOfDef != null)
			{
				firstHediffOfDef.ageTicks = 0;
				return;
			}
			if (base.TryApply(pawn, null))
			{
				base.SendLetter(pawn, cause);
			}
		}
	}
}
