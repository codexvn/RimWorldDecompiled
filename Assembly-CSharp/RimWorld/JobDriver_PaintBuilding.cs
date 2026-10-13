using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012D2 RID: 4818
	public class JobDriver_PaintBuilding : JobDriver
	{
		// Token: 0x17001280 RID: 4736
		// (get) Token: 0x060075F7 RID: 30199 RVA: 0x0023FA3C File Offset: 0x0023DC3C
		private Thing PaintTarget
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060075F8 RID: 30200 RVA: 0x0023FA60 File Offset: 0x0023DC60
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x060075F9 RID: 30201 RVA: 0x0023FAAE File Offset: 0x0023DCAE
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil removeBadTargets = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A, null);
			yield return Toils_Jump.JumpIf(removeBadTargets, () => this.job.GetTargetQueue(TargetIndex.B).NullOrEmpty<LocalTargetInfo>());
			foreach (Toil toil in this.CollectDyeToils())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield return removeBadTargets;
			yield return Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
			yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A, true);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false).JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, removeBadTargets).JumpIfThingMissingDesignation(TargetIndex.A, DesignationDefOf.PaintBuilding, removeBadTargets);
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
					Designation designation = this.PaintTarget.Map.designationManager.DesignationOn(this.PaintTarget, DesignationDefOf.PaintBuilding);
					if (designation != null)
					{
						Building building = this.PaintTarget as Building;
						if (building != null)
						{
							building.ChangePaint(designation.colorDef);
						}
						else
						{
							this.PaintTarget.SetColor(designation.colorDef.color, true);
						}
						this.PaintTarget.Map.designationManager.RemoveDesignation(designation);
					}
					base.ReadyForNextToil();
				}
			};
			toil2.defaultCompleteMode = ToilCompleteMode.Never;
			toil2.WithEffect(EffecterDefOf.Paint, TargetIndex.A, null);
			toil2.WithProgressBar(TargetIndex.A, () => this.paintingTime / 1f, true, -0.5f, false);
			toil2.JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, removeBadTargets);
			toil2.JumpIfThingMissingDesignation(TargetIndex.A, DesignationDefOf.PaintBuilding, removeBadTargets);
			toil2.activeSkill = (() => SkillDefOf.Artistic);
			toil2.handlingFacing = true;
			yield return toil2;
			yield return Toils_Jump.Jump(removeBadTargets);
			yield break;
			yield break;
		}

		// Token: 0x060075FA RID: 30202 RVA: 0x0023FABE File Offset: 0x0023DCBE
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

		// Token: 0x060075FB RID: 30203 RVA: 0x0023FAC7 File Offset: 0x0023DCC7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.paintingTime, "paintingTime", 0f, false);
		}

		// Token: 0x040050AE RID: 20654
		private float paintingTime;

		// Token: 0x040050AF RID: 20655
		private const TargetIndex PaintTargetIndex = TargetIndex.A;

		// Token: 0x040050B0 RID: 20656
		private const TargetIndex DyeIndex = TargetIndex.B;

		// Token: 0x040050B1 RID: 20657
		private const float PaintTimeSecondsBase = 1f;
	}
}
