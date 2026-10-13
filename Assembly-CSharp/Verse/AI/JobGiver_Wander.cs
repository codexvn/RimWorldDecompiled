using System;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200101A RID: 4122
	public abstract class JobGiver_Wander : ThinkNode_JobGiver
	{
		// Token: 0x060064F6 RID: 25846 RVA: 0x001FED5C File Offset: 0x001FCF5C
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_Wander jobGiver_Wander = (JobGiver_Wander)base.DeepCopy(resolve);
			jobGiver_Wander.wanderRadius = this.wanderRadius;
			jobGiver_Wander.wanderDestValidator = this.wanderDestValidator;
			jobGiver_Wander.ticksBetweenWandersRange = this.ticksBetweenWandersRange;
			jobGiver_Wander.locomotionUrgency = this.locomotionUrgency;
			jobGiver_Wander.locomotionUrgencyOutsideRadius = this.locomotionUrgencyOutsideRadius;
			jobGiver_Wander.maxDanger = this.maxDanger;
			jobGiver_Wander.expiryInterval = this.expiryInterval;
			jobGiver_Wander.canBashDoors = this.canBashDoors;
			jobGiver_Wander.expireOnNearbyEnemy = this.expireOnNearbyEnemy;
			jobGiver_Wander.canEatMealsFromInventory = this.canEatMealsFromInventory;
			return jobGiver_Wander;
		}

		// Token: 0x060064F7 RID: 25847 RVA: 0x001FEDF0 File Offset: 0x001FCFF0
		protected override Job TryGiveJob(Pawn pawn)
		{
			bool flag = pawn.CurJob != null && pawn.CurJob.def == JobDefOf.GotoWander;
			bool flying = pawn.Flying;
			bool flag2 = pawn.mindState.nextMoveOrderIsWait && !flying;
			if (!flag)
			{
				pawn.mindState.nextMoveOrderIsWait = !pawn.mindState.nextMoveOrderIsWait;
			}
			if (flag2 && !flag)
			{
				return this.<TryGiveJob>g__GetWaitJob|12_0();
			}
			IntVec3 exactWanderDest = this.GetExactWanderDest(pawn);
			if (exactWanderDest == pawn.Position && !flag)
			{
				return this.<TryGiveJob>g__GetWaitJob|12_0();
			}
			if (!exactWanderDest.IsValid)
			{
				pawn.mindState.nextMoveOrderIsWait = false;
				return null;
			}
			LocomotionUrgency value = this.locomotionUrgency;
			if (this.locomotionUrgencyOutsideRadius != null && !pawn.Position.InHorDistOf(this.GetWanderRoot(pawn), this.wanderRadius))
			{
				value = this.locomotionUrgencyOutsideRadius.Value;
			}
			Job job = JobMaker.MakeJob(JobDefOf.GotoWander, exactWanderDest);
			job.locomotionUrgency = value;
			job.expiryInterval = this.expiryInterval;
			job.checkOverrideOnExpire = true;
			job.reportStringOverride = this.reportStringOverride;
			job.canBashDoors = this.canBashDoors;
			if (this.expireOnNearbyEnemy)
			{
				job.expiryInterval = 30;
				job.checkOverrideOnExpire = true;
			}
			this.DecorateGotoJob(job);
			return job;
		}

		// Token: 0x060064F8 RID: 25848 RVA: 0x001FEF40 File Offset: 0x001FD140
		protected virtual IntVec3 GetExactWanderDest(Pawn pawn)
		{
			IntVec3 wanderRoot = this.GetWanderRoot(pawn);
			float value = this.wanderRadius;
			PawnDuty duty = pawn.mindState.duty;
			if (duty != null && duty.wanderRadius != null)
			{
				value = duty.wanderRadius.Value;
			}
			return RCellFinder.RandomWanderDestFor(pawn, wanderRoot, value, this.wanderDestValidator, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger), this.canBashDoors);
		}

		// Token: 0x060064F9 RID: 25849
		protected abstract IntVec3 GetWanderRoot(Pawn pawn);

		// Token: 0x060064FA RID: 25850 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void DecorateGotoJob(Job job)
		{
		}

		// Token: 0x060064FC RID: 25852 RVA: 0x001FEFD0 File Offset: 0x001FD1D0
		[CompilerGenerated]
		private Job <TryGiveJob>g__GetWaitJob|12_0()
		{
			Job job = JobMaker.MakeJob(JobDefOf.Wait_Wander);
			job.expiryInterval = this.ticksBetweenWandersRange.RandomInRange;
			job.reportStringOverride = this.reportStringOverride;
			if (this.expireOnNearbyEnemy)
			{
				job.expiryInterval = 30;
				job.checkOverrideOnExpire = true;
			}
			return job;
		}

		// Token: 0x0400481D RID: 18461
		protected float wanderRadius;

		// Token: 0x0400481E RID: 18462
		protected Func<Pawn, IntVec3, IntVec3, bool> wanderDestValidator;

		// Token: 0x0400481F RID: 18463
		protected IntRange ticksBetweenWandersRange = new IntRange(20, 100);

		// Token: 0x04004820 RID: 18464
		protected LocomotionUrgency locomotionUrgency = LocomotionUrgency.Walk;

		// Token: 0x04004821 RID: 18465
		protected LocomotionUrgency? locomotionUrgencyOutsideRadius;

		// Token: 0x04004822 RID: 18466
		protected Danger maxDanger = Danger.None;

		// Token: 0x04004823 RID: 18467
		protected int expiryInterval = -1;

		// Token: 0x04004824 RID: 18468
		protected bool canBashDoors;

		// Token: 0x04004825 RID: 18469
		protected bool expireOnNearbyEnemy;

		// Token: 0x04004826 RID: 18470
		protected bool canEatMealsFromInventory;

		// Token: 0x04004827 RID: 18471
		private const int EnemyScanCheckTicks = 30;
	}
}
