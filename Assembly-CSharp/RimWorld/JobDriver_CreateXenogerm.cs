using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001248 RID: 4680
	public class JobDriver_CreateXenogerm : JobDriver
	{
		// Token: 0x170011D5 RID: 4565
		// (get) Token: 0x0600727C RID: 29308 RVA: 0x00234C4F File Offset: 0x00232E4F
		private Building_GeneAssembler Xenogerminator
		{
			get
			{
				return (Building_GeneAssembler)base.TargetThingA;
			}
		}

		// Token: 0x0600727D RID: 29309 RVA: 0x00234C5C File Offset: 0x00232E5C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Xenogerminator, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.ReserveSittableOrSpot(this.Xenogerminator.InteractionCell, this.job, errorOnFailed);
		}

		// Token: 0x0600727E RID: 29310 RVA: 0x00234CAB File Offset: 0x00232EAB
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOn(() => !this.Xenogerminator.CanBeWorkedOnNow.Accepted);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell, false);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.tickIntervalAction = delegate(int delta)
			{
				float workAmount = this.pawn.GetStatValue(StatDefOf.ResearchSpeed, true, -1) * this.Xenogerminator.GetStatValue(StatDefOf.AssemblySpeedFactor, true, -1) * (float)delta;
				this.Xenogerminator.DoWork(workAmount);
				this.pawn.skills.Learn(SkillDefOf.Intellectual, 0.1f * (float)delta, false, false);
				this.pawn.GainComfortFromCellIfPossible(delta, true);
				if (this.Xenogerminator.ProgressPercent >= 1f)
				{
					this.Xenogerminator.Finish();
					this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
					return;
				}
			};
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			toil.WithEffect(EffecterDefOf.GeneAssembler_Working, TargetIndex.A, null);
			toil.WithProgressBar(TargetIndex.A, () => this.Xenogerminator.ProgressPercent, false, 0f, false);
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.defaultDuration = 4000;
			toil.activeSkill = (() => SkillDefOf.Intellectual);
			yield return toil;
			yield break;
		}

		// Token: 0x04004F23 RID: 20259
		private const int JobEndInterval = 4000;
	}
}
