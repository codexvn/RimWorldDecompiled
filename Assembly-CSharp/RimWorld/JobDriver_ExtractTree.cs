using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001355 RID: 4949
	public class JobDriver_ExtractTree : JobDriver
	{
		// Token: 0x17001324 RID: 4900
		// (get) Token: 0x0600796C RID: 31084 RVA: 0x0024ACE0 File Offset: 0x00248EE0
		protected Thing Target
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17001325 RID: 4901
		// (get) Token: 0x0600796D RID: 31085 RVA: 0x0024AD01 File Offset: 0x00248F01
		protected Plant Tree
		{
			get
			{
				return (Plant)this.Target.GetInnerIfMinified();
			}
		}

		// Token: 0x17001326 RID: 4902
		// (get) Token: 0x0600796E RID: 31086 RVA: 0x0024AD13 File Offset: 0x00248F13
		protected DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.ExtractTree;
			}
		}

		// Token: 0x17001327 RID: 4903
		// (get) Token: 0x0600796F RID: 31087 RVA: 0x0024AD1A File Offset: 0x00248F1A
		protected float TotalNeededWork
		{
			get
			{
				return this.Tree.def.plant.harvestWork;
			}
		}

		// Token: 0x06007970 RID: 31088 RVA: 0x0024AD31 File Offset: 0x00248F31
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
			Scribe_Values.Look<float>(ref this.totalNeededWork, "totalNeededWork", 0f, false);
		}

		// Token: 0x06007971 RID: 31089 RVA: 0x0024AD65 File Offset: 0x00248F65
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Target, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007972 RID: 31090 RVA: 0x0024AD88 File Offset: 0x00248F88
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnThingMissingDesignation(TargetIndex.A, this.Designation);
			this.FailOnForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			Toil doWork = ToilMaker.MakeToil("MakeNewToils").FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			doWork.initAction = delegate()
			{
				this.totalNeededWork = this.TotalNeededWork;
				this.workLeft = this.totalNeededWork;
			};
			doWork.tickIntervalAction = delegate(int delta)
			{
				this.workLeft -= JobDriver_PlantWork.WorkDonePerTick(this.pawn, this.Tree) * (float)delta;
				if (this.pawn.skills != null)
				{
					this.pawn.skills.Learn(SkillDefOf.Plants, 0.085f * (float)delta, false, false);
				}
				if (this.workLeft <= 0f)
				{
					SoundDefOf.Finish_Wood.PlayOneShot(SoundInfo.InMap(this.Tree, MaintenanceType.None));
					doWork.actor.jobs.curDriver.ReadyForNextToil();
				}
			};
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			doWork.WithProgressBar(TargetIndex.A, () => 1f - this.workLeft / this.totalNeededWork, false, -0.5f, false);
			doWork.WithEffect(EffecterDefOf.Harvest_Plant, TargetIndex.A, null);
			doWork.PlaySustainerOrSound(() => SoundDefOf.Interact_ConstructDirt, 1f);
			doWork.activeSkill = (() => SkillDefOf.Plants);
			yield return doWork;
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				IntVec3 position = this.Tree.Position;
				bool flag = Find.Selector.IsSelected(this.Tree);
				Thing thing = GenSpawn.Spawn(this.Tree.MakeMinified(DestroyMode.Vanish), position, this.pawn.Map, WipeMode.Vanish);
				if (flag && thing != null)
				{
					Find.Selector.Select(thing, false, false);
				}
				base.Map.designationManager.RemoveAllDesignationsOn(this.Target, false);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil;
			yield break;
		}

		// Token: 0x04005261 RID: 21089
		private float workLeft;

		// Token: 0x04005262 RID: 21090
		private float totalNeededWork;

		// Token: 0x04005263 RID: 21091
		public const TargetIndex TreeInd = TargetIndex.A;
	}
}
