using System;

namespace RimWorld
{
	// Token: 0x02003602 RID: 13826
	[DefOf]
	public static class RitualVisualEffectDefOf
	{
		// Token: 0x060133EF RID: 78831 RVA: 0x005B616A File Offset: 0x005B436A
		static RitualVisualEffectDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RitualVisualEffectDefOf));
		}

		// Token: 0x0400D506 RID: 54534
		[MayRequireIdeology]
		public static RitualVisualEffectDef Basic;
	}
}
