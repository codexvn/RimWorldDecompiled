using System;

namespace RimWorld
{
	// Token: 0x020035F2 RID: 13810
	[DefOf]
	public static class TransportShipDefOf
	{
		// Token: 0x060133DF RID: 78815 RVA: 0x005B605A File Offset: 0x005B425A
		static TransportShipDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TransportShipDefOf));
		}

		// Token: 0x0400D4A8 RID: 54440
		[MayRequireRoyalty]
		public static TransportShipDef Ship_Shuttle;

		// Token: 0x0400D4A9 RID: 54441
		[MayRequireBiotech]
		public static TransportShipDef Ship_ShuttleCrashing;

		// Token: 0x0400D4AA RID: 54442
		[MayRequireBiotech]
		public static TransportShipDef Ship_ShuttleCrashing_Mechanitor;
	}
}
