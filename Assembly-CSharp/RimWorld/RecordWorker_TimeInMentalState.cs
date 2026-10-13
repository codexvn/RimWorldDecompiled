using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020023C5 RID: 9157
	public class RecordWorker_TimeInMentalState : RecordWorker
	{
		// Token: 0x0600C6A9 RID: 50857 RVA: 0x0025721A File Offset: 0x0025541A
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InMentalState;
		}
	}
}
