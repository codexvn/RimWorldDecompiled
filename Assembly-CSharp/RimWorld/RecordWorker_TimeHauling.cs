using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020023C2 RID: 9154
	public class RecordWorker_TimeHauling : RecordWorker
	{
		// Token: 0x0600C6A3 RID: 50851 RVA: 0x0039E16B File Offset: 0x0039C36B
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return !pawn.Dead && pawn.carryTracker.CarriedThing != null;
		}
	}
}
