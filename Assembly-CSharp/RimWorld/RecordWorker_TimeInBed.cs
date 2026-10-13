using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020023C3 RID: 9155
	public class RecordWorker_TimeInBed : RecordWorker
	{
		// Token: 0x0600C6A5 RID: 50853 RVA: 0x002A1743 File Offset: 0x0029F943
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InBed();
		}
	}
}
