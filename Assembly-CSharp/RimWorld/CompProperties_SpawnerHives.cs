using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E31 RID: 11825
	public class CompProperties_SpawnerHives : CompProperties
	{
		// Token: 0x06010A67 RID: 68199 RVA: 0x004CD1E8 File Offset: 0x004CB3E8
		public CompProperties_SpawnerHives()
		{
			this.compClass = typeof(CompSpawnerHives);
		}

		// Token: 0x0400AD82 RID: 44418
		public float HiveSpawnPreferredMinDist = 3.5f;

		// Token: 0x0400AD83 RID: 44419
		public float HiveSpawnRadius = 10f;

		// Token: 0x0400AD84 RID: 44420
		public FloatRange HiveSpawnIntervalDays = new FloatRange(2f, 3f);

		// Token: 0x0400AD85 RID: 44421
		public SimpleCurve ReproduceRateFactorFromNearbyHiveCountCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(7f, 0.35f),
				true
			}
		};
	}
}
