using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012D8 RID: 4824
	public class JobDriver_PaintFloor : JobDriver
	{
		// Token: 0x17001288 RID: 4744
		// (get) Token: 0x06007625 RID: 30245 RVA: 0x00240420 File Offset: 0x0023E620
		private IntVec3 PaintTarget
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Cell;
			}
		}

		// Token: 0x06007626 RID: 30246 RVA: 0x00240444 File Offset: 0x0023E644
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, ReservationLayerDefOf.Floor);
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x06007627 RID: 30247 RVA: 0x00240496 File Offset: 0x0023E696
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil extractFromQueue = Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A, true);
			Toil checkFinished = Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
			yield return Toils_Jump.JumpIf(extractFromQueue, () => this.job.GetTargetQueue(TargetIndex.B).NullOrEmpty<LocalTargetInfo>());
			foreach (Toil toil in this.CollectDyeToils())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield return checkFinished;
			yield return extractFromQueue;
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch).JumpIf(() => base.Map.designationManager.DesignationAt(base.TargetLocA, DesignationDefOf.PaintFloor) == null, checkFinished);
			Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
			toil2.initAction = delegate()
			{
				this.paintingTime = 0f;
			};
			toil2.tickIntervalAction = delegate(int delta)
			{
				this.pawn.rotationTracker.FaceTarget(this.PaintTarget);
				this.paintingTime += this.pawn.GetStatValue(StatDefOf.WorkSpeedGlobal, true, -1) / 60f * (float)delta;
				Pawn_SkillTracker skills = this.pawn.skills;
				if (skills != null)
				{
					skills.Learn(SkillDefOf.Artistic, 0.1f * (float)delta, false, false);
				}
				if (this.paintingTime >= 1f)
				{
					Thing carriedThing = this.pawn.carryTracker.CarriedThing;
					if (carriedThing != null)
					{
						Thing thing = carriedThing.SplitOff(1);
						if (thing != null)
						{
							thing.Destroy(DestroyMode.Vanish);
						}
					}
					Designation designation = base.Map.designationManager.DesignationAt(this.PaintTarget, DesignationDefOf.PaintFloor);
					if (designation != null)
					{
						base.Map.terrainGrid.SetTerrainColor(this.PaintTarget, designation.colorDef);
						base.Map.designationManager.RemoveDesignation(designation);
					}
					base.ReadyForNextToil();
				}
			};
			toil2.defaultCompleteMode = ToilCompleteMode.Never;
			toil2.WithEffect(EffecterDefOf.Paint, TargetIndex.A, null);
			toil2.WithProgressBar(TargetIndex.A, () => this.paintingTime / 1f, true, -0.5f, false);
			toil2.JumpIf(() => base.Map.designationManager.DesignationAt(base.TargetLocA, DesignationDefOf.PaintFloor) == null, checkFinished);
			toil2.activeSkill = (() => SkillDefOf.Artistic);
			toil2.handlingFacing = true;
			yield return toil2;
			yield return Toils_Jump.Jump(checkFinished);
			yield break;
			yield break;
		}

		// Token: 0x06007628 RID: 30248 RVA: 0x002404A6 File Offset: 0x0023E6A6
		private IEnumerable<Toil> CollectDyeToils()
		{
			Toil extract = Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.B, false);
			yield return extract;
			Toil jumpIfHaveTargetInQueue = Toils_Jump.JumpIfHaveTargetInQueue(TargetIndex.B, extract);
			Toil toil = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch, false).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return toil;
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, true, false, false, true, false);
			yield return jumpIfHaveTargetInQueue;
			yield break;
		}

		// Token: 0x06007629 RID: 30249 RVA: 0x002404AF File Offset: 0x0023E6AF
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.paintingTime, "paintingTime", 0f, false);
		}

		// Token: 0x040050C8 RID: 20680
		private float paintingTime;

		// Token: 0x040050C9 RID: 20681
		private const TargetIndex PaintTargetIndex = TargetIndex.A;

		// Token: 0x040050CA RID: 20682
		private const TargetIndex DyeIndex = TargetIndex.B;

		// Token: 0x040050CB RID: 20683
		private const float PaintTimeSecondsBase = 1f;
	}
}
