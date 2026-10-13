using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D78 RID: 15736
	public static class DaysUntilRotCalculator
	{
		// Token: 0x06015708 RID: 87816 RVA: 0x0066B6B0 File Offset: 0x006698B0
		public static float ApproxDaysUntilRot(List<ThingCount> potentiallyFood, PlanetTile tile, WorldPath path = null, float nextTileCostLeft = 0f, int caravanTicksPerMove = 3300)
		{
			DaysUntilRotCalculator.TmpTicksToArrive.Clear();
			if (path != null && path.Found)
			{
				CaravanArrivalTimeEstimator.EstimatedTicksToArriveToEvery(tile, path.LastNode, path, nextTileCostLeft, caravanTicksPerMove, Find.TickManager.TicksAbs, DaysUntilRotCalculator.TmpTicksToArrive);
			}
			DaysUntilRotCalculator.TmpNutritions.Clear();
			for (int i = 0; i < potentiallyFood.Count; i++)
			{
				ThingCount thingCount = potentiallyFood[i];
				if (thingCount.Count > 0 && thingCount.Thing.def.IsNutritionGivingIngestible)
				{
					CompRottable compRottable = thingCount.Thing.TryGetComp<CompRottable>();
					float first;
					if (compRottable != null && compRottable.Active)
					{
						first = (float)DaysUntilRotCalculator.ApproxTicksUntilRot_AssumeTimePassesBy(compRottable, tile, DaysUntilRotCalculator.TmpTicksToArrive) / 60000f;
					}
					else
					{
						first = 600f;
					}
					float second = thingCount.Thing.GetStatValue(StatDefOf.Nutrition, true, -1) * (float)thingCount.Count;
					DaysUntilRotCalculator.TmpNutritions.Add(new Pair<float, float>(first, second));
				}
			}
			return GenMath.WeightedMedian(DaysUntilRotCalculator.TmpNutritions, 600f, 0.5f);
		}

		// Token: 0x06015709 RID: 87817 RVA: 0x0066B7B0 File Offset: 0x006699B0
		public static int ApproxTicksUntilRot_AssumeTimePassesBy(CompRottable rot, PlanetTile tile, [TupleElementNames(new string[]
		{
			"tile",
			"ticks"
		})] List<ValueTuple<PlanetTile, int>> ticksToArrive = null)
		{
			float num = 0f;
			int num2 = Find.TickManager.TicksAbs;
			while (num < 1f && (float)num2 < (float)Find.TickManager.TicksAbs + 36060000f)
			{
				PlanetTile tile2 = ticksToArrive.NullOrEmpty<ValueTuple<PlanetTile, int>>() ? tile : CaravanArrivalTimeEstimator.TileIllBeInAt(num2, ticksToArrive, Find.TickManager.TicksAbs);
				int num3 = Mathf.FloorToInt((float)rot.ApproxTicksUntilRotWhenAtTempOfTile(tile2, num2) * (1f - num));
				if (num3 <= 0)
				{
					break;
				}
				int b;
				if (num3 >= 10800000)
				{
					b = 306000;
				}
				else if (num3 >= 3600000)
				{
					b = 125999;
				}
				else if (num3 >= 600000)
				{
					b = 66000;
				}
				else
				{
					b = 27000;
				}
				int num4 = Mathf.Min(num3, b);
				num += (float)num4 / (float)num3;
				num2 += num4;
			}
			return num2 - Find.TickManager.TicksAbs;
		}

		// Token: 0x0601570A RID: 87818 RVA: 0x0066B889 File Offset: 0x00669A89
		public static float ApproxDaysUntilRot(Caravan caravan)
		{
			return DaysUntilRotCalculator.ApproxDaysUntilRot(CaravanInventoryUtility.AllInventoryItems(caravan), caravan.Tile, caravan.pather.curPath, caravan.pather.nextTileCostLeft, caravan.TicksPerMove);
		}

		// Token: 0x0601570B RID: 87819 RVA: 0x0066B8B8 File Offset: 0x00669AB8
		public static float ApproxDaysUntilRot(List<Thing> potentiallyFood, PlanetTile tile, WorldPath path = null, float nextTileCostLeft = 0f, int caravanTicksPerMove = 3300)
		{
			DaysUntilRotCalculator.TmpThingCounts.Clear();
			for (int i = 0; i < potentiallyFood.Count; i++)
			{
				DaysUntilRotCalculator.TmpThingCounts.Add(new ThingCount(potentiallyFood[i], potentiallyFood[i].stackCount, false));
			}
			float result = DaysUntilRotCalculator.ApproxDaysUntilRot(DaysUntilRotCalculator.TmpThingCounts, tile, path, nextTileCostLeft, caravanTicksPerMove);
			DaysUntilRotCalculator.TmpThingCounts.Clear();
			return result;
		}

		// Token: 0x0601570C RID: 87820 RVA: 0x0066B91C File Offset: 0x00669B1C
		public static float ApproxDaysUntilRot(List<TransferableOneWay> transferables, PlanetTile tile, IgnorePawnsInventoryMode ignoreInventory, WorldPath path = null, float nextTileCostLeft = 0f, int caravanTicksPerMove = 3300)
		{
			DaysUntilRotCalculator.TmpThingCounts.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing)
				{
					if (transferableOneWay.AnyThing is Pawn)
					{
						for (int j = 0; j < transferableOneWay.CountToTransfer; j++)
						{
							Pawn pawn = (Pawn)transferableOneWay.things[j];
							if (!InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, ignoreInventory))
							{
								ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
								for (int k = 0; k < innerContainer.Count; k++)
								{
									DaysUntilRotCalculator.TmpThingCounts.Add(new ThingCount(innerContainer[k], innerContainer[k].stackCount, false));
								}
							}
						}
					}
					else if (transferableOneWay.CountToTransfer > 0)
					{
						TransferableUtility.TransferNoSplit(transferableOneWay.things, transferableOneWay.CountToTransfer, delegate(Thing thing, int count)
						{
							DaysUntilRotCalculator.TmpThingCounts.Add(new ThingCount(thing, count, false));
						}, false, false);
					}
				}
			}
			float result = DaysUntilRotCalculator.ApproxDaysUntilRot(DaysUntilRotCalculator.TmpThingCounts, tile, path, nextTileCostLeft, caravanTicksPerMove);
			DaysUntilRotCalculator.TmpThingCounts.Clear();
			return result;
		}

		// Token: 0x0601570D RID: 87821 RVA: 0x0066BA38 File Offset: 0x00669C38
		public static float ApproxDaysUntilRotLeftAfterTransfer(List<TransferableOneWay> transferables, PlanetTile tile, IgnorePawnsInventoryMode ignoreInventory, WorldPath path = null, float nextTileCostLeft = 0f, int caravanTicksPerMove = 3300)
		{
			DaysUntilRotCalculator.TmpThingCounts.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing)
				{
					if (transferableOneWay.AnyThing is Pawn)
					{
						for (int j = transferableOneWay.things.Count - 1; j >= transferableOneWay.CountToTransfer; j--)
						{
							Pawn pawn = (Pawn)transferableOneWay.things[j];
							if (!InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, ignoreInventory))
							{
								ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
								for (int k = 0; k < innerContainer.Count; k++)
								{
									DaysUntilRotCalculator.TmpThingCounts.Add(new ThingCount(innerContainer[k], innerContainer[k].stackCount, false));
								}
							}
						}
					}
					else if (transferableOneWay.MaxCount - transferableOneWay.CountToTransfer > 0)
					{
						DaysUntilRotCalculator.ThingsInReverse.Clear();
						DaysUntilRotCalculator.ThingsInReverse.AddRange(transferableOneWay.things);
						DaysUntilRotCalculator.ThingsInReverse.Reverse();
						TransferableUtility.TransferNoSplit(DaysUntilRotCalculator.ThingsInReverse, transferableOneWay.MaxCount - transferableOneWay.CountToTransfer, delegate(Thing thing, int count)
						{
							DaysUntilRotCalculator.TmpThingCounts.Add(new ThingCount(thing, count, false));
						}, false, false);
					}
				}
			}
			DaysUntilRotCalculator.ThingsInReverse.Clear();
			float result = DaysUntilRotCalculator.ApproxDaysUntilRot(DaysUntilRotCalculator.TmpThingCounts, tile, path, nextTileCostLeft, caravanTicksPerMove);
			DaysUntilRotCalculator.TmpThingCounts.Clear();
			return result;
		}

		// Token: 0x0601570E RID: 87822 RVA: 0x0066BBA0 File Offset: 0x00669DA0
		public static float ApproxDaysUntilRotLeftAfterTradeableTransfer(List<Thing> allCurrentThings, List<Tradeable> tradeables, PlanetTile tile, IgnorePawnsInventoryMode ignoreInventory)
		{
			DaysUntilRotCalculator.TmpThingCountsFromTradeables.Clear();
			TransferableUtility.SimulateTradeableTransfer(allCurrentThings, tradeables, DaysUntilRotCalculator.TmpThingCountsFromTradeables);
			DaysUntilRotCalculator.TmpThingCounts.Clear();
			for (int i = DaysUntilRotCalculator.TmpThingCountsFromTradeables.Count - 1; i >= 0; i--)
			{
				if (DaysUntilRotCalculator.TmpThingCountsFromTradeables[i].Count > 0)
				{
					Pawn pawn = DaysUntilRotCalculator.TmpThingCountsFromTradeables[i].Thing as Pawn;
					if (pawn != null)
					{
						if (!InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, ignoreInventory))
						{
							ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
							for (int j = 0; j < innerContainer.Count; j++)
							{
								DaysUntilRotCalculator.TmpThingCounts.Add(new ThingCount(innerContainer[j], innerContainer[j].stackCount, false));
							}
						}
					}
					else
					{
						DaysUntilRotCalculator.TmpThingCounts.Add(DaysUntilRotCalculator.TmpThingCountsFromTradeables[i]);
					}
				}
			}
			DaysUntilRotCalculator.TmpThingCountsFromTradeables.Clear();
			float result = DaysUntilRotCalculator.ApproxDaysUntilRot(DaysUntilRotCalculator.TmpThingCounts, tile, null, 0f, 3300);
			DaysUntilRotCalculator.TmpThingCounts.Clear();
			return result;
		}

		// Token: 0x0400EEAD RID: 61101
		private static readonly List<ThingCount> TmpThingCounts = new List<ThingCount>();

		// Token: 0x0400EEAE RID: 61102
		private static readonly List<ThingCount> TmpThingCountsFromTradeables = new List<ThingCount>();

		// Token: 0x0400EEAF RID: 61103
		private static readonly List<Pair<float, float>> TmpNutritions = new List<Pair<float, float>>();

		// Token: 0x0400EEB0 RID: 61104
		private static readonly List<Thing> ThingsInReverse = new List<Thing>();

		// Token: 0x0400EEB1 RID: 61105
		[TupleElementNames(new string[]
		{
			"tile",
			"ticks"
		})]
		private static readonly List<ValueTuple<PlanetTile, int>> TmpTicksToArrive = new List<ValueTuple<PlanetTile, int>>();

		// Token: 0x0400EEB2 RID: 61106
		public const float InfiniteDaysUntilRot = 600f;
	}
}
