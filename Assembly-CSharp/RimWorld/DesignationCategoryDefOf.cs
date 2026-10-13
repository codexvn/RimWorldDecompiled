using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003585 RID: 13701
	[DefOf]
	public static class DesignationCategoryDefOf
	{
		// Token: 0x06013374 RID: 78708 RVA: 0x005B593F File Offset: 0x005B3B3F
		static DesignationCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DesignationCategoryDefOf));
		}

		// Token: 0x0400CABD RID: 51901
		public static DesignationCategoryDef Production;

		// Token: 0x0400CABE RID: 51902
		public static DesignationCategoryDef Floors;

		// Token: 0x0400CABF RID: 51903
		public static DesignationCategoryDef Zone;
	}
}
