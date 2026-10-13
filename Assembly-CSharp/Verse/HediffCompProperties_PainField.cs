using System;

namespace Verse
{
	// Token: 0x020006E9 RID: 1769
	public class HediffCompProperties_PainField : HediffCompProperties
	{
		// Token: 0x06002BAC RID: 11180 RVA: 0x000E3DA0 File Offset: 0x000E1FA0
		public HediffCompProperties_PainField()
		{
			this.compClass = typeof(HediffComp_PainField);
		}

		// Token: 0x04002323 RID: 8995
		public float painDistance = 5.9f;

		// Token: 0x04002324 RID: 8996
		public float painInRange = 0.25f;

		// Token: 0x04002325 RID: 8997
		public SimpleCurve activityMultiplier;

		// Token: 0x04002326 RID: 8998
		public float psychicSensitivityMultiplier = 1f;

		// Token: 0x04002327 RID: 8999
		public float activatedMinimum = 0.2f;

		// Token: 0x04002328 RID: 9000
		public bool disableWhenSuppressed = true;
	}
}
