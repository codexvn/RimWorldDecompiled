using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001183 RID: 4483
	public class JobDriver_SmoothWall : JobDriver
	{
		// Token: 0x170010FC RID: 4348
		// (get) Token: 0x06006DAF RID: 28079 RVA: 0x00224DF7 File Offset: 0x00222FF7
		protected int BaseWorkAmount
		{
			get
			{
				return 6500;
			}
		}

		// Token: 0x170010FD RID: 4349
		// (get) Token: 0x06006DB0 RID: 28080 RVA: 0x00224DFE File Offset: 0x00222FFE
		protected DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.SmoothWall;
			}
		}

		// Token: 0x06006DB1 RID: 28081 RVA: 0x00224E08 File Offset: 0x00223008
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.job.targetA.Cell, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06006DB2 RID: 28082 RVA: 0x00224E65 File Offset: 0x00223065
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !this.job.ignoreDesignations && this.Map.designationManager.DesignationAt(this.TargetLocA, this.DesDef) == null);
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
			Toil doWork = ToilMaker.MakeToil("MakeNewToils");
			doWork.initAction = delegate()
			{
				this.workLeft = (float)this.BaseWorkAmount;
			};
			doWork.tickIntervalAction = delegate(int delta)
			{
				float num = doWork.actor.GetStatValue(StatDefOf.SmoothingSpeed, true, -1) * 1.7f * (float)delta;
				this.workLeft -= num;
				if (doWork.actor.skills != null)
				{
					doWork.actor.skills.Learn(SkillDefOf.Construction, 0.1f * (float)delta, false, false);
				}
				if (this.workLeft <= 0f)
				{
					this.DoEffect();
					Designation designation = this.Map.designationManager.DesignationAt(this.TargetLocA, this.DesDef);
					if (designation != null)
					{
						designation.Delete();
					}
					this.ReadyForNextToil();
					return;
				}
			};
			doWork.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			doWork.WithProgressBar(TargetIndex.A, () => 1f - this.workLeft / (float)this.BaseWorkAmount, false, -0.5f, false);
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			doWork.activeSkill = (() => SkillDefOf.Construction);
			yield return doWork;
			yield break;
		}

		// Token: 0x06006DB3 RID: 28083 RVA: 0x00224E78 File Offset: 0x00223078
		protected void DoEffect()
		{
			SmoothableWallUtility.Notify_SmoothedByPawn(SmoothableWallUtility.SmoothWall(base.TargetA.Thing, this.pawn), this.pawn);
		}

		// Token: 0x06006DB4 RID: 28084 RVA: 0x00224EA9 File Offset: 0x002230A9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		// Token: 0x04004CBA RID: 19642
		private float workLeft = -1000f;
	}
}
