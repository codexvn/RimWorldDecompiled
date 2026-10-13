using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F52 RID: 12114
	public class CompProperties_UseEffectAddHediff : CompProperties_UseEffect
	{
		// Token: 0x0601114B RID: 69963 RVA: 0x004E7C9C File Offset: 0x004E5E9C
		public CompProperties_UseEffectAddHediff()
		{
			this.compClass = typeof(CompUseEffect_AddHediff);
		}

		// Token: 0x0400B159 RID: 45401
		public HediffDef hediffDef;

		// Token: 0x0400B15A RID: 45402
		public bool allowRepeatedUse;
	}
}
