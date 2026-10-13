using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001555 RID: 5461
	public class WorkGiver_Tame : WorkGiver_InteractAnimal
	{
		// Token: 0x0600819D RID: 33181 RVA: 0x0027055F File Offset: 0x0026E75F
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Tame))
			{
				yield return designation.target.Thing;
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600819E RID: 33182 RVA: 0x0027056F File Offset: 0x0026E76F
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Tame);
		}

		// Token: 0x0600819F RID: 33183 RVA: 0x0027058C File Offset: 0x0026E78C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || !TameUtility.CanTame(pawn2))
			{
				return null;
			}
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Tame) == null)
			{
				return null;
			}
			if (!this.CanInteractWithAnimal(pawn, pawn2, forced))
			{
				return null;
			}
			if (TameUtility.TriedToTameTooRecently(pawn2))
			{
				JobFailReason.Is(WorkGiver_InteractAnimal.AnimalInteractedTooRecentlyTrans, null);
				return null;
			}
			Thing thing = null;
			int count = -1;
			if (pawn2.RaceProps.EatsFood)
			{
				Pawn_NeedsTracker needs = pawn2.needs;
				if (((needs != null) ? needs.food : null) != null && !base.HasFoodToInteractAnimal(pawn, pawn2))
				{
					ThingDef foodDef;
					thing = FoodUtility.BestFoodSourceOnMap(pawn, pawn2, false, out foodDef, FoodPreferability.RawTasty, false, false, false, false, false, false, false, false, false, false, false, FoodPreferability.Undefined, new float?(JobDriver_InteractAnimal.RequiredNutritionPerFeed(pawn2) * 2f * 4f), false);
					if (thing == null)
					{
						JobFailReason.Is("NoFood".Translate(), null);
						return null;
					}
					float num = JobDriver_InteractAnimal.RequiredNutritionPerFeed(pawn2) * 2f * 4f;
					float nutrition = FoodUtility.GetNutrition(pawn2, thing, foodDef);
					count = Mathf.CeilToInt(num / nutrition);
				}
			}
			Job job = JobMaker.MakeJob(JobDefOf.Tame, t, null, thing);
			job.count = count;
			return job;
		}
	}
}
