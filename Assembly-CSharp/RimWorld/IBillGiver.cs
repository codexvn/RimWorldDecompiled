using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001101 RID: 4353
	public interface IBillGiver
	{
		// Token: 0x17001064 RID: 4196
		// (get) Token: 0x06006A0B RID: 27147
		Map Map { get; }

		// Token: 0x17001065 RID: 4197
		// (get) Token: 0x06006A0C RID: 27148
		BillStack BillStack { get; }

		// Token: 0x17001066 RID: 4198
		// (get) Token: 0x06006A0D RID: 27149
		IEnumerable<IntVec3> IngredientStackCells { get; }

		// Token: 0x17001067 RID: 4199
		// (get) Token: 0x06006A0E RID: 27150
		string LabelShort { get; }

		// Token: 0x06006A0F RID: 27151
		bool CurrentlyUsableForBills();

		// Token: 0x06006A10 RID: 27152
		bool UsableForBillsAfterFueling();

		// Token: 0x06006A11 RID: 27153
		void Notify_BillDeleted(Bill bill);
	}
}
