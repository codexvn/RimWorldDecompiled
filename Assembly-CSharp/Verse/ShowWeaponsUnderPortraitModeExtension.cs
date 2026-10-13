using System;

namespace Verse
{
	// Token: 0x02000D53 RID: 3411
	public static class ShowWeaponsUnderPortraitModeExtension
	{
		// Token: 0x060056E3 RID: 22243 RVA: 0x001BEF8C File Offset: 0x001BD18C
		public static string ToStringHuman(this ShowWeaponsUnderPortraitMode mode)
		{
			switch (mode)
			{
			case ShowWeaponsUnderPortraitMode.Never:
				return "Never".Translate().CapitalizeFirst();
			case ShowWeaponsUnderPortraitMode.WhileDrafted:
				return "ShowWeapons_WhileDrafted".Translate().CapitalizeFirst();
			case ShowWeaponsUnderPortraitMode.Always:
				return "ShowWeapons_Always".Translate().CapitalizeFirst();
			default:
				throw new NotImplementedException();
			}
		}
	}
}
