using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020035E0 RID: 13792
	[DefOf]
	public static class ApparelLayerDefOf
	{
		// Token: 0x060133CE RID: 78798 RVA: 0x005B5F39 File Offset: 0x005B4139
		static ApparelLayerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ApparelLayerDefOf));
		}

		// Token: 0x0400D38D RID: 54157
		public static ApparelLayerDef OnSkin;

		// Token: 0x0400D38E RID: 54158
		public static ApparelLayerDef Shell;

		// Token: 0x0400D38F RID: 54159
		public static ApparelLayerDef Middle;

		// Token: 0x0400D390 RID: 54160
		public static ApparelLayerDef Belt;

		// Token: 0x0400D391 RID: 54161
		public static ApparelLayerDef Overhead;

		// Token: 0x0400D392 RID: 54162
		public static ApparelLayerDef EyeCover;
	}
}
