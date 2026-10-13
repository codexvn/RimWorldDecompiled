using System;

namespace RimWorld
{
	// Token: 0x020035ED RID: 13805
	[DefOf]
	public static class AbilityDefOf
	{
		// Token: 0x060133DB RID: 78811 RVA: 0x005B6016 File Offset: 0x005B4216
		static AbilityDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(AbilityDefOf));
		}

		// Token: 0x0400D3DB RID: 54235
		[MayRequireRoyalty]
		public static AbilityDef Speech;

		// Token: 0x0400D3DC RID: 54236
		[MayRequireBiotech]
		public static AbilityDef ReimplantXenogerm;

		// Token: 0x0400D3DD RID: 54237
		[MayRequireBiotech]
		public static AbilityDef ResurrectionMech;

		// Token: 0x0400D3DE RID: 54238
		[MayRequireAnomaly]
		public static AbilityDef EntitySkip;

		// Token: 0x0400D3DF RID: 54239
		[MayRequireAnomaly]
		public static AbilityDef UnnaturalCorpseSkip;

		// Token: 0x0400D3E0 RID: 54240
		[MayRequireAnomaly]
		public static AbilityDef ConsumeLeap_Devourer;

		// Token: 0x0400D3E1 RID: 54241
		[MayRequireOdyssey]
		public static AbilityDef SludgeSpew;

		// Token: 0x0400D3E2 RID: 54242
		[MayRequireOdyssey]
		public static AbilityDef EggSpew;
	}
}
