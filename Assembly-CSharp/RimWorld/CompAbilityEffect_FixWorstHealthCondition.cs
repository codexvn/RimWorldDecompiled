using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200222D RID: 8749
	public class CompAbilityEffect_FixWorstHealthCondition : CompAbilityEffect
	{
		// Token: 0x17001D33 RID: 7475
		// (get) Token: 0x0600BF3F RID: 48959 RVA: 0x00377697 File Offset: 0x00375897
		public new CompProperties_AbilityFixWorstHealthCondition Props
		{
			get
			{
				return (CompProperties_AbilityFixWorstHealthCondition)this.props;
			}
		}

		// Token: 0x0600BF40 RID: 48960 RVA: 0x003776A4 File Offset: 0x003758A4
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (target.Pawn == null)
			{
				return false;
			}
			return target.Pawn.health.hediffSet.hediffs.Any((Hediff x) => x.def.isBad && x.def.everCurableByItem && x.Visible) && base.CanApplyOn(target, dest);
		}

		// Token: 0x0600BF41 RID: 48961 RVA: 0x00377704 File Offset: 0x00375904
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			TaggedString value = HealthUtility.FixWorstHealthCondition(target.Pawn, Array.Empty<HediffDef>());
			if (base.SendLetter)
			{
				Find.LetterStack.ReceiveLetter(this.Props.customLetterLabel, this.Props.customLetterText.Formatted(this.parent.pawn, target.Pawn, value), LetterDefOf.PositiveEvent, new LookTargets(target.Pawn), null, null, null, null, 0, true);
			}
		}
	}
}
