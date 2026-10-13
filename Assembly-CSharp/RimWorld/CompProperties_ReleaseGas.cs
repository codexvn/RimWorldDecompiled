using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003863 RID: 14435
	public class CompProperties_ReleaseGas : CompProperties
	{
		// Token: 0x06013F4C RID: 81740 RVA: 0x005EBD87 File Offset: 0x005E9F87
		public CompProperties_ReleaseGas()
		{
			this.compClass = typeof(CompReleaseGas);
		}

		// Token: 0x0400DC5B RID: 56411
		public GasType gasType;

		// Token: 0x0400DC5C RID: 56412
		public float cellsToFill;

		// Token: 0x0400DC5D RID: 56413
		public float durationSeconds;

		// Token: 0x0400DC5E RID: 56414
		public EffecterDef effecterReleasing;
	}
}
