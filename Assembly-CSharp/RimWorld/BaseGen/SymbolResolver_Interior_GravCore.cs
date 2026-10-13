using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020039A6 RID: 14758
	public class SymbolResolver_Interior_GravCore : SymbolResolver
	{
		// Token: 0x0601440B RID: 82955 RVA: 0x0060B6E8 File Offset: 0x006098E8
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			if (rp.sitePart != null)
			{
				resolveParams.singleThingToSpawn = rp.sitePart.things.FirstOrDefault((Thing t) => t.def == ThingDefOf.Gravcore);
			}
			if (resolveParams.singleThingToSpawn == null)
			{
				resolveParams.singleThingToSpawn = ThingMaker.MakeThing(ThingDefOf.Gravcore, null);
			}
			resolveParams.rect = CellRect.CenteredOn(rp.rect.CenterCell, 1, 1);
			BaseGen.symbolStack.Push("thing", resolveParams, null);
			if (rp.sitePart != null)
			{
				foreach (Thing thing in ((IEnumerable<Thing>)rp.sitePart.things))
				{
					if (thing.def == ThingDefOf.GravlitePanel)
					{
						ResolveParams resolveParams2 = rp;
						resolveParams2.singleThingToSpawn = thing;
						resolveParams2.rect = rp.rect;
						BaseGen.symbolStack.Push("thing", resolveParams2, null);
					}
				}
			}
		}
	}
}
