using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003927 RID: 14631
	public class SymbolResolver_LootScatter : SymbolResolver
	{
		// Token: 0x06014289 RID: 82569 RVA: 0x00601BA0 File Offset: 0x005FFDA0
		public override void Resolve(ResolveParams rp)
		{
			float? lootMarketValue = rp.lootMarketValue;
			if (lootMarketValue != null && lootMarketValue.GetValueOrDefault() <= 0f)
			{
				return;
			}
			Map map = BaseGen.globalSettings.map;
			IList<Thing> list = rp.lootConcereteContents;
			if (list == null)
			{
				ThingSetMakerParams parms;
				if (rp.thingSetMakerParams != null)
				{
					parms = rp.thingSetMakerParams.Value;
				}
				else
				{
					parms = new ThingSetMakerParams
					{
						countRange = new IntRange?(SymbolResolver_LootScatter.DefaultLootCountRange),
						techLevel = new TechLevel?((rp.faction != null) ? rp.faction.def.techLevel : TechLevel.Undefined)
					};
				}
				parms.makingFaction = rp.faction;
				parms.totalMarketValueRange = new FloatRange?(new FloatRange(rp.lootMarketValue.Value, rp.lootMarketValue.Value));
				list = rp.thingSetMakerDef.root.Generate(parms);
			}
			MapGenUtility.GenerateLoot(map, rp.rect, list, false);
		}

		// Token: 0x0400DEF1 RID: 57073
		private static readonly IntRange DefaultLootCountRange = new IntRange(3, 10);
	}
}
