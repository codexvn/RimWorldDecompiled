using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D66 RID: 15718
	public static class CaravanShuttleUtility
	{
		// Token: 0x06015690 RID: 87696 RVA: 0x00667CBC File Offset: 0x00665EBC
		private static float GetCaravanShuttleMass(Caravan caravan)
		{
			int num = caravan.GetHashCode();
			foreach (Thing thing in CaravanInventoryUtility.AllInventoryItems(caravan))
			{
				num = Gen.HashCombineInt(num, thing.GetHashCode());
			}
			foreach (Pawn pawn in caravan.pawns)
			{
				num = Gen.HashCombineInt(num, pawn.GetHashCode());
			}
			if (num == CaravanShuttleUtility.cachedCaravanItemsHash)
			{
				return CaravanShuttleUtility.cachedCaravanShuttleMass;
			}
			CaravanShuttleUtility.cachedCaravanItemsHash = num;
			CaravanShuttleUtility.cachedCaravanShuttleMass = CollectionsMassCalculator.MassUsage(caravan.pawns, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, true, false);
			CaravanShuttleUtility.cachedCaravanShuttleMass -= caravan.Shuttle.GetStatValue(StatDefOf.Mass, true, -1);
			return CaravanShuttleUtility.cachedCaravanShuttleMass;
		}

		// Token: 0x06015691 RID: 87697 RVA: 0x00667DB0 File Offset: 0x00665FB0
		public static float FuelInCaravan(Caravan caravan)
		{
			float num = 0f;
			foreach (Thing thing in CaravanInventoryUtility.AllInventoryItems(caravan))
			{
				if (caravan.Shuttle.RefuelableComp.Props.fuelFilter.Allows(thing))
				{
					num += (float)thing.stackCount;
				}
			}
			return num;
		}

		// Token: 0x06015692 RID: 87698 RVA: 0x00667E2C File Offset: 0x0066602C
		public static void ConsumeFuelFromCaravanInventory(Caravan caravan, int fuelAmount)
		{
			int num = fuelAmount;
			foreach (Thing thing in CaravanInventoryUtility.AllInventoryItems(caravan))
			{
				if (caravan.Shuttle.RefuelableComp.Props.fuelFilter.Allows(thing))
				{
					Thing thing2 = thing.SplitOff(Mathf.Min(num, thing.stackCount));
					num -= thing2.stackCount;
				}
				if (num <= 0)
				{
					break;
				}
			}
		}

		// Token: 0x06015693 RID: 87699 RVA: 0x00667EBC File Offset: 0x006660BC
		public static AcceptanceReport CanLaunchCaravanShuttle(Caravan caravan)
		{
			float caravanShuttleMass = CaravanShuttleUtility.GetCaravanShuttleMass(caravan);
			if (caravanShuttleMass > caravan.Shuttle.TransporterComp.MassCapacity)
			{
				return "CommandLaunchGroupFailOverMassCapacity".Translate() + ": " + "MassUsageString".Translate(caravanShuttleMass.ToString("F0"), caravan.Shuttle.TransporterComp.MassCapacity.ToString("F0"));
			}
			float fuelLevel = caravan.Shuttle.FuelLevel;
			return caravan.Shuttle.LaunchableComp.CanLaunch(new float?(fuelLevel));
		}

		// Token: 0x06015694 RID: 87700 RVA: 0x00667F61 File Offset: 0x00666161
		public static bool IsCaravanShuttle(CompTransporter transporter)
		{
			return ((transporter != null) ? transporter.Shuttle : null) != null && transporter.parent.IsInCaravan();
		}

		// Token: 0x06015695 RID: 87701 RVA: 0x00667F80 File Offset: 0x00666180
		public static void LaunchShuttle(Caravan caravan, PlanetTile destinationTile, TransportersArrivalAction arrivalAction)
		{
			Find.WorldTargeter.StopTargeting();
			Building_PassengerShuttle shuttle = caravan.Shuttle;
			float fuelLevel = shuttle.FuelLevel;
			if (!shuttle.LaunchableComp.CanLaunch(new float?(fuelLevel)))
			{
				return;
			}
			int num = Find.WorldGrid.TraversalDistanceBetween(caravan.Tile, destinationTile, true, int.MaxValue, true);
			if (num > shuttle.LaunchableComp.MaxLaunchDistanceAtFuelLevel(fuelLevel, destinationTile.Layer))
			{
				return;
			}
			float amount = Mathf.Max(shuttle.LaunchableComp.FuelNeededToLaunchAtDist((float)num, destinationTile.Layer), 1f);
			shuttle.RefuelableComp.ConsumeFuel(amount);
			CaravanInventoryUtility.GetOwnerOf(caravan, shuttle).inventory.innerContainer.Remove(shuttle);
			ActiveTransporterInfo activeTransporterInfo = new ActiveTransporterInfo();
			activeTransporterInfo.sentTransporterDef = shuttle.TransporterComp.parent.def;
			CaravanShuttleUtility.LoadCaravanItemsIntoContainer(caravan.pawns, activeTransporterInfo.innerContainer);
			activeTransporterInfo.innerContainer.TryAddRangeOrTransfer(caravan.pawns, true, false);
			activeTransporterInfo.SetShuttle(shuttle);
			TravellingTransporters travellingTransporters = (TravellingTransporters)WorldObjectMaker.MakeWorldObject(shuttle.LaunchableComp.Props.worldObjectDef);
			travellingTransporters.SetFaction(Faction.OfPlayer);
			travellingTransporters.destinationTile = destinationTile;
			travellingTransporters.arrivalAction = arrivalAction;
			PlanetTile planetTile = caravan.Tile;
			if (planetTile.Layer != destinationTile.Layer)
			{
				planetTile = destinationTile.Layer.GetClosestTile(planetTile);
			}
			travellingTransporters.Tile = planetTile;
			travellingTransporters.AddTransporter(activeTransporterInfo, true);
			Find.WorldObjects.Add(travellingTransporters);
			shuttle.LaunchableComp.lastLaunchTick = Find.TickManager.TicksGame;
			INotifyLaunchableLaunch notifyLaunchableLaunch = shuttle as INotifyLaunchableLaunch;
			if (notifyLaunchableLaunch != null)
			{
				notifyLaunchableLaunch.Notify_LaunchableLaunched(shuttle.LaunchableComp);
			}
			caravan.Destroy();
			CameraJumper.TryJump(shuttle, CameraJumper.MovementMode.Pan);
		}

		// Token: 0x06015696 RID: 87702 RVA: 0x0066813C File Offset: 0x0066633C
		public static void LoadCaravanItemsIntoContainer(IEnumerable<Pawn> caravanPawns, ThingOwner container)
		{
			foreach (Pawn pawn in caravanPawns)
			{
				int num = 1000;
				for (;;)
				{
					Pawn_InventoryTracker inventory = pawn.inventory;
					if (((inventory != null) ? inventory.FirstUnloadableThing.Thing : null) == null || num-- <= 0)
					{
						break;
					}
					ThingCount firstUnloadableThing = pawn.inventory.FirstUnloadableThing;
					pawn.inventory.innerContainer.TryTransferToContainer(firstUnloadableThing.Thing, container, firstUnloadableThing.Count, true);
				}
			}
		}

		// Token: 0x0400EE55 RID: 61013
		private static int cachedCaravanItemsHash = -1;

		// Token: 0x0400EE56 RID: 61014
		private static float cachedCaravanShuttleMass = -1f;
	}
}
