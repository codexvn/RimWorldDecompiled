using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020038F4 RID: 14580
	public class SymbolResolver_BasePart_Indoors_Leaf_WorshippedTerminal : SymbolResolver
	{
		// Token: 0x060141D9 RID: 82393 RVA: 0x005FD50C File Offset: 0x005FB70C
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
						if (ruleDef.resolvers[j] is SymbolResolver_BasePart_Indoors_Leaf_WorshippedTerminal && ruleDef.resolvers[j].CanResolve(rp))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060141DA RID: 82394 RVA: 0x005FD586 File Offset: 0x005FB786
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_worshippedTerminalsResolved < BaseGen.globalSettings.requiredWorshippedTerminalRooms;
		}

		// Token: 0x060141DB RID: 82395 RVA: 0x005FD5AC File Offset: 0x005FB7AC
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("worshippedTerminal", rp, null);
			BaseGen.globalSettings.basePart_worshippedTerminalsResolved++;
		}
	}
}
