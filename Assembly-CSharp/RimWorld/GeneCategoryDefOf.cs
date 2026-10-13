using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200360A RID: 13834
	[DefOf]
	public static class GeneCategoryDefOf
	{
		// Token: 0x060133F5 RID: 78837 RVA: 0x005B61D0 File Offset: 0x005B43D0
		static GeneCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(GeneCategoryDefOf));
		}

		// Token: 0x0400D51E RID: 54558
		[MayRequireBiotech]
		public static GeneCategoryDef Archite;

		// Token: 0x0400D51F RID: 54559
		public static GeneCategoryDef Miscellaneous;
	}
}
