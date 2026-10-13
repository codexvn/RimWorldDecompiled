using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015C6 RID: 5574
	public class WorkGiver_FillFermentingBarrel : WorkGiver_Scanner
	{
		// Token: 0x1700142C RID: 5164
		// (get) Token: 0x060083C1 RID: 33729 RVA: 0x0027857E File Offset: 0x0027677E
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.FermentingBarrel);
			}
		}

		// Token: 0x1700142D RID: 5165
		// (get) Token: 0x060083C2 RID: 33730 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060083C3 RID: 33731 RVA: 0x0027858A File Offset: 0x0027678A
		public static void ResetStaticData()
		{
			WorkGiver_FillFermentingBarrel.TemperatureTrans = "BadTemperature".Translate();
			WorkGiver_FillFermentingBarrel.NoWortTrans = "NoWort".Translate();
		}

		// Token: 0x060083C4 RID: 33732 RVA: 0x002785B4 File Offset: 0x002767B4
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_FermentingBarrel building_FermentingBarrel = t as Building_FermentingBarrel;
			if (building_FermentingBarrel == null || building_FermentingBarrel.Fermented || building_FermentingBarrel.SpaceLeftForWort <= 0)
			{
				return false;
			}
			float ambientTemperature = building_FermentingBarrel.AmbientTemperature;
			CompProperties_TemperatureRuinable compProperties = building_FermentingBarrel.def.GetCompProperties<CompProperties_TemperatureRuinable>();
			if (ambientTemperature < compProperties.minSafeTemperature + 2f || ambientTemperature > compProperties.maxSafeTemperature - 2f)
			{
				JobFailReason.Is(WorkGiver_FillFermentingBarrel.TemperatureTrans, null);
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
			if (this.FindWort(pawn, building_FermentingBarrel) == null)
			{
				JobFailReason.Is(WorkGiver_FillFermentingBarrel.NoWortTrans, null);
				return false;
			}
			return !t.IsBurning();
		}

		// Token: 0x060083C5 RID: 33733 RVA: 0x0027866C File Offset: 0x0027686C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_FermentingBarrel barrel = (Building_FermentingBarrel)t;
			Thing t2 = this.FindWort(pawn, barrel);
			return JobMaker.MakeJob(JobDefOf.FillFermentingBarrel, t, t2);
		}

		// Token: 0x060083C6 RID: 33734 RVA: 0x002786A0 File Offset: 0x002768A0
		private Thing FindWort(Pawn pawn, Building_FermentingBarrel barrel)
		{
			Predicate<Thing> validator = (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, false);
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.Wort), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false, false);
		}

		// Token: 0x040056F3 RID: 22259
		private static string TemperatureTrans;

		// Token: 0x040056F4 RID: 22260
		private static string NoWortTrans;
	}
}
