using System;

namespace RimWorld
{
	// Token: 0x020035C8 RID: 13768
	[DefOf]
	public static class RoadDefOf
	{
		// Token: 0x060133B6 RID: 78774 RVA: 0x005B5DA1 File Offset: 0x005B3FA1
		static RoadDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoadDefOf));
		}

		// Token: 0x0400D2E2 RID: 53986
		public static RoadDef AncientAsphaltRoad;

		// Token: 0x0400D2E3 RID: 53987
		public static RoadDef AncientAsphaltHighway;
	}
}
