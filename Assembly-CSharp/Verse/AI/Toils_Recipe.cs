using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000EB7 RID: 3767
	public static class Toils_Recipe
	{
		// Token: 0x06005ECD RID: 24269 RVA: 0x001E1BA8 File Offset: 0x001DFDA8
		public static Toil MakeUnfinishedThingIfNeeded()
		{
			Toil toil = ToilMaker.MakeToil("MakeUnfinishedThingIfNeeded");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				if (!curJob.RecipeDef.UsesUnfinishedThing)
				{
					return;
				}
				if (curJob.GetTarget(TargetIndex.B).Thing is UnfinishedThing)
				{
					return;
				}
				List<Thing> list = Toils_Recipe.CalculateIngredients(curJob, actor);
				Thing thing = Toils_Recipe.CalculateDominantIngredient(curJob, list);
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing2 = list[i];
					actor.Map.designationManager.RemoveAllDesignationsOn(thing2, false);
					thing2.DeSpawnOrDeselect(DestroyMode.Vanish);
				}
				ThingDef stuff = curJob.RecipeDef.unfinishedThingDef.MadeFromStuff ? thing.def : null;
				UnfinishedThing unfinishedThing = (UnfinishedThing)ThingMaker.MakeThing(curJob.RecipeDef.unfinishedThingDef, stuff);
				unfinishedThing.Creator = actor;
				unfinishedThing.BoundBill = (Bill_ProductionWithUft)curJob.bill;
				unfinishedThing.ingredients = list;
				unfinishedThing.workLeft = curJob.bill.GetWorkAmount(unfinishedThing);
				CompColorable compColorable = unfinishedThing.TryGetComp<CompColorable>();
				if (compColorable != null)
				{
					compColorable.SetColor(thing.DrawColor);
				}
				GenSpawn.Spawn(unfinishedThing, curJob.GetTarget(TargetIndex.A).Cell, actor.Map, WipeMode.Vanish);
				curJob.SetTarget(TargetIndex.B, unfinishedThing);
				actor.Reserve(unfinishedThing, curJob, 1, -1, null, true, false);
			};
			return toil;
		}

		// Token: 0x06005ECE RID: 24270 RVA: 0x001E1BE8 File Offset: 0x001DFDE8
		public static Toil DoRecipeWork()
		{
			Toil toil = ToilMaker.MakeToil("DoRecipeWork");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				JobDriver_DoBill jobDriver_DoBill = (JobDriver_DoBill)actor.jobs.curDriver;
				Thing thing = curJob.GetTarget(TargetIndex.B).Thing;
				UnfinishedThing unfinishedThing = thing as UnfinishedThing;
				BuildingProperties building = curJob.GetTarget(TargetIndex.A).Thing.def.building;
				if (unfinishedThing != null && unfinishedThing.Initialized)
				{
					jobDriver_DoBill.workLeft = unfinishedThing.workLeft;
				}
				else
				{
					jobDriver_DoBill.workLeft = curJob.bill.GetWorkAmount(thing);
					if (unfinishedThing != null)
					{
						if (unfinishedThing.debugCompleted)
						{
							unfinishedThing.workLeft = (jobDriver_DoBill.workLeft = 0f);
						}
						else
						{
							unfinishedThing.workLeft = jobDriver_DoBill.workLeft;
						}
					}
				}
				jobDriver_DoBill.billStartTick = Find.TickManager.TicksGame;
				jobDriver_DoBill.ticksSpentDoingRecipeWork = 0;
				curJob.bill.Notify_BillWorkStarted(actor);
			};
			toil.tickAction = delegate()
			{
				Pawn actor = toil.actor;
				Thing thing = actor.jobs.curJob.GetTarget(TargetIndex.B).Thing;
				if (thing is UnfinishedThing && thing.Destroyed)
				{
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				IBillGiverWithTickAction billGiverWithTickAction = toil.actor.CurJob.GetTarget(TargetIndex.A).Thing as IBillGiverWithTickAction;
				if (billGiverWithTickAction != null)
				{
					billGiverWithTickAction.UsedThisTick();
				}
			};
			toil.tickIntervalAction = delegate(int delta)
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				JobDriver_DoBill jobDriver_DoBill = (JobDriver_DoBill)actor.jobs.curDriver;
				UnfinishedThing unfinishedThing = curJob.GetTarget(TargetIndex.B).Thing as UnfinishedThing;
				if (unfinishedThing != null && unfinishedThing.Destroyed)
				{
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				jobDriver_DoBill.ticksSpentDoingRecipeWork += delta;
				curJob.bill.Notify_PawnDidWork(actor);
				if (curJob.RecipeDef.workSkill != null && curJob.RecipeDef.UsesUnfinishedThing && actor.skills != null)
				{
					actor.skills.Learn(curJob.RecipeDef.workSkill, 0.1f * curJob.RecipeDef.workSkillLearnFactor * (float)delta, false, false);
				}
				float num = (curJob.RecipeDef.workSpeedStat == null) ? 1f : actor.GetStatValue(curJob.RecipeDef.workSpeedStat, true, -1);
				if (curJob.RecipeDef.workTableSpeedStat != null)
				{
					Building_WorkTable building_WorkTable = jobDriver_DoBill.BillGiver as Building_WorkTable;
					if (building_WorkTable != null)
					{
						num *= building_WorkTable.GetStatValue(curJob.RecipeDef.workTableSpeedStat, true, -1);
					}
				}
				if (DebugSettings.fastCrafting)
				{
					num *= 30f;
				}
				jobDriver_DoBill.workLeft -= num * (float)delta;
				if (unfinishedThing != null)
				{
					if (unfinishedThing.debugCompleted)
					{
						unfinishedThing.workLeft = (jobDriver_DoBill.workLeft = 0f);
					}
					else
					{
						unfinishedThing.workLeft = jobDriver_DoBill.workLeft;
					}
				}
				actor.GainComfortFromCellIfPossible(delta, true);
				if (jobDriver_DoBill.workLeft <= 0f)
				{
					curJob.bill.Notify_BillWorkFinished(actor);
					jobDriver_DoBill.ReadyForNextToil();
					return;
				}
				if (curJob.bill.recipe.UsesUnfinishedThing && Find.TickManager.TicksGame - jobDriver_DoBill.billStartTick >= 3000 && actor.IsHashIntervalTick(1000, delta))
				{
					actor.jobs.CheckForJobOverride(0f, true);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.WithEffect(() => toil.actor.CurJob.bill.recipe.effectWorking, TargetIndex.A, null);
			toil.PlaySustainerOrSound(() => toil.actor.CurJob.bill.recipe.soundWorking, 1f);
			toil.WithProgressBar(TargetIndex.A, delegate
			{
				Pawn actor = toil.actor;
				Job curJob = actor.CurJob;
				Thing thing = curJob.GetTarget(TargetIndex.B).Thing;
				float workLeft = ((JobDriver_DoBill)actor.jobs.curDriver).workLeft;
				Bill_Mech bill_Mech = curJob.bill as Bill_Mech;
				float num = (bill_Mech != null && bill_Mech.State == FormingState.Formed) ? 300f : curJob.bill.recipe.WorkAmountTotal(thing);
				return 1f - workLeft / num;
			}, false, -0.5f, false);
			toil.FailOn(delegate()
			{
				RecipeDef recipeDef = toil.actor.CurJob.RecipeDef;
				if (recipeDef != null && recipeDef.interruptIfIngredientIsRotting)
				{
					LocalTargetInfo target = toil.actor.CurJob.GetTarget(TargetIndex.B);
					if (target.HasThing && target.Thing.GetRotStage() > RotStage.Fresh)
					{
						return true;
					}
				}
				return toil.actor.CurJob.bill.suspended;
			});
			toil.activeSkill = (() => toil.actor.CurJob.bill.recipe.workSkill);
			return toil;
		}

		// Token: 0x06005ECF RID: 24271 RVA: 0x001E1CF0 File Offset: 0x001DFEF0
		public static Toil FinishRecipeAndStartStoringProduct(TargetIndex productIndex = TargetIndex.A)
		{
			Toil toil = ToilMaker.MakeToil("FinishRecipeAndStartStoringProduct");
			toil.AddFinishAction(delegate
			{
				Bill_Production bill_Production = toil.actor.jobs.curJob.bill as Bill_Production;
				if (bill_Production != null && bill_Production.repeatMode == BillRepeatModeDefOf.TargetCount)
				{
					toil.actor.Map.resourceCounter.UpdateResourceCounts();
				}
			});
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				JobDriver_DoBill jobDriver_DoBill = (JobDriver_DoBill)actor.jobs.curDriver;
				if (curJob.RecipeDef.workSkill != null && !curJob.RecipeDef.UsesUnfinishedThing && actor.skills != null)
				{
					float xp = (float)jobDriver_DoBill.ticksSpentDoingRecipeWork * 0.1f * curJob.RecipeDef.workSkillLearnFactor;
					actor.skills.GetSkill(curJob.RecipeDef.workSkill).Learn(xp, false, false);
				}
				List<Thing> ingredients = Toils_Recipe.CalculateIngredients(curJob, actor);
				Thing dominantIngredient = Toils_Recipe.CalculateDominantIngredient(curJob, ingredients);
				ThingStyleDef style = null;
				if (ModsConfig.IdeologyActive && curJob.bill.recipe.products != null && curJob.bill.recipe.products.Count == 1)
				{
					ThingStyleDef thingStyleDef;
					if (!curJob.bill.globalStyle)
					{
						thingStyleDef = curJob.bill.style;
					}
					else
					{
						StyleCategoryPair styleCategoryPair = Faction.OfPlayer.ideos.PrimaryIdeo.style.StyleForThingDef(curJob.bill.recipe.ProducedThingDef, null);
						thingStyleDef = ((styleCategoryPair != null) ? styleCategoryPair.styleDef : null);
					}
					style = thingStyleDef;
				}
				Bill_Mech bill_Mech = curJob.bill as Bill_Mech;
				List<Thing> list = (bill_Mech != null) ? GenRecipe.FinalizeGestatedPawns(bill_Mech, actor, style, null).ToList<Thing>() : GenRecipe.MakeRecipeProducts(curJob.RecipeDef, actor, ingredients, dominantIngredient, jobDriver_DoBill.BillGiver, curJob.bill.precept, style, curJob.bill.graphicIndexOverride).ToList<Thing>();
				Toils_Recipe.ConsumeIngredients(ingredients, curJob.RecipeDef, actor.Map);
				curJob.bill.Notify_IterationCompleted(actor, ingredients);
				RecordsUtility.Notify_BillDone(actor, list);
				if (((curJob != null) ? curJob.bill : null) == null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (!GenPlace.TryPlaceThing(list[i], actor.Position, actor.Map, ThingPlaceMode.Near, null, null, null, 1))
						{
							string[] array = new string[5];
							int num = 0;
							Pawn pawn = actor;
							array[num] = ((pawn != null) ? pawn.ToString() : null);
							array[1] = " could not drop recipe product ";
							int num2 = 2;
							Thing thing = list[i];
							array[num2] = ((thing != null) ? thing.ToString() : null);
							array[3] = " near ";
							array[4] = actor.Position.ToString();
							Log.Error(string.Concat(array));
						}
					}
					return;
				}
				Thing thing2 = curJob.GetTarget(TargetIndex.B).Thing;
				if (curJob.bill.recipe.WorkAmountTotal(thing2) >= 10000f && list.Count > 0)
				{
					TaleRecorder.RecordTale(TaleDefOf.CompletedLongCraftingProject, new object[]
					{
						actor,
						list[0].GetInnerIfMinified().def
					});
				}
				if (list.Any<Thing>())
				{
					Find.QuestManager.Notify_ThingsProduced(actor, list);
				}
				if (list.Count == 0)
				{
					actor.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
					return;
				}
				if (curJob.bill.GetStoreMode() == BillStoreModeDefOf.DropOnFloor)
				{
					for (int j = 0; j < list.Count; j++)
					{
						if (!GenPlace.TryPlaceThing(list[j], actor.Position, actor.Map, ThingPlaceMode.Near, null, null, null, 1))
						{
							Log.Error(string.Format("{0} could not drop recipe product {1} near {2}", actor, list[j], actor.Position));
						}
					}
					actor.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
					return;
				}
				if (list.Count > 1)
				{
					for (int k = 1; k < list.Count; k++)
					{
						if (!GenPlace.TryPlaceThing(list[k], actor.Position, actor.Map, ThingPlaceMode.Near, null, null, null, 1))
						{
							Log.Error(string.Format("{0} could not drop recipe product {1} near {2}", actor, list[k], actor.Position));
						}
					}
				}
				IntVec3 invalid = IntVec3.Invalid;
				if (curJob.bill.GetStoreMode() == BillStoreModeDefOf.BestStockpile)
				{
					StoreUtility.TryFindBestBetterStoreCellFor(list[0], actor, actor.Map, StoragePriority.Unstored, actor.Faction, out invalid, true);
				}
				else if (curJob.bill.GetStoreMode() == BillStoreModeDefOf.SpecificStockpile)
				{
					StoreUtility.TryFindBestBetterStoreCellForIn(list[0], actor, actor.Map, StoragePriority.Unstored, actor.Faction, curJob.bill.GetSlotGroup(), out invalid, true);
				}
				else
				{
					Log.ErrorOnce("Unknown store mode", 9158246);
				}
				if (!invalid.IsValid)
				{
					if (!GenPlace.TryPlaceThing(list[0], actor.Position, actor.Map, ThingPlaceMode.Near, null, null, null, 1))
					{
						Log.Error(string.Format("Bill doer could not drop product {0} near {1}", list[0], actor.Position));
					}
					actor.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
					return;
				}
				int num3 = actor.carryTracker.MaxStackSpaceEver(list[0].def);
				if (num3 < list[0].stackCount)
				{
					int count = list[0].stackCount - num3;
					Thing thing3 = list[0].SplitOff(count);
					if (!GenPlace.TryPlaceThing(thing3, actor.Position, actor.Map, ThingPlaceMode.Near, null, null, null, 1))
					{
						Log.Error(string.Format("{0} could not drop recipe extra product that pawn couldn't carry, {1} near {2}", actor, thing3, actor.Position));
					}
				}
				if (num3 == 0)
				{
					actor.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
					return;
				}
				actor.carryTracker.TryStartCarry(list[0]);
				Pawn_JobTracker jobs = actor.jobs;
				Job newJob = HaulAIUtility.HaulToCellStorageJob(actor, list[0], invalid, false);
				JobCondition lastJobEndCondition = JobCondition.Succeeded;
				ThinkNode jobGiver = null;
				bool resumeCurJobAfterwards = false;
				bool cancelBusyStances = true;
				ThinkTreeDef thinkTree = null;
				bool? keepCarryingThingOverride = new bool?(true);
				jobs.StartJob(newJob, lastJobEndCondition, jobGiver, resumeCurJobAfterwards, cancelBusyStances, thinkTree, null, false, false, keepCarryingThingOverride, false, true, false);
			};
			return toil;
		}

		// Token: 0x06005ED0 RID: 24272 RVA: 0x001E1D48 File Offset: 0x001DFF48
		private static List<Thing> CalculateIngredients(Job job, Pawn actor)
		{
			UnfinishedThing unfinishedThing = job.GetTarget(TargetIndex.B).Thing as UnfinishedThing;
			if (unfinishedThing != null)
			{
				List<Thing> ingredients = unfinishedThing.ingredients;
				job.RecipeDef.Worker.ConsumeIngredient(unfinishedThing, job.RecipeDef, actor.Map);
				job.placedThings = null;
				return ingredients;
			}
			List<Thing> list = new List<Thing>();
			if (job.placedThings != null)
			{
				for (int i = 0; i < job.placedThings.Count; i++)
				{
					if (job.placedThings[i].Count <= 0)
					{
						string[] array = new string[6];
						array[0] = "PlacedThing ";
						int num = 1;
						ThingCountClass thingCountClass = job.placedThings[i];
						array[num] = ((thingCountClass != null) ? thingCountClass.ToString() : null);
						array[2] = " with count ";
						array[3] = job.placedThings[i].Count.ToString();
						array[4] = " for job ";
						array[5] = ((job != null) ? job.ToString() : null);
						Log.Error(string.Concat(array));
					}
					else
					{
						Thing thing;
						if (job.placedThings[i].Count < job.placedThings[i].thing.stackCount)
						{
							thing = job.placedThings[i].thing.SplitOff(job.placedThings[i].Count);
						}
						else
						{
							thing = job.placedThings[i].thing;
						}
						Bill_Medical bill_Medical = job.bill as Bill_Medical;
						if (bill_Medical != null && thing.def.IsMedicine)
						{
							if (bill_Medical.consumedMedicine == null)
							{
								bill_Medical.consumedMedicine = new Dictionary<ThingDef, int>();
							}
							int num2;
							bill_Medical.consumedMedicine.TryGetValue(thing.def, out num2);
							bill_Medical.consumedMedicine[thing.def] = num2 + thing.stackCount;
						}
						job.placedThings[i].Count = 0;
						if (list.Contains(thing))
						{
							string str = "Tried to add ingredient from job placed targets twice: ";
							Thing thing2 = thing;
							Log.Error(str + ((thing2 != null) ? thing2.ToString() : null));
						}
						else
						{
							list.Add(thing);
							if (job.RecipeDef.autoStripCorpses)
							{
								IStrippable strippable = thing as IStrippable;
								if (strippable != null && strippable.AnythingToStrip())
								{
									strippable.Strip(true);
								}
							}
						}
					}
				}
			}
			job.placedThings = null;
			return list;
		}

		// Token: 0x06005ED1 RID: 24273 RVA: 0x001E1F94 File Offset: 0x001E0194
		private static Thing CalculateDominantIngredient(Job job, List<Thing> ingredients)
		{
			Thing thing = job.GetTarget(TargetIndex.B).Thing;
			UnfinishedThing uft = thing as UnfinishedThing;
			if (uft != null && uft.def.MadeFromStuff)
			{
				return uft.ingredients.First((Thing ing) => ing.def == uft.Stuff);
			}
			if (ingredients.NullOrEmpty<Thing>())
			{
				return null;
			}
			RecipeDef recipeDef = job.RecipeDef;
			if (recipeDef.productHasIngredientStuff)
			{
				return ingredients[0];
			}
			if (recipeDef.products.Any((ThingDefCountClass x) => x.thingDef.MadeFromStuff) || (recipeDef.unfinishedThingDef != null && recipeDef.unfinishedThingDef.MadeFromStuff))
			{
				return (from x in ingredients
				where x.def.IsStuff
				select x).RandomElementByWeight((Thing x) => (float)x.stackCount);
			}
			return ingredients.RandomElementByWeight((Thing x) => (float)x.stackCount);
		}

		// Token: 0x06005ED2 RID: 24274 RVA: 0x001E20D0 File Offset: 0x001E02D0
		private static void ConsumeIngredients(List<Thing> ingredients, RecipeDef recipe, Map map)
		{
			for (int i = 0; i < ingredients.Count; i++)
			{
				recipe.Worker.ConsumeIngredient(ingredients[i], recipe, map);
			}
		}

		// Token: 0x06005ED3 RID: 24275 RVA: 0x001E2104 File Offset: 0x001E0304
		public static Toil CheckIfRecipeCanFinishNow()
		{
			Toil toil = ToilMaker.MakeToil("CheckIfRecipeCanFinishNow");
			toil.initAction = delegate()
			{
				if (!toil.actor.jobs.curJob.bill.CanFinishNow)
				{
					toil.actor.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
				}
			};
			return toil;
		}

		// Token: 0x04004460 RID: 17504
		private const int LongCraftingProjectThreshold = 10000;
	}
}
