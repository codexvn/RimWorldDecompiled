using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020023C4 RID: 9156
	public class RecordWorker_TimeInBedForMedicalReasons : RecordWorker
	{
		// Token: 0x0600C6A7 RID: 50855 RVA: 0x0039E188 File Offset: 0x0039C388
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InBed() && (HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn) || (HealthAIUtility.ShouldSeekMedicalRest(pawn) && (pawn.needs.rest == null || pawn.needs.rest.CurLevel >= 1f || pawn.CurJob.restUntilHealed)));
		}
	}
}
