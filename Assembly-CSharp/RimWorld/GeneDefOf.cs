using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003606 RID: 13830
	[DefOf]
	public static class GeneDefOf
	{
		// Token: 0x060133F1 RID: 78833 RVA: 0x005B618C File Offset: 0x005B438C
		static GeneDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(GeneDefOf));
		}

		// Token: 0x0400D511 RID: 54545
		[MayRequireBiotech]
		public static GeneDef Bloodfeeder;

		// Token: 0x0400D512 RID: 54546
		[MayRequireBiotech]
		public static GeneDef Deathless;

		// Token: 0x0400D513 RID: 54547
		[MayRequireBiotech]
		public static GeneDef XenogermReimplanter;

		// Token: 0x0400D514 RID: 54548
		[MayRequireBiotech]
		public static GeneDef Hemogenic;

		// Token: 0x0400D515 RID: 54549
		[MayRequireBiotech]
		public static GeneDef FireTerror;

		// Token: 0x0400D516 RID: 54550
		[MayRequireBiotech]
		public static GeneDef Inbred;

		// Token: 0x0400D517 RID: 54551
		[MayRequireBiotech]
		public static GeneDef WebbedPhalanges;
	}
}
