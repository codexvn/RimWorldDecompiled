using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C85 RID: 11397
	public class CompProperties_FadesInOut : CompProperties
	{
		// Token: 0x0600FFD9 RID: 65497 RVA: 0x004A1FA5 File Offset: 0x004A01A5
		public CompProperties_FadesInOut()
		{
			this.compClass = typeof(CompFadesInOut);
		}

		// Token: 0x0400A750 RID: 42832
		public float fadeInSecs;

		// Token: 0x0400A751 RID: 42833
		public float fadeOutSecs;

		// Token: 0x0400A752 RID: 42834
		public float solidTimeSecs;
	}
}
