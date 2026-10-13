using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001359 RID: 4953
	public class JobDriver_PlantSeed : JobDriver
	{
		// Token: 0x1700132A RID: 4906
		// (get) Token: 0x06007985 RID: 31109 RVA: 0x0024B144 File Offset: 0x00249344
		private IntVec3 PlantCell
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Cell;
			}
		}

		// Token: 0x1700132B RID: 4907
		// (get) Token: 0x06007986 RID: 31110 RVA: 0x0024B168 File Offset: 0x00249368
		private CompPlantable PlantableComp
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompPlantable>();
			}
		}

		// Token: 0x06007987 RID: 31111 RVA: 0x0022C53B File Offset: 0x0022A73B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, 1, null, errorOnFailed, false);
		}

		// Token: 0x06007988 RID: 31112 RVA: 0x0024B18E File Offset: 0x0024938E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => PlantUtility.AdjacentSowBlocker(this.job.plantDefToSow, this.PlantCell, base.Map) != null || !this.job.plantDefToSow.CanEverPlantAt(this.PlantCell, base.Map, false, false) || !this.PlantableComp.PlantCells.Contains(this.PlantCell));
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			yield return Toils_General.Do(delegate
			{
				LocalTargetInfo target = this.job.GetTarget(TargetIndex.A);
				if (target.HasThing && target.Thing.stackCount > this.job.count)
				{
					this.job.SetTarget(TargetIndex.C, target.Thing);
				}
			});
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false, true, false);
			yield return Toils_General.Do(delegate
			{
				LocalTargetInfo target = this.job.GetTarget(TargetIndex.C);
				if (target.IsValid && target.HasThing)
				{
					CompPlantable compPlantable = target.Thing.TryGetComp<CompPlantable>();
					if (compPlantable != null)
					{
						compPlantable.Notify_SeedRemovedFromStackForPlantingAt(this.PlantCell);
					}
				}
				CompPlantable plantableComp = this.PlantableComp;
				if (plantableComp == null)
				{
					return;
				}
				plantableComp.Notify_RemovedFromStackForPlantingAt(this.PlantCell);
			});
			yield return Toils_Haul.CarryHauledThingToCell(TargetIndex.B, PathEndMode.Touch);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.tickIntervalAction = delegate(int delta)
			{
				this.sowWorkDone += this.pawn.GetStatValue(StatDefOf.PlantWorkSpeed, true, -1) * (float)delta;
				if (this.pawn.skills != null)
				{
					this.pawn.skills.Learn(SkillDefOf.Plants, 0.085f * (float)delta, false, false);
				}
				if (this.sowWorkDone >= this.job.plantDefToSow.plant.sowWork)
				{
					base.ReadyForNextToil();
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.WithEffect(EffecterDefOf.Sow, TargetIndex.B, null);
			toil.PlaySustainerOrSound(() => SoundDefOf.Interact_Sow, 1f);
			toil.WithProgressBar(TargetIndex.B, () => this.sowWorkDone / this.job.plantDefToSow.plant.sowWork, true, -0.5f, false);
			toil.activeSkill = (() => SkillDefOf.Plants);
			yield return toil;
			yield return Toils_General.Do(delegate
			{
				this.PlantableComp.DoPlant(this.pawn, this.PlantCell, this.pawn.Map);
			});
			yield break;
		}

		// Token: 0x06007989 RID: 31113 RVA: 0x0024B19E File Offset: 0x0024939E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.sowWorkDone, "sowWorkDone", 0f, false);
		}

		// Token: 0x0400526D RID: 21101
		private float sowWorkDone;

		// Token: 0x0400526E RID: 21102
		private const TargetIndex SeedIndex = TargetIndex.A;

		// Token: 0x0400526F RID: 21103
		private const TargetIndex PlantCellIndex = TargetIndex.B;

		// Token: 0x04005270 RID: 21104
		private const TargetIndex OldSeedStackIndex = TargetIndex.C;
	}
}
