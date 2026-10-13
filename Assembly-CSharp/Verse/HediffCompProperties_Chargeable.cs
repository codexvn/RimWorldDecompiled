using System;

namespace Verse
{
	// Token: 0x02000699 RID: 1689
	public class HediffCompProperties_Chargeable : HediffCompProperties
	{
		// Token: 0x06002A83 RID: 10883 RVA: 0x000DFB57 File Offset: 0x000DDD57
		public HediffCompProperties_Chargeable()
		{
			this.compClass = typeof(HediffComp_Chargeable);
		}

		// Token: 0x04002273 RID: 8819
		public int ticksToFullCharge = -1;

		// Token: 0x04002274 RID: 8820
		public float initialCharge;

		// Token: 0x04002275 RID: 8821
		public float fullChargeAmount = 1f;

		// Token: 0x04002276 RID: 8822
		public float minChargeToActivate;

		// Token: 0x04002277 RID: 8823
		public string labelInBrackets;
	}
}
