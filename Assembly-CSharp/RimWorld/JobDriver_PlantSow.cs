using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200135C RID: 4956
	public class JobDriver_PlantSow : JobDriver
	{
		// Token: 0x1700132E RID: 4910
		// (get) Token: 0x0600799D RID: 31133 RVA: 0x0024B608 File Offset: 0x00249808
		private Plant Plant
		{
			get
			{
				return (Plant)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x0600799E RID: 31134 RVA: 0x0024B62E File Offset: 0x0024982E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.sowWorkDone, "sowWorkDone", 0f, false);
		}

		// Token: 0x0600799F RID: 31135 RVA: 0x001E06E3 File Offset: 0x001DE8E3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060079A0 RID: 31136 RVA: 0x0024B64C File Offset: 0x0024984C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch).FailOn(() => PlantUtility.AdjacentSowBlocker(this.job.plantDefToSow, this.TargetA.Cell, this.Map) != null).FailOn(() => !this.job.plantDefToSow.CanNowPlantAt(this.TargetLocA, this.Map, false)).FailOn(delegate()
			{
				List<Thing> thingList = this.TargetA.Cell.GetThingList(this.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (thingList[i].def == this.job.plantDefToSow)
					{
						return true;
					}
				}
				return false;
			});
			Toil sowToil = ToilMaker.MakeToil("MakeNewToils");
			sowToil.initAction = delegate()
			{
				this.TargetThingA = GenSpawn.Spawn(this.job.plantDefToSow, this.TargetLocA, this.Map, WipeMode.Vanish);
				this.pawn.Reserve(this.TargetThingA, sowToil.actor.CurJob, 1, -1, null, true, false);
				Plant plant = this.Plant;
				plant.Growth = 0f;
				plant.sown = true;
			};
			sowToil.tickIntervalAction = delegate(int delta)
			{
				Pawn actor = sowToil.actor;
				Pawn_SkillTracker skills = actor.skills;
				if (skills != null)
				{
					skills.Learn(SkillDefOf.Plants, 0.085f * (float)delta, false, false);
				}
				Plant plant = this.Plant;
				if (plant.LifeStage != PlantLifeStage.Sowing)
				{
					Log.Error(string.Format("{0} getting sowing work while not in Sowing life stage.", this));
				}
				this.sowWorkDone += actor.GetStatValue(StatDefOf.PlantWorkSpeed, true, -1) * (float)delta;
				if (this.sowWorkDone < plant.def.plant.sowWork)
				{
					return;
				}
				plant.Growth = 0.0001f;
				this.Map.mapDrawer.MapMeshDirty(plant.Position, MapMeshFlagDefOf.Things);
				actor.records.Increment(RecordDefOf.PlantsSown);
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.SowedPlant, actor.Named(HistoryEventArgsNames.Doer)), true);
				if (plant.def.plant.humanFoodPlant)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.SowedHumanFoodPlant, actor.Named(HistoryEventArgsNames.Doer)), true);
				}
				this.ReadyForNextToil();
			};
			sowToil.defaultCompleteMode = ToilCompleteMode.Never;
			sowToil.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			sowToil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			sowToil.WithEffect(EffecterDefOf.Sow, TargetIndex.A, null);
			sowToil.WithProgressBar(TargetIndex.A, () => this.sowWorkDone / this.Plant.def.plant.sowWork, true, -0.5f, false);
			sowToil.PlaySustainerOrSound(() => SoundDefOf.Interact_Sow, 1f);
			sowToil.AddFinishAction(delegate
			{
				if (this.TargetThingA != null)
				{
					Plant plant = (Plant)sowToil.actor.CurJob.GetTarget(TargetIndex.A).Thing;
					if (this.sowWorkDone < plant.def.plant.sowWork && !this.TargetThingA.Destroyed)
					{
						this.TargetThingA.Destroy(DestroyMode.Vanish);
					}
				}
			});
			sowToil.activeSkill = (() => SkillDefOf.Plants);
			yield return sowToil;
			yield break;
		}

		// Token: 0x04005278 RID: 21112
		private float sowWorkDone;
	}
}
