using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012DC RID: 4828
	public class JobDriver_RemovePaintFloor : JobDriver
	{
		// Token: 0x1700128D RID: 4749
		// (get) Token: 0x06007645 RID: 30277 RVA: 0x00240A9C File Offset: 0x0023EC9C
		private IntVec3 PaintTarget
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Cell;
			}
		}

		// Token: 0x06007646 RID: 30278 RVA: 0x00233B26 File Offset: 0x00231D26
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x06007647 RID: 30279 RVA: 0x00240ABD File Offset: 0x0023ECBD
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil checkFinished = Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
			yield return checkFinished;
			yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A, true);
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch).JumpIf(() => base.Map.designationManager.DesignationAt(base.TargetLocA, DesignationDefOf.RemovePaintFloor) == null, checkFinished);
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
					base.Map.terrainGrid.SetTerrainColor(this.PaintTarget, null);
					base.Map.designationManager.TryRemoveDesignation(this.PaintTarget, DesignationDefOf.RemovePaintFloor);
					base.ReadyForNextToil();
					GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.Dye, null), this.pawn.Position, this.pawn.Map, ThingPlaceMode.Near, null, null, null, 1);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.PlaySustainerOrSound(SoundDefOf.Interact_RemovePaint, 1f);
			toil.WithProgressBar(TargetIndex.A, () => this.removalTime / 1f, true, -0.5f, false);
			toil.JumpIf(() => base.Map.designationManager.DesignationAt(base.TargetLocA, DesignationDefOf.RemovePaintFloor) == null, checkFinished);
			toil.handlingFacing = true;
			yield return toil;
			yield return Toils_Jump.Jump(checkFinished);
			yield break;
		}

		// Token: 0x040050DA RID: 20698
		private float removalTime;

		// Token: 0x040050DB RID: 20699
		private const TargetIndex PaintTargetIndex = TargetIndex.A;

		// Token: 0x040050DC RID: 20700
		private const float RemoveTimeSecondsBase = 1f;
	}
}
