using System;

namespace RimWorld
{
	// Token: 0x02002F71 RID: 12145
	public interface ITraderRestockingInfoProvider
	{
		// Token: 0x17002C44 RID: 11332
		// (get) Token: 0x060111B9 RID: 70073
		bool EverVisited { get; }

		// Token: 0x17002C45 RID: 11333
		// (get) Token: 0x060111BA RID: 70074
		bool RestockedSinceLastVisit { get; }

		// Token: 0x17002C46 RID: 11334
		// (get) Token: 0x060111BB RID: 70075
		int NextRestockTick { get; }
	}
}
