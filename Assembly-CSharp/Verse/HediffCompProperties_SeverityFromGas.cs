using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000703 RID: 1795
	public class HediffCompProperties_SeverityFromGas : HediffCompProperties
	{
		// Token: 0x06002BF2 RID: 11250 RVA: 0x000E4B41 File Offset: 0x000E2D41
		public HediffCompProperties_SeverityFromGas()
		{
			this.compClass = typeof(HediffComp_SeverityFromGas);
		}

		// Token: 0x04002356 RID: 9046
		public GasType gasType;

		// Token: 0x04002357 RID: 9047
		public float severityGasDensityFactor;

		// Token: 0x04002358 RID: 9048
		public int intervalTicks = 60;

		// Token: 0x04002359 RID: 9049
		public float severityNotExposed;

		// Token: 0x0400235A RID: 9050
		public StatDef exposureStatFactor;
	}
}
