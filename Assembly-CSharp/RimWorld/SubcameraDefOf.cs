using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020035D7 RID: 13783
	[DefOf]
	public static class SubcameraDefOf
	{
		// Token: 0x060133C5 RID: 78789 RVA: 0x005B5EA0 File Offset: 0x005B40A0
		static SubcameraDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SubcameraDefOf));
		}

		// Token: 0x0400D34E RID: 54094
		public static SubcameraDef WaterDepth;

		// Token: 0x0400D34F RID: 54095
		[MayRequireOdyssey]
		public static SubcameraDef GravshipMask;
	}
}
