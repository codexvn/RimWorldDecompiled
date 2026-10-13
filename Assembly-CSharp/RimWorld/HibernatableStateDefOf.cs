using System;

namespace RimWorld
{
	// Token: 0x020035D6 RID: 13782
	[DefOf]
	public static class HibernatableStateDefOf
	{
		// Token: 0x060133C4 RID: 78788 RVA: 0x005B5E8F File Offset: 0x005B408F
		static HibernatableStateDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(HibernatableStateDefOf));
		}

		// Token: 0x0400D34B RID: 54091
		public static HibernatableStateDef Running;

		// Token: 0x0400D34C RID: 54092
		public static HibernatableStateDef Starting;

		// Token: 0x0400D34D RID: 54093
		public static HibernatableStateDef Hibernating;
	}
}
