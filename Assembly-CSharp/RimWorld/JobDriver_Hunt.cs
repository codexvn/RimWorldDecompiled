using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001291 RID: 4753
	public class JobDriver_Hunt : JobDriver
	{
		// Token: 0x1700122E RID: 4654
		// (get) Token: 0x06007463 RID: 29795 RVA: 0x0023A8E0 File Offset: 0x00238AE0
		public Pawn Victim
		{
			get
			{
				Corpse corpse = this.Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x1700122F RID: 4655
		// (get) Token: 0x06007464 RID: 29796 RVA: 0x0023A918 File Offset: 0x00238B18
		private Corpse Corpse
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing as Corpse;
			}
		}

		// Token: 0x06007465 RID: 29797 RVA: 0x0023A93E File Offset: 0x00238B3E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.jobStartTick, "jobStartTick", 0, false);
		}

		// Token: 0x06007466 RID: 29798 RVA: 0x0023A958 File Offset: 0x00238B58
		public override string GetReport()
		{
			if (this.Victim != null)
			{
				return JobUtility.GetResolvedJobReport(this.job.def.reportString, this.Victim);
			}
			return base.GetReport();
		}

		// Token: 0x06007467 RID: 29799 RVA: 0x0023A989 File Offset: 0x00238B89
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Victim, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007468 RID: 29800 RVA: 0x0023A9AC File Offset: 0x00238BAC
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(delegate()
			{
				if (this.Victim == null)
				{
					return true;
				}
				if (this.Victim.IsForbidden(this.pawn))
				{
					return true;
				}
				if (!this.job.ignoreDesignations)
				{
					Pawn victim = this.Victim;
					if (victim != null && !victim.Dead && base.Map.designationManager.DesignationOn(victim, DesignationDefOf.Hunt) == null)
					{
						return true;
					}
				}
				return false;
			});
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				this.jobStartTick = Find.TickManager.TicksGame;
			};
			yield return toil;
			yield return Toils_Combat.TrySetJobToUseAttackVerb(TargetIndex.A);
			Toil startCollectCorpseLabel = Toils_General.Label();
			Toil slaughterLabel = Toils_General.Label();
			Toil gotoCastPos = Toils_Combat.GotoCastPosition(TargetIndex.A, TargetIndex.None, true, 0.95f).JumpIfDespawnedOrNull(TargetIndex.A, startCollectCorpseLabel).FailOn(() => Find.TickManager.TicksGame > this.jobStartTick + 5000);
			yield return gotoCastPos;
			Toil slaughterIfPossible = Toils_Jump.JumpIf(slaughterLabel, delegate
			{
				Pawn victim = this.Victim;
				return (victim.RaceProps.DeathActionWorker == null || !victim.RaceProps.DeathActionWorker.DangerousInMelee) && victim.Downed;
			});
			yield return slaughterIfPossible;
			yield return Toils_Jump.JumpIfTargetNotHittable(TargetIndex.A, gotoCastPos);
			yield return Toils_Combat.CastVerb(TargetIndex.A, false).JumpIfDespawnedOrNull(TargetIndex.A, startCollectCorpseLabel).FailOn(() => Find.TickManager.TicksGame > this.jobStartTick + 5000);
			yield return Toils_Jump.JumpIfTargetDespawnedOrNull(TargetIndex.A, startCollectCorpseLabel);
			yield return Toils_Jump.Jump(slaughterIfPossible);
			yield return slaughterLabel;
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false).FailOnMobile(TargetIndex.A);
			yield return Toils_General.WaitWith(TargetIndex.A, 180, true, false, false, TargetIndex.None, PathEndMode.Touch).FailOnMobile(TargetIndex.A);
			yield return Toils_General.Do(delegate
			{
				if (this.Victim.Dead)
				{
					return;
				}
				ExecutionUtility.DoHuntingExecution(this.pawn, this.Victim);
				this.pawn.records.Increment(RecordDefOf.AnimalsSlaughtered);
				if (this.pawn.InMentalState)
				{
					this.pawn.MentalState.Notify_SlaughteredTarget();
				}
			});
			yield return Toils_Jump.Jump(startCollectCorpseLabel);
			yield return startCollectCorpseLabel;
			yield return this.StartCollectCorpseToil();
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false, true, false);
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B, PathEndMode.ClosestTouch);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true, false);
			yield break;
		}

		// Token: 0x06007469 RID: 29801 RVA: 0x0023A9BC File Offset: 0x00238BBC
		private Toil StartCollectCorpseToil()
		{
			Toil toil = ToilMaker.MakeToil("StartCollectCorpseToil");
			toil.initAction = delegate()
			{
				if (this.Victim == null)
				{
					toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				TaleRecorder.RecordTale(TaleDefOf.Hunted, new object[]
				{
					this.pawn,
					this.Victim
				});
				Corpse corpse = this.Victim.Corpse;
				if (corpse == null || !this.pawn.CanReserveAndReach(corpse, PathEndMode.ClosestTouch, Danger.Deadly, 1, -1, null, false))
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				corpse.SetForbidden(false, true);
				IntVec3 c;
				if (StoreUtility.TryFindBestBetterStoreCellFor(corpse, this.pawn, this.Map, StoragePriority.Unstored, this.pawn.Faction, out c, true))
				{
					this.pawn.Reserve(corpse, this.job, 1, -1, null, true, false);
					this.pawn.Reserve(c, this.job, 1, -1, null, true, false);
					this.job.SetTarget(TargetIndex.B, c);
					this.job.SetTarget(TargetIndex.A, corpse);
					this.job.count = 1;
					this.job.haulMode = HaulMode.ToCellStorage;
					return;
				}
				this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
			};
			return toil;
		}

		// Token: 0x04004FFA RID: 20474
		private int jobStartTick = -1;

		// Token: 0x04004FFB RID: 20475
		private const TargetIndex VictimInd = TargetIndex.A;

		// Token: 0x04004FFC RID: 20476
		private const TargetIndex CorpseInd = TargetIndex.A;

		// Token: 0x04004FFD RID: 20477
		private const TargetIndex StoreCellInd = TargetIndex.B;

		// Token: 0x04004FFE RID: 20478
		private const int MaxHuntTicks = 5000;

		// Token: 0x04004FFF RID: 20479
		public const float MaxRangeFactor = 0.95f;
	}
}
