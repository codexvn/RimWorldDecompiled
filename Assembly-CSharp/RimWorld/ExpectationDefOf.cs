using System;

namespace RimWorld
{
	// Token: 0x020035E7 RID: 13799
	[DefOf]
	public static class ExpectationDefOf
	{
		// Token: 0x060133D5 RID: 78805 RVA: 0x005B5FB0 File Offset: 0x005B41B0
		static ExpectationDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ExpectationDefOf));
		}

		// Token: 0x0400D3B3 RID: 54195
		public static ExpectationDef ExtremelyLow;

		// Token: 0x0400D3B4 RID: 54196
		public static ExpectationDef VeryLow;

		// Token: 0x0400D3B5 RID: 54197
		public static ExpectationDef Low;

		// Token: 0x0400D3B6 RID: 54198
		public static ExpectationDef Moderate;

		// Token: 0x0400D3B7 RID: 54199
		public static ExpectationDef High;
	}
}
