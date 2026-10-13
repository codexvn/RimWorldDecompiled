using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001964 RID: 6500
	public class CompProperties_EffecterOnDeath : CompProperties
	{
		// Token: 0x060093A6 RID: 37798 RVA: 0x002B6EC4 File Offset: 0x002B50C4
		public CompProperties_EffecterOnDeath()
		{
			this.compClass = typeof(EffecterOnDeath);
		}

		// Token: 0x04005F00 RID: 24320
		public EffecterDef effecterDef;
	}
}
