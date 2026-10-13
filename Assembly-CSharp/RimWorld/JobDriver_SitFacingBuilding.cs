using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011EC RID: 4588
	public class JobDriver_SitFacingBuilding : JobDriver
	{
		// Token: 0x1700115F RID: 4447
		// (get) Token: 0x0600700F RID: 28687 RVA: 0x0022D15B File Offset: 0x0022B35B
		private Building Building
		{
			get
			{
				return (Building)base.TargetThingA;
			}
		}

		// Token: 0x06007010 RID: 28688 RVA: 0x0022D168 File Offset: 0x0022B368
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, this.job.def.joyMaxParticipants, 0, null, errorOnFailed, false) && this.pawn.ReserveSittableOrSpot(this.job.targetB.Cell, this.job, errorOnFailed);
		}

		// Token: 0x06007011 RID: 28689 RVA: 0x0022D1CB File Offset: 0x0022B3CB
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.EndOnDespawnedOrNull(this.BuildingIndex, JobCondition.Incompletable);
			yield return Toils_Goto.Goto(this.SeatIndex, PathEndMode.OnCell);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.tickIntervalAction = delegate(int delta)
			{
				this.pawn.rotationTracker.FaceTarget(this.Building);
				this.pawn.GainComfortFromCellIfPossible(delta, false);
				Pawn pawn = this.pawn;
				Building building = this.Building;
				JoyUtility.JoyTickCheckEnd(pawn, delta, this.job.doUntilGatheringEnded ? JoyTickFullJoyAction.None : JoyTickFullJoyAction.EndJob, 1f, building);
			};
			toil.handlingFacing = true;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = (this.job.doUntilGatheringEnded ? this.job.expiryInterval : this.job.def.joyDuration);
			toil.AddFinishAction(delegate
			{
				JoyUtility.TryGainRecRoomThought(this.pawn);
			});
			this.ModifyPlayToil(toil);
			yield return toil;
			yield break;
		}

		// Token: 0x06007012 RID: 28690 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void ModifyPlayToil(Toil toil)
		{
		}

		// Token: 0x06007013 RID: 28691 RVA: 0x0022D1DC File Offset: 0x0022B3DC
		public override object[] TaleParameters()
		{
			return new object[]
			{
				this.pawn,
				base.TargetA.Thing.def
			};
		}

		// Token: 0x04004E05 RID: 19973
		private readonly TargetIndex BuildingIndex = TargetIndex.A;

		// Token: 0x04004E06 RID: 19974
		private readonly TargetIndex SeatIndex = TargetIndex.B;
	}
}
