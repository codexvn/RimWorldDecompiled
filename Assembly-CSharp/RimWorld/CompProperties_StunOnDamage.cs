using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E6A RID: 11882
	public class CompProperties_StunOnDamage : CompProperties
	{
		// Token: 0x06010BBD RID: 68541 RVA: 0x004D2EDA File Offset: 0x004D10DA
		public CompProperties_StunOnDamage()
		{
			this.compClass = typeof(CompStunOnDamage);
		}

		// Token: 0x0400AE69 RID: 44649
		public int delayTicks;

		// Token: 0x0400AE6A RID: 44650
		public DamageDef damage;
	}
}
