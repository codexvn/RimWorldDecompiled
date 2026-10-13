using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012FF RID: 4863
	public class JobDriver_Research : JobDriver
	{
		// Token: 0x170012C1 RID: 4801
		// (get) Token: 0x0600774B RID: 30539 RVA: 0x00244093 File Offset: 0x00242293
		private ResearchProjectDef Project
		{
			get
			{
				return Find.ResearchManager.GetProject(null);
			}
		}

		// Token: 0x170012C2 RID: 4802
		// (get) Token: 0x0600774C RID: 30540 RVA: 0x002440A0 File Offset: 0x002422A0
		private Building_ResearchBench ResearchBench
		{
			get
			{
				return (Building_ResearchBench)base.TargetThingA;
			}
		}

		// Token: 0x0600774D RID: 30541 RVA: 0x002440B0 File Offset: 0x002422B0
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.ResearchBench, this.job, 1, -1, null, errorOnFailed, false) && (!this.ResearchBench.def.hasInteractionCell || this.pawn.ReserveSittableOrSpot(this.ResearchBench.InteractionCell, this.job, errorOnFailed));
		}

		// Token: 0x0600774E RID: 30542 RVA: 0x00244113 File Offset: 0x00242313
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell, false);
			Toil research = ToilMaker.MakeToil("MakeNewToils");
			research.tickIntervalAction = delegate(int delta)
			{
				Pawn actor = research.actor;
				float num = actor.GetStatValue(StatDefOf.ResearchSpeed, true, -1);
				num *= this.TargetThingA.GetStatValue(StatDefOf.ResearchSpeedFactor, true, -1);
				Find.ResearchManager.ResearchPerformed(num * (float)delta, actor);
				actor.skills.Learn(SkillDefOf.Intellectual, 0.1f * (float)delta, false, false);
				actor.GainComfortFromCellIfPossible(delta, true);
			};
			research.FailOn(() => this.Project == null);
			research.FailOn(() => !this.Project.CanBeResearchedAt(this.ResearchBench, false));
			research.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			research.WithEffect(EffecterDefOf.Research, TargetIndex.A, null);
			research.WithProgressBar(TargetIndex.A, delegate
			{
				ResearchProjectDef project = this.Project;
				if (project == null)
				{
					return 0f;
				}
				return project.ProgressPercent;
			}, false, -0.5f, false);
			research.defaultCompleteMode = ToilCompleteMode.Delay;
			research.defaultDuration = 4000;
			research.activeSkill = (() => SkillDefOf.Intellectual);
			yield return research;
			yield return Toils_General.Wait(2, TargetIndex.None);
			yield break;
		}

		// Token: 0x04005157 RID: 20823
		private const int JobEndInterval = 4000;
	}
}
