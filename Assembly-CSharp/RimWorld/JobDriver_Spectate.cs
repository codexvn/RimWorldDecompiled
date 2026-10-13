using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011BB RID: 4539
	public class JobDriver_Spectate : JobDriver
	{
		// Token: 0x06006EFF RID: 28415 RVA: 0x00229204 File Offset: 0x00227404
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.ReserveSittableOrSpot(this.job.GetTarget(TargetIndex.A).Cell, this.job, errorOnFailed);
		}

		// Token: 0x06006F00 RID: 28416 RVA: 0x00229237 File Offset: 0x00227437
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (this.job.GetTarget(TargetIndex.C).HasThing)
			{
				this.EndOnDespawnedOrNull(TargetIndex.C, JobCondition.Incompletable);
			}
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.tickIntervalAction = delegate(int delta)
			{
				this.pawn.rotationTracker.FaceCell(this.job.GetTarget(TargetIndex.B).Cell);
				this.pawn.GainComfortFromCellIfPossible(delta, false);
				if (this.pawn.IsHashIntervalTick(100, delta))
				{
					this.pawn.jobs.CheckForJobOverride(0f, true);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.handlingFacing = true;
			yield return toil;
			yield break;
		}

		// Token: 0x04004D69 RID: 19817
		private const TargetIndex MySpotOrChairInd = TargetIndex.A;

		// Token: 0x04004D6A RID: 19818
		private const TargetIndex WatchTargetInd = TargetIndex.B;

		// Token: 0x04004D6B RID: 19819
		private const TargetIndex ChairInd = TargetIndex.C;
	}
}
