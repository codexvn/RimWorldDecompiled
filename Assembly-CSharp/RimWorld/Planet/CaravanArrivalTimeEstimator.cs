using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D40 RID: 15680
	public static class CaravanArrivalTimeEstimator
	{
		// Token: 0x060155CE RID: 87502 RVA: 0x00663290 File Offset: 0x00661490
		public static int EstimatedTicksToArrive(Caravan caravan, bool allowCaching)
		{
			if (allowCaching && caravan == CaravanArrivalTimeEstimator.cachedForCaravan && caravan.pather.Destination == CaravanArrivalTimeEstimator.cachedForDest && Find.TickManager.TicksGame - CaravanArrivalTimeEstimator.cacheTicks < 100)
			{
				return CaravanArrivalTimeEstimator.cachedResult;
			}
			PlanetTile to;
			int result;
			if (!caravan.Spawned || !caravan.pather.Moving || caravan.pather.curPath == null)
			{
				to = PlanetTile.Invalid;
				result = 0;
			}
			else
			{
				to = caravan.pather.Destination;
				result = CaravanArrivalTimeEstimator.EstimatedTicksToArrive(caravan.Tile, to, caravan.pather.curPath, caravan.pather.nextTileCostLeft, caravan.TicksPerMove, Find.TickManager.TicksAbs);
			}
			if (allowCaching)
			{
				CaravanArrivalTimeEstimator.cacheTicks = Find.TickManager.TicksGame;
				CaravanArrivalTimeEstimator.cachedForCaravan = caravan;
				CaravanArrivalTimeEstimator.cachedForDest = to;
				CaravanArrivalTimeEstimator.cachedResult = result;
			}
			return result;
		}

		// Token: 0x060155CF RID: 87503 RVA: 0x0066336C File Offset: 0x0066156C
		public static int EstimatedTicksToArrive(PlanetTile from, PlanetTile to, Caravan caravan)
		{
			int result;
			using (WorldPath worldPath = from.Layer.Pather.FindPath(from, to, caravan, null))
			{
				if (!worldPath.Found)
				{
					result = 0;
				}
				else
				{
					result = CaravanArrivalTimeEstimator.EstimatedTicksToArrive(from, to, worldPath, 0f, (caravan != null) ? caravan.TicksPerMove : 3300, Find.TickManager.TicksAbs);
				}
			}
			return result;
		}

		// Token: 0x060155D0 RID: 87504 RVA: 0x006633E0 File Offset: 0x006615E0
		public static int EstimatedTicksToArrive(PlanetTile from, PlanetTile to, WorldPath path, float nextTileCostLeft, int caravanTicksPerMove, int curTicksAbs)
		{
			CaravanArrivalTimeEstimator.tmpTicksToArrive.Clear();
			CaravanArrivalTimeEstimator.EstimatedTicksToArriveToEvery(from, to, path, nextTileCostLeft, caravanTicksPerMove, curTicksAbs, CaravanArrivalTimeEstimator.tmpTicksToArrive);
			return CaravanArrivalTimeEstimator.EstimatedTicksToArrive(to, CaravanArrivalTimeEstimator.tmpTicksToArrive);
		}

		// Token: 0x060155D1 RID: 87505 RVA: 0x0066340C File Offset: 0x0066160C
		public static void EstimatedTicksToArriveToEvery(PlanetTile from, PlanetTile to, WorldPath path, float nextTileCostLeft, int caravanTicksPerMove, int curTicksAbs, [TupleElementNames(new string[]
		{
			"tile",
			"ticks"
		})] List<ValueTuple<PlanetTile, int>> outTicksToArrive)
		{
			outTicksToArrive.Clear();
			outTicksToArrive.Add(new ValueTuple<PlanetTile, int>(from, 0));
			if (from == to || !from.Valid || !from.LayerDef.SurfaceTiles)
			{
				outTicksToArrive.Add(new ValueTuple<PlanetTile, int>(to, 0));
				return;
			}
			int num = 0;
			PlanetTile planetTile = from;
			int num2 = 0;
			int num3 = Mathf.CeilToInt(20000f) - 1;
			int num4 = 60000 - num3;
			int num5 = 0;
			int num6 = 0;
			int num8;
			if (CaravanNightRestUtility.WouldBeRestingAt(from, (long)curTicksAbs))
			{
				if (Caravan_PathFollower.IsValidFinalPushDestination(to) && (path.Peek(0) == to || (nextTileCostLeft <= 0f && path.NodesLeftCount >= 2 && path.Peek(1) == to)))
				{
					int num7 = Mathf.CeilToInt(CaravanArrivalTimeEstimator.GetCostToMove(nextTileCostLeft, path.Peek(0) == to, curTicksAbs, num, caravanTicksPerMove, from, to) / 1f);
					if (num7 <= 10000)
					{
						num += num7;
						outTicksToArrive.Add(new ValueTuple<PlanetTile, int>(to, num));
						return;
					}
				}
				num += CaravanNightRestUtility.LeftRestTicksAt(from, (long)curTicksAbs);
				num8 = num4;
			}
			else
			{
				num8 = CaravanNightRestUtility.LeftNonRestTicksAt(from, (long)curTicksAbs);
			}
			for (;;)
			{
				num6++;
				if (num6 >= 10000)
				{
					break;
				}
				if (num5 <= 0)
				{
					if (planetTile == to)
					{
						goto Block_11;
					}
					bool firstInPath = num2 == 0;
					PlanetTile planetTile2 = planetTile;
					planetTile = path.Peek(num2);
					num2++;
					outTicksToArrive.Add(new ValueTuple<PlanetTile, int>(planetTile2, num));
					num5 = Mathf.CeilToInt(CaravanArrivalTimeEstimator.GetCostToMove(nextTileCostLeft, firstInPath, curTicksAbs, num, caravanTicksPerMove, planetTile2, planetTile) / 1f);
				}
				if (num8 < num5)
				{
					num += num8;
					num5 -= num8;
					if (planetTile == to && num5 <= 10000 && Caravan_PathFollower.IsValidFinalPushDestination(to))
					{
						goto Block_15;
					}
					num += num3;
					num8 = num4;
				}
				else
				{
					num += num5;
					num8 -= num5;
					num5 = 0;
				}
			}
			Log.ErrorOnce("Could not calculate estimated ticks to arrive. Too many iterations.", 1837451324);
			outTicksToArrive.Add(new ValueTuple<PlanetTile, int>(to, num));
			return;
			Block_11:
			outTicksToArrive.Add(new ValueTuple<PlanetTile, int>(to, num));
			return;
			Block_15:
			num += num5;
			outTicksToArrive.Add(new ValueTuple<PlanetTile, int>(to, num));
		}

		// Token: 0x060155D2 RID: 87506 RVA: 0x00663614 File Offset: 0x00661814
		private static float GetCostToMove(float initialNextTileCostLeft, bool firstInPath, int initialTicksAbs, int curResult, int caravanTicksPerMove, PlanetTile curTile, PlanetTile nextTile)
		{
			if (firstInPath)
			{
				return initialNextTileCostLeft;
			}
			int value = initialTicksAbs + curResult;
			return (float)Caravan_PathFollower.CostToMove(caravanTicksPerMove, curTile, nextTile, new int?(value), false, null, null, false);
		}

		// Token: 0x060155D3 RID: 87507 RVA: 0x00663640 File Offset: 0x00661840
		public static int EstimatedTicksToArrive(PlanetTile destinationTile, [TupleElementNames(new string[]
		{
			"tile",
			"ticks"
		})] List<ValueTuple<PlanetTile, int>> estimatedTicksToArriveToEvery)
		{
			if (!destinationTile.Valid)
			{
				return 0;
			}
			for (int i = 0; i < estimatedTicksToArriveToEvery.Count; i++)
			{
				if (destinationTile == estimatedTicksToArriveToEvery[i].Item1)
				{
					return estimatedTicksToArriveToEvery[i].Item2;
				}
			}
			return 0;
		}

		// Token: 0x060155D4 RID: 87508 RVA: 0x0066368C File Offset: 0x0066188C
		public static PlanetTile TileIllBeInAt(int ticksAbs, [TupleElementNames(new string[]
		{
			"tile",
			"ticks"
		})] List<ValueTuple<PlanetTile, int>> estimatedTicksToArriveToEvery, int ticksAbsUsedToCalculateEstimatedTicksToArriveToEvery)
		{
			if (!estimatedTicksToArriveToEvery.Any<ValueTuple<PlanetTile, int>>())
			{
				return PlanetTile.Invalid;
			}
			for (int i = estimatedTicksToArriveToEvery.Count - 1; i >= 0; i--)
			{
				int num = ticksAbsUsedToCalculateEstimatedTicksToArriveToEvery + estimatedTicksToArriveToEvery[i].Item2;
				if (ticksAbs >= num)
				{
					return estimatedTicksToArriveToEvery[i].Item1;
				}
			}
			return estimatedTicksToArriveToEvery[0].Item1;
		}

		// Token: 0x0400EDE2 RID: 60898
		private static int cacheTicks = -1;

		// Token: 0x0400EDE3 RID: 60899
		private static Caravan cachedForCaravan;

		// Token: 0x0400EDE4 RID: 60900
		private static PlanetTile cachedForDest = PlanetTile.Invalid;

		// Token: 0x0400EDE5 RID: 60901
		private static int cachedResult = -1;

		// Token: 0x0400EDE6 RID: 60902
		private const int CacheDuration = 100;

		// Token: 0x0400EDE7 RID: 60903
		private const int MaxIterations = 10000;

		// Token: 0x0400EDE8 RID: 60904
		[TupleElementNames(new string[]
		{
			"tile",
			"ticks"
		})]
		private static readonly List<ValueTuple<PlanetTile, int>> tmpTicksToArrive = new List<ValueTuple<PlanetTile, int>>();
	}
}
