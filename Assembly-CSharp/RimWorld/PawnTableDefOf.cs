using System;

namespace RimWorld
{
	// Token: 0x020035D3 RID: 13779
	[DefOf]
	public static class PawnTableDefOf
	{
		// Token: 0x060133C1 RID: 78785 RVA: 0x005B5E5C File Offset: 0x005B405C
		static PawnTableDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnTableDefOf));
		}

		// Token: 0x0400D324 RID: 54052
		public static PawnTableDef Work;

		// Token: 0x0400D325 RID: 54053
		public static PawnTableDef Assign;

		// Token: 0x0400D326 RID: 54054
		public static PawnTableDef Restrict;

		// Token: 0x0400D327 RID: 54055
		public static PawnTableDef Animals;

		// Token: 0x0400D328 RID: 54056
		public static PawnTableDef Wildlife;

		// Token: 0x0400D329 RID: 54057
		[MayRequireBiotech]
		public static PawnTableDef Mechs;
	}
}
