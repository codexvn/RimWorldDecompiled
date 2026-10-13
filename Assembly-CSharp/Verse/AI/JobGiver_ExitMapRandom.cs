using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02001003 RID: 4099
	public class JobGiver_ExitMapRandom : JobGiver_ExitMap
	{
		// Token: 0x060064A0 RID: 25760 RVA: 0x001FDFD8 File Offset: 0x001FC1D8
		protected override bool TryFindGoodExitDest(Pawn pawn, bool canDig, bool canBash, out IntVec3 spot)
		{
			TraverseMode mode = canDig ? TraverseMode.PassAllDestroyableThings : TraverseMode.ByPawn;
			return RCellFinder.TryFindRandomExitSpot(pawn, out spot, mode);
		}
	}
}
