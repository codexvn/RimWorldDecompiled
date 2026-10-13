using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012D6 RID: 4822
	public class JobDriver_RemovePaintBuilding : JobDriver
	{
		// Token: 0x17001285 RID: 4741
		// (get) Token: 0x06007615 RID: 30229 RVA: 0x002400E8 File Offset: 0x0023E2E8
		private Thing PaintTarget
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06007616 RID: 30230 RVA: 0x00233B26 File Offset: 0x00231D26
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x06007617 RID: 30231 RVA: 0x00240109 File Offset: 0x0023E309
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil extractFromQueue = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A, null);
			Toil goToBuilding = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false).JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, extractFromQueue);
			yield return extractFromQueue;
			yield return Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
			yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A, true);
			yield return goToBuilding;
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				this.removalTime = 0f;
			};
			toil.tickIntervalAction = delegate(int delta)
			{
				this.pawn.rotationTracker.FaceTarget(this.PaintTarget);
				this.removalTime += this.pawn.GetStatValue(StatDefOf.WorkSpeedGlobal, true, -1) / 60f * (float)delta;
				if (this.removalTime >= 1f)
				{
					Building building = this.PaintTarget as Building;
					if (building != null)
					{
						building.ChangePaint(null);
					}
					this.PaintTarget.Map.designationManager.TryRemoveDesignationOn(this.PaintTarget, DesignationDefOf.RemovePaintBuilding);
					base.ReadyForNextToil();
					GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.Dye, null), this.pawn.Position, this.pawn.Map, ThingPlaceMode.Near, null, null, null, 1);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.PlaySustainerOrSound(SoundDefOf.Interact_RemovePaint, 1f);
			toil.WithProgressBar(TargetIndex.A, () => this.removalTime / 1f, true, -0.5f, false);
			toil.JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, extractFromQueue);
			toil.handlingFacing = true;
			yield return toil;
			yield return Toils_Jump.Jump(extractFromQueue);
			yield break;
		}

		// Token: 0x06007618 RID: 30232 RVA: 0x00240119 File Offset: 0x0023E319
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.removalTime, "removalTime", 0f, false);
		}

		// Token: 0x040050BF RID: 20671
		private float removalTime;

		// Token: 0x040050C0 RID: 20672
		private const TargetIndex PaintTargetIndex = TargetIndex.A;

		// Token: 0x040050C1 RID: 20673
		private const float PaintTimeSecondsBase = 1f;
	}
}
