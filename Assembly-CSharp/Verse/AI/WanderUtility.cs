using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200102F RID: 4143
	public static class WanderUtility
	{
		// Token: 0x0600652F RID: 25903 RVA: 0x001FF79C File Offset: 0x001FD99C
		public static IntVec3 BestCloseWanderRoot(IntVec3 trueWanderRoot, Pawn pawn)
		{
			for (int i = 0; i < 50; i++)
			{
				IntVec3 intVec;
				if (i < 8)
				{
					intVec = trueWanderRoot + GenRadial.RadialPattern[i];
				}
				else
				{
					intVec = trueWanderRoot + GenRadial.RadialPattern[i - 8 + 1] * 7;
				}
				if (intVec.InBounds(pawn.Map) && intVec.WalkableBy(pawn.Map, pawn) && pawn.CanReach(intVec, PathEndMode.OnCell, Danger.Some, false, false, TraverseMode.ByPawn))
				{
					return intVec;
				}
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06006530 RID: 25904 RVA: 0x001FF824 File Offset: 0x001FDA24
		public static bool InSameRoom(IntVec3 locA, IntVec3 locB, Map map)
		{
			Room room = locA.GetRoom(map);
			return room == null || room == locB.GetRoom(map);
		}

		// Token: 0x06006531 RID: 25905 RVA: 0x001FF848 File Offset: 0x001FDA48
		public static IntVec3 GetColonyWanderRoot(Pawn pawn)
		{
			if (pawn.RaceProps.Humanlike && !pawn.IsSubhuman)
			{
				WanderUtility.gatherSpots.Clear();
				for (int i = 0; i < pawn.Map.gatherSpotLister.activeSpots.Count; i++)
				{
					IntVec3 position = pawn.Map.gatherSpotLister.activeSpots[i].parent.Position;
					if (!position.IsForbidden(pawn) && pawn.CanReach(position, PathEndMode.Touch, Danger.None, false, false, TraverseMode.ByPawn))
					{
						WanderUtility.gatherSpots.Add(position);
					}
				}
				if (WanderUtility.gatherSpots.Count > 0)
				{
					return WanderUtility.gatherSpots.RandomElement<IntVec3>();
				}
			}
			WanderUtility.candidateCells.Clear();
			WanderUtility.candidateBuildingsInRandomOrder.Clear();
			WanderUtility.candidateBuildingsInRandomOrder.AddRange(pawn.Map.listerBuildings.allBuildingsColonist);
			WanderUtility.candidateBuildingsInRandomOrder.Shuffle<Building>();
			int num = 0;
			int j = 0;
			while (j < WanderUtility.candidateBuildingsInRandomOrder.Count)
			{
				if (num > 80 && WanderUtility.candidateCells.Count > 0)
				{
					return WanderUtility.candidateCells.RandomElement<IntVec3>();
				}
				Building building = WanderUtility.candidateBuildingsInRandomOrder[j];
				if ((building.def == ThingDefOf.Wall || building.def.building.ai_chillDestination) && !building.Position.IsForbidden(pawn) && pawn.Map.areaManager.Home[building.Position])
				{
					IntVec3 intVec = GenAdjFast.AdjacentCells8Way(building).RandomElement<IntVec3>();
					if (intVec.Standable(building.Map) && !intVec.IsForbidden(pawn) && pawn.CanReach(intVec, PathEndMode.OnCell, Danger.None, false, false, TraverseMode.ByPawn) && !intVec.IsInPrisonCell(pawn.Map))
					{
						WanderUtility.candidateCells.Add(intVec);
						if ((pawn.Position - building.Position).LengthHorizontalSquared <= 1225)
						{
							return intVec;
						}
					}
				}
				j++;
				num++;
			}
			Pawn pawn2;
			if ((from c in pawn.Map.mapPawns.FreeColonistsSpawned
			where !c.Position.IsForbidden(pawn) && pawn.CanReach(c.Position, PathEndMode.Touch, Danger.None, false, false, TraverseMode.ByPawn)
			select c).TryRandomElement(out pawn2))
			{
				return pawn2.Position;
			}
			return pawn.Position;
		}

		// Token: 0x06006532 RID: 25906 RVA: 0x001FFAF0 File Offset: 0x001FDCF0
		public static IntVec3 GetHerdWanderRoot(Pawn pawn, Predicate<Thing> isHerdValidator)
		{
			Predicate<Thing> validator = (Thing t) => !t.Position.IsForbidden(pawn) && pawn.CanReach(t, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn) && Rand.Value >= 0.5f && isHerdValidator(t);
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 35f, validator, null, 13, -1, false, RegionType.Set_Passable, false, false);
			if (thing == null)
			{
				return pawn.Position;
			}
			return thing.Position;
		}

		// Token: 0x04004833 RID: 18483
		private static List<IntVec3> gatherSpots = new List<IntVec3>();

		// Token: 0x04004834 RID: 18484
		private static List<IntVec3> candidateCells = new List<IntVec3>();

		// Token: 0x04004835 RID: 18485
		private static List<Building> candidateBuildingsInRandomOrder = new List<Building>();
	}
}
