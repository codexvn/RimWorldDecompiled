using System;

namespace RimWorld
{
	// Token: 0x0200360E RID: 13838
	[DefOf]
	public static class KnowledgeCategoryDefOf
	{
		// Token: 0x060133F9 RID: 78841 RVA: 0x005B6214 File Offset: 0x005B4414
		static KnowledgeCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(KnowledgeCategoryDefOf));
		}

		// Token: 0x0400D527 RID: 54567
		[MayRequireAnomaly]
		public static KnowledgeCategoryDef Basic;

		// Token: 0x0400D528 RID: 54568
		[MayRequireAnomaly]
		public static KnowledgeCategoryDef Advanced;
	}
}
