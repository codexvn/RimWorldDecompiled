using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02003993 RID: 14739
	public class SymbolResolver_Interior_Barracks : SymbolResolver
	{
		// Token: 0x060143D0 RID: 82896 RVA: 0x0060A884 File Offset: 0x00608A84
		public override void Resolve(ResolveParams rp)
		{
			InteriorSymbolResolverUtility.PushBedroomHeatersCoolersAndLightSourcesSymbols(rp, true);
			BaseGen.symbolStack.Push("fillWithBeds", rp, null);
		}
	}
}
