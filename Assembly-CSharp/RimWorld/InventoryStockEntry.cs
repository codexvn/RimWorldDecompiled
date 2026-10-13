using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002453 RID: 9299
	public class InventoryStockEntry : IExposable
	{
		// Token: 0x0600C966 RID: 51558 RVA: 0x003AF15D File Offset: 0x003AD35D
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
		}

		// Token: 0x04008925 RID: 35109
		public ThingDef thingDef;

		// Token: 0x04008926 RID: 35110
		public int count;
	}
}
