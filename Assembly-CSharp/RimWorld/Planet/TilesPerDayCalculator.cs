using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D8D RID: 15757
	public static class TilesPerDayCalculator
	{
		// Token: 0x0601575C RID: 87900 RVA: 0x0066E2D0 File Offset: 0x0066C4D0
		public static float ApproxTilesPerDay(int caravanTicksPerMove, PlanetTile tile, PlanetTile nextTile, StringBuilder explanation = null, string caravanTicksPerMoveExplanation = null, bool immobile = false)
		{
			if (!nextTile.Valid)
			{
				nextTile = Find.WorldGrid.FindMostReasonableAdjacentTileForDisplayedPathCost(tile);
			}
			int num = Mathf.CeilToInt((float)Caravan_PathFollower.CostToMove(caravanTicksPerMove, tile, nextTile, null, false, explanation, caravanTicksPerMoveExplanation, immobile) / 1f);
			if (num == 0)
			{
				return 0f;
			}
			return 60000f / (float)num;
		}

		// Token: 0x0601575D RID: 87901 RVA: 0x0066E330 File Offset: 0x0066C530
		public static float ApproxTilesPerDay(Caravan caravan, StringBuilder explanation = null)
		{
			return TilesPerDayCalculator.ApproxTilesPerDay(caravan.TicksPerMove, caravan.Tile, caravan.pather.Moving ? caravan.pather.nextTile : PlanetTile.Invalid, explanation, (explanation != null) ? caravan.TicksPerMoveExplanation : null, caravan.ImmobilizedByMass || caravan.Shuttle != null);
		}

		// Token: 0x0601575E RID: 87902 RVA: 0x0066E390 File Offset: 0x0066C590
		public static float ApproxTilesPerDay(List<TransferableOneWay> transferables, float massUsage, float massCapacity, PlanetTile tile, PlanetTile nextTile, bool isShuttle, StringBuilder explanation = null)
		{
			TilesPerDayCalculator.tmpPawns.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn)
				{
					for (int j = 0; j < transferableOneWay.CountToTransfer; j++)
					{
						TilesPerDayCalculator.tmpPawns.Add((Pawn)transferableOneWay.things[j]);
					}
				}
			}
			if (!TilesPerDayCalculator.tmpPawns.Any<Pawn>())
			{
				return 0f;
			}
			StringBuilder stringBuilder = (explanation != null) ? new StringBuilder() : null;
			float result = TilesPerDayCalculator.ApproxTilesPerDay(CaravanTicksPerMoveUtility.GetTicksPerMove(TilesPerDayCalculator.tmpPawns, massUsage, massCapacity, isShuttle, stringBuilder), tile, nextTile, explanation, (stringBuilder != null) ? stringBuilder.ToString() : null, massUsage > massCapacity);
			TilesPerDayCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x0601575F RID: 87903 RVA: 0x0066E454 File Offset: 0x0066C654
		public static float ApproxTilesPerDayLeftAfterTransfer(List<TransferableOneWay> transferables, float massUsageLeftAfterTransfer, float massCapacityLeftAfterTransfer, PlanetTile tile, PlanetTile nextTile, bool isShuttle, StringBuilder explanation = null)
		{
			TilesPerDayCalculator.tmpPawns.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn)
				{
					for (int j = transferableOneWay.things.Count - 1; j >= transferableOneWay.CountToTransfer; j--)
					{
						TilesPerDayCalculator.tmpPawns.Add((Pawn)transferableOneWay.things[j]);
					}
				}
			}
			if (!TilesPerDayCalculator.tmpPawns.Any<Pawn>())
			{
				return 0f;
			}
			StringBuilder stringBuilder = (explanation != null) ? new StringBuilder() : null;
			float result = TilesPerDayCalculator.ApproxTilesPerDay(CaravanTicksPerMoveUtility.GetTicksPerMove(TilesPerDayCalculator.tmpPawns, massUsageLeftAfterTransfer, massCapacityLeftAfterTransfer, isShuttle, stringBuilder), tile, nextTile, explanation, (stringBuilder != null) ? stringBuilder.ToString() : null, massUsageLeftAfterTransfer > massCapacityLeftAfterTransfer);
			TilesPerDayCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x06015760 RID: 87904 RVA: 0x0066E523 File Offset: 0x0066C723
		public static float ApproxTilesPerDayLeftAfterTradeableTransfer(List<Thing> allCurrentThings, List<Tradeable> tradeables, float massUsageLeftAfterTradeableTransfer, float massCapacityLeftAfterTradeableTransfer, PlanetTile tile, PlanetTile nextTile, bool isShuttle, StringBuilder explanation = null)
		{
			TilesPerDayCalculator.tmpThingCounts.Clear();
			TransferableUtility.SimulateTradeableTransfer(allCurrentThings, tradeables, TilesPerDayCalculator.tmpThingCounts);
			float result = TilesPerDayCalculator.ApproxTilesPerDay(TilesPerDayCalculator.tmpThingCounts, massUsageLeftAfterTradeableTransfer, massCapacityLeftAfterTradeableTransfer, tile, nextTile, isShuttle, explanation);
			TilesPerDayCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06015761 RID: 87905 RVA: 0x0066E55C File Offset: 0x0066C75C
		public static float ApproxTilesPerDay(List<ThingCount> thingCounts, float massUsage, float massCapacity, PlanetTile tile, PlanetTile nextTile, bool isShuttle, StringBuilder explanation = null)
		{
			TilesPerDayCalculator.tmpPawns.Clear();
			for (int i = 0; i < thingCounts.Count; i++)
			{
				if (thingCounts[i].Count > 0)
				{
					Pawn pawn = thingCounts[i].Thing as Pawn;
					if (pawn != null)
					{
						TilesPerDayCalculator.tmpPawns.Add(pawn);
					}
				}
			}
			if (!TilesPerDayCalculator.tmpPawns.Any<Pawn>())
			{
				return 0f;
			}
			StringBuilder stringBuilder = (explanation != null) ? new StringBuilder() : null;
			float result = TilesPerDayCalculator.ApproxTilesPerDay(CaravanTicksPerMoveUtility.GetTicksPerMove(TilesPerDayCalculator.tmpPawns, massUsage, massCapacity, isShuttle, stringBuilder), tile, nextTile, explanation, (stringBuilder != null) ? stringBuilder.ToString() : null, massUsage > massCapacity);
			TilesPerDayCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x0400EEE8 RID: 61160
		private static readonly List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x0400EEE9 RID: 61161
		private static readonly List<ThingCount> tmpThingCounts = new List<ThingCount>();
	}
}
