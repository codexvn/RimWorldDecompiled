using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003907 RID: 14599
	public class SymbolResolver_Bed : SymbolResolver
	{
		// Token: 0x0601421A RID: 82458 RVA: 0x005FEE80 File Offset: 0x005FD080
		public override void Resolve(ResolveParams rp)
		{
			ThingDef singleThingDef = rp.singleThingDef ?? Rand.Element<ThingDef>(ThingDefOf.Bed, ThingDefOf.Bedroll, ThingDefOf.SleepingSpot);
			ResolveParams resolveParams = rp;
			resolveParams.singleThingDef = singleThingDef;
			resolveParams.skipSingleThingIfHasToWipeBuildingOrDoesntFit = new bool?(rp.skipSingleThingIfHasToWipeBuildingOrDoesntFit ?? true);
			BaseGen.symbolStack.Push("thing", resolveParams, null);
		}
	}
}
