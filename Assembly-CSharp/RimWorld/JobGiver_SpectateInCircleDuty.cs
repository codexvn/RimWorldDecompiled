using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001451 RID: 5201
	public class JobGiver_SpectateInCircleDuty : JobGiver_SpectateDutySpectateRect
	{
		// Token: 0x06007DDA RID: 32218 RVA: 0x0025E7B8 File Offset: 0x0025C9B8
		protected override bool TryFindSpot(Pawn pawn, PawnDuty duty, out IntVec3 spot)
		{
			if (!SpectatorCellFinder.TryFindCircleSpectatorCellFor(pawn, duty.spectateRect, (float)duty.spectateDistance.min, (float)duty.spectateDistance.max, pawn.Map, out spot, null, new Func<IntVec3, Pawn, Map, bool>(RitualUtility.GoodSpectateCellForRitual)))
			{
				return base.TryFindSpot(pawn, duty, out spot);
			}
			return spot.IsValid;
		}
	}
}
