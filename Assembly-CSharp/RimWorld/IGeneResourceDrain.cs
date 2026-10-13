using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003841 RID: 14401
	public interface IGeneResourceDrain
	{
		// Token: 0x170032CF RID: 13007
		// (get) Token: 0x06013EA5 RID: 81573
		Gene_Resource Resource { get; }

		// Token: 0x170032D0 RID: 13008
		// (get) Token: 0x06013EA6 RID: 81574
		bool CanOffset { get; }

		// Token: 0x170032D1 RID: 13009
		// (get) Token: 0x06013EA7 RID: 81575
		float ResourceLossPerDay { get; }

		// Token: 0x170032D2 RID: 13010
		// (get) Token: 0x06013EA8 RID: 81576
		Pawn Pawn { get; }

		// Token: 0x170032D3 RID: 13011
		// (get) Token: 0x06013EA9 RID: 81577
		string DisplayLabel { get; }
	}
}
