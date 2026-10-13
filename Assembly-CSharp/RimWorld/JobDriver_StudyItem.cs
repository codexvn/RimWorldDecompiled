using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200131F RID: 4895
	public class JobDriver_StudyItem : JobDriver
	{
		// Token: 0x170012ED RID: 4845
		// (get) Token: 0x0600782C RID: 30764 RVA: 0x00229653 File Offset: 0x00227853
		protected Thing ThingToStudy
		{
			get
			{
				return base.TargetThingA;
			}
		}

		// Token: 0x170012EE RID: 4846
		// (get) Token: 0x0600782D RID: 30765 RVA: 0x00246D5B File Offset: 0x00244F5B
		protected bool CanStudyInPlace
		{
			get
			{
				CompAnalyzable compAnalyzable = this.ThingToStudy.TryGetComp<CompAnalyzable>();
				return compAnalyzable != null && compAnalyzable.Props.canStudyInPlace;
			}
		}

		// Token: 0x0600782E RID: 30766 RVA: 0x00246D78 File Offset: 0x00244F78
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (!this.pawn.Reserve(this.ThingToStudy, this.job, 1, -1, null, errorOnFailed, false))
			{
				return false;
			}
			if (!this.CanStudyInPlace)
			{
				if (!this.pawn.Reserve(base.TargetB, this.job, 1, -1, null, errorOnFailed, false))
				{
					return false;
				}
				if (!this.pawn.Reserve(base.TargetC, this.job, 1, -1, null, errorOnFailed, false))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600782F RID: 30767 RVA: 0x00246DF3 File Offset: 0x00244FF3
		protected override IEnumerable<Toil> MakeNewToils()
		{
			bool requiresResearchBench = !this.CanStudyInPlace;
			if (requiresResearchBench)
			{
				this.FailOnDespawnedNullOrForbidden(TargetIndex.B);
				this.FailOnBurningImmobile(TargetIndex.B);
				yield return Toils_General.DoAtomic(delegate
				{
					this.job.count = 1;
				});
			}
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, false).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			if (requiresResearchBench)
			{
				yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, true, false, true, false).FailOnDestroyedNullOrForbidden(TargetIndex.A);
				yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.InteractionCell, false);
				yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, null, false, false);
			}
			foreach (Toil toil in this.GetStudyToils())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06007830 RID: 30768 RVA: 0x00246E03 File Offset: 0x00245003
		protected virtual IEnumerable<Toil> GetStudyToils()
		{
			Toil study = ToilMaker.MakeToil("GetStudyToils");
			CompStudiable comp = this.ThingToStudy.TryGetComp<CompStudiable>();
			study.tickIntervalAction = delegate(int delta)
			{
				Pawn actor = study.actor;
				float num = 0.08f;
				if (!actor.WorkTypeIsDisabled(WorkTypeDefOf.Research))
				{
					num = actor.GetStatValue(StatDefOf.ResearchSpeed, true, -1);
				}
				num *= this.TargetThingA.GetStatValue(StatDefOf.ResearchSpeedFactor, true, -1) * (float)delta;
				comp.Study(actor, num, 0f);
				if (comp.Completed)
				{
					this.pawn.jobs.curDriver.ReadyForNextToil();
				}
			};
			study.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			study.WithProgressBar(TargetIndex.A, () => comp.ProgressPercent, false, -0.5f, false);
			study.defaultCompleteMode = ToilCompleteMode.Delay;
			study.defaultDuration = 4000;
			study.activeSkill = (() => SkillDefOf.Intellectual);
			yield return study;
			yield break;
		}

		// Token: 0x040051C2 RID: 20930
		private const int JobEndInterval = 4000;

		// Token: 0x040051C3 RID: 20931
		protected const TargetIndex ThingToStudyIndex = TargetIndex.A;

		// Token: 0x040051C4 RID: 20932
		protected const TargetIndex ResearchBenchInd = TargetIndex.B;

		// Token: 0x040051C5 RID: 20933
		protected const TargetIndex HaulCell = TargetIndex.C;
	}
}
