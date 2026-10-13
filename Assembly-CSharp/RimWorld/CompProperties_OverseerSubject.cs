using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D92 RID: 11666
	public class CompProperties_OverseerSubject : CompProperties
	{
		// Token: 0x06010678 RID: 67192 RVA: 0x004BBC9B File Offset: 0x004B9E9B
		public CompProperties_OverseerSubject()
		{
			this.compClass = typeof(CompOverseerSubject);
		}

		// Token: 0x0400AB2A RID: 43818
		public EffecterDef needsOverseerEffect;

		// Token: 0x0400AB2B RID: 43819
		public int delayUntilFeralCheck = 60000;

		// Token: 0x0400AB2C RID: 43820
		public int feralMtbDays = 10;

		// Token: 0x0400AB2D RID: 43821
		public int feralCascadeRadialDistance = 25;
	}
}
