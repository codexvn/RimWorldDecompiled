using System;

namespace RimWorld
{
	// Token: 0x020035FC RID: 13820
	[DefOf]
	public static class ExpansionDefOf
	{
		// Token: 0x060133E9 RID: 78825 RVA: 0x005B6104 File Offset: 0x005B4304
		static ExpansionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ExpansionDefOf));
		}

		// Token: 0x0400D4E1 RID: 54497
		public static ExpansionDef Core;

		// Token: 0x0400D4E2 RID: 54498
		public static ExpansionDef Anomaly;
	}
}
