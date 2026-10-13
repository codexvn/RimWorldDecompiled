using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02001022 RID: 4130
	public class JobGiver_WanderInRoofedCellsInPen : JobGiver_Wander
	{
		// Token: 0x0600650B RID: 25867 RVA: 0x001FF249 File Offset: 0x001FD449
		public JobGiver_WanderInRoofedCellsInPen()
		{
			this.wanderRadius = 10f;
			this.wanderDestValidator = ((Pawn pawn, IntVec3 cell, IntVec3 root) => cell.Roofed(pawn.Map));
		}

		// Token: 0x0600650C RID: 25868 RVA: 0x001FF284 File Offset: 0x001FD484
		protected override IntVec3 GetExactWanderDest(Pawn pawn)
		{
			Map map = pawn.Map;
			if (!JobGiver_WanderInRoofedCellsInPen.ShouldSeekRoofedCells(pawn))
			{
				return IntVec3.Invalid;
			}
			CompAnimalPenMarker currentPenOf = AnimalPenUtility.GetCurrentPenOf(pawn, false);
			if (currentPenOf == null)
			{
				return IntVec3.Invalid;
			}
			IntVec3 intVec = pawn.Position;
			if (!intVec.Roofed(map))
			{
				JobGiver_WanderInRoofedCellsInPen.wanderRootRegions.Clear();
				JobGiver_WanderInRoofedCellsInPen.wanderRootRegions.Add(pawn.GetRegion(RegionType.Set_Passable));
				intVec = JobGiver_WanderInRoofedCellsInPen.FindNearbyRoofedCellIn(pawn.Position, JobGiver_WanderInRoofedCellsInPen.wanderRootRegions, map);
			}
			if (!intVec.IsValid || !intVec.Roofed(map))
			{
				JobGiver_WanderInRoofedCellsInPen.wanderRootRegions.Clear();
				JobGiver_WanderInRoofedCellsInPen.wanderRootRegions.AddRange(currentPenOf.PenState.ConnectedRegions);
				JobGiver_WanderInRoofedCellsInPen.wanderRootRegions.Shuffle<Region>();
				JobGiver_WanderInRoofedCellsInPen.wanderRootRegions.TruncateToLength(4);
				intVec = JobGiver_WanderInRoofedCellsInPen.FindNearbyRoofedCellIn(pawn.Position, JobGiver_WanderInRoofedCellsInPen.wanderRootRegions, map);
			}
			JobGiver_WanderInRoofedCellsInPen.wanderRootRegions.Clear();
			if (!intVec.IsValid)
			{
				return IntVec3.Invalid;
			}
			return RCellFinder.RandomWanderDestFor(pawn, intVec, this.wanderRadius, this.wanderDestValidator, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger), false);
		}

		// Token: 0x0600650D RID: 25869 RVA: 0x00066B11 File Offset: 0x00064D11
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600650E RID: 25870 RVA: 0x001FF388 File Offset: 0x001FD588
		private static bool ShouldSeekRoofedCells(Pawn pawn)
		{
			using (List<GameCondition>.Enumerator enumerator = pawn.Map.GameConditionManager.ActiveConditions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def.pennedAnimalsSeekShelter)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600650F RID: 25871 RVA: 0x001FF3F0 File Offset: 0x001FD5F0
		private static IntVec3 FindNearbyRoofedCellIn(IntVec3 root, List<Region> regions, Map map)
		{
			JobGiver_WanderInRoofedCellsInPen.<>c__DisplayClass6_0 CS$<>8__locals1 = new JobGiver_WanderInRoofedCellsInPen.<>c__DisplayClass6_0();
			CS$<>8__locals1.map = map;
			IntVec3 result = IntVec3.Invalid;
			int num = int.MaxValue;
			using (List<Region>.Enumerator enumerator = regions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IntVec3 intVec;
					if (enumerator.Current.TryFindRandomCellInRegion(new Predicate<IntVec3>(CS$<>8__locals1.<FindNearbyRoofedCellIn>g__SafeCell|0), out intVec))
					{
						int num2 = root.DistanceToSquared(intVec);
						if (num2 < num)
						{
							result = intVec;
							num = num2;
						}
						if (num <= 100)
						{
							return result;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0400482C RID: 18476
		private const int PenRegionsSampleSize = 4;

		// Token: 0x0400482D RID: 18477
		private static readonly List<Region> wanderRootRegions = new List<Region>();
	}
}
