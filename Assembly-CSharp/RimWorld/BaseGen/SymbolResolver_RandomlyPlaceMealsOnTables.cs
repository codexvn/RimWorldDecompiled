using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200396F RID: 14703
	public class SymbolResolver_RandomlyPlaceMealsOnTables : SymbolResolver
	{
		// Token: 0x06014368 RID: 82792 RVA: 0x00607A7C File Offset: 0x00605C7C
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			ThingDef thingDef = (rp.faction == null || !rp.faction.def.techLevel.IsNeolithicOrWorse()) ? ThingDefOf.MealSimple : ThingDefOf.Pemmican;
			foreach (IntVec3 c in rp.rect)
			{
				List<Thing> thingList = c.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (thingList[i].def.IsTable && Rand.Chance(0.15f))
					{
						int value = Mathf.Clamp(Mathf.RoundToInt(ThingDefOf.MealSimple.ingestible.CachedNutrition * Rand.Range(0.9f, 1.1f) / thingDef.ingestible.CachedNutrition), 1, thingDef.stackLimit);
						ResolveParams resolveParams = rp;
						resolveParams.rect = CellRect.SingleCell(c);
						resolveParams.singleThingDef = thingDef;
						resolveParams.singleThingStackCount = new int?(value);
						BaseGen.symbolStack.Push("thing", resolveParams, null);
					}
				}
			}
		}
	}
}
