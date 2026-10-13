using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001420 RID: 5152
	public class JobGiver_PrepareCaravan_GatherDownedPawns : ThinkNode_JobGiver
	{
		// Token: 0x06007D4C RID: 32076 RVA: 0x0025BD4C File Offset: 0x00259F4C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return null;
			}
			Pawn pawn2 = this.FindDownedPawn(pawn);
			if (pawn2 == null)
			{
				return null;
			}
			IntVec3 c = JobGiver_PrepareCaravan_GatherDownedPawns.FindRandomDropCell(pawn, pawn2);
			Job job = JobMaker.MakeJob(JobDefOf.PrepareCaravan_GatherDownedPawns, pawn2, c);
			job.lord = pawn.GetLord();
			job.count = 1;
			return job;
		}

		// Token: 0x06007D4D RID: 32077 RVA: 0x0025BDB0 File Offset: 0x00259FB0
		private Pawn FindDownedPawn(Pawn pawn)
		{
			float num = 0f;
			Pawn pawn2 = null;
			List<Pawn> downedPawns = ((LordJob_FormAndSendCaravan)pawn.GetLord().LordJob).downedPawns;
			IntVec3 cell = pawn.mindState.duty.focusSecond.Cell;
			Pawn pawn3 = pawn.carryTracker.CarriedThing as Pawn;
			if (pawn3 != null && downedPawns.Contains(pawn3))
			{
				return pawn3;
			}
			for (int i = 0; i < downedPawns.Count; i++)
			{
				Pawn pawn4 = downedPawns[i];
				if (pawn4.Downed && pawn4 != pawn && !JobGiver_PrepareCaravan_GatherDownedPawns.IsDownedPawnNearExitPoint(pawn4, cell))
				{
					float num2 = (float)pawn.Position.DistanceToSquared(pawn4.Position);
					if ((pawn2 == null || num2 < num) && pawn.CanReserveAndReach(pawn4, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
					{
						pawn2 = pawn4;
						num = num2;
					}
				}
			}
			return pawn2;
		}

		// Token: 0x06007D4E RID: 32078 RVA: 0x0025BE84 File Offset: 0x0025A084
		public static IntVec3 FindRandomDropCell(Pawn pawn, Pawn downedPawn)
		{
			return CellFinder.RandomClosewalkCellNear(((LordJob_FormAndSendCaravan)CaravanFormingUtility.GetFormAndSendCaravanLord(downedPawn).LordJob).ExitSpot, pawn.Map, 6, (IntVec3 x) => x.Standable(pawn.Map) && StoreUtility.IsGoodStoreCell(x, pawn.Map, downedPawn, pawn, pawn.Faction));
		}

		// Token: 0x06007D4F RID: 32079 RVA: 0x0025BEDC File Offset: 0x0025A0DC
		public static bool IsDownedPawnNearExitPoint(Pawn downedPawn, IntVec3 exitPoint)
		{
			return downedPawn.PositionHeld.InHorDistOf(exitPoint, 7f);
		}

		// Token: 0x0400544A RID: 21578
		private const float MaxDownedPawnToExitPointDistance = 7f;
	}
}
