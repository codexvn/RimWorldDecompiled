using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CC6 RID: 11462
	public class CompProperties_GrowsFleshmassTendrils : CompProperties
	{
		// Token: 0x0601016E RID: 65902 RVA: 0x004A786F File Offset: 0x004A5A6F
		public CompProperties_GrowsFleshmassTendrils()
		{
			this.compClass = typeof(CompGrowsFleshmassTendrils);
		}

		// Token: 0x0400A816 RID: 43030
		public int mtbGrowthCycleHours;

		// Token: 0x0400A817 RID: 43031
		public int minGrowthCycleHours;

		// Token: 0x0400A818 RID: 43032
		public int maxGrowthCycleHours;

		// Token: 0x0400A819 RID: 43033
		public SimpleCurve startingGrowthPointsByThreat;

		// Token: 0x0400A81A RID: 43034
		public SimpleCurve growthCyclePointsByThreat;

		// Token: 0x0400A81B RID: 43035
		public SimpleCurve fleshbeastPointsByThreat;

		// Token: 0x0400A81C RID: 43036
		public IntRange? fleshbeastBirthThresholdRange;

		// Token: 0x0400A81D RID: 43037
		public bool spawnFleshbeasts = true;
	}
}
