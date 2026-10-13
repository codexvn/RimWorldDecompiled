using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020035AE RID: 13742
	[DefOf]
	public static class ThinkTreeDefOf
	{
		// Token: 0x0601339C RID: 78748 RVA: 0x005B5BE7 File Offset: 0x005B3DE7
		static ThinkTreeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ThinkTreeDefOf));
		}

		// Token: 0x0400CF45 RID: 53061
		[MayRequireAnomaly]
		public static ThinkTreeDef ShamblerConstant;
	}
}
