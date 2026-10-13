using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001204 RID: 4612
	public class JobDriver_NatureRunning : JobDriver
	{
		// Token: 0x060070B0 RID: 28848 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x060070B1 RID: 28849 RVA: 0x0022F0BF File Offset: 0x0022D2BF
		private Toil FindInterestingThing()
		{
			Toil toil = ToilMaker.MakeToil("FindInterestingThing");
			toil.initAction = delegate()
			{
				LocalTargetInfo pack;
				if (NatureRunningUtility.TryFindNatureInterestTarget(this.pawn, out pack))
				{
					this.job.SetTarget(TargetIndex.A, pack);
					return;
				}
				base.EndJobWith(JobCondition.Incompletable);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x060070B2 RID: 28850 RVA: 0x0022F0E4 File Offset: 0x0022D2E4
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnChildLearningConditions<JobDriver_NatureRunning>();
			Toil findInterestingThing = this.FindInterestingThing();
			yield return findInterestingThing;
			Toil toil = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell, false);
			toil.tickIntervalAction = delegate(int delta)
			{
				LearningUtility.LearningTickCheckEnd(this.pawn, delta, false);
			};
			yield return toil;
			Toil wait = ToilMaker.MakeToil("MakeNewToils");
			wait.initAction = delegate()
			{
				wait.actor.pather.StopDead();
			};
			wait.tickIntervalAction = delegate(int delta)
			{
				if (!LearningUtility.LearningTickCheckEnd(this.pawn, delta, false))
				{
					this.pawn.rotationTracker.FaceTarget(this.TargetA);
				}
			};
			wait.defaultCompleteMode = ToilCompleteMode.Delay;
			wait.defaultDuration = JobDriver_NatureRunning.WaitTicksRange.RandomInRange;
			wait.handlingFacing = true;
			yield return wait;
			yield return Toils_Jump.Jump(findInterestingThing);
			yield break;
		}

		// Token: 0x04004E40 RID: 20032
		private static readonly IntRange WaitTicksRange = new IntRange(300, 600);
	}
}
