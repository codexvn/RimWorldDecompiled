using System;

namespace RimWorld
{
	// Token: 0x02003600 RID: 13824
	[DefOf]
	public static class GauranlenTreeModeDefOf
	{
		// Token: 0x060133ED RID: 78829 RVA: 0x005B6148 File Offset: 0x005B4348
		static GauranlenTreeModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(GauranlenTreeModeDefOf));
		}

		// Token: 0x0400D501 RID: 54529
		[MayRequireIdeology]
		public static GauranlenTreeModeDef Gaumaker;
	}
}
