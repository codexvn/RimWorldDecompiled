using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020015CE RID: 5582
	public class WorkGiver_GrowerHarvest : WorkGiver_Grower
	{
		// Token: 0x17001435 RID: 5173
		// (get) Token: 0x060083EB RID: 33771 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060083EC RID: 33772 RVA: 0x002790B8 File Offset: 0x002772B8
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			Plant plant = c.GetPlant(pawn.Map);
			if (plant == null)
			{
				return false;
			}
			if (!plant.HarvestableNow || plant.LifeStage != PlantLifeStage.Mature)
			{
				return false;
			}
			CompPlantPreventCutting compPlantPreventCutting;
			if (!forced && plant.TryGetComp(out compPlantPreventCutting) && compPlantPreventCutting.PreventCutting)
			{
				return false;
			}
			if (!plant.CanYieldNow())
			{
				return false;
			}
			if (!plant.def.plant.autoHarvestable && !forced)
			{
				return false;
			}
			if (WorkGiver_Grower.wantedPlantDef == null)
			{
				WorkGiver_Grower.wantedPlantDef = WorkGiver_Grower.CalculateWantedPlantDef(c, pawn.Map);
			}
			Zone_Growing zone_Growing = c.GetZone(pawn.Map) as Zone_Growing;
			return (zone_Growing == null || zone_Growing.allowCut || plant.def == WorkGiver_Grower.wantedPlantDef) && PlantUtility.PawnWillingToCutPlant_Job(plant, pawn) && pawn.CanReserve(plant, 1, -1, null, forced);
		}

		// Token: 0x060083ED RID: 33773 RVA: 0x00279186 File Offset: 0x00277386
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.GetLord() != null || base.ShouldSkip(pawn, forced);
		}

		// Token: 0x060083EE RID: 33774 RVA: 0x0027919C File Offset: 0x0027739C
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Harvest);
			Map map = pawn.Map;
			Room room = c.GetRoom(map);
			float num = 0f;
			for (int i = 0; i < 40; i++)
			{
				IntVec3 intVec = c + GenRadial.RadialPattern[i];
				if (intVec.GetRoom(map) == room && this.HasJobOnCell(pawn, intVec, forced))
				{
					Plant plant = intVec.GetPlant(map);
					if (!(intVec != c) || plant.def == WorkGiver_Grower.CalculateWantedPlantDef(intVec, map))
					{
						num += plant.def.plant.harvestWork;
						if (intVec != c && num > 2400f)
						{
							break;
						}
						job.AddQueuedTarget(TargetIndex.A, plant);
					}
				}
			}
			if (job.targetQueueA != null && job.targetQueueA.Count >= 3)
			{
				job.targetQueueA.SortBy((LocalTargetInfo targ) => targ.Cell.DistanceToSquared(pawn.Position));
			}
			return job;
		}
	}
}
