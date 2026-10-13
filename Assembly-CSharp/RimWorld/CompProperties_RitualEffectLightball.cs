using System;

namespace RimWorld
{
	// Token: 0x020027FF RID: 10239
	public class CompProperties_RitualEffectLightball : CompProperties_RitualEffectIntervalSpawn
	{
		// Token: 0x0600DFFA RID: 57338 RVA: 0x0041C374 File Offset: 0x0041A574
		public CompProperties_RitualEffectLightball()
		{
			this.compClass = typeof(CompRitualEffect_Lightball);
		}

		// Token: 0x040096BB RID: 38587
		public float radius;
	}
}
