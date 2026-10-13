using System;

namespace Verse
{
	// Token: 0x02000234 RID: 564
	public class CompProperties_AffectsSky : CompProperties
	{
		// Token: 0x06000EB8 RID: 3768 RVA: 0x000445A3 File Offset: 0x000427A3
		public CompProperties_AffectsSky()
		{
			this.compClass = typeof(CompAffectsSky);
		}

		// Token: 0x0400097A RID: 2426
		public float glow = 1f;

		// Token: 0x0400097B RID: 2427
		public SkyColorSet skyColors;

		// Token: 0x0400097C RID: 2428
		public float lightsourceShineSize = 1f;

		// Token: 0x0400097D RID: 2429
		public float lightsourceShineIntensity = 1f;

		// Token: 0x0400097E RID: 2430
		public bool lerpDarken;
	}
}
