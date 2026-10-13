using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002239 RID: 8761
	public class CompAbilityEffect_GiveHediff : CompAbilityEffect_WithDuration
	{
		// Token: 0x17001D39 RID: 7481
		// (get) Token: 0x0600BF65 RID: 48997 RVA: 0x00377F17 File Offset: 0x00376117
		public new CompProperties_AbilityGiveHediff Props
		{
			get
			{
				return (CompProperties_AbilityGiveHediff)this.props;
			}
		}

		// Token: 0x0600BF66 RID: 48998 RVA: 0x00377F24 File Offset: 0x00376124
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			if (this.Props.ignoreSelf && target.Pawn == this.parent.pawn)
			{
				return;
			}
			if (!this.Props.onlyApplyToSelf && this.Props.applyToTarget)
			{
				this.ApplyInner(target.Pawn, this.parent.pawn);
			}
			if (this.Props.applyToSelf || this.Props.onlyApplyToSelf)
			{
				this.ApplyInner(this.parent.pawn, target.Pawn);
			}
		}

		// Token: 0x0600BF67 RID: 48999 RVA: 0x00377FC0 File Offset: 0x003761C0
		protected void ApplyInner(Pawn target, Pawn other)
		{
			if (target != null)
			{
				if (this.TryResist(target))
				{
					MoteMaker.ThrowText(target.DrawPos, target.Map, "Resisted".Translate(), -1f);
					return;
				}
				if (this.Props.replaceExisting)
				{
					Hediff firstHediffOfDef = target.health.hediffSet.GetFirstHediffOfDef(this.Props.hediffDef, false);
					if (firstHediffOfDef != null)
					{
						target.health.RemoveHediff(firstHediffOfDef);
					}
				}
				Hediff hediff = HediffMaker.MakeHediff(this.Props.hediffDef, target, this.Props.onlyBrain ? target.health.hediffSet.GetBrain() : null);
				HediffComp_Disappears hediffComp_Disappears = hediff.TryGetComp<HediffComp_Disappears>();
				if (hediffComp_Disappears != null)
				{
					hediffComp_Disappears.ticksToDisappear = base.GetDurationSeconds(target).SecondsToTicks();
				}
				if (this.Props.severity >= 0f)
				{
					hediff.Severity = this.Props.severity;
				}
				HediffComp_Link hediffComp_Link = hediff.TryGetComp<HediffComp_Link>();
				if (hediffComp_Link != null)
				{
					hediffComp_Link.other = other;
					hediffComp_Link.drawConnection = (target == this.parent.pawn);
				}
				target.health.AddHediff(hediff, null, null, null);
			}
		}

		// Token: 0x0600BF68 RID: 49000 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual bool TryResist(Pawn pawn)
		{
			return false;
		}

		// Token: 0x0600BF69 RID: 49001 RVA: 0x003780E9 File Offset: 0x003762E9
		public override bool AICanTargetNow(LocalTargetInfo target)
		{
			return this.parent.pawn.Faction != Faction.OfPlayer && target.Pawn != null;
		}
	}
}
