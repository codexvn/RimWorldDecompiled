using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001619 RID: 5657
	public abstract class WorkGiver_TakeToBed : WorkGiver_Scanner
	{
		// Token: 0x06008575 RID: 34165 RVA: 0x0027E4B6 File Offset: 0x0027C6B6
		protected Building_Bed FindBed(Pawn pawn, Pawn patient)
		{
			return RestUtility.FindBedFor(patient, pawn, false, false, patient.GuestStatus);
		}
	}
}
