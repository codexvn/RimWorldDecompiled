using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FA1 RID: 12193
	public class StockGenerator_Techprints : StockGenerator
	{
		// Token: 0x060112B6 RID: 70326 RVA: 0x004EBE8A File Offset: 0x004EA08A
		public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			this.tmpGenerated.Clear();
			int countToGenerate = CountChanceUtility.RandomCount(this.countChances);
			int num;
			for (int i = 0; i < countToGenerate; i = num + 1)
			{
				ThingDef thingDef;
				if (!TechprintUtility.TryGetTechprintDefToGenerate(faction, out thingDef, this.tmpGenerated, 3.4028235E+38f))
				{
					yield break;
				}
				this.tmpGenerated.Add(thingDef);
				foreach (Thing thing in StockGeneratorUtility.TryMakeForStock(thingDef, 1, faction))
				{
					yield return thing;
				}
				IEnumerator<Thing> enumerator = null;
				num = i;
			}
			this.tmpGenerated.Clear();
			yield break;
			yield break;
		}

		// Token: 0x060112B7 RID: 70327 RVA: 0x004EBEA1 File Offset: 0x004EA0A1
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.tradeTags != null && thingDef.tradeability != Tradeability.None && thingDef.techLevel <= this.maxTechLevelBuy && thingDef.tradeTags.Contains("Techprint");
		}

		// Token: 0x0400B23C RID: 45628
		private List<CountChance> countChances;

		// Token: 0x0400B23D RID: 45629
		private List<ThingDef> tmpGenerated = new List<ThingDef>();
	}
}
