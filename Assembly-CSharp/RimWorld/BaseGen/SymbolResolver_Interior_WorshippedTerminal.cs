using System;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020039A4 RID: 14756
	public class SymbolResolver_Interior_WorshippedTerminal : SymbolResolver
	{
		// Token: 0x06014406 RID: 82950 RVA: 0x0060B5A0 File Offset: 0x006097A0
		public override void Resolve(ResolveParams rp)
		{
			IntVec3 max = rp.rect.Max;
			max.x -= rp.rect.Width / 2;
			max.z--;
			CellRect rect = rp.rect;
			rect.maxZ -= 3;
			ResolveParams resolveParams = rp;
			if (rp.sitePart != null)
			{
				resolveParams.singleThingToSpawn = rp.sitePart.things.FirstOrDefault((Thing t) => t.def == ThingDefOf.AncientTerminal_Worshipful);
			}
			if (resolveParams.singleThingToSpawn == null)
			{
				resolveParams.singleThingToSpawn = ThingMaker.MakeThing(ThingDefOf.AncientTerminal_Worshipful, null);
			}
			resolveParams.rect = CellRect.CenteredOn(max, 1, 1);
			resolveParams.thingRot = new Rot4?(Rot4.South);
			BaseGen.symbolStack.Push("thing", resolveParams, null);
			if (rect.Height > 1)
			{
				ResolveParams resolveParams2 = rp;
				resolveParams2.rect = rect;
				resolveParams2.singleThingDef = ThingDefOf.SteleLarge;
				BaseGen.symbolStack.Push("thing", resolveParams2, null);
				if (Rand.Chance(0.5f))
				{
					BaseGen.symbolStack.Push("thing", resolveParams2, null);
				}
			}
		}
	}
}
