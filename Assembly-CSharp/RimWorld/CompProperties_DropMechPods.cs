using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002211 RID: 8721
	public class CompProperties_DropMechPods : CompProperties_EffectWithDest
	{
		// Token: 0x0600BED1 RID: 48849 RVA: 0x003757DD File Offset: 0x003739DD
		public CompProperties_DropMechPods()
		{
			this.compClass = typeof(CompAbilityEffect_DropMechPods);
		}

		// Token: 0x04008301 RID: 33537
		public IntRange numPods;
	}
}
