using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200114C RID: 4428
	public class JobDriver_BottleFeedBaby : JobDriver_FeedBaby
	{
		// Token: 0x170010AC RID: 4268
		// (get) Token: 0x06006C3A RID: 27706 RVA: 0x001D95FF File Offset: 0x001D77FF
		protected Thing BabyFood
		{
			get
			{
				return base.TargetThingB;
			}
		}

		// Token: 0x170010AD RID: 4269
		// (get) Token: 0x06006C3B RID: 27707 RVA: 0x00220897 File Offset: 0x0021EA97
		protected LocalTargetInfo BabyFoodTarget
		{
			get
			{
				return base.TargetB;
			}
		}

		// Token: 0x170010AE RID: 4270
		// (get) Token: 0x06006C3C RID: 27708 RVA: 0x0022089F File Offset: 0x0021EA9F
		// (set) Token: 0x06006C3D RID: 27709 RVA: 0x002208A7 File Offset: 0x0021EAA7
		protected override Toil FeedingToil { get; set; }

		// Token: 0x06006C3E RID: 27710 RVA: 0x002208B0 File Offset: 0x0021EAB0
		protected override IEnumerable<Toil> MakeNewToils()
		{
			base.AddFailCondition(() => !this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation));
			Toil failIfNoBabyFoodInInventory = this.FailIfNoBabyFoodInInventory();
			yield return Toils_Jump.JumpIf(failIfNoBabyFoodInInventory, () => !this.BabyFoodTarget.IsValid || this.pawn.inventory.Contains(this.BabyFood));
			yield return Toils_Ingest.ReserveFoodFromStackForIngesting(TargetIndex.B, base.Baby).FailOnDestroyedNullOrForbidden(TargetIndex.B);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch, false).FailOnDestroyedNullOrForbidden(TargetIndex.B);
			yield return Toils_Haul.TakeToInventory(TargetIndex.B, delegate(Thing babyFood)
			{
				int b = FoodUtility.WillIngestStackCountOf(base.Baby, babyFood.def, FoodUtility.NutritionForEater(base.Baby, babyFood));
				return Mathf.Min(babyFood.stackCount, b);
			}).FailOnDestroyedNullOrForbidden(TargetIndex.B);
			yield return failIfNoBabyFoodInInventory;
			foreach (Toil toil in base.MakeNewToils())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06006C3F RID: 27711 RVA: 0x002208C0 File Offset: 0x0021EAC0
		private Toil FailIfNoBabyFoodInInventory()
		{
			Toil toil = ToilMaker.MakeToil("FailIfNoBabyFoodInInventory");
			toil.FailOn(() => FoodUtility.BestFoodInInventory(this.pawn, base.Baby, FoodPreferability.NeverForNutrition, FoodPreferability.MealLavish, 0f, false, false) == null);
			return toil;
		}

		// Token: 0x06006C40 RID: 27712 RVA: 0x002208DF File Offset: 0x0021EADF
		protected override IEnumerable<Toil> FeedBaby()
		{
			yield return this.FeedBabyFoodFromInventory();
			yield break;
		}

		// Token: 0x06006C41 RID: 27713 RVA: 0x002208F0 File Offset: 0x0021EAF0
		private Toil FeedBabyFoodFromInventory()
		{
			this.FeedingToil = ToilMaker.MakeToil("FeedBabyFoodFromInventory");
			this.FeedingToil.initAction = delegate()
			{
				this.initialNutritionNeeded = base.Baby.needs.food.NutritionWanted;
				this.initialFoodPercentage = base.Baby.needs.food.CurLevelPercentage;
				base.Baby.jobs.StartJob(ChildcareUtility.MakeBabySuckleJob(this.pawn), JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
			};
			this.FeedingToil.tickIntervalAction = delegate(int delta)
			{
				float num = base.Baby.needs.food.MaxLevel / 5000f;
				float num2 = Mathf.Min(base.Baby.needs.food.NutritionWanted, num * (float)delta);
				this.bottleNutrition += num2;
				this.totalBottleNutrition += num2;
				this.pawn.GainComfortFromCellIfPossible(delta, false);
				Pawn_IdeoTracker ideo = base.Baby.ideo;
				if (ideo != null)
				{
					ideo.IncreaseIdeoExposureIfBabyTick(this.pawn.Ideo, 1);
				}
				if (!this.pawn.Downed && this.pawn.Rotation == Rot4.North)
				{
					this.pawn.Rotation = Rot4.East;
				}
				for (;;)
				{
					Thing thing = FoodUtility.BestFoodInInventory(this.pawn, base.Baby, FoodPreferability.NeverForNutrition, FoodPreferability.MealLavish, 0f, false, false);
					if (thing == null)
					{
						break;
					}
					if (this.bottleNutrition >= base.Baby.needs.food.NutritionWanted)
					{
						float num3 = thing.Ingested(base.Baby, this.bottleNutrition);
						base.Baby.records.AddTo(RecordDefOf.NutritionEaten, num3);
						this.bottleNutrition -= num3;
						base.Baby.needs.food.CurLevel = Mathf.Clamp(base.Baby.needs.food.CurLevel + num3, 0f, base.Baby.needs.food.MaxLevel);
						if (base.Baby.needs.food.CurLevel >= base.Baby.needs.food.MaxLevel)
						{
							goto Block_6;
						}
					}
					else
					{
						float num4 = FoodUtility.NutritionForEater(base.Baby, thing);
						if (this.bottleNutrition < num4)
						{
							return;
						}
						float num5 = thing.Ingested(base.Baby, num4);
						base.Baby.records.AddTo(RecordDefOf.NutritionEaten, num5);
						this.bottleNutrition -= num5;
						base.Baby.needs.food.CurLevel = Mathf.Clamp(base.Baby.needs.food.CurLevel + num5, 0f, base.Baby.needs.food.MaxLevel);
					}
				}
				base.ReadyForNextToil();
				return;
				Block_6:
				base.ReadyForNextToil();
			};
			this.FeedingToil.AddFinishAction(delegate
			{
				if (base.Baby.needs != null && base.Baby.needs.food.CurLevelPercentage - this.initialFoodPercentage > 0.6f)
				{
					base.Baby.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FedMe, this.pawn, null);
					this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FedBaby, base.Baby, null);
				}
				if (base.Baby.CurJobDef == JobDefOf.BabySuckle)
				{
					base.Baby.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
				}
			});
			this.FeedingToil.handlingFacing = true;
			this.FeedingToil.WithProgressBar(TargetIndex.A, () => this.totalBottleNutrition / this.initialNutritionNeeded, false, -0.5f, false);
			this.FeedingToil.defaultCompleteMode = ToilCompleteMode.Never;
			this.FeedingToil.WithEffect(EffecterDefOf.Breastfeeding, TargetIndex.A, null);
			return this.FeedingToil;
		}

		// Token: 0x06006C42 RID: 27714 RVA: 0x002209AC File Offset: 0x0021EBAC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.bottleNutrition, "bottleNutrition", 0f, false);
			Scribe_Values.Look<float>(ref this.totalBottleNutrition, "totalBottleNutrition", 0f, false);
			Scribe_Values.Look<float>(ref this.initialNutritionNeeded, "initialNutritionNeeded", 0f, false);
		}

		// Token: 0x04004C1E RID: 19486
		protected const TargetIndex BabyFoodInd = TargetIndex.B;

		// Token: 0x04004C20 RID: 19488
		private float bottleNutrition;

		// Token: 0x04004C21 RID: 19489
		private float totalBottleNutrition;

		// Token: 0x04004C22 RID: 19490
		private float initialNutritionNeeded;
	}
}
