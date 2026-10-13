using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200359E RID: 13726
	[DefOf]
	public static class MentalBreakDefOf
	{
		// Token: 0x0601338C RID: 78732 RVA: 0x005B5AD7 File Offset: 0x005B3CD7
		static MentalBreakDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MentalBreakDefOf));
		}

		// Token: 0x0400CE86 RID: 52870
		public static MentalBreakDef Berserk;

		// Token: 0x0400CE87 RID: 52871
		public static MentalBreakDef CorpseObsession;

		// Token: 0x0400CE88 RID: 52872
		public static MentalBreakDef Catatonic;

		// Token: 0x0400CE89 RID: 52873
		[MayRequireAnomaly]
		public static MentalBreakDef BerserkShort;

		// Token: 0x0400CE8A RID: 52874
		[MayRequireAnomaly]
		public static MentalBreakDef CubeSculpting;

		// Token: 0x0400CE8B RID: 52875
		[MayRequireAnomaly]
		public static MentalBreakDef HumanityBreak;
	}
}
