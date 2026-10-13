using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003623 RID: 13859
	[DefOf]
	public static class GraphicStateDefOf
	{
		// Token: 0x0601340E RID: 78862 RVA: 0x005B6379 File Offset: 0x005B4579
		static GraphicStateDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(GraphicStateDefOf));
		}

		// Token: 0x0400D595 RID: 54677
		public static GraphicStateDef Swimming;

		// Token: 0x0400D596 RID: 54678
		public static GraphicStateDef Stationary;
	}
}
