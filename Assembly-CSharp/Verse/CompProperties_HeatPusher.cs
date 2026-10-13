using System;

namespace Verse
{
	// Token: 0x02000235 RID: 565
	public class CompProperties_HeatPusher : CompProperties
	{
		// Token: 0x06000EB9 RID: 3769 RVA: 0x000445DC File Offset: 0x000427DC
		public CompProperties_HeatPusher()
		{
			this.compClass = typeof(CompHeatPusher);
		}

		// Token: 0x0400097F RID: 2431
		public float heatPerSecond;

		// Token: 0x04000980 RID: 2432
		public float heatPushMaxTemperature = 99999f;

		// Token: 0x04000981 RID: 2433
		public float heatPushMinTemperature = -99999f;
	}
}
