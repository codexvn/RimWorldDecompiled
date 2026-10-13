using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200393C RID: 14652
	public class SymbolResolver_Symbol : SymbolResolver
	{
		// Token: 0x060142CB RID: 82635 RVA: 0x00603214 File Offset: 0x00601414
		public override bool CanResolve(ResolveParams rp)
		{
			if (!base.CanResolve(rp))
			{
				return false;
			}
			List<RuleDef> allDefsListForReading = DefDatabase<RuleDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				RuleDef ruleDef = allDefsListForReading[i];
				if (!(ruleDef.symbol != this.symbol))
				{
					for (int j = 0; j < ruleDef.resolvers.Count; j++)
					{
						if (ruleDef.resolvers[j].CanResolve(rp))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060142CC RID: 82636 RVA: 0x0060328B File Offset: 0x0060148B
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push(this.symbol, rp, null);
		}

		// Token: 0x0400DF0C RID: 57100
		public string symbol;
	}
}
