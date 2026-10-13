using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F1C RID: 12060
	public class CompProperties_SkyfallerRandomizeDirection : CompProperties
	{
		// Token: 0x06011069 RID: 69737 RVA: 0x004E5B63 File Offset: 0x004E3D63
		public CompProperties_SkyfallerRandomizeDirection()
		{
			this.compClass = typeof(CompSkyfallerRandomizeDirection);
		}

		// Token: 0x0400B0F1 RID: 45297
		public IntRange directionChangeInterval;

		// Token: 0x0400B0F2 RID: 45298
		public float maxDeviationFromStartingAngle;
	}
}
