using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012A8 RID: 4776
	public class JobDriver_LayDown : JobDriver
	{
		// Token: 0x1700124B RID: 4683
		// (get) Token: 0x06007502 RID: 29954 RVA: 0x0023C944 File Offset: 0x0023AB44
		public Building_Bed Bed
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing as Building_Bed;
			}
		}

		// Token: 0x1700124C RID: 4684
		// (get) Token: 0x06007503 RID: 29955 RVA: 0x0023C96C File Offset: 0x0023AB6C
		public IntVec3 Cell
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Cell;
			}
		}

		// Token: 0x1700124D RID: 4685
		// (get) Token: 0x06007504 RID: 29956 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanSleep
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700124E RID: 4686
		// (get) Token: 0x06007505 RID: 29957 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanRest
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700124F RID: 4687
		// (get) Token: 0x06007506 RID: 29958 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool LookForOtherJobs
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007507 RID: 29959 RVA: 0x0023C990 File Offset: 0x0023AB90
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (this.Bed != null)
			{
				if (!this.pawn.Reserve(this.Bed, this.job, this.Bed.SleepingSlotsCount, 0, null, errorOnFailed, false))
				{
					return false;
				}
			}
			else if (this.job.targetA.Cell.IsValid && !this.pawn.ageTracker.CurLifeStage.alwaysDowned && !this.job.forceSleep && !this.pawn.Reserve(this.job.targetA.Cell, this.job, 1, -1, null, errorOnFailed, false))
			{
				return false;
			}
			return true;
		}

		// Token: 0x06007508 RID: 29960 RVA: 0x00234F9B File Offset: 0x0023319B
		public override bool CanBeginNowWhileLyingDown()
		{
			return JobInBedUtility.InBedOrRestSpotNow(this.pawn, this.job.GetTarget(TargetIndex.A));
		}

		// Token: 0x06007509 RID: 29961 RVA: 0x0023CA45 File Offset: 0x0023AC45
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.canMoveOrCrawl = (!this.pawn.Downed || this.pawn.health.CanCrawl);
			this.hasSavedValues = true;
		}

		// Token: 0x0600750A RID: 29962 RVA: 0x0023CA7A File Offset: 0x0023AC7A
		protected override IEnumerable<Toil> MakeNewToils()
		{
			bool hasBed = this.Bed != null;
			if (hasBed)
			{
				yield return Toils_Bed.ClaimBedIfNonMedical(TargetIndex.A, TargetIndex.None);
				yield return Toils_Bed.GotoBed(TargetIndex.A).FailOn(() => this.pawn.Downed && !this.pawn.health.CanCrawl && !this.Bed.OccupiedRect().Contains(this.pawn.Position));
			}
			else
			{
				Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
				if ((thing == null || this.pawn.SpawnedParentOrMe != thing) && this.canMoveOrCrawl)
				{
					yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell).FailOn(() => this.pawn.Downed && !this.pawn.health.CanCrawl);
				}
			}
			yield return this.LayDownToil(hasBed);
			yield break;
		}

		// Token: 0x0600750B RID: 29963 RVA: 0x0023CA8A File Offset: 0x0023AC8A
		public virtual Toil LayDownToil(bool hasBed)
		{
			return Toils_LayDown.LayDown(TargetIndex.A, hasBed, this.LookForOtherJobs, this.CanSleep, this.CanRest, PawnPosture.LayingOnGroundNormal, false);
		}

		// Token: 0x0600750C RID: 29964 RVA: 0x0023CAA8 File Offset: 0x0023ACA8
		public override string GetReport()
		{
			string reportStringOverride = base.GetReportStringOverride();
			if (!reportStringOverride.NullOrEmpty())
			{
				return reportStringOverride;
			}
			if (this.asleep)
			{
				return "ReportSleeping".Translate();
			}
			Thing spawnedParentOrMe = this.pawn.SpawnedParentOrMe;
			if (spawnedParentOrMe != null && this.pawn != spawnedParentOrMe)
			{
				return JobDriver_Carried.GetReport(this.pawn, spawnedParentOrMe);
			}
			if (this.pawn.health.hediffSet.InLabor(false))
			{
				return "GivingBirth".Translate();
			}
			return "ReportResting".Translate();
		}

		// Token: 0x0600750D RID: 29965 RVA: 0x0023CB3A File Offset: 0x0023AD3A
		public override bool IsContinuation(Job j)
		{
			return !(this.job.GetTarget(TargetIndex.A) != j.GetTarget(TargetIndex.A)) && base.IsContinuation(j);
		}

		// Token: 0x0600750E RID: 29966 RVA: 0x0023CB60 File Offset: 0x0023AD60
		public override void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.canMoveOrCrawl, "canMoveOrCrawl", false, false);
			Scribe_Values.Look<bool>(ref this.hasSavedValues, "hasSavedValues", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && !this.hasSavedValues)
			{
				this.canMoveOrCrawl = (!this.pawn.Downed || this.pawn.health.CanCrawl);
				this.hasSavedValues = true;
			}
			base.ExposeData();
		}

		// Token: 0x04005041 RID: 20545
		private bool canMoveOrCrawl;

		// Token: 0x04005042 RID: 20546
		private bool hasSavedValues;

		// Token: 0x04005043 RID: 20547
		public const TargetIndex BedOrRestSpotIndex = TargetIndex.A;
	}
}
