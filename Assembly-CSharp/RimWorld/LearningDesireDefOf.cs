using System;

namespace RimWorld
{
	// Token: 0x0200360C RID: 13836
	[DefOf]
	public static class LearningDesireDefOf
	{
		// Token: 0x060133F7 RID: 78839 RVA: 0x005B61F2 File Offset: 0x005B43F2
		static LearningDesireDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(LearningDesireDefOf));
		}

		// Token: 0x0400D522 RID: 54562
		[MayRequireBiotech]
		public static LearningDesireDef Lessontaking;

		// Token: 0x0400D523 RID: 54563
		[MayRequireBiotech]
		public static LearningDesireDef Workwatching;

		// Token: 0x0400D524 RID: 54564
		[MayRequireBiotech]
		public static LearningDesireDef Reading;
	}
}
