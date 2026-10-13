using System;

namespace RimWorld
{
	// Token: 0x020035C9 RID: 13769
	[DefOf]
	public static class RoadPathingDefOf
	{
		// Token: 0x060133B7 RID: 78775 RVA: 0x005B5DB2 File Offset: 0x005B3FB2
		static RoadPathingDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoadPathingDefOf));
		}

		// Token: 0x0400D2E4 RID: 53988
		public static RoadPathingDef Avoid;

		// Token: 0x0400D2E5 RID: 53989
		public static RoadPathingDef Bulldoze;
	}
}
