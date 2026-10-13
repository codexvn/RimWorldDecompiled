using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200022A RID: 554
	public class DamageDefAdditionalHediff
	{
		// Token: 0x04000964 RID: 2404
		public HediffDef hediff;

		// Token: 0x04000965 RID: 2405
		public float severityPerDamageDealt = 0.1f;

		// Token: 0x04000966 RID: 2406
		public float severityFixed;

		// Token: 0x04000967 RID: 2407
		public StatDef victimSeverityScaling;

		// Token: 0x04000968 RID: 2408
		public bool inverseStatScaling;

		// Token: 0x04000969 RID: 2409
		public bool victimSeverityScalingByInvBodySize;
	}
}
