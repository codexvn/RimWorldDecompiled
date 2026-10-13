using System;

namespace RimWorld
{
	// Token: 0x02003627 RID: 13863
	[DefOf]
	public static class WeaponCategoryDefOf
	{
		// Token: 0x06013412 RID: 78866 RVA: 0x005B63BD File Offset: 0x005B45BD
		static WeaponCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WeaponCategoryDefOf));
		}

		// Token: 0x0400D5A7 RID: 54695
		[MayRequireRoyalty]
		public static WeaponCategoryDef BladeLink;
	}
}
