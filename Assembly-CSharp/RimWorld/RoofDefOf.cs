using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020035A0 RID: 13728
	[DefOf]
	public static class RoofDefOf
	{
		// Token: 0x0601338E RID: 78734 RVA: 0x005B5AF9 File Offset: 0x005B3CF9
		static RoofDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoofDefOf));
		}

		// Token: 0x0400CED3 RID: 52947
		public static RoofDef RoofConstructed;

		// Token: 0x0400CED4 RID: 52948
		public static RoofDef RoofRockThick;

		// Token: 0x0400CED5 RID: 52949
		public static RoofDef RoofRockThin;
	}
}
