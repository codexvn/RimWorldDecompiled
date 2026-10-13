using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001471 RID: 5233
	public class JobGiver_UnnaturalCorpseSkip : ThinkNode_JobGiver
	{
		// Token: 0x06007E34 RID: 32308 RVA: 0x0025FFF0 File Offset: 0x0025E1F0
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.Downed)
			{
				return null;
			}
			UnnaturalCorpseTracker unnaturalCorpseTracker;
			if (!Find.Anomaly.TryGetUnnaturalCorpseTrackerForAwoken(pawn, out unnaturalCorpseTracker))
			{
				return null;
			}
			Pawn haunted = unnaturalCorpseTracker.Haunted;
			if (haunted.DestroyedOrNull())
			{
				return null;
			}
			if (!SkipUtility.CanEntitySkipNow(pawn, AbilityDefOf.UnnaturalCorpseSkip))
			{
				return null;
			}
			if (pawn.CanReachImmediate(haunted, PathEndMode.Touch))
			{
				return null;
			}
			IntVec3 cell;
			if (this.TryGetSkipCell(pawn, haunted, out cell))
			{
				return SkipUtility.GetEntitySkipJob(pawn, cell, AbilityDefOf.UnnaturalCorpseSkip);
			}
			return null;
		}

		// Token: 0x06007E35 RID: 32309 RVA: 0x00260064 File Offset: 0x0025E264
		protected virtual bool TryGetSkipCell(Pawn pawn, Pawn victim, out IntVec3 cell)
		{
			int maxCells = Mathf.CeilToInt(pawn.GetStatValue(StatDefOf.MoveSpeed, true, -1) * 40f);
			return this.TryGetNearbySkipCell(pawn, pawn.PositionHeld, victim, maxCells, out cell);
		}

		// Token: 0x06007E36 RID: 32310 RVA: 0x0026009C File Offset: 0x0025E29C
		protected bool TryGetNearbySkipCell(Pawn pawn, IntVec3 startPos, Pawn victim, int maxCells, out IntVec3 cell)
		{
			return RCellFinder.TryFindRandomCellNearWith(startPos, (IntVec3 x) => this.ValidateCell(pawn, x, victim.SpawnedParentOrMe, maxCells, pawn.Map), pawn.Map, out cell, 5, int.MaxValue);
		}

		// Token: 0x06007E37 RID: 32311 RVA: 0x002600F4 File Offset: 0x0025E2F4
		protected virtual bool ValidateCell(Pawn pawn, IntVec3 start, Thing goal, int maxCells, Map map)
		{
			return start.Standable(map) && start.GetFence(map) == null && start.GetDoor(map) == null && !start.Fogged(map) && start.GetFirstPawn(map) == null && pawn.Position.InHorDistOf(goal.PositionHeld, (float)maxCells) && !pawn.Position.InHorDistOf(goal.PositionHeld, 5f) && pawn.CanReach(start, goal, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn);
		}

		// Token: 0x06007E38 RID: 32312 RVA: 0x00260184 File Offset: 0x0025E384
		protected bool TryGetCellAlongPath(Pawn pawn, Pawn victim, int maxDist, out IntVec3 cell)
		{
			if (pawn.CanReach(pawn.Position, victim.PositionHeld, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) && pawn.pather.Moving && pawn.pather.curPath != null && pawn.pather.curPath.NodesLeftCount > 1)
			{
				for (int i = 1; i < pawn.pather.curPath.NodesLeftCount - 1; i++)
				{
					IntVec3 intVec = pawn.pather.curPath.Peek(i);
					if (this.ValidateCell(pawn, intVec, victim.SpawnedParentOrMe, maxDist, pawn.Map))
					{
						cell = intVec;
						return true;
					}
				}
			}
			cell = IntVec3.Invalid;
			return false;
		}

		// Token: 0x040054B1 RID: 21681
		private const float MoveSpeedCellsFactor = 40f;

		// Token: 0x040054B2 RID: 21682
		private const float MinDistFromTarget = 5f;
	}
}
