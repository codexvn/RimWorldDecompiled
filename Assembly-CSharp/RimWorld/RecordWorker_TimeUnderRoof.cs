using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020023C7 RID: 9159
	public class RecordWorker_TimeUnderRoof : RecordWorker
	{
		// Token: 0x0600C6AD RID: 50861 RVA: 0x0039E1EC File Offset: 0x0039C3EC
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Spawned && pawn.Position.Roofed(pawn.Map);
		}
	}
}
