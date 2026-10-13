using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F2C RID: 12076
	public class CompProperties_TargetEffect_BrainDamageChance : CompProperties
	{
		// Token: 0x060110C8 RID: 69832 RVA: 0x004E679C File Offset: 0x004E499C
		public CompProperties_TargetEffect_BrainDamageChance()
		{
			this.compClass = typeof(CompTargetEffect_BrainDamageChance);
		}

		// Token: 0x0400B11E RID: 45342
		public float brainDamageChance = 0.3f;
	}
}
