using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CDF RID: 11487
	public class CompProperties_Hatcher : CompProperties
	{
		// Token: 0x0601022A RID: 66090 RVA: 0x004AB17F File Offset: 0x004A937F
		public CompProperties_Hatcher()
		{
			this.compClass = typeof(CompHatcher);
		}

		// Token: 0x0400A88F RID: 43151
		public float hatcherDaystoHatch = 1f;

		// Token: 0x0400A890 RID: 43152
		public PawnKindDef hatcherPawn;
	}
}
