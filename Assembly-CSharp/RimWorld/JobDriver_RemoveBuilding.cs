using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001177 RID: 4471
	public abstract class JobDriver_RemoveBuilding : JobDriver
	{
		// Token: 0x170010E9 RID: 4329
		// (get) Token: 0x06006D5D RID: 27997 RVA: 0x001E5D2F File Offset: 0x001E3F2F
		protected Thing Target
		{
			get
			{
				return this.job.targetA.Thing;
			}
		}

		// Token: 0x170010EA RID: 4330
		// (get) Token: 0x06006D5E RID: 27998 RVA: 0x00224153 File Offset: 0x00222353
		protected Building Building
		{
			get
			{
				return (Building)this.Target.GetInnerIfMinified();
			}
		}

		// Token: 0x170010EB RID: 4331
		// (get) Token: 0x06006D5F RID: 27999
		protected abstract DesignationDef Designation { get; }

		// Token: 0x170010EC RID: 4332
		// (get) Token: 0x06006D60 RID: 28000
		protected abstract float TotalNeededWork { get; }

		// Token: 0x170010ED RID: 4333
		// (get) Token: 0x06006D61 RID: 28001
		protected abstract EffecterDef WorkEffecter { get; }

		// Token: 0x06006D62 RID: 28002 RVA: 0x00224165 File Offset: 0x00222365
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
			Scribe_Values.Look<float>(ref this.totalNeededWork, "totalNeededWork", 0f, false);
		}

		// Token: 0x06006D63 RID: 28003 RVA: 0x00224199 File Offset: 0x00222399
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Target, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06006D64 RID: 28004 RVA: 0x002241BC File Offset: 0x002223BC
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (this.Designation != null)
			{
				this.FailOnThingMissingDesignation(TargetIndex.A, this.Designation);
			}
			this.FailOnForbidden(TargetIndex.A);
			this.FailOn(delegate()
			{
				CompExplosive compExplosive;
				return this.Building.TryGetComp(out compExplosive) && compExplosive.wickStarted;
			});
			yield return Toils_Goto.GotoThing(TargetIndex.A, (this.Target is Building_Trap) ? PathEndMode.OnCell : PathEndMode.Touch, false);
			Toil doWork = ToilMaker.MakeToil("MakeNewToils").FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			doWork.initAction = delegate()
			{
				this.totalNeededWork = this.TotalNeededWork;
				this.workLeft = this.totalNeededWork;
			};
			doWork.tickIntervalAction = delegate(int delta)
			{
				this.workLeft -= this.pawn.GetStatValue(StatDefOf.ConstructionSpeed, true, -1) * 1.7f * (float)delta;
				this.TickActionInterval(delta);
				if (this.workLeft <= 0f)
				{
					doWork.actor.jobs.curDriver.ReadyForNextToil();
				}
			};
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			if (this.WorkEffecter != null)
			{
				doWork.WithEffect(this.WorkEffecter, TargetIndex.A, null);
			}
			doWork.WithProgressBar(TargetIndex.A, () => 1f - this.workLeft / this.totalNeededWork, false, -0.5f, false);
			doWork.activeSkill = (() => SkillDefOf.Construction);
			yield return doWork;
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				if (this.Target.Faction != null)
				{
					this.Target.Faction.Notify_BuildingRemoved(this.Building, this.pawn);
				}
				this.FinishedRemoving();
				base.Map.designationManager.RemoveAllDesignationsOn(this.Target, false);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil;
			yield break;
		}

		// Token: 0x06006D65 RID: 28005 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void FinishedRemoving()
		{
		}

		// Token: 0x06006D66 RID: 28006 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void TickActionInterval(int delta)
		{
		}

		// Token: 0x04004C98 RID: 19608
		private float workLeft;

		// Token: 0x04004C99 RID: 19609
		private float totalNeededWork;
	}
}
