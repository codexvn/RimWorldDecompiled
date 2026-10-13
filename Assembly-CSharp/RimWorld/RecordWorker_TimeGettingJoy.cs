using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020023C1 RID: 9153
	public class RecordWorker_TimeGettingJoy : RecordWorker
	{
		// Token: 0x0600C6A1 RID: 50849 RVA: 0x0039E144 File Offset: 0x0039C344
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			Job curJob = pawn.CurJob;
			return curJob != null && curJob.def.joyKind != null;
		}
	}
}
