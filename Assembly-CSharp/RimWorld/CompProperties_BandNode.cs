using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C04 RID: 11268
	public class CompProperties_BandNode : CompProperties
	{
		// Token: 0x0600FC7C RID: 64636 RVA: 0x004937CF File Offset: 0x004919CF
		public CompProperties_BandNode()
		{
			this.compClass = typeof(CompBandNode);
		}

		// Token: 0x0400A577 RID: 42359
		public HediffDef hediff;

		// Token: 0x0400A578 RID: 42360
		public float retuneDays = 3f;

		// Token: 0x0400A579 RID: 42361
		public float tuneSeconds = 5f;

		// Token: 0x0400A57A RID: 42362
		public int powerConsumptionIdle = 100;

		// Token: 0x0400A57B RID: 42363
		public int emissionInterval;

		// Token: 0x0400A57C RID: 42364
		public EffecterDef untunedEffect;

		// Token: 0x0400A57D RID: 42365
		public EffecterDef tuningEffect;

		// Token: 0x0400A57E RID: 42366
		public EffecterDef tunedEffect;

		// Token: 0x0400A57F RID: 42367
		public EffecterDef retuningEffect;

		// Token: 0x0400A580 RID: 42368
		public SoundDef tuningCompleteSound;
	}
}
