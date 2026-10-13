using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020038F5 RID: 14581
	public class SymbolResolver_BasePart_Indoors_Leaf_Gravcore : SymbolResolver
	{
		// Token: 0x060141DD RID: 82397 RVA: 0x005FD5D4 File Offset: 0x005FB7D4
		public static bool CanResolve(string symbol, ResolveParams rp)
		{
			List<RuleDef> allDefsListForReading = DefDatabase<RuleDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				RuleDef ruleDef = allDefsListForReading[i];
				if (!(ruleDef.symbol != symbol))
				{
					for (int j = 0; j < ruleDef.resolvers.Count; j++)
					{
						if (ruleDef.resolvers[j] is SymbolResolver_BasePart_Indoors_Leaf_Gravcore && ruleDef.resolvers[j].CanResolve(rp))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060141DE RID: 82398 RVA: 0x005FD64E File Offset: 0x005FB84E
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_gravcoresResolved < BaseGen.globalSettings.requiredGravcoreRooms;
		}

		// Token: 0x060141DF RID: 82399 RVA: 0x005FD674 File Offset: 0x005FB874
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("gravcore", rp, null);
			BaseGen.globalSettings.basePart_gravcoresResolved++;
		}
	}
}
