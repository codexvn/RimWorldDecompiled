using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020035B1 RID: 13745
	[DefOf]
	public static class SpecialThingFilterDefOf
	{
		// Token: 0x0601339F RID: 78751 RVA: 0x005B5C1A File Offset: 0x005B3E1A
		static SpecialThingFilterDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SpecialThingFilterDefOf));
		}

		// Token: 0x0400CF8B RID: 53131
		public static SpecialThingFilterDef AllowFresh;

		// Token: 0x0400CF8C RID: 53132
		public static SpecialThingFilterDef AllowDeadmansApparel;

		// Token: 0x0400CF8D RID: 53133
		public static SpecialThingFilterDef AllowNonDeadmansApparel;

		// Token: 0x0400CF8E RID: 53134
		public static SpecialThingFilterDef AllowLargeCorpses;

		// Token: 0x0400CF8F RID: 53135
		[MayRequireIdeology]
		public static SpecialThingFilterDef AllowVegetarian;

		// Token: 0x0400CF90 RID: 53136
		[MayRequireIdeology]
		public static SpecialThingFilterDef AllowCarnivore;

		// Token: 0x0400CF91 RID: 53137
		[MayRequireIdeology]
		public static SpecialThingFilterDef AllowCannibal;

		// Token: 0x0400CF92 RID: 53138
		[MayRequireIdeology]
		public static SpecialThingFilterDef AllowInsectMeat;

		// Token: 0x0400CF93 RID: 53139
		[MayRequireAnomaly]
		public static SpecialThingFilterDef AllowCorpsesUnnatural;
	}
}
