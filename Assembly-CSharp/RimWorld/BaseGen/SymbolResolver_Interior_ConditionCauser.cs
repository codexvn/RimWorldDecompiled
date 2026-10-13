using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02003996 RID: 14742
	public class SymbolResolver_Interior_ConditionCauser : SymbolResolver
	{
		// Token: 0x060143D7 RID: 82903 RVA: 0x0060AA58 File Offset: 0x00608C58
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			resolveParams.singleThingToSpawn = rp.conditionCauser;
			BaseGen.symbolStack.Push("thing", resolveParams, null);
		}
	}
}
