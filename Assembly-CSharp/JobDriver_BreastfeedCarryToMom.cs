using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

// Token: 0x02000008 RID: 8
public class JobDriver_BreastfeedCarryToMom : JobDriver
{
	// Token: 0x17000009 RID: 9
	// (get) Token: 0x0600001E RID: 30 RVA: 0x000024E7 File Offset: 0x000006E7
	private Pawn Baby
	{
		get
		{
			return (Pawn)base.TargetThingA;
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x0600001F RID: 31 RVA: 0x000024F4 File Offset: 0x000006F4
	private Pawn Mom
	{
		get
		{
			return (Pawn)base.TargetThingB;
		}
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x06000020 RID: 32 RVA: 0x00002501 File Offset: 0x00000701
	protected virtual bool MomMustBeImmobile
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00002504 File Offset: 0x00000704
	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return this.pawn.Reserve(this.Baby, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.Mom, this.job, 1, -1, null, errorOnFailed, false);
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00002557 File Offset: 0x00000757
	protected override IEnumerable<Toil> MakeNewToils()
	{
		this.FailOnDestroyedNullOrForbidden(TargetIndex.A);
		this.FailOnDestroyedNullOrForbidden(TargetIndex.B);
		base.AddFailCondition(delegate
		{
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			return (this.MomMustBeImmobile && !this.Mom.Downed && !this.Mom.IsPrisoner) || !ChildcareUtility.CanMomBreastfeedBaby(this.Mom, this.Baby, out breastfeedFailReason);
		});
		base.SetFinalizerJob(delegate(JobCondition condition)
		{
			if (!this.pawn.IsCarryingPawn(this.Baby))
			{
				return null;
			}
			return ChildcareUtility.MakeBringBabyToSafetyJob(this.pawn, this.Baby);
		});
		Toil carryingBabyStart = Toils_General.Label();
		yield return Toils_Jump.JumpIf(carryingBabyStart, () => this.pawn.IsCarryingPawn(this.Baby));
		yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell, false).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
		yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false, true, false);
		yield return carryingBabyStart;
		yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell, false).FailOnDestroyedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
		yield return Toils_Reserve.Release(TargetIndex.A);
		yield return Toils_Reserve.Release(TargetIndex.B);
		yield return this.StartBreastfeedJobOnMom();
		yield break;
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00002567 File Offset: 0x00000767
	private Toil StartBreastfeedJobOnMom()
	{
		Toil toil = ToilMaker.MakeToil("StartBreastfeedJobOnMom");
		toil.initAction = delegate()
		{
			this.Mom.jobs.SuspendCurrentJob(JobCondition.InterruptForced, true, new bool?(false));
			if (!this.pawn.carryTracker.innerContainer.TryTransferToContainer(this.pawn.carryTracker.CarriedThing, this.Mom.carryTracker.innerContainer, true))
			{
				this.Mom.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				base.EndJobWith(JobCondition.Incompletable);
				return;
			}
			Pawn_JobTracker jobs = this.Mom.jobs;
			Job newJob = ChildcareUtility.MakeBreastfeedJob(this.Baby, this.Mom.CurrentBed());
			JobCondition lastJobEndCondition = JobCondition.InterruptForced;
			ThinkNode jobGiver = null;
			bool resumeCurJobAfterwards = false;
			bool cancelBusyStances = true;
			ThinkTreeDef thinkTree = null;
			bool? keepCarryingThingOverride = new bool?(true);
			jobs.StartJob(newJob, lastJobEndCondition, jobGiver, resumeCurJobAfterwards, cancelBusyStances, thinkTree, null, false, false, keepCarryingThingOverride, false, true, false);
		};
		return toil;
	}

	// Token: 0x04000008 RID: 8
	private const TargetIndex BabyInd = TargetIndex.A;

	// Token: 0x04000009 RID: 9
	private const TargetIndex MomInd = TargetIndex.B;
}
