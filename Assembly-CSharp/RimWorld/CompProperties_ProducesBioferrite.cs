using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DB8 RID: 11704
	public class CompProperties_ProducesBioferrite : CompProperties
	{
		// Token: 0x06010740 RID: 67392 RVA: 0x004BEED1 File Offset: 0x004BD0D1
		public CompProperties_ProducesBioferrite()
		{
			this.compClass = typeof(CompProducesBioferrite);
		}

		// Token: 0x0400AB98 RID: 43928
		public float bioferriteDensity = 1f;
	}
}
