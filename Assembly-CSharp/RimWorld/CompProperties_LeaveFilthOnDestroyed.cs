using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D18 RID: 11544
	public class CompProperties_LeaveFilthOnDestroyed : CompProperties
	{
		// Token: 0x06010398 RID: 66456 RVA: 0x004B1460 File Offset: 0x004AF660
		public CompProperties_LeaveFilthOnDestroyed()
		{
			this.compClass = typeof(CompLeaveFilthOnDestroyed);
		}

		// Token: 0x0400A970 RID: 43376
		public int thickness;

		// Token: 0x0400A971 RID: 43377
		public ThingDef filthDef;
	}
}
