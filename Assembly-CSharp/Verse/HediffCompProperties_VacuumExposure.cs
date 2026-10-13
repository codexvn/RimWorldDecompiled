using System;

namespace Verse
{
	// Token: 0x02000718 RID: 1816
	public class HediffCompProperties_VacuumExposure : HediffCompProperties
	{
		// Token: 0x06002C56 RID: 11350 RVA: 0x000E6175 File Offset: 0x000E4375
		public HediffCompProperties_VacuumExposure()
		{
			this.compClass = typeof(HediffComp_VacuumExposure);
		}

		// Token: 0x0400238D RID: 9101
		public float severityPerSecondUnexposed;
	}
}
