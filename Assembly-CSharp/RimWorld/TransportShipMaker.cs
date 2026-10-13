using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002832 RID: 10290
	public static class TransportShipMaker
	{
		// Token: 0x0600E100 RID: 57600 RVA: 0x0042049C File Offset: 0x0041E69C
		public static TransportShip MakeTransportShip(TransportShipDef def, IEnumerable<Thing> contents, Thing shipThing = null)
		{
			TransportShip transportShip = new TransportShip(def);
			transportShip.shipThing = (shipThing ?? ThingMaker.MakeThing(def.shipThing, null));
			CompShuttle compShuttle = transportShip.shipThing.TryGetComp<CompShuttle>();
			if (compShuttle != null)
			{
				compShuttle.shipParent = transportShip;
			}
			if (contents != null)
			{
				transportShip.TransporterComp.innerContainer.TryAddRangeOrTransfer(contents, true, true);
			}
			return transportShip;
		}
	}
}
