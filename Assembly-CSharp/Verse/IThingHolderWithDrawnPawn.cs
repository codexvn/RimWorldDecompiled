using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020008DE RID: 2270
	public interface IThingHolderWithDrawnPawn : IThingHolder
	{
		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x060039F1 RID: 14833
		float HeldPawnDrawPos_Y { get; }

		// Token: 0x17000ACE RID: 2766
		// (get) Token: 0x060039F2 RID: 14834
		float HeldPawnBodyAngle { get; }

		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x060039F3 RID: 14835
		PawnPosture HeldPawnPosture { get; }
	}
}
