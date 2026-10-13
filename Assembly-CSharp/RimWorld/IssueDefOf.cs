using System;

namespace RimWorld
{
	// Token: 0x020035F4 RID: 13812
	[DefOf]
	public static class IssueDefOf
	{
		// Token: 0x060133E1 RID: 78817 RVA: 0x005B607C File Offset: 0x005B427C
		static IssueDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(IssueDefOf));
		}

		// Token: 0x0400D4B2 RID: 54450
		[MayRequireIdeology]
		public static IssueDef Charity;

		// Token: 0x0400D4B3 RID: 54451
		[MayRequireIdeology]
		public static IssueDef Ranching;

		// Token: 0x0400D4B4 RID: 54452
		[MayRequireIdeology]
		public static IssueDef Blindness;

		// Token: 0x0400D4B5 RID: 54453
		[MayRequireIdeology]
		public static IssueDef IdeoRitualSeat;
	}
}
