using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001207 RID: 4615
	public static class NatureRunningUtility
	{
		// Token: 0x060070C2 RID: 28866 RVA: 0x0022F3B4 File Offset: 0x0022D5B4
		public static bool TryFindNatureInterestTarget(Pawn searcher, out LocalTargetInfo interestTarget)
		{
			interestTarget = LocalTargetInfo.Invalid;
			if (!JoyUtility.EnjoyableOutsideNow(searcher, null))
			{
				return false;
			}
			NatureRunningUtility.tmpRegionContainsOwnedBuildingCache.Clear();
			LocalTargetInfo bfsTarget = LocalTargetInfo.Invalid;
			TraverseParms traverseParms = TraverseParms.For(searcher, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true);
			int additionalRegionsToCheck = 8;
			RegionTraverser.BreadthFirstTraverse(searcher.Position, searcher.Map, (Region from, Region r) => r.Allows(traverseParms, false), delegate(Region r)
			{
				if (bfsTarget.IsValid)
				{
					int num = additionalRegionsToCheck - 1;
					additionalRegionsToCheck = num;
					if (num <= 0)
					{
						return true;
					}
				}
				if (r.IsForbiddenEntirely(searcher))
				{
					return false;
				}
				if (r.DangerFor(searcher) == Danger.Deadly)
				{
					return false;
				}
				if (r.extentsClose.ClosestDistSquaredTo(searcher.Position) < 225f)
				{
					return false;
				}
				CellRect checkNearbyRegionsTouchingArea = r.extentsClose.ExpandedBy(15);
				bool foundNearbyRegionWithBuildings = false;
				RegionTraverser.BreadthFirstTraverse(r, (Region from, Region to) => to.extentsClose.Overlaps(checkNearbyRegionsTouchingArea), delegate(Region region)
				{
					bool flag;
					if (!NatureRunningUtility.tmpRegionContainsOwnedBuildingCache.TryGetValue(region, out flag))
					{
						using (List<Thing>.Enumerator enumerator = region.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial).GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (enumerator.Current.Faction != null)
								{
									foundNearbyRegionWithBuildings = true;
									NatureRunningUtility.tmpRegionContainsOwnedBuildingCache[region] = true;
									return true;
								}
							}
						}
						NatureRunningUtility.tmpRegionContainsOwnedBuildingCache[region] = false;
						return false;
					}
					if (flag)
					{
						foundNearbyRegionWithBuildings = true;
						return true;
					}
					return false;
				}, 999999, RegionType.Set_Passable);
				if (foundNearbyRegionWithBuildings)
				{
					return false;
				}
				IntVec3 intVec;
				if (r.TryFindRandomCellInRegion(new Predicate<IntVec3>(base.<TryFindNatureInterestTarget>g__CellValidator|4), out intVec) && (!bfsTarget.IsValid || searcher.Position.DistanceTo(bfsTarget.Cell) > searcher.Position.DistanceTo(intVec)))
				{
					bfsTarget = intVec;
				}
				return false;
			}, 300, RegionType.Set_Passable);
			NatureRunningUtility.tmpRegionContainsOwnedBuildingCache.Clear();
			interestTarget = bfsTarget;
			return interestTarget.IsValid;
		}

		// Token: 0x04004E49 RID: 20041
		private const int MinRegionExtantDistanceFromVisited = 15;

		// Token: 0x04004E4A RID: 20042
		private const int MinRegionExtantDistanceFromBuildingRegion = 15;

		// Token: 0x04004E4B RID: 20043
		private const int MaxRegionsSearch = 300;

		// Token: 0x04004E4C RID: 20044
		private const int RegionsToCheckAfterValidFound = 8;

		// Token: 0x04004E4D RID: 20045
		private static readonly Dictionary<Region, bool> tmpRegionContainsOwnedBuildingCache = new Dictionary<Region, bool>();
	}
}
