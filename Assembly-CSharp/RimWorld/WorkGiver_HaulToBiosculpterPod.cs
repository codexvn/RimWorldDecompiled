using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015DE RID: 5598
	public class WorkGiver_HaulToBiosculpterPod : WorkGiver_Scanner
	{
		// Token: 0x1700143D RID: 5181
		// (get) Token: 0x0600842A RID: 33834 RVA: 0x0027A17A File Offset: 0x0027837A
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.BiosculpterPod);
			}
		}

		// Token: 0x1700143E RID: 5182
		// (get) Token: 0x0600842B RID: 33835 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x0600842C RID: 33836 RVA: 0x0027A188 File Offset: 0x00278388
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModLister.CheckIdeology("Biosculpting"))
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
			{
				return false;
			}
			CompBiosculpterPod compBiosculpterPod = t.TryGetComp<CompBiosculpterPod>();
			if (compBiosculpterPod == null || !compBiosculpterPod.PowerOn || compBiosculpterPod.State != BiosculpterPodState.LoadingNutrition || (!forced && !compBiosculpterPod.autoLoadNutrition))
			{
				return false;
			}
			if (t.IsBurning())
			{
				return false;
			}
			if (compBiosculpterPod.RequiredNutritionRemaining <= 0f)
			{
				return false;
			}
			if (this.FindNutrition(pawn, compBiosculpterPod).Thing == null)
			{
				JobFailReason.Is("NoFood".Translate(), null);
				return false;
			}
			return true;
		}

		// Token: 0x0600842D RID: 33837 RVA: 0x0027A23C File Offset: 0x0027843C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompBiosculpterPod compBiosculpterPod = t.TryGetComp<CompBiosculpterPod>();
			if (compBiosculpterPod == null)
			{
				return null;
			}
			if (compBiosculpterPod.RequiredNutritionRemaining > 0f)
			{
				ThingCount thingCount = this.FindNutrition(pawn, compBiosculpterPod);
				if (thingCount.Thing != null)
				{
					Job job = HaulAIUtility.HaulToContainerJob(pawn, thingCount.Thing, t);
					job.count = Mathf.Min(job.count, thingCount.Count);
					return job;
				}
			}
			return null;
		}

		// Token: 0x0600842E RID: 33838 RVA: 0x0027A29C File Offset: 0x0027849C
		private ThingCount FindNutrition(Pawn pawn, CompBiosculpterPod pod)
		{
			WorkGiver_HaulToBiosculpterPod.<>c__DisplayClass6_0 CS$<>8__locals1 = new WorkGiver_HaulToBiosculpterPod.<>c__DisplayClass6_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.pod = pod;
			Thing thing = GenClosest.ClosestThingReachable(CS$<>8__locals1.pawn.Position, CS$<>8__locals1.pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.FoodSourceNotPlantOrTree), PathEndMode.ClosestTouch, TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, new Predicate<Thing>(CS$<>8__locals1.<FindNutrition>g__Validator|0), null, 0, -1, false, RegionType.Set_Passable, false, false);
			if (thing == null)
			{
				return default(ThingCount);
			}
			int b = Mathf.CeilToInt(CS$<>8__locals1.pod.RequiredNutritionRemaining / thing.GetStatValue(StatDefOf.Nutrition, true, -1));
			return new ThingCount(thing, Mathf.Min(thing.stackCount, b), false);
		}
	}
}
