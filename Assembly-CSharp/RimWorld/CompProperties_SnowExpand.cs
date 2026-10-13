using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001985 RID: 6533
	public class CompProperties_SnowExpand : CompProperties
	{
		// Token: 0x0600943A RID: 37946 RVA: 0x002B8BFE File Offset: 0x002B6DFE
		public CompProperties_SnowExpand()
		{
			this.compClass = typeof(CompSnowExpand);
		}

		// Token: 0x04005FBD RID: 24509
		public int expandInterval = 500;

		// Token: 0x04005FBE RID: 24510
		public float addAmount = 0.12f;

		// Token: 0x04005FBF RID: 24511
		public float maxRadius = 55f;
	}
}
