using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020021F1 RID: 8689
	public class CompProperties_AbilityConnectingFleckLine : CompProperties_AbilityEffect
	{
		// Token: 0x0600BE5C RID: 48732 RVA: 0x003733DF File Offset: 0x003715DF
		public CompProperties_AbilityConnectingFleckLine()
		{
			this.compClass = typeof(CompAbilityEffect_ConnectingFleckLine);
		}

		// Token: 0x040082CC RID: 33484
		public FleckDef fleckDef;
	}
}
