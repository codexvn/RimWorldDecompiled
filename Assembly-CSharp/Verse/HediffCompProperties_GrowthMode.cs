using System;

namespace Verse
{
	// Token: 0x020006CB RID: 1739
	public class HediffCompProperties_GrowthMode : HediffCompProperties
	{
		// Token: 0x06002B39 RID: 11065 RVA: 0x000E21EC File Offset: 0x000E03EC
		public HediffCompProperties_GrowthMode()
		{
			this.compClass = typeof(HediffComp_GrowthMode);
		}

		// Token: 0x040022DB RID: 8923
		public float severityPerDayGrowing;

		// Token: 0x040022DC RID: 8924
		public float severityPerDayRemission;

		// Token: 0x040022DD RID: 8925
		public FloatRange severityPerDayGrowingRandomFactor = new FloatRange(1f, 1f);

		// Token: 0x040022DE RID: 8926
		public FloatRange severityPerDayRemissionRandomFactor = new FloatRange(1f, 1f);
	}
}
