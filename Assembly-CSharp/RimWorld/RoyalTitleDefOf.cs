using System;

namespace RimWorld
{
	// Token: 0x020035EC RID: 13804
	[DefOf]
	public static class RoyalTitleDefOf
	{
		// Token: 0x060133DA RID: 78810 RVA: 0x005B6005 File Offset: 0x005B4205
		static RoyalTitleDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoyalTitleDefOf));
		}

		// Token: 0x0400D3D9 RID: 54233
		[MayRequireRoyalty]
		public static RoyalTitleDef Knight;

		// Token: 0x0400D3DA RID: 54234
		[MayRequireRoyalty]
		public static RoyalTitleDef Count;
	}
}
