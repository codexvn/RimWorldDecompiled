using System;

namespace Verse
{
	// Token: 0x020006F3 RID: 1779
	public class HediffCompProperties_ReactOnDamage : HediffCompProperties
	{
		// Token: 0x06002BC8 RID: 11208 RVA: 0x000E433C File Offset: 0x000E253C
		public HediffCompProperties_ReactOnDamage()
		{
			this.compClass = typeof(HediffComp_ReactOnDamage);
		}

		// Token: 0x04002339 RID: 9017
		public DamageDef damageDefIncoming;

		// Token: 0x0400233A RID: 9018
		public BodyPartDef createHediffOn;

		// Token: 0x0400233B RID: 9019
		public HediffDef createHediff;

		// Token: 0x0400233C RID: 9020
		public bool vomit;
	}
}
