using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F96 RID: 12182
	public class StockGenerator_SingleDef : StockGenerator
	{
		// Token: 0x06011281 RID: 70273 RVA: 0x004EB457 File Offset: 0x004E9657
		public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			foreach (Thing thing in StockGeneratorUtility.TryMakeForStock(this.thingDef, base.RandomCountOf(this.thingDef), faction))
			{
				yield return thing;
			}
			IEnumerator<Thing> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06011282 RID: 70274 RVA: 0x004EB46E File Offset: 0x004E966E
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef == this.thingDef;
		}

		// Token: 0x06011283 RID: 70275 RVA: 0x004EB479 File Offset: 0x004E9679
		public override IEnumerable<string> ConfigErrors(TraderKindDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (!this.thingDef.tradeability.TraderCanSell())
			{
				ThingDef thingDef = this.thingDef;
				yield return ((thingDef != null) ? thingDef.ToString() : null) + " tradeability doesn't allow traders to sell this thing";
			}
			yield break;
			yield break;
		}

		// Token: 0x0400B20C RID: 45580
		private ThingDef thingDef;
	}
}
