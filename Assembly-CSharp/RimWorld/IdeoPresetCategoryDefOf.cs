using System;

namespace RimWorld
{
	// Token: 0x02003603 RID: 13827
	[DefOf]
	public static class IdeoPresetCategoryDefOf
	{
		// Token: 0x060133F0 RID: 78832 RVA: 0x005B617B File Offset: 0x005B437B
		static IdeoPresetCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(IdeoPresetCategoryDefOf));
		}

		// Token: 0x0400D507 RID: 54535
		[MayRequireIdeology]
		public static IdeoPresetCategoryDef Classic;

		// Token: 0x0400D508 RID: 54536
		[MayRequireIdeology]
		public static IdeoPresetCategoryDef Custom;

		// Token: 0x0400D509 RID: 54537
		[MayRequireIdeology]
		public static IdeoPresetCategoryDef Fluid;
	}
}
