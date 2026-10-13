using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020035D0 RID: 13776
	[DefOf]
	public static class ReservationLayerDefOf
	{
		// Token: 0x060133BE RID: 78782 RVA: 0x005B5E29 File Offset: 0x005B4029
		static ReservationLayerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ReservationLayerDefOf));
		}

		// Token: 0x0400D31D RID: 54045
		public static ReservationLayerDef Floor;

		// Token: 0x0400D31E RID: 54046
		public static ReservationLayerDef Ceiling;

		// Token: 0x0400D31F RID: 54047
		public static ReservationLayerDef Empty;
	}
}
