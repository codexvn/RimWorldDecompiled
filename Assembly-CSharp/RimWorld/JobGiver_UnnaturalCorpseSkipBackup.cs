using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001473 RID: 5235
	public class JobGiver_UnnaturalCorpseSkipBackup : JobGiver_UnnaturalCorpseSkip
	{
		// Token: 0x06007E3C RID: 32316 RVA: 0x00260269 File Offset: 0x0025E469
		protected override bool TryGetSkipCell(Pawn pawn, Pawn victim, out IntVec3 cell)
		{
			if (base.TryGetCellAlongPath(pawn, victim, 64, out cell))
			{
				return true;
			}
			if (base.TryGetNearbySkipCell(pawn, victim.PositionHeld, victim, 48, out cell))
			{
				return true;
			}
			cell = victim.Position;
			return true;
		}
	}
}
