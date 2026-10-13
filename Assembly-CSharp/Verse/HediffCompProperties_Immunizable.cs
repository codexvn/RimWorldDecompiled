using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200071B RID: 1819
	public class HediffCompProperties_Immunizable : HediffCompProperties
	{
		// Token: 0x06002C5D RID: 11357 RVA: 0x000E6244 File Offset: 0x000E4444
		public HediffCompProperties_Immunizable()
		{
			this.compClass = typeof(HediffComp_Immunizable);
		}

		// Token: 0x04002391 RID: 9105
		public float immunityPerDayNotSick;

		// Token: 0x04002392 RID: 9106
		public float immunityPerDaySick;

		// Token: 0x04002393 RID: 9107
		public float severityPerDayNotImmune;

		// Token: 0x04002394 RID: 9108
		public float severityPerDayImmune;

		// Token: 0x04002395 RID: 9109
		public FloatRange severityPerDayNotImmuneRandomFactor = new FloatRange(1f, 1f);

		// Token: 0x04002396 RID: 9110
		public List<HediffDefFactor> severityFactorsFromHediffs = new List<HediffDefFactor>();

		// Token: 0x04002397 RID: 9111
		public bool hidden;
	}
}
