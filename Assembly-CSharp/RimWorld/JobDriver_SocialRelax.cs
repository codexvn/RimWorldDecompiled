using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011F0 RID: 4592
	public class JobDriver_SocialRelax : JobDriver
	{
		// Token: 0x17001164 RID: 4452
		// (get) Token: 0x06007030 RID: 28720 RVA: 0x0022D6BC File Offset: 0x0022B8BC
		private Thing GatherSpotParent
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17001165 RID: 4453
		// (get) Token: 0x06007031 RID: 28721 RVA: 0x0022D6E0 File Offset: 0x0022B8E0
		private bool HasChair
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).HasThing;
			}
		}

		// Token: 0x17001166 RID: 4454
		// (get) Token: 0x06007032 RID: 28722 RVA: 0x0022D704 File Offset: 0x0022B904
		private bool HasDrink
		{
			get
			{
				return this.job.GetTarget(TargetIndex.C).HasThing;
			}
		}

		// Token: 0x17001167 RID: 4455
		// (get) Token: 0x06007033 RID: 28723 RVA: 0x0022D728 File Offset: 0x0022B928
		private IntVec3 ClosestGatherSpotParentCell
		{
			get
			{
				return this.GatherSpotParent.OccupiedRect().ClosestCellTo(this.pawn.Position);
			}
		}

		// Token: 0x06007034 RID: 28724 RVA: 0x0022D754 File Offset: 0x0022B954
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			IntVec3 cell;
			if (!this.HasChair || !Toils_Ingest.TryFindFreeSittingSpotOnThing(this.job.GetTarget(TargetIndex.B).Thing, this.pawn, out cell))
			{
				cell = this.job.GetTarget(TargetIndex.B).Cell;
			}
			return this.pawn.ReserveSittableOrSpot(cell, this.pawn.CurJob, true) && (!this.HasDrink || this.pawn.Reserve(this.job.GetTarget(TargetIndex.C), this.job, 1, -1, null, errorOnFailed, false));
		}

		// Token: 0x06007035 RID: 28725 RVA: 0x0022D7EC File Offset: 0x0022B9EC
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
			if (this.HasChair)
			{
				this.EndOnDespawnedOrNull(TargetIndex.B, JobCondition.Incompletable);
			}
			if (this.HasDrink)
			{
				this.FailOnDestroyedNullOrForbidden(TargetIndex.C);
				yield return Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.OnCell, false).FailOnSomeonePhysicallyInteracting(TargetIndex.C);
				yield return Toils_Haul.StartCarryThing(TargetIndex.C, false, false, false, true, false);
			}
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.tickIntervalAction = delegate(int delta)
			{
				this.pawn.rotationTracker.FaceCell(this.ClosestGatherSpotParentCell);
				this.pawn.GainComfortFromCellIfPossible(delta, false);
				JoyUtility.JoyTickCheckEnd(this.pawn, delta, JoyTickFullJoyAction.GoToNextToil, 1f, null);
			};
			toil.handlingFacing = true;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = this.job.def.joyDuration;
			toil.AddFinishAction(delegate
			{
				JoyUtility.TryGainRecRoomThought(this.pawn);
			});
			toil.socialMode = RandomSocialMode.SuperActive;
			Toils_Ingest.AddIngestionEffects(toil, this.pawn, TargetIndex.C, TargetIndex.None);
			yield return toil;
			if (this.HasDrink)
			{
				yield return Toils_Ingest.FinalizeIngest(this.pawn, TargetIndex.C);
			}
			yield break;
		}

		// Token: 0x06007036 RID: 28726 RVA: 0x0022D7FC File Offset: 0x0022B9FC
		public override bool ModifyCarriedThingDrawPos(ref Vector3 drawPos, ref bool flip)
		{
			IntVec3 closestGatherSpotParentCell = this.ClosestGatherSpotParentCell;
			return JobDriver_Ingest.ModifyCarriedThingDrawPosWorker(ref drawPos, ref flip, closestGatherSpotParentCell, this.pawn);
		}

		// Token: 0x04004E0F RID: 19983
		private const TargetIndex GatherSpotParentInd = TargetIndex.A;

		// Token: 0x04004E10 RID: 19984
		private const TargetIndex ChairOrSpotInd = TargetIndex.B;

		// Token: 0x04004E11 RID: 19985
		private const TargetIndex OptionalIngestibleInd = TargetIndex.C;
	}
}
