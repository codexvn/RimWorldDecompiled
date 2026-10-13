using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020023BE RID: 9150
	public class RecordWorker_TimeAsPrisoner : RecordWorker
	{
		// Token: 0x0600C69B RID: 50843 RVA: 0x001F192B File Offset: 0x001EFB2B
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.IsPrisoner;
		}
	}
}
