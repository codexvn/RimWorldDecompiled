using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02001004 RID: 4100
	public class JobGiver_ExitMapBest : JobGiver_ExitMap
	{
		// Token: 0x060064A2 RID: 25762 RVA: 0x001FE000 File Offset: 0x001FC200
		protected override bool TryFindGoodExitDest(Pawn pawn, bool canDig, bool canBash, out IntVec3 spot)
		{
			TraverseMode mode = canDig ? TraverseMode.PassAllDestroyableThings : TraverseMode.ByPawn;
			return RCellFinder.TryFindBestExitSpot(pawn, out spot, mode, canBash);
		}
	}
}
