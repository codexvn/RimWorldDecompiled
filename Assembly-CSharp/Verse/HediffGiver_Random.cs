using System;

namespace Verse
{
	// Token: 0x0200077F RID: 1919
	public class HediffGiver_Random : HediffGiver
	{
		// Token: 0x06002EF2 RID: 12018 RVA: 0x000F14AC File Offset: 0x000EF6AC
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			float num = this.mtbDays;
			float num2 = this.ChanceFactor(pawn);
			if (num2 == 0f)
			{
				return;
			}
			if (Rand.MTBEventOccurs(num / num2, 60000f, 60f) && base.TryApply(pawn, null))
			{
				base.SendLetter(pawn, cause);
			}
		}

		// Token: 0x040024B6 RID: 9398
		public float mtbDays;
	}
}
