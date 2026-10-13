using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002EF7 RID: 12023
	public class CompProperties_GrayStatueGas : CompProperties_GrayStatue
	{
		// Token: 0x06010F9B RID: 69531 RVA: 0x004E2B4A File Offset: 0x004E0D4A
		public CompProperties_GrayStatueGas()
		{
			this.compClass = typeof(CompGrayStatueGas);
		}

		// Token: 0x0400B08F RID: 45199
		public GasType gas = GasType.DeadlifeDust;
	}
}
