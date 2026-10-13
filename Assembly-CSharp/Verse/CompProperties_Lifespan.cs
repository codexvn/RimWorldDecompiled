using System;

namespace Verse
{
	// Token: 0x02000236 RID: 566
	public class CompProperties_Lifespan : CompProperties
	{
		// Token: 0x06000EBA RID: 3770 RVA: 0x0004460A File Offset: 0x0004280A
		public CompProperties_Lifespan()
		{
			this.compClass = typeof(CompLifespan);
		}

		// Token: 0x04000982 RID: 2434
		public int lifespanTicks = 100;

		// Token: 0x04000983 RID: 2435
		public EffecterDef expireEffect;

		// Token: 0x04000984 RID: 2436
		public ThingDef plantDefToSpawn;
	}
}
