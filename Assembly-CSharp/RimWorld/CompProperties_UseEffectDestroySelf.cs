using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F5A RID: 12122
	public class CompProperties_UseEffectDestroySelf : CompProperties_UseEffect
	{
		// Token: 0x06011168 RID: 69992 RVA: 0x004E82C2 File Offset: 0x004E64C2
		public CompProperties_UseEffectDestroySelf()
		{
			this.compClass = typeof(CompUseEffect_DestroySelf);
		}

		// Token: 0x0400B169 RID: 45417
		public int delayTicks = -1;

		// Token: 0x0400B16A RID: 45418
		public float orderPriority = -1000f;

		// Token: 0x0400B16B RID: 45419
		public EffecterDef effecterDef;

		// Token: 0x0400B16C RID: 45420
		public bool spawnKilledLeavings;

		// Token: 0x0400B16D RID: 45421
		public List<ThingDefCountClass> leavings;
	}
}
