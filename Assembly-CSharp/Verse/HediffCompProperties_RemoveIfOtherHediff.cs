using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020006F9 RID: 1785
	public class HediffCompProperties_RemoveIfOtherHediff : HediffCompProperties_MessageBase
	{
		// Token: 0x06002BD7 RID: 11223 RVA: 0x000E454C File Offset: 0x000E274C
		public HediffCompProperties_RemoveIfOtherHediff()
		{
			this.compClass = typeof(HediffComp_RemoveIfOtherHediff);
		}

		// Token: 0x0400233F RID: 9023
		public List<HediffDef> hediffs = new List<HediffDef>();

		// Token: 0x04002340 RID: 9024
		public IntRange? stages;

		// Token: 0x04002341 RID: 9025
		public int mtbHours;
	}
}
