using System;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x020005CC RID: 1484
	public static class PathUtility
	{
		// Token: 0x06002620 RID: 9760 RVA: 0x000C9060 File Offset: 0x000C7260
		public static Area GetAllowedArea(Pawn pawn)
		{
			if (pawn != null && pawn.playerSettings != null && !pawn.Drafted && ForbidUtility.CaresAboutForbidden(pawn, true, false))
			{
				Area area = pawn.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap;
				if (area != null && area.TrueCount <= 0)
				{
					area = null;
				}
				return area;
			}
			return null;
		}

		// Token: 0x06002621 RID: 9761 RVA: 0x000C90A7 File Offset: 0x000C72A7
		public static bool IsDestroyable(Thing thing)
		{
			return thing.def.useHitPoints && thing.def.destroyable;
		}

		// Token: 0x06002622 RID: 9762 RVA: 0x000C90C4 File Offset: 0x000C72C4
		public static ushort GetDoorCost(Building_Door door, TraverseParms traverseParms, Pawn pawn, PathFinderCostTuning? tuningParams)
		{
			PathFinderCostTuning pathFinderCostTuning = tuningParams ?? PathFinderCostTuning.DefaultTuning;
			switch (traverseParms.mode)
			{
			case TraverseMode.ByPawn:
				if (!traverseParms.canBashDoors && door.IsForbiddenToPass(pawn))
				{
					return ushort.MaxValue;
				}
				if (door.PawnCanOpen(pawn) && !door.FreePassage)
				{
					return (ushort)door.TicksToOpenNow;
				}
				if (door.CanPhysicallyPass(pawn))
				{
					return 0;
				}
				if (traverseParms.canBashDoors)
				{
					return 300;
				}
				return ushort.MaxValue;
			case TraverseMode.PassDoors:
				if (pawn != null && door.PawnCanOpen(pawn) && !door.IsForbiddenToPass(pawn) && !door.FreePassage)
				{
					return (ushort)door.TicksToOpenNow;
				}
				if ((pawn != null && door.CanPhysicallyPass(pawn)) || door.FreePassage)
				{
					return 0;
				}
				return 150;
			case TraverseMode.NoPassClosedDoors:
			case TraverseMode.NoPassClosedDoorsOrWater:
				if (door.FreePassage)
				{
					return 0;
				}
				return ushort.MaxValue;
			case TraverseMode.PassAllDestroyableThings:
			case TraverseMode.PassAllDestroyablePlayerOwnedThings:
			case TraverseMode.PassAllDestroyableThingsNotWater:
				if (pawn != null && door.PawnCanOpen(pawn) && !door.IsForbiddenToPass(pawn) && !door.FreePassage)
				{
					return (ushort)door.TicksToOpenNow;
				}
				if ((pawn != null && door.CanPhysicallyPass(pawn)) || door.FreePassage)
				{
					return 0;
				}
				if (traverseParms.mode == TraverseMode.PassAllDestroyablePlayerOwnedThings && door.Faction != null && !door.Faction.IsPlayer)
				{
					return ushort.MaxValue;
				}
				return (ushort)((float)pathFinderCostTuning.costBlockedDoor + (float)door.HitPoints * pathFinderCostTuning.costBlockedWallExtraPerHitPoint);
			default:
				return 0;
			}
		}

		// Token: 0x06002623 RID: 9763 RVA: 0x000C9234 File Offset: 0x000C7434
		public static ushort GetBuildingCost(Building b, TraverseParms traverseParms, Pawn pawn, PathFinderCostTuning? tuningParms)
		{
			PathFinderCostTuning pathFinderCostTuning = tuningParms ?? PathFinderCostTuning.DefaultTuning;
			int costBlockedDoor = pathFinderCostTuning.costBlockedDoor;
			float costBlockedDoorPerHitPoint = pathFinderCostTuning.costBlockedDoorPerHitPoint;
			Building_Door building_Door = b as Building_Door;
			if (building_Door != null)
			{
				return PathUtility.GetDoorCost(building_Door, traverseParms, pawn, tuningParms);
			}
			if (b.def.IsFence && traverseParms.fenceBlocked)
			{
				switch (traverseParms.mode)
				{
				case TraverseMode.ByPawn:
					if (traverseParms.canBashFences)
					{
						return 300;
					}
					return ushort.MaxValue;
				case TraverseMode.PassDoors:
				case TraverseMode.NoPassClosedDoors:
				case TraverseMode.NoPassClosedDoorsOrWater:
					return 0;
				case TraverseMode.PassAllDestroyableThings:
				case TraverseMode.PassAllDestroyableThingsNotWater:
					return (ushort)(costBlockedDoor + (int)((ushort)((float)b.HitPoints * costBlockedDoorPerHitPoint)));
				case TraverseMode.PassAllDestroyablePlayerOwnedThings:
					if (!b.Faction.IsPlayer)
					{
						return ushort.MaxValue;
					}
					return (ushort)(costBlockedDoor + (int)((float)b.HitPoints * costBlockedDoorPerHitPoint));
				}
			}
			else if (pawn != null)
			{
				IPathFindCostProvider pathFindCostProvider = b as IPathFindCostProvider;
				if (pathFindCostProvider != null)
				{
					return pathFindCostProvider.PathFindCostFor(pawn);
				}
			}
			return 0;
		}

		// Token: 0x06002624 RID: 9764 RVA: 0x000C931B File Offset: 0x000C751B
		public static bool BlocksDiagonalMovement(int x, int z, PathingContext pc, bool canBashFences)
		{
			return PathUtility.BlocksDiagonalMovement(pc.map.cellIndices.CellToIndex(x, z), pc, canBashFences);
		}

		// Token: 0x06002625 RID: 9765 RVA: 0x000C9338 File Offset: 0x000C7538
		public static bool BlocksDiagonalMovement(int index, PathingContext pc, bool canBashFences)
		{
			if (!pc.pathGrid.WalkableFast(index))
			{
				return true;
			}
			Building building = pc.map.edificeGrid[index];
			return building != null && canBashFences && building.def.IsFence;
		}

		// Token: 0x04001FA6 RID: 8102
		private const int Cost_DoorToBash = 300;

		// Token: 0x04001FA7 RID: 8103
		private const int Cost_FenceToBash = 300;
	}
}
