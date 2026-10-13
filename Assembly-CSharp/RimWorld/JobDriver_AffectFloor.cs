using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200115D RID: 4445
	public abstract class JobDriver_AffectFloor : JobDriver
	{
		// Token: 0x170010C1 RID: 4289
		// (get) Token: 0x06006CC2 RID: 27842
		protected abstract int BaseWorkAmount { get; }

		// Token: 0x170010C2 RID: 4290
		// (get) Token: 0x06006CC3 RID: 27843
		protected abstract DesignationDef DesDef { get; }

		// Token: 0x170010C3 RID: 4291
		// (get) Token: 0x06006CC4 RID: 27844 RVA: 0x00002C42 File Offset: 0x00000E42
		protected virtual StatDef SpeedStat
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06006CC5 RID: 27845 RVA: 0x002228DC File Offset: 0x00220ADC
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, ReservationLayerDefOf.Floor, errorOnFailed, false);
		}

		// Token: 0x06006CC6 RID: 27846 RVA: 0x00222903 File Offset: 0x00220B03
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !this.job.ignoreDesignations && this.Map.designationManager.DesignationAt(this.TargetLocA, this.DesDef) == null);
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
			Toil doWork = ToilMaker.MakeToil("MakeNewToils");
			doWork.initAction = delegate()
			{
				this.workLeft = (float)this.BaseWorkAmount;
			};
			doWork.tickIntervalAction = delegate(int delta)
			{
				float num = (this.SpeedStat != null && !this.SpeedStat.Worker.IsDisabledFor(doWork.actor)) ? doWork.actor.GetStatValue(this.SpeedStat, true, -1) : 1f;
				num *= 1.7f * (float)delta;
				this.workLeft -= num;
				if (doWork.actor.skills != null)
				{
					doWork.actor.skills.Learn(SkillDefOf.Construction, 0.1f * (float)delta, false, false);
				}
				if (this.clearSnow)
				{
					this.Map.snowGrid.SetDepth(this.TargetLocA, 0f);
					SandGrid sandGrid = this.Map.sandGrid;
					if (sandGrid != null)
					{
						sandGrid.SetDepth(this.TargetLocA, 0f);
					}
				}
				if (this.workLeft <= 0f)
				{
					this.DoEffect(this.TargetLocA);
					Designation designation = this.Map.designationManager.DesignationAt(this.TargetLocA, this.DesDef);
					if (designation != null)
					{
						designation.Delete();
					}
					this.ReadyForNextToil();
				}
			};
			doWork.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			doWork.WithProgressBar(TargetIndex.A, () => 1f - this.workLeft / (float)this.BaseWorkAmount, false, -0.5f, false);
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			doWork.activeSkill = (() => SkillDefOf.Construction);
			yield return doWork;
			yield break;
		}

		// Token: 0x06006CC7 RID: 27847
		protected abstract void DoEffect(IntVec3 c);

		// Token: 0x06006CC8 RID: 27848 RVA: 0x00222913 File Offset: 0x00220B13
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		// Token: 0x04004C5C RID: 19548
		private float workLeft = -1000f;

		// Token: 0x04004C5D RID: 19549
		protected bool clearSnow;
	}
}
