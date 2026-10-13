using System;

namespace RimWorld
{
	// Token: 0x0200359C RID: 13724
	[DefOf]
	public static class ResearchTabDefOf
	{
		// Token: 0x0601338A RID: 78730 RVA: 0x005B5AB5 File Offset: 0x005B3CB5
		static ResearchTabDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ResearchTabDefOf));
		}

		// Token: 0x0400CE70 RID: 52848
		public static ResearchTabDef Main;

		// Token: 0x0400CE71 RID: 52849
		[MayRequireAnomaly]
		public static ResearchTabDef Anomaly;
	}
}
