using System;

namespace RimWorld
{
	// Token: 0x02003608 RID: 13832
	[DefOf]
	public static class XenotypeIconDefOf
	{
		// Token: 0x060133F3 RID: 78835 RVA: 0x005B61AE File Offset: 0x005B43AE
		static XenotypeIconDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(XenotypeIconDefOf));
		}

		// Token: 0x0400D51A RID: 54554
		[MayRequireBiotech]
		public static XenotypeIconDef Basic;
	}
}
