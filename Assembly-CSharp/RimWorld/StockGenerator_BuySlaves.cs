using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F7E RID: 12158
	public class StockGenerator_BuySlaves : StockGenerator
	{
		// Token: 0x060111FE RID: 70142 RVA: 0x00419816 File Offset: 0x00417A16
		public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			return Enumerable.Empty<Thing>();
		}

		// Token: 0x060111FF RID: 70143 RVA: 0x004E9A83 File Offset: 0x004E7C83
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Pawn && thingDef.race.Humanlike && thingDef.tradeability > Tradeability.None;
		}

		// Token: 0x06011200 RID: 70144 RVA: 0x004E9AA6 File Offset: 0x004E7CA6
		public override Tradeability TradeabilityFor(ThingDef thingDef)
		{
			if (!this.HandlesThingDef(thingDef))
			{
				return Tradeability.None;
			}
			return Tradeability.Sellable;
		}
	}
}
