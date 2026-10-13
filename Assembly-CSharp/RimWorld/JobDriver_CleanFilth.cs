using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200123B RID: 4667
	public class JobDriver_CleanFilth : JobDriver
	{
		// Token: 0x170011C9 RID: 4553
		// (get) Token: 0x0600722B RID: 29227 RVA: 0x00233B00 File Offset: 0x00231D00
		private Filth Filth
		{
			get
			{
				return (Filth)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x0600722C RID: 29228 RVA: 0x00233B26 File Offset: 0x00231D26
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x0600722D RID: 29229 RVA: 0x00233B49 File Offset: 0x00231D49
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil initExtractTargetFromQueue = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A, null);
			yield return initExtractTargetFromQueue;
			yield return Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
			yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A, true);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false).JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, initExtractTargetFromQueue).JumpIfOutsideHomeArea(TargetIndex.A, initExtractTargetFromQueue);
			Toil clean = ToilMaker.MakeToil("MakeNewToils");
			clean.initAction = delegate()
			{
				this.cleaningWorkDone = 0f;
				this.totalCleaningWorkDone = 0f;
				this.totalCleaningWorkRequired = this.Filth.def.filth.cleaningWorkToReduceThickness * (float)this.Filth.thickness;
			};
			clean.tickIntervalAction = delegate(int delta)
			{
				Filth filth = this.Filth;
				float statValueAbstract = filth.Position.GetTerrain(filth.Map).GetStatValueAbstract(StatDefOf.CleaningTimeFactor, null);
				float num = this.pawn.GetStatValue(StatDefOf.CleaningSpeed, true, -1) * (float)delta;
				if (statValueAbstract != 0f)
				{
					num /= statValueAbstract;
				}
				this.cleaningWorkDone += num;
				this.totalCleaningWorkDone += num;
				if (this.cleaningWorkDone > filth.def.filth.cleaningWorkToReduceThickness)
				{
					filth.ThinFilth();
					this.cleaningWorkDone = 0f;
					if (filth.Destroyed)
					{
						clean.actor.records.Increment(RecordDefOf.MessesCleaned);
						this.ReadyForNextToil();
						return;
					}
				}
			};
			clean.defaultCompleteMode = ToilCompleteMode.Never;
			clean.WithEffect(EffecterDefOf.Clean, TargetIndex.A, null);
			clean.WithProgressBar(TargetIndex.A, () => this.totalCleaningWorkDone / this.totalCleaningWorkRequired, true, -0.5f, false);
			clean.PlaySustainerOrSound(delegate()
			{
				ThingDef def = this.Filth.def;
				if (!def.filth.cleaningSound.NullOrUndefined())
				{
					return def.filth.cleaningSound;
				}
				return SoundDefOf.Interact_CleanFilth;
			}, 1f);
			clean.JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, initExtractTargetFromQueue);
			clean.JumpIfOutsideHomeArea(TargetIndex.A, initExtractTargetFromQueue);
			clean.JumpIf(delegate
			{
				Thing thing = clean.actor.jobs.curJob.GetTarget(TargetIndex.A).Thing;
				return thing != null && thing.Destroyed;
			}, initExtractTargetFromQueue);
			yield return clean;
			yield return Toils_Jump.Jump(initExtractTargetFromQueue);
			yield break;
		}

		// Token: 0x0600722E RID: 29230 RVA: 0x00233B5C File Offset: 0x00231D5C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.cleaningWorkDone, "cleaningWorkDone", 0f, false);
			Scribe_Values.Look<float>(ref this.totalCleaningWorkDone, "totalCleaningWorkDone", 0f, false);
			Scribe_Values.Look<float>(ref this.totalCleaningWorkRequired, "totalCleaningWorkRequired", 0f, false);
		}

		// Token: 0x04004EF9 RID: 20217
		private float cleaningWorkDone;

		// Token: 0x04004EFA RID: 20218
		private float totalCleaningWorkDone;

		// Token: 0x04004EFB RID: 20219
		private float totalCleaningWorkRequired;

		// Token: 0x04004EFC RID: 20220
		private const TargetIndex FilthInd = TargetIndex.A;
	}
}
