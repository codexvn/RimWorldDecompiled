using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020022AA RID: 8874
	public class CompProperties_EffectWithDest : CompProperties_AbilityEffect
	{
		// Token: 0x04008415 RID: 33813
		public AbilityEffectDestination destination;

		// Token: 0x04008416 RID: 33814
		public bool requiresLineOfSight;

		// Token: 0x04008417 RID: 33815
		public float range;

		// Token: 0x04008418 RID: 33816
		public FloatRange randomRange;

		// Token: 0x04008419 RID: 33817
		public ClamorDef destClamorType;

		// Token: 0x0400841A RID: 33818
		public int destClamorRadius;
	}
}
