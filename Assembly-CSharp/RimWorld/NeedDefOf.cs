using System;

namespace RimWorld
{
	// Token: 0x02003588 RID: 13704
	[DefOf]
	public static class NeedDefOf
	{
		// Token: 0x06013376 RID: 78710 RVA: 0x005B5961 File Offset: 0x005B3B61
		static NeedDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(NeedDefOf));
		}

		// Token: 0x0400CB64 RID: 52068
		public static NeedDef Food;

		// Token: 0x0400CB65 RID: 52069
		public static NeedDef Rest;

		// Token: 0x0400CB66 RID: 52070
		public static NeedDef Indoors;

		// Token: 0x0400CB67 RID: 52071
		[MayRequireBiotech]
		public static NeedDef Learning;
	}
}
