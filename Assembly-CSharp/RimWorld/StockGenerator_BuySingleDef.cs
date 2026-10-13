using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F7C RID: 12156
	public class StockGenerator_BuySingleDef : StockGenerator
	{
		// Token: 0x060111F6 RID: 70134 RVA: 0x00419816 File Offset: 0x00417A16
		public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			return Enumerable.Empty<Thing>();
		}

		// Token: 0x060111F7 RID: 70135 RVA: 0x004E9A53 File Offset: 0x004E7C53
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == this.thingDef;
		}

		// Token: 0x060111F8 RID: 70136 RVA: 0x004E9A2A File Offset: 0x004E7C2A
		public override Tradeability TradeabilityFor(ThingDef thingDef)
		{
			if (thingDef.tradeability == Tradeability.None || !this.HandlesThingDef(thingDef))
			{
				return Tradeability.None;
			}
			return Tradeability.Sellable;
		}

		// Token: 0x0400B1AA RID: 45482
		public ThingDef thingDef;
	}
}
