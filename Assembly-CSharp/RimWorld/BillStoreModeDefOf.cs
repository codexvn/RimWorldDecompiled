using System;

namespace RimWorld
{
	// Token: 0x020035CF RID: 13775
	[DefOf]
	public static class BillStoreModeDefOf
	{
		// Token: 0x060133BD RID: 78781 RVA: 0x005B5E18 File Offset: 0x005B4018
		static BillStoreModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BillStoreModeDefOf));
		}

		// Token: 0x0400D31A RID: 54042
		public static BillStoreModeDef DropOnFloor;

		// Token: 0x0400D31B RID: 54043
		public static BillStoreModeDef BestStockpile;

		// Token: 0x0400D31C RID: 54044
		public static BillStoreModeDef SpecificStockpile;
	}
}
