using System;

namespace Verse
{
	// Token: 0x02000709 RID: 1801
	public class HediffCompProperties_SeverityFromHemogen : HediffCompProperties
	{
		// Token: 0x06002C0A RID: 11274 RVA: 0x000E5018 File Offset: 0x000E3218
		public HediffCompProperties_SeverityFromHemogen()
		{
			this.compClass = typeof(HediffComp_SeverityFromHemogen);
		}

		// Token: 0x04002365 RID: 9061
		public float severityPerHourEmpty;

		// Token: 0x04002366 RID: 9062
		public float severityPerHourHemogen;
	}
}
