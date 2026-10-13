using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200071F RID: 1823
	public class HediffCompProperties_RandomizeSeverityPhases : HediffCompProperties_Randomizer
	{
		// Token: 0x06002C73 RID: 11379 RVA: 0x000E6723 File Offset: 0x000E4923
		public HediffCompProperties_RandomizeSeverityPhases()
		{
			this.compClass = typeof(HediffComp_RandomizeSeverityPhases);
		}

		// Token: 0x0400239C RID: 9116
		public List<HediffCompProperties_RandomizeSeverityPhases.Phase> phases;

		// Token: 0x0400239D RID: 9117
		[MustTranslate]
		public string notifyMessage;

		// Token: 0x02000720 RID: 1824
		public class Phase
		{
			// Token: 0x0400239E RID: 9118
			public HediffCompProperties comp;

			// Token: 0x0400239F RID: 9119
			[MustTranslate]
			public string labelPrefix;

			// Token: 0x040023A0 RID: 9120
			[MustTranslate]
			public string descriptionExtra;

			// Token: 0x040023A1 RID: 9121
			public float severityPerDay;
		}
	}
}
