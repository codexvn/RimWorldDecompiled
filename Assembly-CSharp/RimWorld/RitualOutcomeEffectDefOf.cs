using System;

namespace RimWorld
{
	// Token: 0x020035FB RID: 13819
	[DefOf]
	public static class RitualOutcomeEffectDefOf
	{
		// Token: 0x060133E8 RID: 78824 RVA: 0x005B60F3 File Offset: 0x005B42F3
		static RitualOutcomeEffectDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RitualOutcomeEffectDefOf));
		}

		// Token: 0x0400D4DF RID: 54495
		[MayRequireRoyalty]
		public static RitualOutcomeEffectDef BestowingCeremony;

		// Token: 0x0400D4E0 RID: 54496
		[MayRequireBiotech]
		public static RitualOutcomeEffectDef ChildBirth;
	}
}
