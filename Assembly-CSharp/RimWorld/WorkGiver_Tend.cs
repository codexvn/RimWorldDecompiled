using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200161C RID: 5660
	public class WorkGiver_Tend : WorkGiver_Scanner
	{
		// Token: 0x1700147E RID: 5246
		// (get) Token: 0x06008584 RID: 34180 RVA: 0x000E9647 File Offset: 0x000E7847
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x06008585 RID: 34181 RVA: 0x00075162 File Offset: 0x00073362
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x1700147F RID: 5247
		// (get) Token: 0x06008586 RID: 34182 RVA: 0x002735F9 File Offset: 0x002717F9
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x06008587 RID: 34183 RVA: 0x0027E6FB File Offset: 0x0027C8FB
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsWithAnyHediff;
		}

		// Token: 0x06008588 RID: 34184 RVA: 0x0027E710 File Offset: 0x0027C910
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			return pawn2 != null && WorkGiver_Tend.GoodLayingStatusForTend(pawn2, pawn) && HealthAIUtility.ShouldBeTendedNowByPlayer(pawn2) && pawn.CanReserve(pawn2, 1, -1, null, forced) && (!pawn2.IsMutant || pawn2.mutant.Def.entitledToMedicalCare) && (!pawn2.InAggroMentalState || pawn2.health.hediffSet.HasHediff(HediffDefOf.Scaria, false));
		}

		// Token: 0x06008589 RID: 34185 RVA: 0x0027E787 File Offset: 0x0027C987
		public static bool GoodLayingStatusForTend(Pawn patient, Pawn doctor)
		{
			if (patient == doctor)
			{
				return true;
			}
			if (patient.RaceProps.Humanlike)
			{
				return patient.InBed();
			}
			return patient.GetPosture() > PawnPosture.Standing;
		}

		// Token: 0x0600858A RID: 34186 RVA: 0x0027E7AC File Offset: 0x0027C9AC
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			Thing thing = HealthAIUtility.FindBestMedicine(pawn, pawn2, false);
			if (thing != null && thing.SpawnedParentOrMe != thing)
			{
				return JobMaker.MakeJob(JobDefOf.TendPatient, pawn2, thing, thing.SpawnedParentOrMe);
			}
			if (thing != null)
			{
				return JobMaker.MakeJob(JobDefOf.TendPatient, pawn2, thing);
			}
			return JobMaker.MakeJob(JobDefOf.TendPatient, pawn2);
		}
	}
}
