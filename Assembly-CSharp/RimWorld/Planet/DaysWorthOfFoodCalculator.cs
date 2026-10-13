using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D7A RID: 15738
	public static class DaysWorthOfFoodCalculator
	{
		// Token: 0x06015714 RID: 87828 RVA: 0x0066BD04 File Offset: 0x00669F04
		private static float ApproxDaysWorthOfFood(List<Pawn> pawns, List<ThingDefCount> extraFood, PlanetTile tile, IgnorePawnsInventoryMode ignoreInventory, Faction faction, WorldPath path = null, float nextTileCostLeft = 0f, int caravanTicksPerMove = 3300, bool assumeCaravanMoving = true)
		{
			if (!DaysWorthOfFoodCalculator.AnyFoodEatingPawn(pawns))
			{
				return 600f;
			}
			if (!assumeCaravanMoving)
			{
				path = null;
			}
			DaysWorthOfFoodCalculator.tmpFood.Clear();
			if (extraFood != null)
			{
				int i = 0;
				int count = extraFood.Count;
				while (i < count)
				{
					ThingDefCount item = extraFood[i];
					if (item.ThingDef.IsNutritionGivingIngestible && item.Count > 0)
					{
						DaysWorthOfFoodCalculator.tmpFood.Add(item);
					}
					i++;
				}
			}
			int j = 0;
			int count2 = pawns.Count;
			while (j < count2)
			{
				Pawn pawn4 = pawns[j];
				if (!InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn4, ignoreInventory))
				{
					ThingOwner<Thing> innerContainer = pawn4.inventory.innerContainer;
					int k = 0;
					int count3 = innerContainer.Count;
					while (k < count3)
					{
						Thing thing = innerContainer[k];
						if (thing.def.IsNutritionGivingIngestible)
						{
							DaysWorthOfFoodCalculator.tmpFood.Add(new ThingDefCount(thing.def, thing.stackCount));
						}
						k++;
					}
				}
				j++;
			}
			DaysWorthOfFoodCalculator.tmpFood2.Clear();
			DaysWorthOfFoodCalculator.tmpFood2.AddRange(DaysWorthOfFoodCalculator.tmpFood);
			DaysWorthOfFoodCalculator.tmpFood.Clear();
			int l = 0;
			int count4 = DaysWorthOfFoodCalculator.tmpFood2.Count;
			while (l < count4)
			{
				ThingDefCount item2 = DaysWorthOfFoodCalculator.tmpFood2[l];
				bool flag = false;
				int m = 0;
				int count5 = DaysWorthOfFoodCalculator.tmpFood.Count;
				while (m < count5)
				{
					ThingDefCount thingDefCount = DaysWorthOfFoodCalculator.tmpFood[m];
					if (thingDefCount.ThingDef == item2.ThingDef)
					{
						DaysWorthOfFoodCalculator.tmpFood[m] = thingDefCount.WithCount(thingDefCount.Count + item2.Count);
						flag = true;
						break;
					}
					m++;
				}
				if (!flag)
				{
					DaysWorthOfFoodCalculator.tmpFood.Add(item2);
				}
				l++;
			}
			DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn.Clear();
			int n = 0;
			int count6 = pawns.Count;
			while (n < count6)
			{
				DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn.Add(0f);
				n++;
			}
			int ticksAbs = Find.TickManager.TicksAbs;
			DaysWorthOfFoodCalculator.tmpTicksToArrive.Clear();
			if (path != null && path.Found)
			{
				CaravanArrivalTimeEstimator.EstimatedTicksToArriveToEvery(tile, path.LastNode, path, nextTileCostLeft, caravanTicksPerMove, ticksAbs, DaysWorthOfFoodCalculator.tmpTicksToArrive);
			}
			DaysWorthOfFoodCalculator.cachedNutritionBetweenHungryAndFed.Clear();
			DaysWorthOfFoodCalculator.cachedTicksUntilHungryWhenFed.Clear();
			DaysWorthOfFoodCalculator.cachedMaxFoodLevel.Clear();
			int num = 0;
			int count7 = pawns.Count;
			while (num < count7)
			{
				Pawn pawn2 = pawns[num];
				if (pawn2.RaceProps.EatsFood && pawn2.needs.food != null)
				{
					Need_Food food = pawn2.needs.food;
					DaysWorthOfFoodCalculator.cachedNutritionBetweenHungryAndFed.Add(food.NutritionBetweenHungryAndFed);
					DaysWorthOfFoodCalculator.cachedTicksUntilHungryWhenFed.Add(food.TicksUntilHungryWhenFedIgnoringMalnutrition);
					DaysWorthOfFoodCalculator.cachedMaxFoodLevel.Add(food.MaxLevel);
				}
				else
				{
					DaysWorthOfFoodCalculator.cachedNutritionBetweenHungryAndFed.Add(0f);
					DaysWorthOfFoodCalculator.cachedTicksUntilHungryWhenFed.Add(0);
					DaysWorthOfFoodCalculator.cachedMaxFoodLevel.Add(0f);
				}
				num++;
			}
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			bool flag2 = false;
			WorldGrid worldGrid = Find.WorldGrid;
			DaysWorthOfFoodCalculator.babiesWithFeeders.Clear();
			DaysWorthOfFoodCalculator.tmpLactatingPawns.Clear();
			DaysWorthOfFoodCalculator.tmpLactatingPawns.AddRange(pawns);
			DaysWorthOfFoodCalculator.tmpLactatingPawns.RemoveAll(delegate(Pawn pawn)
			{
				ChildcareUtility.BreastfeedFailReason? breastfeedFailReason2;
				return !ChildcareUtility.CanBreastfeed(pawn, out breastfeedFailReason2);
			});
			int count8 = DaysWorthOfFoodCalculator.tmpLactatingPawns.Count;
			for (int num5 = 0; num5 < count8; num5++)
			{
				for (int num6 = 0; num6 < 1; num6++)
				{
					DaysWorthOfFoodCalculator.tmpLactatingPawns.Add(DaysWorthOfFoodCalculator.tmpLactatingPawns[num5]);
				}
			}
			using (List<Pawn>.Enumerator enumerator = pawns.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Pawn pawn = enumerator.Current;
					ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
					if (ChildcareUtility.CanSuckle(pawn, out breastfeedFailReason))
					{
						int num7 = DaysWorthOfFoodCalculator.tmpLactatingPawns.FindIndex(delegate(Pawn feeder)
						{
							ChildcareUtility.BreastfeedFailReason? breastfeedFailReason2;
							return ChildcareUtility.CanMomBreastfeedBaby(feeder, pawn, out breastfeedFailReason2) && pawn.mindState.AutofeedSetting(feeder) > AutofeedMode.Never;
						});
						if (num7 >= 0)
						{
							DaysWorthOfFoodCalculator.tmpLactatingPawns[num7] = null;
							DaysWorthOfFoodCalculator.babiesWithFeeders.Add(pawn);
						}
					}
				}
			}
			bool flag3;
			do
			{
				flag3 = false;
				int num8 = ticksAbs + (int)(num3 * 60000f);
				PlanetTile tile2 = (path != null) ? CaravanArrivalTimeEstimator.TileIllBeInAt(num8, DaysWorthOfFoodCalculator.tmpTicksToArrive, ticksAbs) : tile;
				bool flag4 = CaravanNightRestUtility.WouldBeRestingAt(tile2, (long)num8);
				float progressPerTick = ForagedFoodPerDayCalculator.GetProgressPerTick(assumeCaravanMoving && !flag4, flag4, null);
				float num9 = 1f / progressPerTick;
				bool flag5 = VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsAt(tile2, num8);
				float num10 = num3 - num2;
				if (num10 > 0f)
				{
					num4 += num10 * 60000f;
					if (num4 >= num9)
					{
						BiomeDef primaryBiome = worldGrid[tile2].PrimaryBiome;
						int num11 = Mathf.RoundToInt(ForagedFoodPerDayCalculator.GetForagedFoodCountPerInterval(pawns, primaryBiome, faction, null));
						ThingDef foragedFood = primaryBiome.foragedFood;
						while (num4 >= num9)
						{
							num4 -= num9;
							if (num11 > 0)
							{
								bool flag6 = false;
								for (int num12 = DaysWorthOfFoodCalculator.tmpFood.Count - 1; num12 >= 0; num12--)
								{
									ThingDefCount thingDefCount2 = DaysWorthOfFoodCalculator.tmpFood[num12];
									if (thingDefCount2.ThingDef == foragedFood)
									{
										DaysWorthOfFoodCalculator.tmpFood[num12] = thingDefCount2.WithCount(thingDefCount2.Count + num11);
										flag6 = true;
										break;
									}
								}
								if (!flag6)
								{
									DaysWorthOfFoodCalculator.tmpFood.Add(new ThingDefCount(foragedFood, num11));
								}
							}
						}
					}
				}
				num2 = num3;
				int num13 = 0;
				int count9 = pawns.Count;
				while (num13 < count9)
				{
					Pawn pawn3 = pawns[num13];
					if (pawn3.RaceProps.EatsFood)
					{
						Pawn_NeedsTracker needs = pawn3.needs;
						if (((needs != null) ? needs.food : null) != null && !DaysWorthOfFoodCalculator.babiesWithFeeders.Contains(pawn3))
						{
							if (flag5 && VirtualPlantsUtility.CanEverEatVirtualPlants(pawn3))
							{
								if (DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn[num13] < num3)
								{
									DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn[num13] = num3;
								}
								else
								{
									List<float> list = DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn;
									int index = num13;
									list[index] += 0.45f;
								}
								flag3 = true;
							}
							else
							{
								float num14 = DaysWorthOfFoodCalculator.cachedNutritionBetweenHungryAndFed[num13];
								int num15 = DaysWorthOfFoodCalculator.cachedTicksUntilHungryWhenFed[num13];
								for (;;)
								{
									int num16 = DaysWorthOfFoodCalculator.BestEverEdibleFoodIndexFor(pawn3, DaysWorthOfFoodCalculator.tmpFood);
									if (num16 < 0)
									{
										break;
									}
									ThingDefCount thingDefCount3 = DaysWorthOfFoodCalculator.tmpFood[num16];
									float num17 = Mathf.Min(thingDefCount3.ThingDef.ingestible.CachedNutrition, num14);
									float num18 = num17 / num14 * (float)num15 / 60000f;
									int num19 = Mathf.Min(Mathf.CeilToInt(Mathf.Min(0.2f, DaysWorthOfFoodCalculator.cachedMaxFoodLevel[num13]) / num17), thingDefCount3.Count);
									List<float> list = DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn;
									int index = num13;
									list[index] += num18 * (float)num19;
									DaysWorthOfFoodCalculator.tmpFood[num16] = thingDefCount3.WithCount(thingDefCount3.Count - num19);
									flag3 = true;
									if (DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn[num13] >= num3)
									{
										goto IL_702;
									}
								}
								if (DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn[num13] < num3)
								{
									flag2 = true;
								}
							}
							IL_702:
							if (flag2)
							{
								break;
							}
							num3 = Mathf.Max(num3, DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn[num13]);
						}
					}
					num13++;
				}
			}
			while (flag3 && !flag2 && num3 <= 601f);
			float num20 = 600f;
			int num21 = 0;
			int count10 = pawns.Count;
			while (num21 < count10)
			{
				if (pawns[num21].RaceProps.EatsFood)
				{
					Pawn_NeedsTracker needs2 = pawns[num21].needs;
					if (((needs2 != null) ? needs2.food : null) != null && !DaysWorthOfFoodCalculator.babiesWithFeeders.Contains(pawns[num21]))
					{
						num20 = Mathf.Min(num20, DaysWorthOfFoodCalculator.tmpDaysWorthOfFoodForPawn[num21]);
					}
				}
				num21++;
			}
			return num20;
		}

		// Token: 0x06015715 RID: 87829 RVA: 0x0066C4D8 File Offset: 0x0066A6D8
		public static float ApproxDaysWorthOfFood(Caravan caravan)
		{
			return DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(caravan.PawnsListForReading, null, caravan.Tile, IgnorePawnsInventoryMode.DontIgnore, caravan.Faction, caravan.pather.curPath, caravan.pather.nextTileCostLeft, caravan.TicksPerMove, caravan.pather.Moving && !caravan.pather.Paused);
		}

		// Token: 0x06015716 RID: 87830 RVA: 0x0066C538 File Offset: 0x0066A738
		public static float ApproxDaysWorthOfFood(List<TransferableOneWay> transferables, PlanetTile tile, IgnorePawnsInventoryMode ignoreInventory, Faction faction, WorldPath path = null, float nextTileCostLeft = 0f, int caravanTicksPerMove = 3300)
		{
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			DaysWorthOfFoodCalculator.tmpPawns.Clear();
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
							if (pawn.RaceProps.EatsFood)
							{
								Pawn_NeedsTracker needs = pawn.needs;
								if (((needs != null) ? needs.food : null) != null)
								{
									DaysWorthOfFoodCalculator.tmpPawns.Add(pawn);
								}
							}
						}
					}
					else
					{
						Corpse corpse = transferableOneWay.AnyThing as Corpse;
						if (corpse == null || corpse.GetRotStage() == RotStage.Fresh)
						{
							DaysWorthOfFoodCalculator.tmpThingDefCounts.Add(new ThingDefCount(transferableOneWay.ThingDef, transferableOneWay.CountToTransfer));
						}
					}
				}
			}
			float result = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(DaysWorthOfFoodCalculator.tmpPawns, DaysWorthOfFoodCalculator.tmpThingDefCounts, tile, ignoreInventory, faction, path, nextTileCostLeft, caravanTicksPerMove, true);
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			DaysWorthOfFoodCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x06015717 RID: 87831 RVA: 0x0066C644 File Offset: 0x0066A844
		public static float ApproxDaysWorthOfFoodLeftAfterTransfer(List<TransferableOneWay> transferables, PlanetTile tile, IgnorePawnsInventoryMode ignoreInventory, Faction faction, WorldPath path = null, float nextTileCostLeft = 0f, int caravanTicksPerMove = 3300)
		{
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			DaysWorthOfFoodCalculator.tmpPawns.Clear();
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
							if (pawn.RaceProps.EatsFood)
							{
								Pawn_NeedsTracker needs = pawn.needs;
								if (((needs != null) ? needs.food : null) != null)
								{
									DaysWorthOfFoodCalculator.tmpPawns.Add(pawn);
								}
							}
						}
					}
					else
					{
						DaysWorthOfFoodCalculator.tmpThingDefCounts.Add(new ThingDefCount(transferableOneWay.ThingDef, transferableOneWay.MaxCount - transferableOneWay.CountToTransfer));
					}
				}
			}
			float result = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(DaysWorthOfFoodCalculator.tmpPawns, DaysWorthOfFoodCalculator.tmpThingDefCounts, tile, ignoreInventory, faction, path, nextTileCostLeft, caravanTicksPerMove, true);
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			DaysWorthOfFoodCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x06015718 RID: 87832 RVA: 0x0066C748 File Offset: 0x0066A948
		public static float ApproxDaysWorthOfFood(List<Pawn> pawns, List<Thing> potentiallyFood, PlanetTile tile, IgnorePawnsInventoryMode ignoreInventory, Faction faction)
		{
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			DaysWorthOfFoodCalculator.tmpPawns.Clear();
			for (int i = 0; i < pawns.Count; i++)
			{
				Pawn pawn = pawns[i];
				if (pawn.RaceProps.EatsFood)
				{
					Pawn_NeedsTracker needs = pawn.needs;
					if (((needs != null) ? needs.food : null) != null)
					{
						DaysWorthOfFoodCalculator.tmpPawns.Add(pawn);
					}
				}
			}
			for (int j = 0; j < potentiallyFood.Count; j++)
			{
				DaysWorthOfFoodCalculator.tmpThingDefCounts.Add(new ThingDefCount(potentiallyFood[j].def, potentiallyFood[j].stackCount));
			}
			float result = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(DaysWorthOfFoodCalculator.tmpPawns, DaysWorthOfFoodCalculator.tmpThingDefCounts, tile, ignoreInventory, faction, null, 0f, 3300, true);
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			DaysWorthOfFoodCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x06015719 RID: 87833 RVA: 0x0066C81C File Offset: 0x0066AA1C
		public static float ApproxDaysWorthOfFood(List<Pawn> pawns, List<ThingCount> potentiallyFood, PlanetTile tile, IgnorePawnsInventoryMode ignoreInventory, Faction faction)
		{
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			for (int i = 0; i < potentiallyFood.Count; i++)
			{
				if (potentiallyFood[i].Count > 0)
				{
					DaysWorthOfFoodCalculator.tmpThingDefCounts.Add(new ThingDefCount(potentiallyFood[i].Thing.def, potentiallyFood[i].Count));
				}
			}
			float result = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(pawns, DaysWorthOfFoodCalculator.tmpThingDefCounts, tile, ignoreInventory, faction, null, 0f, 3300, true);
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			return result;
		}

		// Token: 0x0601571A RID: 87834 RVA: 0x0066C8B0 File Offset: 0x0066AAB0
		public static float ApproxDaysWorthOfFoodLeftAfterTradeableTransfer(List<Thing> allCurrentThings, List<Tradeable> tradeables, PlanetTile tile, IgnorePawnsInventoryMode ignoreInventory, Faction faction)
		{
			DaysWorthOfFoodCalculator.tmpThingCounts.Clear();
			TransferableUtility.SimulateTradeableTransfer(allCurrentThings, tradeables, DaysWorthOfFoodCalculator.tmpThingCounts);
			DaysWorthOfFoodCalculator.tmpPawns.Clear();
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			for (int i = DaysWorthOfFoodCalculator.tmpThingCounts.Count - 1; i >= 0; i--)
			{
				if (DaysWorthOfFoodCalculator.tmpThingCounts[i].Count > 0)
				{
					Pawn pawn = DaysWorthOfFoodCalculator.tmpThingCounts[i].Thing as Pawn;
					if (pawn != null)
					{
						if (pawn.RaceProps.EatsFood)
						{
							Pawn_NeedsTracker needs = pawn.needs;
							if (((needs != null) ? needs.food : null) != null)
							{
								DaysWorthOfFoodCalculator.tmpPawns.Add(pawn);
							}
						}
					}
					else
					{
						DaysWorthOfFoodCalculator.tmpThingDefCounts.Add(new ThingDefCount(DaysWorthOfFoodCalculator.tmpThingCounts[i].Thing.def, DaysWorthOfFoodCalculator.tmpThingCounts[i].Count));
					}
				}
			}
			DaysWorthOfFoodCalculator.tmpThingCounts.Clear();
			float result = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(DaysWorthOfFoodCalculator.tmpPawns, DaysWorthOfFoodCalculator.tmpThingDefCounts, tile, ignoreInventory, faction, null, 0f, 3300, true);
			DaysWorthOfFoodCalculator.tmpPawns.Clear();
			DaysWorthOfFoodCalculator.tmpThingDefCounts.Clear();
			return result;
		}

		// Token: 0x0601571B RID: 87835 RVA: 0x0066C9E0 File Offset: 0x0066ABE0
		public static bool AnyFoodEatingPawn(List<Pawn> pawns)
		{
			int i = 0;
			int count = pawns.Count;
			while (i < count)
			{
				if (pawns[i].RaceProps.EatsFood)
				{
					Pawn_NeedsTracker needs = pawns[i].needs;
					if (((needs != null) ? needs.food : null) != null)
					{
						return true;
					}
				}
				i++;
			}
			return false;
		}

		// Token: 0x0601571C RID: 87836 RVA: 0x0066CA30 File Offset: 0x0066AC30
		private static int BestEverEdibleFoodIndexFor(Pawn pawn, List<ThingDefCount> food)
		{
			int num = -1;
			float num2 = 0f;
			int i = 0;
			int count = food.Count;
			while (i < count)
			{
				if (food[i].Count > 0)
				{
					ThingDef thingDef = food[i].ThingDef;
					if (CaravanPawnsNeedsUtility.CanEatForNutritionEver(thingDef, pawn))
					{
						float foodScore = CaravanPawnsNeedsUtility.GetFoodScore(thingDef, pawn, thingDef.ingestible.CachedNutrition);
						if (num < 0 || foodScore > num2)
						{
							num = i;
							num2 = foodScore;
						}
					}
				}
				i++;
			}
			return num;
		}

		// Token: 0x0400EEB6 RID: 61110
		private static readonly List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x0400EEB7 RID: 61111
		private static readonly List<ThingDefCount> tmpThingDefCounts = new List<ThingDefCount>();

		// Token: 0x0400EEB8 RID: 61112
		private static readonly List<ThingCount> tmpThingCounts = new List<ThingCount>();

		// Token: 0x0400EEB9 RID: 61113
		public const float InfiniteDaysWorthOfFood = 600f;

		// Token: 0x0400EEBA RID: 61114
		private static readonly List<float> tmpDaysWorthOfFoodForPawn = new List<float>();

		// Token: 0x0400EEBB RID: 61115
		private static readonly List<ThingDefCount> tmpFood = new List<ThingDefCount>();

		// Token: 0x0400EEBC RID: 61116
		private static readonly List<ThingDefCount> tmpFood2 = new List<ThingDefCount>();

		// Token: 0x0400EEBD RID: 61117
		private static readonly List<ValueTuple<PlanetTile, int>> tmpTicksToArrive = new List<ValueTuple<PlanetTile, int>>();

		// Token: 0x0400EEBE RID: 61118
		private static readonly List<float> cachedNutritionBetweenHungryAndFed = new List<float>();

		// Token: 0x0400EEBF RID: 61119
		private static readonly List<int> cachedTicksUntilHungryWhenFed = new List<int>();

		// Token: 0x0400EEC0 RID: 61120
		private static readonly List<float> cachedMaxFoodLevel = new List<float>();

		// Token: 0x0400EEC1 RID: 61121
		private static readonly HashSet<Pawn> babiesWithFeeders = new HashSet<Pawn>();

		// Token: 0x0400EEC2 RID: 61122
		private static readonly List<Pawn> tmpLactatingPawns = new List<Pawn>(16);
	}
}
