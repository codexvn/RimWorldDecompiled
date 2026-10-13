using System;

namespace RimWorld
{
	// Token: 0x020035CE RID: 13774
	[DefOf]
	public static class BillRepeatModeDefOf
	{
		// Token: 0x060133BC RID: 78780 RVA: 0x005B5E07 File Offset: 0x005B4007
		static BillRepeatModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BillRepeatModeDefOf));
		}

		// Token: 0x0400D317 RID: 54039
		public static BillRepeatModeDef RepeatCount;

		// Token: 0x0400D318 RID: 54040
		public static BillRepeatModeDef TargetCount;

		// Token: 0x0400D319 RID: 54041
		public static BillRepeatModeDef Forever;
	}
}
