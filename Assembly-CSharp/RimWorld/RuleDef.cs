using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ADB RID: 6875
	public class RuleDef : Def
	{
		// Token: 0x0400694B RID: 26955
		[NoTranslate]
		public string symbol;

		// Token: 0x0400694C RID: 26956
		public List<SymbolResolver> resolvers;
	}
}
