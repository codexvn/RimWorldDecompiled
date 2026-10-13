using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001973 RID: 6515
	public class CompProperties_Milkable : CompProperties
	{
		// Token: 0x060093E4 RID: 37860 RVA: 0x002B7C2B File Offset: 0x002B5E2B
		public CompProperties_Milkable()
		{
			this.compClass = typeof(CompMilkable);
		}

		// Token: 0x04005F6E RID: 24430
		public int milkIntervalDays;

		// Token: 0x04005F6F RID: 24431
		public int milkAmount = 1;

		// Token: 0x04005F70 RID: 24432
		public ThingDef milkDef;

		// Token: 0x04005F71 RID: 24433
		public bool milkFemaleOnly = true;
	}
}
