using System;

namespace RimWorld
{
	// Token: 0x02002801 RID: 10241
	public class CompProperties_RitualEffectDrum : CompProperties_RitualEffectIntervalSpawn
	{
		// Token: 0x0600DFFF RID: 57343 RVA: 0x0041C514 File Offset: 0x0041A714
		public CompProperties_RitualEffectDrum()
		{
			this.compClass = typeof(CompRitualEffect_Drum);
		}

		// Token: 0x040096BC RID: 38588
		public int maxDistance;

		// Token: 0x040096BD RID: 38589
		public float maxOffset;
	}
}
