using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001984 RID: 6532
	public class CompProperties_Shearable : CompProperties
	{
		// Token: 0x06009439 RID: 37945 RVA: 0x002B8BDF File Offset: 0x002B6DDF
		public CompProperties_Shearable()
		{
			this.compClass = typeof(CompShearable);
		}

		// Token: 0x04005FBA RID: 24506
		public int shearIntervalDays;

		// Token: 0x04005FBB RID: 24507
		public int woolAmount = 1;

		// Token: 0x04005FBC RID: 24508
		public ThingDef woolDef;
	}
}
