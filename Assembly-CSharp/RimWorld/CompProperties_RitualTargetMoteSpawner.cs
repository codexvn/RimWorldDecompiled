using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F4D RID: 12109
	public class CompProperties_RitualTargetMoteSpawner : CompProperties
	{
		// Token: 0x06011136 RID: 69942 RVA: 0x004E7951 File Offset: 0x004E5B51
		public CompProperties_RitualTargetMoteSpawner()
		{
			this.compClass = typeof(CompRitualTargetMoteSpawner);
		}

		// Token: 0x0400B152 RID: 45394
		public ThingDef mote;
	}
}
