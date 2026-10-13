using System;

namespace Verse
{
	// Token: 0x02000780 RID: 1920
	public class HediffGiver_RandomDrugEffect : HediffGiver
	{
		// Token: 0x06002EF4 RID: 12020 RVA: 0x000F14F8 File Offset: 0x000EF6F8
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			if ((this.severityToMtbDaysCurve == null && cause.Severity <= this.minSeverity) || (this.severityToMtbDaysCurve != null && cause.Severity <= this.severityToMtbDaysCurve.Points[0].x))
			{
				return;
			}
			float num = (this.severityToMtbDaysCurve != null) ? this.severityToMtbDaysCurve.Evaluate(cause.Severity) : this.baseMtbDays;
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

		// Token: 0x040024B7 RID: 9399
		public SimpleCurve severityToMtbDaysCurve;

		// Token: 0x040024B8 RID: 9400
		public float baseMtbDays;

		// Token: 0x040024B9 RID: 9401
		public float minSeverity;
	}
}
