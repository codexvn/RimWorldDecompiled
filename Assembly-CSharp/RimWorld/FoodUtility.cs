using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001106 RID: 4358
	public static class FoodUtility
	{
		// Token: 0x06006A1B RID: 27163 RVA: 0x00216A40 File Offset: 0x00214C40
		public static bool WillEat(this Pawn p, Thing food, Pawn getter = null, bool careIfNotAcceptableForTitle = true, bool allowVenerated = false)
		{
			if (!p.WillEat(food.def, getter, careIfNotAcceptableForTitle, allowVenerated))
			{
				return false;
			}
			if (!allowVenerated && FoodUtility.IsVeneratedAnimalMeatOrCorpseOrHasIngredients(food, p))
			{
				return false;
			}
			if (p.foodRestriction != null && !p.IsSubhuman && !p.DevelopmentalStage.Baby())
			{
				FoodPolicy currentRespectedRestriction = p.foodRestriction.GetCurrentRespectedRestriction(getter);
				if (currentRespectedRestriction != null && !currentRespectedRestriction.Allows(food))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06006A1C RID: 27164 RVA: 0x00216AAC File Offset: 0x00214CAC
		public static bool WillEat(this Pawn p, ThingDef food, Pawn getter = null, bool careIfNotAcceptableForTitle = true, bool allowVenerated = false)
		{
			if (!p.FoodIsSuitable(food))
			{
				return false;
			}
			if (p.foodRestriction != null)
			{
				if (p.DevelopmentalStage.Baby())
				{
					if (!p.foodRestriction.BabyFoodAllowed(food))
					{
						return false;
					}
				}
				else
				{
					FoodPolicy currentRespectedRestriction = p.foodRestriction.GetCurrentRespectedRestriction(getter);
					if (currentRespectedRestriction != null && !currentRespectedRestriction.Allows(food) && Dialog_ManageFoodPolicies.FoodGlobalFilter.Allows(food))
					{
						return false;
					}
				}
			}
			return (allowVenerated || !FoodUtility.IsVeneratedAnimalMeatOrCorpse(food, p, null)) && (!careIfNotAcceptableForTitle || !FoodUtility.InappropriateForTitle(food, p, true));
		}

		// Token: 0x06006A1D RID: 27165 RVA: 0x00216B34 File Offset: 0x00214D34
		public static bool FoodIsSuitable(this Pawn p, ThingDef food)
		{
			if (p.needs.food == null)
			{
				return false;
			}
			if (p.IsMutant && p.mutant.Def.overrideFoodType)
			{
				return (p.mutant.Def.foodType & food.ingestible.foodType) > FoodTypeFlags.None;
			}
			return (!p.DevelopmentalStage.Baby() || food.ingestible.babiesCanIngest) && p.RaceProps.CanEverEat(food);
		}

		// Token: 0x06006A1E RID: 27166 RVA: 0x00216BB7 File Offset: 0x00214DB7
		public static bool DrugIsSuitable(this Pawn p, ThingDef drug)
		{
			return !p.IsMutant || MutantUtility.CanUseDrug(p, drug);
		}

		// Token: 0x06006A1F RID: 27167 RVA: 0x00216BD0 File Offset: 0x00214DD0
		public static bool HasVeneratedAnimalMeatOrCorpseIngredients(Thing food, Pawn ingester)
		{
			CompIngredients compIngredients = food.TryGetComp<CompIngredients>();
			int num = 0;
			for (;;)
			{
				int num2 = num;
				int? num3 = (compIngredients != null) ? new int?(compIngredients.ingredients.Count) : null;
				if (!(num2 < num3.GetValueOrDefault() & num3 != null))
				{
					return false;
				}
				if (FoodUtility.IsVeneratedAnimalMeatOrCorpse(compIngredients.ingredients[num], ingester, null))
				{
					break;
				}
				num++;
			}
			return true;
		}

		// Token: 0x06006A20 RID: 27168 RVA: 0x00216C37 File Offset: 0x00214E37
		public static bool IsVeneratedAnimalMeatOrCorpseOrHasIngredients(Thing food, Pawn ingester)
		{
			return FoodUtility.IsVeneratedAnimalMeatOrCorpse(food.def, ingester, food) || FoodUtility.HasVeneratedAnimalMeatOrCorpseIngredients(food, ingester);
		}

		// Token: 0x06006A21 RID: 27169 RVA: 0x00216C58 File Offset: 0x00214E58
		public static bool InappropriateForTitle(ThingDef food, Pawn p, bool allowIfStarving)
		{
			if (p.royalty == null)
			{
				return false;
			}
			Pawn_NeedsTracker needs = p.needs;
			if (((needs != null) ? needs.food : null) == null)
			{
				return false;
			}
			if ((allowIfStarving && p.needs.food.Starving) || (p.story != null && p.story.traits.HasTrait(TraitDefOf.Ascetic)) || p.IsPrisoner || (food.ingestible.joyKind != null && food.ingestible.joy > 0f))
			{
				return false;
			}
			Pawn_RoyaltyTracker royalty = p.royalty;
			RoyalTitle royalTitle = (royalty != null) ? royalty.MostSeniorTitle : null;
			return royalTitle != null && royalTitle.conceited && royalTitle.def.foodRequirement.Defined && !royalTitle.def.foodRequirement.Acceptable(food);
		}

		// Token: 0x06006A22 RID: 27170 RVA: 0x00216D28 File Offset: 0x00214F28
		public static bool TryFindBestFoodSourceFor(Pawn getter, Pawn eater, bool desperate, out Thing foodSource, out ThingDef foodDef, bool canRefillDispenser = true, bool canUseInventory = true, bool canUsePackAnimalInventory = false, bool allowForbidden = false, bool allowCorpse = true, bool allowSociallyImproper = false, bool allowHarvest = false, bool forceScanWholeMap = false, bool ignoreReservations = false, bool calculateWantedStackCount = false, bool allowVenerated = false, FoodPreferability minPrefOverride = FoodPreferability.Undefined)
		{
			FoodUtility.<>c__DisplayClass11_0 CS$<>8__locals1;
			CS$<>8__locals1.canUseInventory = canUseInventory;
			CS$<>8__locals1.canUsePackAnimalInventory = canUsePackAnimalInventory;
			CS$<>8__locals1.eater = eater;
			CS$<>8__locals1.getter = getter;
			CS$<>8__locals1.allowVenerated = allowVenerated;
			bool result;
			using (new ProfilerBlock("TryFindBestFoodSourceFor"))
			{
				if (CS$<>8__locals1.allowVenerated && FoodUtility.TryFindBestFoodSourceFor(CS$<>8__locals1.getter, CS$<>8__locals1.eater, desperate, out foodSource, out foodDef, canRefillDispenser, CS$<>8__locals1.canUseInventory, CS$<>8__locals1.canUsePackAnimalInventory, allowForbidden, allowCorpse, allowSociallyImproper, allowHarvest, forceScanWholeMap, ignoreReservations, calculateWantedStackCount, false, minPrefOverride))
				{
					result = true;
				}
				else
				{
					FoodUtility.<>c__DisplayClass11_1 CS$<>8__locals2;
					CS$<>8__locals2.canManipulateTools = (CS$<>8__locals1.getter.RaceProps.ToolUser && CS$<>8__locals1.getter.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation));
					bool flag = !CS$<>8__locals1.eater.IsTeetotaler();
					Thing thing = null;
					CS$<>8__locals2.packAnimal = null;
					if (CS$<>8__locals1.canUseInventory)
					{
						if (CS$<>8__locals2.canManipulateTools)
						{
							thing = FoodUtility.BestFoodInInventory(CS$<>8__locals1.getter, CS$<>8__locals1.eater, (minPrefOverride == FoodPreferability.Undefined) ? FoodPreferability.MealAwful : minPrefOverride, FoodPreferability.MealLavish, 0f, false, CS$<>8__locals1.allowVenerated);
						}
						if (thing != null)
						{
							if (CS$<>8__locals1.getter.Faction != Faction.OfPlayer)
							{
								foodSource = thing;
								foodDef = FoodUtility.GetFinalIngestibleDef(foodSource, false);
								return true;
							}
							CompRottable compRottable = thing.TryGetComp<CompRottable>();
							if (compRottable != null && compRottable.Stage == RotStage.Fresh && compRottable.TicksUntilRotAtCurrentTemp < 30000)
							{
								foodSource = thing;
								foodDef = FoodUtility.GetFinalIngestibleDef(foodSource, false);
								return true;
							}
						}
					}
					Pawn getter2 = CS$<>8__locals1.getter;
					Pawn eater2 = CS$<>8__locals1.eater;
					FoodPreferability maxPref = FoodPreferability.MealLavish;
					bool allowPlant = CS$<>8__locals1.getter == CS$<>8__locals1.eater;
					bool allowDrug = flag;
					bool allowDispenserFull = true;
					bool allowVenerated2 = CS$<>8__locals1.allowVenerated;
					ThingDef thingDef;
					Thing thing2 = FoodUtility.BestFoodSourceOnMap(getter2, eater2, desperate, out thingDef, maxPref, allowPlant, allowDrug, allowCorpse, allowDispenserFull, canRefillDispenser, allowForbidden, allowSociallyImproper, allowHarvest, forceScanWholeMap, ignoreReservations, calculateWantedStackCount, minPrefOverride, null, allowVenerated2);
					if (thing2 == null && thing == null)
					{
						thing = FoodUtility.<TryFindBestFoodSourceFor>g__FirstFoodInClosestPackAnimalInventory|11_0((minPrefOverride == FoodPreferability.Undefined) ? FoodPreferability.MealAwful : minPrefOverride, ref CS$<>8__locals1, ref CS$<>8__locals2);
					}
					if (thing != null || thing2 != null)
					{
						if (thing == null && thing2 != null)
						{
							foodSource = thing2;
							foodDef = thingDef;
							result = true;
						}
						else
						{
							ThingDef finalIngestibleDef = FoodUtility.GetFinalIngestibleDef(thing, false);
							if (thing2 == null)
							{
								foodSource = thing;
								foodDef = finalIngestibleDef;
								result = true;
							}
							else
							{
								float num = FoodUtility.FoodOptimality(CS$<>8__locals1.eater, thing2, thingDef, (float)(CS$<>8__locals1.getter.Position - thing2.Position).LengthManhattan, false);
								float num2 = FoodUtility.FoodOptimality(CS$<>8__locals1.eater, thing, finalIngestibleDef, (float)((CS$<>8__locals2.packAnimal == null) ? 0 : (CS$<>8__locals1.getter.Position - CS$<>8__locals2.packAnimal.Position).LengthManhattan), false);
								num2 -= 32f;
								if (num > num2)
								{
									foodSource = thing2;
									foodDef = thingDef;
									result = true;
								}
								else
								{
									foodSource = thing;
									foodDef = FoodUtility.GetFinalIngestibleDef(foodSource, false);
									result = true;
								}
							}
						}
					}
					else
					{
						if (CS$<>8__locals1.canUseInventory & CS$<>8__locals2.canManipulateTools)
						{
							thing = FoodUtility.BestFoodInInventory(CS$<>8__locals1.getter, CS$<>8__locals1.eater, FoodPreferability.DesperateOnly, FoodPreferability.MealLavish, 0f, flag, false);
							if (thing == null)
							{
								thing = FoodUtility.<TryFindBestFoodSourceFor>g__FirstFoodInClosestPackAnimalInventory|11_0(FoodPreferability.DesperateOnly, ref CS$<>8__locals1, ref CS$<>8__locals2);
							}
							if (thing != null)
							{
								foodSource = thing;
								foodDef = FoodUtility.GetFinalIngestibleDef(foodSource, false);
								return true;
							}
						}
						if (thing2 == null && CS$<>8__locals1.getter == CS$<>8__locals1.eater && CS$<>8__locals1.getter.IsPotentiallyPredator())
						{
							Pawn pawn = FoodUtility.BestPawnToHuntForPredator(CS$<>8__locals1.getter, forceScanWholeMap);
							if (pawn != null)
							{
								foodSource = pawn;
								foodDef = FoodUtility.GetFinalIngestibleDef(foodSource, false);
								return true;
							}
						}
						foodSource = null;
						foodDef = null;
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x06006A23 RID: 27171 RVA: 0x002170D4 File Offset: 0x002152D4
		public static ThingDef GetFinalIngestibleDef(Thing foodSource, bool harvest = false)
		{
			Building_NutrientPasteDispenser building_NutrientPasteDispenser = foodSource as Building_NutrientPasteDispenser;
			if (building_NutrientPasteDispenser != null)
			{
				return building_NutrientPasteDispenser.DispensableDef;
			}
			Pawn pawn = foodSource as Pawn;
			if (pawn != null)
			{
				return pawn.RaceProps.corpseDef;
			}
			if (harvest)
			{
				Plant plant = foodSource as Plant;
				if (plant != null && plant.HarvestableNow && plant.def.plant.harvestedThingDef.IsIngestible)
				{
					return plant.def.plant.harvestedThingDef;
				}
			}
			return foodSource.def;
		}

		// Token: 0x06006A24 RID: 27172 RVA: 0x0021714C File Offset: 0x0021534C
		public static Thing BestFoodInInventory(Pawn holder, Pawn eater = null, FoodPreferability minFoodPref = FoodPreferability.NeverForNutrition, FoodPreferability maxFoodPref = FoodPreferability.MealLavish, float minStackNutrition = 0f, bool allowDrug = false, bool allowVenerated = false)
		{
			if (holder.inventory == null)
			{
				return null;
			}
			if (eater == null)
			{
				eater = holder;
			}
			ThingOwner<Thing> innerContainer = holder.inventory.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				Thing thing = innerContainer[i];
				if (thing.def.IsNutritionGivingIngestible && thing.IngestibleNow && eater.WillEat(thing, holder, true, allowVenerated) && thing.def.ingestible.preferability >= minFoodPref && thing.def.ingestible.preferability <= maxFoodPref && (allowDrug || !thing.def.IsDrug) && FoodUtility.NutritionForEater(eater, thing) * (float)thing.stackCount >= minStackNutrition)
				{
					return thing;
				}
			}
			return null;
		}

		// Token: 0x06006A25 RID: 27173 RVA: 0x00217200 File Offset: 0x00215400
		public static int GetMaxAmountToPickup(Thing food, Pawn pawn, int wantedCount)
		{
			if (food == null)
			{
				return 0;
			}
			if (food is Building_NutrientPasteDispenser)
			{
				if (!pawn.CanReserve(food, 1, -1, null, false))
				{
					return 0;
				}
				return -1;
			}
			else if (food is Corpse)
			{
				if (!pawn.CanReserve(food, 1, -1, null, false))
				{
					return 0;
				}
				return 1;
			}
			else
			{
				if (wantedCount < 0)
				{
					wantedCount = 1;
				}
				int num = Math.Min(wantedCount, food.stackCount);
				if (food.Spawned && food.Map != null)
				{
					return Math.Min(num, food.Map.reservationManager.CanReserveStack(pawn, food, 10, null, false));
				}
				return num;
			}
		}

		// Token: 0x06006A26 RID: 27174 RVA: 0x00217298 File Offset: 0x00215498
		public static Thing BestFoodSourceOnMap(Pawn getter, Pawn eater, bool desperate, out ThingDef foodDef, FoodPreferability maxPref = FoodPreferability.MealLavish, bool allowPlant = true, bool allowDrug = true, bool allowCorpse = true, bool allowDispenserFull = true, bool allowDispenserEmpty = true, bool allowForbidden = false, bool allowSociallyImproper = false, bool allowHarvest = false, bool forceScanWholeMap = false, bool ignoreReservations = false, bool calculateWantedStackCount = false, FoodPreferability minPrefOverride = FoodPreferability.Undefined, float? minNutrition = null, bool allowVenerated = false)
		{
			foodDef = null;
			bool getterCanManipulate = getter.RaceProps.ToolUser && getter.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation);
			if (!getterCanManipulate && getter != eater)
			{
				string[] array = new string[6];
				int num = 0;
				Pawn getter2 = getter;
				array[num] = ((getter2 != null) ? getter2.ToString() : null);
				array[1] = " tried to find food to bring to ";
				int num2 = 2;
				Pawn eater2 = eater;
				array[num2] = ((eater2 != null) ? eater2.ToString() : null);
				array[3] = " but ";
				int num3 = 4;
				Pawn getter3 = getter;
				array[num3] = ((getter3 != null) ? getter3.ToString() : null);
				array[5] = " is incapable of Manipulation.";
				Log.Error(string.Concat(array));
				return null;
			}
			FoodPreferability minPref;
			if (minPrefOverride == FoodPreferability.Undefined)
			{
				if (eater.NonHumanlikeOrWildMan())
				{
					minPref = FoodPreferability.NeverForNutrition;
				}
				else if (desperate)
				{
					minPref = FoodPreferability.DesperateOnly;
				}
				else
				{
					minPref = ((eater.needs.food.CurCategory >= HungerCategory.UrgentlyHungry) ? FoodPreferability.RawBad : FoodPreferability.MealAwful);
					if (minPref == FoodPreferability.MealAwful && eater.genes != null && eater.genes.DontMindRawFood)
					{
						minPref = FoodPreferability.RawBad;
					}
				}
			}
			else
			{
				minPref = minPrefOverride;
			}
			Predicate<Thing> foodValidator = delegate(Thing t)
			{
				Building_NutrientPasteDispenser building_NutrientPasteDispenser = t as Building_NutrientPasteDispenser;
				IntVec3 intVec;
				if (building_NutrientPasteDispenser != null)
				{
					if (!allowDispenserFull || !getterCanManipulate || ThingDefOf.MealNutrientPaste.ingestible.preferability < minPref || ThingDefOf.MealNutrientPaste.ingestible.preferability > maxPref || !eater.WillEat(ThingDefOf.MealNutrientPaste, getter, true, allowVenerated) || (t.Faction != getter.Faction && t.Faction != getter.HostFaction) || (!allowForbidden && t.IsForbidden(getter)) || (!building_NutrientPasteDispenser.powerComp.PowerOn || (!allowDispenserEmpty && !building_NutrientPasteDispenser.HasEnoughFeedstockInHoppers())) || !t.InteractionCell.Standable(t.Map) || !FoodUtility.IsFoodSourceOnMapSociallyProper(t, getter, eater, allowSociallyImproper) || !getter.Map.reachability.CanReachNonLocal(getter.Position, new TargetInfo(t.InteractionCell, t.Map, false), PathEndMode.OnCell, TraverseParms.For(getter, Danger.Some, TraverseMode.ByPawn, false, false, false, true)))
					{
						return false;
					}
					intVec = building_NutrientPasteDispenser.InteractionCell;
				}
				else
				{
					if (t.def.ingestible.preferability < minPref || t.def.ingestible.preferability > maxPref || !eater.WillEat(t, getter, true, allowVenerated) || !t.def.IsNutritionGivingIngestible || !t.IngestibleNow || (!allowCorpse && t is Corpse) || (!allowDrug && t.def.IsDrug) || (!allowForbidden && t.IsForbidden(getter)) || (!desperate && t.IsNotFresh()) || t.IsDessicated() || !FoodUtility.IsFoodSourceOnMapSociallyProper(t, getter, eater, allowSociallyImproper) || (!getter.AnimalAwareOf(t) && !forceScanWholeMap))
					{
						return false;
					}
					int stackCount = 1;
					float singleFoodNutrition = FoodUtility.NutritionForEater(eater, t);
					if (minNutrition != null)
					{
						stackCount = FoodUtility.StackCountForNutrition(minNutrition.Value, singleFoodNutrition);
					}
					else if (calculateWantedStackCount)
					{
						stackCount = FoodUtility.WillIngestStackCountOf(eater, t.def, singleFoodNutrition);
					}
					if (!ignoreReservations && !getter.CanReserve(t, 10, stackCount, null, false))
					{
						return false;
					}
					intVec = t.PositionHeld;
				}
				return !getter.roping.IsRoped || intVec.InHorDistOf(getter.roping.RopedTo.Cell, 8f);
			};
			ThingRequest thingRequest;
			if ((eater.RaceProps.foodType & (FoodTypeFlags.Plant | FoodTypeFlags.Tree)) > FoodTypeFlags.None && allowPlant)
			{
				thingRequest = ThingRequest.ForGroup(ThingRequestGroup.FoodSource);
			}
			else
			{
				thingRequest = ThingRequest.ForGroup(ThingRequestGroup.FoodSourceNotPlantOrTree);
			}
			Thing bestThing;
			if (getter.RaceProps.Humanlike || getter.IsColonyMechPlayerControlled)
			{
				bestThing = FoodUtility.SpawnedFoodSearchInnerScan(eater, getter.Position, getter.Map.listerThings.ThingsMatching(thingRequest), PathEndMode.ClosestTouch, TraverseParms.For(getter, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, foodValidator);
				if (allowHarvest & getterCanManipulate)
				{
					int searchRegionsMax;
					if (forceScanWholeMap && bestThing == null)
					{
						searchRegionsMax = -1;
					}
					else
					{
						searchRegionsMax = 30;
					}
					Thing thing = GenClosest.ClosestThingReachable(getter.Position, getter.Map, ThingRequest.ForGroup(ThingRequestGroup.HarvestablePlant), PathEndMode.Touch, TraverseParms.For(getter, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, delegate(Thing x)
					{
						Plant plant = (Plant)x;
						if (!plant.HarvestableNow)
						{
							return false;
						}
						ThingDef harvestedThingDef = plant.def.plant.harvestedThingDef;
						return harvestedThingDef.IsNutritionGivingIngestible && eater.WillEat(harvestedThingDef, getter, true, allowVenerated) && getter.CanReserve(plant, 1, -1, null, false) && (allowForbidden || !plant.IsForbidden(getter)) && (bestThing == null || FoodUtility.GetFinalIngestibleDef(bestThing, false).ingestible.preferability < harvestedThingDef.ingestible.preferability);
					}, null, 0, searchRegionsMax, false, RegionType.Set_Passable, false, false);
					if (thing != null)
					{
						bestThing = thing;
						foodDef = FoodUtility.GetFinalIngestibleDef(thing, true);
					}
				}
				if (foodDef == null && bestThing != null)
				{
					foodDef = FoodUtility.GetFinalIngestibleDef(bestThing, false);
				}
			}
			else
			{
				int maxRegionsToScan = FoodUtility.GetMaxRegionsToScan(getter, forceScanWholeMap);
				FoodUtility.filtered.Clear();
				foreach (Thing thing2 in GenRadial.RadialDistinctThingsAround(getter.Position, getter.Map, 2f, true))
				{
					Pawn pawn = thing2 as Pawn;
					if (pawn != null && pawn != getter && pawn.IsAnimal && pawn.CurJob != null && pawn.CurJob.def == JobDefOf.Ingest && pawn.CurJob.GetTarget(TargetIndex.A).HasThing)
					{
						FoodUtility.filtered.Add(pawn.CurJob.GetTarget(TargetIndex.A).Thing);
					}
				}
				bool flag;
				if (!allowForbidden && ForbidUtility.CaresAboutForbidden(getter, true, false))
				{
					Pawn_PlayerSettings playerSettings = getter.playerSettings;
					flag = (((playerSettings != null) ? playerSettings.EffectiveAreaRestrictionInPawnCurrentMap : null) != null);
				}
				else
				{
					flag = false;
				}
				bool ignoreEntirelyForbiddenRegions = flag;
				Predicate<Thing> validator = (Thing t) => foodValidator(t) && !FoodUtility.filtered.Contains(t) && (t is Building_NutrientPasteDispenser || t.def.ingestible.preferability > FoodPreferability.DesperateOnly) && !t.IsNotFresh();
				bestThing = GenClosest.ClosestThingReachable(getter.Position, getter.Map, thingRequest, PathEndMode.OnCell, TraverseParms.For(getter, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, validator, null, 0, maxRegionsToScan, false, RegionType.Set_Passable, ignoreEntirelyForbiddenRegions, false);
				FoodUtility.filtered.Clear();
				if (bestThing == null)
				{
					desperate = true;
					bestThing = GenClosest.ClosestThingReachable(getter.Position, getter.Map, thingRequest, PathEndMode.OnCell, TraverseParms.For(getter, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, foodValidator, null, 0, maxRegionsToScan, false, RegionType.Set_Passable, ignoreEntirelyForbiddenRegions, false);
				}
				if (bestThing != null)
				{
					foodDef = FoodUtility.GetFinalIngestibleDef(bestThing, false);
				}
			}
			return bestThing;
		}

		// Token: 0x06006A27 RID: 27175 RVA: 0x002177A8 File Offset: 0x002159A8
		private static int GetMaxRegionsToScan(Pawn getter, bool forceScanWholeMap)
		{
			if (getter.RaceProps.Humanlike)
			{
				return -1;
			}
			if (forceScanWholeMap)
			{
				return -1;
			}
			if (getter.Faction == Faction.OfPlayer)
			{
				if (AnimalPenUtility.GetFixedAnimalFilter().Allows(getter))
				{
					CompAnimalPenMarker currentPenOf = AnimalPenUtility.GetCurrentPenOf(getter, false);
					if (currentPenOf != null)
					{
						return Mathf.Min(currentPenOf.PenState.ConnectedRegions.Count, 100);
					}
				}
				return 100;
			}
			return 30;
		}

		// Token: 0x06006A28 RID: 27176 RVA: 0x0021780C File Offset: 0x00215A0C
		private static bool IsFoodSourceOnMapSociallyProper(Thing t, Pawn getter, Pawn eater, bool allowSociallyImproper)
		{
			if (!allowSociallyImproper)
			{
				bool animalsCare = !getter.IsAnimal;
				if (!t.IsSociallyProper(getter) && !t.IsSociallyProper(eater, eater.IsPrisonerOfColony, animalsCare))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06006A29 RID: 27177 RVA: 0x00217844 File Offset: 0x00215A44
		public static float FoodOptimality(Pawn eater, Thing foodSource, ThingDef foodDef, float dist, bool takingToInventory = false)
		{
			float num = 300f;
			num -= dist;
			switch (foodDef.ingestible.preferability)
			{
			case FoodPreferability.NeverForNutrition:
				return -9999999f;
			case FoodPreferability.DesperateOnly:
				num -= 150f;
				break;
			case FoodPreferability.DesperateOnlyForHumanlikes:
				if (eater.RaceProps.Humanlike)
				{
					num -= 150f;
				}
				break;
			}
			CompRottable compRottable = foodSource.TryGetComp<CompRottable>();
			if (compRottable != null)
			{
				if (compRottable.Stage == RotStage.Dessicated)
				{
					return -9999999f;
				}
				if (!takingToInventory && compRottable.Stage == RotStage.Fresh && compRottable.TicksUntilRotAtCurrentTemp < 30000)
				{
					num += 12f;
				}
			}
			if (eater.needs != null && eater.needs.mood != null)
			{
				List<FoodUtility.ThoughtFromIngesting> list = FoodUtility.ThoughtsFromIngesting(eater, foodSource, foodDef);
				for (int i = 0; i < list.Count; i++)
				{
					num += FoodUtility.FoodOptimalityEffectFromMoodCurve.Evaluate(list[i].thought.stages[0].baseMoodEffect);
				}
			}
			if (foodDef.ingestible != null)
			{
				if (eater.RaceProps.Humanlike)
				{
					num += foodDef.ingestible.optimalityOffsetHumanlikes;
					if (eater.genes != null && foodDef.IsRawHumanFood() && eater.genes.DontMindRawFood)
					{
						num += ThingDefOf.MealSimple.ingestible.optimalityOffsetHumanlikes;
					}
				}
				else if (eater.IsAnimal)
				{
					num += foodDef.ingestible.optimalityOffsetFeedingAnimals;
				}
			}
			if (eater.story != null && eater.story.traits.AnyTraitHasIngestibleOverrides)
			{
				List<Trait> allTraits = eater.story.traits.allTraits;
				for (int j = 0; j < allTraits.Count; j++)
				{
					if (!allTraits[j].Suppressed)
					{
						List<IngestibleModifiers> ingestibleModifiers = allTraits[j].CurrentData.ingestibleModifiers;
						if (!ingestibleModifiers.NullOrEmpty<IngestibleModifiers>())
						{
							for (int k = 0; k < ingestibleModifiers.Count; k++)
							{
								if (ingestibleModifiers[k].ingestible == foodDef)
								{
									num += ingestibleModifiers[k].optimalityOffset;
								}
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06006A2A RID: 27178 RVA: 0x00217A54 File Offset: 0x00215C54
		private static Thing SpawnedFoodSearchInnerScan(Pawn eater, IntVec3 root, List<Thing> searchSet, PathEndMode peMode, TraverseParms traverseParams, float maxDistance = 9999f, Predicate<Thing> validator = null)
		{
			if (searchSet == null)
			{
				return null;
			}
			Pawn pawn = traverseParams.pawn ?? eater;
			int num = 0;
			int num2 = 0;
			Thing result = null;
			float num3 = float.MinValue;
			for (int i = 0; i < searchSet.Count; i++)
			{
				Thing thing = searchSet[i];
				num2++;
				float num4 = (float)(root - thing.Position).LengthManhattan;
				if (num4 <= maxDistance)
				{
					float num5 = FoodUtility.FoodOptimality(eater, thing, FoodUtility.GetFinalIngestibleDef(thing, false), num4, false);
					if (num5 >= num3 && pawn.Map.reachability.CanReach(root, thing, peMode, traverseParams) && thing.Spawned && (validator == null || validator(thing)))
					{
						result = thing;
						num3 = num5;
						num++;
					}
				}
			}
			return result;
		}

		// Token: 0x06006A2B RID: 27179 RVA: 0x00217B2C File Offset: 0x00215D2C
		public static void DebugFoodSearchFromMouse_Update()
		{
			IntVec3 root = UI.MouseCell();
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			if (pawn == null)
			{
				return;
			}
			if (pawn.Map != Find.CurrentMap)
			{
				return;
			}
			Thing thing = FoodUtility.SpawnedFoodSearchInnerScan(pawn, root, Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.FoodSourceNotPlantOrTree), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, null);
			if (thing != null)
			{
				GenDraw.DrawLineBetween(root.ToVector3Shifted(), thing.Position.ToVector3Shifted());
			}
		}

		// Token: 0x06006A2C RID: 27180 RVA: 0x00217BAC File Offset: 0x00215DAC
		public static void DebugFoodSearchFromMouse_OnGUI()
		{
			IntVec3 a = UI.MouseCell();
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			if (pawn == null)
			{
				return;
			}
			if (pawn.Map != Find.CurrentMap)
			{
				return;
			}
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Tiny;
			foreach (Thing thing in Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.FoodSourceNotPlantOrTree))
			{
				ThingDef finalIngestibleDef = FoodUtility.GetFinalIngestibleDef(thing, false);
				float num = FoodUtility.FoodOptimality(pawn, thing, finalIngestibleDef, (a - thing.Position).LengthHorizontal, false);
				Vector2 vector = thing.DrawPos.MapToUIPosition();
				Rect rect = new Rect(vector.x - 100f, vector.y - 100f, 200f, 200f);
				string text = num.ToString("F0");
				List<FoodUtility.ThoughtFromIngesting> list = FoodUtility.ThoughtsFromIngesting(pawn, thing, finalIngestibleDef);
				for (int i = 0; i < list.Count; i++)
				{
					text = string.Concat(new string[]
					{
						text,
						"\n",
						list[i].thought.defName,
						"(",
						FoodUtility.FoodOptimalityEffectFromMoodCurve.Evaluate(list[i].thought.stages[0].baseMoodEffect).ToString("F0"),
						")"
					});
				}
				Widgets.Label(rect, text);
			}
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06006A2D RID: 27181 RVA: 0x00217D64 File Offset: 0x00215F64
		private static Pawn BestPawnToHuntForPredator(Pawn predator, bool forceScanWholeMap)
		{
			if (predator.meleeVerbs.TryGetMeleeVerb(null) == null)
			{
				return null;
			}
			bool flag = false;
			if (predator.health.summaryHealth.SummaryHealthPercent < 0.25f)
			{
				flag = true;
			}
			FoodUtility.tmpPredatorCandidates.Clear();
			int maxRegionsToScan = FoodUtility.GetMaxRegionsToScan(predator, forceScanWholeMap);
			if (maxRegionsToScan < 0)
			{
				FoodUtility.tmpPredatorCandidates.AddRange(predator.Map.mapPawns.AllPawnsSpawned);
			}
			else
			{
				TraverseParms traverseParms = TraverseParms.For(predator, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true);
				RegionTraverser.BreadthFirstTraverse(predator.Position, predator.Map, (Region from, Region to) => to.Allows(traverseParms, true), delegate(Region x)
				{
					List<Thing> list = x.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn);
					for (int j = 0; j < list.Count; j++)
					{
						FoodUtility.tmpPredatorCandidates.Add((Pawn)list[j]);
					}
					return false;
				}, maxRegionsToScan, RegionType.Set_Passable);
			}
			Pawn pawn = null;
			float num = 0f;
			bool tutorialMode = TutorSystem.TutorialMode;
			for (int i = 0; i < FoodUtility.tmpPredatorCandidates.Count; i++)
			{
				Pawn pawn2 = FoodUtility.tmpPredatorCandidates[i];
				if (predator.GetDistrict(RegionType.Set_Passable) == pawn2.GetDistrict(RegionType.Set_Passable) && predator != pawn2 && (!flag || pawn2.Downed) && FoodUtility.IsAcceptablePreyFor(predator, pawn2) && predator.CanReach(pawn2, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn) && !pawn2.IsForbidden(predator) && (!tutorialMode || pawn2.Faction != Faction.OfPlayer))
				{
					float preyScoreFor = FoodUtility.GetPreyScoreFor(predator, pawn2);
					if (preyScoreFor > num || pawn == null)
					{
						num = preyScoreFor;
						pawn = pawn2;
					}
				}
			}
			FoodUtility.tmpPredatorCandidates.Clear();
			return pawn;
		}

		// Token: 0x06006A2E RID: 27182 RVA: 0x00217EE8 File Offset: 0x002160E8
		public static bool IsAcceptablePreyFor(Pawn predator, Pawn prey)
		{
			if (!prey.RaceProps.canBePredatorPrey)
			{
				return false;
			}
			if (!prey.RaceProps.IsFlesh)
			{
				return false;
			}
			if (!Find.Storyteller.difficulty.predatorsHuntHumanlikes && prey.RaceProps.Humanlike)
			{
				return false;
			}
			if (prey.BodySize > predator.RaceProps.maxPreyBodySize)
			{
				return false;
			}
			if (!prey.Downed)
			{
				if (prey.kindDef.combatPower > 2f * predator.kindDef.combatPower)
				{
					return false;
				}
				float num = prey.kindDef.combatPower * prey.health.summaryHealth.SummaryHealthPercent * prey.BodySize;
				float num2 = predator.kindDef.combatPower * predator.health.summaryHealth.SummaryHealthPercent * predator.BodySize;
				if (num >= num2)
				{
					return false;
				}
			}
			return (predator.Faction == null || prey.Faction == null || predator.HostileTo(prey)) && (predator.Faction == null || prey.HostFaction == null || predator.HostileTo(prey)) && (predator.Faction != Faction.OfPlayer || prey.Faction != Faction.OfPlayer) && (!predator.RaceProps.herdAnimal || predator.def != prey.def) && !prey.IsHiddenFromPlayer() && !prey.IsPsychologicallyInvisible() && (!ModsConfig.AnomalyActive || !prey.IsMutant || prey.mutant.Def.canBleed);
		}

		// Token: 0x06006A2F RID: 27183 RVA: 0x00218064 File Offset: 0x00216264
		public static float GetPreyScoreFor(Pawn predator, Pawn prey)
		{
			float num = prey.kindDef.combatPower / predator.kindDef.combatPower;
			float num2 = prey.health.summaryHealth.SummaryHealthPercent;
			float bodySizeFactor = prey.ageTracker.CurLifeStage.bodySizeFactor;
			float lengthHorizontal = (predator.Position - prey.Position).LengthHorizontal;
			if (prey.Downed)
			{
				num2 = Mathf.Min(num2, 0.2f);
			}
			float num3 = -lengthHorizontal - 56f * num2 * num2 * num * bodySizeFactor;
			if (prey.RaceProps.Humanlike)
			{
				num3 -= 35f;
			}
			else if (FoodUtility.IsPreyProtectedFromPredatorByFence(predator, prey))
			{
				num3 -= 17f;
			}
			return num3;
		}

		// Token: 0x06006A30 RID: 27184 RVA: 0x00218114 File Offset: 0x00216314
		private static bool IsPreyProtectedFromPredatorByFence(Pawn predator, Pawn prey)
		{
			if (predator.GetDistrict(RegionType.Set_Passable) == prey.GetDistrict(RegionType.Set_Passable))
			{
				return false;
			}
			TraverseParms traverseParams = TraverseParms.For(predator, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true).WithFenceblocked(true);
			return !predator.Map.reachability.CanReach(predator.Position, prey.Position, PathEndMode.ClosestTouch, traverseParams);
		}

		// Token: 0x06006A31 RID: 27185 RVA: 0x00218174 File Offset: 0x00216374
		public static void DebugDrawPredatorFoodSource()
		{
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			if (pawn == null)
			{
				return;
			}
			Thing thing;
			ThingDef thingDef;
			if (FoodUtility.TryFindBestFoodSourceFor(pawn, pawn, true, out thing, out thingDef, false, false, false, false, true, false, false, false, false, false, false, FoodPreferability.Undefined))
			{
				GenDraw.DrawLineBetween(pawn.Position.ToVector3Shifted(), thing.Position.ToVector3Shifted());
				if (!(thing is Pawn))
				{
					Pawn pawn2 = FoodUtility.BestPawnToHuntForPredator(pawn, true);
					if (pawn2 != null)
					{
						GenDraw.DrawLineBetween(pawn.Position.ToVector3Shifted(), pawn2.Position.ToVector3Shifted());
					}
				}
			}
		}

		// Token: 0x06006A32 RID: 27186 RVA: 0x00218207 File Offset: 0x00216407
		public static float MoodFromIngesting(Pawn ingester, Thing foodSource, ThingDef foodDef)
		{
			return FoodUtility.ThoughtsFromIngesting(ingester, foodSource, foodDef).Sum((FoodUtility.ThoughtFromIngesting ingestingThought) => ingestingThought.thought.stages[0].baseMoodEffect);
		}

		// Token: 0x06006A33 RID: 27187 RVA: 0x00218238 File Offset: 0x00216438
		public static List<FoodUtility.ThoughtFromIngesting> ThoughtsFromIngesting(Pawn ingester, Thing foodSource, ThingDef foodDef)
		{
			FoodUtility.ingestThoughts.Clear();
			FoodUtility.extraIngestThoughtsFromTraits.Clear();
			Pawn_NeedsTracker needs = ingester.needs;
			if (((needs != null) ? needs.mood : null) == null)
			{
				return FoodUtility.ingestThoughts;
			}
			MeatSourceCategory meatSourceCategory;
			if (foodSource.def.IsCorpse)
			{
				meatSourceCategory = FoodUtility.GetMeatSourceCategoryFromCorpse(foodSource);
			}
			else
			{
				meatSourceCategory = FoodUtility.GetMeatSourceCategory(foodDef);
			}
			Pawn_StoryTracker story = ingester.story;
			if (story != null)
			{
				TraitSet traits = story.traits;
				if (traits != null)
				{
					traits.GetExtraThoughtsFromIngestion(FoodUtility.extraIngestThoughtsFromTraits, foodDef, meatSourceCategory, true);
				}
			}
			if (!ingester.story.traits.HasTrait(TraitDefOf.Ascetic) && foodDef.ingestible.tasteThought != null)
			{
				FoodUtility.TryAddIngestThought(ingester, foodDef.ingestible.tasteThought, null, FoodUtility.ingestThoughts, foodDef, meatSourceCategory);
			}
			CompIngredients compIngredients = foodSource.TryGetComp<CompIngredients>();
			Building_NutrientPasteDispenser building_NutrientPasteDispenser = foodSource as Building_NutrientPasteDispenser;
			for (int i = 0; i < FoodUtility.extraIngestThoughtsFromTraits.Count; i++)
			{
				FoodUtility.TryAddIngestThought(ingester, FoodUtility.extraIngestThoughtsFromTraits[i], null, FoodUtility.ingestThoughts, foodDef, meatSourceCategory);
			}
			if (compIngredients != null)
			{
				bool flag = false;
				bool flag2 = false;
				for (int j = 0; j < compIngredients.ingredients.Count; j++)
				{
					bool flag3;
					bool flag4;
					FoodUtility.AddIngestThoughtsFromIngredient(compIngredients.ingredients[j], ingester, FoodUtility.ingestThoughts, out flag3, out flag4);
					if (flag3)
					{
						flag = true;
					}
					if (flag4)
					{
						flag2 = true;
					}
				}
				if (ModsConfig.IdeologyActive && flag2 && !flag)
				{
					FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteNonFungusMealWithPlants, ingester, foodDef, meatSourceCategory);
				}
			}
			else if (building_NutrientPasteDispenser != null)
			{
				Thing thing = building_NutrientPasteDispenser.FindFeedInAnyHopper();
				if (thing != null)
				{
					bool flag5;
					bool flag6;
					FoodUtility.AddIngestThoughtsFromIngredient(thing.def, ingester, FoodUtility.ingestThoughts, out flag5, out flag6);
				}
			}
			if (foodDef.ingestible.specialThoughtDirect != null)
			{
				FoodUtility.TryAddIngestThought(ingester, foodDef.ingestible.specialThoughtDirect, null, FoodUtility.ingestThoughts, foodDef, meatSourceCategory);
			}
			if (foodSource.IsNotFresh())
			{
				FoodUtility.TryAddIngestThought(ingester, ThoughtDefOf.AteRottenFood, null, FoodUtility.ingestThoughts, foodDef, meatSourceCategory);
			}
			if (ModsConfig.RoyaltyActive && FoodUtility.InappropriateForTitle(foodDef, ingester, false))
			{
				FoodUtility.TryAddIngestThought(ingester, ThoughtDefOf.AteFoodInappropriateForTitle, null, FoodUtility.ingestThoughts, foodDef, meatSourceCategory);
			}
			if (ingester.Ideo != null)
			{
				bool flag7 = FoodUtility.IsHumanlikeCorpseOrHumanlikeMeat(foodSource, foodDef);
				bool flag8 = FoodUtility.IsHumanlikeCorpseOrHumanlikeMeatOrIngredient(foodSource);
				if (flag8)
				{
					FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteHumanMeat, ingester, foodDef, meatSourceCategory);
					if (flag7)
					{
						FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteHumanMeatDirect, ingester, foodDef, meatSourceCategory);
					}
				}
				else if (!FoodUtility.AcceptableCannibalNonHumanlikeMeatFood(foodDef))
				{
					FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteNonCannibalFood, ingester, foodDef, meatSourceCategory);
				}
				FoodKind foodKind = FoodUtility.GetFoodKind(foodSource);
				FoodKind foodKind2 = FoodUtility.GetFoodKind(foodDef);
				if (!FoodUtility.AcceptableVegetarian(foodDef, foodKind, foodKind2))
				{
					if (!flag8 && foodKind != FoodKind.NonMeat)
					{
						FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteMeat, ingester, foodDef, meatSourceCategory);
					}
				}
				else if (!FoodUtility.AcceptableCarnivore(foodDef, foodKind, foodKind2))
				{
					FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteNonMeat, ingester, foodDef, meatSourceCategory);
				}
				if (FoodUtility.IsVeneratedAnimalMeatOrCorpse(foodDef, ingester, foodSource))
				{
					FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteVeneratedAnimalMeat, ingester, foodDef, meatSourceCategory);
				}
				if (meatSourceCategory == MeatSourceCategory.Insect)
				{
					FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteInsectMeatDirect, ingester, foodDef, meatSourceCategory);
				}
				if (ModsConfig.IdeologyActive && foodDef.thingCategories != null && foodDef.thingCategories.Contains(ThingCategoryDefOf.PlantFoodRaw))
				{
					if (foodDef.IsFungus)
					{
						FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteFungus, ingester, foodDef, meatSourceCategory);
					}
					else
					{
						FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteNonFungusPlant, ingester, foodDef, meatSourceCategory);
					}
				}
				if (foodDef.ingestible.ateEvent != null)
				{
					FoodUtility.AddThoughtsFromIdeo(foodDef.ingestible.ateEvent, ingester, foodDef, meatSourceCategory);
				}
			}
			return FoodUtility.ingestThoughts;
		}

		// Token: 0x06006A34 RID: 27188 RVA: 0x00218558 File Offset: 0x00216758
		private static void AddIngestThoughtsFromIngredient(ThingDef ingredient, Pawn ingester, List<FoodUtility.ThoughtFromIngesting> ingestThoughts, out bool ateFungus, out bool ateNonFungusRawPlant)
		{
			ateFungus = false;
			ateNonFungusRawPlant = false;
			if (ingredient.ingestible == null)
			{
				return;
			}
			MeatSourceCategory meatSourceCategory = FoodUtility.GetMeatSourceCategory(ingredient);
			FoodUtility.extraIngestThoughtsFromTraits.Clear();
			Pawn_StoryTracker story = ingester.story;
			if (story != null)
			{
				TraitSet traits = story.traits;
				if (traits != null)
				{
					traits.GetExtraThoughtsFromIngestion(FoodUtility.extraIngestThoughtsFromTraits, ingredient, meatSourceCategory, false);
				}
			}
			for (int i = 0; i < FoodUtility.extraIngestThoughtsFromTraits.Count; i++)
			{
				FoodUtility.TryAddIngestThought(ingester, FoodUtility.extraIngestThoughtsFromTraits[i], null, ingestThoughts, ingredient, meatSourceCategory);
			}
			if (ingredient.ingestible.specialThoughtAsIngredient != null)
			{
				FoodUtility.TryAddIngestThought(ingester, ingredient.ingestible.specialThoughtAsIngredient, null, ingestThoughts, ingredient, meatSourceCategory);
			}
			if (meatSourceCategory == MeatSourceCategory.Humanlike)
			{
				FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteHumanMeat, ingester, ingredient, meatSourceCategory);
				FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteHumanMeatAsIngredient, ingester, ingredient, meatSourceCategory);
			}
			else if (FoodUtility.IsVeneratedAnimalMeatOrCorpse(ingredient, ingester, null))
			{
				FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteVeneratedAnimalMeat, ingester, ingredient, meatSourceCategory);
			}
			if (meatSourceCategory == MeatSourceCategory.Insect)
			{
				FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteInsectMeatAsIngredient, ingester, ingredient, meatSourceCategory);
			}
			if (ModsConfig.IdeologyActive && ingredient.thingCategories != null && ingredient.thingCategories.Contains(ThingCategoryDefOf.PlantFoodRaw))
			{
				if (ingredient.IsFungus)
				{
					FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteFungusAsIngredient, ingester, ingredient, meatSourceCategory);
					ateFungus = true;
					return;
				}
				ateNonFungusRawPlant = true;
			}
		}

		// Token: 0x06006A35 RID: 27189 RVA: 0x0021867C File Offset: 0x0021687C
		private static void TryAddIngestThought(Pawn ingester, ThoughtDef def, Precept fromPrecept, List<FoodUtility.ThoughtFromIngesting> ingestThoughts, ThingDef foodDef, MeatSourceCategory meatSourceCategory)
		{
			if (ThoughtUtility.NullifyingGene(def, ingester) != null)
			{
				return;
			}
			if (ThoughtUtility.NullifyingHediff(def, ingester) != null)
			{
				return;
			}
			if (ThoughtUtility.NullifyingPrecept(def, ingester) != null)
			{
				return;
			}
			if (fromPrecept != null && !fromPrecept.def.enabledForNPCFactions && !ingester.CountsAsNonNPCForPrecepts())
			{
				return;
			}
			FoodUtility.ThoughtFromIngesting item = new FoodUtility.ThoughtFromIngesting
			{
				thought = def,
				fromPrecept = fromPrecept
			};
			if (ingester.story != null && ingester.story.traits != null)
			{
				if (!ingester.story.traits.IsThoughtFromIngestionDisallowed(def, foodDef, meatSourceCategory))
				{
					ingestThoughts.Add(item);
					return;
				}
			}
			else
			{
				ingestThoughts.Add(item);
			}
		}

		// Token: 0x06006A36 RID: 27190 RVA: 0x00218718 File Offset: 0x00216918
		private static void AddThoughtsFromIdeo(HistoryEventDef eventDef, Pawn ingester, ThingDef foodDef, MeatSourceCategory meatSourceCategory)
		{
			if (ingester.Ideo == null || eventDef == null)
			{
				return;
			}
			Dictionary<HistoryEventDef, List<Precept>> dictionary;
			if (!FoodUtility.ideoIngestThoughtsCache.TryGetValue(ingester.Ideo, out dictionary))
			{
				dictionary = new Dictionary<HistoryEventDef, List<Precept>>();
				FoodUtility.ideoIngestThoughtsCache[ingester.Ideo] = dictionary;
			}
			List<Precept> list;
			if (!dictionary.TryGetValue(eventDef, out list))
			{
				list = new List<Precept>();
				List<Precept> preceptsListForReading = ingester.Ideo.PreceptsListForReading;
				for (int i = 0; i < preceptsListForReading.Count; i++)
				{
					List<PreceptComp> comps = preceptsListForReading[i].def.comps;
					for (int j = 0; j < comps.Count; j++)
					{
						PreceptComp_SelfTookMemoryThought preceptComp_SelfTookMemoryThought = comps[j] as PreceptComp_SelfTookMemoryThought;
						if (preceptComp_SelfTookMemoryThought != null && preceptComp_SelfTookMemoryThought.eventDef == eventDef)
						{
							list.Add(preceptsListForReading[i]);
						}
					}
				}
				dictionary[eventDef] = list;
			}
			for (int k = 0; k < list.Count; k++)
			{
				Precept precept = list[k];
				List<PreceptComp> comps2 = precept.def.comps;
				for (int l = 0; l < comps2.Count; l++)
				{
					PreceptComp_SelfTookMemoryThought preceptComp_SelfTookMemoryThought2 = comps2[l] as PreceptComp_SelfTookMemoryThought;
					if (preceptComp_SelfTookMemoryThought2 != null && preceptComp_SelfTookMemoryThought2.eventDef == eventDef)
					{
						FoodUtility.TryAddIngestThought(ingester, preceptComp_SelfTookMemoryThought2.thought, precept, FoodUtility.ingestThoughts, foodDef, meatSourceCategory);
					}
				}
			}
		}

		// Token: 0x06006A37 RID: 27191 RVA: 0x00218860 File Offset: 0x00216A60
		public static bool IsHumanlikeCorpseOrHumanlikeMeatOrIngredient(Thing food)
		{
			CompIngredients compIngredients = food.TryGetComp<CompIngredients>();
			if (compIngredients == null)
			{
				return FoodUtility.IsHumanlikeCorpseOrHumanlikeMeat(food, food.def);
			}
			foreach (ThingDef foodDef in compIngredients.ingredients)
			{
				if (FoodUtility.IsHumanlikeCorpseOrHumanlikeMeat(food, foodDef))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006A38 RID: 27192 RVA: 0x002188D4 File Offset: 0x00216AD4
		public static bool IsHumanlikeCorpseOrHumanlikeMeat(Thing source, ThingDef foodDef)
		{
			if (source.def.IsCorpse)
			{
				return FoodUtility.GetMeatSourceCategoryFromCorpse(source) == MeatSourceCategory.Humanlike;
			}
			return FoodUtility.GetMeatSourceCategory(foodDef) == MeatSourceCategory.Humanlike;
		}

		// Token: 0x06006A39 RID: 27193 RVA: 0x002188F8 File Offset: 0x00216AF8
		public static bool IsInsectCorpseOrInsectMeatIngredient(Thing food)
		{
			CompIngredients compIngredients = food.TryGetComp<CompIngredients>();
			if (compIngredients != null)
			{
				using (List<ThingDef>.Enumerator enumerator = compIngredients.ingredients.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (FoodUtility.GetMeatSourceCategory(enumerator.Current) == MeatSourceCategory.Insect)
						{
							return true;
						}
					}
				}
				return false;
			}
			if (food.def.IsCorpse)
			{
				return FoodUtility.GetMeatSourceCategoryFromCorpse(food) == MeatSourceCategory.Insect;
			}
			return FoodUtility.GetMeatSourceCategory(food.def) == MeatSourceCategory.Insect;
		}

		// Token: 0x06006A3A RID: 27194 RVA: 0x00218984 File Offset: 0x00216B84
		public static bool IsVeneratedAnimalMeatOrCorpse(ThingDef foodDef, Pawn ingester, Thing source = null)
		{
			if (ingester.Ideo == null)
			{
				return false;
			}
			if (source != null && source.def.IsCorpse)
			{
				return ingester.Ideo.IsVeneratedAnimal(((Corpse)source).InnerPawn);
			}
			return foodDef.IsMeat && ingester.Ideo.IsVeneratedAnimal(foodDef.ingestible.sourceDef);
		}

		// Token: 0x06006A3B RID: 27195 RVA: 0x002189E4 File Offset: 0x00216BE4
		public static MeatSourceCategory GetMeatSourceCategory(ThingDef source)
		{
			if (source.ingestible == null)
			{
				return MeatSourceCategory.Undefined;
			}
			if ((source.ingestible.foodType & FoodTypeFlags.Meat) != FoodTypeFlags.Meat)
			{
				return MeatSourceCategory.NotMeat;
			}
			ThingDef sourceDef = source.ingestible.sourceDef;
			if (((sourceDef != null) ? sourceDef.race : null) != null && source.ingestible.sourceDef.race.Humanlike)
			{
				return MeatSourceCategory.Humanlike;
			}
			ThingDef sourceDef2 = source.ingestible.sourceDef;
			if (((sourceDef2 != null) ? sourceDef2.race.FleshType : null) != null && source.ingestible.sourceDef.race.FleshType == FleshTypeDefOf.Insectoid)
			{
				return MeatSourceCategory.Insect;
			}
			return MeatSourceCategory.Undefined;
		}

		// Token: 0x06006A3C RID: 27196 RVA: 0x00218A7C File Offset: 0x00216C7C
		public static MeatSourceCategory GetMeatSourceCategoryFromCorpse(Thing thing)
		{
			Corpse corpse = thing as Corpse;
			if (corpse == null)
			{
				return MeatSourceCategory.NotMeat;
			}
			if (corpse.InnerPawn.RaceProps.Humanlike)
			{
				return MeatSourceCategory.Humanlike;
			}
			if (corpse.InnerPawn.RaceProps.Insect)
			{
				return MeatSourceCategory.Insect;
			}
			return MeatSourceCategory.Undefined;
		}

		// Token: 0x06006A3D RID: 27197 RVA: 0x00218ABE File Offset: 0x00216CBE
		public static bool HasVegetarianRequiredPrecept(this Ideo ideo)
		{
			return ideo.HasPrecept(PreceptDefOf.MeatEating_Disapproved) || ideo.HasPrecept(PreceptDefOf.MeatEating_Horrible) || ideo.HasPrecept(PreceptDefOf.MeatEating_Abhorrent);
		}

		// Token: 0x06006A3E RID: 27198 RVA: 0x00218AE7 File Offset: 0x00216CE7
		public static bool HasMeatEatingRequiredPrecept(this Ideo ideo)
		{
			return ideo.HasPrecept(PreceptDefOf.MeatEating_NonMeat_Disapproved) || ideo.HasPrecept(PreceptDefOf.MeatEating_NonMeat_Horrible) || ideo.HasPrecept(PreceptDefOf.MeatEating_NonMeat_Abhorrent);
		}

		// Token: 0x06006A3F RID: 27199 RVA: 0x00218B10 File Offset: 0x00216D10
		public static bool HasHumanMeatEatingRequiredPrecept(this Ideo ideo)
		{
			return ideo.HasPrecept(PreceptDefOf.Cannibalism_Preferred) || ideo.HasPrecept(PreceptDefOf.Cannibalism_RequiredRavenous) || ideo.HasPrecept(PreceptDefOf.Cannibalism_RequiredStrong);
		}

		// Token: 0x06006A40 RID: 27200 RVA: 0x00218B3C File Offset: 0x00216D3C
		public static void GenerateGoodIngredients(Thing meal, Ideo ideo)
		{
			CompIngredients compIngredients = meal.TryGetComp<CompIngredients>();
			if (compIngredients != null)
			{
				compIngredients.ingredients.Clear();
				if (ideo.HasHumanMeatEatingRequiredPrecept())
				{
					compIngredients.ingredients.Add(ThingDefOf.Meat_Human);
					return;
				}
				if (ideo.HasMeatEatingRequiredPrecept())
				{
					compIngredients.ingredients.Add((from d in DefDatabase<ThingDef>.AllDefsListForReading
					where d.race != null && d.race.IsFlesh && d.race.Animal
					select d.race.meatDef).RandomElement<ThingDef>());
					return;
				}
				if (ideo.HasVegetarianRequiredPrecept())
				{
					compIngredients.ingredients.Add((from d in DefDatabase<ThingDef>.AllDefsListForReading
					where d.IsNutritionGivingIngestible && d.ingestible.HumanEdible && d.ingestible.foodType.HasFlag(FoodTypeFlags.VegetableOrFruit) && FoodUtility.MealCanBeMadeFrom(meal.def, d)
					select d).RandomElement<ThingDef>());
				}
			}
		}

		// Token: 0x06006A41 RID: 27201 RVA: 0x00218C20 File Offset: 0x00216E20
		public static bool MealCanBeMadeFrom(ThingDef mealDef, ThingDef ingredient)
		{
			Predicate<IngredientCount> <>9__1;
			foreach (RecipeDef recipeDef in from x in DefDatabase<RecipeDef>.AllDefs
			where x.ProducedThingDef == mealDef
			select x)
			{
				if (!recipeDef.ingredients.NullOrEmpty<IngredientCount>())
				{
					List<IngredientCount> ingredients = recipeDef.ingredients;
					Predicate<IngredientCount> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = ((IngredientCount x) => x.filter.Allows(ingredient)));
					}
					if (ingredients.Any(predicate))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006A42 RID: 27202 RVA: 0x00218CCC File Offset: 0x00216ECC
		public static int WillIngestStackCountOf(Pawn ingester, ThingDef def, float singleFoodNutrition)
		{
			Pawn_NeedsTracker needs = ingester.needs;
			if (((needs != null) ? needs.food : null) == null || def.IsDrug)
			{
				return def.ingestible.defaultNumToIngestAtOnce;
			}
			int num = FoodUtility.StackCountForNutrition(ingester.needs.food.NutritionWanted, singleFoodNutrition);
			if (def.ingestible.maxNumToIngestAtOnce > 0)
			{
				num = Mathf.Min(num, def.ingestible.maxNumToIngestAtOnce);
			}
			if (num < 1)
			{
				num = 1;
			}
			return num;
		}

		// Token: 0x06006A43 RID: 27203 RVA: 0x00218D3F File Offset: 0x00216F3F
		public static float GetBodyPartNutrition(Corpse corpse, BodyPartRecord part)
		{
			return FoodUtility.GetBodyPartNutrition(corpse.GetStatValue(StatDefOf.Nutrition, true, -1), corpse.InnerPawn, part);
		}

		// Token: 0x06006A44 RID: 27204 RVA: 0x00218D5C File Offset: 0x00216F5C
		public static float GetBodyPartNutrition(float currentCorpseNutrition, Pawn pawn, BodyPartRecord part)
		{
			HediffSet hediffSet = pawn.health.hediffSet;
			float coverageOfNotMissingNaturalParts = hediffSet.GetCoverageOfNotMissingNaturalParts(pawn.RaceProps.body.corePart);
			if (coverageOfNotMissingNaturalParts <= 0f)
			{
				return 0f;
			}
			float num = hediffSet.GetCoverageOfNotMissingNaturalParts(part) / coverageOfNotMissingNaturalParts;
			return currentCorpseNutrition * num;
		}

		// Token: 0x06006A45 RID: 27205 RVA: 0x00218DA7 File Offset: 0x00216FA7
		public static int StackCountForNutrition(float wantedNutrition, float singleFoodNutrition)
		{
			if (wantedNutrition <= 0.0001f)
			{
				return 0;
			}
			return Mathf.Max(Mathf.RoundToInt(wantedNutrition / singleFoodNutrition), 1);
		}

		// Token: 0x06006A46 RID: 27206 RVA: 0x00218DC1 File Offset: 0x00216FC1
		public static bool ShouldBeFedBySomeone(Pawn pawn)
		{
			return FeedPatientUtility.ShouldBeFed(pawn) || WardenFeedUtility.ShouldBeFed(pawn);
		}

		// Token: 0x06006A47 RID: 27207 RVA: 0x00218DD4 File Offset: 0x00216FD4
		public static void AddFoodPoisoningHediff(Pawn pawn, Thing ingestible, FoodPoisonCause cause)
		{
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.FoodPoisoning, false);
			if (firstHediffOfDef != null)
			{
				if (firstHediffOfDef.CurStageIndex != 2)
				{
					firstHediffOfDef.Severity = HediffDefOf.FoodPoisoning.stages[2].minSeverity - 0.001f;
				}
			}
			else
			{
				pawn.health.AddHediff(HediffMaker.MakeHediff(HediffDefOf.FoodPoisoning, pawn, null), null, null, null);
			}
			if (PawnUtility.ShouldSendNotificationAbout(pawn) && MessagesRepeatAvoider.MessageShowAllowed("MessageFoodPoisoning-" + pawn.thingIDNumber.ToString(), 0.1f))
			{
				Messages.Message("MessageFoodPoisoning".Translate(pawn.LabelShort, ingestible.LabelCapNoCount, cause.ToStringHuman().CapitalizeFirst(), pawn.Named("PAWN"), ingestible.Named("FOOD")).CapitalizeFirst(), pawn, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x06006A48 RID: 27208 RVA: 0x00218EDC File Offset: 0x002170DC
		public static float GetFoodPoisonChanceFactor(Pawn ingester)
		{
			if (ModsConfig.AnomalyActive && ingester.IsMutant && ingester.mutant.Def.preventIllnesses)
			{
				return 0f;
			}
			float num = Find.Storyteller.difficulty.foodPoisonChanceFactor;
			if (ingester.health != null && ingester.health.hediffSet != null)
			{
				foreach (Hediff hediff in ingester.health.hediffSet.hediffs)
				{
					HediffStage curStage = hediff.CurStage;
					if (curStage != null)
					{
						num *= curStage.foodPoisoningChanceFactor;
					}
				}
			}
			if (ModsConfig.BiotechActive && ingester.genes != null)
			{
				List<Gene> genesListForReading = ingester.genes.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					num *= genesListForReading[i].def.foodPoisoningChanceFactor;
				}
			}
			return num;
		}

		// Token: 0x06006A49 RID: 27209 RVA: 0x00218FD8 File Offset: 0x002171D8
		public static bool TryGetFoodPoisoningChanceOverrideFromTraits(Pawn pawn, Thing ingestible, out float poisonChanceOverride)
		{
			if (ModsConfig.AnomalyActive && pawn.IsMutant && pawn.mutant.Def.preventIllnesses)
			{
				poisonChanceOverride = 0f;
				return false;
			}
			if (pawn.story != null && pawn.story.traits.AnyTraitHasIngestibleOverrides)
			{
				List<Trait> allTraits = pawn.story.traits.allTraits;
				for (int i = 0; i < allTraits.Count; i++)
				{
					if (!allTraits[i].Suppressed)
					{
						List<IngestibleModifiers> ingestibleModifiers = allTraits[i].CurrentData.ingestibleModifiers;
						if (!ingestibleModifiers.NullOrEmpty<IngestibleModifiers>())
						{
							for (int j = 0; j < ingestibleModifiers.Count; j++)
							{
								if (ingestibleModifiers[j].ingestible == ingestible.def)
								{
									poisonChanceOverride = ingestibleModifiers[j].poisonChanceOverride;
									return true;
								}
							}
						}
					}
				}
			}
			poisonChanceOverride = 0f;
			return false;
		}

		// Token: 0x06006A4A RID: 27210 RVA: 0x002190B3 File Offset: 0x002172B3
		public static bool Starving(this Pawn p)
		{
			return p.needs != null && p.needs.food != null && p.needs.food.Starving;
		}

		// Token: 0x06006A4B RID: 27211 RVA: 0x002190DC File Offset: 0x002172DC
		public static float GetNutrition(Pawn eater, Thing foodSource, ThingDef foodDef)
		{
			if (foodSource == null || foodDef == null)
			{
				return 0f;
			}
			if (foodSource.def == foodDef)
			{
				return FoodUtility.NutritionForEater(eater, foodSource);
			}
			return foodDef.GetStatValueAbstract(StatDefOf.Nutrition, null);
		}

		// Token: 0x06006A4C RID: 27212 RVA: 0x00219107 File Offset: 0x00217307
		public static bool WillIngestFromInventoryNow(Pawn pawn, Thing inv)
		{
			return ((inv.def.IsNutritionGivingIngestible && pawn.WillEat(inv, null, true, false)) || pawn.CanTakeDrug(inv.def)) && inv.IngestibleNow;
		}

		// Token: 0x06006A4D RID: 27213 RVA: 0x00219138 File Offset: 0x00217338
		public static void IngestFromInventoryNow(Pawn pawn, Thing inv)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Ingest, inv);
			job.count = Mathf.Min(inv.stackCount, FoodUtility.WillIngestStackCountOf(pawn, inv.def, FoodUtility.NutritionForEater(pawn, inv)));
			pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
		}

		// Token: 0x06006A4E RID: 27214 RVA: 0x00219190 File Offset: 0x00217390
		public static FoodKind GetFoodKind(Thing food)
		{
			if (food == null)
			{
				return FoodKind.Any;
			}
			CompIngredients compIngredients = food.TryGetComp<CompIngredients>();
			if (compIngredients == null)
			{
				return FoodUtility.GetFoodKind(food.def);
			}
			if (compIngredients.ingredients.NullOrEmpty<ThingDef>())
			{
				return compIngredients.Props.noIngredientsFoodKind;
			}
			bool flag = false;
			for (int i = 0; i < compIngredients.ingredients.Count; i++)
			{
				if (FoodUtility.GetFoodKind(compIngredients.ingredients[i]) == FoodKind.Meat)
				{
					return FoodKind.Meat;
				}
				if (compIngredients.ingredients[i].IsAnimalProduct)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				return FoodKind.NonMeat;
			}
			return FoodKind.Any;
		}

		// Token: 0x06006A4F RID: 27215 RVA: 0x0021921C File Offset: 0x0021741C
		public static FoodKind GetFoodKind(ThingDef foodDef)
		{
			if (foodDef == null)
			{
				return FoodKind.Any;
			}
			if (ModsConfig.BiotechActive && foodDef == ThingDefOf.HemogenPack)
			{
				return FoodKind.Any;
			}
			if (!foodDef.IsIngestible)
			{
				return FoodKind.Any;
			}
			if (foodDef.IsMeat)
			{
				return FoodKind.Meat;
			}
			if (foodDef.ingestible != null && (foodDef.ingestible.foodType.HasFlag(FoodTypeFlags.Meat) || foodDef.ingestible.foodType.HasFlag(FoodTypeFlags.Corpse)))
			{
				return FoodKind.Meat;
			}
			if (foodDef.IsAnimalProduct)
			{
				return FoodKind.Any;
			}
			CompProperties_Ingredients compProperties = foodDef.GetCompProperties<CompProperties_Ingredients>();
			if (compProperties != null)
			{
				return compProperties.noIngredientsFoodKind;
			}
			return FoodKind.NonMeat;
		}

		// Token: 0x06006A50 RID: 27216 RVA: 0x002192B4 File Offset: 0x002174B4
		public static bool WillGiveNegativeThoughts(Thing food, Pawn pawn)
		{
			List<FoodUtility.ThoughtFromIngesting> list = FoodUtility.ThoughtsFromIngesting(pawn, food, FoodUtility.GetFinalIngestibleDef(food, false));
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].thought.stages[0].baseMoodEffect < 0f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006A51 RID: 27217 RVA: 0x00219307 File Offset: 0x00217507
		public static bool AcceptableVegetarian(Thing food)
		{
			return FoodUtility.AcceptableVegetarian(food, food.def);
		}

		// Token: 0x06006A52 RID: 27218 RVA: 0x00219315 File Offset: 0x00217515
		private static bool AcceptableVegetarian(Thing source, ThingDef def)
		{
			return FoodUtility.AcceptableVegetarian(def, FoodUtility.GetFoodKind(source), FoodUtility.GetFoodKind(def));
		}

		// Token: 0x06006A53 RID: 27219 RVA: 0x00219329 File Offset: 0x00217529
		private static bool AcceptableVegetarian(ThingDef def, FoodKind sourceKind, FoodKind defKind)
		{
			return def.IsProcessedFood || (sourceKind != FoodKind.Meat && defKind > FoodKind.Meat);
		}

		// Token: 0x06006A54 RID: 27220 RVA: 0x0021933E File Offset: 0x0021753E
		public static bool UnacceptableVegetarian(ThingDef foodDef)
		{
			return foodDef.IsIngestible && !foodDef.IsProcessedFood && FoodUtility.GetFoodKind(foodDef) == FoodKind.Meat;
		}

		// Token: 0x06006A55 RID: 27221 RVA: 0x0021935C File Offset: 0x0021755C
		public static bool AcceptableCannibalNonHumanlikeMeatFood(ThingDef foodDef)
		{
			return foodDef.IsDrug || foodDef.IsProcessedFood || foodDef.ingestible.drugCategory == DrugCategory.Medical || foodDef.ingestible.CachedNutrition <= 0f || (ModsConfig.BiotechActive && foodDef == ThingDefOf.HemogenPack);
		}

		// Token: 0x06006A56 RID: 27222 RVA: 0x002193AC File Offset: 0x002175AC
		public static bool AcceptableCarnivore(Thing food)
		{
			return FoodUtility.AcceptableCarnivore(food, food.def);
		}

		// Token: 0x06006A57 RID: 27223 RVA: 0x002193BA File Offset: 0x002175BA
		public static bool AcceptableCarnivore(Thing source, ThingDef def)
		{
			return FoodUtility.AcceptableCarnivore(def, FoodUtility.GetFoodKind(source), FoodUtility.GetFoodKind(def));
		}

		// Token: 0x06006A58 RID: 27224 RVA: 0x002193CE File Offset: 0x002175CE
		private static bool AcceptableCarnivore(ThingDef def, FoodKind sourceKind, FoodKind defKind)
		{
			return def.IsProcessedFood || (sourceKind != FoodKind.NonMeat && defKind != FoodKind.NonMeat) || def.IsDrug || def.ingestible.CachedNutrition <= 0f;
		}

		// Token: 0x06006A59 RID: 27225 RVA: 0x002193FF File Offset: 0x002175FF
		public static bool UnacceptableCarnivore(ThingDef foodDef)
		{
			return foodDef.IsIngestible && !foodDef.IsProcessedFood && !foodDef.IsDrug && FoodUtility.GetFoodKind(foodDef) == FoodKind.NonMeat;
		}

		// Token: 0x06006A5A RID: 27226 RVA: 0x00219424 File Offset: 0x00217624
		public static bool MaybeAcceptableCannibalDef(ThingDef foodDef)
		{
			if (!foodDef.IsIngestible)
			{
				return false;
			}
			if (FoodUtility.GetMeatSourceCategory(foodDef) == MeatSourceCategory.Humanlike || foodDef.ingestible.CachedNutrition == 0f || (foodDef.IsDrug && foodDef.ingestible.drugCategory == DrugCategory.Medical) || foodDef.IsProcessedFood || (foodDef.HasComp(typeof(CompIngredients)) && FoodUtility.GetFoodKind(foodDef) != FoodKind.NonMeat))
			{
				return true;
			}
			if (!foodDef.IsCorpse)
			{
				return false;
			}
			ThingDef sourceDef = foodDef.ingestible.sourceDef;
			if (sourceDef == null)
			{
				return false;
			}
			RaceProperties race = sourceDef.race;
			bool? flag = (race != null) ? new bool?(race.Humanlike) : null;
			bool flag2 = true;
			return flag.GetValueOrDefault() == flag2 & flag != null;
		}

		// Token: 0x06006A5B RID: 27227 RVA: 0x002194E8 File Offset: 0x002176E8
		public static bool MaybeAcceptableInsectMeatEatersDef(ThingDef foodDef)
		{
			if (!foodDef.IsIngestible)
			{
				return false;
			}
			if (FoodUtility.GetMeatSourceCategory(foodDef) == MeatSourceCategory.Insect || foodDef.ingestible.CachedNutrition == 0f || foodDef.IsDrug || foodDef.IsProcessedFood || foodDef.IsAnimalProduct || (foodDef.HasComp(typeof(CompIngredients)) && FoodUtility.GetFoodKind(foodDef) != FoodKind.NonMeat))
			{
				return true;
			}
			if (foodDef.IsCorpse)
			{
				ThingDef sourceDef = foodDef.ingestible.sourceDef;
				FleshTypeDef fleshTypeDef;
				if (sourceDef == null)
				{
					fleshTypeDef = null;
				}
				else
				{
					RaceProperties race = sourceDef.race;
					fleshTypeDef = ((race != null) ? race.FleshType : null);
				}
				return fleshTypeDef == FleshTypeDefOf.Insectoid;
			}
			return false;
		}

		// Token: 0x06006A5C RID: 27228 RVA: 0x00219588 File Offset: 0x00217788
		public static float NutritionForEater(Pawn eater, Thing food)
		{
			float num = food.GetStatValue(StatDefOf.Nutrition, true, -1);
			if (eater != null && ModsConfig.BiotechActive && food.def.IsRawHumanFood())
			{
				num *= eater.GetStatValue(StatDefOf.RawNutritionFactor, true, -1);
			}
			return num;
		}

		// Token: 0x06006A5D RID: 27229 RVA: 0x002195CB File Offset: 0x002177CB
		public static bool IsRawHumanFood(this ThingDef thingDef)
		{
			return thingDef.ingestible != null && thingDef.ingestible.HumanEdible && (thingDef.ingestible.preferability == FoodPreferability.RawBad || thingDef.ingestible.preferability == FoodPreferability.RawTasty);
		}

		// Token: 0x06006A5E RID: 27230 RVA: 0x00219604 File Offset: 0x00217804
		public static bool IsHumanFood(this ThingDef thingDef)
		{
			return thingDef.ingestible != null && thingDef.ingestible.HumanEdible && thingDef.ingestible.preferability != FoodPreferability.Undefined && thingDef.ingestible.preferability != FoodPreferability.NeverForNutrition;
		}

		// Token: 0x06006A60 RID: 27232 RVA: 0x00219710 File Offset: 0x00217910
		[CompilerGenerated]
		internal static Thing <TryFindBestFoodSourceFor>g__FirstFoodInClosestPackAnimalInventory|11_0(FoodPreferability foodPref, ref FoodUtility.<>c__DisplayClass11_0 A_1, ref FoodUtility.<>c__DisplayClass11_1 A_2)
		{
			Thing result = null;
			if ((A_1.canUseInventory & A_1.canUsePackAnimalInventory & A_2.canManipulateTools) && A_1.eater.IsColonist && A_1.getter.IsColonist && A_1.getter.Map != null)
			{
				foreach (Pawn pawn in A_1.getter.Map.mapPawns.SpawnedColonyAnimals)
				{
					Thing thing = FoodUtility.BestFoodInInventory(pawn, A_1.eater, foodPref, FoodPreferability.MealLavish, 0f, false, A_1.allowVenerated);
					if (thing != null && (A_2.packAnimal == null || (A_1.getter.Position - A_2.packAnimal.Position).LengthManhattan > (A_1.getter.Position - pawn.Position).LengthManhattan) && !pawn.IsForbidden(A_1.getter) && A_1.getter.CanReach(pawn, PathEndMode.OnCell, Danger.Some, false, false, TraverseMode.ByPawn))
					{
						A_2.packAnimal = pawn;
						result = thing;
					}
				}
			}
			return result;
		}

		// Token: 0x04004AFF RID: 19199
		public const int FoodPoisoningStageInitial = 2;

		// Token: 0x04004B00 RID: 19200
		public const int FoodPoisoningStageMajor = 1;

		// Token: 0x04004B01 RID: 19201
		public const int FoodPoisoningStageRecovering = 0;

		// Token: 0x04004B02 RID: 19202
		public const Danger DefaultFoodSearchDanger = Danger.Some;

		// Token: 0x04004B03 RID: 19203
		private static HashSet<Thing> filtered = new HashSet<Thing>();

		// Token: 0x04004B04 RID: 19204
		private static readonly SimpleCurve FoodOptimalityEffectFromMoodCurve = new SimpleCurve
		{
			{
				new CurvePoint(-100f, -600f),
				true
			},
			{
				new CurvePoint(-10f, -100f),
				true
			},
			{
				new CurvePoint(-5f, -70f),
				true
			},
			{
				new CurvePoint(-1f, -50f),
				true
			},
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(100f, 800f),
				true
			}
		};

		// Token: 0x04004B05 RID: 19205
		private static List<Pawn> tmpPredatorCandidates = new List<Pawn>();

		// Token: 0x04004B06 RID: 19206
		private static List<FoodUtility.ThoughtFromIngesting> ingestThoughts = new List<FoodUtility.ThoughtFromIngesting>();

		// Token: 0x04004B07 RID: 19207
		private static List<ThoughtDef> extraIngestThoughtsFromTraits = new List<ThoughtDef>();

		// Token: 0x04004B08 RID: 19208
		private static Dictionary<Ideo, Dictionary<HistoryEventDef, List<Precept>>> ideoIngestThoughtsCache = new Dictionary<Ideo, Dictionary<HistoryEventDef, List<Precept>>>();

		// Token: 0x02001107 RID: 4359
		public struct ThoughtFromIngesting
		{
			// Token: 0x04004B09 RID: 19209
			public ThoughtDef thought;

			// Token: 0x04004B0A RID: 19210
			public Precept fromPrecept;
		}
	}
}
