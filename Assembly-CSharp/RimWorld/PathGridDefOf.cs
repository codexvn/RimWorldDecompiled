using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003626 RID: 13862
	[DefOf]
	public static class PathGridDefOf
	{
		// Token: 0x06013411 RID: 78865 RVA: 0x005B63AC File Offset: 0x005B45AC
		static PathGridDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PathGridDefOf));
		}

		// Token: 0x0400D5A4 RID: 54692
		public static PathGridDef Normal;

		// Token: 0x0400D5A5 RID: 54693
		public static PathGridDef FenceBlocked;

		// Token: 0x0400D5A6 RID: 54694
		public static PathGridDef Flying;
	}
}
