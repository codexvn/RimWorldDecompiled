using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003612 RID: 13842
	[DefOf]
	public static class PawnRenderNodeTagDefOf
	{
		// Token: 0x060133FD RID: 78845 RVA: 0x005B6258 File Offset: 0x005B4458
		static PawnRenderNodeTagDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnRenderNodeTagDefOf));
		}

		// Token: 0x0400D539 RID: 54585
		public static PawnRenderNodeTagDef Head;

		// Token: 0x0400D53A RID: 54586
		public static PawnRenderNodeTagDef Body;

		// Token: 0x0400D53B RID: 54587
		public static PawnRenderNodeTagDef ApparelHead;

		// Token: 0x0400D53C RID: 54588
		public static PawnRenderNodeTagDef ApparelBody;
	}
}
