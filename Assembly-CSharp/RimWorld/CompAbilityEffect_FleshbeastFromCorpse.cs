using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002235 RID: 8757
	public class CompAbilityEffect_FleshbeastFromCorpse : CompAbilityEffect
	{
		// Token: 0x17001D37 RID: 7479
		// (get) Token: 0x0600BF5C RID: 48988 RVA: 0x00377DA3 File Offset: 0x00375FA3
		public new CompProperties_FleshbeastFromCorpse Props
		{
			get
			{
				return (CompProperties_FleshbeastFromCorpse)this.props;
			}
		}

		// Token: 0x0600BF5D RID: 48989 RVA: 0x00377DB0 File Offset: 0x00375FB0
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Corpse corpse = target.Thing as Corpse;
			if (corpse == null || corpse.InnerPawn == null)
			{
				return;
			}
			FleshbeastUtility.SpawnFleshbeastFromPawn(corpse.InnerPawn, false, true, Array.Empty<PawnKindDef>());
		}

		// Token: 0x0600BF5E RID: 48990 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AICanTargetNow(LocalTargetInfo target)
		{
			return false;
		}
	}
}
