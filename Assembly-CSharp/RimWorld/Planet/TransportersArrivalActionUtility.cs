using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DFF RID: 15871
	public static class TransportersArrivalActionUtility
	{
		// Token: 0x06015A67 RID: 88679 RVA: 0x0067AB5B File Offset: 0x00678D5B
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions<T>(Func<FloatMenuAcceptanceReport> acceptanceReportGetter, Func<T> arrivalActionGetter, string label, Action<PlanetTile, TransportersArrivalAction> launchAction, PlanetTile destinationTile, Action<Action> uiConfirmationCallback = null) where T : TransportersArrivalAction
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = acceptanceReportGetter();
			if (floatMenuAcceptanceReport.Accepted || !floatMenuAcceptanceReport.FailReason.NullOrEmpty() || !floatMenuAcceptanceReport.FailMessage.NullOrEmpty())
			{
				if (!floatMenuAcceptanceReport.Accepted && !floatMenuAcceptanceReport.FailReason.NullOrEmpty())
				{
					label = label + " (" + floatMenuAcceptanceReport.FailReason + ")";
				}
				Action <>9__1;
				yield return new FloatMenuOption(label, delegate()
				{
					FloatMenuAcceptanceReport floatMenuAcceptanceReport2 = acceptanceReportGetter();
					if (!floatMenuAcceptanceReport2.Accepted)
					{
						if (!floatMenuAcceptanceReport2.FailMessage.NullOrEmpty())
						{
							Messages.Message(floatMenuAcceptanceReport2.FailMessage, new GlobalTargetInfo(destinationTile), MessageTypeDefOf.RejectInput, false);
						}
						return;
					}
					if (uiConfirmationCallback == null)
					{
						launchAction(destinationTile, arrivalActionGetter());
						return;
					}
					Action<Action> uiConfirmationCallback2 = uiConfirmationCallback;
					Action obj;
					if ((obj = <>9__1) == null)
					{
						obj = (<>9__1 = delegate()
						{
							launchAction(destinationTile, arrivalActionGetter());
						});
					}
					uiConfirmationCallback2(obj);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			yield break;
		}

		// Token: 0x06015A68 RID: 88680 RVA: 0x0067AB90 File Offset: 0x00678D90
		public static bool AnyNonDownedColonist(IEnumerable<IThingHolder> pods)
		{
			if (CaravanShuttleUtility.IsCaravanShuttle(pods.First<IThingHolder>() as CompTransporter))
			{
				return true;
			}
			foreach (IThingHolder thingHolder in pods)
			{
				ThingOwner directlyHeldThings = thingHolder.GetDirectlyHeldThings();
				for (int i = 0; i < directlyHeldThings.Count; i++)
				{
					Pawn pawn = directlyHeldThings[i] as Pawn;
					if (pawn != null && pawn.IsColonist && !pawn.Downed)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06015A69 RID: 88681 RVA: 0x0067AC24 File Offset: 0x00678E24
		public static bool AnyPotentialCaravanOwner(IEnumerable<IThingHolder> pods, Faction faction)
		{
			foreach (IThingHolder thingHolder in pods)
			{
				CompTransporter compTransporter = thingHolder as CompTransporter;
				if (compTransporter != null && CaravanShuttleUtility.IsCaravanShuttle(compTransporter))
				{
					return true;
				}
				ThingOwner directlyHeldThings = thingHolder.GetDirectlyHeldThings();
				for (int i = 0; i < directlyHeldThings.Count; i++)
				{
					Pawn pawn = directlyHeldThings[i] as Pawn;
					if (pawn != null && CaravanUtility.IsOwner(pawn, faction))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06015A6A RID: 88682 RVA: 0x0067ACC0 File Offset: 0x00678EC0
		public static Thing GetLookTarget(List<ActiveTransporterInfo> pods)
		{
			for (int i = 0; i < pods.Count; i++)
			{
				ThingOwner directlyHeldThings = pods[i].GetDirectlyHeldThings();
				for (int j = 0; j < directlyHeldThings.Count; j++)
				{
					Pawn pawn = directlyHeldThings[j] as Pawn;
					if (pawn != null && pawn.IsColonist)
					{
						return pawn;
					}
				}
			}
			for (int k = 0; k < pods.Count; k++)
			{
				Thing thing = pods[k].GetDirectlyHeldThings().FirstOrDefault<Thing>();
				if (thing != null)
				{
					return thing;
				}
			}
			return null;
		}

		// Token: 0x06015A6B RID: 88683 RVA: 0x0067AD48 File Offset: 0x00678F48
		public static void DropTravellingDropPods(List<ActiveTransporterInfo> transporters, IntVec3 near, Map map)
		{
			TransportersArrivalActionUtility.RemovePawnsFromWorldPawns(transporters);
			for (int i = 0; i < transporters.Count; i++)
			{
				IntVec3 c;
				DropCellFinder.TryFindDropSpotNear(near, map, out c, false, true, true, null, true);
				DropPodUtility.MakeDropPodAt(c, map, transporters[i], null);
			}
		}

		// Token: 0x06015A6C RID: 88684 RVA: 0x0067AD94 File Offset: 0x00678F94
		public static Thing DropShuttle(ActiveTransporterInfo transporter, Map map, IntVec3 near, Rot4? rotation = null, Faction faction = null)
		{
			TransportersArrivalActionUtility.<>c__DisplayClass5_0 CS$<>8__locals1 = new TransportersArrivalActionUtility.<>c__DisplayClass5_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.rotation = rotation;
			TransportersArrivalActionUtility.RemovePawnsFromWorldPawns(Gen.YieldSingle<ActiveTransporterInfo>(transporter));
			Thing thing = transporter.RemoveShuttle();
			if (thing == null)
			{
				thing = QuestGen_Shuttle.GenerateShuttle(faction, null, null, false, false, false, 0, false, false, true, false, null, null, -1, null, false, true, false, false, false);
			}
			TransportersArrivalActionUtility.<>c__DisplayClass5_0 CS$<>8__locals2 = CS$<>8__locals1;
			CompShuttle compShuttle = thing.TryGetComp<CompShuttle>();
			CS$<>8__locals2.shipDef = (((compShuttle != null) ? compShuttle.Props.shipDef : null) ?? TransportShipDefOf.Ship_Shuttle);
			Rot4 value = CS$<>8__locals1.rotation.GetValueOrDefault();
			if (CS$<>8__locals1.rotation == null)
			{
				value = CS$<>8__locals1.shipDef.shipThing.defaultPlacingRot;
				CS$<>8__locals1.rotation = new Rot4?(value);
			}
			thing.Rotation = CS$<>8__locals1.rotation.Value;
			IntVec3 cell;
			if (RoyalTitlePermitWorker_CallShuttle.ShuttleCanLandHere(near, CS$<>8__locals1.map, CS$<>8__locals1.shipDef.shipThing, new Rot4?(CS$<>8__locals1.rotation.Value)))
			{
				cell = near;
			}
			else if (!CellFinder.TryFindRandomCellNear(near, CS$<>8__locals1.map, 20, (IntVec3 c) => RoyalTitlePermitWorker_CallShuttle.ShuttleCanLandHere(c, CS$<>8__locals1.map, CS$<>8__locals1.shipDef.shipThing, new Rot4?(CS$<>8__locals1.rotation.Value)), out cell, -1) && !CellFinder.TryFindRandomCell(CS$<>8__locals1.map, (IntVec3 c) => RoyalTitlePermitWorker_CallShuttle.ShuttleCanLandHere(c, CS$<>8__locals1.map, CS$<>8__locals1.shipDef.shipThing, new Rot4?(CS$<>8__locals1.rotation.Value)), out cell))
			{
				Log.Warning("Could not find a suitable cell for shuttle landing.");
				cell = near;
			}
			CompTransporter compTransporter = thing.TryGetComp<CompTransporter>();
			compTransporter.innerContainer.TryAddRangeOrTransfer(transporter.innerContainer, true, false);
			TransportShip transportShip = compTransporter.Shuttle.shipParent;
			if (transportShip == null)
			{
				transportShip = TransportShipMaker.MakeTransportShip(CS$<>8__locals1.shipDef, null, thing);
			}
			if (!cell.IsValid)
			{
				cell = DropCellFinder.GetBestShuttleLandingSpot(CS$<>8__locals1.map, Faction.OfPlayer);
			}
			transportShip.ArriveAt(cell, CS$<>8__locals1.map.Parent);
			TransportShipDef shipDef = thing.TryGetComp<CompShuttle>().Props.shipDef;
			if (shipDef == null || !shipDef.playerShuttle)
			{
				transportShip.AddJobs(new ShipJobDef[]
				{
					ShipJobDefOf.Unload,
					ShipJobDefOf.FlyAway
				});
			}
			else
			{
				ShipJob_Unload shipJob_Unload = (ShipJob_Unload)ShipJobMaker.MakeShipJob(ShipJobDefOf.Unload);
				shipJob_Unload.dropMode = TransportShipDropMode.PawnsOnly;
				transportShip.AddJob(shipJob_Unload);
			}
			return thing;
		}

		// Token: 0x06015A6D RID: 88685 RVA: 0x0067AF94 File Offset: 0x00679194
		public static void RemovePawnsFromWorldPawns(IEnumerable<ActiveTransporterInfo> transporters)
		{
			foreach (ActiveTransporterInfo activeTransporterInfo in transporters)
			{
				ThingOwner innerContainer = activeTransporterInfo.innerContainer;
				for (int i = 0; i < innerContainer.Count; i++)
				{
					Pawn pawn = innerContainer[i] as Pawn;
					if (pawn != null && pawn.IsWorldPawn())
					{
						Find.WorldPawns.RemovePawn(pawn);
					}
				}
			}
		}
	}
}
