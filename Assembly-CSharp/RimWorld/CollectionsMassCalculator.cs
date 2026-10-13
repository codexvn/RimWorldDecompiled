using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003561 RID: 13665
	public static class CollectionsMassCalculator
	{
		// Token: 0x06013290 RID: 78480 RVA: 0x005B1FE0 File Offset: 0x005B01E0
		public static float Capacity(List<ThingCount> thingCounts, StringBuilder explanation = null)
		{
			float num = 0f;
			for (int i = 0; i < thingCounts.Count; i++)
			{
				if (thingCounts[i].Count > 0)
				{
					Pawn pawn = thingCounts[i].Thing as Pawn;
					if (pawn != null)
					{
						num += MassUtility.Capacity(pawn, explanation) * (float)thingCounts[i].Count;
					}
				}
			}
			return Mathf.Max(num, 0f);
		}

		// Token: 0x06013291 RID: 78481 RVA: 0x005B2054 File Offset: 0x005B0254
		public static float MassUsage(List<ThingCount> thingCounts, IgnorePawnsInventoryMode ignoreInventory, bool includePawnsMass = false, bool ignoreSpawnedCorpsesGearAndInventory = false)
		{
			float num = 0f;
			for (int i = 0; i < thingCounts.Count; i++)
			{
				int count = thingCounts[i].Count;
				if (count > 0)
				{
					Thing thing = thingCounts[i].Thing;
					Pawn pawn = thing as Pawn;
					if (pawn != null)
					{
						if (includePawnsMass)
						{
							num += pawn.GetStatValue(StatDefOf.Mass, true, -1) * (float)count;
						}
						else
						{
							num += MassUtility.GearAndInventoryMass(pawn) * (float)count;
						}
						if (InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, ignoreInventory))
						{
							num -= MassUtility.InventoryMass(pawn) * (float)count;
						}
					}
					else
					{
						num += thing.GetStatValue(StatDefOf.Mass, true, -1) * (float)count;
						if (ignoreSpawnedCorpsesGearAndInventory)
						{
							Corpse corpse = thing as Corpse;
							if (corpse != null && corpse.Spawned)
							{
								num -= MassUtility.GearAndInventoryMass(corpse.InnerPawn) * (float)count;
							}
						}
					}
				}
			}
			return Mathf.Max(num, 0f);
		}

		// Token: 0x06013292 RID: 78482 RVA: 0x005B213C File Offset: 0x005B033C
		public static float MassUsage(ThingOwner container, IgnorePawnsInventoryMode ignoreInventory, bool includePawnsMass = false, bool ignoreSpawnedCorpsesGearAndInventory = false)
		{
			float num = 0f;
			foreach (Thing thing in ((IEnumerable<Thing>)container))
			{
				int stackCount = thing.stackCount;
				if (stackCount > 0)
				{
					Pawn pawn = thing as Pawn;
					if (pawn != null)
					{
						if (includePawnsMass)
						{
							num += pawn.GetStatValue(StatDefOf.Mass, true, -1) * (float)stackCount;
						}
						else
						{
							num += MassUtility.GearAndInventoryMass(pawn) * (float)stackCount;
						}
						if (InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, ignoreInventory))
						{
							num -= MassUtility.InventoryMass(pawn) * (float)stackCount;
						}
					}
					else
					{
						num += thing.GetStatValue(StatDefOf.Mass, true, -1) * (float)stackCount;
						if (ignoreSpawnedCorpsesGearAndInventory)
						{
							Corpse corpse = thing as Corpse;
							if (corpse != null && corpse.Spawned)
							{
								num -= MassUtility.GearAndInventoryMass(corpse.InnerPawn) * (float)stackCount;
							}
						}
					}
				}
			}
			return Mathf.Max(num, 0f);
		}

		// Token: 0x06013293 RID: 78483 RVA: 0x005B222C File Offset: 0x005B042C
		public static float CapacityTransferables(List<TransferableOneWay> transferables, StringBuilder explanation = null)
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				if (transferables[i].HasAnyThing && transferables[i].AnyThing is Pawn)
				{
					TransferableUtility.TransferNoSplit(transferables[i].things, transferables[i].CountToTransfer, delegate(Thing originalThing, int toTake)
					{
						CollectionsMassCalculator.tmpThingCounts.Add(new ThingCount(originalThing, toTake, false));
					}, false, false);
				}
			}
			float result = CollectionsMassCalculator.Capacity(CollectionsMassCalculator.tmpThingCounts, explanation);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06013294 RID: 78484 RVA: 0x005B22C8 File Offset: 0x005B04C8
		public static float CapacityLeftAfterTransfer(List<TransferableOneWay> transferables, StringBuilder explanation = null)
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				if (transferables[i].HasAnyThing && transferables[i].AnyThing is Pawn)
				{
					CollectionsMassCalculator.thingsInReverse.Clear();
					CollectionsMassCalculator.thingsInReverse.AddRange(transferables[i].things);
					CollectionsMassCalculator.thingsInReverse.Reverse();
					TransferableUtility.TransferNoSplit(CollectionsMassCalculator.thingsInReverse, transferables[i].MaxCount - transferables[i].CountToTransfer, delegate(Thing originalThing, int toTake)
					{
						CollectionsMassCalculator.tmpThingCounts.Add(new ThingCount(originalThing, toTake, false));
					}, false, false);
				}
			}
			CollectionsMassCalculator.thingsInReverse.Clear();
			float result = CollectionsMassCalculator.Capacity(CollectionsMassCalculator.tmpThingCounts, explanation);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06013295 RID: 78485 RVA: 0x005B23A7 File Offset: 0x005B05A7
		public static float CapacityLeftAfterTradeableTransfer(List<Thing> allCurrentThings, List<Tradeable> tradeables, StringBuilder explanation = null)
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			TransferableUtility.SimulateTradeableTransfer(allCurrentThings, tradeables, CollectionsMassCalculator.tmpThingCounts);
			float result = CollectionsMassCalculator.Capacity(CollectionsMassCalculator.tmpThingCounts, explanation);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06013296 RID: 78486 RVA: 0x005B23D4 File Offset: 0x005B05D4
		public static float Capacity<T>(List<T> things, StringBuilder explanation = null) where T : Thing
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			for (int i = 0; i < things.Count; i++)
			{
				CollectionsMassCalculator.tmpThingCounts.Add(new ThingCount(things[i], things[i].stackCount, false));
			}
			float result = CollectionsMassCalculator.Capacity(CollectionsMassCalculator.tmpThingCounts, explanation);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06013297 RID: 78487 RVA: 0x005B2440 File Offset: 0x005B0640
		public static float MassUsageTransferables(List<TransferableOneWay> transferables, IgnorePawnsInventoryMode ignoreInventory, bool includePawnsMass = false, bool ignoreSpawnedCorpsesGearAndInventory = false)
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableUtility.TransferNoSplit(transferables[i].things, transferables[i].CountToTransfer, delegate(Thing originalThing, int toTake)
				{
					CollectionsMassCalculator.tmpThingCounts.Add(new ThingCount(originalThing, toTake, false));
				}, false, false);
			}
			float result = CollectionsMassCalculator.MassUsage(CollectionsMassCalculator.tmpThingCounts, ignoreInventory, includePawnsMass, ignoreSpawnedCorpsesGearAndInventory);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06013298 RID: 78488 RVA: 0x005B24C0 File Offset: 0x005B06C0
		public static float MassUsageLeftAfterTransfer(List<TransferableOneWay> transferables, IgnorePawnsInventoryMode ignoreInventory, bool includePawnsMass = false, bool ignoreSpawnedCorpsesGearAndInventory = false)
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				CollectionsMassCalculator.thingsInReverse.Clear();
				CollectionsMassCalculator.thingsInReverse.AddRange(transferables[i].things);
				CollectionsMassCalculator.thingsInReverse.Reverse();
				TransferableUtility.TransferNoSplit(CollectionsMassCalculator.thingsInReverse, transferables[i].MaxCount - transferables[i].CountToTransfer, delegate(Thing originalThing, int toTake)
				{
					CollectionsMassCalculator.tmpThingCounts.Add(new ThingCount(originalThing, toTake, false));
				}, false, false);
			}
			float result = CollectionsMassCalculator.MassUsage(CollectionsMassCalculator.tmpThingCounts, ignoreInventory, includePawnsMass, ignoreSpawnedCorpsesGearAndInventory);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06013299 RID: 78489 RVA: 0x005B2570 File Offset: 0x005B0770
		public static float MassUsage<T>(List<T> things, IgnorePawnsInventoryMode ignoreInventory, bool includePawnsMass = false, bool ignoreSpawnedCorpsesGearAndInventory = false) where T : Thing
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			for (int i = 0; i < things.Count; i++)
			{
				CollectionsMassCalculator.tmpThingCounts.Add(new ThingCount(things[i], things[i].stackCount, false));
			}
			float result = CollectionsMassCalculator.MassUsage(CollectionsMassCalculator.tmpThingCounts, ignoreInventory, includePawnsMass, ignoreSpawnedCorpsesGearAndInventory);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x0601329A RID: 78490 RVA: 0x005B25DC File Offset: 0x005B07DC
		public static float MassUsageLeftAfterTradeableTransfer(List<Thing> allCurrentThings, List<Tradeable> tradeables, IgnorePawnsInventoryMode ignoreInventory, bool includePawnsMass = false, bool ignoreSpawnedCorpsesGearAndInventory = false)
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			TransferableUtility.SimulateTradeableTransfer(allCurrentThings, tradeables, CollectionsMassCalculator.tmpThingCounts);
			float result = CollectionsMassCalculator.MassUsage(CollectionsMassCalculator.tmpThingCounts, ignoreInventory, includePawnsMass, ignoreSpawnedCorpsesGearAndInventory);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x0400C6E6 RID: 50918
		private static List<ThingCount> tmpThingCounts = new List<ThingCount>();

		// Token: 0x0400C6E7 RID: 50919
		private static List<Thing> thingsInReverse = new List<Thing>();
	}
}
