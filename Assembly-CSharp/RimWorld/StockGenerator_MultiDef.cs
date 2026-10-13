using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F93 RID: 12179
	public class StockGenerator_MultiDef : StockGenerator
	{
		// Token: 0x0601126A RID: 70250 RVA: 0x004EB047 File Offset: 0x004E9247
		public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			ThingDef thingDef = this.thingDefs.RandomElement<ThingDef>();
			foreach (Thing thing in StockGeneratorUtility.TryMakeForStock(thingDef, base.RandomCountOf(thingDef), faction))
			{
				yield return thing;
			}
			IEnumerator<Thing> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0601126B RID: 70251 RVA: 0x004EB05E File Offset: 0x004E925E
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return this.thingDefs.Contains(thingDef);
		}

		// Token: 0x0601126C RID: 70252 RVA: 0x004EB06C File Offset: 0x004E926C
		public override IEnumerable<string> ConfigErrors(TraderKindDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			int num;
			for (int i = 0; i < this.thingDefs.Count; i = num + 1)
			{
				if (!this.thingDefs[i].tradeability.TraderCanSell())
				{
					ThingDef thingDef = this.thingDefs[i];
					yield return ((thingDef != null) ? thingDef.ToString() : null) + " tradeability doesn't allow traders to sell this thing";
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x0400B1FC RID: 45564
		private List<ThingDef> thingDefs = new List<ThingDef>();
	}
}
