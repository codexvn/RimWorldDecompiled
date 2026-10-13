using System;

namespace RimWorld
{
	// Token: 0x02003609 RID: 13833
	[DefOf]
	public static class DebugTabMenuDefOf
	{
		// Token: 0x060133F4 RID: 78836 RVA: 0x005B61BF File Offset: 0x005B43BF
		static DebugTabMenuDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DebugTabMenuDefOf));
		}

		// Token: 0x0400D51B RID: 54555
		public static DebugTabMenuDef Actions;

		// Token: 0x0400D51C RID: 54556
		public static DebugTabMenuDef Settings;

		// Token: 0x0400D51D RID: 54557
		public static DebugTabMenuDef Output;
	}
}
