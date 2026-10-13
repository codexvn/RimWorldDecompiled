using System;

namespace RimWorld
{
	// Token: 0x020027FD RID: 10237
	public class CompProperties_RitualEffectIntervalSpawnDividedCircle : CompProperties_RitualEffectIntervalSpawn
	{
		// Token: 0x0600DFF5 RID: 57333 RVA: 0x0041C210 File Offset: 0x0041A410
		public CompProperties_RitualEffectIntervalSpawnDividedCircle()
		{
			this.compClass = typeof(CompRitualEffect_IntervalSpawnDividedCircleEffecter);
		}

		// Token: 0x040096B9 RID: 38585
		public float radius = 5f;

		// Token: 0x040096BA RID: 38586
		public int numCopies = 5;
	}
}
