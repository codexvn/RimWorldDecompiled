using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001495 RID: 5269
	public class JobGiver_GetFood : ThinkNode_JobGiver
	{
		// Token: 0x06007E9C RID: 32412 RVA: 0x002618C3 File Offset: 0x0025FAC3
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_GetFood jobGiver_GetFood = (JobGiver_GetFood)base.DeepCopy(resolve);
			jobGiver_GetFood.minCategory = this.minCategory;
			jobGiver_GetFood.maxLevelPercentage = this.maxLevelPercentage;
			jobGiver_GetFood.forceScanWholeMap = this.forceScanWholeMap;
			return jobGiver_GetFood;
		}

		// Token: 0x06007E9D RID: 32413 RVA: 0x002618F8 File Offset: 0x0025FAF8
		public override float GetPriority(Pawn pawn)
		{
			Need_Food food = pawn.needs.food;
			if (food == null)
			{
				return 0f;
			}
			if (pawn.needs.food.CurCategory < HungerCategory.Starving && FoodUtility.ShouldBeFedBySomeone(pawn))
			{
				return 0f;
			}
			if (food.CurCategory < this.minCategory)
			{
				return 0f;
			}
			if (food.CurLevelPercentage > this.maxLevelPercentage)
			{
				return 0f;
			}
			if (food.CurLevelPercentage < pawn.RaceProps.FoodLevelPercentageWantEat)
			{
				return 9.5f;
			}
			return 0f;
		}

		// Token: 0x06007E9E RID: 32414 RVA: 0x00261984 File Offset: 0x0025FB84
		protected override Job TryGiveJob(Pawn pawn)
		{
			Need_Food food = pawn.needs.food;
			if (food == null || food.CurCategory < this.minCategory || food.CurLevelPercentage > this.maxLevelPercentage)
			{
				return null;
			}
			FoodPreferability foodPreferability = FoodPreferability.Undefined;
			bool flag;
			if (pawn.AnimalOrWildMan())
			{
				flag = true;
			}
			else
			{
				Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Malnutrition, false);
				flag = (firstHediffOfDef != null && firstHediffOfDef.Severity > 0.4f);
			}
			if (pawn.IsMutant && pawn.mutant.Def.allowEatingCorpses)
			{
				flag = true;
				foodPreferability = FoodPreferability.DesperateOnly;
			}
			bool flag2 = pawn.needs.food.CurCategory == HungerCategory.Starving;
			bool desperate = flag2;
			bool canRefillDispenser = true;
			bool canUseInventory = true;
			bool canUsePackAnimalInventory = true;
			bool allowForbidden = false;
			bool allowCorpse = flag;
			bool allowSociallyImproper = false;
			FoodPreferability minPrefOverride = foodPreferability;
			Thing thing;
			ThingDef thingDef;
			if (!FoodUtility.TryFindBestFoodSourceFor(pawn, pawn, desperate, out thing, out thingDef, canRefillDispenser, canUseInventory, canUsePackAnimalInventory, allowForbidden, allowCorpse, allowSociallyImproper, pawn.IsWildMan(), this.forceScanWholeMap, false, false, false, minPrefOverride))
			{
				if (ModsConfig.OdysseyActive && pawn.RaceProps.canFishForFood)
				{
					return JobGiver_GetFood.TryFindFishJob(pawn);
				}
				return null;
			}
			else
			{
				Pawn pawn2 = thing as Pawn;
				if (pawn2 != null)
				{
					Job job = JobMaker.MakeJob(JobDefOf.PredatorHunt, pawn2);
					job.killIncappedTarget = true;
					return job;
				}
				if (thing is Plant && thing.def.plant.harvestedThingDef == thingDef)
				{
					return JobMaker.MakeJob(JobDefOf.Harvest, thing);
				}
				Building_NutrientPasteDispenser building_NutrientPasteDispenser = thing as Building_NutrientPasteDispenser;
				if (building_NutrientPasteDispenser != null && !building_NutrientPasteDispenser.HasEnoughFeedstockInHoppers())
				{
					Building building = building_NutrientPasteDispenser.AdjacentReachableHopper(pawn);
					if (building != null)
					{
						ISlotGroupParent hopperSgp = building as ISlotGroupParent;
						Job job2 = WorkGiver_CookFillHopper.HopperFillFoodJob(pawn, hopperSgp, false);
						if (job2 != null)
						{
							return job2;
						}
					}
					thing = FoodUtility.BestFoodSourceOnMap(pawn, pawn, flag2, out thingDef, FoodPreferability.MealLavish, false, !pawn.IsTeetotaler(), false, false, false, false, false, false, this.forceScanWholeMap, false, false, FoodPreferability.Undefined, null, false);
					if (thing == null)
					{
						return null;
					}
				}
				IntVec3 intVec;
				if (!Toils_Ingest.TryFindChairOrSpot(pawn, thing, out intVec))
				{
					return null;
				}
				float nutrition = FoodUtility.GetNutrition(pawn, thing, thingDef);
				Pawn_InventoryTracker pawn_InventoryTracker = thing.ParentHolder as Pawn_InventoryTracker;
				Pawn pawn3 = (pawn_InventoryTracker != null) ? pawn_InventoryTracker.pawn : null;
				if (pawn3 != null && pawn3 != pawn)
				{
					Job job3 = JobMaker.MakeJob(JobDefOf.TakeFromOtherInventory, thing, pawn3);
					job3.count = FoodUtility.WillIngestStackCountOf(pawn, thingDef, nutrition);
					return job3;
				}
				Job job4 = JobMaker.MakeJob(JobDefOf.Ingest, thing);
				job4.count = FoodUtility.WillIngestStackCountOf(pawn, thingDef, nutrition);
				return job4;
			}
		}

		// Token: 0x06007E9F RID: 32415 RVA: 0x00261BC8 File Offset: 0x0025FDC8
		public static Job TryFindFishJob(Pawn pawn)
		{
			Map map = pawn.MapHeld;
			if (map != null)
			{
				WaterBodyTracker waterBodyTracker = map.waterBodyTracker;
				if (waterBodyTracker != null)
				{
					if (!waterBodyTracker.AnyBodyContainsFish)
					{
						return null;
					}
					List<WaterBody> validBodies = (from waterBody in waterBodyTracker.Bodies
					where !waterBody.CommonFish.EnumerableNullOrEmpty<ThingDef>() && waterBody.Population > 10f
					select waterBody).ToList<WaterBody>();
					if (validBodies.Count == 0)
					{
						return null;
					}
					TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true);
					Region foundRegion = null;
					WaterBody foundBody = null;
					IntVec3 c;
					RegionTraverser.BreadthFirstTraverse(pawn.PositionHeld, map, (Region from, Region to) => to.Allows(traverseParams, false), delegate(Region r)
					{
						using (List<WaterBody>.Enumerator enumerator = validBodies.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								WaterBody validBody = enumerator.Current;
								if (r.extentsClose.Overlaps(validBody.Bounds) && r.Cells.Any((IntVec3 c) => validBody.cells.Contains(c)))
								{
									foundRegion = r;
									foundBody = validBody;
									return true;
								}
							}
						}
						return false;
					}, 999999, RegionType.Set_Passable);
					if (foundRegion == null)
					{
						return null;
					}
					List<IntVec3> source = (from c in foundRegion.Cells
					where c.Standable(map) && c.InBounds(map) && c.GetWaterBody(map) == foundBody
					select c).ToList<IntVec3>();
					for (int i = 0; i < 30; i++)
					{
						IntVec3 intVec = source.RandomElement<IntVec3>();
						if (intVec.IsValid && intVec.Standable(map))
						{
							c = WorkGiver_Fish.BestStandSpotFor(pawn, intVec, true);
							if (c.IsValid)
							{
								return JobMaker.MakeJob(JobDefOf.FishAnimal, intVec, c);
							}
						}
					}
					return null;
				}
			}
			return null;
		}

		// Token: 0x040054E4 RID: 21732
		private HungerCategory minCategory;

		// Token: 0x040054E5 RID: 21733
		private float maxLevelPercentage = 1f;

		// Token: 0x040054E6 RID: 21734
		public bool forceScanWholeMap;
	}
}
