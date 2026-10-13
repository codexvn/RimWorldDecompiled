using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003861 RID: 14433
	public class CompProperties_HeatPusherEffecter : CompProperties
	{
		// Token: 0x06013F46 RID: 81734 RVA: 0x005EBCA5 File Offset: 0x005E9EA5
		public CompProperties_HeatPusherEffecter()
		{
			this.compClass = typeof(CompHeatPusherEffecter);
		}

		// Token: 0x0400DC58 RID: 56408
		public EffecterDef effecterDef;
	}
}
