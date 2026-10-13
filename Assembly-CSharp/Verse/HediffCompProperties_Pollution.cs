using System;

namespace Verse
{
	// Token: 0x020006EB RID: 1771
	public class HediffCompProperties_Pollution : HediffCompProperties
	{
		// Token: 0x06002BAF RID: 11183 RVA: 0x000E3E03 File Offset: 0x000E2003
		public HediffCompProperties_Pollution()
		{
			this.compClass = typeof(HediffComp_Pollution);
		}

		// Token: 0x04002329 RID: 9001
		public float pollutedSeverity;

		// Token: 0x0400232A RID: 9002
		public float unpollutedSeverity;

		// Token: 0x0400232B RID: 9003
		public int interval = 1;
	}
}
