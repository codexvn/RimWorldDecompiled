using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020029B0 RID: 10672
	public interface ISlotGroupParent : IStoreSettingsParent, IHaulDestination
	{
		// Token: 0x17002427 RID: 9255
		// (get) Token: 0x0600EB2C RID: 60204
		bool IgnoreStoredThingsBeauty { get; }

		// Token: 0x0600EB2D RID: 60205
		IEnumerable<IntVec3> AllSlotCells();

		// Token: 0x0600EB2E RID: 60206
		List<IntVec3> AllSlotCellsList();

		// Token: 0x0600EB2F RID: 60207
		void Notify_ReceivedThing(Thing newItem);

		// Token: 0x0600EB30 RID: 60208
		void Notify_LostThing(Thing newItem);

		// Token: 0x0600EB31 RID: 60209
		string SlotYielderLabel();

		// Token: 0x17002428 RID: 9256
		// (get) Token: 0x0600EB32 RID: 60210
		string GroupingLabel { get; }

		// Token: 0x17002429 RID: 9257
		// (get) Token: 0x0600EB33 RID: 60211
		int GroupingOrder { get; }

		// Token: 0x0600EB34 RID: 60212
		SlotGroup GetSlotGroup();
	}
}
