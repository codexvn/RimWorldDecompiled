using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001554 RID: 5460
	public class WorkGiver_RebalanceAnimalsInPens : WorkGiver_TakeToPen
	{
		// Token: 0x0600819C RID: 33180 RVA: 0x00270550 File Offset: 0x0026E750
		public WorkGiver_RebalanceAnimalsInPens()
		{
			this.ropingPriority = RopingPriority.Balanced;
		}
	}
}
