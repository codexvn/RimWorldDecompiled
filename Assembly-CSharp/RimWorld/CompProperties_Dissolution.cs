using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002ED7 RID: 11991
	public class CompProperties_Dissolution : CompProperties
	{
		// Token: 0x06010EE3 RID: 69347 RVA: 0x004DF6F3 File Offset: 0x004DD8F3
		public CompProperties_Dissolution()
		{
			this.compClass = typeof(CompDissolution);
		}

		// Token: 0x0400B01B RID: 45083
		public int dissolutionAfterDays = 4;

		// Token: 0x0400B01C RID: 45084
		public float dissolutinFactorIndoors = 0.5f;

		// Token: 0x0400B01D RID: 45085
		public float dissolutionFactorRain = 2f;
	}
}
