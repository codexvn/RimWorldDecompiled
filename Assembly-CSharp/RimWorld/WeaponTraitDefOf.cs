using System;

namespace RimWorld
{
	// Token: 0x020035F0 RID: 13808
	[DefOf]
	public static class WeaponTraitDefOf
	{
		// Token: 0x060133DD RID: 78813 RVA: 0x005B6038 File Offset: 0x005B4238
		static WeaponTraitDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WeaponTraitDefOf));
		}

		// Token: 0x0400D418 RID: 54296
		[MayRequireRoyalty]
		public static WeaponTraitDef NeedKill;
	}
}
