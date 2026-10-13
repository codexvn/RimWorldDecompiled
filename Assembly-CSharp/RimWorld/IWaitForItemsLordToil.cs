using System;

namespace RimWorld
{
	// Token: 0x02001730 RID: 5936
	public interface IWaitForItemsLordToil
	{
		// Token: 0x1700157C RID: 5500
		// (get) Token: 0x06008B9F RID: 35743
		int CountRemaining { get; }

		// Token: 0x1700157D RID: 5501
		// (get) Token: 0x06008BA0 RID: 35744
		bool HasAllRequestedItems { get; }
	}
}
