using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F7B RID: 12155
	public class StockGenerator_BuyExpensiveSimple : StockGenerator
	{
		// Token: 0x060111F2 RID: 70130 RVA: 0x00419816 File Offset: 0x00417A16
		public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			return Enumerable.Empty<Thing>();
		}

		// Token: 0x060111F3 RID: 70131 RVA: 0x004E99C8 File Offset: 0x004E7BC8
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Item && !thingDef.IsApparel && !thingDef.IsWeapon && !thingDef.IsMedicine && !thingDef.IsDrug && thingDef.genericMarketSellable && (thingDef == ThingDefOf.InsectJelly || thingDef.BaseMarketValue / thingDef.VolumePerUnit >= this.minValuePerUnit);
		}

		// Token: 0x060111F4 RID: 70132 RVA: 0x004E9A2A File Offset: 0x004E7C2A
		public override Tradeability TradeabilityFor(ThingDef thingDef)
		{
			if (thingDef.tradeability == Tradeability.None || !this.HandlesThingDef(thingDef))
			{
				return Tradeability.None;
			}
			return Tradeability.Sellable;
		}

		// Token: 0x0400B1A9 RID: 45481
		public float minValuePerUnit = 15f;
	}
}
