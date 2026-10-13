using System;

namespace RimWorld
{
	// Token: 0x020035AA RID: 13738
	[DefOf]
	public static class TimeAssignmentDefOf
	{
		// Token: 0x06013398 RID: 78744 RVA: 0x005B5BA3 File Offset: 0x005B3DA3
		static TimeAssignmentDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TimeAssignmentDefOf));
		}

		// Token: 0x0400CF0D RID: 53005
		public static TimeAssignmentDef Anything;

		// Token: 0x0400CF0E RID: 53006
		public static TimeAssignmentDef Work;

		// Token: 0x0400CF0F RID: 53007
		public static TimeAssignmentDef Joy;

		// Token: 0x0400CF10 RID: 53008
		public static TimeAssignmentDef Sleep;

		// Token: 0x0400CF11 RID: 53009
		[MayRequireRoyalty]
		public static TimeAssignmentDef Meditate;
	}
}
