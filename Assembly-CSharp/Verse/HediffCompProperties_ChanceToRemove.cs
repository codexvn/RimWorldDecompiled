using System;

namespace Verse
{
	// Token: 0x02000691 RID: 1681
	public class HediffCompProperties_ChanceToRemove : HediffCompProperties
	{
		// Token: 0x06002A66 RID: 10854 RVA: 0x000DF594 File Offset: 0x000DD794
		public HediffCompProperties_ChanceToRemove()
		{
			this.compClass = typeof(HediffComp_ChanceToRemove);
		}

		// Token: 0x0400225F RID: 8799
		public int intervalTicks;

		// Token: 0x04002260 RID: 8800
		public float chance;
	}
}
