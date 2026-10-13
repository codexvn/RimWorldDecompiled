using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C61 RID: 11361
	public class CompProperties_DisruptorFlare : CompProperties
	{
		// Token: 0x0600FEC8 RID: 65224 RVA: 0x0049D71F File Offset: 0x0049B91F
		public CompProperties_DisruptorFlare()
		{
			this.compClass = typeof(CompDisruptorFlare);
		}

		// Token: 0x0400A6E1 RID: 42721
		public float radius;

		// Token: 0x0400A6E2 RID: 42722
		public EffecterDef destroyWarningEffecterDef;

		// Token: 0x0400A6E3 RID: 42723
		public EffecterDef effecterDef;
	}
}
