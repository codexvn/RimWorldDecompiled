using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020035E8 RID: 13800
	[DefOf]
	public static class GenStepDefOf
	{
		// Token: 0x060133D6 RID: 78806 RVA: 0x005B5FC1 File Offset: 0x005B41C1
		static GenStepDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(GenStepDefOf));
		}

		// Token: 0x0400D3B8 RID: 54200
		public static GenStepDef PreciousLump;

		// Token: 0x0400D3B9 RID: 54201
		[MayRequireAnomaly]
		public static GenStepDef HarbingerTrees;

		// Token: 0x0400D3BA RID: 54202
		[MayRequireOdyssey]
		public static GenStepDef GravshipMarker;

		// Token: 0x0400D3BB RID: 54203
		[MayRequireOdyssey]
		public static GenStepDef ReserveGravshipArea;

		// Token: 0x0400D3BC RID: 54204
		[MayRequireOdyssey]
		public static GenStepDef AncientRuins_Special;

		// Token: 0x0400D3BD RID: 54205
		[MayRequireOdyssey]
		public static GenStepDef AncientStockpile;
	}
}
