using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020023C6 RID: 9158
	public class RecordWorker_TimeOnFire : RecordWorker
	{
		// Token: 0x0600C6AB RID: 50859 RVA: 0x0039E1E4 File Offset: 0x0039C3E4
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.IsBurning();
		}
	}
}
