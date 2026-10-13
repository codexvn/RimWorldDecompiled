using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001360 RID: 4960
	public abstract class JobDriver_PlantWork : JobDriver
	{
		// Token: 0x17001331 RID: 4913
		// (get) Token: 0x060079B6 RID: 31158 RVA: 0x0024BC57 File Offset: 0x00249E57
		protected Plant Plant
		{
			get
			{
				return (Plant)this.job.targetA.Thing;
			}
		}

		// Token: 0x17001332 RID: 4914
		// (get) Token: 0x060079B7 RID: 31159 RVA: 0x00002C42 File Offset: 0x00000E42
		protected virtual DesignationDef RequiredDesignation
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001333 RID: 4915
		// (get) Token: 0x060079B8 RID: 31160 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual PlantDestructionMode PlantDestructionMode
		{
			get
			{
				return PlantDestructionMode.Smash;
			}
		}

		// Token: 0x060079B9 RID: 31161 RVA: 0x0024BC6E File Offset: 0x00249E6E
		public static float WorkDonePerTick(Pawn actor, Plant plant)
		{
			return actor.GetStatValue(StatDefOf.PlantWorkSpeed, true, -1) * Mathf.Lerp(3.3f, 1f, plant.Growth);
		}

		// Token: 0x060079BA RID: 31162 RVA: 0x0024BC94 File Offset: 0x00249E94
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			LocalTargetInfo target = this.job.GetTarget(TargetIndex.A);
			if (target.IsValid && !this.pawn.Reserve(target, this.job, 1, -1, null, errorOnFailed, false))
			{
				return false;
			}
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x060079BB RID: 31163 RVA: 0x0024BCF3 File Offset: 0x00249EF3
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.Init();
			yield return Toils_JobTransforms.MoveCurrentTargetIntoQueue(TargetIndex.A);
			Toil initExtractTargetFromQueue = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A, (this.RequiredDesignation != null) ? ((Thing t) => this.Map.designationManager.DesignationOn(t, this.RequiredDesignation) != null) : null);
			yield return initExtractTargetFromQueue;
			yield return Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
			yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A, true);
			Toil toil = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false).JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, initExtractTargetFromQueue);
			if (this.RequiredDesignation != null)
			{
				toil.FailOnThingMissingDesignation(TargetIndex.A, this.RequiredDesignation);
			}
			yield return toil;
			Toil cut = ToilMaker.MakeToil("MakeNewToils");
			cut.tickIntervalAction = delegate(int delta)
			{
				Pawn actor = cut.actor;
				if (actor.skills != null)
				{
					actor.skills.Learn(SkillDefOf.Plants, this.xpPerTick * (float)delta, false, false);
				}
				Plant plant2 = this.Plant;
				this.workDone += JobDriver_PlantWork.WorkDonePerTick(actor, plant2) * (float)delta;
				if (this.workDone < plant2.def.plant.harvestWork)
				{
					return;
				}
				if (plant2.def.plant.harvestedThingDef != null)
				{
					StatDef stat = (plant2.def.plant.harvestedThingDef.IsDrug || plant2.def.plant.drugForHarvestPurposes) ? StatDefOf.DrugHarvestYield : StatDefOf.PlantHarvestYield;
					float statValue = actor.GetStatValue(stat, true, -1);
					if (actor.RaceProps.Humanlike && plant2.def.plant.harvestFailable && !plant2.Blighted && Rand.Value > statValue)
					{
						MoteMaker.ThrowText((this.pawn.DrawPos + plant2.DrawPos) / 2f, this.Map, "TextMote_HarvestFailed".Translate(), 3.65f);
					}
					else
					{
						int num = plant2.YieldNow();
						if (statValue > 1f)
						{
							num = GenMath.RoundRandom((float)num * statValue);
						}
						if (num > 0)
						{
							Thing thing = ThingMaker.MakeThing(plant2.def.plant.harvestedThingDef, null);
							thing.stackCount = num;
							if (actor.Faction != Faction.OfPlayer)
							{
								thing.SetForbidden(true, true);
							}
							Find.QuestManager.Notify_PlantHarvested(actor, thing);
							GenPlace.TryPlaceThing(thing, actor.Position, this.Map, ThingPlaceMode.Near, null, null, null, 1);
							actor.records.Increment(RecordDefOf.PlantsHarvested);
						}
						if (plant2.HarvestableNow)
						{
							foreach (ThingComp thingComp in plant2.AllComps)
							{
								foreach (ThingDefCountClass thingDefCountClass in thingComp.GetAdditionalHarvestYield())
								{
									Thing thing2 = ThingMaker.MakeThing(thingDefCountClass.thingDef, null);
									thing2.stackCount = thingDefCountClass.count;
									GenPlace.TryPlaceThing(thing2, actor.Position, this.Map, ThingPlaceMode.Near, null, null, null, 1);
								}
							}
						}
					}
				}
				plant2.def.plant.soundHarvestFinish.PlayOneShot(actor);
				plant2.PlantCollected(this.pawn, this.PlantDestructionMode);
				this.workDone = 0f;
				this.ReadyForNextToil();
			};
			cut.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			if (this.RequiredDesignation != null)
			{
				cut.FailOnThingMissingDesignation(TargetIndex.A, this.RequiredDesignation);
			}
			cut.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			cut.defaultCompleteMode = ToilCompleteMode.Never;
			Toil cut2 = cut;
			Plant plant = this.Plant;
			cut2.WithEffect((plant != null && plant.def.plant.IsTree) ? EffecterDefOf.Harvest_Tree : EffecterDefOf.Harvest_Plant, TargetIndex.A, null);
			cut.WithProgressBar(TargetIndex.A, () => this.workDone / this.Plant.def.plant.harvestWork, true, -0.5f, false);
			cut.PlaySustainerOrSound(() => this.Plant.def.plant.soundHarvesting, 1f);
			cut.activeSkill = (() => SkillDefOf.Plants);
			yield return cut;
			Toil toil2 = this.PlantWorkDoneToil();
			if (toil2 != null)
			{
				yield return toil2;
			}
			yield return Toils_Jump.Jump(initExtractTargetFromQueue);
			yield break;
		}

		// Token: 0x060079BC RID: 31164 RVA: 0x0024BD03 File Offset: 0x00249F03
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workDone, "workDone", 0f, false);
		}

		// Token: 0x060079BD RID: 31165 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void Init()
		{
		}

		// Token: 0x060079BE RID: 31166 RVA: 0x00002C42 File Offset: 0x00000E42
		protected virtual Toil PlantWorkDoneToil()
		{
			return null;
		}

		// Token: 0x04005283 RID: 21123
		private float workDone;

		// Token: 0x04005284 RID: 21124
		protected float xpPerTick;

		// Token: 0x04005285 RID: 21125
		protected const TargetIndex PlantInd = TargetIndex.A;
	}
}
