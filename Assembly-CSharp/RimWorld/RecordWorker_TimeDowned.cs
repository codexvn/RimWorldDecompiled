using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020023BF RID: 9151
	public class RecordWorker_TimeDowned : RecordWorker
	{
		// Token: 0x0600C69D RID: 50845 RVA: 0x002A0A10 File Offset: 0x0029EC10
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Downed;
		}
	}
}
