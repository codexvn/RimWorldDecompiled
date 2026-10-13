using System;

namespace Verse
{
	// Token: 0x02000725 RID: 1829
	public class HediffCompProperties_SeverityPerSecond : HediffCompProperties
	{
		// Token: 0x06002C8C RID: 11404 RVA: 0x000E6D08 File Offset: 0x000E4F08
		public HediffCompProperties_SeverityPerSecond()
		{
			this.compClass = typeof(HediffComp_SeverityPerSecond);
		}

		// Token: 0x06002C8D RID: 11405 RVA: 0x000E6D2B File Offset: 0x000E4F2B
		public float CalculateSeverityPerSecond()
		{
			return this.severityPerSecond + this.severityPerSecondRange.RandomInRange;
		}

		// Token: 0x040023AC RID: 9132
		public float severityPerSecond;

		// Token: 0x040023AD RID: 9133
		public FloatRange severityPerSecondRange = FloatRange.Zero;
	}
}
