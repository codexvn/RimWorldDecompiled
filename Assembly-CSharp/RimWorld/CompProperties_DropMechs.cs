using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002213 RID: 8723
	public class CompProperties_DropMechs : CompProperties_EffectWithDest
	{
		// Token: 0x0600BED6 RID: 48854 RVA: 0x003758AD File Offset: 0x00373AAD
		public CompProperties_DropMechs()
		{
			this.compClass = typeof(CompAbilityEffect_DropMechs);
		}

		// Token: 0x04008303 RID: 33539
		public FloatRange points;
	}
}
