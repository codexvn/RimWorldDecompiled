using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E08 RID: 11784
	public class CompProperties_Shield : CompProperties
	{
		// Token: 0x0601094F RID: 67919 RVA: 0x004C768C File Offset: 0x004C588C
		public CompProperties_Shield()
		{
			this.compClass = typeof(CompShield);
		}

		// Token: 0x0400ACD1 RID: 44241
		public int startingTicksToReset = 3200;

		// Token: 0x0400ACD2 RID: 44242
		public float minDrawSize = 1.2f;

		// Token: 0x0400ACD3 RID: 44243
		public float maxDrawSize = 1.55f;

		// Token: 0x0400ACD4 RID: 44244
		public float energyLossPerDamage = 0.033f;

		// Token: 0x0400ACD5 RID: 44245
		public float energyOnReset = 0.2f;

		// Token: 0x0400ACD6 RID: 44246
		public bool blocksRangedWeapons = true;
	}
}
