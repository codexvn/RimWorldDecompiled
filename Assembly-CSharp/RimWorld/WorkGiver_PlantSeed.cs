using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001601 RID: 5633
	public class WorkGiver_PlantSeed : WorkGiver_Scanner
	{
		// Token: 0x17001460 RID: 5216
		// (get) Token: 0x060084F0 RID: 34032 RVA: 0x0027CF0B File Offset: 0x0027B10B
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Seed);
			}
		}

		// Token: 0x060084F1 RID: 34033 RVA: 0x0027CF14 File Offset: 0x0027B114
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.IdeologyActive && !ModsConfig.BiotechActive && !ModsConfig.AnomalyActive && !ModsConfig.OdysseyActive;
		}

		// Token: 0x060084F2 RID: 34034 RVA: 0x0027CF38 File Offset: 0x0027B138
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompPlantable compPlantable = t.TryGetComp<CompPlantable>();
			if (compPlantable == null || !pawn.CanReserve(t, 1, 1, null, forced))
			{
				return false;
			}
			List<IntVec3> plantCells = compPlantable.PlantCells;
			for (int i = 0; i < plantCells.Count; i++)
			{
				if (pawn.CanReach(plantCells[i], PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) && (compPlantable.Props.plantDefToSpawn.plant.vacuumResistant || plantCells[i].GetVacuum(t.Map) < 0.5f))
				{
					Plant plant = plantCells[i].GetPlant(t.Map);
					if (plant == null || this.CanDoCutJob(pawn, plant, forced))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060084F3 RID: 34035 RVA: 0x0027CFE8 File Offset: 0x0027B1E8
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompPlantable compPlantable = t.TryGetComp<CompPlantable>();
			if (compPlantable == null)
			{
				return null;
			}
			List<IntVec3> plantCells = compPlantable.PlantCells;
			for (int i = 0; i < plantCells.Count; i++)
			{
				if (pawn.CanReach(plantCells[i], PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) && (compPlantable.Props.plantDefToSpawn.plant.vacuumResistant || plantCells[i].GetVacuum(t.Map) < 0.5f))
				{
					Plant plant = plantCells[i].GetPlant(t.Map);
					if (plant == null)
					{
						Job job = JobMaker.MakeJob(JobDefOf.PlantSeed, t, plantCells[i]);
						job.playerForced = forced;
						job.plantDefToSow = compPlantable.Props.plantDefToSpawn;
						job.count = 1;
						return job;
					}
					if (this.CanDoCutJob(pawn, plant, forced))
					{
						return JobMaker.MakeJob(JobDefOf.CutPlant, plant);
					}
				}
			}
			return null;
		}

		// Token: 0x060084F4 RID: 34036 RVA: 0x0027D0DA File Offset: 0x0027B2DA
		private bool CanDoCutJob(Pawn pawn, Thing plant, bool forced)
		{
			return pawn.CanReserve(plant, 1, -1, null, forced) && PlantUtility.PawnWillingToCutPlant_Job(plant, pawn);
		}
	}
}
