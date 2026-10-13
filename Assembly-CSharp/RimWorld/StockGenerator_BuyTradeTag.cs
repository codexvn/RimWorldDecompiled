using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F7D RID: 12157
	public class StockGenerator_BuyTradeTag : StockGenerator
	{
		// Token: 0x060111FA RID: 70138 RVA: 0x00419816 File Offset: 0x00417A16
		public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			return Enumerable.Empty<Thing>();
		}

		// Token: 0x060111FB RID: 70139 RVA: 0x004E9A66 File Offset: 0x004E7C66
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeTags != null && thingDef.tradeTags.Contains(this.tag);
		}

		// Token: 0x060111FC RID: 70140 RVA: 0x004E9A2A File Offset: 0x004E7C2A
		public override Tradeability TradeabilityFor(ThingDef thingDef)
		{
			if (thingDef.tradeability == Tradeability.None || !this.HandlesThingDef(thingDef))
			{
				return Tradeability.None;
			}
			return Tradeability.Sellable;
		}

		// Token: 0x0400B1AB RID: 45483
		public string tag;
	}
}
