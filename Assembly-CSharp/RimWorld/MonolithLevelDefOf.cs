using System;

namespace RimWorld
{
	// Token: 0x02003616 RID: 13846
	[DefOf]
	public static class MonolithLevelDefOf
	{
		// Token: 0x06013401 RID: 78849 RVA: 0x005B629C File Offset: 0x005B449C
		static MonolithLevelDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MonolithLevelDefOf));
		}

		// Token: 0x0400D543 RID: 54595
		[MayRequireAnomaly]
		public static MonolithLevelDef Inactive;

		// Token: 0x0400D544 RID: 54596
		[MayRequireAnomaly]
		public static MonolithLevelDef Waking;

		// Token: 0x0400D545 RID: 54597
		[MayRequireAnomaly]
		public static MonolithLevelDef VoidAwakened;

		// Token: 0x0400D546 RID: 54598
		[MayRequireAnomaly]
		public static MonolithLevelDef Gleaming;

		// Token: 0x0400D547 RID: 54599
		[MayRequireAnomaly]
		public static MonolithLevelDef Embraced;

		// Token: 0x0400D548 RID: 54600
		[MayRequireAnomaly]
		public static MonolithLevelDef Disrupted;
	}
}
