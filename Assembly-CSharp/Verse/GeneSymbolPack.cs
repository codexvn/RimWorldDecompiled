using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000181 RID: 385
	public class GeneSymbolPack
	{
		// Token: 0x04000642 RID: 1602
		public List<GeneSymbolPack.WeightedSymbol> prefixSymbols;

		// Token: 0x04000643 RID: 1603
		public List<GeneSymbolPack.WeightedSymbol> suffixSymbols;

		// Token: 0x04000644 RID: 1604
		public List<GeneSymbolPack.WeightedSymbol> wholeNameSymbols;

		// Token: 0x02000182 RID: 386
		public class WeightedSymbol
		{
			// Token: 0x04000645 RID: 1605
			[MustTranslate]
			public string symbol;

			// Token: 0x04000646 RID: 1606
			public float weight = 1f;
		}
	}
}
