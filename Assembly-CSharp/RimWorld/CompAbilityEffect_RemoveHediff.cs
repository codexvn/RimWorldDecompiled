using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200226B RID: 8811
	public class CompAbilityEffect_RemoveHediff : CompAbilityEffect
	{
		// Token: 0x17001D57 RID: 7511
		// (get) Token: 0x0600C00C RID: 49164 RVA: 0x0037A605 File Offset: 0x00378805
		public new CompProperties_AbilityRemoveHediff Props
		{
			get
			{
				return (CompProperties_AbilityRemoveHediff)this.props;
			}
		}

		// Token: 0x0600C00D RID: 49165 RVA: 0x0037A614 File Offset: 0x00378814
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			if (this.Props.applyToSelf)
			{
				this.RemoveHediff(this.parent.pawn);
			}
			if (target.Pawn != null && this.Props.applyToTarget && target.Pawn != this.parent.pawn)
			{
				this.RemoveHediff(target.Pawn);
			}
		}

		// Token: 0x0600C00E RID: 49166 RVA: 0x0037A680 File Offset: 0x00378880
		private void RemoveHediff(Pawn pawn)
		{
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(this.Props.hediffDef, false);
			if (firstHediffOfDef != null)
			{
				pawn.health.RemoveHediff(firstHediffOfDef);
			}
		}
	}
}
