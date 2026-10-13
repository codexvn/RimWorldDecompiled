using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200221D RID: 8733
	public class CompProperties_AbilityFarskip : CompProperties_AbilityEffect
	{
		// Token: 0x0600BEEC RID: 48876 RVA: 0x00375E82 File Offset: 0x00374082
		public CompProperties_AbilityFarskip()
		{
			this.compClass = typeof(CompAbilityEffect_Farskip);
		}

		// Token: 0x04008321 RID: 33569
		public IntRange stunTicks;
	}
}
