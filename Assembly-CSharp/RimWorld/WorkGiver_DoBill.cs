using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015AD RID: 5549
	public class WorkGiver_DoBill : WorkGiver_Scanner
	{
		// Token: 0x17001416 RID: 5142
		// (get) Token: 0x06008331 RID: 33585 RVA: 0x000E9647 File Offset: 0x000E7847
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x06008332 RID: 33586 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Some;
		}

		// Token: 0x17001417 RID: 5143
		// (get) Token: 0x06008333 RID: 33587 RVA: 0x00275C45 File Offset: 0x00273E45
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				if (this.def.fixedBillGiverDefs != null && this.def.fixedBillGiverDefs.Count == 1)
				{
					return ThingRequest.ForDef(this.def.fixedBillGiverDefs[0]);
				}
				return ThingRequest.ForGroup(ThingRequestGroup.PotentialBillGiver);
			}
		}

		// Token: 0x06008334 RID: 33588 RVA: 0x00275C88 File Offset: 0x00273E88
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.PotentialBillGiver);
			for (int i = 0; i < list.Count; i++)
			{
				IBillGiver billGiver = list[i] as IBillGiver;
				if (billGiver != null && billGiver != pawn && this.ThingIsUsableBillGiver(list[i]) && billGiver.BillStack.AnyShouldDoNow)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06008335 RID: 33589 RVA: 0x00275CEC File Offset: 0x00273EEC
		public override Job JobOnThing(Pawn pawn, Thing thing, bool forced = false)
		{
			IBillGiver billGiver = thing as IBillGiver;
			if (billGiver == null || !this.ThingIsUsableBillGiver(thing) || !billGiver.BillStack.AnyShouldDoNow || !billGiver.UsableForBillsAfterFueling() || !pawn.CanReserve(thing, 1, -1, null, forced) || thing.IsBurning())
			{
				return null;
			}
			if (thing.def.hasInteractionCell && !pawn.CanReserveSittableOrSpot(thing.InteractionCell, thing, forced))
			{
				return null;
			}
			CompRefuelable compRefuelable = thing.TryGetComp<CompRefuelable>();
			if (compRefuelable == null || compRefuelable.HasFuel)
			{
				billGiver.BillStack.RemoveIncompletableBills();
				return this.StartOrResumeBillJob(pawn, billGiver, forced);
			}
			if (!RefuelWorkGiverUtility.CanRefuel(pawn, thing, forced))
			{
				return null;
			}
			return RefuelWorkGiverUtility.RefuelJob(pawn, thing, forced, null, null);
		}

		// Token: 0x06008336 RID: 33590 RVA: 0x00275D9C File Offset: 0x00273F9C
		private static UnfinishedThing ClosestUnfinishedThingForBill(Pawn pawn, Bill_ProductionWithUft bill)
		{
			Predicate<Thing> <>9__1;
			Predicate<Thing> validator = delegate(Thing t)
			{
				if (!t.IsForbidden(pawn) && ((UnfinishedThing)t).Recipe == bill.recipe && ((UnfinishedThing)t).Creator == pawn)
				{
					List<Thing> ingredients = ((UnfinishedThing)t).ingredients;
					Predicate<Thing> match;
					if ((match = <>9__1) == null)
					{
						match = (<>9__1 = ((Thing x) => bill.IsFixedOrAllowedIngredient(x.def)));
					}
					if (ingredients.TrueForAll(match))
					{
						return pawn.CanReserve(t, 1, -1, null, false);
					}
				}
				return false;
			};
			return (UnfinishedThing)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(bill.recipe.unfinishedThingDef), PathEndMode.InteractionCell, TraverseParms.For(pawn, pawn.NormalMaxDanger(), TraverseMode.ByPawn, false, false, false, true), 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false, false);
		}

		// Token: 0x06008337 RID: 33591 RVA: 0x00275E2C File Offset: 0x0027402C
		private static Job FinishUftJob(Pawn pawn, UnfinishedThing uft, Bill_ProductionWithUft bill)
		{
			if (uft.Creator != pawn)
			{
				string[] array = new string[6];
				array[0] = "Tried to get FinishUftJob for ";
				array[1] = ((pawn != null) ? pawn.ToString() : null);
				array[2] = " finishing ";
				array[3] = ((uft != null) ? uft.ToString() : null);
				array[4] = " but its creator is ";
				int num = 5;
				Pawn creator = uft.Creator;
				array[num] = ((creator != null) ? creator.ToString() : null);
				Log.Error(string.Concat(array));
				return null;
			}
			Job job = WorkGiverUtility.HaulStuffOffBillGiverJob(pawn, bill.billStack.billGiver, uft);
			if (job != null && job.targetA.Thing != uft)
			{
				return job;
			}
			Job job2 = JobMaker.MakeJob(JobDefOf.DoBill, (Thing)bill.billStack.billGiver);
			job2.bill = bill;
			job2.targetQueueB = new List<LocalTargetInfo>
			{
				uft
			};
			job2.countQueue = new List<int>
			{
				1
			};
			job2.haulMode = HaulMode.ToCellNonStorage;
			return job2;
		}

		// Token: 0x06008338 RID: 33592 RVA: 0x00275F20 File Offset: 0x00274120
		private Job StartOrResumeBillJob(Pawn pawn, IBillGiver giver, bool forced = false)
		{
			bool flag = FloatMenuMakerMap.makingFor == pawn;
			for (int i = 0; i < giver.BillStack.Count; i++)
			{
				Bill bill = giver.BillStack[i];
				if ((bill.recipe.requiredGiverWorkType == null || bill.recipe.requiredGiverWorkType == this.def.workType) && (Find.TickManager.TicksGame > bill.nextTickToSearchForIngredients || FloatMenuMakerMap.makingFor == pawn) && bill.ShouldDoNow() && bill.PawnAllowedToStartAnew(pawn))
				{
					SkillRequirement skillRequirement = bill.recipe.FirstSkillRequirementPawnDoesntSatisfy(pawn);
					if (skillRequirement != null)
					{
						JobFailReason.Is("UnderRequiredSkill".Translate(skillRequirement.minLevel), bill.Label);
					}
					else
					{
						Bill_Medical bill_Medical = bill as Bill_Medical;
						if (bill_Medical != null)
						{
							if (bill_Medical.IsSurgeryViolationOnExtraFactionMember(pawn))
							{
								JobFailReason.Is("SurgeryViolationFellowFactionMember".Translate(), null);
								goto IL_4CE;
							}
							if (!pawn.CanReserve(bill_Medical.GiverPawn, 1, -1, null, forced))
							{
								Pawn pawn2 = pawn.MapHeld.reservationManager.FirstRespectedReserver(bill_Medical.GiverPawn, pawn, null);
								JobFailReason.Is("IsReservedBy".Translate(bill_Medical.GiverPawn.LabelShort, pawn2.LabelShort), null);
								goto IL_4CE;
							}
						}
						Bill_Mech bill_Mech = bill as Bill_Mech;
						if (bill_Mech != null && bill_Mech.Gestator.WasteProducer.Waste != null && bill_Mech.Gestator.GestatingMech == null)
						{
							JobFailReason.Is("WasteContainerFull".Translate(), null);
						}
						else
						{
							Bill_ProductionWithUft bill_ProductionWithUft = bill as Bill_ProductionWithUft;
							if (bill_ProductionWithUft != null)
							{
								if (bill_ProductionWithUft.BoundUft != null)
								{
									if (bill_ProductionWithUft.BoundWorker == pawn && pawn.CanReserveAndReach(bill_ProductionWithUft.BoundUft, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false) && !bill_ProductionWithUft.BoundUft.IsForbidden(pawn))
									{
										return WorkGiver_DoBill.FinishUftJob(pawn, bill_ProductionWithUft.BoundUft, bill_ProductionWithUft);
									}
									goto IL_4CE;
								}
								else
								{
									UnfinishedThing unfinishedThing = WorkGiver_DoBill.ClosestUnfinishedThingForBill(pawn, bill_ProductionWithUft);
									if (unfinishedThing != null)
									{
										return WorkGiver_DoBill.FinishUftJob(pawn, unfinishedThing, bill_ProductionWithUft);
									}
								}
							}
							Bill_Autonomous bill_Autonomous = bill as Bill_Autonomous;
							if (bill_Autonomous != null && bill_Autonomous.State != FormingState.Gathering)
							{
								return WorkGiver_DoBill.WorkOnFormedBill((Thing)giver, bill_Autonomous);
							}
							List<IngredientCount> list = null;
							if (flag)
							{
								list = WorkGiver_DoBill.missingIngredients;
								list.Clear();
								WorkGiver_DoBill.tmpMissingUniqueIngredients.Clear();
							}
							Bill_Medical bill_Medical2 = bill as Bill_Medical;
							if (bill_Medical2 != null)
							{
								List<Thing> uniqueRequiredIngredients = bill_Medical2.uniqueRequiredIngredients;
								bool? flag2 = (uniqueRequiredIngredients != null) ? new bool?(uniqueRequiredIngredients.NullOrEmpty<Thing>()) : null;
								bool flag3 = false;
								if (flag2.GetValueOrDefault() == flag3 & flag2 != null)
								{
									foreach (Thing thing in bill_Medical2.uniqueRequiredIngredients)
									{
										if (thing.IsForbidden(pawn) || !pawn.CanReserveAndReach(thing, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, false))
										{
											WorkGiver_DoBill.tmpMissingUniqueIngredients.Add(thing);
										}
									}
								}
							}
							if (WorkGiver_DoBill.TryFindBestBillIngredients(bill, pawn, (Thing)giver, this.chosenIngThings, list) && WorkGiver_DoBill.tmpMissingUniqueIngredients.NullOrEmpty<Thing>())
							{
								flag = false;
								if (bill_Medical2 != null)
								{
									List<Thing> uniqueRequiredIngredients2 = bill_Medical2.uniqueRequiredIngredients;
									bool? flag2 = (uniqueRequiredIngredients2 != null) ? new bool?(uniqueRequiredIngredients2.NullOrEmpty<Thing>()) : null;
									bool flag3 = false;
									if (flag2.GetValueOrDefault() == flag3 & flag2 != null)
									{
										foreach (Thing thing2 in bill_Medical2.uniqueRequiredIngredients)
										{
											this.chosenIngThings.Add(new ThingCount(thing2, 1, false));
										}
									}
								}
								Job job;
								Job result = WorkGiver_DoBill.TryStartNewDoBillJob(pawn, bill, giver, this.chosenIngThings, out job, true);
								this.chosenIngThings.Clear();
								return result;
							}
							if (FloatMenuMakerMap.makingFor != pawn)
							{
								bill.nextTickToSearchForIngredients = Find.TickManager.TicksGame + WorkGiver_DoBill.ReCheckFailedBillTicksRange.RandomInRange;
							}
							else if (flag)
							{
								if (WorkGiver_DoBill.CannotDoBillDueToMedicineRestriction(giver, bill, list))
								{
									JobFailReason.Is("NoMedicineMatchingCategory".Translate(WorkGiver_DoBill.GetMedicalCareCategory((Thing)giver).GetLabel().Named("CATEGORY")), bill.Label);
								}
								else
								{
									string value = (from missing in list
									select missing.Summary).Concat(from t in WorkGiver_DoBill.tmpMissingUniqueIngredients
									select t.Label).ToCommaList(false, false);
									JobFailReason.Is("MissingMaterials".Translate(value), bill.Label);
								}
								flag = false;
							}
							this.chosenIngThings.Clear();
						}
					}
				}
				IL_4CE:;
			}
			this.chosenIngThings.Clear();
			return null;
		}

		// Token: 0x06008339 RID: 33593 RVA: 0x00276438 File Offset: 0x00274638
		private static bool CannotDoBillDueToMedicineRestriction(IBillGiver giver, Bill bill, List<IngredientCount> missingIngredients)
		{
			Pawn pawn = giver as Pawn;
			if (pawn == null)
			{
				return false;
			}
			bool flag = false;
			using (List<IngredientCount>.Enumerator enumerator = missingIngredients.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.filter.Allows(ThingDefOf.MedicineIndustrial))
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				MedicalCareCategory medicalCareCategory = WorkGiver_DoBill.GetMedicalCareCategory(pawn);
				foreach (Thing thing in pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Medicine))
				{
					if (WorkGiver_DoBill.IsUsableIngredient(thing, bill) && medicalCareCategory.AllowsMedicine(thing.def))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600833A RID: 33594 RVA: 0x00276518 File Offset: 0x00274718
		public static Job TryStartNewDoBillJob(Pawn pawn, Bill bill, IBillGiver giver, List<ThingCount> chosenIngThings, out Job haulOffJob, bool dontCreateJobIfHaulOffRequired = true)
		{
			haulOffJob = WorkGiverUtility.HaulStuffOffBillGiverJob(pawn, giver, null);
			if (haulOffJob != null && dontCreateJobIfHaulOffRequired)
			{
				return haulOffJob;
			}
			Job job = JobMaker.MakeJob(JobDefOf.DoBill, (Thing)giver);
			job.targetQueueB = new List<LocalTargetInfo>(chosenIngThings.Count);
			job.countQueue = new List<int>(chosenIngThings.Count);
			for (int i = 0; i < chosenIngThings.Count; i++)
			{
				job.targetQueueB.Add(chosenIngThings[i].Thing);
				job.countQueue.Add(chosenIngThings[i].Count);
			}
			if (bill.xenogerm != null)
			{
				job.targetQueueB.Add(bill.xenogerm);
				job.countQueue.Add(1);
			}
			job.haulMode = HaulMode.ToCellNonStorage;
			job.bill = bill;
			return job;
		}

		// Token: 0x0600833B RID: 33595 RVA: 0x002765FA File Offset: 0x002747FA
		private static Job WorkOnFormedBill(Thing giver, Bill_Autonomous bill)
		{
			Job job = JobMaker.MakeJob(JobDefOf.DoBill, giver);
			job.bill = bill;
			return job;
		}

		// Token: 0x0600833C RID: 33596 RVA: 0x00276614 File Offset: 0x00274814
		public bool ThingIsUsableBillGiver(Thing thing)
		{
			Pawn pawn = thing as Pawn;
			Corpse corpse = thing as Corpse;
			Pawn pawn2 = null;
			if (corpse != null)
			{
				pawn2 = corpse.InnerPawn;
			}
			if (this.def.fixedBillGiverDefs != null && this.def.fixedBillGiverDefs.Contains(thing.def))
			{
				return true;
			}
			if (pawn != null)
			{
				if (this.def.billGiversAllHumanlikes && pawn.RaceProps.Humanlike)
				{
					return true;
				}
				if (this.def.billGiversAllMechanoids && pawn.RaceProps.IsMechanoid)
				{
					return true;
				}
				if (this.def.billGiversAllAnimals && pawn.IsAnimal)
				{
					return true;
				}
			}
			if (corpse != null && pawn2 != null)
			{
				if (this.def.billGiversAllHumanlikesCorpses && pawn2.RaceProps.Humanlike)
				{
					return true;
				}
				if (this.def.billGiversAllMechanoidsCorpses && pawn2.RaceProps.IsMechanoid)
				{
					return true;
				}
				if (this.def.billGiversAllAnimalsCorpses && pawn2.IsAnimal)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600833D RID: 33597 RVA: 0x0027670C File Offset: 0x0027490C
		private static bool IsUsableIngredient(Thing t, Bill bill)
		{
			if (!bill.IsFixedOrAllowedIngredient(t))
			{
				return false;
			}
			using (List<IngredientCount>.Enumerator enumerator = bill.recipe.ingredients.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.filter.Allows(t))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600833E RID: 33598 RVA: 0x0027677C File Offset: 0x0027497C
		public static bool TryFindBestFixedIngredients(List<IngredientCount> ingredients, Pawn pawn, Thing ingredientDestination, List<ThingCount> chosen, float searchRadius = 999f)
		{
			return WorkGiver_DoBill.TryFindBestIngredientsHelper(delegate(Thing t)
			{
				using (List<IngredientCount>.Enumerator enumerator = ingredients.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.filter.Allows(t))
						{
							return true;
						}
					}
				}
				return false;
			}, (List<Thing> foundThings) => WorkGiver_DoBill.TryFindBestIngredientsInSet_NoMixHelper(foundThings, ingredients, chosen, WorkGiver_DoBill.GetBillGiverRootCell(ingredientDestination, pawn), false, null, null), ingredients, pawn, ingredientDestination, chosen, searchRadius);
		}

		// Token: 0x0600833F RID: 33599 RVA: 0x002767E4 File Offset: 0x002749E4
		private static bool TryFindBestBillIngredients(Bill bill, Pawn pawn, Thing billGiver, List<ThingCount> chosen, List<IngredientCount> missingIngredients)
		{
			return WorkGiver_DoBill.TryFindBestIngredientsHelper((Thing t) => WorkGiver_DoBill.IsUsableIngredient(t, bill), (List<Thing> foundThings) => WorkGiver_DoBill.TryFindBestBillIngredientsInSet(foundThings, bill, chosen, WorkGiver_DoBill.GetBillGiverRootCell(billGiver, pawn), billGiver is Pawn, missingIngredients), bill.recipe.ingredients, pawn, billGiver, chosen, bill.ingredientSearchRadius);
		}

		// Token: 0x06008340 RID: 33600 RVA: 0x00276868 File Offset: 0x00274A68
		private static bool TryFindBestIngredientsHelper(Predicate<Thing> thingValidator, Predicate<List<Thing>> foundAllIngredientsAndChoose, List<IngredientCount> ingredients, Pawn pawn, Thing billGiver, List<ThingCount> chosen, float searchRadius)
		{
			chosen.Clear();
			WorkGiver_DoBill.newRelevantThings.Clear();
			if (ingredients.Count == 0)
			{
				return true;
			}
			IntVec3 billGiverRootCell = WorkGiver_DoBill.GetBillGiverRootCell(billGiver, pawn);
			Region rootReg = billGiverRootCell.GetRegion(pawn.Map, RegionType.Set_Passable);
			if (rootReg == null)
			{
				return false;
			}
			WorkGiver_DoBill.relevantThings.Clear();
			WorkGiver_DoBill.processedThings.Clear();
			bool foundAll = false;
			float radiusSq = searchRadius * searchRadius;
			Predicate<Thing> baseValidator = (Thing t) => t.Spawned && thingValidator(t) && (float)(t.Position - billGiver.Position).LengthHorizontalSquared < radiusSq && !t.IsForbidden(pawn) && pawn.CanReserve(t, 1, -1, null, false);
			bool billGiverIsPawn = billGiver is Pawn;
			if (billGiverIsPawn)
			{
				WorkGiver_DoBill.AddEveryMedicineToRelevantThings(pawn, billGiver, WorkGiver_DoBill.relevantThings, baseValidator, pawn.Map);
				if (foundAllIngredientsAndChoose(WorkGiver_DoBill.relevantThings))
				{
					WorkGiver_DoBill.relevantThings.Clear();
					return true;
				}
			}
			Building_WorkTableAutonomous building_WorkTableAutonomous = billGiver as Building_WorkTableAutonomous;
			if (building_WorkTableAutonomous != null)
			{
				WorkGiver_DoBill.relevantThings.AddRange(building_WorkTableAutonomous.innerContainer);
				if (foundAllIngredientsAndChoose(WorkGiver_DoBill.relevantThings))
				{
					WorkGiver_DoBill.relevantThings.Clear();
					return true;
				}
			}
			foreach (IHaulSource haulSource in pawn.Map.haulDestinationManager.AllHaulSourcesListForReading)
			{
				if (haulSource.HaulSourceEnabled)
				{
					Thing thing = haulSource as Thing;
					if (thing != null && thing.Spawned && thing.Position.InHorDistOf(billGiver.Position, searchRadius) && !thing.IsForbidden(pawn))
					{
						ThingOwnerUtility.GetAllThingsRecursively(haulSource, WorkGiver_DoBill.newRelevantThings, true, null);
						foreach (Thing thing2 in WorkGiver_DoBill.newRelevantThings)
						{
							if (!WorkGiver_DoBill.processedThings.Contains(thing2) && !thing2.IsForbidden(pawn) && pawn.CanReserve(thing2, 1, -1, null, false) && thingValidator(thing2))
							{
								WorkGiver_DoBill.relevantThings.Add(thing2);
								WorkGiver_DoBill.processedThings.Add(thing2);
							}
						}
					}
				}
			}
			WorkGiver_DoBill.newRelevantThings.Clear();
			TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true);
			RegionEntryPredicate entryCondition = null;
			if (Math.Abs(999f - searchRadius) >= 1f)
			{
				entryCondition = delegate(Region from, Region r)
				{
					if (!r.Allows(traverseParams, false))
					{
						return false;
					}
					CellRect extentsClose = r.extentsClose;
					int num = Math.Abs(billGiver.Position.x - Math.Max(extentsClose.minX, Math.Min(billGiver.Position.x, extentsClose.maxX)));
					if ((float)num > searchRadius)
					{
						return false;
					}
					int num2 = Math.Abs(billGiver.Position.z - Math.Max(extentsClose.minZ, Math.Min(billGiver.Position.z, extentsClose.maxZ)));
					return (float)num2 <= searchRadius && (float)(num * num + num2 * num2) <= radiusSq;
				};
			}
			else
			{
				entryCondition = ((Region from, Region r) => r.Allows(traverseParams, false));
			}
			int adjacentRegionsAvailable = rootReg.Neighbors.Count((Region region) => entryCondition(rootReg, region));
			int regionsProcessed = 0;
			WorkGiver_DoBill.processedThings.AddRange(WorkGiver_DoBill.relevantThings);
			if (foundAllIngredientsAndChoose(WorkGiver_DoBill.relevantThings))
			{
				return true;
			}
			RegionProcessor regionProcessor = delegate(Region r)
			{
				foreach (Thing thing3 in r.ListerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.HaulableEver)))
				{
					if (!WorkGiver_DoBill.processedThings.Contains(thing3) && ReachabilityWithinRegion.ThingFromRegionListerReachable(thing3, r, PathEndMode.ClosestTouch, pawn) && baseValidator(thing3) && !(thing3.def.IsMedicine & billGiverIsPawn))
					{
						WorkGiver_DoBill.newRelevantThings.Add(thing3);
						WorkGiver_DoBill.processedThings.Add(thing3);
					}
				}
				int regionsProcessed;
				regionsProcessed++;
				regionsProcessed = regionsProcessed;
				if (WorkGiver_DoBill.newRelevantThings.Count > 0 && regionsProcessed > adjacentRegionsAvailable)
				{
					WorkGiver_DoBill.relevantThings.AddRange(WorkGiver_DoBill.newRelevantThings);
					WorkGiver_DoBill.newRelevantThings.Clear();
					if (foundAllIngredientsAndChoose(WorkGiver_DoBill.relevantThings))
					{
						foundAll = true;
						return true;
					}
				}
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(rootReg, entryCondition, regionProcessor, 99999, RegionType.Set_Passable);
			WorkGiver_DoBill.relevantThings.Clear();
			WorkGiver_DoBill.newRelevantThings.Clear();
			WorkGiver_DoBill.processedThings.Clear();
			return foundAll;
		}

		// Token: 0x06008341 RID: 33601 RVA: 0x00276C40 File Offset: 0x00274E40
		private static IntVec3 GetBillGiverRootCell(Thing billGiver, Pawn forPawn)
		{
			Building building = billGiver as Building;
			if (building == null)
			{
				return billGiver.Position;
			}
			if (building.def.hasInteractionCell)
			{
				return building.InteractionCell;
			}
			Log.Error("Tried to find bill ingredients for " + ((billGiver != null) ? billGiver.ToString() : null) + " which has no interaction cell.");
			return forPawn.Position;
		}

		// Token: 0x06008342 RID: 33602 RVA: 0x00276C9C File Offset: 0x00274E9C
		private static void AddEveryMedicineToRelevantThings(Pawn pawn, Thing billGiver, List<Thing> relevantThings, Predicate<Thing> baseValidator, Map map)
		{
			MedicalCareCategory medicalCareCategory = WorkGiver_DoBill.GetMedicalCareCategory(billGiver);
			List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.Medicine);
			WorkGiver_DoBill.tmpMedicine.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (medicalCareCategory.AllowsMedicine(thing.def) && baseValidator(thing) && pawn.CanReach(thing, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					WorkGiver_DoBill.tmpMedicine.Add(thing);
				}
			}
			WorkGiver_DoBill.tmpMedicine.SortBy((Thing x) => -x.GetStatValue(StatDefOf.MedicalPotency, true, -1), (Thing x) => x.Position.DistanceToSquared(billGiver.Position));
			relevantThings.AddRange(WorkGiver_DoBill.tmpMedicine);
			WorkGiver_DoBill.tmpMedicine.Clear();
		}

		// Token: 0x06008343 RID: 33603 RVA: 0x00276D78 File Offset: 0x00274F78
		public static MedicalCareCategory GetMedicalCareCategory(Thing billGiver)
		{
			Pawn pawn = billGiver as Pawn;
			if (pawn != null && pawn.playerSettings != null)
			{
				return pawn.playerSettings.medCare;
			}
			return MedicalCareCategory.Best;
		}

		// Token: 0x06008344 RID: 33604 RVA: 0x00276DA4 File Offset: 0x00274FA4
		private static bool TryFindBestBillIngredientsInSet(List<Thing> availableThings, Bill bill, List<ThingCount> chosen, IntVec3 rootCell, bool alreadySorted, List<IngredientCount> missingIngredients)
		{
			if (bill.recipe.allowMixingIngredients)
			{
				return WorkGiver_DoBill.TryFindBestBillIngredientsInSet_AllowMix(availableThings, bill, chosen, rootCell, missingIngredients);
			}
			return WorkGiver_DoBill.TryFindBestBillIngredientsInSet_NoMix(availableThings, bill, chosen, rootCell, alreadySorted, missingIngredients);
		}

		// Token: 0x06008345 RID: 33605 RVA: 0x00276DCC File Offset: 0x00274FCC
		private static bool TryFindBestBillIngredientsInSet_NoMix(List<Thing> availableThings, Bill bill, List<ThingCount> chosen, IntVec3 rootCell, bool alreadySorted, List<IngredientCount> missingIngredients)
		{
			return WorkGiver_DoBill.TryFindBestIngredientsInSet_NoMixHelper(availableThings, bill.recipe.ingredients, chosen, rootCell, alreadySorted, missingIngredients, bill);
		}

		// Token: 0x06008346 RID: 33606 RVA: 0x00276DE8 File Offset: 0x00274FE8
		private static bool TryFindBestIngredientsInSet_NoMixHelper(List<Thing> availableThings, List<IngredientCount> ingredients, List<ThingCount> chosen, IntVec3 rootCell, bool alreadySorted, List<IngredientCount> missingIngredients, Bill bill = null)
		{
			if (!alreadySorted)
			{
				Comparison<Thing> comparison = delegate(Thing t1, Thing t2)
				{
					float num5 = (float)(t1.PositionHeld - rootCell).LengthHorizontalSquared;
					float value = (float)(t2.PositionHeld - rootCell).LengthHorizontalSquared;
					return num5.CompareTo(value);
				};
				availableThings.Sort(comparison);
			}
			chosen.Clear();
			WorkGiver_DoBill.availableCounts.Clear();
			if (missingIngredients != null)
			{
				missingIngredients.Clear();
			}
			WorkGiver_DoBill.availableCounts.GenerateFrom(availableThings);
			for (int i = 0; i < ingredients.Count; i++)
			{
				IngredientCount ingredientCount = ingredients[i];
				bool flag = false;
				for (int j = 0; j < WorkGiver_DoBill.availableCounts.Count; j++)
				{
					float num = (bill != null) ? ((float)ingredientCount.CountRequiredOfFor(WorkGiver_DoBill.availableCounts.GetDef(j), bill.recipe, bill)) : ingredientCount.GetBaseCount();
					if ((bill == null || bill.recipe.ignoreIngredientCountTakeEntireStacks || num <= WorkGiver_DoBill.availableCounts.GetCount(j)) && ingredientCount.filter.Allows(WorkGiver_DoBill.availableCounts.GetDef(j)) && (bill == null || ingredientCount.IsFixedIngredient || bill.ingredientFilter.Allows(WorkGiver_DoBill.availableCounts.GetDef(j))))
					{
						for (int k = 0; k < availableThings.Count; k++)
						{
							if (availableThings[k].def == WorkGiver_DoBill.availableCounts.GetDef(j))
							{
								int num2 = availableThings[k].stackCount - ThingCountUtility.CountOf(chosen, availableThings[k]);
								if (num2 > 0)
								{
									if (bill != null && bill.recipe.ignoreIngredientCountTakeEntireStacks)
									{
										ThingCountUtility.AddToList(chosen, availableThings[k], num2);
										return true;
									}
									int num3 = Mathf.Min(Mathf.FloorToInt(num), num2);
									ThingCountUtility.AddToList(chosen, availableThings[k], num3);
									num -= (float)num3;
									if (num < 0.001f)
									{
										flag = true;
										float num4 = WorkGiver_DoBill.availableCounts.GetCount(j);
										num4 -= num;
										WorkGiver_DoBill.availableCounts.SetCount(j, num4);
										break;
									}
								}
							}
						}
						if (flag)
						{
							break;
						}
					}
				}
				if (!flag)
				{
					if (missingIngredients == null)
					{
						return false;
					}
					missingIngredients.Add(ingredientCount);
				}
			}
			return missingIngredients == null || missingIngredients.Count == 0;
		}

		// Token: 0x06008347 RID: 33607 RVA: 0x00277014 File Offset: 0x00275214
		private static bool TryFindBestBillIngredientsInSet_AllowMix(List<Thing> availableThings, Bill bill, List<ThingCount> chosen, IntVec3 rootCell, List<IngredientCount> missingIngredients)
		{
			chosen.Clear();
			if (missingIngredients != null)
			{
				missingIngredients.Clear();
			}
			availableThings.SortBy((Thing t) => bill.recipe.IngredientValueGetter.ValuePerUnitOf(t.def), (Thing t) => (t.Position - rootCell).LengthHorizontalSquared);
			for (int i = 0; i < bill.recipe.ingredients.Count; i++)
			{
				IngredientCount ingredientCount = bill.recipe.ingredients[i];
				float num = ingredientCount.GetBaseCount();
				for (int j = 0; j < availableThings.Count; j++)
				{
					Thing thing = availableThings[j];
					if (ingredientCount.filter.Allows(thing) && (ingredientCount.IsFixedIngredient || bill.ingredientFilter.Allows(thing)))
					{
						float num2 = bill.recipe.IngredientValueGetter.ValuePerUnitOf(thing.def);
						int num3 = Mathf.Min(Mathf.CeilToInt(num / num2), thing.stackCount);
						ThingCountUtility.AddToList(chosen, thing, num3);
						num -= (float)num3 * num2;
						if (num <= 0.0001f)
						{
							break;
						}
					}
				}
				if (num > 0.0001f)
				{
					if (missingIngredients == null)
					{
						return false;
					}
					missingIngredients.Add(ingredientCount);
				}
			}
			return missingIngredients == null || missingIngredients.Count == 0;
		}

		// Token: 0x040056B6 RID: 22198
		private List<ThingCount> chosenIngThings = new List<ThingCount>();

		// Token: 0x040056B7 RID: 22199
		private static List<IngredientCount> missingIngredients = new List<IngredientCount>();

		// Token: 0x040056B8 RID: 22200
		private static List<Thing> tmpMissingUniqueIngredients = new List<Thing>();

		// Token: 0x040056B9 RID: 22201
		private static readonly IntRange ReCheckFailedBillTicksRange = new IntRange(500, 600);

		// Token: 0x040056BA RID: 22202
		private static List<Thing> relevantThings = new List<Thing>();

		// Token: 0x040056BB RID: 22203
		private static HashSet<Thing> processedThings = new HashSet<Thing>();

		// Token: 0x040056BC RID: 22204
		private static List<Thing> newRelevantThings = new List<Thing>();

		// Token: 0x040056BD RID: 22205
		private static List<Thing> tmpMedicine = new List<Thing>();

		// Token: 0x040056BE RID: 22206
		private static WorkGiver_DoBill.DefCountList availableCounts = new WorkGiver_DoBill.DefCountList();

		// Token: 0x020015AE RID: 5550
		private class DefCountList
		{
			// Token: 0x17001418 RID: 5144
			// (get) Token: 0x0600834A RID: 33610 RVA: 0x002771EB File Offset: 0x002753EB
			public int Count
			{
				get
				{
					return this.defs.Count;
				}
			}

			// Token: 0x17001419 RID: 5145
			public float this[ThingDef def]
			{
				get
				{
					int num = this.defs.IndexOf(def);
					if (num < 0)
					{
						return 0f;
					}
					return this.counts[num];
				}
				set
				{
					int num = this.defs.IndexOf(def);
					if (num < 0)
					{
						this.defs.Add(def);
						this.counts.Add(value);
						num = this.defs.Count - 1;
					}
					else
					{
						this.counts[num] = value;
					}
					this.CheckRemove(num);
				}
			}

			// Token: 0x0600834D RID: 33613 RVA: 0x00277282 File Offset: 0x00275482
			public float GetCount(int index)
			{
				return this.counts[index];
			}

			// Token: 0x0600834E RID: 33614 RVA: 0x00277290 File Offset: 0x00275490
			public void SetCount(int index, float val)
			{
				this.counts[index] = val;
				this.CheckRemove(index);
			}

			// Token: 0x0600834F RID: 33615 RVA: 0x002772A6 File Offset: 0x002754A6
			public ThingDef GetDef(int index)
			{
				return this.defs[index];
			}

			// Token: 0x06008350 RID: 33616 RVA: 0x002772B4 File Offset: 0x002754B4
			private void CheckRemove(int index)
			{
				if (this.counts[index] == 0f)
				{
					this.counts.RemoveAt(index);
					this.defs.RemoveAt(index);
				}
			}

			// Token: 0x06008351 RID: 33617 RVA: 0x002772E1 File Offset: 0x002754E1
			public void Clear()
			{
				this.defs.Clear();
				this.counts.Clear();
			}

			// Token: 0x06008352 RID: 33618 RVA: 0x002772FC File Offset: 0x002754FC
			public void GenerateFrom(List<Thing> things)
			{
				this.Clear();
				for (int i = 0; i < things.Count; i++)
				{
					ThingDef def = things[i].def;
					this[def] += (float)things[i].stackCount;
				}
			}

			// Token: 0x040056BF RID: 22207
			private List<ThingDef> defs = new List<ThingDef>();

			// Token: 0x040056C0 RID: 22208
			private List<float> counts = new List<float>();
		}
	}
}
