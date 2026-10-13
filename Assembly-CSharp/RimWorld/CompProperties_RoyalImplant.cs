using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DF2 RID: 11762
	public class CompProperties_RoyalImplant : CompProperties
	{
		// Token: 0x060108E6 RID: 67814 RVA: 0x004C5F52 File Offset: 0x004C4152
		public CompProperties_RoyalImplant()
		{
			this.compClass = typeof(CompRoyalImplant);
		}

		// Token: 0x0400AC9C RID: 44188
		public HediffDef implantHediff;
	}
}
