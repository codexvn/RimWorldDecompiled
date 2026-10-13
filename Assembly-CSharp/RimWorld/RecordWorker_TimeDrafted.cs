using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020023C0 RID: 9152
	public class RecordWorker_TimeDrafted : RecordWorker
	{
		// Token: 0x0600C69F RID: 50847 RVA: 0x002A0C2B File Offset: 0x0029EE2B
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Drafted;
		}
	}
}
