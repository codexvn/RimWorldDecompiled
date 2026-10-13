using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002862 RID: 10338
	public class TransportersArrivalAction_TransportShip : TransportersArrivalAction
	{
		// Token: 0x17002273 RID: 8819
		// (get) Token: 0x0600E203 RID: 57859 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool GeneratesMap
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600E204 RID: 57860 RVA: 0x00423504 File Offset: 0x00421704
		public TransportersArrivalAction_TransportShip()
		{
		}

		// Token: 0x0600E205 RID: 57861 RVA: 0x00423517 File Offset: 0x00421717
		public TransportersArrivalAction_TransportShip(MapParent mapParent, TransportShip transportShip)
		{
			this.mapParent = mapParent;
			this.transportShip = transportShip;
		}

		// Token: 0x0600E206 RID: 57862 RVA: 0x00423538 File Offset: 0x00421738
		public override bool ShouldUseLongEvent(List<ActiveTransporterInfo> pods, PlanetTile tile)
		{
			return !this.mapParent.HasMap;
		}

		// Token: 0x0600E207 RID: 57863 RVA: 0x00423548 File Offset: 0x00421748
		public override void Arrived(List<ActiveTransporterInfo> transporters, PlanetTile tile)
		{
			if (this.transportShip == null || this.transportShip.Disposed)
			{
				Log.Error("Trying to arrive in a null or disposed transport ship.");
				return;
			}
			if (this.mapParent.Destroyed)
			{
				new TransportersArrivalAction_FormCaravan().Arrived(transporters, tile);
				return;
			}
			bool flag = !this.mapParent.HasMap;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(tile, null, null);
			if (!this.cell.IsValid)
			{
				this.cell = DropCellFinder.GetBestShuttleLandingSpot(orGenerateMap, Faction.OfPlayer);
			}
			LookTargets lookTargets = new LookTargets(this.cell, orGenerateMap);
			if (!this.cell.IsValid)
			{
				Log.Error("Could not find cell for transport ship arrival.");
				return;
			}
			Settlement settlement = orGenerateMap.Parent as Settlement;
			if (settlement != null && settlement.Faction != Faction.OfPlayer)
			{
				TaggedString label = "LetterLabelCaravanEnteredEnemyBase".Translate();
				TaggedString text = "LetterShuttleLandedInEnemyBase".Translate(settlement.Label).CapitalizeFirst();
				SettlementUtility.AffectRelationsOnAttacked(settlement, ref text);
				if (flag)
				{
					Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
					PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(orGenerateMap.mapPawns.AllPawns, ref label, ref text, "LetterRelatedPawnsInMapWherePlayerLanded".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
				}
				Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, lookTargets, settlement.Faction, null, null, null, 0, true);
			}
			transporters[0].innerContainer.Remove(this.transportShip.shipThing);
			this.transportShip.TransporterComp.innerContainer.TryAddRangeOrTransfer(transporters[0].innerContainer, true, true);
			this.transportShip.ArriveAt(this.cell, this.mapParent);
			TransportShipDef shipDef = this.transportShip.shipThing.TryGetComp<CompShuttle>().Props.shipDef;
			if (shipDef != null && shipDef.playerShuttle)
			{
				ShipJob_Unload shipJob_Unload = (ShipJob_Unload)ShipJobMaker.MakeShipJob(ShipJobDefOf.Unload);
				shipJob_Unload.dropMode = TransportShipDropMode.PawnsOnly;
				this.transportShip.AddJob(shipJob_Unload);
			}
			Messages.Message("MessageShuttleArrived".Translate(), lookTargets, MessageTypeDefOf.TaskCompletion, true);
		}

		// Token: 0x0600E208 RID: 57864 RVA: 0x00423768 File Offset: 0x00421968
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<TransportShip>(ref this.transportShip, "transportShip", false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
		}

		// Token: 0x040097B7 RID: 38839
		public MapParent mapParent;

		// Token: 0x040097B8 RID: 38840
		public TransportShip transportShip;

		// Token: 0x040097B9 RID: 38841
		public IntVec3 cell = IntVec3.Invalid;
	}
}
