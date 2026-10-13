using System;

namespace RimWorld
{
	// Token: 0x02002B5E RID: 11102
	public class ThingSetMaker_Conditional_MinMaxTotalMarketValue : ThingSetMaker_Conditional
	{
		// Token: 0x0600F842 RID: 63554 RVA: 0x0048348D File Offset: 0x0048168D
		protected override bool Condition(ThingSetMakerParams parms)
		{
			return parms.totalMarketValueRange != null && parms.totalMarketValueRange.Value.max >= this.minMaxTotalMarketValue;
		}

		// Token: 0x0400A33F RID: 41791
		public float minMaxTotalMarketValue;
	}
}
