using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001120 RID: 4384
	public abstract class JobDriver_InteractAnimal : JobDriver
	{
		// Token: 0x1700107F RID: 4223
		// (get) Token: 0x06006AEC RID: 27372 RVA: 0x0021B747 File Offset: 0x00219947
		protected Pawn Animal
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x17001080 RID: 4224
		// (get) Token: 0x06006AED RID: 27373 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool CanInteractNow
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001081 RID: 4225
		// (get) Token: 0x06006AEE RID: 27374 RVA: 0x0021B75E File Offset: 0x0021995E
		protected virtual bool CanFeedEver
		{
			get
			{
				Pawn animal = this.Animal;
				object obj;
				if (animal == null)
				{
					obj = null;
				}
				else
				{
					Pawn_NeedsTracker needs = animal.needs;
					obj = ((needs != null) ? needs.food : null);
				}
				return obj != null;
			}
		}

		// Token: 0x06006AEF RID: 27375 RVA: 0x0021B781 File Offset: 0x00219981
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.feedNutritionLeft, "feedNutritionLeft", 0f, false);
		}

		// Token: 0x06006AF0 RID: 27376 RVA: 0x0021B79F File Offset: 0x0021999F
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Animal, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06006AF1 RID: 27377 RVA: 0x0021B7C2 File Offset: 0x002199C2
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnDowned(TargetIndex.A);
			this.FailOnNotCasualInterruptible(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return this.TalkToAnimal(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return this.TalkToAnimal(TargetIndex.A);
			if (this.CanFeedEver)
			{
				foreach (Toil toil in this.FeedToils())
				{
					yield return toil;
				}
				IEnumerator<Toil> enumerator = null;
			}
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return this.TalkToAnimal(TargetIndex.A);
			if (this.CanFeedEver)
			{
				foreach (Toil toil2 in this.FeedToils())
				{
					yield return toil2;
				}
				IEnumerator<Toil> enumerator = null;
			}
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false).FailOn(() => !this.CanInteractNow);
			yield return Toils_Interpersonal.SetLastInteractTime(TargetIndex.A);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield break;
			yield break;
		}

		// Token: 0x06006AF2 RID: 27378 RVA: 0x0021B7D2 File Offset: 0x002199D2
		public static float RequiredNutritionPerFeed(Pawn animal)
		{
			if (animal.needs.food == null)
			{
				return 0f;
			}
			return Mathf.Min(animal.needs.food.MaxLevel * 0.15f, 0.3f);
		}

		// Token: 0x06006AF3 RID: 27379 RVA: 0x0021B807 File Offset: 0x00219A07
		private IEnumerable<Toil> FeedToils()
		{
			Toil toil = ToilMaker.MakeToil("FeedToils");
			toil.initAction = delegate()
			{
				this.feedNutritionLeft = JobDriver_InteractAnimal.RequiredNutritionPerFeed(this.Animal);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil;
			Toil gotoAnimal = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			yield return gotoAnimal;
			yield return this.StartFeedAnimal(TargetIndex.A);
			yield return Toils_Ingest.FinalizeIngest(this.Animal, TargetIndex.B);
			yield return Toils_General.PutCarriedThingInInventory();
			yield return Toils_General.ClearTarget(TargetIndex.B);
			yield return Toils_Jump.JumpIf(gotoAnimal, () => this.feedNutritionLeft > 0f);
			yield break;
		}

		// Token: 0x06006AF4 RID: 27380 RVA: 0x0021B818 File Offset: 0x00219A18
		private Toil TalkToAnimal(TargetIndex tameeInd)
		{
			Toil toil = ToilMaker.MakeToil("TalkToAnimal");
			toil.initAction = delegate()
			{
				Pawn actor = toil.GetActor();
				Pawn recipient = (Pawn)((Thing)actor.CurJob.GetTarget(tameeInd));
				actor.interactions.TryInteractWith(recipient, InteractionDefOf.AnimalChat);
			};
			toil.FailOn(() => !this.CanInteractNow);
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 270;
			toil.activeSkill = (() => SkillDefOf.Animals);
			return toil;
		}

		// Token: 0x06006AF5 RID: 27381 RVA: 0x0021B8C4 File Offset: 0x00219AC4
		private Toil StartFeedAnimal(TargetIndex tameeInd)
		{
			Toil toil = ToilMaker.MakeToil("StartFeedAnimal");
			toil.initAction = delegate()
			{
				Pawn actor = toil.GetActor();
				Pawn pawn = (Pawn)((Thing)actor.CurJob.GetTarget(tameeInd));
				PawnUtility.ForceWait(pawn, 270, actor, false, false);
				Thing thing = FoodUtility.BestFoodInInventory(actor, pawn, FoodPreferability.NeverForNutrition, FoodPreferability.RawTasty, 0f, false, false);
				if (thing == null)
				{
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				actor.mindState.lastInventoryRawFoodUseTick = Find.TickManager.TicksGame;
				int num = FoodUtility.StackCountForNutrition(this.feedNutritionLeft, thing.GetStatValue(StatDefOf.Nutrition, true, -1));
				int stackCount = thing.stackCount;
				Thing thing2 = actor.inventory.innerContainer.Take(thing, Mathf.Min(num, stackCount));
				actor.carryTracker.TryStartCarry(thing2);
				actor.CurJob.SetTarget(TargetIndex.B, thing2);
				float num2 = (float)thing2.stackCount * thing2.GetStatValue(StatDefOf.Nutrition, true, -1);
				this.ticksLeftThisToil = Mathf.CeilToInt(270f * (num2 / JobDriver_InteractAnimal.RequiredNutritionPerFeed(pawn)));
				if (num <= stackCount)
				{
					this.feedNutritionLeft = 0f;
					return;
				}
				this.feedNutritionLeft -= num2;
				if (this.feedNutritionLeft < 0.001f)
				{
					this.feedNutritionLeft = 0f;
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.activeSkill = (() => SkillDefOf.Animals);
			return toil;
		}

		// Token: 0x04004B65 RID: 19301
		protected const TargetIndex AnimalInd = TargetIndex.A;

		// Token: 0x04004B66 RID: 19302
		private const TargetIndex FoodHandInd = TargetIndex.B;

		// Token: 0x04004B67 RID: 19303
		private const int FeedDuration = 270;

		// Token: 0x04004B68 RID: 19304
		private const int TalkDuration = 270;

		// Token: 0x04004B69 RID: 19305
		private const float NutritionPercentagePerFeed = 0.15f;

		// Token: 0x04004B6A RID: 19306
		private const float MaxMinNutritionPerFeed = 0.3f;

		// Token: 0x04004B6B RID: 19307
		public const int FeedCount = 2;

		// Token: 0x04004B6C RID: 19308
		public const FoodPreferability MaxFoodPreferability = FoodPreferability.RawTasty;

		// Token: 0x04004B6D RID: 19309
		private float feedNutritionLeft;
	}
}
