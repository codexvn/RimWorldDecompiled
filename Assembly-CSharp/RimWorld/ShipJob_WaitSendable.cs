using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001783 RID: 6019
	public class ShipJob_WaitSendable : ShipJob_Wait
	{
		// Token: 0x170015B3 RID: 5555
		// (get) Token: 0x06008D3A RID: 36154 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool ShouldEnd
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170015B4 RID: 5556
		// (get) Token: 0x06008D3B RID: 36155 RVA: 0x002A00A1 File Offset: 0x0029E2A1
		public override bool HasDestination
		{
			get
			{
				return this.destination != null;
			}
		}

		// Token: 0x06008D3C RID: 36156 RVA: 0x000B7861 File Offset: 0x000B5A61
		public override IEnumerable<Gizmo> GetJobGizmos()
		{
			return Enumerable.Empty<Gizmo>();
		}

		// Token: 0x06008D3D RID: 36157 RVA: 0x002A00AC File Offset: 0x0029E2AC
		protected override void SendAway()
		{
			if (this.targetPlayerSettlement && (this.destination == null || !this.destination.HasMap))
			{
				MapParent mapParent = null;
				foreach (Settlement settlement in Find.World.worldObjects.Settlements)
				{
					if (settlement.HasMap && settlement.Faction == Faction.OfPlayer && (mapParent == null || settlement.Map.mapPawns.ColonistCount > mapParent.Map.mapPawns.ColonistCount))
					{
						mapParent = settlement;
					}
				}
				if (mapParent == null)
				{
					mapParent = Find.AnyPlayerHomeMap.Parent;
				}
				if (mapParent == null)
				{
					if (!this.sentMessage)
					{
						Messages.Message("ShipNoSettlementToReturnTo".Translate(), this.transportShip.shipThing, MessageTypeDefOf.CautionInput, true);
						this.sentMessage = true;
					}
					return;
				}
				this.destination = mapParent;
			}
			ShipJob_FlyAway shipJob_FlyAway = (ShipJob_FlyAway)ShipJobMaker.MakeShipJob(ShipJobDefOf.FlyAway);
			shipJob_FlyAway.destinationTile = this.destination.Tile;
			shipJob_FlyAway.arrivalAction = new TransportersArrivalAction_TransportShip(this.destination, this.transportShip);
			shipJob_FlyAway.dropMode = TransportShipDropMode.None;
			this.transportShip.SetNextJob(shipJob_FlyAway);
			this.transportShip.TryGetNextJob();
		}

		// Token: 0x06008D3E RID: 36158 RVA: 0x002A020C File Offset: 0x0029E40C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.destination, "destination", false);
			Scribe_Values.Look<bool>(ref this.sentMessage, "sentMessage", false, false);
		}

		// Token: 0x04005BC5 RID: 23493
		public MapParent destination;

		// Token: 0x04005BC6 RID: 23494
		private bool sentMessage;
	}
}
