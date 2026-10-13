using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DAF RID: 11695
	public class CompProperties_PolluteOverTime : CompProperties
	{
		// Token: 0x06010715 RID: 67349 RVA: 0x004BE5E4 File Offset: 0x004BC7E4
		public CompProperties_PolluteOverTime()
		{
			this.compClass = typeof(CompPolluteOverTime);
		}

		// Token: 0x0400AB82 RID: 43906
		public int cellsToPollutePerDay;
	}
}
