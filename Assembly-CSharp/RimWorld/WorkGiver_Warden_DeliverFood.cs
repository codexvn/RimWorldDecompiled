using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001587 RID: 5511
	public class WorkGiver_Warden_DeliverFood : WorkGiver_Warden
	{
		// Token: 0x060082A4 RID: 33444 RVA: 0x00273B48 File Offset: 0x00271D48
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfPrisoner(pawn, t, false))
			{
				return null;
			}
			Pawn pawn2 = (Pawn)t;
			if (!pawn2.guest.CanBeBroughtFood)
			{
				return null;
			}
			if (!pawn2.Position.IsInPrisonCell(pawn2.Map))
			{
				return null;
			}
			if (pawn2.needs.food == null)
			{
				return null;
			}
			if (pawn2.needs.food.CurLevelPercentage >= pawn2.needs.food.PercentageThreshHungry + 0.02f)
			{
				return null;
			}
			if (WardenFeedUtility.ShouldBeFed(pawn2))
			{
				return null;
			}
			Thing thing;
			ThingDef thingDef;
			if (!FoodUtility.TryFindBestFoodSourceFor(pawn, pawn2, pawn2.needs.food.CurCategory == HungerCategory.Starving, out thing, out thingDef, false, true, false, false, false, false, false, false, false, true, false, FoodPreferability.Undefined))
			{
				return null;
			}
			if (thing.GetRoom(RegionType.Set_All) == pawn2.GetRoom(RegionType.Set_All))
			{
				return null;
			}
			if (WorkGiver_Warden_DeliverFood.FoodAvailableInRoomTo(pawn2))
			{
				return null;
			}
			float nutrition = FoodUtility.GetNutrition(pawn2, thing, thingDef);
			Job job = JobMaker.MakeJob(JobDefOf.DeliverFood, thing, pawn2);
			job.count = FoodUtility.WillIngestStackCountOf(pawn2, thingDef, nutrition);
			job.targetC = RCellFinder.SpotToChewStandingNear(pawn2, thing, null);
			return job;
		}

		// Token: 0x060082A5 RID: 33445 RVA: 0x00273C5C File Offset: 0x00271E5C
		private static bool FoodAvailableInRoomTo(Pawn prisoner)
		{
			if (prisoner.carryTracker.CarriedThing != null && WorkGiver_Warden_DeliverFood.NutritionAvailableForFrom(prisoner, prisoner.carryTracker.CarriedThing) > 0f)
			{
				return true;
			}
			float num = 0f;
			float num2 = 0f;
			Room room = prisoner.GetRoom(RegionType.Set_All);
			if (room == null)
			{
				return false;
			}
			List<Region> regions = room.Regions;
			for (int i = 0; i < regions.Count; i++)
			{
				Region region = regions[i];
				List<Thing> list = region.ListerThings.ThingsInGroup(ThingRequestGroup.FoodSourceNotPlantOrTree);
				for (int j = 0; j < list.Count; j++)
				{
					Thing thing = list[j];
					if (!thing.def.IsIngestible || thing.def.ingestible.preferability > FoodPreferability.DesperateOnlyForHumanlikes)
					{
						num2 += WorkGiver_Warden_DeliverFood.NutritionAvailableForFrom(prisoner, thing);
					}
				}
				List<Thing> list2 = region.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn);
				for (int k = 0; k < list2.Count; k++)
				{
					Pawn pawn = (Pawn)list2[k];
					if (pawn.IsPrisonerOfColony && pawn.needs.food != null && pawn.needs.food.CurLevelPercentage < pawn.needs.food.PercentageThreshHungry + 0.02f && (pawn.carryTracker.CarriedThing == null || !pawn.WillEat(pawn.carryTracker.CarriedThing, null, true, false)))
					{
						num += pawn.needs.food.NutritionWanted;
					}
				}
			}
			return num2 + 0.5f >= num;
		}

		// Token: 0x060082A6 RID: 33446 RVA: 0x00273DF8 File Offset: 0x00271FF8
		private static float NutritionAvailableForFrom(Pawn p, Thing foodSource)
		{
			if (foodSource.def.IsNutritionGivingIngestible && p.WillEat(foodSource, null, true, false))
			{
				return foodSource.GetStatValue(StatDefOf.Nutrition, true, -1) * (float)foodSource.stackCount;
			}
			if (p.RaceProps.ToolUser && p.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				Building_NutrientPasteDispenser building_NutrientPasteDispenser = foodSource as Building_NutrientPasteDispenser;
				if (building_NutrientPasteDispenser != null && building_NutrientPasteDispenser.CanDispenseNow && p.CanReach(building_NutrientPasteDispenser.InteractionCell, PathEndMode.OnCell, Danger.Some, false, false, TraverseMode.ByPawn))
				{
					return 99999f;
				}
			}
			return 0f;
		}
	}
}
