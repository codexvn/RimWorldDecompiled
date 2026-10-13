using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015C3 RID: 5571
	public class WorkGiver_FeedPatient : WorkGiver_Scanner
	{
		// Token: 0x17001428 RID: 5160
		// (get) Token: 0x060083AF RID: 33711 RVA: 0x002735F9 File Offset: 0x002717F9
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x17001429 RID: 5161
		// (get) Token: 0x060083B0 RID: 33712 RVA: 0x00075162 File Offset: 0x00073362
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x060083B1 RID: 33713 RVA: 0x00075162 File Offset: 0x00073362
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060083B2 RID: 33714 RVA: 0x00274EA6 File Offset: 0x002730A6
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedHungryPawns;
		}

		// Token: 0x060083B3 RID: 33715 RVA: 0x002780AC File Offset: 0x002762AC
		private bool TryFindBestFoodSourceFor(Pawn pawn, Pawn patient, out Thing foodSource, out ThingDef foodDef)
		{
			return FoodUtility.TryFindBestFoodSourceFor(pawn, patient, patient.needs.food.CurCategory == HungerCategory.Starving, out foodSource, out foodDef, false, true, true, false, true, false, false, false, false, false, true, FoodPreferability.Undefined);
		}

		// Token: 0x060083B4 RID: 33716 RVA: 0x002780E4 File Offset: 0x002762E4
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || pawn2 == pawn)
			{
				return false;
			}
			if (this.def.feedHumanlikesOnly && !pawn2.RaceProps.Humanlike)
			{
				return false;
			}
			if (this.def.feedAnimalsOnly && !pawn2.IsAnimal)
			{
				return false;
			}
			if (pawn2.DevelopmentalStage.Baby())
			{
				return false;
			}
			if (!FeedPatientUtility.IsHungry(pawn2))
			{
				return false;
			}
			if (!FeedPatientUtility.ShouldBeFed(pawn2))
			{
				return false;
			}
			if (WardenFeedUtility.ShouldBeFed(pawn2) && !pawn.IsColonyMech)
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (pawn2.foodRestriction != null)
			{
				FoodPolicy currentRespectedRestriction = pawn2.foodRestriction.GetCurrentRespectedRestriction(pawn);
				if (currentRespectedRestriction != null && currentRespectedRestriction.filter.AllowedDefCount == 0)
				{
					JobFailReason.Is("NoFoodMatchingRestrictions".Translate(), null);
					return false;
				}
			}
			Thing thing;
			ThingDef thingDef;
			if (!this.TryFindBestFoodSourceFor(pawn, pawn2, out thing, out thingDef))
			{
				JobFailReason.Is("NoFood".Translate(), null);
				return false;
			}
			return true;
		}

		// Token: 0x060083B5 RID: 33717 RVA: 0x002781E0 File Offset: 0x002763E0
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = (Pawn)t;
			Thing thing;
			ThingDef thingDef;
			if (this.TryFindBestFoodSourceFor(pawn, pawn2, out thing, out thingDef))
			{
				float nutrition = FoodUtility.GetNutrition(pawn2, thing, thingDef);
				Job job = JobMaker.MakeJob(JobDefOf.FeedPatient);
				job.targetA = thing;
				job.targetB = pawn2;
				job.count = FoodUtility.WillIngestStackCountOf(pawn2, thingDef, nutrition);
				return job;
			}
			return null;
		}

		// Token: 0x060083B6 RID: 33718 RVA: 0x00278240 File Offset: 0x00276440
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
