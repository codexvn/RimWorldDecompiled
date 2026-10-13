using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A23 RID: 14883
	public class QuestPart_AddShipJob_FlyAway : QuestPart_AddShipJob
	{
		// Token: 0x06014620 RID: 83488 RVA: 0x00617390 File Offset: 0x00615590
		public override ShipJob GetShipJob()
		{
			ShipJob_FlyAway shipJob_FlyAway = (ShipJob_FlyAway)ShipJobMaker.MakeShipJob(this.shipJobDef);
			shipJob_FlyAway.destinationTile = this.destinationTile;
			shipJob_FlyAway.arrivalAction = this.arrivalAction;
			shipJob_FlyAway.dropMode = this.dropMode;
			return shipJob_FlyAway;
		}

		// Token: 0x06014621 RID: 83489 RVA: 0x006173C8 File Offset: 0x006155C8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<PlanetTile>(ref this.destinationTile, "destinationTile", default(PlanetTile), false);
			Scribe_Deep.Look<TransportersArrivalAction>(ref this.arrivalAction, "arrivalAction", Array.Empty<object>());
			Scribe_Values.Look<TransportShipDropMode>(ref this.dropMode, "dropMode", TransportShipDropMode.None, false);
		}

		// Token: 0x0400E120 RID: 57632
		public PlanetTile destinationTile = PlanetTile.Invalid;

		// Token: 0x0400E121 RID: 57633
		public TransportersArrivalAction arrivalAction;

		// Token: 0x0400E122 RID: 57634
		public TransportShipDropMode dropMode = TransportShipDropMode.All;
	}
}
