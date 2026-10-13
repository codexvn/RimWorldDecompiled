using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011EA RID: 4586
	public class JobDriver_RelaxAlone : JobDriver
	{
		// Token: 0x1700115B RID: 4443
		// (get) Token: 0x06006FFD RID: 28669 RVA: 0x0022CDF0 File Offset: 0x0022AFF0
		private bool FromBed
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing is Building_Bed;
			}
		}

		// Token: 0x1700115C RID: 4444
		// (get) Token: 0x06006FFE RID: 28670 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool CanSleep
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006FFF RID: 28671 RVA: 0x0022CE19 File Offset: 0x0022B019
		public override bool CanBeginNowWhileLyingDown()
		{
			return this.FromBed && JobInBedUtility.InBedOrRestSpotNow(this.pawn, this.job.GetTarget(TargetIndex.A));
		}

		// Token: 0x06007000 RID: 28672 RVA: 0x0022CE3C File Offset: 0x0022B03C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (this.FromBed)
			{
				if (!this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, ((Building_Bed)this.job.GetTarget(TargetIndex.A).Thing).SleepingSlotsCount, 0, null, errorOnFailed, false))
				{
					return false;
				}
			}
			else if (!this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false))
			{
				return false;
			}
			return true;
		}

		// Token: 0x06007001 RID: 28673 RVA: 0x0022CEBB File Offset: 0x0022B0BB
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil toil;
			if (this.FromBed)
			{
				this.KeepLyingDown(TargetIndex.A);
				yield return Toils_Bed.ClaimBedIfNonMedical(TargetIndex.A, TargetIndex.None);
				yield return Toils_Bed.GotoBed(TargetIndex.A);
				toil = Toils_LayDown.LayDown(TargetIndex.A, true, false, this.CanSleep, true, PawnPosture.LayingOnGroundNormal, false);
				toil.AddFailCondition(() => !this.pawn.Awake());
			}
			else
			{
				yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
				toil = ToilMaker.MakeToil("MakeNewToils");
				toil.initAction = delegate()
				{
					this.faceDir = (this.job.def.faceDir.IsValid ? this.job.def.faceDir : Rot4.Random);
				};
				toil.handlingFacing = true;
			}
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = this.job.def.joyDuration;
			toil.AddPreTickIntervalAction(delegate(int delta)
			{
				if (this.faceDir.IsValid)
				{
					this.pawn.rotationTracker.FaceCell(this.pawn.Position + this.faceDir.FacingCell);
				}
				this.pawn.GainComfortFromCellIfPossible(delta, false);
				JoyUtility.JoyTickCheckEnd(this.pawn, delta, JoyTickFullJoyAction.EndJob, 1f, null);
			});
			yield return toil;
			yield break;
		}

		// Token: 0x06007002 RID: 28674 RVA: 0x0022CECC File Offset: 0x0022B0CC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Rot4>(ref this.faceDir, "faceDir", default(Rot4), false);
		}

		// Token: 0x04004DFF RID: 19967
		protected Rot4 faceDir = Rot4.Invalid;

		// Token: 0x04004E00 RID: 19968
		protected const TargetIndex SpotOrBedInd = TargetIndex.A;
	}
}
