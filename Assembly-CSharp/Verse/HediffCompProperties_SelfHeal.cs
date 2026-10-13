using System;

namespace Verse
{
	// Token: 0x020006FF RID: 1791
	public class HediffCompProperties_SelfHeal : HediffCompProperties
	{
		// Token: 0x06002BE8 RID: 11240 RVA: 0x000E4A5C File Offset: 0x000E2C5C
		public HediffCompProperties_SelfHeal()
		{
			this.compClass = typeof(HediffComp_SelfHeal);
		}

		// Token: 0x04002353 RID: 9043
		public int healIntervalTicksStanding = 50;

		// Token: 0x04002354 RID: 9044
		public float healAmount = 1f;
	}
}
