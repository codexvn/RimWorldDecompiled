using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000EDA RID: 3802
	public abstract class JobDriver : IExposable, IJobEndable
	{
		// Token: 0x17000F3F RID: 3903
		// (get) Token: 0x06005F6B RID: 24427 RVA: 0x001E5B21 File Offset: 0x001E3D21
		public virtual Rot4 ForcedLayingRotation
		{
			get
			{
				return Rot4.Invalid;
			}
		}

		// Token: 0x17000F40 RID: 3904
		// (get) Token: 0x06005F6C RID: 24428 RVA: 0x001E5B28 File Offset: 0x001E3D28
		public virtual Vector3 ForcedBodyOffset
		{
			get
			{
				return Vector3.zero;
			}
		}

		// Token: 0x17000F41 RID: 3905
		// (get) Token: 0x06005F6D RID: 24429 RVA: 0x001E5B30 File Offset: 0x001E3D30
		protected Toil CurToil
		{
			get
			{
				if (this.curToilIndex < 0 || this.job == null || this.pawn.CurJob != this.job)
				{
					return null;
				}
				if (this.curToilIndex >= this.toils.Count)
				{
					string[] array = new string[8];
					int num = 0;
					Pawn pawn = this.pawn;
					array[num] = ((pawn != null) ? pawn.ToString() : null);
					array[1] = " with job ";
					int num2 = 2;
					Job curJob = this.pawn.CurJob;
					array[num2] = ((curJob != null) ? curJob.ToString() : null);
					array[3] = " tried to get CurToil with curToilIndex=";
					array[4] = this.curToilIndex.ToString();
					array[5] = " but only has ";
					array[6] = this.toils.Count.ToString();
					array[7] = " toils.";
					Log.Error(string.Concat(array));
					return null;
				}
				return this.toils[this.curToilIndex];
			}
		}

		// Token: 0x17000F42 RID: 3906
		// (get) Token: 0x06005F6E RID: 24430 RVA: 0x001E5C0F File Offset: 0x001E3E0F
		protected bool HaveCurToil
		{
			get
			{
				return this.curToilIndex >= 0 && this.curToilIndex < this.toils.Count && this.job != null && this.pawn.CurJob == this.job;
			}
		}

		// Token: 0x17000F43 RID: 3907
		// (get) Token: 0x06005F6F RID: 24431 RVA: 0x001E5C4C File Offset: 0x001E3E4C
		private bool CanStartNextToilInBusyStance
		{
			get
			{
				int num = this.curToilIndex + 1;
				return num < this.toils.Count && this.toils[num].atomicWithPrevious;
			}
		}

		// Token: 0x17000F44 RID: 3908
		// (get) Token: 0x06005F70 RID: 24432 RVA: 0x001E5C83 File Offset: 0x001E3E83
		public int CurToilIndex
		{
			get
			{
				return this.curToilIndex;
			}
		}

		// Token: 0x17000F45 RID: 3909
		// (get) Token: 0x06005F71 RID: 24433 RVA: 0x001E5C8B File Offset: 0x001E3E8B
		public string CurToilString
		{
			get
			{
				if (this.curToilIndex < this.toils.Count)
				{
					return this.CurToil.ToStringSafe<Toil>();
				}
				return "<end>";
			}
		}

		// Token: 0x17000F46 RID: 3910
		// (get) Token: 0x06005F72 RID: 24434 RVA: 0x001E5CB1 File Offset: 0x001E3EB1
		public bool OnLastToil
		{
			get
			{
				return this.CurToilIndex == this.toils.Count - 1;
			}
		}

		// Token: 0x17000F47 RID: 3911
		// (get) Token: 0x06005F73 RID: 24435 RVA: 0x001E5CC8 File Offset: 0x001E3EC8
		public SkillDef ActiveSkill
		{
			get
			{
				if (!this.HaveCurToil || this.CurToil.activeSkill == null)
				{
					return null;
				}
				return this.CurToil.activeSkill();
			}
		}

		// Token: 0x17000F48 RID: 3912
		// (get) Token: 0x06005F74 RID: 24436 RVA: 0x001E5CF1 File Offset: 0x001E3EF1
		public bool HandlingFacing
		{
			get
			{
				return this.CurToil != null && this.CurToil.handlingFacing;
			}
		}

		// Token: 0x17000F49 RID: 3913
		// (get) Token: 0x06005F75 RID: 24437 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool PlayerInterruptable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000F4A RID: 3914
		// (get) Token: 0x06005F76 RID: 24438 RVA: 0x001E5D08 File Offset: 0x001E3F08
		protected LocalTargetInfo TargetA
		{
			get
			{
				return this.job.targetA;
			}
		}

		// Token: 0x17000F4B RID: 3915
		// (get) Token: 0x06005F77 RID: 24439 RVA: 0x001E5D15 File Offset: 0x001E3F15
		protected LocalTargetInfo TargetB
		{
			get
			{
				return this.job.targetB;
			}
		}

		// Token: 0x17000F4C RID: 3916
		// (get) Token: 0x06005F78 RID: 24440 RVA: 0x001E5D22 File Offset: 0x001E3F22
		protected LocalTargetInfo TargetC
		{
			get
			{
				return this.job.targetC;
			}
		}

		// Token: 0x17000F4D RID: 3917
		// (get) Token: 0x06005F79 RID: 24441 RVA: 0x001E5D2F File Offset: 0x001E3F2F
		// (set) Token: 0x06005F7A RID: 24442 RVA: 0x001E5D41 File Offset: 0x001E3F41
		protected Thing TargetThingA
		{
			get
			{
				return this.job.targetA.Thing;
			}
			set
			{
				this.job.targetA = value;
			}
		}

		// Token: 0x17000F4E RID: 3918
		// (get) Token: 0x06005F7B RID: 24443 RVA: 0x001E5D54 File Offset: 0x001E3F54
		// (set) Token: 0x06005F7C RID: 24444 RVA: 0x001E5D66 File Offset: 0x001E3F66
		protected Thing TargetThingB
		{
			get
			{
				return this.job.targetB.Thing;
			}
			set
			{
				this.job.targetB = value;
			}
		}

		// Token: 0x17000F4F RID: 3919
		// (get) Token: 0x06005F7D RID: 24445 RVA: 0x001E5D79 File Offset: 0x001E3F79
		// (set) Token: 0x06005F7E RID: 24446 RVA: 0x001E5D8B File Offset: 0x001E3F8B
		protected Thing TargetThingC
		{
			get
			{
				return this.job.targetC.Thing;
			}
			set
			{
				this.job.targetC = value;
			}
		}

		// Token: 0x17000F50 RID: 3920
		// (get) Token: 0x06005F7F RID: 24447 RVA: 0x001E5D9E File Offset: 0x001E3F9E
		// (set) Token: 0x06005F80 RID: 24448 RVA: 0x001E5D41 File Offset: 0x001E3F41
		protected Pawn TargetPawnA
		{
			get
			{
				return this.job.targetA.Thing as Pawn;
			}
			set
			{
				this.job.targetA = value;
			}
		}

		// Token: 0x17000F51 RID: 3921
		// (get) Token: 0x06005F81 RID: 24449 RVA: 0x001E5DB5 File Offset: 0x001E3FB5
		// (set) Token: 0x06005F82 RID: 24450 RVA: 0x001E5D66 File Offset: 0x001E3F66
		protected Pawn TargetPawnB
		{
			get
			{
				return this.job.targetB.Thing as Pawn;
			}
			set
			{
				this.job.targetB = value;
			}
		}

		// Token: 0x17000F52 RID: 3922
		// (get) Token: 0x06005F83 RID: 24451 RVA: 0x001E5DCC File Offset: 0x001E3FCC
		// (set) Token: 0x06005F84 RID: 24452 RVA: 0x001E5D8B File Offset: 0x001E3F8B
		protected Pawn TargetPawnC
		{
			get
			{
				return this.job.targetC.Thing as Pawn;
			}
			set
			{
				this.job.targetC = value;
			}
		}

		// Token: 0x17000F53 RID: 3923
		// (get) Token: 0x06005F85 RID: 24453 RVA: 0x001E5DE3 File Offset: 0x001E3FE3
		protected IntVec3 TargetLocA
		{
			get
			{
				return this.job.targetA.Cell;
			}
		}

		// Token: 0x17000F54 RID: 3924
		// (get) Token: 0x06005F86 RID: 24454 RVA: 0x001E5DF5 File Offset: 0x001E3FF5
		protected Map Map
		{
			get
			{
				return this.pawn.MapHeld;
			}
		}

		// Token: 0x06005F87 RID: 24455 RVA: 0x001E5E04 File Offset: 0x001E4004
		public virtual string GetReport()
		{
			string reportStringOverride = this.GetReportStringOverride();
			if (!reportStringOverride.NullOrEmpty())
			{
				return reportStringOverride;
			}
			return this.ReportStringProcessed(this.job.def.reportString);
		}

		// Token: 0x06005F88 RID: 24456 RVA: 0x001E5E38 File Offset: 0x001E4038
		public string GetReportStringOverride()
		{
			if (this.pawn.Crawling && !this.job.crawlingReportStringOverride.NullOrEmpty())
			{
				return this.job.crawlingReportStringOverride;
			}
			return this.job.reportStringOverride;
		}

		// Token: 0x06005F89 RID: 24457 RVA: 0x001E5E70 File Offset: 0x001E4070
		protected virtual string ReportStringProcessed(string str)
		{
			LocalTargetInfo a = this.job.targetA.IsValid ? this.job.targetA : this.job.targetQueueA.FirstValid();
			LocalTargetInfo b = this.job.targetB.IsValid ? this.job.targetB : this.job.targetQueueB.FirstValid();
			LocalTargetInfo targetC = this.job.targetC;
			return JobUtility.GetResolvedJobReport(str, a, b, targetC);
		}

		// Token: 0x06005F8A RID: 24458
		public abstract bool TryMakePreToilReservations(bool errorOnFailed);

		// Token: 0x06005F8B RID: 24459
		protected abstract IEnumerable<Toil> MakeNewToils();

		// Token: 0x06005F8C RID: 24460 RVA: 0x001E5EF2 File Offset: 0x001E40F2
		public virtual void SetInitialPosture()
		{
			this.pawn.jobs.posture = PawnPosture.Standing;
			PortraitsCache.SetDirty(this.pawn);
		}

		// Token: 0x06005F8D RID: 24461 RVA: 0x001E5F10 File Offset: 0x001E4110
		public virtual void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.ended, "ended", false, false);
			Scribe_Values.Look<int>(ref this.curToilIndex, "curToilIndex", 0, true);
			Scribe_Values.Look<int>(ref this.ticksLeftThisToil, "ticksLeftThisToil", 0, false);
			Scribe_Values.Look<bool>(ref this.wantBeginNextToil, "wantBeginNextToil", false, false);
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Values.Look<TargetIndex>(ref this.rotateToFace, "rotateToFace", TargetIndex.A, false);
			Scribe_Values.Look<bool>(ref this.asleep, "asleep", false, false);
			Scribe_Values.Look<float>(ref this.uninstallWorkLeft, "uninstallWorkLeft", 0f, false);
			Scribe_Values.Look<int>(ref this.nextToilIndex, "nextToilIndex", -1, false);
			Scribe_Values.Look<bool>(ref this.collideWithPawns, "collideWithPawns", false, false);
			Scribe_References.Look<Pawn>(ref this.locomotionUrgencySameAs, "locomotionUrgencySameAs", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.SetupToils();
			}
		}

		// Token: 0x06005F8E RID: 24462 RVA: 0x001E5FF4 File Offset: 0x001E41F4
		public void Cleanup(JobCondition condition)
		{
			for (int i = 0; i < this.globalFinishActions.Count; i++)
			{
				try
				{
					this.globalFinishActions[i](condition);
				}
				catch (Exception ex)
				{
					string[] array = new string[10];
					array[0] = "Pawn ";
					array[1] = this.pawn.ToStringSafe<Pawn>();
					array[2] = " threw exception while executing a global finish action (";
					array[3] = i.ToString();
					array[4] = "), jobDriver=";
					array[5] = this.ToStringSafe<JobDriver>();
					array[6] = ", job=";
					array[7] = this.job.ToStringSafe<Job>();
					array[8] = ": ";
					int num = 9;
					Exception ex2 = ex;
					array[num] = ((ex2 != null) ? ex2.ToString() : null);
					Log.Error(string.Concat(array));
				}
			}
			if (this.curToilIndex >= 0 && this.curToilIndex < this.toils.Count)
			{
				this.toils[this.curToilIndex].Cleanup(this.curToilIndex, this);
			}
		}

		// Token: 0x06005F8F RID: 24463 RVA: 0x001E60F8 File Offset: 0x001E42F8
		protected void SetFinalizerJob(Func<JobCondition, Job> finalizerJobFactory)
		{
			this.finalizerJobFactory = finalizerJobFactory;
		}

		// Token: 0x06005F90 RID: 24464 RVA: 0x001E6101 File Offset: 0x001E4301
		public Job GetFinalizerJob(JobCondition condition)
		{
			Func<JobCondition, Job> func = this.finalizerJobFactory;
			if (func == null)
			{
				return null;
			}
			return func(condition);
		}

		// Token: 0x06005F91 RID: 24465 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool CanBeginNowWhileLyingDown()
		{
			return false;
		}

		// Token: 0x06005F92 RID: 24466 RVA: 0x001E6118 File Offset: 0x001E4318
		internal void SetupToils()
		{
			try
			{
				this.toils.Clear();
				foreach (Toil toil in this.MakeNewToils())
				{
					if (toil.defaultCompleteMode == ToilCompleteMode.Undefined)
					{
						Log.Error("Toil has undefined complete mode.");
						toil.defaultCompleteMode = ToilCompleteMode.Instant;
					}
					toil.actor = this.pawn;
					this.toils.Add(toil);
				}
			}
			catch (Exception exception)
			{
				JobUtility.TryStartErrorRecoverJob(this.pawn, "Exception in SetupToils for pawn " + this.pawn.ToStringSafe<Pawn>(), exception, this);
			}
		}

		// Token: 0x06005F93 RID: 24467 RVA: 0x001E61D0 File Offset: 0x001E43D0
		public void ClearToils()
		{
			foreach (Toil toil in this.toils)
			{
				if (!toil.inPool)
				{
					toil.ReturnToPool();
				}
			}
			this.toils.Clear();
		}

		// Token: 0x06005F94 RID: 24468 RVA: 0x001E6238 File Offset: 0x001E4438
		public void DriverTick()
		{
			try
			{
				this.ticksLeftThisToil--;
				this.debugTicksSpentThisToil++;
				if (this.CurToil == null)
				{
					if (!this.pawn.stances.FullBodyBusy || this.CanStartNextToilInBusyStance)
					{
						this.ReadyForNextToil();
					}
				}
				else if (!this.CheckCurrentToilEndOrFail())
				{
					if (this.CurToil.defaultCompleteMode == ToilCompleteMode.Instant)
					{
						this.ReadyForNextToil();
					}
					else
					{
						if (this.CurToil.defaultCompleteMode == ToilCompleteMode.Delay)
						{
							if (this.ticksLeftThisToil <= 0)
							{
								this.ReadyForNextToil();
								return;
							}
						}
						else if (this.CurToil.defaultCompleteMode == ToilCompleteMode.FinishedBusy && !this.pawn.stances.FullBodyBusy)
						{
							this.ReadyForNextToil();
							return;
						}
						if (this.wantBeginNextToil)
						{
							this.TryActuallyStartNextToil();
						}
						else
						{
							JobDriver.<>c__DisplayClass82_0 CS$<>8__locals1;
							CS$<>8__locals1.startingJob = this.pawn.CurJob;
							CS$<>8__locals1.startingJobId = CS$<>8__locals1.startingJob.loadID;
							if (this.CurToil.preTickActions != null)
							{
								Toil curToil = this.CurToil;
								for (int i = 0; i < this.CurToil.preTickActions.Count; i++)
								{
									this.CurToil.preTickActions[i]();
									if (this.<DriverTick>g__JobChanged|82_0(ref CS$<>8__locals1))
									{
										return;
									}
									if (this.CurToil != curToil || this.wantBeginNextToil)
									{
										return;
									}
								}
							}
							if (this.CurToil.tickAction != null)
							{
								this.CurToil.tickAction();
								if (this.<DriverTick>g__JobChanged|82_0(ref CS$<>8__locals1))
								{
									return;
								}
							}
							if (this.job.mote != null)
							{
								this.job.mote.Maintain();
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				JobUtility.TryStartErrorRecoverJob(this.pawn, "Exception in JobDriver fixed tick for pawn " + this.pawn.ToStringSafe<Pawn>(), exception, this);
			}
		}

		// Token: 0x06005F95 RID: 24469 RVA: 0x001E642C File Offset: 0x001E462C
		public void DriverTickInterval(int delta)
		{
			try
			{
				if (!this.wantBeginNextToil)
				{
					JobDriver.<>c__DisplayClass83_0 CS$<>8__locals1;
					CS$<>8__locals1.startingJob = this.pawn.CurJob;
					CS$<>8__locals1.startingJobId = CS$<>8__locals1.startingJob.loadID;
					if (this.CurToil.preTickIntervalActions != null)
					{
						Toil curToil = this.CurToil;
						for (int i = 0; i < curToil.preTickIntervalActions.Count; i++)
						{
							curToil.preTickIntervalActions[i](delta);
							if (this.<DriverTickInterval>g__JobChanged|83_0(ref CS$<>8__locals1))
							{
								return;
							}
							if (this.CurToil != curToil || this.wantBeginNextToil)
							{
								return;
							}
						}
					}
					if (this.CurToil.tickIntervalAction != null)
					{
						this.CurToil.tickIntervalAction(delta);
					}
				}
			}
			catch (Exception exception)
			{
				JobUtility.TryStartErrorRecoverJob(this.pawn, "Exception in JobDriver tick for pawn " + this.pawn.ToStringSafe<Pawn>(), exception, this);
			}
		}

		// Token: 0x06005F96 RID: 24470 RVA: 0x001E651C File Offset: 0x001E471C
		public void ReadyForNextToil()
		{
			this.wantBeginNextToil = true;
			this.TryActuallyStartNextToil();
		}

		// Token: 0x06005F97 RID: 24471 RVA: 0x001E652C File Offset: 0x001E472C
		private void TryActuallyStartNextToil()
		{
			if (this.pawn.stances.FullBodyBusy && !this.CanStartNextToilInBusyStance)
			{
				return;
			}
			if (this.job == null || this.pawn.CurJob != this.job)
			{
				return;
			}
			if (this.HaveCurToil)
			{
				this.CurToil.Cleanup(this.curToilIndex, this);
			}
			if (this.nextToilIndex >= 0)
			{
				this.curToilIndex = this.nextToilIndex;
				this.nextToilIndex = -1;
			}
			else
			{
				this.curToilIndex++;
			}
			this.wantBeginNextToil = false;
			if (!this.HaveCurToil)
			{
				if (this.pawn.stances != null && this.pawn.stances.curStance.StanceBusy)
				{
					Log.ErrorOnce(this.pawn.ToStringSafe<Pawn>() + " ended job " + this.job.ToStringSafe<Job>() + " due to running out of toils during a busy stance.", 6453432);
				}
				this.EndJobWith(JobCondition.Succeeded);
				return;
			}
			this.debugTicksSpentThisToil = 0;
			this.ticksLeftThisToil = this.CurToil.defaultDuration;
			if (!this.CheckCurrentToilEndOrFail())
			{
				Toil curToil = this.CurToil;
				if (this.CurToil.preInitActions != null)
				{
					for (int i = 0; i < this.CurToil.preInitActions.Count; i++)
					{
						try
						{
							this.CurToil.preInitActions[i]();
						}
						catch (Exception exception)
						{
							JobUtility.TryStartErrorRecoverJob(this.pawn, string.Format("JobDriver threw exception in toil {0}'s preInitActions[{1}] for pawn {2}", this.CurToil.ToStringSafe<Toil>(), i, this.pawn.ToStringSafe<Pawn>()), exception, this);
							return;
						}
						if (this.CurToil != curToil)
						{
							break;
						}
					}
				}
				if (this.CurToil == curToil)
				{
					if (this.CurToil.initAction != null)
					{
						try
						{
							this.CurToil.initAction();
						}
						catch (Exception exception2)
						{
							JobUtility.TryStartErrorRecoverJob(this.pawn, "JobDriver threw exception in toil " + this.CurToil.ToStringSafe<Toil>() + "'s initAction for pawn " + this.pawn.ToStringSafe<Pawn>(), exception2, this);
							return;
						}
					}
					if (!this.ended && this.CurToil.defaultCompleteMode == ToilCompleteMode.Instant && this.CurToil == curToil)
					{
						this.ReadyForNextToil();
					}
				}
			}
		}

		// Token: 0x06005F98 RID: 24472 RVA: 0x001E676C File Offset: 0x001E496C
		public void EndJobWith(JobCondition condition)
		{
			if (!this.pawn.Destroyed && this.job != null && this.pawn.CurJob == this.job)
			{
				this.pawn.jobs.EndCurrentJob(condition, true, true);
			}
		}

		// Token: 0x06005F99 RID: 24473 RVA: 0x001E67A9 File Offset: 0x001E49A9
		public virtual object[] TaleParameters()
		{
			return new object[]
			{
				this.pawn
			};
		}

		// Token: 0x06005F9A RID: 24474 RVA: 0x001E67BC File Offset: 0x001E49BC
		private bool CheckCurrentToilEndOrFail()
		{
			bool result;
			try
			{
				Toil curToil = this.CurToil;
				if (this.globalFailConditions != null)
				{
					for (int i = 0; i < this.globalFailConditions.Count; i++)
					{
						JobCondition jobCondition = this.globalFailConditions[i]();
						if (jobCondition != JobCondition.Ongoing)
						{
							if (this.pawn.jobs.debugLog)
							{
								this.pawn.jobs.DebugLogEvent(string.Concat(new string[]
								{
									base.GetType().Name,
									" ends current job ",
									this.job.ToStringSafe<Job>(),
									" because of globalFailConditions[",
									i.ToString(),
									"]"
								}));
							}
							this.EndJobWith(jobCondition);
							return true;
						}
					}
				}
				if (curToil != null && curToil.endConditions != null)
				{
					for (int j = 0; j < curToil.endConditions.Count; j++)
					{
						JobCondition jobCondition2 = curToil.endConditions[j]();
						if (jobCondition2 != JobCondition.Ongoing)
						{
							if (this.pawn.jobs.debugLog)
							{
								this.pawn.jobs.DebugLogEvent(string.Format("{0} ends current job {1} because of toil {2} at toils[{3}].endConditions[{4}]", new object[]
								{
									base.GetType().Name,
									this.job.ToStringSafe<Job>(),
									curToil.ToStringSafe<Toil>(),
									this.curToilIndex,
									j
								}));
							}
							this.EndJobWith(jobCondition2);
							return true;
						}
					}
				}
				result = false;
			}
			catch (Exception exception)
			{
				JobUtility.TryStartErrorRecoverJob(this.pawn, "Exception in CheckCurrentToilEndOrFail for pawn " + this.pawn.ToStringSafe<Pawn>(), exception, this);
				result = true;
			}
			return result;
		}

		// Token: 0x06005F9B RID: 24475 RVA: 0x001E6998 File Offset: 0x001E4B98
		private void SetNextToil(Toil to)
		{
			if (to != null && !this.toils.Contains(to))
			{
				Log.Warning(string.Concat(new string[]
				{
					"SetNextToil with non-existent toil (",
					to.ToStringSafe<Toil>(),
					"). pawn=",
					this.pawn.ToStringSafe<Pawn>(),
					", job=",
					this.pawn.CurJob.ToStringSafe<Job>()
				}));
			}
			this.nextToilIndex = this.toils.IndexOf(to);
		}

		// Token: 0x06005F9C RID: 24476 RVA: 0x001E6A1C File Offset: 0x001E4C1C
		public void JumpToToil(Toil to)
		{
			if (to == null)
			{
				Log.Warning("JumpToToil with null toil. pawn=" + this.pawn.ToStringSafe<Pawn>() + ", job=" + this.pawn.CurJob.ToStringSafe<Job>());
			}
			this.SetNextToil(to);
			this.ReadyForNextToil();
		}

		// Token: 0x06005F9D RID: 24477 RVA: 0x001E6A68 File Offset: 0x001E4C68
		public virtual void Notify_Starting()
		{
			this.startTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06005F9E RID: 24478 RVA: 0x001E6A7A File Offset: 0x001E4C7A
		public virtual void Notify_PatherArrived()
		{
			if (this.CurToil == null)
			{
				return;
			}
			if (this.CurToil.defaultCompleteMode == ToilCompleteMode.PatherArrival)
			{
				this.ReadyForNextToil();
			}
		}

		// Token: 0x06005F9F RID: 24479 RVA: 0x001E6A99 File Offset: 0x001E4C99
		public virtual void Notify_PatherFailed()
		{
			this.EndJobWith(JobCondition.ErroredPather);
		}

		// Token: 0x06005FA0 RID: 24480 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_StanceChanged()
		{
		}

		// Token: 0x06005FA1 RID: 24481 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_DamageTaken(DamageInfo dinfo)
		{
		}

		// Token: 0x06005FA2 RID: 24482 RVA: 0x001E6AA6 File Offset: 0x001E4CA6
		public Pawn GetActor()
		{
			return this.pawn;
		}

		// Token: 0x06005FA3 RID: 24483 RVA: 0x001E6AAE File Offset: 0x001E4CAE
		public void AddEndCondition(Func<JobCondition> newEndCondition)
		{
			this.globalFailConditions.Add(newEndCondition);
		}

		// Token: 0x06005FA4 RID: 24484 RVA: 0x001E6ABC File Offset: 0x001E4CBC
		public void AddFailCondition(Func<bool> newFailCondition)
		{
			this.globalFailConditions.Add(delegate
			{
				if (newFailCondition())
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
		}

		// Token: 0x06005FA5 RID: 24485 RVA: 0x001E6AED File Offset: 0x001E4CED
		public void AddFinishAction(Action<JobCondition> newAct)
		{
			this.globalFinishActions.Add(newAct);
		}

		// Token: 0x06005FA6 RID: 24486 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool ModifyCarriedThingDrawPos(ref Vector3 drawPos, ref bool flip)
		{
			return false;
		}

		// Token: 0x06005FA7 RID: 24487 RVA: 0x001E6AFB File Offset: 0x001E4CFB
		public virtual RandomSocialMode DesiredSocialMode()
		{
			if (this.CurToil != null)
			{
				return this.CurToil.socialMode;
			}
			return RandomSocialMode.Normal;
		}

		// Token: 0x06005FA8 RID: 24488 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool IsContinuation(Job j)
		{
			return true;
		}

		// Token: 0x06005FA9 RID: 24489 RVA: 0x001E6B14 File Offset: 0x001E4D14
		public virtual bool? IsSameJobAs(Job j)
		{
			return null;
		}

		// Token: 0x06005FAB RID: 24491 RVA: 0x001E6B87 File Offset: 0x001E4D87
		[CompilerGenerated]
		private bool <DriverTick>g__JobChanged|82_0(ref JobDriver.<>c__DisplayClass82_0 A_1)
		{
			return this.pawn.CurJob != A_1.startingJob || this.pawn.CurJob.loadID != A_1.startingJobId;
		}

		// Token: 0x06005FAC RID: 24492 RVA: 0x001E6BB9 File Offset: 0x001E4DB9
		[CompilerGenerated]
		private bool <DriverTickInterval>g__JobChanged|83_0(ref JobDriver.<>c__DisplayClass83_0 A_1)
		{
			return this.pawn.CurJob != A_1.startingJob || this.pawn.CurJob.loadID != A_1.startingJobId;
		}

		// Token: 0x040044CA RID: 17610
		public const int PlaceInInventoryDuration = 25;

		// Token: 0x040044CB RID: 17611
		public Pawn pawn;

		// Token: 0x040044CC RID: 17612
		public Job job;

		// Token: 0x040044CD RID: 17613
		private List<Toil> toils = new List<Toil>();

		// Token: 0x040044CE RID: 17614
		public List<Func<JobCondition>> globalFailConditions = new List<Func<JobCondition>>();

		// Token: 0x040044CF RID: 17615
		public List<Action<JobCondition>> globalFinishActions = new List<Action<JobCondition>>();

		// Token: 0x040044D0 RID: 17616
		private Func<JobCondition, Job> finalizerJobFactory;

		// Token: 0x040044D1 RID: 17617
		public bool ended;

		// Token: 0x040044D2 RID: 17618
		private int curToilIndex = -1;

		// Token: 0x040044D3 RID: 17619
		public int ticksLeftThisToil = 99999;

		// Token: 0x040044D4 RID: 17620
		private bool wantBeginNextToil;

		// Token: 0x040044D5 RID: 17621
		protected int startTick = -1;

		// Token: 0x040044D6 RID: 17622
		public TargetIndex rotateToFace = TargetIndex.A;

		// Token: 0x040044D7 RID: 17623
		private int nextToilIndex = -1;

		// Token: 0x040044D8 RID: 17624
		public bool asleep;

		// Token: 0x040044D9 RID: 17625
		public float uninstallWorkLeft;

		// Token: 0x040044DA RID: 17626
		public bool collideWithPawns;

		// Token: 0x040044DB RID: 17627
		public Pawn locomotionUrgencySameAs;

		// Token: 0x040044DC RID: 17628
		public int debugTicksSpentThisToil;
	}
}
