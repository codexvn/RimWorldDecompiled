using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200148C RID: 5260
	public class JobGiver_Forage : ThinkNode_JobGiver
	{
		// Token: 0x06007E80 RID: 32384 RVA: 0x002611BC File Offset: 0x0025F3BC
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c2;
			if (!CellFinder.TryFindRandomCellNear(pawn.Position, pawn.Map, 30, (IntVec3 c) => c.GetFertility(pawn.Map) >= 0.7f && c.GetTerrain(pawn.Map).natural && !c.IsForbidden(pawn) && pawn.CanReserveAndReach(c, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, false), out c2, -1))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Forage, c2);
		}

		// Token: 0x040054D6 RID: 21718
		private const int SearchRadius = 30;

		// Token: 0x040054D7 RID: 21719
		private const float MinFertility = 0.7f;
	}
}
