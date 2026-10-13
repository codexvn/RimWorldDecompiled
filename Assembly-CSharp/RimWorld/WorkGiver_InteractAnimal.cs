using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200154B RID: 5451
	public abstract class WorkGiver_InteractAnimal : WorkGiver_Scanner
	{
		// Token: 0x170013C4 RID: 5060
		// (get) Token: 0x0600816A RID: 33130 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x0600816B RID: 33131 RVA: 0x0026F8F8 File Offset: 0x0026DAF8
		public static void ResetStaticData()
		{
			WorkGiver_InteractAnimal.NoUsableFoodTrans = "NoUsableFood".Translate();
			WorkGiver_InteractAnimal.AnimalInteractedTooRecentlyTrans = "AnimalInteractedTooRecently".Translate();
			WorkGiver_InteractAnimal.CantInteractAnimalDownedTrans = "CantInteractAnimalDowned".Translate();
			WorkGiver_InteractAnimal.CantInteractAnimalAsleepTrans = "CantInteractAnimalAsleep".Translate();
			WorkGiver_InteractAnimal.CantInteractAnimalBusyTrans = "CantInteractAnimalBusy".Translate();
		}

		// Token: 0x0600816C RID: 33132 RVA: 0x0026F96C File Offset: 0x0026DB6C
		protected virtual bool CanInteractWithAnimal(Pawn pawn, Pawn animal, bool forced)
		{
			string text;
			if (WorkGiver_InteractAnimal.CanInteractWithAnimal(pawn, animal, out text, forced, this.canInteractWhileSleeping, false, false))
			{
				return true;
			}
			if (text != null)
			{
				JobFailReason.Is(text, null);
			}
			return false;
		}

		// Token: 0x0600816D RID: 33133 RVA: 0x0026F99C File Offset: 0x0026DB9C
		public static bool CanInteractWithAnimal(Pawn pawn, Pawn animal, out string jobFailReason, bool forced, bool canInteractWhileSleeping = false, bool ignoreSkillRequirements = false, bool canInteractWhileRoaming = false)
		{
			jobFailReason = null;
			if (!pawn.CanReserve(animal, 1, -1, null, forced))
			{
				return false;
			}
			if (animal.Downed)
			{
				jobFailReason = WorkGiver_InteractAnimal.CantInteractAnimalDownedTrans;
				return false;
			}
			if (!animal.Awake() && !canInteractWhileSleeping)
			{
				jobFailReason = WorkGiver_InteractAnimal.CantInteractAnimalAsleepTrans;
				return false;
			}
			if (!animal.CanCasuallyInteractNow(false, canInteractWhileSleeping, canInteractWhileRoaming, false))
			{
				jobFailReason = WorkGiver_InteractAnimal.CantInteractAnimalBusyTrans;
				return false;
			}
			int num = TrainableUtility.MinimumHandlingSkill(animal);
			if (!ignoreSkillRequirements && num > pawn.skills.GetSkill(SkillDefOf.Animals).Level)
			{
				jobFailReason = "AnimalsSkillTooLow".Translate(num);
				return false;
			}
			return true;
		}

		// Token: 0x0600816E RID: 33134 RVA: 0x0026FA3C File Offset: 0x0026DC3C
		protected bool HasFoodToInteractAnimal(Pawn pawn, Pawn tamee)
		{
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			int num = 0;
			float num2 = JobDriver_InteractAnimal.RequiredNutritionPerFeed(tamee);
			float num3 = 0f;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				Thing thing = innerContainer[i];
				if (tamee.WillEat(thing, pawn, true, false) && thing.def.ingestible.preferability <= FoodPreferability.RawTasty && !thing.def.IsDrug)
				{
					for (int j = 0; j < thing.stackCount; j++)
					{
						num3 += thing.GetStatValue(StatDefOf.Nutrition, true, -1);
						if (num3 >= num2)
						{
							num++;
							num3 = 0f;
						}
						if (num >= 2)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600816F RID: 33135 RVA: 0x0026FAF4 File Offset: 0x0026DCF4
		protected Job TakeFoodForAnimalInteractJob(Pawn pawn, Pawn tamee)
		{
			ThingDef foodDef;
			Thing thing = FoodUtility.BestFoodSourceOnMap(pawn, tamee, false, out foodDef, FoodPreferability.RawTasty, false, false, false, false, false, false, false, false, false, false, false, FoodPreferability.Undefined, new float?(JobDriver_InteractAnimal.RequiredNutritionPerFeed(tamee) * 2f * 4f), false);
			if (thing == null)
			{
				return null;
			}
			float wantedNutrition = JobDriver_InteractAnimal.RequiredNutritionPerFeed(tamee) * 2f * 4f;
			float nutrition = FoodUtility.GetNutrition(tamee, thing, foodDef);
			int count = FoodUtility.StackCountForNutrition(wantedNutrition, nutrition);
			Job job = JobMaker.MakeJob(JobDefOf.TakeInventory, thing);
			job.count = count;
			return job;
		}

		// Token: 0x0400563A RID: 22074
		protected static string NoUsableFoodTrans;

		// Token: 0x0400563B RID: 22075
		protected static string AnimalInteractedTooRecentlyTrans;

		// Token: 0x0400563C RID: 22076
		private static string CantInteractAnimalDownedTrans;

		// Token: 0x0400563D RID: 22077
		private static string CantInteractAnimalAsleepTrans;

		// Token: 0x0400563E RID: 22078
		private static string CantInteractAnimalBusyTrans;

		// Token: 0x0400563F RID: 22079
		protected bool canInteractWhileSleeping;
	}
}
