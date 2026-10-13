using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015FC RID: 5628
	public class WorkGiver_PatientGoToBedEmergencyTreatment : WorkGiver_PatientGoToBedRecuperate
	{
		// Token: 0x060084D7 RID: 34007 RVA: 0x0027C9BB File Offset: 0x0027ABBB
		public override Job NonScanJob(Pawn pawn)
		{
			if (!HealthAIUtility.ShouldBeTendedNowByPlayerUrgent(pawn))
			{
				return null;
			}
			return base.NonScanJob(pawn);
		}
	}
}
