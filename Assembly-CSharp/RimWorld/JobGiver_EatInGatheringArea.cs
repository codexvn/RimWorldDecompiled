using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200143D RID: 5181
	public class JobGiver_EatInGatheringArea : ThinkNode_JobGiver
	{
		// Token: 0x06007DA7 RID: 32167 RVA: 0x0025D85C File Offset: 0x0025BA5C
		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			if (duty == null)
			{
				return null;
			}
			Pawn_NeedsTracker needs = pawn.needs;
			if (((needs != null) ? needs.food : null) == null)
			{
				return null;
			}
			if ((double)pawn.needs.food.CurLevelPercentage > 0.9)
			{
				return null;
			}
			IntVec3 cell = duty.focus.Cell;
			Thing thing = this.FindFood(pawn, cell);
			if (thing == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Ingest, thing);
			job.count = FoodUtility.WillIngestStackCountOf(pawn, thing.def, FoodUtility.NutritionForEater(pawn, thing));
			return job;
		}

		// Token: 0x06007DA8 RID: 32168 RVA: 0x0025D8F4 File Offset: 0x0025BAF4
		private Thing FindFood(Pawn pawn, IntVec3 gatheringSpot)
		{
			Predicate<Thing> validator = (Thing x) => x.IngestibleNow && x.def.IsNutritionGivingIngestible && GatheringsUtility.InGatheringArea(x.Position, gatheringSpot, pawn.Map) && !x.def.IsDrug && x.def.ingestible.preferability > FoodPreferability.RawBad && pawn.WillEat(x, null, true, false) && !x.IsForbidden(pawn) && x.IsSociallyProper(pawn) && pawn.CanReserve(x, 1, -1, null, false);
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.FoodSourceNotPlantOrTree), PathEndMode.ClosestTouch, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false), 14f, validator, null, 0, 12, false, RegionType.Set_Passable, false, false);
		}
	}
}
