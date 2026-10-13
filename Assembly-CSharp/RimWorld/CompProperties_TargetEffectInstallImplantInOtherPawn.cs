using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F34 RID: 12084
	public class CompProperties_TargetEffectInstallImplantInOtherPawn : CompProperties
	{
		// Token: 0x060110D8 RID: 69848 RVA: 0x004E699E File Offset: 0x004E4B9E
		public CompProperties_TargetEffectInstallImplantInOtherPawn()
		{
			this.compClass = typeof(CompTargetEffect_InstallImplantInOtherPawn);
		}

		// Token: 0x0400B122 RID: 45346
		public HediffDef hediffDef;

		// Token: 0x0400B123 RID: 45347
		public BodyPartDef bodyPart;

		// Token: 0x0400B124 RID: 45348
		public bool canUpgrade;

		// Token: 0x0400B125 RID: 45349
		public bool requiresExistingHediff;

		// Token: 0x0400B126 RID: 45350
		public SoundDef soundOnUsed;
	}
}
