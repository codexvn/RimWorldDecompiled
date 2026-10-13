using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D76 RID: 11638
	public class CompProperties_NoiseSource : CompProperties
	{
		// Token: 0x060105AF RID: 66991 RVA: 0x004B88F7 File Offset: 0x004B6AF7
		public CompProperties_NoiseSource()
		{
			this.compClass = typeof(CompNoiseSource);
		}

		// Token: 0x0400AAAC RID: 43692
		public float radius;
	}
}
