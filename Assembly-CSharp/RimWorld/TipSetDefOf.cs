using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020035C1 RID: 13761
	[DefOf]
	public static class TipSetDefOf
	{
		// Token: 0x060133AF RID: 78767 RVA: 0x005B5D2A File Offset: 0x005B3F2A
		static TipSetDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TipSetDefOf));
		}

		// Token: 0x0400D28A RID: 53898
		public static TipSetDef GameplayTips;
	}
}
