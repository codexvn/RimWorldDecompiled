using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200158E RID: 5518
	public class WorkGiver_Warden_Feed : WorkGiver_Warden
	{
		// Token: 0x060082B9 RID: 33465 RVA: 0x002743A8 File Offset: 0x002725A8
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfPrisoner(pawn, t, false))
			{
				return null;
			}
			Pawn pawn2 = (Pawn)t;
			if (!WardenFeedUtility.ShouldBeFed(pawn2))
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
			if (pawn2.foodRestriction != null)
			{
				FoodPolicy currentRespectedRestriction = pawn2.foodRestriction.GetCurrentRespectedRestriction(pawn);
				if (currentRespectedRestriction != null && currentRespectedRestriction.filter.AllowedDefCount == 0)
				{
					JobFailReason.Is("NoFoodMatchingRestrictions".Translate(), null);
					return null;
				}
			}
			Thing thing;
			ThingDef thingDef;
			if (!FoodUtility.TryFindBestFoodSourceFor(pawn, pawn2, pawn2.needs.food.CurCategory == HungerCategory.Starving, out thing, out thingDef, false, true, false, false, false, false, false, false, false, false, false, FoodPreferability.Undefined))
			{
				JobFailReason.Is("NoFood".Translate(), null);
				return null;
			}
			float nutrition = FoodUtility.GetNutrition(pawn2, thing, thingDef);
			Job job = JobMaker.MakeJob(JobDefOf.FeedPatient, thing, pawn2);
			job.count = FoodUtility.WillIngestStackCountOf(pawn2, thingDef, nutrition);
			return job;
		}

		// Token: 0x060082BA RID: 33466 RVA: 0x002744BC File Offset: 0x002726BC
		public override string JobInfo(Pawn pawn, Job job)
		{
			if (FoodUtility.MoodFromIngesting((Pawn)((Thing)job.targetB), job.targetA.Thing, FoodUtility.GetFinalIngestibleDef(job.targetA.Thing, false)) < 0f)
			{
				return string.Format("({0})", "WarningFoodDisliked".Translate());
			}
			return string.Empty;
		}
	}
}
