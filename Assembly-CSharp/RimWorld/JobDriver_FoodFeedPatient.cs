using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200138E RID: 5006
	public class JobDriver_FoodFeedPatient : JobDriver
	{
		// Token: 0x17001353 RID: 4947
		// (get) Token: 0x06007A9B RID: 31387 RVA: 0x001E5D2F File Offset: 0x001E3F2F
		protected Thing Food
		{
			get
			{
				return this.job.targetA.Thing;
			}
		}

		// Token: 0x17001354 RID: 4948
		// (get) Token: 0x06007A9C RID: 31388 RVA: 0x0024EF63 File Offset: 0x0024D163
		protected Pawn Deliveree
		{
			get
			{
				return this.job.targetB.Pawn;
			}
		}

		// Token: 0x17001355 RID: 4949
		// (get) Token: 0x06007A9D RID: 31389 RVA: 0x0024EF75 File Offset: 0x0024D175
		protected Pawn_InventoryTracker FoodHolderInventory
		{
			get
			{
				Thing food = this.Food;
				return ((food != null) ? food.ParentHolder : null) as Pawn_InventoryTracker;
			}
		}

		// Token: 0x17001356 RID: 4950
		// (get) Token: 0x06007A9E RID: 31390 RVA: 0x00229C84 File Offset: 0x00227E84
		protected Pawn FoodHolder
		{
			get
			{
				return this.job.targetC.Pawn;
			}
		}

		// Token: 0x06007A9F RID: 31391 RVA: 0x0024EF90 File Offset: 0x0024D190
		public override string GetReport()
		{
			if (this.job.GetTarget(TargetIndex.A).Thing is Building_NutrientPasteDispenser && this.Deliveree != null)
			{
				return JobUtility.GetResolvedJobReportRaw(this.job.def.reportString, ThingDefOf.MealNutrientPaste.label, ThingDefOf.MealNutrientPaste, this.Deliveree.LabelShort, this.Deliveree, "", "");
			}
			return base.GetReport();
		}

		// Token: 0x06007AA0 RID: 31392 RVA: 0x0024F008 File Offset: 0x0024D208
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (!this.pawn.Reserve(this.Deliveree, this.job, 1, -1, null, errorOnFailed, false))
			{
				return false;
			}
			if (!(base.TargetThingA is Building_NutrientPasteDispenser) && (this.pawn.inventory == null || !this.pawn.inventory.Contains(base.TargetThingA)))
			{
				int maxAmountToPickup = FoodUtility.GetMaxAmountToPickup(this.Food, this.pawn, this.job.count);
				if (!this.pawn.Reserve(this.Food, this.job, 10, maxAmountToPickup, null, errorOnFailed, false))
				{
					return false;
				}
				this.job.count = maxAmountToPickup;
			}
			return true;
		}

		// Token: 0x06007AA1 RID: 31393 RVA: 0x0024F0BC File Offset: 0x0024D2BC
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.B);
			this.FailOn(() => !FoodUtility.ShouldBeFedBySomeone(this.Deliveree));
			Toil carryFoodFromInventory = Toils_Misc.TakeItemFromInventoryToCarrier(this.pawn, TargetIndex.A);
			Toil goToNutrientDispenser = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell, false).FailOnForbidden(TargetIndex.A);
			Toil goToFoodHolder = Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.Touch, false).FailOn(delegate()
			{
				Pawn foodHolder = this.FoodHolder;
				Pawn_InventoryTracker foodHolderInventory2 = this.FoodHolderInventory;
				return foodHolder != ((foodHolderInventory2 != null) ? foodHolderInventory2.pawn : null) || this.FoodHolder.IsForbidden(this.pawn);
			});
			Toil carryFoodToPatient = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch, false);
			yield return Toils_Jump.JumpIf(carryFoodFromInventory, () => this.pawn.inventory != null && this.pawn.inventory.Contains(base.TargetThingA));
			yield return Toils_Haul.CheckItemCarriedByOtherPawn(this.Food, TargetIndex.C, goToFoodHolder);
			yield return Toils_Jump.JumpIf(goToNutrientDispenser, () => base.TargetThingA is Building_NutrientPasteDispenser);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, false).FailOnForbidden(TargetIndex.A);
			yield return Toils_Ingest.PickupIngestible(TargetIndex.A, this.Deliveree);
			yield return Toils_Jump.Jump(carryFoodToPatient);
			yield return goToFoodHolder;
			yield return Toils_General.Wait(25, TargetIndex.None).WithProgressBarToilDelay(TargetIndex.C, false, -0.5f);
			Thing food = this.Food;
			ThingOwner innerContainer = this.pawn.inventory.innerContainer;
			Pawn_InventoryTracker foodHolderInventory = this.FoodHolderInventory;
			yield return Toils_Haul.TakeFromOtherInventory(food, innerContainer, (foodHolderInventory != null) ? foodHolderInventory.innerContainer : null, this.job.count, TargetIndex.A);
			yield return carryFoodFromInventory;
			yield return Toils_Jump.Jump(carryFoodToPatient);
			yield return goToNutrientDispenser;
			yield return Toils_Ingest.TakeMealFromDispenser(TargetIndex.A, this.pawn);
			yield return carryFoodToPatient;
			yield return Toils_Ingest.ChewIngestible(this.Deliveree, 1.5f, TargetIndex.A, TargetIndex.None).FailOnCannotTouch(TargetIndex.B, PathEndMode.Touch);
			Toil toil = Toils_Ingest.FinalizeIngest(this.Deliveree, TargetIndex.A);
			toil.finishActions = new List<Action>
			{
				delegate()
				{
					if (!ModsConfig.AnomalyActive)
					{
						return;
					}
					if (Rand.Chance(0.3f) && MetalhorrorUtility.IsInfected(this.pawn))
					{
						MetalhorrorUtility.Infect(this.Deliveree, this.pawn, "FeedingImplant", null);
					}
				}
			};
			yield return toil;
			yield break;
		}

		// Token: 0x040052F8 RID: 21240
		private const TargetIndex FoodSourceInd = TargetIndex.A;

		// Token: 0x040052F9 RID: 21241
		private const TargetIndex DelivereeInd = TargetIndex.B;

		// Token: 0x040052FA RID: 21242
		private const TargetIndex FoodHolderInd = TargetIndex.C;

		// Token: 0x040052FB RID: 21243
		private const float FeedDurationMultiplier = 1.5f;

		// Token: 0x040052FC RID: 21244
		private const float MetalhorrorInfectionChance = 0.3f;
	}
}
