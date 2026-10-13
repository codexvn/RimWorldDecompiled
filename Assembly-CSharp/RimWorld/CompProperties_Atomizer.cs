using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BF8 RID: 11256
	public class CompProperties_Atomizer : CompProperties_ThingContainer
	{
		// Token: 0x0600FC3A RID: 64570 RVA: 0x004927D4 File Offset: 0x004909D4
		public CompProperties_Atomizer()
		{
			this.compClass = typeof(CompAtomizer);
		}

		// Token: 0x0400A53B RID: 42299
		public ThingDef thingDef;

		// Token: 0x0400A53C RID: 42300
		public EffecterDef resolveEffecter;

		// Token: 0x0400A53D RID: 42301
		public EffecterDef workingEffecter;

		// Token: 0x0400A53E RID: 42302
		public SoundDef materialsAddedSound;

		// Token: 0x0400A53F RID: 42303
		public int resolveEffecterTicks = 40;

		// Token: 0x0400A540 RID: 42304
		public int ticksPerAtomize = 2500;

		// Token: 0x0400A541 RID: 42305
		public Vector3 contentsDrawOffset = Vector3.zero;
	}
}
