using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003857 RID: 14423
	public class HediffCompProperties_PsychicHarmonizer : HediffCompProperties
	{
		// Token: 0x06013F17 RID: 81687 RVA: 0x005EACC0 File Offset: 0x005E8EC0
		public HediffCompProperties_PsychicHarmonizer()
		{
			this.compClass = typeof(HediffComp_PsychicHarmonizer);
		}

		// Token: 0x0400DC41 RID: 56385
		public float range;

		// Token: 0x0400DC42 RID: 56386
		public ThoughtDef thought;
	}
}
