using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200223E RID: 8766
	public class CompProperties_AbilityGiveMentalState : CompProperties_AbilityEffect
	{
		// Token: 0x0400835A RID: 33626
		public MentalStateDef stateDef;

		// Token: 0x0400835B RID: 33627
		public MentalStateDef stateDefForMechs;

		// Token: 0x0400835C RID: 33628
		public StatDef durationMultiplier;

		// Token: 0x0400835D RID: 33629
		public bool applyToSelf;

		// Token: 0x0400835E RID: 33630
		public EffecterDef casterEffect;

		// Token: 0x0400835F RID: 33631
		public EffecterDef targetEffect;

		// Token: 0x04008360 RID: 33632
		public bool excludeNPCFactions;

		// Token: 0x04008361 RID: 33633
		public bool forced;
	}
}
