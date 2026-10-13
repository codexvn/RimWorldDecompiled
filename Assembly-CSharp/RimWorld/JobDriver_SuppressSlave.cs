using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013BC RID: 5052
	public class JobDriver_SuppressSlave : JobDriver
	{
		// Token: 0x17001376 RID: 4982
		// (get) Token: 0x06007BA4 RID: 31652 RVA: 0x0021B747 File Offset: 0x00219947
		protected Pawn Slave
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x06007BA5 RID: 31653 RVA: 0x001E06E3 File Offset: 0x001DE8E3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007BA6 RID: 31654 RVA: 0x0025414F File Offset: 0x0025234F
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Suppress slave"))
			{
				yield break;
			}
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOnMentalState(TargetIndex.A);
			this.FailOnNotAwake(TargetIndex.A);
			this.FailOnForbidden(TargetIndex.A);
			this.FailOn(() => !this.Slave.IsSlaveOfColony);
			yield return Toils_Interpersonal.GotoSlave(this.pawn, this.Slave);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return this.SetLastSuppressionTime(TargetIndex.A);
			yield return this.TrySuppress(TargetIndex.A);
			yield break;
		}

		// Token: 0x06007BA7 RID: 31655 RVA: 0x0025415F File Offset: 0x0025235F
		private Toil TrySuppress(TargetIndex slaveInd)
		{
			Toil toil = ToilMaker.MakeToil("TrySuppress");
			toil.initAction = delegate()
			{
				this.pawn.interactions.TryInteractWith(this.Slave, InteractionDefOf.Suppress);
				PawnUtility.ForceWait(this.Slave, this.SuppressDuration, this.pawn, false, false);
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = this.SuppressDuration;
			return toil;
		}

		// Token: 0x06007BA8 RID: 31656 RVA: 0x00254198 File Offset: 0x00252398
		private Toil SetLastSuppressionTime(TargetIndex targetInd)
		{
			Toil toil = ToilMaker.MakeToil("SetLastSuppressionTime");
			toil.initAction = delegate()
			{
				((Pawn)toil.actor.jobs.curJob.GetTarget(targetInd).Thing).mindState.lastSlaveSuppressedTick = Find.TickManager.TicksGame;
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x04005397 RID: 21399
		private int SuppressDuration = 180;
	}
}
