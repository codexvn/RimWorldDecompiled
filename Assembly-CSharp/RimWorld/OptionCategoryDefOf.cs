using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020035AD RID: 13741
	[DefOf]
	public static class OptionCategoryDefOf
	{
		// Token: 0x0601339B RID: 78747 RVA: 0x005B5BD6 File Offset: 0x005B3DD6
		static OptionCategoryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(OptionCategoryDefOf));
		}

		// Token: 0x0400CF3D RID: 53053
		public static OptionCategoryDef General;

		// Token: 0x0400CF3E RID: 53054
		public static OptionCategoryDef Graphics;

		// Token: 0x0400CF3F RID: 53055
		public static OptionCategoryDef Audio;

		// Token: 0x0400CF40 RID: 53056
		public static OptionCategoryDef Gameplay;

		// Token: 0x0400CF41 RID: 53057
		public static OptionCategoryDef Interface;

		// Token: 0x0400CF42 RID: 53058
		public static OptionCategoryDef Controls;

		// Token: 0x0400CF43 RID: 53059
		public static OptionCategoryDef Dev;

		// Token: 0x0400CF44 RID: 53060
		public static OptionCategoryDef Mods;
	}
}
