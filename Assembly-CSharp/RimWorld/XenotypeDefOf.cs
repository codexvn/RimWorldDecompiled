using System;

namespace RimWorld
{
	// Token: 0x02003607 RID: 13831
	[DefOf]
	public static class XenotypeDefOf
	{
		// Token: 0x060133F2 RID: 78834 RVA: 0x005B619D File Offset: 0x005B439D
		static XenotypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(XenotypeDefOf));
		}

		// Token: 0x0400D518 RID: 54552
		[MayRequireBiotech]
		public static XenotypeDef Baseliner;

		// Token: 0x0400D519 RID: 54553
		[MayRequireBiotech]
		public static XenotypeDef Sanguophage;
	}
}
