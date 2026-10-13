using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02003946 RID: 14662
	public class SymbolResolver_AncientRuins : SymbolResolver
	{
		// Token: 0x060142E5 RID: 82661 RVA: 0x006040EC File Offset: 0x006022EC
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			resolveParams.wallStuff = (rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, true));
			resolveParams.chanceToSkipWallBlock = new float?(rp.chanceToSkipWallBlock ?? 0.1f);
			resolveParams.clearEdificeOnly = new bool?(rp.clearEdificeOnly ?? true);
			resolveParams.noRoof = new bool?(rp.noRoof ?? true);
			BaseGen.symbolStack.Push("emptyRoom", resolveParams, null);
		}
	}
}
