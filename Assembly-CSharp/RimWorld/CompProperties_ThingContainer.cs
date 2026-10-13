using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E8A RID: 11914
	public class CompProperties_ThingContainer : CompProperties
	{
		// Token: 0x0400AEB4 RID: 44724
		public int stackLimit;

		// Token: 0x0400AEB5 RID: 44725
		public int minCountToEmpty;

		// Token: 0x0400AEB6 RID: 44726
		public IntVec3 containedThingOffset;

		// Token: 0x0400AEB7 RID: 44727
		public bool drawStackLabel;

		// Token: 0x0400AEB8 RID: 44728
		public bool drawContainedThing = true;

		// Token: 0x0400AEB9 RID: 44729
		public EffecterDef dropEffecterDef;
	}
}
