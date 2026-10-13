using System;

namespace RimWorld
{
	// Token: 0x02003613 RID: 13843
	[DefOf]
	public static class MutantDefOf
	{
		// Token: 0x060133FE RID: 78846 RVA: 0x005B6269 File Offset: 0x005B4469
		static MutantDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MutantDefOf));
		}

		// Token: 0x0400D53D RID: 54589
		[MayRequireAnomaly]
		public static MutantDef Shambler;

		// Token: 0x0400D53E RID: 54590
		[MayRequireAnomaly]
		public static MutantDef Ghoul;

		// Token: 0x0400D53F RID: 54591
		[MayRequireAnomaly]
		public static MutantDef AwokenCorpse;
	}
}
