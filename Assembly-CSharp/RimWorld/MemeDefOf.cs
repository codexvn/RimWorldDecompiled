using System;

namespace RimWorld
{
	// Token: 0x020035F5 RID: 13813
	[DefOf]
	public static class MemeDefOf
	{
		// Token: 0x060133E2 RID: 78818 RVA: 0x005B608D File Offset: 0x005B428D
		static MemeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MemeDefOf));
		}

		// Token: 0x0400D4B6 RID: 54454
		[MayRequireIdeology]
		public static MemeDef MaleSupremacy;

		// Token: 0x0400D4B7 RID: 54455
		[MayRequireIdeology]
		public static MemeDef FemaleSupremacy;

		// Token: 0x0400D4B8 RID: 54456
		[MayRequireIdeology]
		public static MemeDef TreeConnection;

		// Token: 0x0400D4B9 RID: 54457
		[MayRequireIdeology]
		public static MemeDef Tunneler;

		// Token: 0x0400D4BA RID: 54458
		[MayRequireIdeology]
		public static MemeDef Darkness;
	}
}
