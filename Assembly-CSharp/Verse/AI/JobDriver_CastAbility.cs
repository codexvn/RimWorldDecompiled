using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000E98 RID: 3736
	public class JobDriver_CastAbility : JobDriver_CastVerbOnce
	{
		// Token: 0x06005E29 RID: 24105 RVA: 0x001DFE87 File Offset: 0x001DE087
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOn(() => !this.job.ability.CanCast && !this.job.ability.Casting);
			base.AddFinishAction(delegate(JobCondition condition)
			{
				if (this.job.ability != null && this.job.def.abilityCasting)
				{
					this.job.ability.StartCooldown(this.job.ability.def.cooldownTicksRange.RandomInRange);
				}
			});
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				this.pawn.pather.StopDead();
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil;
			Toil toil2 = Toils_Combat.CastVerb(TargetIndex.A, TargetIndex.B, false);
			if (this.job.ability != null && this.job.ability.def.showCastingProgressBar && this.job.verbToUse != null)
			{
				toil2.WithProgressBar(TargetIndex.A, () => this.job.verbToUse.WarmupProgress, false, -0.5f, false);
			}
			yield return toil2;
			yield break;
		}

		// Token: 0x06005E2A RID: 24106 RVA: 0x0003A9D8 File Offset: 0x00038BD8
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			Ability ability = this.job.ability;
			if (ability == null)
			{
				return;
			}
			ability.Notify_StartedCasting();
		}

		// Token: 0x06005E2B RID: 24107 RVA: 0x001DFE98 File Offset: 0x001DE098
		public override string GetReport()
		{
			string text;
			if (this.job.ability != null && !this.job.ability.def.targetRequired)
			{
				text = "UsingVerbNoTarget".Translate(this.job.verbToUse.ReportLabel);
			}
			else
			{
				text = base.GetReport();
			}
			if (this.job.ability != null && this.job.ability.def.showCastingProgressBar)
			{
				text += " " + "DurationLeft".Translate(this.job.verbToUse.WarmupTicksLeft.ToStringSecondsFromTicks()) + ".";
			}
			return text;
		}
	}
}
