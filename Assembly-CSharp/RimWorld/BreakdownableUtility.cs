using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C30 RID: 11312
	public static class BreakdownableUtility
	{
		// Token: 0x0600FDAB RID: 64939 RVA: 0x00499288 File Offset: 0x00497488
		public static bool IsBrokenDown(this Thing t)
		{
			CompBreakdownable compBreakdownable = t.TryGetComp<CompBreakdownable>();
			return compBreakdownable != null && compBreakdownable.BrokenDown;
		}
	}
}
