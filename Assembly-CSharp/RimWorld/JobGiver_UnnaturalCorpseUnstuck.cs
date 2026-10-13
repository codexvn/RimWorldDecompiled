using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001476 RID: 5238
	public class JobGiver_UnnaturalCorpseUnstuck : JobGiver_UnnaturalCorpseSkip
	{
		// Token: 0x06007E43 RID: 32323 RVA: 0x002603B6 File Offset: 0x0025E5B6
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.Downed)
			{
				return null;
			}
			if (GenTicks.TicksGame - pawn.TickSpawned <= 30)
			{
				return null;
			}
			if (pawn.pather.MovedRecently(30))
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x06007E44 RID: 32324 RVA: 0x002603EC File Offset: 0x0025E5EC
		protected override bool TryGetSkipCell(Pawn pawn, Pawn victim, out IntVec3 cell)
		{
			if (base.TryGetCellAlongPath(pawn, victim, 10, out cell))
			{
				return true;
			}
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(pawn.Position, 2f, 10f))
			{
				if (this.ValidateCell(pawn, intVec, victim.PositionHeld, pawn.Map))
				{
					cell = intVec;
					return true;
				}
			}
			if (!pawn.pather.MovedRecently(300))
			{
				cell = victim.PositionHeld;
				return true;
			}
			cell = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06007E45 RID: 32325 RVA: 0x002604A0 File Offset: 0x0025E6A0
		private bool ValidateCell(Pawn pawn, IntVec3 start, IntVec3 goal, Map map)
		{
			if (!start.Standable(map))
			{
				return false;
			}
			if (start.GetFirstPawn(map) != null)
			{
				return false;
			}
			float num = pawn.Position.DistanceTo(goal);
			return start.DistanceTo(goal) + 1f < num && pawn.CanReach(start, goal, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn);
		}

		// Token: 0x040054B9 RID: 21689
		private const int StuckTicks = 30;

		// Token: 0x040054BA RID: 21690
		private const int StuckTeleportToTicks = 300;

		// Token: 0x040054BB RID: 21691
		private const int MaxDist = 10;

		// Token: 0x040054BC RID: 21692
		private const int MinDist = 2;
	}
}
