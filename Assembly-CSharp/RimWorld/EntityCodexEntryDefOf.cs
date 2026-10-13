using System;

namespace RimWorld
{
	// Token: 0x02003619 RID: 13849
	[DefOf]
	public static class EntityCodexEntryDefOf
	{
		// Token: 0x06013404 RID: 78852 RVA: 0x005B62CF File Offset: 0x005B44CF
		static EntityCodexEntryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(EntityCodexEntryDefOf));
		}

		// Token: 0x0400D55E RID: 54622
		[MayRequireAnomaly]
		public static EntityCodexEntryDef UnnaturalCorpse;
	}
}
