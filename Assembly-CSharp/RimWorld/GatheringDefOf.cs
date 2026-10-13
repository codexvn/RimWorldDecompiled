using System;

namespace RimWorld
{
	// Token: 0x020035EA RID: 13802
	[DefOf]
	public static class GatheringDefOf
	{
		// Token: 0x060133D8 RID: 78808 RVA: 0x005B5FE3 File Offset: 0x005B41E3
		static GatheringDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(GatheringDefOf));
		}

		// Token: 0x0400D3C0 RID: 54208
		public static GatheringDef Party;

		// Token: 0x0400D3C1 RID: 54209
		public static GatheringDef MarriageCeremony;
	}
}
