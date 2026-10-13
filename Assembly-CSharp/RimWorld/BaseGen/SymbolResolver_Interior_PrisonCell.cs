using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02003998 RID: 14744
	public class SymbolResolver_Interior_PrisonCell : SymbolResolver
	{
		// Token: 0x060143DB RID: 82907 RVA: 0x0060AB14 File Offset: 0x00608D14
		public override void Resolve(ResolveParams rp)
		{
			ThingSetMakerParams value = default(ThingSetMakerParams);
			value.techLevel = new TechLevel?((rp.faction != null) ? rp.faction.def.techLevel : TechLevel.Spacer);
			ResolveParams resolveParams = rp;
			resolveParams.thingSetMakerDef = ThingSetMakerDefOf.MapGen_PrisonCellStockpile;
			resolveParams.thingSetMakerParams = new ThingSetMakerParams?(value);
			resolveParams.innerStockpileSize = new int?(3);
			BaseGen.symbolStack.Push("innerStockpile", resolveParams, null);
			InteriorSymbolResolverUtility.PushBedroomHeatersCoolersAndLightSourcesSymbols(rp, false);
			BaseGen.symbolStack.Push("prisonerBed", rp, null);
		}

		// Token: 0x0400DFAD RID: 57261
		private const int FoodStockpileSize = 3;
	}
}
