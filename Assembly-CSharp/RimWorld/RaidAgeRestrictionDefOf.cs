using System;

namespace RimWorld
{
	// Token: 0x020035A9 RID: 13737
	[DefOf]
	public static class RaidAgeRestrictionDefOf
	{
		// Token: 0x06013397 RID: 78743 RVA: 0x005B5B92 File Offset: 0x005B3D92
		static RaidAgeRestrictionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RaidAgeRestrictionDefOf));
		}

		// Token: 0x0400CF0C RID: 53004
		[MayRequireBiotech]
		public static RaidAgeRestrictionDef Children;
	}
}
