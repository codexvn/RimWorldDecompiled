using System;

namespace RimWorld
{
	// Token: 0x02003618 RID: 13848
	[DefOf]
	public static class RenderSkipFlagDefOf
	{
		// Token: 0x06013403 RID: 78851 RVA: 0x005B62BE File Offset: 0x005B44BE
		static RenderSkipFlagDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RenderSkipFlagDefOf));
		}

		// Token: 0x0400D558 RID: 54616
		public static RenderSkipFlagDef None;

		// Token: 0x0400D559 RID: 54617
		public static RenderSkipFlagDef Hair;

		// Token: 0x0400D55A RID: 54618
		public static RenderSkipFlagDef Head;

		// Token: 0x0400D55B RID: 54619
		public static RenderSkipFlagDef Beard;

		// Token: 0x0400D55C RID: 54620
		public static RenderSkipFlagDef Eyes;

		// Token: 0x0400D55D RID: 54621
		[MayRequireIdeology]
		public static RenderSkipFlagDef Tattoos;
	}
}
