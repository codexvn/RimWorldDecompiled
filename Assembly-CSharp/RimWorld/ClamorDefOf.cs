using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020035E2 RID: 13794
	[DefOf]
	public static class ClamorDefOf
	{
		// Token: 0x060133D0 RID: 78800 RVA: 0x005B5F5B File Offset: 0x005B415B
		static ClamorDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ClamorDefOf));
		}

		// Token: 0x0400D39A RID: 54170
		public static ClamorDef Movement;

		// Token: 0x0400D39B RID: 54171
		public static ClamorDef Harm;

		// Token: 0x0400D39C RID: 54172
		public static ClamorDef Construction;

		// Token: 0x0400D39D RID: 54173
		public static ClamorDef Impact;

		// Token: 0x0400D39E RID: 54174
		public static ClamorDef Ability;

		// Token: 0x0400D39F RID: 54175
		[MayRequireBiotech]
		public static ClamorDef BabyCry;
	}
}
