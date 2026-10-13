using System;

namespace RimWorld
{
	// Token: 0x02002BAF RID: 11183
	public class CompProperties_CompAnalyzableUnlockResearch : CompProperties_Analyzable
	{
		// Token: 0x0600FA09 RID: 64009 RVA: 0x0048B490 File Offset: 0x00489690
		public CompProperties_CompAnalyzableUnlockResearch()
		{
			this.compClass = typeof(CompAnalyzableUnlockResearch);
		}

		// Token: 0x0400A461 RID: 42081
		public int analysisID;

		// Token: 0x0400A462 RID: 42082
		public bool requiresMechanitor;
	}
}
